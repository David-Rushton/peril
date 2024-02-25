using Dr.Peril.Script.Internals.Abstractions;

namespace Dr.Peril.Script.Internals.DTOs;

internal abstract class StateBase
{
    internal required string Id { get; init; }
    internal required Source Source { get; init; }
    internal abstract string TypeName { get; }
}
