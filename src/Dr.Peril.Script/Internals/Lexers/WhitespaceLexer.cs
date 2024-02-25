using Dr.Peril.Script.Internals.Abstractions;
using System.Diagnostics;

namespace Dr.Peril.Script.Internals.Lexers;

internal class WhitespaceLexer : ILexer
{
    private static readonly TokenType[] Whitespace = [
        TokenType.NewLine,
        TokenType.Indent,
        TokenType.Outdent,
        TokenType.EndOfFile];
    private Token? _token = null;

    internal override bool CanAccept(Token token) =>
        Whitespace.Contains(token.Type);

    internal override bool IsTerminal =>
        true;

    internal override void Take(Token current, Token _) =>
        _token = current;

    internal override IEnumerable<Lexeme> ToLexemes()
    {
        Debug.Assert(_token.HasValue);

        // we swallow everyting exception end of file.
        if (_token.Value.Type is TokenType.EndOfFile)
            yield return new LexemeEndOfSection(_token.Value.Source);

        _token = null;
    }
}
