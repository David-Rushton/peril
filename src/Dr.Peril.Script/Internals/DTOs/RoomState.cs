namespace Dr.Peril.Script.Internals.DTOs;

internal class RoomState : StateBase
{
    internal override string TypeName => "room";
    internal required string Name { get; init; }
    internal required string Description { get; init; }
    internal required string Detailed { get; init; }
    internal required List<string> Objects { get; init; }
    internal required Dictionary<string, string> Connections { get; init; }
}
