using MelonLoader;
using MelonLoader.Utils;
using Newtonsoft.Json;
using System;
using System.IO;
using TLDTestMod.Entities;

namespace TLDTestMod
{
    public class FileManager
    {
        private readonly string _CONFIG_FILENAME = "TLDTestModConfig.json";
        private readonly string _REPORT_FILENAME = "TLDTestModReport.json";
        private readonly JsonSerializerSettings _JSON_CONFIG = new JsonSerializerSettings
        {
            DateFormatString = "yyyy-MM-dd HH:mm:ss",
            Formatting = Formatting.Indented,
        };

        private readonly string _configPath;
        private readonly string _reportPath;
        private readonly MelonLogger.Instance _logger;

        public FileManager(string modName, MelonLogger.Instance logger)
        {
            _logger = logger;

            string modDirectory = Path.Combine(MelonEnvironment.ModsDirectory, modName);
            if (!Directory.Exists(modDirectory))
            {
                Directory.CreateDirectory(modDirectory);
            }

            _configPath = Path.Combine(modDirectory, _CONFIG_FILENAME);
            _reportPath = Path.Combine(modDirectory, _REPORT_FILENAME);
        }

        public ModConfig? LoadConfig()
        {
            if (!File.Exists(_configPath))
            {
                _logger.Warning("Config file not found");
                return null;
            }

            try
            {
                string json = File.ReadAllText(_configPath);
                var config = JsonConvert.DeserializeObject<ModConfig>(json);

                if (config == null)
                {
                    _logger.Warning("Config deserialized to null");
                    return null;
                }

                _logger.Msg(ConsoleColor.Green, $"Config loaded. Monitoring {config.TrackedScenes.Count} scenes");
                return config;
            }
            catch (Exception e)
            {
                _logger.Error($"Error loading config: {e.Message}");
                return null;
            }
        }

        public void SaveReport(BaseReport report)
        {
            try
            {
                string json = JsonConvert.SerializeObject(report, _JSON_CONFIG);
                File.WriteAllText(_reportPath, json);
                _logger.Msg(ConsoleColor.Green, "Report saved successfully");
            }
            catch (Exception e)
            {
                _logger.Error($"Error saving report: {e.Message}");
            }
        }
    }
}