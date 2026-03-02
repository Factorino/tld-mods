using System.Collections.Generic;
using TLDTestMod.Data.Enums;

namespace TLDTestMod.Formatting.Entities
{
    public class CategoryData
    {
        public ItemType Type { get; private set; }

        public List<ItemData> Items { get; private set; } = new();

        public CategoryData(
            ItemType type,
            List<ItemData> items)
        {
            Type = type;
            Items = items;
        }
    }
}
