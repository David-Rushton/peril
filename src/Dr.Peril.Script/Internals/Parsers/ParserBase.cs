using Dr.Peril.Script.Internals.Abstractions;
using Dr.Peril.Script.Internals.DTOs;

namespace Dr.Peril.Script.Internals.Parsers;

internal abstract class ParserBase
{
    protected string _id = string.Empty;
    protected Dictionary<string, string> _properties = new();
    protected Dictionary<string, List<string>> _lists = new();
    protected Dictionary<string, Dictionary<string, string>> _dictionaries = new();

    internal abstract bool CanParse(Lexeme lexeme);
    internal StateBase Parse(Queue<Lexeme> lexemes)
    {
        var source = lexemes.Peek().Source;

        while (lexemes.TryDequeue(out var current) && current is not LexemeEndOfSection)
        {
            if (current is not LexemeKeyword keywordLexeme)
                throw InterpreterException.FromLexeme(current, $"Expected keyword. Not {current.Type}.");

            // There is only one game per code base.
            // No further identifiers are allowed.
            if (IsGame(keywordLexeme))
                continue;

            if (IsId(lexemes))
            {
                DequeueId(lexemes);
                continue;
            }

            if (IsProperty(lexemes))
            {
                DequeueProperty(keywordLexeme, lexemes);
                continue;
            }

            if (IsDictionary(lexemes))
            {
                DequeueDictionary(keywordLexeme, lexemes);
                continue;
            }

            if (IsList(lexemes))
            {
                DequeueList(keywordLexeme, lexemes);
                continue;
            }

            throw InterpreterException.FromLexeme(current, $"Unexpected type {current.Type}.");
        }

        return BuildGameBase(source);
    }

    protected abstract StateBase BuildGameBase(Source source);

    protected string ReadIdOrThrow(Source source) =>
        string.IsNullOrWhiteSpace(_id)
            ? throw InterpreterException.FromSource(source, $"Missing required property id.")
            : _id;

    protected string ReadPropertyOrDefault(string propertyName, string? @default = null) =>
        _properties.TryGetValue(propertyName, out var value)
            ? value
            : @default ?? string.Empty;

    protected string ReadPropertyOrThrow(string propertyName, Source source) =>
        _properties.TryGetValue(propertyName, out var value)
            ? value
            : throw InterpreterException.FromSource(source, $"Missing required property {propertyName}.");

    protected List<string> ReadListOrDefault(string listName) =>
        _lists.TryGetValue(listName, out var list)
            ? list
            : new();

    protected List<string> ReadListOrThrow(string listName, Source source) =>
        _lists.TryGetValue(listName, out var list)
            ? list
            : throw InterpreterException.FromSource(source, $"Missing required list {listName}.");

    protected Dictionary<string, string> ReadDictionaryOrDefault(string dictionaryName) =>
        _dictionaries.TryGetValue(dictionaryName, out var dictionary)
            ? dictionary
            : new();

    protected Dictionary<string, string> ReadDictionaryOrThrow(string dictionaryName, Source source) =>
        _dictionaries.TryGetValue(dictionaryName, out var dictionary)
            ? dictionary
            : throw InterpreterException.FromSource(source, $"Missing required dictionary {dictionaryName}.");

    private bool IsGame(LexemeKeyword keyword) =>
        keyword.Value.ToLower() == LanguageConstants.GameKeyword;

    private bool IsId(Queue<Lexeme> lexemes) =>
        lexemes.TryPeek(out var next)
        && next is LexemeIdentifier;

    private bool IsProperty(Queue<Lexeme> lexemes) =>
        lexemes.TryPeek(out var next)
        && next is LexemeLiteral;

    private bool IsList(Queue<Lexeme> lexemes) =>
        lexemes.TryPeek(out var next)
        && next is LexemeListItem;

    private bool IsDictionary(Queue<Lexeme> lexemes) =>
        lexemes.TryPeek(out var next)
        && next is LexemeKey;

    private void DequeueId(Queue<Lexeme> lexemes)
    {
        var id = DequeueTypeOrThrow<LexemeIdentifier>(lexemes);

        _id = string.IsNullOrWhiteSpace(_id)
            ? id.Value
            : throw InterpreterException.FromLexeme(id, "Id can only be defined once.");
    }

    private void DequeueProperty(LexemeKeyword keyword, Queue<Lexeme> lexemes)
    {
        var literal = DequeueTypeOrThrow<LexemeLiteral>(lexemes);

        if (_properties.ContainsKey(keyword.Value))
            throw InterpreterException.FromLexeme(keyword, $"Duplicate property {keyword.Value}.");

        _properties.Add(keyword.Value, literal.Value);
    }

    private void DequeueList(LexemeKeyword listName, Queue<Lexeme> lexemes)
    {
        if (_lists.ContainsKey(listName.Value))
            throw InterpreterException.FromLexeme(listName, $"Duplicate list {listName.Value}.");
        _lists.Add(listName.Value, new());

        while (lexemes.TryPeek(out var next) && next is not LexemeEndOfSection)
        {
            var listItem = DequeueTypeOrThrow<LexemeListItem>(lexemes);
            _lists[listName.Value].Add(listItem.Value);
        }

        DequeueTypeOrThrow<LexemeEndOfSection>(lexemes);
    }

    private void DequeueDictionary(LexemeKeyword dictionaryName, Queue<Lexeme> lexemes)
    {
        if (_dictionaries.ContainsKey(dictionaryName.Value))
            throw InterpreterException.FromLexeme(dictionaryName, $"Duplicate dictionary {dictionaryName.Value}.");
        _dictionaries.Add(dictionaryName.Value, new());

        while (lexemes.TryPeek(out var next) && next is not LexemeEndOfSection)
        {
            var key = DequeueTypeOrThrow<LexemeKey>(lexemes);
            var value = DequeueTypeOrThrow<LexemeValue>(lexemes);

            if (_dictionaries[dictionaryName.Value].ContainsKey(key.Key))
                throw InterpreterException.FromLexeme(key, $"Duplicate key {key.Key} in dictionary {dictionaryName.Value}.");
            _dictionaries[dictionaryName.Value][key.Key] = value.Value;
        }

        DequeueTypeOrThrow<LexemeEndOfSection>(lexemes);
    }

    private T DequeueTypeOrThrow<T>(Queue<Lexeme> lexemes) where T : Lexeme
    {
        if (lexemes.TryDequeue(out var candidate))
        {
            if (candidate is not T typed)
                throw InterpreterException.FromLexeme(candidate, $"Expected {typeof(T).Name}.  Not {candidate.GetType().Name}.");

            return typed;
        }

        throw new InterpreterException("Unexpected end of file.");
    }
}
