using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace MisteryDungeon.MysteryDungeon.Utility {
    public class TwoDimensionalIntArrayJsonConverter : JsonConverter<int[,]> {
        public override int[,] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            var jsonDoc = JsonDocument.ParseValue(ref reader);

            var rowLength = jsonDoc.RootElement.GetArrayLength();
            var columnLength = jsonDoc.RootElement.EnumerateArray().First().GetArrayLength();

            int[,] grid = new int[rowLength, columnLength];

            int row = 0;
            foreach (var array in jsonDoc.RootElement.EnumerateArray()) {
                int column = 0;
                foreach (var number in array.EnumerateArray()) {
                    grid[row, column] = number.GetInt32();
                    column++;
                }
                row++;
            }

            return grid;
        }
        public override void Write(Utf8JsonWriter writer, int[,] value, JsonSerializerOptions options) {
            writer.WriteStartArray();
            for (int i = 0; i < value.GetLength(0); i++) {
                writer.WriteStartArray();
                for (int j = 0; j < value.GetLength(1); j++) {
                    writer.WriteNumberValue(value[i, j]);
                }
                writer.WriteEndArray();
            }
            writer.WriteEndArray();
        }
    }
}
