using Dr.Peril.Script.Model;

namespace Dr.Peril.VerbProcessors;

public class LookVerbProcessor : VerbProcessorBase
{
    public override Message Process(Game game, string verb, string @object, string adjunct)
    {
        var result = string.IsNullOrEmpty(game.CurrentRoom.Detailed)
            ? game.CurrentRoom.Description
            : game.CurrentRoom.Detailed;

        if (game.CurrentRoom.Objects.Count == 1)
            result += $"\n\nThere is a {game.CurrentRoom.Objects.First().Value.Name} here.";

        if (game.CurrentRoom.Objects.Count > 0)
        {
            result += "\n\nThere are some objects here:";
            result += string.Join(
                string.Empty,
                game.CurrentRoom.Objects.Select(o => $"\n - {o.Value.Name}")); ;
        }

        return new Message(result, IsError: false);
    }
}
