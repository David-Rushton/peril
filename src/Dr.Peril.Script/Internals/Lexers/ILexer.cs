using Dr.Peril.Script.Internals.Abstractions;

namespace Dr.Peril.Script.Internals.Lexers;

internal abstract class ILexer
{
    internal abstract bool CanAccept(Token token);
    internal abstract void Take(Token current, Token next);
    internal abstract bool IsTerminal { get; }
    internal abstract IEnumerable<Lexeme> ToLexemes();
}
