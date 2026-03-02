using System;
using System.Collections.Generic;

namespace TLDTestMod.Formatting.Entities
{
    public class SessionData
    {
        public string SessionName { get; private set; } = string.Empty;

        public DateTime SessionStart { get; private set; } = DateTime.Now;

        public DateTime LastUpdated { get; private set; } = DateTime.Now;
    
        public List<LocationData> Locations { get; private set; } = new();
    
        public SessionData(
            string SessionName,
            DateTime SessionStart)
        {

        }
    }
}
