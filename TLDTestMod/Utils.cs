using Il2Cpp;
using MelonLoader;
using System;

namespace TLDTestMod
{
    public static class Utils
    {
        public static string GetCurrentSaveName()
        {
            return SaveGameSystem.GetCurrentSaveName();
        }

        public static bool IsScenePlayable(string scene)
        {
            return !(
                string.IsNullOrEmpty(scene)
                || scene.Contains("MainMenu")
                || scene == "Boot"
                || scene == "Empty"
            );
        }
    }
}
