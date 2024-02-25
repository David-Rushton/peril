namespace Dr.Peril.Script.Internals.Abstractions;

internal enum TokenType
{
    Indent,
    Outdent,
    Literal,
    Operator,
    NewLine,
    EndOfFile
}

internal readonly record struct Token(
    Source Source,
    TokenType Type,
    string Value)
{
    internal static Token New(string path, int lineNumber, TokenType type) =>
        New(new(path, lineNumber), type);

    internal static Token New(string path, int lineNumber, TokenType type, int indent)
    {
        if (type is not (TokenType.Indent or TokenType.Outdent))
            throw new ArgumentException("Type must be Indent or Outdent", nameof(type));

        return new(new(path, lineNumber), type, indent.ToString());
    }

    internal static Token New(string path, int lineNumber, string value) =>
        New(new(path, lineNumber), value);

    internal static Token New(string path, int lineNumber, TokenType type, string value) =>
        new(new(path, lineNumber), type, value);

    internal static Token New(Source source, TokenType type)
        => new(source, type, string.Empty);

    internal static Token New(Source source, string value)
        => new(source, TokenType.Literal, value);
}
