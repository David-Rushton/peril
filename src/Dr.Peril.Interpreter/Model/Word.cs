namespace Dr.Peril.Interpreter.Model;

internal enum WordType
{
    Verb,
    Object,
    Adjunct,
    Unknown
}

internal readonly record struct Word(
    int Sequence,
    string Value,
    WordType Type)
{
    internal static Word Empty => new(int.MaxValue, string.Empty, WordType.Unknown);

    internal static bool IsEmpty(Word word) =>
        word == Word.Empty;
}
