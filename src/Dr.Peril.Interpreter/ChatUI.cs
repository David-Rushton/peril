using Dr.Peril.Interpreter.Extensions;
using Dr.Peril.Interpreter.Model;
using System.Diagnostics.CodeAnalysis;

namespace Dr.Peril.Interpreter;

public class ChatUI
{
    private readonly Vocabulary _vocabulary;

    private ChatUI(Vocabulary vocabulary) =>
        _vocabulary = vocabulary;

    /// <summary>
    ///   <para>
    ///     Converts user natural language into a <see cref="Command"/>.
    ///   </para>
    ///   <para>
    ///     Parsing is best effort.  We expect the user to issue short command sentences like
    ///     "open the door", "open door with key", "walk north", "head north", etc.
    ///   </para>
    ///   <para>
    ///     Use hints to provide context, and extend the built-in vocabulary.  By hinting at
    ///     available objects and actions the parser can make better decisions.
    ///   </para>
    ///   <para>
    ///     Hinted verbs, objects and adjuncts are prioritised.  If found they are much more likely
    ///     to be returned.
    ///   </para>
    /// </summary>
    public Command Parse(
        string command,
        IEnumerable<string> verbHints,
        IEnumerable<string> objectHints,
        IEnumerable<string> adjunctHints)
    {
        // TODO: should we perform basic spell correction?

        if (IsSpecialCase(command, out var response))
            return (Command)response;

        var words = GetWords();

        var verbs = new List<Word>();
        var objects = new List<Word>();
        var adjuncts = new List<Word>();
        var unknowns = new List<Word>();
        foreach (var word in words)
            switch (word.Type)
            {
                case WordType.Verb:
                    verbs.Add(word);
                    break;
                case WordType.Object:
                    objects.Add(word);
                    break;
                case WordType.Adjunct:
                    adjuncts.Add(word);
                    break;
                case WordType.Unknown:
                    unknowns.Add(word);
                    break;
            }

        var verb = verbs
            .OrderBy(v => verbHints.Contains(v.Value))
            .ThenBy(v => v.Value)
            .FirstOrDefault(Word.Empty);
        var @object = objects
            .OrderBy(o => objectHints.Contains(o.Value))
            .ThenBy(o => o.Value)
            .FirstOrDefault(Word.Empty);
        var adjunct = adjuncts
            .OrderBy(a => adjunctHints.Contains(a.Value))
            .ThenBy(a => a.Value)
            .FirstOrDefault(Word.Empty);

        if (!Word.IsEmpty(verb))
        {
            // verb should come before the object
            if (Word.IsEmpty(@object) && unknowns.Any(u => u.Sequence > verb.Sequence))
                @object = unknowns.First(f => f.Sequence > verb.Sequence);

            // TODO: the adjunct could come the object
            if (!Word.IsEmpty(@object))
                if (Word.IsEmpty(adjunct) && unknowns.Any(u => u.Sequence > @object.Sequence))
                    adjunct = unknowns.Skip(1).First(f => f.Sequence > @object.Sequence);
        }

        return new Command(
            !Word.IsEmpty(verb) && !Word.IsEmpty(@object),
            verb.Value,
            @object.Value,
            adjunct.Value
        );

        IEnumerable<Word> GetWords()
        {
            var sequence = 0;
            var words = command
                .Split(' ')
                .Except(_vocabulary.Functions)
                .Intersect(_vocabulary.Words)
                .Replace(_vocabulary.Synonyms);

            foreach (var word in words)
            {
                if (verbHints.Union(_vocabulary.Verbs).Contains(word))
                {
                    yield return _vocabulary.Synonyms.ContainsKey(word)
                        ? new(sequence++, _vocabulary.Synonyms[word], WordType.Verb)
                        : new(sequence++, word, WordType.Verb);
                    continue;
                }

                if (objectHints.Contains(word))
                {
                    yield return new(sequence++, word, WordType.Object);
                    continue;
                }

                if (adjunctHints.Contains(word))
                {
                    yield return new(sequence++, word, WordType.Adjunct);
                    continue;
                }

                yield return new(sequence++, word, WordType.Unknown);
            }
        }
    }


    // TODO: Why is this a method, when others are a local function?
    private static bool IsSpecialCase(
        string command, [NotNullWhen(returnValue: true)] out Command? response)
    {
        response = command switch
        {
            "q"     => new Command(true, "quit", string.Empty, string.Empty),
            "quit"  => new Command(true, "quit", string.Empty, string.Empty),
            "n"     => new Command(true, "move", "north", string.Empty),
            "north" => new Command(true, "move", "north", string.Empty),
            "e"     => new Command(true, "move", "east", string.Empty),
            "east"  => new Command(true, "move", "east", string.Empty),
            "s"     => new Command(true, "move", "south", string.Empty),
            "south" => new Command(true, "move", "south", string.Empty),
            "w"     => new Command(true, "move", "west", string.Empty),
            "west"  => new Command(true, "move", "west", string.Empty),
            _       => null
        };

        return response is not null;
    }

    public static ChatUI Build() =>
        new ChatUI(Vocabulary.Build());
}
