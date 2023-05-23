using flashPriceFX;
using HCPLUSFx.DAL;
using othersFx;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flashPriceFx.User
{
    public class DBUser
    {
        const string defaultFields = @"
        select *
        from userFlashPrice u with(nolock)
        where 1=1";

        #region doLogin
        public static BOUser doLogin(String username)
        {
            string xSQL = "";
            if (username != "")
            {
                xSQL = defaultFields;
                xSQL += " and u.username = " + "'" + username + "'";
                return getContentQR(xSQL);

            }
            else
            {
                return null;
            }

        }
        #endregion

        #region getContentQR
        private static BOUser getContentQR(string xSQL)
        {
            BOUser xUser = null;
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

                    using (SqlDataReader myReader = myComm.ExecuteReader())
                    {
                        if (myReader.HasRows)
                        {
                            xUser = new BOUser();
                            while (myReader.Read())
                            {
                                xUser = fillDataRecord(myReader);
                            }
                        }
                        myReader.Close();
                    }
                    myComm.Dispose();
                }
            }

            return xUser;
        }
        #endregion

        #region fillDataRecord

        #region fillDataRecord original
        public static BOUser fillDataRecord(IDataRecord mD)
        {
            BOUser xBO = new BOUser();
            xBO.userID = (!mD.IsDBNull(mD.GetOrdinal("userID"))) ? mD.GetString(mD.GetOrdinal("userID")) : null;
            xBO.userName = (!mD.IsDBNull(mD.GetOrdinal("userName"))) ? mD.GetString(mD.GetOrdinal("userName")) : null;
            xBO.userPassword = (!mD.IsDBNull(mD.GetOrdinal("userPassword"))) ? mD.GetString(mD.GetOrdinal("userPassword")) : null;

            xBO.entryDate = DBHelper.getDBDateTime(mD, "entryDate");
            xBO.lastUpdate = DBHelper.getDBDateTime(mD, "lastUpdate");

            return xBO;
        }
        #endregion

        #endregion
    }
}
