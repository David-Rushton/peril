using Dr.Peril.Script.Internals.DTOs;
using System.Diagnostics;

namespace Dr.Peril.Script.Model;

public enum GameStage
{
    Playing,
    Quit,
    Won,
    Lost
}

public class Game
{
    private readonly GameState _state;
    private readonly IEnumerable<GameObject> _objects;
    private readonly Dictionary<string, Room> _rooms;

    private Game(
        GameState state,
        Player player,
        IEnumerable<GameObject> objects,
        Dictionary<string, Room> rooms,
        Inventory inventory)
    {
        _state = state;
        _rooms = rooms;
        _objects = objects;

        Player = player;
        Inventory = inventory;
        CurrentRoom = _rooms[state.StartRoomId];
    }

    public string Name => _state.Name;
    public Player Player { get; init;}
    public GameStage GameStage { get; set; } = GameStage.Playing;
    public Room CurrentRoom { get; private set; }
    public Inventory Inventory { get; init; }
    public string Introduction => _state.Introduction;

    public bool TryMove(string direction)
    {
        if (!CurrentRoom.Connections.TryGetValue(direction, out var roomId))
            return false;

        Debug.Assert(_rooms.TryGetValue(roomId, out var room));

        CurrentRoom = room;
        return true;
    }

    public bool TryTakeObject(string id)
    {
        if (CurrentRoom.Objects.TryGetValue(id, out var @object))
        {
            Inventory.Items.Add(id, @object);
            CurrentRoom.Objects.Remove(id);
            return true;
        }

        return false;
    }

    internal static Game Build(
        GameState state,
        Player player,
        IEnumerable<GameObject> objects,
        Dictionary<string, Room> rooms,
        Inventory inventory) =>
        new(state, player, objects, rooms, inventory);
}
