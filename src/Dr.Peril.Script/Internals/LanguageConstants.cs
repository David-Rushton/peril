namespace Dr.Peril.Script.Internals;

internal static class LanguageConstants
{
    internal const char Space = ' ';
    internal const char DoubleQuote = '"';
    internal const char NewLine = '\n';

    internal const string LocationKeyword = "location";
    internal const string NameKeyword = "name";
    internal const string DescriptionKeyword = "description";
    internal const string DetailedKeyword = "detailed";
    internal const string ExitKeyword = "exit";
    internal const string ToKeyword = "to";
    internal const string GameKeyword = "game";
    internal const string ObjectKeyword = "object";
    internal const string StartKeyword = "start";
    internal const string IntroductionKeyword = "introduction";
    internal static readonly string[] Keywords = [
        LocationKeyword,
        NameKeyword,
        DescriptionKeyword,
        DetailedKeyword,
        ExitKeyword,
        ToKeyword,
        GameKeyword,
        ObjectKeyword,
        StartKeyword,
        IntroductionKeyword
    ];
}
