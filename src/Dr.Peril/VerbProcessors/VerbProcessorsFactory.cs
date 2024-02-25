namespace Dr.Peril.VerbProcessors;

public class VerbProcessorsFactory
{
    public static Dictionary<string, VerbProcessorBase> Build() =>
        new()
        {
            { "move", new MoveVerbProcessor() },
            { "look", new LookVerbProcessor() },
            { "quit", new QuitVerbProcessor() },
            { "take", new TakeVerbProcessor() },
            { "list", new ListVerbProcessor() }
        };
}
