using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HCFx.Extender.DataType
{
    public static class AdoUtilities
    {
        /// <summary>
        /// Retrieves all available field names from specified <see cref="IDataRecord"/> instance
        /// </summary>
        /// <param name="record">The record to retrieve field names from</param>
        /// <returns></returns>
        public static IEnumerable<string> GetFieldNames(this IDataRecord record)
        {
            for (int i = 0; i < record.FieldCount; i++)
            {
                yield return record.GetName(i);
            }
        }

        public static bool IsFieldNameExists(this IDataRecord record, string fieldName)
        {
            return record.GetFieldNames().Contains(fieldName);
        }

        public static IDictionary<string, int> CreateFieldNumberAndNameMap(this IDataRecord record)
        {
            var dictionary = new Dictionary<string, int>();
            for (int i = 0; i < record.FieldCount; i++)
            {
                dictionary.Add(record.GetName(i), i);
            }

            return dictionary;
        }

        public static TResult MapDataRecordToResult<TResult>(IDataRecord record) where TResult: new()
        {
            var instance = new TResult();

            //retrieve fields
            //TODO: Currently Properties only
            var propertyFields = typeof(TResult).GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(p => p.Name);
            var recordColumns = record.CreateFieldNumberAndNameMap();

            foreach (var recordColumn in recordColumns)
            {
                PropertyInfo correspondingProperty;
                if (!propertyFields.TryGetValue(recordColumn.Key, out correspondingProperty)) continue;
                var value = record.GetValue(recordColumn.Value);
                SetProcessedValue(instance, correspondingProperty, value);
            }

            return instance;
        }

        private static void SetProcessedValue<TResult>(TResult instance, PropertyInfo property, object value)
        {
            var propertyTypeCode = Type.GetTypeCode(property.PropertyType);
            switch (propertyTypeCode)
            {
                case TypeCode.Boolean:
                    var boolStrValue = value.ToString();
                    if (!(boolStrValue.Equals("T", StringComparison.InvariantCultureIgnoreCase) ||
                          boolStrValue.Equals("F", StringComparison.InvariantCultureIgnoreCase)))
                        throw new ArgumentException("The field is not T or F value");
                    var convertedValue = boolStrValue.Equals("T", StringComparison.InvariantCultureIgnoreCase);

                    property.SetValue(instance, convertedValue, null);
                    break;
            }
        }

    }
}
