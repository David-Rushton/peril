using Dr.Peril.Script.Internals.Abstractions;

namespace Dr.Peril.Script.Internals.Parsers;

internal class ParserFactory
{
    internal ParserBase Build(Lexeme lexeme)
    {
        if (lexeme is not LexemeKeyword keyword)
            throw InterpreterException.FromLexeme(lexeme, $"Cannot parse type {lexeme.Type}.");

        return keyword switch
        {
            { Value: "game" } => new GameParser(),
            { Value: "object" } => new ObjectParser(),
            { Value: "room" } => new RoomParser(),
            _ => throw InterpreterException.FromLexeme(lexeme, $"Cannot parse type {lexeme.Type}.")
        };
    }
}
