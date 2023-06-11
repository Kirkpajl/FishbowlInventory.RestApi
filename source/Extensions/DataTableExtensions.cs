using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FishbowlInventory.Extensions
{
    public static class DataTableExtensions
    {
        public static DataTable ToDataTable(this JsonElement dataRoot)
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
                        var colType = colValue.ValueKind.ValueKindToType(colValue.ToString());

                        dataTable.Columns.Add(new DataColumn(col.Name, colType));
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
                    if (int.TryParse(value, out _))
                    {
                        return typeof(int);
                    }
                    else if (long.TryParse(value, out _))
                    {
                        return typeof(long);
                    }
                    else if (decimal.TryParse(value, out _))
                    {
                        return typeof(decimal);
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
                    return typeof(string);  //return null;
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
                    return DBNull.Value;  //return null;
                default:
                    return jsonElement.ToString();
            }
        }



        public static T ToObject<T>(this DataRow dataRow) where T : new()
        {
            T item = new T();

            foreach (DataColumn column in dataRow.Table.Columns)
            {
                PropertyInfo property = GetProperty(typeof(T), column.ColumnName);

                if (property != null && dataRow[column] != DBNull.Value && dataRow[column].ToString() != "NULL")
                {
                    property.SetValue(item, ChangeType(dataRow[column], property.PropertyType), null);
                }
            }

            return item;
        }

        private static PropertyInfo GetProperty(Type type, string attributeName)
        {
            PropertyInfo property = type.GetProperty(attributeName);

            if (property != null)
            {
                return property;
            }

            return type.GetProperties()
                 .Where(p => p.IsDefined(typeof(DisplayAttribute), false) && p.GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().Single().Name == attributeName)
                 .FirstOrDefault();
        }

        public static object ChangeType(object value, Type type)
        {
            if (type.IsEnum)
            {
                return Enum.Parse(type, value.ToString());
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                return Convert.ChangeType(value, Nullable.GetUnderlyingType(type));
            }

            return Convert.ChangeType(value, type);
        }
    }
}
