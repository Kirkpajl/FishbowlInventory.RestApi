using FishbowlInventory.Extensions;
using System;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FishbowlInventory.Converters
{
    public class DataTableJsonConverter : JsonConverter<DataTable>
    {
        public override DataTable Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var jsonDoc = JsonDocument.ParseValue(ref reader);
            return jsonDoc.RootElement.JsonElementToDataTable();
        }

        public override void Write(Utf8JsonWriter writer, DataTable value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (DataRow dr in value.Rows)
            {
                writer.WriteStartObject();
                foreach (DataColumn col in value.Columns)
                {
                    var key = col.ColumnName.Trim();
                    var valueString = dr[col].ToString();
                    switch (col.DataType.FullName)
                    {
                        case "System.Guid":
                            writer.WriteString(key, valueString);
                            break;
                        case "System.Char":
                        case "System.String":
                            writer.WriteString(key, valueString);
                            break;
                        case "System.Boolean":
                            writer.WriteBoolean(key, bool.Parse(valueString));
                            //Boolean.TryParse(valueString, out bool boolValue);
                            //writer.WriteBoolean(key, boolValue);
                            break;
                        case "System.DateTime":
                            if (DateTime.TryParse(valueString, out DateTime dateValue))
                            {
                                writer.WriteString(key, dateValue);
                            }
                            else
                            {
                                writer.WriteString(key, "");
                            }
                            break;
                        case "System.TimeSpan":
                            if (DateTime.TryParse(valueString, out DateTime timeSpanValue))
                            {
                                writer.WriteString(key, timeSpanValue.ToString());
                            }
                            else
                            {
                                writer.WriteString(key, "");
                            }
                            break;
                        case "System.Byte":
                        case "System.SByte":
                        case "System.Decimal":
                        case "System.Double":
                        case "System.Single":
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                        case "System.UInt16":
                        case "System.UInt32":
                        case "System.UInt64":
                            if (long.TryParse(valueString, out long intValue))
                            {
                                writer.WriteNumber(key, intValue);
                            }
                            else
                            {
                                writer.WriteNumber(key, double.Parse(valueString));
                                //double.TryParse(valueString, out double doubleValue);
                                //writer.WriteNumber(key, doubleValue);
                            }
                            break;
                        default:
                            writer.WriteString(key, valueString);
                            break;
                    }
                }
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }
    }

    /*
    [
        {
        "id": 30,
        "num": "S10040"
        }
    ]
    */
}
