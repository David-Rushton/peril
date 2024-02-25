namespace Dr.Peril.Script.Internals.Abstractions;

internal enum LexemeType
{
    Keyword,
    Identifier,
    Literal,
    Key,
    Value,
    ListItem,
    EndOfSection
}

internal record Lexeme(
    Source Source,
    LexemeType Type);

internal record LexemeKeyword(
    Source Source,
    string Value
    ) : Lexeme(Source, LexemeType.Keyword);

internal record LexemeIdentifier(
    Source Source,
    string Value) : Lexeme(Source, LexemeType.Identifier);

internal record LexemeLiteral(
    Source Source,
    string Value) : Lexeme(Source, LexemeType.Literal);

internal record LexemeKey(
    Source Source,
    string Key) : Lexeme(Source, LexemeType.Key);

internal record LexemeValue(
    Source Source,
    string Value) : Lexeme(Source, LexemeType.Value);

internal record LexemeListItem(
    Source Source,
    string Value) : Lexeme(Source, LexemeType.ListItem);

internal record LexemeEndOfSection(
    Source Source
    ) : Lexeme(Source, LexemeType.EndOfSection);
