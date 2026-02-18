using MelonLoader;
using MelonLoader.Utils;
using Newtonsoft.Json;
using System;
using System.IO;
using TLDTestMod.Source.Entities;

namespace TLDTestMod.Source.Services
{
    public class FileManager
    {
        private readonly string _REPORT_PREFIX = "TLD_Scan_";
        private readonly string _REPORT_EXT = ".json";

        private readonly JsonSerializerSettings _jsonSettings = new()
        {
            DateFormatString = "yyyy-MM-dd HH:mm:ss",
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };

        private readonly string _modDir;

        private readonly MelonLogger.Instance _logger;

        public FileManager(string modName, MelonLogger.Instance logger)
        {
            _logger = logger;
            _modDir = Path.Combine(MelonEnvironment.ModsDirectory, modName);
            Directory.CreateDirectory(_modDir);
        }

        public SessionReport LoadSessionReport(string sessionName)
        {
            string path = _getSessionReportPath(sessionName);

            if (!File.Exists(path))
            {
                _logger.Msg($"[FileManager] New session report: {sessionName}");
                return new SessionReport { SessionName = sessionName };
            }

            try
            {
                string json = File.ReadAllText(path);
                var report = JsonConvert.DeserializeObject<SessionReport>(json);
                _logger.Msg($"[FileManager] Loaded session report: {report?.SessionName} ({report?.Scenes?.Count} scenes)");
                return report ?? new SessionReport { SessionName = sessionName };
            }
            catch (Exception e)
            {
                _logger.Error($"[FileManager] Load session report error: {e.Message}");
                return new SessionReport { SessionName = sessionName };
            }
        }

        public void SaveSessionReport(SessionReport report)
        {
            try
            {
                string path = _getSessionReportPath(report.SessionName);
                string json = JsonConvert.SerializeObject(report, _jsonSettings);
                File.WriteAllText(path, json);

                if (Settings.Options.DebugLogs)
                {
                    _logger.Msg($"[FileManager] Saved session report: {Path.GetFileName(path)}");
                }
            }
            catch (Exception e)
            {
                _logger.Error($"[FileManager] Save session report error: {e.Message}");
            }
        }

        private string _getSessionReportPath(string sessionName) =>
            Path.Combine(_modDir, $"{_REPORT_PREFIX}Slot_{sessionName}{_REPORT_EXT}");
    }
}