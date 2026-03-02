using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TLDTestMod.Data.Enums;

namespace TLDTestMod.Data.Entities
{
    public class ItemData
    {
        [JsonProperty("Id", Required = Required.Always)]
        public string Id { get; private set; } = string.Empty;

        [JsonProperty("Name", Required = Required.Always)]
        public string Name { get; private set; } = string.Empty;

        [JsonProperty("Type", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public ItemType Type { get; private set; }

        [JsonProperty("Condition", Required = Required.Always)]
        public float Condition { get; private set; } = float.MinValue;

        [JsonProperty("Count", Required = Required.Always)]
        public int Count { get; set; } = 1;

        [JsonConstructor]
        public ItemData(
            string id,
            string name,
            ItemType type,
            float condition,
            int count)
        {
            Id = id;
            Name = name;
            Type = type;
            Condition = condition;
            Count = count;
        }
    }
}
