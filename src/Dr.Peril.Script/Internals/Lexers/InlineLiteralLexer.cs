using Dr.Peril.Script.Internals.Abstractions;
using System.Diagnostics;
using System.Text;

namespace Dr.Peril.Script.Internals.Lexers;

internal class InlineLiteralLexer : ILexer
{
    private bool _isTerminal = false;
    private Source? _literalSource = null;
    private readonly StringBuilder _builder = new();

    internal override bool CanAccept(Token token)
    {
        if (token.Value == LanguageConstants.InlineAssignmentOperator.ToString())
        {
            _literalSource = token.Source;
            return true;
        }

        return false;
    }

    internal override void Take(Token current, Token next)
    {
        if (current.Type is not (TokenType.Operator or TokenType.Literal))
            throw InterpreterException.FromSource(current.Source, $"Cannot build string.  Unexpected literal, not {current.Type}.");

        if (current.Type is TokenType.Operator && _builder.Length == 0)
            return;

        _builder.Append($"{current.Value} ");
        _isTerminal = next.Type is TokenType.NewLine;
    }

    internal override bool IsTerminal =>
        _isTerminal;

    internal override IEnumerable<Lexeme> ToLexemes()
    {
        Debug.Assert(_isTerminal);
        Debug.Assert(_literalSource.HasValue);

        yield return new LexemeLiteral(_literalSource.Value, _builder.ToString().Trim());

        _isTerminal = false;
        _literalSource = null;
        _builder.Clear();
    }
}
