using Dr.Peril.Script.Internals.Abstractions;
using System.Diagnostics;

namespace Dr.Peril.Script.Internals.Lexers;

internal class KeywordLexer : ILexer
{
    private Token? _keyword = null;
    private Token? _identifier = null;
    private bool _isTerminal = false;

    internal override bool CanAccept(Token token) =>
        LanguageConstants.Keywords.Contains(token.Value.ToLower());

    internal override bool IsTerminal =>
        _isTerminal;

    internal override void Take(Token current, Token _)
    {
        // room & object are a special cases
        // they are keywords immediately followed by identifier, without an assignment operator
        var takeIdentifier = new[] { "room", "object" };
        _isTerminal = !takeIdentifier.Contains(current.Value.ToLower());

        if (LanguageConstants.Keywords.Contains(current.Value.ToLower()))
            _keyword = current;
        else
            _identifier = current;
    }

    internal override IEnumerable<Lexeme> ToLexemes()
    {
        Debug.Assert(_keyword.HasValue);

        yield return new LexemeKeyword(
            _keyword.Value.Source, _keyword.Value.Value.ToLower());
        _keyword = null;
        _isTerminal = false;

        if (_identifier.HasValue)
        {
            yield return new LexemeIdentifier(
                _identifier.Value.Source, _identifier.Value.Value);
            _identifier = null;
        }
    }
}
