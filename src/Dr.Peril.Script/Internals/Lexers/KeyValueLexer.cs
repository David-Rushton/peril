using Dr.Peril.Script.Internals.Abstractions;
using System.Diagnostics;

namespace Dr.Peril.Script.Internals.Lexers;

internal class KeyValuesLexer : ILexer
{
    private static readonly TokenType[] _expectedStartSequence = [
        TokenType.Operator,
        TokenType.NewLine,
        TokenType.Indent
    ];
    private static readonly TokenType[] _expectedKeyValueSequence = [
        TokenType.Literal,
        TokenType.Operator,
        TokenType.Literal,
        TokenType.NewLine
    ];
    private readonly List<Token> _takenStart = new();
    private readonly List<Token> _takenKeyValues = new();
    private readonly Dictionary<Token, Token?> _items = new();
    private Token? _key;

    private bool _isTerminal = false;

    internal override bool CanAccept(Token token) =>
        token.Value == LanguageConstants.DictionaryAssignmentOperator.ToString();

    internal override bool IsTerminal =>
        _isTerminal;

    internal override void Take(Token current, Token next)
    {
        // initial sequence.
        if (_takenStart.Count < 3)
        {
            if (_expectedStartSequence[_takenStart.Count] != current.Type)
                throw InterpreterException.FromToken(current, $"Cannot build dictionary.  Expected {_expectedStartSequence[_takenStart.Count]}.");

            _takenStart.Add(current);
            return;
        }

        var index = _takenKeyValues.Count % 4;
        if (_expectedKeyValueSequence[index] != current.Type)
            throw InterpreterException.FromToken(current, $"Cannot build dictionary.  Expected {_expectedKeyValueSequence[index]}.");
        _takenKeyValues.Add(current);

        switch (index)
        {
            // literal
            case 0:
                if (_items.ContainsKey(current))
                    throw InterpreterException.FromToken(current, $"Cannot build dictionary.  Duplicate key found: {current.Value}.");

                _key = current;
                _items.Add(current, null);
                break;
            // inline operator
            case 1:
                if (current.Value != LanguageConstants.InlineAssignmentOperator.ToString())
                    throw InterpreterException.FromToken(current, $"Cannot build dictionary.  Operator not supported: {current.Value}.");
                break;
            // literal
            case 2:
                Debug.Assert(_key.HasValue);
                _items[(Token)_key] = current;
                break;
            default:
                // no-op.
                // we don't need to do anything with the newline (case 3).
                break;
        }

        _isTerminal = next.Type is TokenType.Outdent or TokenType.EndOfFile;
    }

    internal override IEnumerable<Lexeme> ToLexemes()
    {
        foreach (var (key, value) in _items)
        {
            if (value is null)
                throw InterpreterException.FromSource(key.Source, "Cannot build dictionary.  Missing value.");

            var item = value.Value;
            yield return new LexemeKey(key.Source, key.Value);
            yield return new LexemeValue(item.Source, item.Value);
        }

        yield return new LexemeEndOfSection(_items.First().Key.Source);

        _takenStart.Clear();
        _takenKeyValues.Clear();
        _items.Clear();
        _key = null;
        _isTerminal = false;
    }
}
