namespace Dr.Peril.Script.Internals.Abstractions;

internal class InterpreterException(string message) : Exception(message)
{
    internal static InterpreterException FromSource(Source source, string message) =>
        new InterpreterException($"Build failed\n | {message}\n | {source.Path}:line {source.LineNumber}");

    internal static InterpreterException FromToken(Token token, string message) =>
        new InterpreterException($"Build failed\n | {message}\n | {token.Source.Path}:line {token.Source.LineNumber}");

    internal static InterpreterException FromLexeme(Lexeme lexeme, string message) =>
        new InterpreterException($"Build failed\n | {message}\n | {lexeme.Source.Path}:line {lexeme.Source.LineNumber}");
}
