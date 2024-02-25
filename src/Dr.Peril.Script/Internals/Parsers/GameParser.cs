using Dr.Peril.Script.Internals.Abstractions;
using Dr.Peril.Script.Internals.DTOs;

namespace Dr.Peril.Script.Internals.Parsers;

internal class GameParser : ParserBase
{
    internal override bool CanParse(Lexeme lexeme) =>
        lexeme is LexemeKeyword keywordLexeme
        && keywordLexeme.Value == "game";

    protected override StateBase BuildGameBase(Source source) =>
        new GameState
        {
            Id = ReadPropertyOrThrow("name", source),
            Source = source,
            Name = ReadPropertyOrThrow("name", source),
            StartRoomId = ReadPropertyOrThrow("start", source),
            Rooms = new(),
            Introduction = ReadPropertyOrThrow("introduction", source)
        };
}
