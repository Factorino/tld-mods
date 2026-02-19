using Il2Cpp;
using Il2CppRewired.Utils;
using MelonLoader;
using System;
using TLDTestMod.Source.Entities;
using TLDTestMod.Source.Services;
using TLDTestMod.Source.Utilities;
using UnityEngine;

namespace TLDTestMod
{
    public class GearScanner
    {
        private readonly FileManager _fileManager;

        private readonly MelonLogger.Instance _logger;

        public GearScanner(MelonLogger.Instance logger, FileManager fileManager)
        {
            _logger = logger;
            _fileManager = fileManager;
        }

        public void ScanScene(string sceneName, string sessionName, SessionReport sessionReport)
        {
            SceneReport report = new SceneReport
            {
                SceneName = sceneName,
                ScanTime = DateTime.Now
            };

            int processed = 0;

            GearItem[] items = UnityEngine.Object.FindObjectsByType<GearItem>(FindObjectsSortMode.None);
            processed += _processItems(report, items);

            Container[] containers = UnityEngine.Object.FindObjectsByType<Container>(FindObjectsSortMode.None);
            foreach (Container container in containers)
            {
                if (!container.gameObject.activeInHierarchy)
                {
                    continue;
                }

                GearItem[] containerItems = container.GetComponentsInChildren<GearItem>(includeInactive: true);
                processed += _processItems(report, containerItems);
            }

            sessionReport.AddOrUpdateScene(report);
            _fileManager.SaveSessionReport(sessionReport);

            _logger.Msg(ConsoleColor.Green, $"Scanned: {sceneName} | Items: {processed}/{items.Length} | Session: {sessionName}");
        }

        public bool RemoveSceneFromSession(string sceneName, string sessionName, SessionReport sessionReport)
        {
            if (!sessionReport.RemoveScene(sceneName))
            {
                _logger.Warning($"Not found: {sceneName} in Session {sessionName}");
                return false;
            }

            _fileManager.SaveSessionReport(sessionReport);
            _logger.Msg(ConsoleColor.Yellow, $"Removed: {sceneName} from Session {sessionName}");
            return true;
        }

        private int _processItems(SceneReport report, GearItem[] items)
        {
            int processed = 0;

            foreach (GearItem item in items)
            {
                if (item == null || !item.gameObject.activeInHierarchy)
                {
                    continue;
                }
                if (!Settings.Options.IncludeDestroyed && item.IsNullOrDestroyed())
                {
                    continue;
                }

                try
                {
                    ItemInfo info = GearItemMapper.MapToItemInfo(item);
                    report.AddItem(info);
                    processed++;
                }
                catch (Exception ex)
                {
                    _logger.Warning($"Error processing '{item?.name}': {ex.Message}");
                }
            }

            return processed;
        }
    }
}