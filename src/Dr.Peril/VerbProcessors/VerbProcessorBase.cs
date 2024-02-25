using Dr.Peril.Script.Model;

namespace Dr.Peril.VerbProcessors;

public readonly record struct Message(string Value, bool IsError);

public abstract class VerbProcessorBase
{
    public abstract Message Process(Game game, string verb, string @object, string adjunct);
}
