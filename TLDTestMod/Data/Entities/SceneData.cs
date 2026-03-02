using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TLDTestMod.Data.Entities
{
    public class SceneData
    {
        [JsonProperty("SceneName", Required = Required.Always)]
        public string SceneName { get; private set; } = string.Empty;

        [JsonProperty("ScanTime", Required = Required.Always)]
        public DateTime ScanTime { get; private set; } = DateTime.Now;

        [JsonProperty("Clothing")]
        public List<ItemData> Clothing { get; private set; } = new();

        [JsonProperty("Firestarting")]
        public List<ItemData> Firestarting { get; private set; } = new();

        [JsonProperty("FirstAid")]
        public List<ItemData> FirstAid { get; private set; } = new();

        [JsonProperty("Food")]
        public List<ItemData> Food { get; private set; } = new();

        [JsonProperty("Tools")]
        public List<ItemData> Tools { get; private set; } = new();

        [JsonProperty("Materials")]
        public List<ItemData> Materials { get; private set; } = new();

        [JsonProperty("Other")]
        public List<ItemData> Other { get; private set; } = new();

        [JsonConstructor]
        public SceneData(
            string sceneName,
            DateTime scanTime,
            List<ItemData> clothing,
            List<ItemData> firestarting,
            List<ItemData> firstAid,
            List<ItemData> food,
            List<ItemData> tools,
            List<ItemData> materials,
            List<ItemData> other)
        {
            SceneName = sceneName;
            ScanTime = scanTime;
            Clothing = clothing ?? new();
            Firestarting = firestarting ?? new();
            FirstAid = firstAid ?? new();
            Food = food ?? new();
            Tools = tools ?? new();
            Materials = materials ?? new();
            Other = other ?? new();
        }
    }
}
