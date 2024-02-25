using Dr.Peril.Script.Model;

namespace Dr.Peril.VerbProcessors;

public class QuitVerbProcessor : VerbProcessorBase
{
    public override Message Process(Game game, string verb, string @object, string adjunct)
    {
        game.GameStage = GameStage.Quit;
        return new Message(string.Empty, IsError: false);
    }
}
