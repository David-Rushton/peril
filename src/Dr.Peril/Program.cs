using Dr.Peril.VerbProcessors;
using Dr.Peril.Interpreter;
using Dr.Peril.Script;

Bootstrap().Play();


GameView Bootstrap()
{
    string[] verboseFlags = ["-v", "--verbose"];

    var isVerboseMode = args.Any(a => verboseFlags.Contains(a));

    var interpreter = new Interpreter();
    var path = @"C:\Users\David\Source\games\escape-from-doom-island\src\escape-from-doom-island";
    var game = interpreter.Execute(path);
    var chatUI = ChatUI.Build();
    var verbProcessors = VerbProcessorsFactory.Build();
    var gameView = new GameView(game, chatUI, verbProcessors, isVerboseMode);

    return gameView;
}
