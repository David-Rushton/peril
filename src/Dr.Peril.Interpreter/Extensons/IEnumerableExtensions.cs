namespace Dr.Peril.Interpreter.Extensions;

internal static class IEnumerableExtensions
{
    public static IEnumerable<T> Replace<T>(
        this IEnumerable<T> source, Dictionary<T, T> replacements) where T : notnull
    {
        foreach (var item in source)
            if (replacements.TryGetValue(item, out var replacement))
                yield return replacement;
            else
                yield return item;
    }
}
