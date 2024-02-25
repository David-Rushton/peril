using Dr.Peril.Script.Internals.Abstractions;
using System.Diagnostics;
using System.Text;

namespace Dr.Peril.Script.Internals.Lexers;

internal class MultilineLiteralLexer : ILexer
{
    private bool _isTerminal = false;
    private Source? _source = null;
    private TokenType? _expectedNext = TokenType.Operator;
    private readonly StringBuilder _builder = new();

    internal override bool CanAccept(Token token)
    {
        if (token.Value == LanguageConstants.MultilineAssignmentOperator.ToString())
        {
            _source = token.Source;
            return true;
        }

        return false;
    }

    internal override void Take(Token current, Token next)
    {
        // this is the expected start sequence.
        // none of these tokens are required in the final output.
        if (_expectedNext.HasValue)
        {
            _expectedNext = _expectedNext.Value switch
            {
                TokenType.Operator => TokenType.NewLine,
                TokenType.NewLine => TokenType.Indent,
                TokenType.Indent => null,
                _ => throw InterpreterException.FromSource(current.Source, "Unexpected token.")
            };

            return;
        }

        if (current.Type is TokenType.Indent)
            throw InterpreterException.FromSource(current.Source, "Cannot build string.  Unexpected indent.");

        _isTerminal = next.Type is TokenType.Outdent or TokenType.EndOfFile;
        _builder.Append($"{current.Value} ");
    }

    internal override bool IsTerminal =>
        _isTerminal;

    internal override IEnumerable<Lexeme> ToLexemes()
    {
        Debug.Assert(_isTerminal);
        Debug.Assert(!_expectedNext.HasValue);
        Debug.Assert(_source.HasValue);

        var value = _builder.ToString().Trim();
        _builder.Clear();
        _isTerminal = false;
        _expectedNext = TokenType.Operator;

        yield return new LexemeLiteral(_source.Value, value);
    }
}
