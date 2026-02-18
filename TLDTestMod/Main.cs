using MelonLoader;
using TLDTestMod.Entities;

namespace TLDTestMod
{
    public class Main : MelonMod
    {
        private MelonLogger.Instance _logger = null!;
        private FileManager _fileManager = null!;
        private GearScanner _gearScanner = null!;
        private ModConfig? _config;

        public override void OnInitializeMelon()
        {
            _logger = new MelonLogger.Instance(Info.Name);
            _logger.Msg($"Version {Info.Version} loaded");

            _fileManager = new FileManager(Info.Name, _logger);
            _gearScanner = new GearScanner(_logger, _fileManager);

            _config = _fileManager.LoadConfig();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            _logger.Msg($"Scene loaded: {sceneName}");

            if (_config?.TrackedScenes?.Contains(sceneName) == true)
            {
                _logger.Msg(System.ConsoleColor.Green, "Tracked scene: {sceneName}. Starting scan...");
                MelonCoroutines.Start(_gearScanner.ScanSceneAsync(sceneName));
            }
        }
    }
}