using Dr.Peril.Script.Internals;
using Dr.Peril.Script.Internals.Lexers;
using Dr.Peril.Script.Internals.Parsers;
using Dr.Peril.Script.Model;

namespace Dr.Peril.Script;

public class Interpreter
{
    public Game Execute(string path)
    {
        // TODO: Implement try/catch with pretty print exceptions & stack traces.

        var preprocessor = new Preprocessor();
        var tokeniser = new Tokeniser(preprocessor);
        var subLexers = new ILexer[]
        {
            new KeywordLexer(),
            new InlineLiteralLexer(),
            new MultilineLiteralLexer(),
            new KeyValuesLexer(),
            new ListLexer(),
            new WhitespaceLexer()
        };
        var lexer = new Lexer(tokeniser, subLexers);
        var parserFactory = new ParserFactory();
        var parser = new Parser(lexer, parserFactory);

        return parser.Parse(path);
    }
}
