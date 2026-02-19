using Il2Cpp;
using MelonLoader;
using TLDTestMod.Source.Entities;
using TLDTestMod.Source.Services;
using UnityEngine;

namespace TLDTestMod
{
    public class Main : MelonMod
    {
        private FileManager _fileManager = null!;
        private GearScanner _gearScanner = null!;

        private SessionReport _currentSessionReport = null!;
        private string _currentSession = string.Empty;
        private string _currentScene = string.Empty;

        private bool _scanPressed;
        private bool _deletePressed;

        private MelonLogger.Instance _logger = null!;

        public override void OnInitializeMelon()
        {
            _logger = new MelonLogger.Instance(Info.Name);
            _logger.Msg($"Version {Info.Version} loaded");

            Settings.Initialize();
            _fileManager = new FileManager(Info.Name, _logger);
            _gearScanner = new GearScanner(_logger, _fileManager);
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            _currentScene = sceneName;
            _updateSessionContext();

            if (Settings.Options.DebugLogs)
            {
                _logger.Msg($"Scene loaded: {sceneName} | Session: {_currentSession}");
            }
        }

        public override void OnUpdate()
        {
            _updateSessionContext();

            if (Input.GetKeyDown(Settings.Options.ScanKey) && !_scanPressed)
            {
                _scanPressed = true;
                if (!string.IsNullOrEmpty(_currentScene) && _currentSessionReport != null)
                {
                    _gearScanner.ScanScene(_currentScene, _currentSession, _currentSessionReport);
                }
            }
            else if (Input.GetKeyUp(Settings.Options.ScanKey))
            {
                _scanPressed = false;
            }

            if (Input.GetKeyDown(Settings.Options.DeleteKey) && !_deletePressed)
            {
                _deletePressed = true;
                if (!string.IsNullOrEmpty(_currentScene) && _currentSessionReport != null)
                {
                    _gearScanner.RemoveSceneFromSession(_currentScene, _currentSession, _currentSessionReport);
                }
            }
            else if (Input.GetKeyUp(Settings.Options.DeleteKey))
            {
                _deletePressed = false;
            }
        }

        private void _updateSessionContext()
        {
            string newSession = _getCurrentSaveSlot();

            if (newSession != _currentSession)
            {
                _currentSession = newSession;
                _currentSessionReport = _fileManager.LoadSessionReport(_currentSession);
                _logger.Msg(System.ConsoleColor.Cyan, $"Session switched: Slot {_currentSession}");
            }
        }

        private string _getCurrentSaveSlot()
        {
            try
            {
                return SaveGameSystem.GetCurrentSaveName() ?? "Unknown";
            }
            catch
            {
                return "NoSave";
            }
        }
    }
}