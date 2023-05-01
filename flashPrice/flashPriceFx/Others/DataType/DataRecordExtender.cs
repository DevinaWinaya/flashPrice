using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace HCFx.Extender.DataType
{
    public static class DataRecordExtender
    {
        /// <summary>
        /// Checks if the specified <see cref="IDataRecord"/> instance has a columnName
        /// </summary>
        /// <param name="record"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">When any of the parameters are null</exception>
        public static bool HasColumn(this IDataRecord record, string columnName)
        {
            if(record == null)
                throw new ArgumentNullException("record");

            if(columnName == null)
                throw new ArgumentNullException("columnName");

            for (int i = 0; i < record.FieldCount; i++)
            {
                if (record.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        public static IEnumerable<string> GetColumnNames(this IDataRecord record)
        {
            for (int i = 0; i < record.FieldCount; i++)
            {
                yield return record.GetName(i);
            }
        }
    }
}
