using MelonLoader;
using System.IO;
using TLDTestMod.Components;

namespace TLDTestMod
{
    public class Main : MelonMod
    {
        private MelonLogger.Instance _logger = null!;

        public override void OnInitializeMelon()
        {
            Settings.OnLoad();
            _logger = new MelonLogger.Instance(Info.Name);
            _logger.Msg($"Version {Info.Version} loaded");

            string? modsPath = Path.GetDirectoryName(typeof(Main).Assembly.Location);
            _logger.Msg(System.ConsoleColor.Blue, $"Mods path: {modsPath}");
        }

        public override void OnSceneWasInitialized(int level, string name)
        {
            if (!Utils.IsScenePlayable(name)) return;
        }

        public override void OnUpdate()
        {
            BookController.Update();
        }

        public override void OnSceneWasUnloaded(int level, string name)
        {
            BookController.OnSceneUnload();
        }

        public override void OnApplicationQuit()
        {
            BookController.Cleanup();
        }
    }
}