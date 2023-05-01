using System;
using System.Data;
using System.Text;
using System.Linq;
namespace othersFx
{
    public static class DBHelper
    {
        #region static method

        /// <summary>
        /// get string type boolean in DB to boolean
        /// </summary>
        /// <param name="myBOOL">string to convert.</param>        
        public static bool readMyBoolean(string myBOOL)
        {
            if (myBOOL == "T")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ColumnExists(IDataReader reader, string columnName)
        {

            return reader.GetSchemaTable()
                         .Rows
                         .OfType<DataRow>()
                         .Any(row => row["ColumnName"].ToString() == columnName);
        }


        /// <summary>
        /// get string type boolean in DB to boolean
        /// </summary>
        /// <param name="myBOOL">string to convert.</param>        
        public static bool? readMyNullBoolean(string myBOOL)
        {
            if (myBOOL == "T")
            {
                return true;
            }
            else if (myBOOL == "F")
            {
                return false;
            }
            else
            {
                return null;
            }
        }



        /// <summary>
        /// get string type boolean Y / N in DB to boolean
        /// </summary>
        /// <param name="myBOOL">string to convert.</param>        
        public static bool readMyBooleanYN(string myBOOL)
        {
            if (myBOOL == "Y")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// get boolean to string type boolean in DB
        /// </summary>
        /// <param name="myBOOL">boolean to convert.</param>    
        public static string setMyBoolean(bool myBOOL)
        {
            if (myBOOL)
            {
                return "T";
            }
            else
            {
                return "F";
            }
        }

        /// <summary>
        /// get boolean to string type boolean (Y/N) in DB
        /// </summary>
        /// <param name="myBOOL">boolean to convert.</param>    
        public static string setMyBooleanYN(bool myBOOL)
        {
            if (myBOOL)
            {
                return "Y";
            }
            else
            {
                return "N";
            }
        }

        /// <summary>
        /// get nullable boolean to string type boolean in DB
        /// </summary>
        /// <param name="myBOOL">boolean to convert.</param>
        public static string setMyBoolean(bool? myBOOL)
        {
            if (myBOOL.Value == true)
            {
                return "T";
            }
            else
            {
                return "F";
            }
        }

        /// <summary>
        /// get boolean to string type boolean (Y/N) in DB
        /// </summary>
        /// <param name="myBOOL">boolean to convert.</param>    
        public static string setMyBooleanYN(bool? myBOOL)
        {
            if (myBOOL.Value == true)
            {
                return "Y";
            }
            else
            {
                return "N";
            }
        }

        /// <summary>
        /// clean parameter send to DB
        /// </summary>
        /// <param name="myBOOL">boolean to convert.</param>
        public static string cleanParam(string str)
        {
            StringBuilder b = new StringBuilder(str);
            return b.Replace("'", "''").Replace("--", " ").ToString();
        }


        /// <summary>
        /// baca DOM_MYBOOLEAN varchar(1) T / F translate ke boolean. 
        /// </summary>
        /// <param name="dataRecord">dataRecord nya</param>
        /// <param name="fieldName">nama field database</param>
        /// <param name="notFoundDefaultValue">kalau gk dapet value, return nilai apa?</param>
        /// <returns></returns>
        public static bool getDBBool(this IDataRecord dataRecord, string fieldName, bool notFoundDefaultValue)
        {
            bool retVal = new bool();
            if (!dataRecord.IsDBNull(dataRecord.GetOrdinal(fieldName)))
            {
                retVal = readMyBoolean(dataRecord.GetString(dataRecord.GetOrdinal(fieldName)));
            }
            else
            {
                retVal = notFoundDefaultValue;
            }
            return retVal;
        }

        /// <summary>
        /// Returns a boolean value from an <see cref="IDataRecord"/> instance's field name. Throws an exception if the field is null.
        /// </summary>
        /// <param name="record"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static bool getDBBoolNonNullable(this IDataRecord record, string fieldName)
        {
            var ordinal = record.GetOrdinal(fieldName);
            if(record.IsDBNull(ordinal))
                throw new IndexOutOfRangeException("The specified field is null");

            var tfBooleanValue = record.GetString(ordinal);
            if(!(tfBooleanValue.Equals("T", StringComparison.InvariantCultureIgnoreCase) || tfBooleanValue.Equals("F", StringComparison.InvariantCultureIgnoreCase)))
                throw new IndexOutOfRangeException("The value is not T or F");

            return tfBooleanValue.Equals("T", StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool? getDBBoolNullable(this IDataRecord dataRecord, string fieldName)
        {
            bool? retVal = new bool?();
            if (!dataRecord.IsDBNull(dataRecord.GetOrdinal(fieldName)))
            {
                retVal = readMyBoolean(dataRecord.GetString(dataRecord.GetOrdinal(fieldName)));
            }
            else
            {
                retVal = null;
            }
            return retVal;
        }

        public static DateTime getDBDateTime(this IDataRecord dataRecord, string fieldName)
        {
            return getDBDateTime(dataRecord, fieldName, false);
        }

        public static DateTime getDBDateTime(this IDataRecord dataRecord, string fieldName, bool notFoundReturnMinValue)
        {
            DateTime retVal = new DateTime();
            if (!dataRecord.IsDBNull(dataRecord.GetOrdinal(fieldName)))
            {
                retVal = dataRecord.GetDateTime(dataRecord.GetOrdinal(fieldName));
            }
            else
            {
                if (notFoundReturnMinValue)
                { retVal = DateTime.MinValue; }
            }
            return retVal;
        }

        public static DateTime? getDBDateTimeNullable(this IDataRecord dataRecord, string fieldName)
        {
            return getDBDateTimeNullable(dataRecord, fieldName, false);
        }

        public static DateTime? getDBDateTimeNullable(this IDataRecord dataRecord, string fieldName, bool ifNotFoundReturnNull)
        {
            DateTime? retVal = new DateTime?();
            if (!dataRecord.IsDBNull(dataRecord.GetOrdinal(fieldName)))
            {
                retVal = dataRecord.GetDateTime(dataRecord.GetOrdinal(fieldName));
            }
            else
            {
                if (ifNotFoundReturnNull)
                {
                    retVal = null;
                }
                else
                {
                    retVal = DateTime.MinValue;
                }
            }
            return retVal;
        }

        public static string getDBString(this IDataRecord dataRecord, string fieldName)
        {
            string retVal = "";
            if (!dataRecord.IsDBNull(dataRecord.GetOrdinal(fieldName)))
            {
                retVal = dataRecord.GetString(dataRecord.GetOrdinal(fieldName));
            }
            return retVal;
        }

        public static string getDBStringNullable(this IDataRecord dataRecord, string fieldName)
        {
            var ordinal = dataRecord.GetOrdinal(fieldName);
            return dataRecord.IsDBNull(ordinal) ? null : dataRecord.GetString(ordinal);
        }

        public static string getDBStringNonNullable(this IDataRecord record, string fieldName)
        {
            var ordinal = record.GetOrdinal(fieldName);
            return record.GetString(ordinal);
        }

        public static int getDBInteger(this IDataRecord record, string fieldName)
        {
            var ordinal = record.GetOrdinal(fieldName);
            return record.GetInt32(ordinal);
        }

        /// <summary>
        /// return null klo isi database nya null. 
        /// </summary>
        /// <param name="dataRecord"></param>
        /// <param name="fieldName"></param>
        /// <param name="notFoundReturnNull"></param>
        /// <returns></returns>
        public static Int32? getDBIntegerNullable(this IDataRecord dataRecord, string fieldName, bool notFoundReturnNull)
        {
            Int32? retVal = new Int32?();
            if (!dataRecord.IsDBNull(dataRecord.GetOrdinal(fieldName)))
            {
                retVal = dataRecord.GetInt32(dataRecord.GetOrdinal(fieldName));
            }
            else
            {
                if (notFoundReturnNull)
                {
                    retVal = null;
                }
                else
                {
                    retVal = 0;
                }
            }
            return retVal;
        }

        /// <summary>
        /// return 0 (nol) jika isi database nya null. 
        /// </summary>
        /// <param name="dataRecord"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static Int32? getDBIntegerNullable(this IDataRecord dataRecord, string fieldName)
        {
            return getDBIntegerNullable(dataRecord, fieldName, true);
        }

        public static long getDBLong(this IDataRecord dataRecord, string fieldName)
        {
            var ordinal = dataRecord.GetOrdinal(fieldName);
            return dataRecord.GetInt64(ordinal);
        }


        /// <summary>
        /// return 0 (nol) jika isi database nya null. 
        /// </summary>
        /// <param name="dataRecord"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static Int64? getDBBigIntegerNullable(this IDataRecord dataRecord, string fieldName, bool notFoundReturnNull)
        {
            Int64? retVal = new Int64?();
            if (!dataRecord.IsDBNull(dataRecord.GetOrdinal(fieldName)))
            {
                retVal = dataRecord.GetInt64(dataRecord.GetOrdinal(fieldName));
            }
            else
            {
                if (notFoundReturnNull)
                {
                    retVal = null;
                }
                else
                {
                    retVal = 0;
                }
            }
            return retVal;
        }



        /// <summary>
        /// return null klo isi database nya null. 
        /// </summary>
        /// <param name="dataRecord"></param>
        /// <param name="fieldName"></param>
        /// <param name="notFoundReturnNull"></param>
        /// <returns></returns>
        public static Decimal? getDBDecimalNullable(this IDataRecord dataRecord, string fieldName, bool notFoundReturnNull)
        {
            Decimal? retVal = new Decimal?();
            if (!dataRecord.IsDBNull(dataRecord.GetOrdinal(fieldName)))
            {
                retVal = dataRecord.GetDecimal(dataRecord.GetOrdinal(fieldName));
            }
            else
            {
                if (notFoundReturnNull)
                {
                    retVal = null;
                }
                else
                {
                    retVal = 0;
                }
            }
            return retVal;
        }

        /// <summary>
        /// return 0 (nol) jika isi database nya null. 
        /// </summary>
        /// <param name="dataRecord"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static Decimal? getDBDecimalNullable(this IDataRecord dataRecord, string fieldName)
        {
            return getDBDecimalNullable(dataRecord, fieldName, true);
        }


        /// <summary>
        /// return 0 (nol) jika isi database nya null. 
        /// </summary>
        /// <param name="dataRecord"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static Decimal getDBDecimal(this IDataRecord dataRecord, string fieldName)
        {
            Decimal retVal = new Decimal();
            if (!dataRecord.IsDBNull(dataRecord.GetOrdinal(fieldName)))
            {
                retVal = dataRecord.GetDecimal(dataRecord.GetOrdinal(fieldName));
            }
            else
            {                
                    retVal = 0;
            }
            return retVal;
        }
          
        //bool? to bool checkbox
        public static bool getBoolCheckBox(this IDataRecord dataRecord, string myBOOL)
        {


            if (!dataRecord.IsDBNull(dataRecord.GetOrdinal(myBOOL)))
            {
                if (dataRecord.GetString(dataRecord.GetOrdinal(myBOOL)) == "T")
                {
                    return false;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }








        #endregion

        

        /// <summary>
        /// Encapsulates null check and returns DBNull.Value if it's null, otherwise just return the value.
        /// It also saves
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns>value or DBNull.Value</returns>
        public static object GetSqlValue(this object value)
        {
            return value ?? DBNull.Value;
        }
    }
}
