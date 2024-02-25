namespace Dr.Peril.Script.Model
{
    public class Inventory
    {
        private Inventory()
        { }

        public Dictionary<string, GameObject> Items { get; } = new();

        internal static Inventory Build() =>
            new();
    }
}
