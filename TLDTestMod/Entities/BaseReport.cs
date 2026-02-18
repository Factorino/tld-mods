using Il2CppTLD.Gear;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TLDTestMod.Entities
{
    public class BaseReport
    {
        public string SceneName { get; set; } = string.Empty;

        public DateTime ScanTime { get; set; } = DateTime.MinValue;

        public List<ItemInfo> Clothing { get; set; } = new List<ItemInfo>();
        public List<ItemInfo> Firestarting { get; set; } = new List<ItemInfo>();
        public List<ItemInfo> FirstAid { get; set; } = new List<ItemInfo>();
        public List<ItemInfo> Food { get; set; } = new List<ItemInfo>();
        public List<ItemInfo> Tools { get; set; } = new List<ItemInfo>();
        public List<ItemInfo> Materials { get; set; } = new List<ItemInfo>();
        public List<ItemInfo> Other { get; set; } = new List<ItemInfo>();

        public void AddItem(ItemInfo item)
        {
            var targetList = _getListByType(item.Type);

            var existing = targetList.FirstOrDefault(i =>
                i.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase));

            if (existing != null)
            {
                existing.Count += item.Count;
            }
            else
            {
                targetList.Add(item);
            }
        }

        private List<ItemInfo> _getListByType(GearType type) => type switch
        {
            GearType.Clothing => Clothing,
            GearType.Firestarting => Firestarting,
            GearType.FirstAid => FirstAid,
            GearType.Food => Food,
            GearType.Tool => Tools,
            GearType.Material => Materials,
            _ => Other
        };
    }
}