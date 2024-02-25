using Dr.Peril.Script.Internals.Abstractions;
using System.Text;

namespace Dr.Peril.Script.Internals;

internal class Tokeniser(Preprocessor preprocessor)
{
    private const string EscapeScriptExtension = "*.peril";

    internal IEnumerable<Token> ToTokens(string sourceRoot)
    {
        if (!Directory.Exists(sourceRoot))
            throw new ArgumentException("Source root does not exist", nameof(sourceRoot));

        var buffer = new StringBuilder();

        var directories = Directory.GetFiles(sourceRoot, EscapeScriptExtension, SearchOption.AllDirectories);
        foreach (var file in directories)
        {
            var lineNumber = 0;
            var indent = 0;
            var lastIndent = 0;
            var source = preprocessor.Process(file);

            foreach (var line in source.Split('\n'))
            {
                lineNumber++;

                // we shouldn't yield outdents for empty lines
                // they are not semantically meaningful
                // they exist to make the source files easier to read
                var readingIndent = line.Trim().Length > 0;

                // ensuring every line ends with a newline simplifies later stages
                foreach (var character in $"{line}\n")
                {
                    if (readingIndent)
                    {
                        if (character is not LanguageConstants.Space)
                        {
                            if (indent > lastIndent)
                                yield return Token.New(file, lineNumber, TokenType.Indent, indent);
                            if (indent < lastIndent)
                                yield return Token.New(file, lineNumber, TokenType.Outdent, indent);

                            lastIndent = indent;
                            indent = 0;
                            readingIndent = false;
                        }

                        indent++;
                    }

                    // Always return delimiters in a dedicated token.  These have special meaning.
                    // Making them easier to location and parse downstream.
                    if (IsDelimiter(character))
                    {
                        var value = buffer.ToString().Trim();
                        buffer.Clear();

                        if (!string.IsNullOrWhiteSpace(value))
                            yield return Token.New(file, lineNumber, value);

                        if (character is LanguageConstants.DoubleQuote or LanguageConstants.NewLine)
                            yield return Token.New(
                                file, lineNumber, TokenType.NewLine, character.ToString());
                    }
                    else
                    {
                        // not delimiter
                        buffer.Append(character);
                    }
                }
            }

            yield return Token.New(file, lineNumber, TokenType.EndOfFile);
        }
    }

    private static bool IsDelimiter(char character) =>
        character is
            LanguageConstants.Space
            or LanguageConstants.DoubleQuote
            or LanguageConstants.NewLine;
}
