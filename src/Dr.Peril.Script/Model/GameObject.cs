using Dr.Peril.Script.Internals.DTOs;

namespace Dr.Peril.Script.Model;

public class GameObject
{
    private readonly ObjectState _state;

    private GameObject(ObjectState state) =>
        _state = state;

    public string Id => _state.Id;
    public string Name => _state.Name;
    public string Description => _state.Description;

    internal static GameObject Build(ObjectState state) =>
        new(state);
}
