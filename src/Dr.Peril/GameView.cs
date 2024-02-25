using Dr.Peril.Interpreter;
using Dr.Peril.Script.Model;
using Dr.Peril.VerbProcessors;
using Spectre.Console;

namespace Dr.Peril.Script;

public class GameView(
    Game game,
    ChatUI chatUI,
    IDictionary<string, VerbProcessorBase> verbProcessors,
    bool isVerboseMode)
{
    private const string TitleColour = "#F18F01";
    private const string ContentColour = "#EBEBEB";
    private const string DebugColour = "#ED33B9";  // 2B9720
    private readonly TextPrompt<string> _prompt = new($"[{TitleColour}]❯[/]");
    private Room? _lastRoom = null;

    public void Play()
    {
        ShowIntroduction();

        while (game.GameStage == GameStage.Playing)
        {
            if (_lastRoom != game.CurrentRoom)
            {
                ShowRoom();
                _lastRoom = game.CurrentRoom;
            }

            var command = PromptUserForCommand();
            if (verbProcessors.TryGetValue(command.Verb, out var processor))
            {
                if (isVerboseMode)
                    AnsiConsole.MarkupLine($"[{DebugColour} italic]Verb: {command.Verb}, Object: {command.Object}, Adjunct: {command.Adjunct}[/]\n");

                var message = processor.Process(game, command.Verb, command.Object, command.Adjunct);
                if (!string.IsNullOrEmpty(message.Value))
                    AnsiConsole.MarkupLine($"[{ContentColour}]{message.Value}[/]\n");
            }
            else
            {
                AnsiConsole.MarkupLine($"[{ContentColour}]I don't understand that command.[/]\n");
            }
        }

        var outcome = game.GameStage switch
        {
            GameStage.Won => $"[{TitleColour}]You won![/]",
            GameStage.Lost => $"[{TitleColour}]You lost![/]",
            GameStage.Quit => $"[{TitleColour}]You quit![/]",
            _ => throw new InvalidOperationException("Invalid game stage")
        };
        AnsiConsole.MarkupLine(outcome);
    }

    private void ShowIntroduction()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine($"[{TitleColour}]{game.Name}[/]\n");
        AnsiConsole.MarkupLine($"[{ContentColour}]{game.Introduction}[/]\n");
    }

    private void ShowRoom()
    {
        AnsiConsole.MarkupLine($"[{TitleColour}]{game.CurrentRoom.Name}[/]\n");
        AnsiConsole.MarkupLine($"[{ContentColour}]{game.CurrentRoom.Description}[/]\n");
    }

    public Command PromptUserForCommand()
    {
        var verbHints = new[] { "move" };
        var objectHints = game
            .CurrentRoom.Connections.Select(kvp => kvp.Key)
            .Union(game.CurrentRoom.Objects.Select(kvp => kvp.Value.Name))
            .Union(game.Inventory.Items.Select(kvp => kvp.Value.Name));
        var adjunctHints = Array.Empty<string>();

        if (game.CurrentRoom.Objects.Any())
            objectHints = objectHints.Append("all");

        var input = AnsiConsole.Prompt(_prompt);
        AnsiConsole.WriteLine();

        return chatUI.Parse(input, verbHints, objectHints, adjunctHints);
    }
}
