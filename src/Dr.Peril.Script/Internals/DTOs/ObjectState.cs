namespace Dr.Peril.Script.Internals.DTOs;

internal class ObjectState : StateBase
{
    internal override string TypeName => "object";
    internal required string Name { get; init; }
    internal required string Description { get; init; }
}
