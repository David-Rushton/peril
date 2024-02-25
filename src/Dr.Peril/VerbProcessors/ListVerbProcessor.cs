using Dr.Peril.Script.Model;
using System.Text;

namespace Dr.Peril.VerbProcessors;

public class ListVerbProcessor : VerbProcessorBase
{
    public override Message Process(Game game, string verb, string @object, string adjunct)
    {
        if (game.Inventory.Items.Any())
        {
            var result = new StringBuilder("You are carrying:\n\n");
            foreach (var item in game.Inventory.Items)
            {
                result.AppendLine($" - {item.Value.Name}");
            }

            return new(result.ToString(), IsError: false);
        }

        return new("You are not carrying anything.", IsError: false);
    }
}
