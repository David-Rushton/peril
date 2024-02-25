using Dr.Peril.Script.Internals.Abstractions;
using Dr.Peril.Script.Internals.DTOs;

namespace Dr.Peril.Script.Internals.Parsers;

internal class ObjectParser : ParserBase
{
    protected override StateBase BuildGameBase(Source source) =>
        new ObjectState
        {
            Id = ReadIdOrThrow(source),
            Source = source,
            Name = ReadPropertyOrThrow("name", source),
            Description = ReadPropertyOrThrow("description", source)
        };

    internal override bool CanParse(Lexeme lexeme) =>
        lexeme is LexemeKeyword keywordLexeme
        && keywordLexeme.Value == "object";
}
