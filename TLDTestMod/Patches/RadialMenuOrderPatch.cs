using HarmonyLib;
using Il2Cpp;

namespace TLDTestMod.Patches
{
    [HarmonyPatch(typeof(Panel_ActionsRadial), nameof(Panel_ActionsRadial.Initialize))]
    public static class RadialMenuOrderPatch
    {
        private static readonly string[] _customItemOrder = new string[]
        {
            "GEAR_Charcoal",
            "SprayPaint",
            "RockCache",
            "Map",
            "GEAR_BookA",
            "GEAR_HandheldShortwave",
            "GEAR_Camera"
        };

        [HarmonyPostfix]
        private static void setCustomNavigationOrder(ref Panel_ActionsRadial __instance)
        {
            __instance.m_NavigationRadialOrder = _customItemOrder;
        }
    }
}