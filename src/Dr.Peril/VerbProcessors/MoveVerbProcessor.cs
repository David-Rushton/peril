using Dr.Peril.Script.Model;

namespace Dr.Peril.VerbProcessors;

public class MoveVerbProcessor : VerbProcessorBase
{
    public override Message Process(Game game, string verb, string @object, string adjunct) =>
        game.TryMove(@object)
            ? new Message(string.Empty, false)
            : new Message($"You can't move {@object}.", true);
}
