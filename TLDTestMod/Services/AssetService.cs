using MelonLoader;
using PastimeReading.Core;
using System.IO;
using UnityEngine;

namespace TLDTestMod.Services
{
    public static class AssetService
    {
        public static AssetBundle MainBundle { get; private set; } = new();
        public static AssetBundle HandsBundle { get; private set; } = new();

        public static void Load(string modsPath)
        {
            string mainPath = Path.Combine(modsPath, "aseets", ModConstants.BundleMain);
            string handsPath = Path.Combine(modsPath, "assets", ModConstants.BundleHands);

            MainBundle = AssetBundle.LoadFromFile(mainPath);
            HandsBundle = AssetBundle.LoadFromFile(handsPath);

            if (MainBundle == null)
            {
                MelonLogger.Error("Failed to load main AssetBundle");
            }
        }

        public static T? LoadAsset<T>(string name, bool fromHandsBundle = false) where T : UnityEngine.Object
        {
            var bundle = fromHandsBundle ? HandsBundle : MainBundle;
            return bundle?.LoadAsset<T>(name);
        }

        public static void UnloadAll()
        {
            MainBundle?.Unload(true);
            HandsBundle?.Unload(true);
        }
    }
}