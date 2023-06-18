﻿using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MisteryDungeon.MysteryDungeon.Utility {
    public static class JsonFileUtils {
        private static readonly JsonSerializerOptions _options =
            new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

        public static void SimpleWrite(object obj, string fileName) {
            var jsonString = JsonSerializer.Serialize(obj, _options);
            File.WriteAllText(fileName, jsonString);
        }

        public static void PrettyWrite(object obj, string fileName) {
            var options = new JsonSerializerOptions(_options) {
                WriteIndented = true
            };
            var jsonString = JsonSerializer.Serialize(obj, options);
            File.AppendAllText(fileName, jsonString);
        }

        public static void SaveJaggeredArray<T>(T array, string fileName) {
            var options = new JsonSerializerOptions(_options) {
                WriteIndented = true
            };
            options.Converters.Add(new TwoDimensionalIntArrayJsonConverter());
            string json = JsonSerializer.Serialize(array, options);
            File.AppendAllText(fileName, json);
        }
    }
}