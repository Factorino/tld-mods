using HarmonyLib;
using Il2Cpp;

namespace SpeedMod.Patches
{
    [HarmonyPatch(typeof(vp_FPSController), nameof(vp_FPSController.GetSlopeMultiplier))]
    internal class SprintSpeedPatch
    {
        public static void Postfix(ref float __result)
        {
            if (GameManager.GetPlayerManagerComponent().PlayerIsSprinting())
            {
                __result *= 3.0f;
            }
        }
    }
}