using Il2Cpp;
using MelonLoader;
using System;
using System.Collections;
using TLDTestMod.Entities;
using UnityEngine;

namespace TLDTestMod
{
    public class GearScanner
    {
        private readonly MelonLogger.Instance _logger;
        private readonly FileManager _fileManager;

        public GearScanner(MelonLogger.Instance logger, FileManager fileManager)
        {
            _logger = logger;
            _fileManager = fileManager;
        }

        public IEnumerator ScanSceneAsync(string sceneName, Action<BaseReport>? onComplete = null)
        {
            yield return new WaitForSeconds(2f);

            var report = new BaseReport
            {
                SceneName = sceneName,
                ScanTime = DateTime.Now
            };

            GearItem[] gearItems = UnityEngine.Object.FindObjectsByType<GearItem>(FindObjectsSortMode.None);
            _logger.Msg($"Found {gearItems.Length} gear items in scene");

            int processedCount = 0;

            foreach (GearItem item in gearItems)
            {
                if (item == null || item.gameObject == null || !item.gameObject.activeInHierarchy)
                {
                    continue; 
                }

                try
                {
                    ItemInfo itemInfo = GearItemMapper.MapToItemInfo(item);
                    report.AddItem(itemInfo);
                    processedCount++;
                }
                catch (Exception ex)
                {
                    _logger.Warning($"Error processing gear item '{item?.name}': {ex.Message}");
                }
            }

            _fileManager.SaveReport(report);
            _logger.Msg(ConsoleColor.Green, $"Scan completed: {processedCount}/{gearItems.Length} items processed");

            onComplete?.Invoke(report);
        }
    }
}