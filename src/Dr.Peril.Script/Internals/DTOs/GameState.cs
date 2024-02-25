namespace Dr.Peril.Script.Internals.DTOs;

internal class GameState : StateBase
{
    internal override string TypeName => "game";
    internal required string Name { get; init; }
    internal required Dictionary<string, string> Rooms { get; init; }
    internal required string StartRoomId { get; init; }
    internal required string Introduction { get; init; }
}
