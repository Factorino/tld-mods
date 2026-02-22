using Il2Cpp;
using UnityEngine;

namespace TLDTestMod.Services
{
    public static class InputService
    {
        public static bool IsOpenBookPressed(KeyCode key)
        {
            if (InterfaceManager.IsOverlayActiveCached()) return false;
            if (GameManager.GetPlayerManagerComponent()?.IsInPlacementMode() == true) return false;
            if (string.IsNullOrEmpty(GameManager.m_ActiveScene)) return false;

            return InputManager.GetKeyDown(InputManager.m_CurrentContext, key);
        }

        public static bool IsTurnNext() =>
            GameManager.GetPlayerManagerComponent()?.m_ItemInHands == null &&
            InputManager.GetRotateClockwiseHeld(InputManager.m_CurrentContext);

        public static bool IsTurnPrev() =>
            GameManager.GetPlayerManagerComponent()?.m_ItemInHands == null &&
            InputManager.GetRotateCounterClockwiseHeld(InputManager.m_CurrentContext);

        public static bool IsClosePressed() =>
            !InterfaceManager.IsOverlayActiveCached() &&
            InputManager.GetHolsterPressed(InputManager.m_CurrentContext);

        public static bool HasItemInHands() =>
            GameManager.GetPlayerManagerComponent()?.m_ItemInHands != null;

        public static bool IsInStruggle() =>
            GameManager.GetPlayerStruggleComponent()?.InStruggle() == true;
    }
}
