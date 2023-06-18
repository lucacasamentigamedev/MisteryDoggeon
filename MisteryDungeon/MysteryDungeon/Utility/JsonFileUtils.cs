using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MisteryDungeon.MysteryDungeon.Utility {
    public static class JsonFileUtils {
        private static readonly JsonSerializerOptions _options =
            new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

        public static void SimpleWrite(object obj, string fileName) {
            var jsonString = System.Text.Json.JsonSerializer.Serialize(obj, _options);
            File.WriteAllText(fileName, jsonString);
        }

        public static void PrettyWrite(object obj, string fileName) {
            var options = new JsonSerializerOptions(_options) {
                WriteIndented = true
            };
            var jsonString = System.Text.Json.JsonSerializer.Serialize(obj, options);
            File.WriteAllText(fileName, jsonString);
        }

        public static void WriteJaggeredArray<T>(T array, string fileName) {
            var options = new JsonSerializerOptions(_options) {
                WriteIndented = true
            };
            options.Converters.Add(new TwoDimensionalIntArrayJsonConverter());
            string json = System.Text.Json.JsonSerializer.Serialize(array, options);
            File.WriteAllText(fileName, json);
        }

        public class MovementGridArrayElemSerialized {
            public List<List<int>> Map { get; set; }
        }

        public static T ReadJaggeredArray<T>(string fileName) {
            string json = File.ReadAllText(fileName);
            T obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }

        public static T ReadJson<T>(string filePath) {
            string text = File.ReadAllText(filePath);
            return System.Text.Json.JsonSerializer.Deserialize<T>(text);
        }
    }
}
