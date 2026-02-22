using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using System;

namespace TLDTestMod.Patches
{
    [HarmonyPatch(typeof(Panel_ActionsRadial), nameof(Panel_ActionsRadial.UseItem))]
    public static class RadialBookInteractionPatch
    {
        private static readonly string _bookGearName = "GEAR_Book";

        [HarmonyPrefix]
        private static bool preventDefaultBookInteraction(ref Panel_ActionsRadial __instance)
        {
            RadialMenuArm activeArm = __instance.m_RadialArmThatDidInvoke;
            GearItem gearItem = activeArm.GetGearItem();
            if (!gearItem.name.StartsWith(_bookGearName))
            {
                return true;
            }

            return false;
        }

        [HarmonyPostfix]
        private static void addCustomBookInteraction(ref Panel_ActionsRadial __instance)
        {
            RadialMenuArm activeArm = __instance.m_RadialArmThatDidInvoke;
            GearItem gearItem = activeArm.GetGearItem();
            if (!gearItem.name.StartsWith(_bookGearName))
            {
                return;
            }

            MelonLogger.Msg(ConsoleColor.Blue, $"[BookPatch] Detected custom action with book: {gearItem.name}");
        }
    }
}