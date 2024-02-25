using Dr.Peril.Script.Internals.Abstractions;
using Dr.Peril.Script.Internals.DTOs;

namespace Dr.Peril.Script.Internals.Parsers;

internal class RoomParser : ParserBase
{
    internal override bool CanParse(Lexeme lexeme) =>
        lexeme is LexemeKeyword keywordLexeme
        && keywordLexeme.Value == "room";

    protected override StateBase BuildGameBase(Source source) =>
        new RoomState
        {
            Id = ReadIdOrThrow(source),
            Source = source,
            Name = ReadPropertyOrThrow("name", source),
            Description = ReadPropertyOrThrow("description", source),
            Detailed = ReadPropertyOrDefault("detailed"),
            Objects = ReadListOrDefault("objects"),
            Connections = ReadDictionaryOrThrow("connections", source)
        };
}
