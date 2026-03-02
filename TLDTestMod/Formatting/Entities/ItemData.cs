namespace TLDTestMod.Formatting.Entities
{
    public class ItemData
    {
        public string Name { get; private set; } = string.Empty;

        public float Condition { get; private set; } = float.MinValue;

        public int Count { get; set; } = 1;

        public ItemData(
            string name,
            float condition,
            int count)
        {
            Name = name;
            Condition = condition;
            Count = count;
        }
    }
}
