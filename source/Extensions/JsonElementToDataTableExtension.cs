using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FishbowlInventory.Extensions
{
    internal static class JsonElementToDataTableExtension
    {
        // This needs some work, since I don't like the double enumeration and could use some optimization (and I am not sure it is even a good idea, but I needed reusable bits)...
        // but again, this could just be used as a stepping stone for others that are stuck and need a bit of a boost?

        public static DataTable JsonElementToDataTable(this JsonElement dataRoot)
        {
            var dataTable = new DataTable();
            var firstPass = true;
            foreach (var element in dataRoot.EnumerateArray())
            {
                if (firstPass)
                {
                    foreach (var col in element.EnumerateObject())
                    {
                        var colValue = col.Value;
                        dataTable.Columns.Add(new DataColumn(col.Name, colValue.ValueKind.ValueKindToType(colValue.ToString())));
                    }
                    firstPass = false;
                }
                var row = dataTable.NewRow();
                foreach (var col in element.EnumerateObject())
                {
                    row[col.Name] = col.Value.JsonElementToTypedValue();
                }
                dataTable.Rows.Add(row);
            }
            return dataTable;
        }

        private static Type ValueKindToType(this JsonValueKind valueKind, string value)
        {
            switch (valueKind)
            {
                case JsonValueKind.String:      // 3
                    return typeof(string);
                case JsonValueKind.Number:      // 4    
                    if (long.TryParse(value, out _))
                    {
                        return typeof(long);
                    }
                    else
                    {
                        return typeof(double);
                    }
                case JsonValueKind.True:        // 5
                case JsonValueKind.False:       // 6
                    return typeof(bool);
                case JsonValueKind.Undefined:   // 0
                    return null;
                case JsonValueKind.Object:      // 1 
                    return typeof(object);
                case JsonValueKind.Array:       // 2
                    return typeof(Array);
                case JsonValueKind.Null:        // 7
                    return null;
                default:
                    return typeof(object);
            }
        }

        private static object JsonElementToTypedValue(this JsonElement jsonElement)
        {
            switch (jsonElement.ValueKind)
            {
                case JsonValueKind.Object:      // 1  (these need special handling)?
                case JsonValueKind.Array:       // 2
                case JsonValueKind.String:      // 3
                    if (jsonElement.TryGetGuid(out Guid guidValue))
                    {
                        return guidValue;
                    }
                    else
                    {
                        if (jsonElement.TryGetDateTime(out DateTime datetime))
                        {
                            // If an offset was provided, use DateTimeOffset.
                            if (datetime.Kind == DateTimeKind.Local)
                            {
                                if (jsonElement.TryGetDateTimeOffset(out DateTimeOffset datetimeOffset))
                                {
                                    return datetimeOffset;
                                }
                            }
                            return datetime;
                        }
                        return jsonElement.ToString();
                    }
                case JsonValueKind.Number:      // 4    
                    if (jsonElement.TryGetInt64(out long longValue))
                    {
                        return longValue;
                    }
                    else
                    {
                        return jsonElement.GetDouble();
                    }
                case JsonValueKind.True:        // 5
                case JsonValueKind.False:       // 6
                    return jsonElement.GetBoolean();
                case JsonValueKind.Undefined:   // 0
                case JsonValueKind.Null:        // 7
                    return null;
                default:
                    return jsonElement.ToString();
            }
        }
    }
}
