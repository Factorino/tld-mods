using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                MelonCoroutines.Start(DelayedObjectCollection(sceneName));
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

                int i = 0;
                foreach (GameObject obj in allObjects)
                {
                    if (!obj.name.StartsWith("OBJ_Counter")) continue;
                    string path = GetHierarchyPath(obj);
                    Logger.Msg($"  [{i + 1}] {obj.name} | Active: {obj.activeInHierarchy} | Path: {path}");
                    i++;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while collecting objects: {ex.GetType().Name} - {ex.Message}");
                Logger.Error(ex.StackTrace);
            }
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

        private string GetHierarchyPath(GameObject obj)
        {
            var path = new System.Text.StringBuilder();
            Transform t = obj.transform;
            while (t != null)
            {
                if (path.Length > 0) path.Insert(0, " / ");
                path.Insert(0, t.name);
                t = t.parent;
            }
            return path.ToString();
        }
    }
}