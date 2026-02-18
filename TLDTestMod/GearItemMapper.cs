using Il2Cpp;
using Il2CppRewired.Utils;
using Il2CppTLD.Gear;
using TLDTestMod.Entities;
using UnityEngine;

namespace TLDTestMod
{
    public static class GearItemMapper
    {
        public static ItemInfo MapToItemInfo(GearItem item)
        {
            return new ItemInfo
            {
                Name = GetItemName(item),
                Type = DetermineGearType(item),
                Condition = FormatCondition(item),
                Count = GetItemCount(item)
            };
        }

        private static string GetItemName(GearItem item) =>
            !string.IsNullOrEmpty(item.DisplayName) ? item.DisplayName : item.name;

        private static GearType DetermineGearType(GearItem item)
        {
            if (item.IsAnyGearType(GearType.Clothing)) return GearType.Clothing;
            if (item.IsAnyGearType(GearType.Firestarting)) return GearType.Firestarting;
            if (item.IsAnyGearType(GearType.FirstAid)) return GearType.FirstAid;
            if (item.IsAnyGearType(GearType.Food)) return GearType.Food;
            if (item.IsAnyGearType(GearType.Tool)) return GearType.Tool;
            if (item.IsAnyGearType(GearType.Material)) return GearType.Material;
            return GearType.Other;
        }

        private static string FormatCondition(GearItem item)
        {
            if (item.IsNullOrDestroyed())
            {
                return "0%";
            }

            float normalized = Mathf.Clamp01(item.GetNormalizedCondition());
            int percent = Mathf.RoundToInt(normalized * 100f);
            return $"{percent}%";
        }

        private static int GetItemCount(GearItem item)
        {
            var stackable = item.GetComponent<StackableItem>();
            return stackable != null && stackable.m_Units > 0 ? stackable.m_Units : 1;
        }
    }
}