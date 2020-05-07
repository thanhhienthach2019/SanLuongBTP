using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Services
{
    public class ConvertData
    {
        public static DataTable ConvertModelToDataTable<T>(List<T> models)
        {
            DataTable dataTable = new DataTable();
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in models)
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