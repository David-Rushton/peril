namespace Dr.Peril.Script.Internals;

internal class Preprocessor
{
    // TODO: we should try to detect this from the source file
    private const int TabWidth = 4;

    internal string Process(string path)
    {
        if (!File.Exists(path))
            throw new ArgumentException("Source file does not exist", nameof(path));

        var source = File.ReadAllText(path)
            .Replace("\r", string.Empty)
            .Replace("\t", new string(' ', TabWidth))
            .Trim();

        // appending two final new lines ensures every file ends with a blank line.
        // this resets the indent level, and ensures a final token is flushed through the system.
        // that token simplifies identifying the end of various sequences within the lexer.
        return source;
    }
}
