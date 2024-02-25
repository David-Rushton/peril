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

        return source;
    }
}
