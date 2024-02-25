namespace Dr.Peril.Script.Model
{
    public class Player
    {
        private Player()
        { }

        internal static Player Build() =>
            new();
    }
}
