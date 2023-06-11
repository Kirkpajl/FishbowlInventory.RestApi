using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FishbowlInventory
{
    internal static class DataRowExtensions
    {
        public static DateTime? DateField(this DataRow row, string fieldName) => DateTime.TryParse(row.Field<string>(fieldName), out DateTime dt) ? dt : (DateTime?)null;
        
        public static decimal DecimalField(this DataRow row, string fieldName)
        {
            object fieldValue = row.Field<object>(fieldName);

            if (decimal.TryParse(fieldValue.ToString(), out decimal dec))
            {
                return dec;
            }
            else if (double.TryParse(fieldValue.ToString(), out double dbl))
            {
                return Convert.ToDecimal(dbl);
            }
            else
            {
                return default;
            }
        }
    }
}
