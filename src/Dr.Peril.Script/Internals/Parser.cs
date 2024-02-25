namespace Dr.Peril.Script.Internals;

using Dr.Peril.Script.Internals.Abstractions;
using Dr.Peril.Script.Internals.DTOs;
using Dr.Peril.Script.Internals.Extensions;
using Dr.Peril.Script.Internals.Parsers;
using Dr.Peril.Script.Model;
using ValidatedStates = (
    DTOs.GameState game,
    Dictionary<string, DTOs.RoomState> rooms,
    Dictionary<string, DTOs.ObjectState> objects);

internal class Parser(Lexer lexer, ParserFactory parserFactory)
{
    internal Game Parse(string sourceRoot) =>
        BuildGame(ValidateOrThrow(ParseInternal(sourceRoot)));

    private IEnumerable<StateBase> ParseInternal(string sourceRoot)
    {
        var lexemes = new Queue<Lexeme>(lexer.ToLexemes(sourceRoot));

        while (lexemes.TryPeek(out var next))
        {
            var parser = parserFactory.Build(next);

            if (parser.CanParse(next))
            {
                yield return parser.Parse(lexemes);
                continue;
            }

            throw InterpreterException.FromLexeme(next, $"Cannot parse.  Unknown object type\n\n {next}.");
        }
    }

    private static Game BuildGame(ValidatedStates states)
    {
        var player = Player.Build();
        var inventory = Inventory.Build();
        var objects = states.objects
            .Select(o => GameObject.Build(o.Value));
        var rooms = states.rooms
            .Select(r => Room.Build(r.Value, objects.Where(o => r.Value.Objects.Contains(o.Id))))
            .ToDictionary(r => r.Id);
        var game = Game.Build(states.game, player, objects, rooms, inventory);

        return game;
    }

    private static ValidatedStates ValidateOrThrow(IEnumerable<StateBase> gamesObjects)
    {
        var gameStates = new Dictionary<string, GameState>();;
        var roomStates = new Dictionary<string, RoomState>();
        var objectStates = new Dictionary<string, ObjectState>();

        foreach (var gameObject in gamesObjects)
            switch (gameObject)
            {
                case GameState games:
                    gameStates.AddOrThrow(games);
                    break;
                case ObjectState objects:
                    objectStates.AddOrThrow(objects);
                    break;
                case RoomState rooms:
                    roomStates.AddOrThrow(rooms);
                    break;
                default:
                    throw new InterpreterException($"Unsupported game object type: {gameObject.GetType().Name}.");
            }

        // there must be only 1
        if (gameStates.Count == 0)
            throw new InterpreterException("Every project must contain a Game object.");
        if (gameStates.Count > 1)
            throw InterpreterException.FromSource(
                gamesObjects.Last().Source,
                "Only one Game object can be defined in a project.");

        // every connection between rooms must be valid
        foreach (var (id, room) in roomStates)
            foreach (var (_, roomId) in room.Connections)
                if (!roomStates.ContainsKey(roomId))
                    throw InterpreterException.FromSource(
                        room.Source,
                        $"Room {roomId} connected to room {id} is not defined.");

        // the first room must exist
        var game = gameStates.Values.Single();
        if (!roomStates.ContainsKey(game.StartRoomId))
            throw InterpreterException.FromSource(
                game.Source,
                $"Start room {game.StartRoomId} is not defined.");

        // every object must be defined
        foreach (var (_, room) in roomStates)
            foreach (var @object in room.Objects)
                if (!objectStates.ContainsKey(@object))
                    throw InterpreterException.FromSource(
                        room.Source,
                        $"Object {@object} in room {room.Id} is not defined.");


        return (game, roomStates, objectStates);
    }
}
