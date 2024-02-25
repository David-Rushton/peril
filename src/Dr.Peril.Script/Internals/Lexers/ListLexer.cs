using Dr.Peril.Script.Internals.Abstractions;
using System.Diagnostics;

namespace Dr.Peril.Script.Internals.Lexers;

internal class ListLexer : ILexer
{
    private readonly List<Token> _tokens = new();
    private bool _isTerminal = false;
    private int _takenCount = 0;

    internal override bool IsTerminal => _isTerminal;

    internal override bool CanAccept(Token token) =>
        token.Value == LanguageConstants.ListAssignmentOperator.ToString();

    internal override void Take(Token current, Token next)
    {
        // expected sequence:
        // operator, newline, indent
        // followed by 1 or more repetitions of:
        // Literal, newline
        TokenType[] startSequence =
        [
            TokenType.Operator,
            TokenType.NewLine,
            TokenType.Indent
        ];

        if (_takenCount < startSequence.Length)
        {
            if (current.Type != startSequence[_takenCount])
                throw InterpreterException.FromSource(current.Source, $"Expected {startSequence[_takenCount]}.");

            _takenCount++;
            return;
        }

        if (current.Type is TokenType.Literal)
            _tokens.Add(current);

        if (current.Type is not (TokenType.Literal or TokenType.NewLine))
            throw InterpreterException.FromSource(current.Source, $"Expected literal value or new line.  Not {current.Type}.");

        _isTerminal = next.Type is TokenType.Outdent or TokenType.EndOfFile;
    }

    internal override IEnumerable<Lexeme> ToLexemes()
    {
        Debug.Assert(_isTerminal);
        Debug.Assert(_tokens.Count > 0);

        foreach (var token in _tokens)
            yield return new LexemeListItem(token.Source, token.Value);

        yield return new LexemeEndOfSection(_tokens.First().Source);

        _tokens.Clear();
        _takenCount = 0;
    }
}
