namespace Dr.Peril.Script.Internals;

internal static class LanguageConstants
{
    internal const char Space = ' ';

    internal const string RoomKeyword = "room";
    internal const string NameKeyword = "name";
    internal const string DescriptionKeyword = "description";
    internal const string DetailedKeyword = "detailed";
    internal const string ConnectionsKeyword = "connections";
    internal const string GameKeyword = "game";
    internal const string ObjectKeyword = "object";
    internal const string ObjectsKeyword = "objects";
    internal const string StartKeyword = "start";
    internal const string IntroductionKeyword = "introduction";
    internal static readonly string[] Keywords = [
        RoomKeyword,
        NameKeyword,
        DescriptionKeyword,
        DetailedKeyword,
        ConnectionsKeyword,
        GameKeyword,
        ObjectKeyword,
        ObjectsKeyword,
        StartKeyword,
        IntroductionKeyword
    ];

    internal const char DictionaryAssignmentOperator = '@';
    internal const char InlineAssignmentOperator = ':';
    internal const char ListAssignmentOperator = '!';
    internal const char MultilineAssignmentOperator = '>';
    internal static readonly char[] Operators = [
        DictionaryAssignmentOperator,
        InlineAssignmentOperator,
        ListAssignmentOperator,
        MultilineAssignmentOperator
    ];
}
