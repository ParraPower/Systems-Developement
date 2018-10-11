using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class IDataRecordExtensions
    {
        public static int GetOrdinalOf(this IDataRecord reader, string name)
        {
            List<string> fieldNames = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                fieldNames.Add(reader.GetName(i));
            }
            return fieldNames.IndexOf(name);
        }
    }
}
