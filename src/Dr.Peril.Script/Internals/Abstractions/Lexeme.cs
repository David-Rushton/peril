namespace Dr.Peril.Script.Internals.Abstractions;

internal enum LexemeType
{
    Keyword,
    Identifier,
    Literal,
    EndOfSection
}

internal abstract record Lexeme(
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

internal record LexemeEndOfSection(
    Source Source
    ) : Lexeme(Source, LexemeType.EndOfSection);
