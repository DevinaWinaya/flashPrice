using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using othersFx.Crypto;

namespace HCPLUSFx.DAL
{
    public static class DBUtil
    {

        public enum databaseServer
        {
            dbflashPrice
        };

        static string getConnectionString(databaseServer dbServer)
        {
            string conStr = "";

            switch (dbServer)
            {
                case databaseServer.dbflashPrice:
                    conStr = conStringdbflashPrice;
                    break;
            }
            return conStr;
        }

        #region connection string


        public static string conStringdbflashPrice
        {
            get
            {
                string xA = "";

                xA = "Data Source=" + ConfigurationManager.AppSettings["ServerflashPrice"];
                xA += ";Initial Catalog=" + ConfigurationManager.AppSettings["DatabaseflashPrice"];
                xA += ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["DBUserNameflashPrice"];
                xA += ";Password=" + ConfigurationManager.AppSettings["DBPasswordflashPrice"];
                xA += ";Application Name=" + ConfigurationManager.AppSettings["DBAppNameflashPrice"];
                xA += ";TrustServerCertificate=True";
                xA += ";MultipleActiveResultSets=true; ";
                return xA;
                //return ConfigurationManager.ConnectionStrings["HCPLUSConnectionString"].ConnectionString;                
            }
        }

        public static string conStringMailer
        {
            get
            {

                string xA = "";
                xA = "Data Source=" + ConfigurationManager.AppSettings["ServerMail"];
                xA += ";Initial Catalog=" + ConfigurationManager.AppSettings["DatabaseMail"];
                xA += ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["DBUserNameMail"];
                xA += ";Password=" + CryptoAES.DecryptStringAES(ConfigurationManager.AppSettings["DBPasswordMail"], "dbpass");
                xA += ";Application Name=" + ConfigurationManager.AppSettings["DBAppNameflashPrice"];
                xA += ";MultipleActiveResultSets=true; ";
                return xA;
            }

        }


        public static string conStringMailerExternal
        {
            get
            {

                string xA = "";
                xA = "Data Source=" + ConfigurationManager.AppSettings["ServerMailExternal"];
                xA += ";Initial Catalog=" + ConfigurationManager.AppSettings["DatabaseMailExternal"];
                xA += ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["DBUserNameMailExternal"];
                xA += ";Password=" + CryptoAES.DecryptStringAES(ConfigurationManager.AppSettings["DBPasswordMailExternal"], "dbpass");
                xA += ";Application Name=" + ConfigurationManager.AppSettings["DBAppNameExternal"];
                xA += ";MultipleActiveResultSets=true; ";
                return xA;
            }

        }

        #endregion

        #region exec scalar (untuk execute query count, sum, min, max, dll)
        public static Object execScalar(string xSQL)
        {
            object retVal = new object();
            try
            {
                using (SqlConnection myConnection = new SqlConnection(DBUtil.conStringdbflashPrice))
                {
                    DataSet dasetReturn = new DataSet();
                    myConnection.Open();
                    if (myConnection.State == ConnectionState.Open)
                    {
                        SqlCommand myComm = new SqlCommand();
                        myComm.Connection = myConnection;
                        myComm.CommandText = xSQL;
                        myComm.CommandType = CommandType.Text;
                        retVal = myComm.ExecuteScalar();
                        myComm.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                retVal = "EXCEPTION!DBUtil.execScalar " + ex.Message;
            }
            return retVal;
        }

        public static decimal execScalarDecimal(string xSQL)
        {
            decimal retVal = 0;

            object hasil = execScalar(xSQL);

            if (hasil is DBNull)
            {
                retVal = 0;
            }
            else if (hasil == null)
            {
                retVal = 0;
            }
            else
            {
                retVal = Convert.ToDecimal(hasil.ToString());
            }


            return retVal;
        }

        public static decimal execScalarInt(string xSQL)
        {
            decimal retVal = 0;

            object hasil = execScalar(xSQL);

            if (hasil is DBNull)
            {
                retVal = 0;
            }
            else if (hasil == null)
            {
                retVal = 0;
            }
            else
            {
                retVal = Convert.ToInt32(hasil.ToString());
            }


            return retVal;
        }
        #endregion

        #region fill data

        public static void fillDatabGeneric(DataTable datab, string xSQL) //untuk fill dataset nya saja. query nya ambil dari atas
        {
            using (SqlConnection myConnection = new SqlConnection(DBUtil.conStringdbflashPrice))
            {
                myConnection.Open();
                if (myConnection.State == ConnectionState.Open)
                {
                    SqlCommand myComm = new SqlCommand();
                    myComm.Connection = myConnection;
                    myComm.CommandText = xSQL;
                    myComm.CommandType = CommandType.Text;

                    myComm.Parameters.Clear();

                    SqlDataAdapter myReader = new SqlDataAdapter(myComm);
                    myReader.Fill(datab);


                    myReader.Dispose();
                    myComm.Dispose();
                }
            }
        }

        #region " dataset generic "

        public static DataSet fillDasetGeneric(string dasetName, string xSQL) //untuk fill dataset nya saja. query nya ambil dari atas
        {
            return fillDasetGeneric(dasetName, xSQL, 600, DBUtil.databaseServer.dbflashPrice);
        }

        public static DataSet fillDasetGeneric(string dasetName, string xSQL, int timeOutVal) //untuk fill dataset nya saja. query nya ambil dari atas
        {
            return fillDasetGeneric(dasetName, xSQL, timeOutVal, databaseServer.dbflashPrice);
        }


        public static DataSet fillDasetGeneric(string dasetName, string xSQL, int timeOutVal, databaseServer dbServer) //untuk fill dataset nya saja. query nya ambil dari atas
        {
            string conStr = getConnectionString(dbServer);



            using (SqlConnection myConnection = new SqlConnection(conStr))
            {
                DataSet dasetReturn = new DataSet();
                myConnection.Open();
                if (myConnection.State == ConnectionState.Open)
                {
                    SqlCommand myComm = new SqlCommand();
                    myComm.Connection = myConnection;
                    myComm.CommandText = xSQL;
                    myComm.CommandType = CommandType.Text;
                    myComm.CommandTimeout = timeOutVal;

                    myComm.Parameters.Clear();

                    SqlDataAdapter myReader = new SqlDataAdapter(myComm);
                    myReader.Fill(dasetReturn, dasetName);
                    myReader.Dispose();
                    myComm.Dispose();
                }
                return dasetReturn;
            }
        }

        #endregion

        #region " dataset SP "

        public static DataSet fillDasetSP(string dasetName, string xSQL, string[] paramName, object[] paramValue) //untuk fill dataset nya saja. query nya ambil dari atas
        {
            return fillDasetSP(dasetName, xSQL, paramName, paramValue, 600);
        }

        //ni yg bisa custom ke mana2 database nya
        public static DataSet fillDasetSP(string dasetName, string xSQL, string[] paramName, object[] paramValue, int timeOutVal, databaseServer dbServer) //untuk fill dataset nya saja. query nya ambil dari atas
        {
            string conStr = getConnectionString(dbServer);



            using (SqlConnection myConnection = new SqlConnection(conStr))
            {
                DataSet dasetReturn = new DataSet();
                myConnection.Open();
                if (myConnection.State == ConnectionState.Open)
                {
                    SqlCommand myComm = new SqlCommand();
                    myComm.Connection = myConnection;

                    myComm.CommandText = xSQL;
                    myComm.CommandType = CommandType.StoredProcedure;
                    myComm.CommandTimeout = timeOutVal;

                    myComm.Parameters.Clear();
                    for (int ix = 0; ix < paramName.Length; ix++)
                    {
                        myComm.Parameters.AddWithValue(paramName[ix], paramValue[ix]);
                    }
                    SqlDataAdapter myReader = new SqlDataAdapter(myComm);
                    myReader.Fill(dasetReturn, dasetName);
                    myReader.Dispose();
                    myComm.Dispose();
                }
                return dasetReturn;
            }
        }

        public static DataSet fillDasetSP(string dasetName, string xSQL, string[] paramName, object[] paramValue, int timeOutVal) //untuk fill dataset nya saja. query nya ambil dari atas
        {
            using (SqlConnection myConnection = new SqlConnection(DBUtil.conStringdbflashPrice))
            {
                DataSet dasetReturn = new DataSet();
                myConnection.Open();
                if (myConnection.State == ConnectionState.Open)
                {
                    SqlCommand myComm = new SqlCommand();
                    myComm.Connection = myConnection;
                    myComm.CommandText = xSQL;
                    myComm.CommandType = CommandType.StoredProcedure;
                    myComm.CommandTimeout = timeOutVal;

                    myComm.Parameters.Clear();
                    for (int ix = 0; ix < paramName.Length; ix++)
                    {
                        myComm.Parameters.AddWithValue(paramName[ix], paramValue[ix]);
                    }
                    SqlDataAdapter myReader = new SqlDataAdapter(myComm);
                    myReader.Fill(dasetReturn, dasetName);
                    myReader.Dispose();
                    myComm.Dispose();
                }
                return dasetReturn;
            }
        }

        #endregion


        #region " datatable generic "

        public static DataTable fillDatatableGeneric(string datatableName, string xSQL) //untuk fill dataset nya saja. query nya ambil dari atas
        {
            using (SqlConnection myConnection = new SqlConnection(DBUtil.conStringdbflashPrice))
            {
                DataTable dasetReturn = new DataTable(datatableName);
                myConnection.Open();
                if (myConnection.State == ConnectionState.Open)
                {
                    SqlCommand myComm = new SqlCommand();
                    myComm.Connection = myConnection;
                    myComm.CommandText = xSQL;
                    myComm.CommandType = CommandType.Text;

                    myComm.Parameters.Clear();

                    SqlDataAdapter myReader = new SqlDataAdapter(myComm);
                    myReader.Fill(dasetReturn);
                    myReader.Dispose();
                    myComm.Dispose();
                }
                return dasetReturn;
            }
        }

        public static DataTable fillDatabGeneric(string xSQL) //untuk fill dataset nya saja. query nya ambil dari atas
        {
            using (SqlConnection myConnection = new SqlConnection(DBUtil.conStringdbflashPrice))
            {
                DataTable dasetReturn = new DataTable();
                myConnection.Open();
                if (myConnection.State == ConnectionState.Open)
                {
                    SqlCommand myComm = new SqlCommand();
                    myComm.Connection = myConnection;
                    myComm.CommandText = xSQL;
                    myComm.CommandType = CommandType.Text;

                    myComm.Parameters.Clear();

                    SqlDataAdapter myReader = new SqlDataAdapter(myComm);
                    myReader.Fill(dasetReturn);
                    myReader.Dispose();
                    myComm.Dispose();
                }
                return dasetReturn;
            }
        }

        public static DataTable fillDatabGeneric(String spName, object[] paramssName, object[] paramValue)
        {
            try
            {
                DataTable result = new DataTable();
                SqlConnection myConnection = new SqlConnection(DBUtil.conStringdbflashPrice);
                SqlCommand cmd = new SqlCommand(spName, myConnection);
                for (int i = 0; i < paramssName.Count(); i++)
                {
                    cmd.Parameters.Add(paramssName[i].ToString(), SqlDbType.VarChar, 1000).Value = paramValue[i].ToString();
                }

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(result);

                return result;

            }
            catch (Exception ex)
            {
                return new DataTable();
            }
        }

        public static DataTable fillDatabGeneric(string xSQL, databaseServer dbServer)
        {
            return fillDatabGeneric(xSQL, int.MaxValue, dbServer);
        }

        public static DataTable fillDatabGeneric(string xSQL, int timeOutVal, databaseServer dbServer)
        {
            string conStr = getConnectionString(dbServer);


            using (SqlConnection myConnection = new SqlConnection(conStr))
            {
                DataTable dasetReturn = new DataTable();
                myConnection.Open();
                if (myConnection.State == ConnectionState.Open)
                {
                    SqlCommand myComm = new SqlCommand();
                    myComm.Connection = myConnection;
                    myComm.CommandTimeout = timeOutVal;
                    myComm.CommandText = xSQL;
                    myComm.CommandType = CommandType.Text;

                    myComm.Parameters.Clear();

                    SqlDataAdapter myReader = new SqlDataAdapter(myComm);
                    myReader.Fill(dasetReturn);
                    myReader.Dispose();
                    myComm.Dispose();
                }
                return dasetReturn;
            }
        }

        #endregion

        #endregion
    }
}



