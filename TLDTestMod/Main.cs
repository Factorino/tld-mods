using Il2Cpp;
using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace TLDTestMod
{
    public class Main : MelonMod
    {
        private static MelonLogger.Instance Logger;

        public override void OnInitializeMelon()
        {
            Logger = new MelonLogger.Instance(Info.Name);
            Logger.Msg($"Version {Info.Version} loaded");
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName.Equals("CampOffice"))
            {
                Logger.Msg(System.ConsoleColor.Green, $"Scene loaded: {sceneName}");
                MelonCoroutines.Start(ReplaceStaticCounters(sceneName));
            }
        }

        private IEnumerator DelayedObjectCollection(string sceneName)
        {
            yield return null;

            var scene = SceneManager.GetSceneByName(sceneName);
            if (!scene.IsValid() || !scene.isLoaded)
            {
                Logger.Warning($"Scene {sceneName} is not available for analysis");
                yield break;
            }

            try
            {
                List<GameObject> allObjects = GetAllGameObjectsInScene(scene);
                Logger.Msg(System.ConsoleColor.Cyan, $"Found {allObjects.Count} objects in scene '{sceneName}'");

                List<GameObject> countersObjects = allObjects
                    .Select(x => x.gameObject)
                    .Where(x => x.name.StartsWith("OBJ_Counter") && x.name.EndsWith("Prefab"))
                    .ToList();

                int i = 0;
                foreach (GameObject obj in countersObjects)
                {
                    string path = GetHierarchyPath(obj);
                    Logger.Msg($"[{i + 1}] {obj.name} | Active: {obj.activeInHierarchy} | Path: {path}");

                    Vector3 position = obj.transform.position;
                    Quaternion rotation = obj.transform.rotation;
                    Vector3 scale = obj.transform.localScale;

                    InstantiateObject(obj, position, rotation, scale);
                    obj.SetActive(false);

                    i++;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error occurred: {ex.GetType().Name} - {ex.Message}");
                Logger.Error(ex.StackTrace);
            }
        }

        private IEnumerator ReplaceStaticCounters(string sceneName)
        {
            yield return new WaitForSeconds(1.0f);

            Scene scene = SceneManager.GetSceneByName(sceneName);
            if (!scene.isLoaded) yield break;

            List<GameObject> allObjects = GetAllGameObjectsInScene(scene);

            var staticCounters = allObjects
                .Where(obj => obj != null
                           && obj.name.Contains("OBJ_Counter")
                           && obj.name.EndsWith("Prefab"))
                .ToList();

            if (staticCounters.Count == 0)
            {
                Logger.Msg("No static objects found for replacement.");
                yield break;
            }

            Logger.Msg($"Found static objects: {staticCounters.Count}");

            GameObject counterPrefab = FindCounterPrefab();
            if (counterPrefab == null)
            {
                Logger.Error("❌ CRITICAL ERROR: Failed to find OBJ_Counter_Prefab!");
                yield break;
            }

            Logger.Msg($"✓ Using prefab: {counterPrefab.name}");

            int replacedCount = 0;
            foreach (GameObject staticObj in staticCounters)
            {
                if (staticObj == null || !staticObj.activeInHierarchy) continue;

                Vector3 pos = staticObj.transform.position;
                Quaternion rot = staticObj.transform.rotation;
                Vector3 scale = staticObj.transform.localScale;

                GameObject newCounter = GameObject.Instantiate(counterPrefab, pos, rot);
                newCounter.transform.localScale = scale;
                newCounter.name = staticObj.name.Replace("_Prefab", "_Interactive");

                staticObj.SetActive(false);

                Logger.Msg($"✓ [{++replacedCount}] Replaced: {staticObj.name} → {newCounter.name} | Position: {pos}");
            }

            Logger.Msg(System.ConsoleColor.Green, $"✅ Objects replaced: {replacedCount}");
        }

        private List<GameObject> GetAllGameObjectsInScene(Scene scene)
        {
            List<GameObject> result = new List<GameObject>();
            GameObject[] rootObjects = scene.GetRootGameObjects();

            foreach (GameObject root in rootObjects)
            {
                if (root == null) continue;

                Stack<GameObject> stack = new Stack<GameObject>();
                stack.Push(root);

                while (stack.Count > 0)
                {
                    GameObject current = stack.Pop();
                    if (current == null) continue;

                    result.Add(current);

                    Transform transform = current.transform;
                    if (transform == null) continue;

                    int childCount = transform.childCount;
                    for (int i = childCount - 1; i >= 0; i--)
                    {
                        Transform child = transform.GetChild(i);
                        if (child != null && child.gameObject != null)
                        {
                            stack.Push(child.gameObject);
                        }
                    }
                }
            }

            return result;
        }

        private void InstantiateObject(GameObject obj, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            if (obj == null)
            {
                return;
            }

            GameObject newObject = GameObject.Instantiate(obj, position, rotation);
            newObject.transform.localScale = scale;
            newObject.name = "Custom_" + obj.name;

            if (newObject.GetComponent<Collider>() == null)
            {
                newObject.AddComponent<MeshCollider>();
            }
        }

        private GameObject? FindCounterPrefab()
        {
            string[] possiblePaths = new string[]
            {
                "Root/Art/B_Geo/OBJ_CounterCorner_Prefab",
                "OBJ_CounterCorner_Prefab",
            };

            foreach (string path in possiblePaths)
            {
                GameObject prefab = Resources.Load<GameObject>(path);
                if (prefab != null)
                {
                    Logger.Msg(System.ConsoleColor.Green, $"Prefab found: {path}");
                    return prefab;
                }
            }

            Logger.Warning("Prefab not found");
            return null;
        }

        private string GetHierarchyPath(GameObject obj)
        {
            var path = new StringBuilder();
            Transform t = obj.transform;
            while (t != null)
            {
                if (path.Length > 0)
                {
                    path.Insert(0, "/");
                }

                path.Insert(0, t.name);
                t = t.parent;
            }
            return path.ToString();
        }
    }
}