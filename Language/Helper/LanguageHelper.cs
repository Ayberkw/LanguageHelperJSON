using System.Collections.Generic;
using System.IO;
using Language.Models;
using Newtonsoft.Json;

namespace Language.Helpers
{
    public static class LanguageHelper
    {
        private static readonly string filesDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,".. ","..","Files");

        public static string GetTranslation(string key, string languageCode = "en")
        {

            var filePath = Path.Combine("Files", $"{languageCode}.json");

            if (!File.Exists(filePath))
            {
                // Check if the default language file exists
                var defaultFilePath = Path.Combine(filesDirectoryPath, "en.json");
                if (File.Exists(defaultFilePath))
                {
                    // Read the default language file
                    string defaultJsonData = File.ReadAllText(defaultFilePath);
                    var defaultKeyValueList = JsonConvert.DeserializeObject<List<KeyValue>>(defaultJsonData);

                    // Find the default translation for the key
                    var defaultKeyValue = defaultKeyValueList.Find(x => x.Key == key);

                    if (defaultKeyValue != null)
                    {
                        // Create a new JSON file with a single key-value pair using the default language file
                        var keyValue = new KeyValue { Key = key, Value = defaultKeyValue.Value };
                        var keyValueList = new List<KeyValue> { keyValue };
                        string jsonString = JsonConvert.SerializeObject(keyValueList);
                        File.WriteAllText(filePath, jsonString);

                        return defaultKeyValue.Value;
                    }
                }
            }

            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                var keyValueList = JsonConvert.DeserializeObject<List<KeyValue>>(jsonData);

                if (keyValueList != null)
                {
                    var keyValue = keyValueList.Find(x => x.Key == key);

                    if (keyValue != null)
                    {
                        return keyValue.Value;
                    }
                    else
                    {
                        // Find the default translation for the key
                        var defaultFilePath = Path.Combine(filesDirectoryPath, "en.json");
                        if (File.Exists(defaultFilePath))
                        {
                            string defaultJsonData = File.ReadAllText(defaultFilePath);
                            var defaultKeyValueList = JsonConvert.DeserializeObject<List<KeyValue>>(defaultJsonData);
                            var defaultKeyValue = defaultKeyValueList.Find(x => x.Key == key);

                            if (defaultKeyValue != null)
                            {
                                // Add a new key-value pair to the existing JSON file using the default language file
                                keyValue = new KeyValue { Key = key, Value = defaultKeyValue.Value };
                                keyValueList.Add(keyValue);
                                string jsonString = JsonConvert.SerializeObject(keyValueList);
                                File.WriteAllText(filePath, jsonString);

                                return defaultKeyValue.Value;
                            }
                        }
                    }
                }
            }

            return "";
        }




        public static void SaveTranslation(string key, string value, string languageCode = "en")
        {
            var filePath = Path.Combine(filesDirectoryPath, $"{languageCode}.json");
            var keyValueList = new List<KeyValue>();

            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                keyValueList = JsonConvert.DeserializeObject<List<KeyValue>>(jsonData);
            }

            var keyValue = keyValueList.Find(x => x.Key == key);

            if (keyValue != null)
            {
                keyValue.Value = value;
            }
            else
            {
                keyValueList.Add(new KeyValue { Key = key, Value = value });
            }

            string jsonString = JsonConvert.SerializeObject(keyValueList);
            File.WriteAllText(filePath, jsonString);
        }
    }

}
