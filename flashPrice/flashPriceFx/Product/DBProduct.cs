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
        public static BOProductList getListProductQuery(String searchText, String categoryProduct)
        {
            string xSQL = defaultFields;

            if (searchText != "")
            {
                xSQL += " and p.productName like " + "'%" + searchText + "%'";
            }

            if (categoryProduct != "")
            {
                xSQL += " and p.productCategoryID like " + "'%" + categoryProduct + "%'";
            }

            return getProuctListQR(xSQL);
        }
        #endregion

        #region getListQR
        private static BOProductList getProuctListQR(string xSQL)
        {
            BOProductList xBOList = null;

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
                            xBOList = new BOProductList();
                            while (myReader.Read())
                            {
                                xBOList.Add(fillDataRecord(myReader));
                            }
                        }
                        myReader.Close();
                    }
                    myComm.Dispose();
                }
            }

            return xBOList;

        }
        #endregion


        #endregion

        #region fillDataRecord

        public static BOProduct fillDataRecord(IDataRecord mD)
        {
            BOProduct xBO = new BOProduct();

            xBO.productID = (!mD.IsDBNull(mD.GetOrdinal("productID"))) ? mD.GetString(mD.GetOrdinal("productID")) : null;
            xBO.productName = (!mD.IsDBNull(mD.GetOrdinal("productName"))) ? mD.GetString(mD.GetOrdinal("productName")) : null;
            xBO.productDescription = (!mD.IsDBNull(mD.GetOrdinal("productDescription"))) ? mD.GetString(mD.GetOrdinal("productDescription")) : null;
            xBO.productPrice = (!mD.IsDBNull(mD.GetOrdinal("productPrice"))) ? mD.GetInt32(mD.GetOrdinal("productPrice")) : 0;
            xBO.productCategoryID = (!mD.IsDBNull(mD.GetOrdinal("productCategoryID"))) ? mD.GetString(mD.GetOrdinal("productCategoryID")) : null;
            xBO.productImageUrl = (!mD.IsDBNull(mD.GetOrdinal("productImageUrl"))) ? mD.GetString(mD.GetOrdinal("productImageUrl")) : null;

            xBO.miniMarketAddress = (!mD.IsDBNull(mD.GetOrdinal("miniMarketAddress"))) ? mD.GetString(mD.GetOrdinal("miniMarketAddress")) : null;
            xBO.miniMarketName = (!mD.IsDBNull(mD.GetOrdinal("miniMarketName"))) ? mD.GetString(mD.GetOrdinal("miniMarketName")) : null;
            xBO.miniMarketType = (!mD.IsDBNull(mD.GetOrdinal("miniMarketType"))) ? mD.GetString(mD.GetOrdinal("miniMarketType")) : null;
            
            xBO.entryDate = DBHelper.getDBDateTime(mD, "entryDate");
            xBO.lastUpdate = DBHelper.getDBDateTime(mD, "lastUpdate");

            return xBO;
        }

        #endregion

    }
}
