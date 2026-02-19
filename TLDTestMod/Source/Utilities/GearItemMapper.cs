using Il2Cpp;
using Il2CppRewired.Utils;
using Il2CppTLD.Gear;
using TLDTestMod.Source.Entities;
using UnityEngine;

namespace TLDTestMod.Source.Utilities
{
    public static class GearItemMapper
    {
        public static ItemInfo MapToItemInfo(GearItem item)
        {
            return new ItemInfo
            {
                Id = item.name,
                Name = _getItemName(item),
                Type = _getGearType(item),
                Condition = _getCondition(item),
                Count = _getItemCount(item)
            };
        }

        private static string _getItemName(GearItem item) =>
            !string.IsNullOrEmpty(item.DisplayName) ? item.DisplayName : item.name;

        private static GearType _getGearType(GearItem item)
        {
            if (item.IsAnyGearType(GearType.Clothing)) return GearType.Clothing;
            if (item.IsAnyGearType(GearType.Firestarting)) return GearType.Firestarting;
            if (item.IsAnyGearType(GearType.FirstAid)) return GearType.FirstAid;
            if (item.IsAnyGearType(GearType.Food)) return GearType.Food;
            if (item.IsAnyGearType(GearType.Tool)) return GearType.Tool;
            if (item.IsAnyGearType(GearType.Material)) return GearType.Material;
            return GearType.Other;
        }

        private static string _getCondition(GearItem item)
        {
            if (item.IsNullOrDestroyed())
            {
                return "0%";
            }

            float normalized = Mathf.Clamp01(item.GetNormalizedCondition());
            int percent = Mathf.RoundToInt(normalized * 100f);
            return $"{percent}%";
        }

        private static int _getItemCount(GearItem item)
        {
            StackableItem stackable = item.GetComponent<StackableItem>();
            return stackable != null && stackable.m_Units > 0 ? stackable.m_Units : 1;
        }
    }
}