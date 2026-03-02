using Newtonsoft.Json;
using System;
using System.IO;
using TLDTestMod.Data.Entities;

namespace TLDTestMod.Data.Implementations
{
    public class JsonDataProvider : IDataProvider
    {
        private readonly string _filePath;
        
        public JsonDataProvider(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path cannot be empty:", nameof(filePath));
            }

            _filePath = filePath;
        }

        public SessionData Load()
        {
            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException("Save file not found:", _filePath);
            }

            try
            {
                string jsonContent = File.ReadAllText(_filePath);

                var settings = new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    DateParseHandling = DateParseHandling.DateTime,
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
                };

                SessionData? sessionData = JsonConvert.DeserializeObject<SessionData>(jsonContent, settings);

                if (sessionData == null)
                {
                    throw new JsonSerializationException("Failed to deserialize session data (result is null).");
                }

                return sessionData;
            }
            catch (JsonException ex)
            {
                throw new InvalidDataException($"Invalid JSON format in file {_filePath}", ex);
            }
            catch (IOException ex)
            {
                throw new IOException($"Error reading file {_filePath}", ex);
            }
        }
    }
}
