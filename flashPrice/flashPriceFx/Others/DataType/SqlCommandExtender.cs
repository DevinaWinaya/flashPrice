using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace HCFx.Extender.DataType
{
    public static class SqlCommandExtender
    {
        

        #region "SqlParameter Extensions"

        /// <summary>
        /// Add array parameter to <see cref="IDbCommand"/> instance
        /// </summary>
        /// <typeparam name="T">The type corresponds to given <see cref="IEnumerable{T}"/> generic type</typeparam>
        /// <param name="cmd">The <see cref="IDbCommand"/> object to add parameters</param>
        /// <param name="name">The parameter name</param>
        /// <param name="values">The <see cref="IEnumerable{T}"/> instance containing values to add to IN clause</param>
        /// <remarks>
        /// Enclose the parameter name in parentheses. Example: WHERE IN (@param)
        /// 
        /// Source: https://stackoverflow.com/questions/2377506/pass-array-parameter-in-sqlcommand
        /// Licensed under cc by-sa 3.0 with attribution required. rev 2018.3.20.29500
        /// </remarks>
        public static void AddArrayParameters<T>(this IDbCommand cmd, string name, IEnumerable<T> values, DbType? dbType = null, int? size = null)
        {
            name = name.StartsWith("@") ? name : "@" + name;
            var names = string.Join(", ", values.Select((value, i) =>
            {
                var paramName = name + i;
                cmd.AddParameterWithValue(paramName, value, dbType, size);
                return paramName;
            }));

            cmd.CommandText = cmd.CommandText.ReplaceIgnoreCase(name, names);
        }

        /// <summary>
        /// Add array parameter to <see cref="SqlCommand"/> instance
        /// </summary>
        /// <typeparam name="T">The type corresponds to given <see cref="IEnumerable{T}"/> generic type</typeparam>
        /// <param name="cmd">The <see cref="SqlCommand"/> object to add parameters</param>
        /// <param name="name">The parameter name</param>
        /// <param name="values">The <see cref="IEnumerable{T}"/> instance containing values to add to IN clause</param>
        /// <param name="sqlDbType"></param>
        /// <param name="size"></param>
        /// <remarks>
        /// Enclose the parameter name in parentheses. Example: WHERE IN (@param)
        /// 
        /// Source: https://stackoverflow.com/questions/2377506/pass-array-parameter-in-sqlcommand
        /// Licensed under cc by-sa 3.0 with attribution required. rev 2018.3.20.29500
        /// </remarks>
        public static void AddArrayParameters<T>(this SqlCommand cmd, string name, IEnumerable<T> values,
            SqlDbType? sqlDbType = null, int? size = null)
        {
            name = name.StartsWith("@") ? name : "@" + name;
            var names = string.Join(", ", values.Select((value, i) =>
            {
                var paramName = name + i;
                cmd.AddParameterWithValue(paramName, value, sqlDbType, size);
                return paramName;
            }));

            cmd.CommandText = cmd.CommandText.ReplaceIgnoreCase(name, names);
        }

        /// <summary>
        /// Adds new parameter to <see cref="IDbCommand"/> instance parameters with any value type.
        /// </summary>
        /// <param name="cmd">The instance of <see cref="IDbCommand"/> to add parameter to</param>
        /// <param name="name">The name of parameter</param>
        /// <param name="value">The value of parameter</param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        public static void AddParameterWithValue(this IDbCommand cmd, string name, object value, DbType? dbType = null, int? size = null)
        {
            var parameter = cmd.CreateParameter();
            parameter.ParameterName = name;
            if (dbType.HasValue)
                parameter.DbType = dbType.Value;
            if (size.HasValue)
                parameter.Size = size.Value;

            parameter.Value = value;
            cmd.Parameters.Add(parameter);
        }

        public static void AddParameterWithValue(this SqlCommand cmd, string name, object value,
            SqlDbType? sqlDbType = null, int? size = null)
        {
            var parameter = cmd.CreateParameter();
            parameter.ParameterName = name;
            if (sqlDbType.HasValue)
                parameter.SqlDbType = sqlDbType.Value;
            if (size.HasValue)
                parameter.Size = size.Value;

            parameter.Value = value;
            cmd.Parameters.Add(parameter);
        }

        /// <summary>
        /// Add array parameter to <see cref="ICollection{T}"/> instance
        /// </summary>
        /// <typeparam name="TParam"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="parameterCollection">The collection of parameters to add</param>
        /// <param name="name">The name of parameter</param>
        /// <param name="values">The collection of values</param>
        public static void AddArrayParameters<TParam, TValue>(this ICollection<TParam> parameterCollection, string name,
            IEnumerable<TValue> values) where TParam: IDataParameter, new()
        {
            var paramName = name.StartsWith("@") ? name : "@" + name;
            var number = 0;
            foreach (var value in values)
            {
                var parameter = new TParam
                {
                    ParameterName = paramName + number++,
                    Value = value
                };
                parameterCollection.Add(parameter);
            }
        }

        public static void AddArrayParameters<TParam, TValue>(this ICollection<TParam> parameterCollection,
            ref StringBuilder queryBuilder, string parameterName, IEnumerable<TValue> values)
            where TParam : IDataParameter, new()
        {
            var normalizedParamName = parameterName.StartsWith("@") ? parameterName : "@" + parameterName;
            var names = values.Select((value, i) =>
            {
                var name = normalizedParamName + i;
                var param = new TParam
                {
                    ParameterName = name,
                    Value = value
                };
                parameterCollection.Add(param);
                return name;
            });

            queryBuilder = queryBuilder.ReplaceIgnoreCase(parameterName, string.Join(", ", names));
        }

        public static void AddWithValue<TParam>(this ICollection<TParam> parameterCollection, string parameterName,
            object parameterValue, DbType? dbType = null) where TParam: IDataParameter, new()
        {
            var parameter = new TParam();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            if (dbType.HasValue)
                parameter.DbType = dbType.Value;

            parameterCollection.Add(parameter);
        }

        public static void AddWithValue<TParam>(this ICollection<TParam> parameterCollection, string parameterName,
            object parameterValue, DbType dbType, int size, byte? precision = null) where TParam : IDbDataParameter, new()
        {
            var parameter = new TParam();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            parameter.DbType = dbType;
            parameter.Size = size;
            if (precision.HasValue)
                parameter.Precision = precision.Value;

            parameterCollection.Add(parameter);
        }

        /// <summary>
        /// Adds SQL Parameter as Table type. Useful to pass array to stored procedures
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd"></param>
        /// <param name="name"></param>
        /// <param name="values"></param>
        /// <remarks>Source: https://stackoverflow.com/questions/11102358/how-to-pass-an-array-into-a-sql-server-stored-procedure
        /// </remarks>
        public static void AddArrayParametersAsDataTable<T>(this SqlCommand cmd, string name, IEnumerable<T> values)
        {
            var dt = new DataTable();
            dt.Columns.Add("Item");
            foreach (var item in values)
            {
                var row = dt.NewRow();
                row[0] = item;
            }

            var parameter = new SqlParameter();
            parameter.ParameterName = name;
            parameter.SqlDbType = SqlDbType.Structured;
            parameter.Value = dt;

            cmd.Parameters.Add(parameter);
        }

        #endregion

        /// <summary>
        /// Executes SQL query in SqlCommand object and returns the enumerator of results
        /// </summary>
        /// <typeparam name="TResult">The type of entity </typeparam>
        /// <param name="command"></param>
        /// <param name="recordToObjectFunc"></param>
        /// <param name="connectionString"></param>
        /// <returns><see cref="IEnumerable{T}"/> instance of TResult</returns>
        public static IEnumerable<TResult> ExecuteAndEnumerate<TResult>(this SqlCommand command,
            Func<IDataRecord, TResult> recordToObjectFunc, string connectionString)
        {
            if(command == null)
                throw new ArgumentNullException("command");
            if(recordToObjectFunc == null)
                throw new ArgumentNullException("recordToObjectFunc");
            if(connectionString == null)
                throw new ArgumentNullException("connectionString");

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (command)
                {
                    command.Connection = conn;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return recordToObjectFunc(reader);
                        }
                    }
                }
            }
        }

        public static IEnumerable<TResult> ExecutePreparedAndEnumerate<TResult>(this SqlCommand command,
            Func<IDataRecord, TResult> recordToObjectFunc, string connectionString)
        {
            if (command == null)
                throw new ArgumentNullException("command");
            if (recordToObjectFunc == null)
                throw new ArgumentNullException("recordToObjectFunc");
            if (connectionString == null)
                throw new ArgumentNullException("connectionString");

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (command)
                {
                    command.Connection = conn;
                    command.Prepare();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return recordToObjectFunc(reader);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Executes SQL query in SqlCommand object and returns the list of results
        /// </summary>
        /// <typeparam name="TResult">The type of entity </typeparam>
        /// <param name="command"></param>
        /// <param name="recordToObjectFunc"></param>
        /// <param name="connectionString"></param>
        /// <returns><see cref="IList{T}"/> instance of TResult</returns>
        [Obsolete("Use ExecuteAndEnumerate instead")]
        public static IList<TResult> ExecuteAndReturnList<TResult>(this SqlCommand command,
            Func<IDataRecord, TResult> recordToObjectFunc, string connectionString)
        {
            if (command == null)
                throw new ArgumentNullException("command");
            if (recordToObjectFunc == null)
                throw new ArgumentNullException("recordToObjectFunc");
            if (connectionString == null)
                throw new ArgumentNullException("connectionString");

            IList<TResult> resultList = new List<TResult>();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (command)
                {
                    command.Connection = conn;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultList.Add(recordToObjectFunc(reader));
                        }
                    }
                }
            }

            return resultList;
        }

        /// <summary>
        /// Executes SQL query in SqlCommand object and returns the list of results
        /// </summary>
        /// <typeparam name="TResult">The type of entity </typeparam>
        /// <param name="command"></param>
        /// <param name="recordToObjectFunc"></param>
        /// <param name="connectionString"></param>
        /// <returns><see cref="ICollection{T}"/> instance of TResult</returns>
        public static ICollection<TResult> ExecuteAndReturnColection<TResult>(this SqlCommand command,
            Func<IDataRecord, TResult> recordToObjectFunc, string connectionString)
        {
            if (command == null)
                throw new ArgumentNullException("command");
            if (recordToObjectFunc == null)
                throw new ArgumentNullException("recordToObjectFunc");
            if (connectionString == null)
                throw new ArgumentNullException("connectionString");

            ICollection<TResult> resultList = new List<TResult>();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (command)
                {
                    command.Connection = conn;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultList.Add(recordToObjectFunc(reader));
                        }
                    }
                }
            }

            return resultList;
        }

        /// <summary>
        /// Executes SQL query in SqlCommand object and returns the collection of results
        /// </summary>
        /// <typeparam name="TResult">The type of entity </typeparam>
        /// <param name="command"></param>
        /// <param name="recordToObjectFunc"></param>
        /// <param name="connectionString"></param>
        /// <param name="containerType">The type of container, it must be an instance of <see cref="ICollection{T}"/></param>
        /// <returns><see cref="ICollection{T}"/> instance of TResult</returns>
        [Obsolete("Use ExecuteAndEnumerate instead")]
        public static ICollection<TResult> ExecuteAndReturnCollection<TResult>(this SqlCommand command,
            Func<IDataRecord, TResult> recordToObjectFunc, string connectionString, Type containerType)
        {
            if (command == null)
                throw new ArgumentNullException("command");
            if (recordToObjectFunc == null)
                throw new ArgumentNullException("recordToObjectFunc");
            if (connectionString == null)
                throw new ArgumentNullException("connectionString");
            if(containerType == null)
                throw new ArgumentNullException("containerType");

            if (!typeof(ICollection<TResult>).IsAssignableFrom(containerType))
                throw new ArgumentException("The given type must be an instance of ICollection<T>", "containerType");

            if(containerType.IsAbstract || containerType.IsInterface)
                throw new ArgumentException("The type must be a concrete type", "containerType");

            var resultCollection = (ICollection<TResult>)Activator.CreateInstance(containerType);
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (command)
                {
                    command.Connection = conn;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultCollection.Add(recordToObjectFunc(reader));
                        }
                    }
                }
            }

            return resultCollection;
        }

        /// <summary>
        /// Executes SQL Select query in command object and returns <see cref="IDictionary{TKey,TValue}"/> with specified
        /// key selector function with specified record to object transformation function
        /// </summary>
        /// <typeparam name="TKey">The type of key selector</typeparam>
        /// <typeparam name="TResult">The object type of dictionary entry to return</typeparam>
        /// <param name="command">The SQL Select command object to execute</param>
        /// <param name="recordToObjectFunc">The function to transform <see cref="IDataRecord"/> object to <see cref="TResult"/></param>
        /// <param name="keySelectorFunc">The function to specify the key</param>
        /// <param name="connectionString">The SQL Server connection string</param>
        /// <param name="comparer"></param>
        /// <returns><see cref="IDictionary{TKey,TValue}"/> instance with result with specified key selector</returns>
        public static IDictionary<TKey, TResult> ExecuteAndReturnDictionary<TKey, TResult>(this SqlCommand command,
            Func<IDataRecord, TResult> recordToObjectFunc, Func<TResult, TKey> keySelectorFunc, string connectionString, IEqualityComparer<TKey> comparer = null)
        {
            return command.ExecuteAndEnumerate(recordToObjectFunc, connectionString).ToDictionary(keySelectorFunc, comparer);
            //if (command == null)
            //    throw new ArgumentNullException("command");
            //if (recordToObjectFunc == null)
            //    throw new ArgumentNullException("recordToObjectFunc");
            //if (connectionString == null)
            //    throw new ArgumentNullException("connectionString");
            //if(keySelectorFunc == null)
            //    throw new ArgumentNullException("keySelectorFunc");

            //var dictionary = new Dictionary<TKey, TResult>();
            //using (var conn = new SqlConnection(connectionString))
            //{
            //    conn.Open();
            //    using (command)
            //    {
            //        command.Connection = conn;
            //        using (var reader = command.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                var resultObject = recordToObjectFunc(reader);
            //                dictionary.Add(
            //                    keySelectorFunc(resultObject),
            //                    resultObject
            //                );
            //            } 
            //        }
            //    }
            //}

            //return dictionary;
        }

        /// <summary>
        /// Executes an <see cref="SqlCommand"/> object and returns an object as processed by recordToObjectFunc
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="command">The <see cref="SqlCommand"/> object to execute</param>
        /// <param name="recordToObjectFunc"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static TResult ExecuteReturnObject<TResult>(this SqlCommand command,
            Func<IDataRecord, TResult> recordToObjectFunc, string connectionString)
        {
            return command.ExecuteAndEnumerate(recordToObjectFunc, connectionString).SingleOrDefault();
            //if (command == null)
            //    throw new ArgumentNullException("command");
            //if (recordToObjectFunc == null)
            //    throw new ArgumentNullException("recordToObjectFunc");
            //if (connectionString == null)
            //    throw new ArgumentNullException("connectionString");

            //using (var conn = new SqlConnection(connectionString))
            //{
            //    conn.Open();
            //    using (command)
            //    {
            //        command.Connection = conn;
            //        using (var reader = command.ExecuteReader())
            //        {
            //            if (reader.Read())
            //                return recordToObjectFunc(reader);
            //        }
            //    }
            //}
            //return default(TResult);
        }

        /// <summary>
        /// Prints SQL query as well as its commands for debugging purpose
        /// </summary>
        /// <param name="cmd">The <see cref="IDbCommand"/> instance to print it's <see cref="IDataParameter"/></param>
        /// <returns>The Sql Command as well as its parameters</returns>
        public static string DebugPrint(this IDbCommand cmd)
        {
            if (cmd == null)
                throw new ArgumentNullException("cmd");

            var sb = new StringBuilder();
            sb.AppendLine(cmd.CommandText);
            sb.AppendLine();
            foreach (IDataParameter param in cmd.Parameters)
            {
                sb.Append(param.ParameterName);
                sb.Append(" = \"");
                sb.Append(param.Value);
                sb.AppendLine("\"");
            }
            return sb.ToString();
        }

        public static int ExecuteNonQueryThroughConnection(this SqlCommand command, string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (command)
                {
                    command.Connection = conn;
                    return command.ExecuteNonQuery();
                }
            }
        }

        public static int ExecutePreparedNonQueryThroughConnection(this SqlCommand command, string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (command)
                {
                    command.Connection = conn;
                    command.Prepare();
                    return command.ExecuteNonQuery();
                }
            }
        }

        public static TResult ExecuteScalarThroughConnection<TResult>(this SqlCommand command, string connectionString) //where TResult: IConvertible
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (command)
                {
                    command.Connection = conn;
                    var result = command.ExecuteScalar();

                    return (TResult) result;
                    //var converter = TypeDescriptor.GetConverter(typeof(TResult));
                    //return (TResult) converter.ConvertFrom(result);
                }
            }
        }

        public static TResult ExecuteScalarThroughConnection<TResult>(this SqlCommand command, string connectionString,
            Func<object, TResult> conversionFunc)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (command)
                {
                    command.Connection = conn;
                    return conversionFunc(command.ExecuteScalar());
                }
            }
        }

        public static StringBuilder AppendSingleOrderByStatement(this StringBuilder stringBuilder, string columnName,
            SortOrder sortOrder = SortOrder.Unspecified)
        {
            if (string.IsNullOrWhiteSpace(columnName))
                return stringBuilder;

            stringBuilder.AppendLine(" ORDER BY ");
            stringBuilder.Append(columnName);
            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    stringBuilder.AppendLine(" ASC ");
                    break;
                case SortOrder.Descending:
                    stringBuilder.AppendLine(" DESC ");
                    break;
                case SortOrder.Unspecified:
                    stringBuilder.AppendLine();
                    break;

                default:
                    throw new ArgumentOutOfRangeException("sortOrder", "Invalid sort order value!");
            }

            return stringBuilder;
        }

        public static string GetSingleOrderByStatement(string columnName, SortOrder sortOrder = SortOrder.Unspecified)
        {
            return new StringBuilder().AppendSingleOrderByStatement(columnName, sortOrder).ToString();
        }
    }

    public static class SqlConnectionExtender
    {
        /// <summary>
        /// Instantiates new <see cref="IDbConnection"/> instance and opens it upon creation
        /// </summary>
        /// <typeparam name="TDbConnection"></typeparam>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static TDbConnection CreateAndOpenDbConnection<TDbConnection>(string connectionString)
            where TDbConnection : IDbConnection, new()
        {
            if (connectionString == null) throw new ArgumentNullException("connectionString");

            var conn = new TDbConnection { ConnectionString = connectionString };

            conn.Open();
            return conn;
        }

        /// <summary>
        /// Instantiates new <see cref="SqlConnection"/> and opens it upon creation
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static SqlConnection CreateAndOpenSqlConnection(string connectionString)
        {
            return CreateAndOpenDbConnection<SqlConnection>(connectionString);
        }
    }

    public static class SqlParameterExtender
    {
        public static SqlParameter CreateDomKodeIdParameter(string paramName, string value)
        {
            return new SqlParameter(paramName, SqlDbType.NVarChar, 200) { Value = value };
        }

        public static SqlParameter CreateDomDescriptionParameter(string paramName, string value)
        {
            return new SqlParameter(paramName, SqlDbType.NVarChar, 400) { Value = value };
        }

        public static SqlParameter CreateDomMyBooleanParameter(string paramName, bool value)
        {
            return new SqlParameter(paramName, SqlDbType.VarChar, 1) { Value = value ? "T" : "F" };
        }

        public static SqlParameter CreateDomMyBooleanParameter(string paramName, bool? value)
        {
            return new SqlParameter(paramName, SqlDbType.VarChar, 1)
            {
                Value = value != null
                    ? value.Value ? "T" : "F"
                    : (object)DBNull.Value
            };
        }

        public static SqlParameter AddDomKodeIdParameter(this SqlParameterCollection parameterCollection, string paramName, string value)
        {
            return parameterCollection.Add(CreateDomKodeIdParameter(paramName, value));
        }

        public static SqlParameter AddDomDescriptionParameter(this SqlParameterCollection parameterCollection, string paramName, string value)
        {
            return parameterCollection.Add(CreateDomDescriptionParameter(paramName, value));
        }

        public static SqlParameter AddDomMyBooleanParameter(this SqlParameterCollection parameterCollection,
            string paramName, bool value)
        {
            return parameterCollection.Add(CreateDomMyBooleanParameter(paramName, value));
        }

        public static SqlParameter AddDomMyBooleanParameter(this SqlParameterCollection parameterCollection,
            string paramName, bool? value)
        {
            return parameterCollection.Add(CreateDomMyBooleanParameter(paramName, value));
        }
    }
}
