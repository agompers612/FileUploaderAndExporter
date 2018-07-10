using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace FileUploaderAndExporter.Classes
{
    public class DataTableToCSV
    {

        public static byte[] ConvertDataTableToCSV(DataTable table)
        {
            var result = new StringBuilder();
            for (int i = 0; i < table.Columns.Count; i++)
            {
                result.Append(spacedName(table.Columns[i].ColumnName));
                result.Append(i == table.Columns.Count - 1 ? "\n" : ",");
            }

            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    if (!table.Columns[i].ColumnName.Contains("filter"))
                    {
                        var cleanValue = row[i].ToString().Replace("\"", "\\\"");
                        cleanValue = row[i].ToString().Replace(',', ' ');
                        result.Append('"' + cleanValue + '"');
                        result.Append(i == table.Columns.Count - 1 ? "\n" : ",");
                    }
                    
                }
            }
            var bytes = Encoding.GetEncoding("utf-8").GetBytes(result.ToString());
            return bytes;
        }

        public static string spacedName(string value)
        {
            return Regex.Replace(value, "(?!^)([A-Z])", " $1");
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {

            DataTable dataTable = new DataTable(typeof(T).Name);

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    }
}