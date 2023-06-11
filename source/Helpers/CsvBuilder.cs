using FishbowlInventory.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FishbowlInventory.Helpers
{
    internal class CsvBuilder
    {
        protected List<string> _items = new List<string>();

        public void Add(string item) => _items.Add(item);
        public void AddFormat(string format, params object[] args) => _items.Add(string.Format(format, args));

        public override string ToString() => string.Join(",", _items.Select(i => string.IsNullOrEmpty(i) ? string.Empty : $"\"{i}\""));
    }

    internal class CsvBuilder<T>
    {
        public static string[] Build(params T[] items)
        {
            // Initialize the CSV lines collection
            var lines = new List<string>
            {
                BuildRow(GetColumnNames())  // Headers line
            };

            // Add line for each item
            foreach (var item in items)
                lines.Add(BuildRow(GetRowValues(item)));

            // Return the Lines collection
            return lines.ToArray();
        }


        protected static string[] GetColumnNames()
        {
            // Declare a collection of Property names and the desired sort orders
            var propertyOrderDictionary = new Dictionary<string, int>();

            // Iterate through the public properties
            var properties = typeof(T).GetProperties(BindingFlags.Public);
            foreach (var property in properties)
            {
                // Declare the default property name/sort order
                string propertyName = property.Name;
                int propertyOrder = int.MaxValue;

                // Check for CSV attributes
                object[] attributes = property.GetCustomAttributes(true);
                foreach (object attribute in attributes)
                {
                    // Ignore the property
                    if (attribute is CsvIgnoreAttribute csvIgnore)
                    {
                        continue;
                        //if (csvIgnore.Condition == Enumerations.CsvIgnoreCondition.Always) continue;
                        //if (csvIgnore.Condition == Enumerations.CsvIgnoreCondition.WhenWritingDefault && property.GetValue(item))
                    }

                    // Override the default property name/sort order
                    if (attribute is CsvPropertyNameAttribute csvProperty)
                    {
                        propertyName = csvProperty.Name;
                        propertyOrder = csvProperty.Order ?? int.MaxValue;
                    }
                }

                // Add the property name/sort order to the collection
                propertyOrderDictionary.Add(propertyName, propertyOrder);
            }

            // Return the sorted collection of property names
            return propertyOrderDictionary
                .OrderBy(d => d.Value)
                .Select(d => d.Key)
                .ToArray();
        }

        protected static string[] GetRowValues(T item)
        {
            // Declare a collection of Property names and the desired sort orders
            var sortedProperties = new Dictionary<int, object>();
            var unsortedProperties = new List<object>();

            // Iterate through the public properties
            var properties = typeof(T).GetProperties(BindingFlags.Public);
            foreach (var property in properties)
            {
                // Declare the default property name/sort order
                object propertyValue = property.GetValue(item);
                int propertyOrder = int.MaxValue;

                // Check for CSV attributes
                object[] attributes = property.GetCustomAttributes(true);
                foreach (object attribute in attributes)
                {
                    // Ignore the property
                    if (attribute is CsvIgnoreAttribute csvIgnore)
                    {
                        if (csvIgnore.Condition == Enumerations.CsvIgnoreCondition.Always) continue;
                        if (csvIgnore.Condition == Enumerations.CsvIgnoreCondition.WhenWritingDefault && property.GetValue(item) == default) continue;
                        if (csvIgnore.Condition == Enumerations.CsvIgnoreCondition.WhenWritingNull && property.GetValue(item) == null) continue;
                    }

                    // Override the default property name/sort order
                    if (attribute is CsvPropertyNameAttribute csvProperty)
                    {
                        propertyOrder = csvProperty.Order ?? int.MaxValue;
                    }
                }

                // Add the property name/sort order to the collection
                if (propertyOrder < int.MaxValue)
                    sortedProperties.Add(propertyOrder, propertyValue);
                else
                    unsortedProperties.Add(propertyValue);
            }

            // Return the sorted collection of property names
            return sortedProperties
                .OrderBy(d => d.Key)
                .Select(d => d.Value)
                .Union(unsortedProperties)
                .Select(t => t.ToString())
                .ToArray();
        }



        protected static string BuildRow(string[] values) => string.Join(',', values.Select(v => EscapeValue(v ?? string.Empty)));

        protected static string EscapeValue(string value) => $"\"{value.Replace("\"", "\"\"")}\"";
    }

    /*internal class CsvBuilder<T>
    {
        protected string[] _escapeIndicators = new[] { ",", "\n", "\"" };

        public string Build(IEnumerable<T> objects, params Func<T, string>[] columnsGetters)
        {
            var csv = new StringBuilder();

            foreach (var @object in objects)
            {
                var values = columnsGetters.Select(cg => cg(@object));
                var row = BuildRow(values);
                csv.Append(row);
            }

            return csv.ToString();
        }

        public string Build(IEnumerable<T> objects, IEnumerable<string> columnsNames, params Func<T, string>[] columnsGetters)
        {
            bool isHeadersAndColumnsLengthEqual = columnsNames.Count() == columnsGetters.Length;
            if (!isHeadersAndColumnsLengthEqual)
                throw new ArgumentException("Header length and columns length are not equal.");

            var header = BuildRow(columnsNames);
            var values = Build(objects, columnsGetters);

            var headerWithValues = header + values;

            return headerWithValues;
        }

        protected string BuildRow(IEnumerable<string> values)
        {
            var row = new StringBuilder();
            foreach (var value in values)
            {
                var safeValue = value ?? string.Empty;

                if (HasToEscape(safeValue))
                    safeValue = EscapeValue(safeValue);

                row.Append(safeValue);
                row.Append(",");
            }

            row.Length -= 1;
            row.AppendLine();

            return row.ToString();
        }

        protected bool HasToEscape(string value) => _escapeIndicators.Any(i => value.Contains(i));

        protected string EscapeValue(string value)
        {
            value = value.Replace("\"", "\"\"");
            value = "\"" + value + "\"";
            return value;
        }
    }*/
}
