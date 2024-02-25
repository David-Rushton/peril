using Dr.Peril.Script.Model;
using System.Text;

namespace Dr.Peril.VerbProcessors;

public class TakeVerbProcessor : VerbProcessorBase
{
    public override Message Process(Game game, string verb, string @object, string adjunct)
    {
        if (string.IsNullOrEmpty(@object.ToLower()))
            return new Message("What do you want to take?", IsError: true);

        if (@object == "all")
        {
            if (!game.CurrentRoom.Objects.Any())
                return new Message("There is nothing to take.", IsError: true);

            var builder = new StringBuilder();
            builder.AppendLine("You took:\n");

            foreach (var (id, item) in game.CurrentRoom.Objects)
            {
                if (game.TryTakeObject(id))
                    builder.AppendLine($" - {item.Name}");
            }

            return new Message(builder.ToString(), true);
        }

        if (game.TryTakeObject(@object))
            return new Message($"You took the {@object}.", IsError: false);
        else
            return new Message($"I don't see a {@object} here.", IsError: true);
    }
}
