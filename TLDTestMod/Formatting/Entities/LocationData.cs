using System.Collections.Generic;

namespace TLDTestMod.Formatting.Entities
{
    public class LocationData
    {
        public string LocationName { get; private set; } = string.Empty;

        public List<CategoryData> Categories { get; private set; } = new();

        public LocationData(
            string locationName,
            List<CategoryData> categories)
        {
            LocationName = locationName;
            Categories = categories;
        }
    }
}
