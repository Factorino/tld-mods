using Il2Cpp;
using MelonLoader;
using System.Collections;
using UnityEngine;

namespace TLDTestMod
{
    public class Main : MelonMod
    {
        private MelonLogger.Instance _logger = null!;

        public override void OnInitializeMelon()
        {
            _logger = new MelonLogger.Instance(Info.Name);
            _logger.Msg($"Version {Info.Version} loaded");
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            _logger.Msg($"Scene loaded: {sceneName}");
            MelonCoroutines.Start(_logic(sceneName));
        }

        private IEnumerator _logic(string sceneName)
        {
            yield return new WaitForSeconds(2f);

            Container[] containers = UnityEngine.Object.FindObjectsByType<Container>(FindObjectsSortMode.None);
            _logger.Msg($"Found {containers.Length} containers in scene {sceneName}:");

            for (int i = 0; i < containers.Length; i++)
            {
                Container container = containers[i];
                if (!container.gameObject.activeInHierarchy)
                {
                    continue;
                }

                GearItem[] gearItems = container.GetComponentsInChildren<GearItem>(includeInactive: true);
                _logger.Msg($"  [{i + 1}] Container: {container.name} with {gearItems.Length} items:");

                for (int j = 0; j < gearItems.Length; j++)
                {
                    GearItem gearItem = gearItems[j];
                    _logger.Msg($"\t[{j + 1}] Gear: {gearItem.name}");
                }
            }
        }
    }
}