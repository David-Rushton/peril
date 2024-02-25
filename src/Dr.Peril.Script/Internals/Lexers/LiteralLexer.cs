using System.Diagnostics;
using System.Text;

using Dr.Peril.Script.Internals.Abstractions;

namespace Dr.Peril.Script.Internals.Lexers;

internal class StringLexer : ILexer
{
    private static readonly string DoubleQuote = LanguageConstants.DoubleQuote.ToString();
    private readonly StringBuilder _stringBuilder = new();
    private Token? _firstToken = null;
    private int _doubleQuotesConsumed = 0;

    internal override bool IsTerminal => throw new NotImplementedException();

    internal override bool CanAccept(Token token) =>
        token.Value == DoubleQuote;

    internal override void Take(Token current, Token next)
    {
        if (current.Value == DoubleQuote)
            _doubleQuotesConsumed++;

        // TODO: Document this 👇.
        // We support multiline strings.  Which follow these rules:
        //
        //  - Whitespace at the start and end of the string is removed.
        //  - Whitespace at the start and end of each line is trimmed.
        //  - "Words" are always seperated by a single space.
        //  - Whitespace is swallowed, except consecutive new lines.
        //
        // These rules allow the programmer to decouple source code layout, from presetnation
        // layout.  While still providing a way to separate sections into paragraphs.

        if (current.Type is TokenType.NewLine && next.Type is TokenType.NewLine)
            _stringBuilder.AppendLine();

        if (current.Type is TokenType.Indent or TokenType.Outdent or TokenType.NewLine)
            return;

        _stringBuilder.Append(current.Value);
    }

    internal override IEnumerable<Lexeme> ToLexemes()
    {
        Debug.Assert(_firstToken != null);
        Debug.Assert(_doubleQuotesConsumed == 2);

        yield return new LexemeLiteral(_firstToken.Value.Source, _stringBuilder.ToString().Trim());

        _firstToken = null;
        _doubleQuotesConsumed = 0;
        _stringBuilder.Clear();
    }
}
