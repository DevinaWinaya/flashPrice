﻿using othersFx;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flashPriceFx.Product
{
    public class DBProduct
    {
        const string defaultFields = @"
        select *
        from Product p with(nolock)  
        where 1=1";

        #region getContent

        #region getPinjaman
        public static BOProduct getPinjaman(String txnID)
        {
            string xSQL = defaultFields + " and p.pinjamanID='" + DBHelper.cleanParam(txnID) + "' ";

            return getContentQR(xSQL);

        }
        #endregion


        private static BOProduct getContentQR(string xSQL)
        {
            BOProduct xPinjaman = null;
            using (SqlConnection myConnection = new SqlConnection(DBUtil.conStringdbKopkari))
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
                            xPinjaman = new BOProduct();
                            while (myReader.Read())
                            {
                                xPinjaman = fillDataRecord(myReader);
                            }
                        }
                        myReader.Close();
                    }
                    myComm.Dispose();
                }
            }

            return xPinjaman;
        }
        #endregion

        #region fillDataRecord

        public static BOProduct fillDataRecord(IDataRecord mD)
        {
            BOProduct xTxn = new BOProduct();

            xTxn.productID = (!mD.IsDBNull(mD.GetOrdinal("productID"))) ? mD.GetString(mD.GetOrdinal("productID")) : null;
            xTxn.productName = (!mD.IsDBNull(mD.GetOrdinal("productName"))) ? mD.GetString(mD.GetOrdinal("productName")) : null;
            xTxn.productCategory = (!mD.IsDBNull(mD.GetOrdinal("productCategory"))) ? mD.GetString(mD.GetOrdinal("productCategory")) : null;
            xTxn.productPrice = (!mD.IsDBNull(mD.GetOrdinal("productPrice"))) ? mD.GetInt32(mD.GetOrdinal("productPrice")) : 0;
            xTxn.productImageContent = (!mD.IsDBNull(mD.GetOrdinal("productImageContent"))) ? (byte[])((SqlDataReader)mD).GetSqlBinary(mD.GetOrdinal("productImageContent")) : null;
            xTxn.entryDate = DBHelper.getDBDateTime(mD, "entryDate");     
            xTxn.lastUpdate = DBHelper.getDBDateTime(mD, "lastUpdate");
      
            return xTxn;
        }

        #endregion

    }
}
