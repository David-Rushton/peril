namespace Dr.Peril.Interpreter;

internal class Vocabulary
{
    internal required HashSet<string> Verbs { get; init; }
    internal required Dictionary<string, string> Synonyms { get; init; }
    internal required HashSet<string> Functions { get; init; }
    internal required HashSet<string> Words { get; init; }

    internal static Vocabulary Build()
    {
        // TODO: Magic.
        const string root = @"C:\Users\David\Source\games\escape-from-doom-island\src\EscapeChatUI\VocabularyDb";
        var verbsPath = Path.Combine(root, "Verbs.txt");
        var synonymsPath = Path.Combine(root, "Synonyms.txt");
        var functionsPath = Path.Combine(root, "Functions.txt");
        var wordsPath = Path.Combine(root, "Words.txt");

        return new Vocabulary
        {
            Verbs = [.. File.ReadAllLines(verbsPath)],
            Synonyms = GetSynonyms(),
            Functions = [.. File.ReadAllLines(functionsPath)],
            Words = [.. File.ReadAllLines(wordsPath)]
        };

        Dictionary<string, string> GetSynonyms()
        {
            var lines = File.ReadAllLines(synonymsPath);
            var result = new Dictionary<string, string>();

            var word = string.Empty;
            var synonym = string.Empty;
            foreach (var line in lines)
            {
                if (line.StartsWith(" "))
                {
                    if (string.IsNullOrEmpty(word))
                        throw new InvalidOperationException(
                            "Synonym file is invalid.  It must always start with the root word.  Followed by a series of synonyms, each indented by one or more spaces.");
                    synonym = line.Trim();
                }
                else
                {
                    word = line.Trim();
                    synonym = string.Empty;
                }

                if (!string.IsNullOrEmpty(word) && !string.IsNullOrEmpty(synonym))
                    result.Add(synonym, word);
            }

            return result;
        }
    }
}
