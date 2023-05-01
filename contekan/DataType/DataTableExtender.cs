using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Reflection;
using HCFx.Extender.DataType;
using System.IO;
using HCFx.Other;

using System.Collections;

namespace System.Data
{
     public static partial class DataTableExtender
    {
  

     ///###############################################################
        /// <summary>
        /// Convert a List to a DataTable.
        /// </summary>
        /// <remarks>
        /// Based on MIT-licensed code presented at http://www.chinhdo.com/20090402/convert-list-to-datatable/ as "ToDataTable"
        /// <para/>Code modifications made by Nick Campbell.
        /// <para/>Source code provided on this web site (chinhdo.com) is under the MIT license.
        /// <para/>Copyright © 2010 Chinh Do
        /// <para/>Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
        /// <para/>The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
        /// <para/>THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
        /// <para/>(As per http://www.chinhdo.com/20080825/transactional-file-manager/)
        /// </remarks>
        /// <typeparam name="T">Type representing the type to convert.</typeparam>
        /// <param name="l_oItems">List of requested type representing the values to convert.</param>
        /// <returns></returns>
        ///###############################################################
        /// <LastUpdated>February 15, 2010</LastUpdated>
        public static DataTable ToDataTable<T>(this IEnumerable<T> l_oItems, bool convertEnumToUnderlyingType = false)
        {
            DataTable oReturn = new DataTable(typeof(T).Name);
            object[] a_oValues;
            int i;

            //#### Collect the a_oProperties for the passed T
            PropertyInfo[] a_oProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            //#### Traverse each oProperty, .Add'ing each .Name/.BaseType into our oReturn value
            //####     NOTE: The call to .BaseType is required as DataTables/DataSets do not support nullable types, so it's non-nullable counterpart Type is required in the .Column definition
            foreach (PropertyInfo oProperty in a_oProperties)
            {
                oReturn.Columns.Add(oProperty.Name, BaseType(oProperty.PropertyType, convertEnumToUnderlyingType));
            }

            //#### Traverse the l_oItems
            foreach (T oItem in l_oItems)
            {
                //#### Collect the a_oValues for this loop
                a_oValues = new object[a_oProperties.Length];

                //#### Traverse the a_oProperties, populating each a_oValues as we go
                for (i = 0; i < a_oProperties.Length; i++)
                {
                    a_oValues[i] = a_oProperties[i].GetValue(oItem, null);
                }

                //#### .Add the .Row that represents the current a_oValues into our oReturn value
                oReturn.Rows.Add(a_oValues);
            }

            //#### Return the above determined oReturn value to the caller
            return oReturn;
        }

        ///###############################################################
        /// <summary>
        /// Returns the underlying/base type of nullable types.
        /// </summary>
        /// <remarks>
        /// Based on MIT-licensed code presented at http://www.chinhdo.com/20090402/convert-list-to-datatable/ as "GetCoreType"
        /// <para/>Code modifications made by Nick Campbell.
        /// <para/>Source code provided on this web site (chinhdo.com) is under the MIT license.
        /// <para/>Copyright © 2010 Chinh Do
        /// <para/>Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
        /// <para/>The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
        /// <para/>THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
        /// <para/>(As per http://www.chinhdo.com/20080825/transactional-file-manager/)
        /// </remarks>
        /// <param name="oType">Type representing the type to query.</param>
        /// <returns>Type representing the underlying/base type.</returns>
        ///###############################################################
        /// <LastUpdated>February 15, 2010</LastUpdated>
        public static Type BaseType(Type oType, bool convertEnumToUnderlyingType = false)
        {
            //#### If the passed oType is valid, .IsValueType and is logicially nullable, .Get(its)UnderlyingType
            if (oType != null && oType.IsValueType &&
                oType.IsGenericType && oType.GetGenericTypeDefinition() == typeof(Nullable<>)
            )
            {
                var underlyingType = Nullable.GetUnderlyingType(oType);
                if (convertEnumToUnderlyingType && underlyingType.IsEnum)
                    return Enum.GetUnderlyingType(underlyingType);
                return underlyingType;
            }
            //#### Else the passed oType was null or was not logicially nullable, so simply return the passed oType
            else
            {
                if (convertEnumToUnderlyingType && oType.IsEnum)
                    return Enum.GetUnderlyingType(oType);

                return oType;
            }
        }

        public static void SetColumnsOrder(this DataTable dataTable, params string[] columnNames)
        {
            columnNames.ForEach((name, index) => dataTable.Columns[name].SetOrdinal(index));
        }

        /// <summary>
        /// Converts column data type in <paramref name="dataTable"/> to specified destination type by relying on <see cref="IConvertible"/> capability.
        /// </summary>
        /// <typeparam name="TDestType">The destination type to convert to</typeparam>
        /// <param name="dataTable">The <see cref="DataTable"/> whose <see cref="DataColumn"/> wanted to convert to destination type</param>
        /// <param name="columnName">The column name to convert</param>
        /// <exception cref="InvalidCastException">When the source type is not an <see cref="IConvertible"/></exception>
        public static void ChangeColumnType<TDestType>(this DataTable dataTable, string columnName)
        {
            if(dataTable == null) throw new ArgumentNullException("dataTable");
            if(columnName == null) throw new ArgumentNullException("columnName");

            var originalColumn = dataTable.Columns[columnName];
            var ordinal = originalColumn.Ordinal;
            var originalType = originalColumn.DataType;
            if (!originalType.GetInterfaces().Contains(typeof(IConvertible)))
                throw new InvalidCastException(
                    "Unable to convert from " + originalType.FullName + " to " + typeof(TDestType).FullName +
                    ". Ensure the original type implements " + typeof(IConvertible).FullName + ".");

            var newColumn = new DataColumn(columnName + "_new", typeof(TDestType));
            dataTable.Columns.Add(newColumn);
            newColumn.SetOrdinal(ordinal);

            foreach (DataRow row in dataTable.Rows)
            {
                row[newColumn] = Convert.ChangeType(row[originalColumn], typeof(TDestType));
            }

            dataTable.Columns.Remove(originalColumn);
            dataTable.Columns[newColumn.ColumnName].ColumnName = columnName;
        }

        /// <summary>
        /// Converts column data type in <paramref name="dataTable"/> to specified destination type by applying <paramref name="transformationFunc"/>
        /// </summary>
        /// <typeparam name="TSourceType">The source type of column</typeparam>
        /// <typeparam name="TDestType">The destination type to convert to</typeparam>
        /// <param name="dataTable">The <see cref="DataTable"/> whose <see cref="DataColumn"/> wanted to convert to destination type</param>
        /// <param name="columnName">The column name to convert</param>
        /// <param name="transformationFunc">The function to perform data transformation</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void ChangeColumnType<TSourceType, TDestType>(this DataTable dataTable, string columnName,
            Func<TSourceType, TDestType> transformationFunc)
        {
            if (dataTable == null) throw new ArgumentNullException("dataTable");
            if (columnName == null) throw new ArgumentNullException("columnName");
            if(transformationFunc == null) throw new ArgumentNullException("transformationFunc");

            var originalColumn = dataTable.Columns[columnName];
            var originalType = originalColumn.DataType;
            if(!typeof(TSourceType).IsAssignableFrom(originalType))
                throw new InvalidCastException("The source DataColumn type is not assignable to " + typeof(TSourceType).FullName);
            
            int ordinal = originalColumn.Ordinal;
            var newColumn = new DataColumn(columnName + "_new", typeof(TDestType));
            
            dataTable.Columns.Add(newColumn);
            newColumn.SetOrdinal(ordinal);

            foreach (DataRow row in dataTable.Rows)
            {
                var originalValue = !row.IsNull(originalColumn) ? (TSourceType) row[originalColumn] : default(TSourceType);
                row[newColumn] = transformationFunc(originalValue);
            }

            dataTable.Columns.Remove(originalColumn);
            dataTable.Columns[newColumn.ColumnName].ColumnName = columnName;
        }

        public static void ChangeColumnType<TSourceType, TDestType>(this DataTable dataTable, string columnName,
            Func<TSourceType?, TDestType> transformationFunc) where TSourceType: struct
        {
            if (dataTable == null) throw new ArgumentNullException("dataTable");
            if (columnName == null) throw new ArgumentNullException("columnName");
            if (transformationFunc == null) throw new ArgumentNullException("transformationFunc");

            var originalColumn = dataTable.Columns[columnName];
            var originalType = originalColumn.DataType;
            if (!typeof(TSourceType).IsAssignableFrom(originalType))
                throw new InvalidCastException("The source DataColumn type is not assignable to " + typeof(TSourceType).FullName);

            if (!originalColumn.AllowDBNull)
                throw new InvalidOperationException("The original column is not nullable");

            int ordinal = originalColumn.Ordinal;
            var newColumn = new DataColumn(columnName + "_new", typeof(TDestType));
            dataTable.Columns.Add(newColumn);
            newColumn.SetOrdinal(ordinal);
            foreach (DataRow row in dataTable.Rows)
            {
                var currentValue = row.IsNull(originalColumn) ? null :  new TSourceType?((TSourceType) row[originalColumn]);
                row[newColumn] = transformationFunc(currentValue);
            }

            dataTable.Columns.Remove(originalColumn);
            dataTable.Columns[newColumn.ColumnName].ColumnName = columnName;
        }

        public static DataTable leftJoinDataTable(DataTable LeftTable, DataTable RightTable,
          String LeftPrimaryColumn, String RightPrimaryColumn)
        {
            DataTable myDataTable = new DataTable();

            //add left table columns 
            DataColumn[] dcLeftTableColumns = new DataColumn[LeftTable.Columns.Count];
            LeftTable.Columns.CopyTo(dcLeftTableColumns, 0);

            //define column kiri
            foreach (DataColumn LeftTableColumn in dcLeftTableColumns)
            {
                if (!myDataTable.Columns.Contains(LeftTableColumn.ToString()))
                {
                    myDataTable.Columns.Add(LeftTableColumn.ColumnName, LeftTableColumn.DataType);
                }
            }

            //now add right table columns 
            DataColumn[] dcRightTableColumns = new DataColumn[RightTable.Columns.Count];
            RightTable.Columns.CopyTo(dcRightTableColumns, 0);

            //define column kanan
            foreach (DataColumn RightTableColumn in dcRightTableColumns)
            {
                if (!myDataTable.Columns.Contains(RightTableColumn.ToString()))
                {
                    if (RightTableColumn.ToString() != RightPrimaryColumn)
                    {
                        //myDataTable.Columns.Add(RightTableColumn.ToString());
                        myDataTable.Columns.Add(RightTableColumn.ColumnName, RightTableColumn.DataType);
                    }
                }
            }

            //add left-table data to mytable 
            foreach (DataRow LeftTableDataRows in LeftTable.Rows)
            {
                myDataTable.ImportRow(LeftTableDataRows);
            }

            ArrayList var = new ArrayList(); //this variable holds the id's which have joined 

            ArrayList LeftTableIDs = new ArrayList();
            LeftTableIDs = DataTableToArrayList(0, LeftTable);

            //import righttable which having not equal Id's with lefttable 
            foreach (DataRow rightTableDataRows in RightTable.Rows)
            {
                if (LeftTableIDs.Contains(rightTableDataRows[0]))
                {
                    string wherecondition = "[" + myDataTable.Columns[0].ColumnName + "]='"
                            + rightTableDataRows[0].ToString() + "'";
                    DataRow[] dr = myDataTable.Select(wherecondition);
                    int iIndex = myDataTable.Rows.IndexOf(dr[0]);

                    foreach (DataColumn dc in RightTable.Columns)
                    {
                        if (dc.Ordinal != 0)
                        {
                            //myDataTable.Rows[iIndex][dc.ColumnName.ToString().Trim()] =
                            //        rightTableDataRows[dc.ColumnName.ToString().Trim()].ToString();

                            myDataTable.Rows[iIndex][dc.ColumnName.ToString().Trim()] =
                                    rightTableDataRows[dc.ColumnName.ToString().Trim()];
                        }
                    }
                }
                else
                {
                    int count = myDataTable.Rows.Count;
                    DataRow row = myDataTable.NewRow();
                    row[0] = rightTableDataRows[0].ToString();
                    myDataTable.Rows.Add(row);
                    foreach (DataColumn dc in RightTable.Columns)
                    {
                        if (dc.Ordinal != 0)
                            myDataTable.Rows[count][dc.ColumnName.ToString().Trim()] =
                                rightTableDataRows[dc.ColumnName.ToString().Trim()];
                    }
                }
            }

            return myDataTable;
        }

        private static ArrayList DataTableToArrayList(int ColumnIndex, DataTable dataTable)
        {
            ArrayList output = new ArrayList();

            foreach (DataRow row in dataTable.Rows)
                output.Add(row[ColumnIndex]);

            return output;
        }

        public static DataTable readCSVtoDataTable(string filePath, DataTable xMatrix) //xMatrix = datab yang mau diisi
        {
            string Fulltext;

            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    Fulltext = sr.ReadToEnd().ToString(); //read full file text  
                    string[] rows = Fulltext.Split('\n'); //split full file text into rows  
                    for (int i = 0; i < rows.Count() - 1; i++)
                    {
                        string[] rowValues = rows[i].Split(','); //split each row with comma to get individual values  
                        {
                            if (i == 0)
                            {
                                for (int j = 0; j < rowValues.Count(); j++)
                                {
                                    xMatrix.Columns.Add(rowValues[j]); //add headers  
                                }
                            }
                            else
                            {
                                DataRow dr = xMatrix.NewRow();
                                for (int k = 0; k < rowValues.Count(); k++)
                                {
                                    dr[k] = rowValues[k].ToString();
                                }
                                xMatrix.Rows.Add(dr); //add other rows  
                            }
                        }
                    }
                }
            }

            return xMatrix;
        }

        public static DataTable gridViewToDataTable(GridView dtg)
        {
            DataTable dt = new DataTable();

            // add the columns to the datatable            
            if (dtg.HeaderRow != null)
            {

                for (int i = 0; i < dtg.HeaderRow.Cells.Count; i++)
                {
                    dt.Columns.Add(dtg.HeaderRow.Cells[i].Text);
                }
            }

            //  add each of the data rows to the table
            foreach (GridViewRow row in dtg.Rows)
            {
                DataRow dr;
                dr = dt.NewRow();

                for (int i = 0; i < row.Cells.Count; i++)
                {
                    dr[i] = row.Cells[i].Text.Replace(" ", "");
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
