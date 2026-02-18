using System;
using System.Collections.Generic;

namespace TLDTestMod.Source.Entities
{
    public class SessionReport
    {
        public string SessionName { get; set; } = string.Empty;
        
        public DateTime SessionStart { get; set; } = DateTime.Now;
        
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public Dictionary<string, SceneReport> Scenes { get; set; } = new();

        public void AddOrUpdateScene(SceneReport sceneReport)
        {
            Scenes[sceneReport.SceneName] = sceneReport;
            LastUpdated = DateTime.Now;
        }

        public bool RemoveScene(string sceneName)
        {
            if (!Scenes.ContainsKey(sceneName))
            {
                return false;
            }

            Scenes.Remove(sceneName);
            LastUpdated = DateTime.Now;
            return true;
        }

        public SceneReport? GetScene(string sceneName) =>
            Scenes.TryGetValue(sceneName, out var report) ? report : null;
    }
}