using Dr.Peril.Script.Internals.Abstractions;
using System.Diagnostics;

namespace Dr.Peril.Script.Internals.Lexers;

internal class KeywordLexer : ILexer
{
    private readonly static string[] KeywordsThatRequireIdentifier = [
        LanguageConstants.ExitKeyword,
        LanguageConstants.LocationKeyword,
        LanguageConstants.ObjectKeyword,
        LanguageConstants.ToKeyword];

    private Token? _keyword = null;
    private Token? _identifier = null;
    private bool _isTerminal = false;

    internal override bool CanAccept(Token token) =>
        LanguageConstants.Keywords.Contains(token.Value);

    internal override bool IsTerminal =>
        _isTerminal;

    internal override void Take(Token current, Token _)
    {
        _isTerminal = !KeywordsThatRequireIdentifier.Contains(current.Value);

        if (LanguageConstants.Keywords.Contains(current.Value))
            _keyword = current;
        else
            _identifier = current;
    }

    internal override IEnumerable<Lexeme> ToLexemes()
    {
        Debug.Assert(_keyword.HasValue);

        yield return new LexemeKeyword(
            _keyword.Value.Source, _keyword.Value.Value);

        if (_identifier.HasValue)
            yield return new LexemeIdentifier(
                _identifier.Value.Source, _identifier.Value.Value);

        _keyword = null;
        _isTerminal = false;
        _identifier = null;
    }
}
