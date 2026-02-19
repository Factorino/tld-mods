using Il2CppTLD.Gear;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TLDTestMod.Source.Entities
{
    public class ItemInfo
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        [JsonConverter(typeof(StringEnumConverter))]
        public GearType Type { get; set; }

        public string Condition { get; set; } = string.Empty;

        public int Count { get; set; }
    }
}