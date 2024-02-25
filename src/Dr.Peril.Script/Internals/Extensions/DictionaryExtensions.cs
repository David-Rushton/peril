using Dr.Peril.Script.Internals.Abstractions;
using Dr.Peril.Script.Internals.DTOs;

namespace Dr.Peril.Script.Internals.Extensions;

internal static class DictionaryExtensions
{
    internal static void AddOrThrow<T>(
        this Dictionary<string, T> dictionary, T value) where T : StateBase
    {
        if (dictionary.ContainsKey(value.Id))
            throw InterpreterException.FromSource(
                value.Source, $"Duplicate {value.TypeName} name: {value.Id}.");

        dictionary.Add(value.Id, value);
    }
}
