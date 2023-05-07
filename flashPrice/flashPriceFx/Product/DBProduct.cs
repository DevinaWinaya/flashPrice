using HCPLUSFx.DAL;
using othersFx;
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
        left join MiniMarket m with(nolock) on m.miniMarketID = p.miniMarketID 
        where 1=1";

        #region getContent

        #region getProduct
        public static BOProduct getContent(String productID)
        {
            string xSQL = defaultFields + " and p.productID='" + DBHelper.cleanParam(productID) + "' ";

            return getContentQR(xSQL);

        }
        #endregion


        private static BOProduct getContentQR(string xSQL)
        {
            BOProduct xPinjaman = null;
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


        #region getList

        #region getListProductQuery
        public static BOProductList getListProductQuery(String searchText)
        {
            string xSQL = defaultFields;

            if (searchText != "")
            {
                xSQL += " and p.productName like " + "'%" + searchText + "%'";
            }

            return getPinjamanTxnListQR(xSQL);
        }
        #endregion

        #region getListQR
        private static BOProductList getPinjamanTxnListQR(string xSQL)
        {
            BOProductList xTxnList = null;

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
                            xTxnList = new BOProductList();
                            while (myReader.Read())
                            {
                                xTxnList.Add(fillDataRecord(myReader));
                            }
                        }
                        myReader.Close();
                    }
                    myComm.Dispose();
                }
            }

            return xTxnList;

        }
        #endregion


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

            xTxn.miniMarketAddress = (!mD.IsDBNull(mD.GetOrdinal("miniMarketAddress"))) ? mD.GetString(mD.GetOrdinal("miniMarketAddress")) : null;
            xTxn.miniMarketName = (!mD.IsDBNull(mD.GetOrdinal("miniMarketName"))) ? mD.GetString(mD.GetOrdinal("miniMarketName")) : null;

            xTxn.entryDate = DBHelper.getDBDateTime(mD, "entryDate");
            xTxn.lastUpdate = DBHelper.getDBDateTime(mD, "lastUpdate");

            return xTxn;
        }

        #endregion

    }
}
