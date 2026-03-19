using HarmonyLib;
using Il2Cpp;

namespace SpeedMod.Patches
{
    [HarmonyPatch(typeof(SafeCracking), nameof(SafeCracking.EnableSafeCrackingInterface))]
    internal class SafeCracking_EnableSafeCrackingInterface_Patch
    {
        public static void Postfix(SafeCracking __instance)
        {
            __instance.UnlockSafe();
        }
    }
}