using Dr.Peril.Script.Internals.Abstractions;
using Dr.Peril.Script.Internals.Lexers;

namespace Dr.Peril.Script.Internals;

internal class Lexer(Tokeniser tokeniser, IEnumerable<ILexer> lexers)
{
    internal IEnumerable<Lexeme> ToLexemes(string sourceRoot)
    {
        ILexer? lexer = null;

        var tokens = new Queue<Token>(tokeniser.ToTokens(sourceRoot));
        while (tokens.TryDequeue(out var current))
        {
            if (!tokens.TryPeek(out var next))
                next = NewEndOfFileToken(current);

            if (lexer is null)
                lexer = GetLexerOrThrow(current);

            lexer.Take(current, next);

            if (lexer.IsTerminal)
            {
                foreach (var lexeme in lexer.ToLexemes())
                    yield return lexeme;

                lexer = null;
            }
        }
    }

    private Token NewEndOfFileToken(Token token) =>
        Token.New(token.Source, TokenType.EndOfFile);

    private ILexer GetLexerOrThrow(Token token)
    {
        foreach (var lexer in lexers)
            if (lexer.CanAccept(token))
                return lexer;

        throw InterpreterException.FromSource(token.Source, $"Unable to process token: {{ value = {token.Value}, type = {token.Type} }}.");
    }
}
