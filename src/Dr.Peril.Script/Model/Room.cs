using Dr.Peril.Script.Internals.DTOs;

namespace Dr.Peril.Script.Model;

public class Room
{
    private readonly RoomState _state;

    private Room(RoomState state, IEnumerable<GameObject> objects)
    {
        _state = state;
        Objects = objects.ToDictionary(o => o.Id, o => o);
    }

    public string Id => _state.Id;
    public string Name => _state.Name;
    public string Description => _state.Description;
    public string Detailed => _state.Detailed;
    public Dictionary<string, string> Connections => _state.Connections;
    public Dictionary<string, GameObject> Objects { get; init; }

    internal static Room Build(RoomState state, IEnumerable<GameObject> objects) =>
        new(state, objects);
}
