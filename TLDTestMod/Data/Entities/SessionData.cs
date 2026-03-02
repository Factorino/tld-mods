using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TLDTestMod.Data.Entities
{
    public class SessionData
    {
        [JsonProperty("SessionName", Required = Required.Always)]
        public string SessionName { get; private set; } = string.Empty;

        [JsonProperty("SessionStart", Required = Required.Always)]
        public DateTime SessionStart { get; private set; } = DateTime.Now;

        [JsonProperty("LastUpdated", Required = Required.Always)]
        public DateTime LastUpdated { get; private set; } = DateTime.Now;

        [JsonProperty("Scenes", Required = Required.Always)]
        public Dictionary<string, SceneData> Scenes { get; private set; } = new();

        [JsonConstructor]
        public SessionData(
            string sessionName,
            DateTime sessionStart,
            DateTime lastUpdated,
            Dictionary<string, SceneData> scenes)
        {
            SessionName = sessionName;
            SessionStart = sessionStart;
            LastUpdated = lastUpdated;
            Scenes = scenes ?? new();
        }
    }
}
