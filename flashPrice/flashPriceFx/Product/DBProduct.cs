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

namespace flashPriceFx.Product
{
    public class DBProduct
    {
        const string defaultFields = @"
        select ROW_NUMBER() OVER (ORDER BY productID) AS rowNum, p.*, m.*
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

        public static BOProduct getMostExpensiveProduct(String searchText, String categoryProduct, bool isViewSponsorship)
        {
            string xSQL = @"
            select top 1 ROW_NUMBER() OVER (ORDER BY productID) AS rowNum, p.*, m.* from product p
            left join MiniMarket m with(nolock) on m.miniMarketID = p.miniMarketID
            where 1=1"; // ubah ini

            bool isIncludeZeroPrice = false;

            if (!isIncludeZeroPrice)
            {
                xSQL += " and p.productPrice != 0";
            }

            if (isViewSponsorship)
            {
                xSQL += " and isSponsorShip = 'T'";
            }

            if (searchText != "")
            {
                xSQL += " and p.productName like " + "'%" + searchText + "%'";
            }

            if (categoryProduct != "")
            {
                xSQL += " and p.productCategoryID like " + "'%" + categoryProduct + "%'";
            }

            xSQL += " order by p.productPrice asc"; // ubah ini

            return getContentQR(xSQL);

        }
        #endregion


        #region getContentQR
        private static BOProduct getContentQR(string xSQL)
        {
            BOProduct xProduct = null;
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
                            xProduct = new BOProduct();
                            while (myReader.Read())
                            {
                                xProduct = fillDataRecord(myReader);
                            }
                        }
                        myReader.Close();
                    }
                    myComm.Dispose();
                }
            }

            return xProduct;
        }
        #endregion
        #endregion

        #region getList

        #region getListProductQuery
        public static BOProductList getListProductQuery(String searchText, String categoryProduct, String minimarketTarget, bool isViewSponsorship, String sortBy, String sortDir, int startRow, int maxRow)
        {
            if (sortBy == "")
            {
                sortBy = "productID";
                sortDir = "asc";
            }

            string xSQL = @"
            select * from (select ROW_NUMBER() OVER (ORDER BY " + sortBy + @") AS rowNum, p.*, m.miniMarketName, m.miniMarketAddress, m.miniMarketType
            from Product p with(nolock)
            left join MiniMarket m with(nolock) on m.miniMarketID = p.miniMarketID
            where 1=1";

            bool isIncludeZeroPrice = false;

            if (!isIncludeZeroPrice)
            {
                xSQL += " and p.productPrice != 0";
            }

            if (isViewSponsorship)
            {
                xSQL += " and isSponsorShip = 'T'";
            }

            if (searchText != "")
            {
                xSQL += " and p.productName like " + "'%" + searchText + "%'";
            }

            if (categoryProduct != "")
            {
                xSQL += " and p.productCategoryID like " + "'%" + categoryProduct + "%'";
            }

            if (minimarketTarget != "")
            {
                xSQL += " and p.miniMarketID like " + "'%" + minimarketTarget + "%'";
            }

            xSQL += ") as field where rowNum between '" + startRow.ToString() + "' and '" + (startRow + maxRow).ToString() + "'";

            if (sortBy != "")
            {
                xSQL += "  order by " + sortBy + " " + sortDir;
            }

            /* old codes 
                
            string xSQL = defaultFields;

                if (searchText != "")
                {
                    xSQL += " and p.productName like " + "'%" + searchText + "%'";
                }

                if (categoryProduct != "")
                {
                    xSQL += " and p.productCategoryID like " + "'%" + categoryProduct + "%'";
                }
            */

            return getProductListQR(xSQL);
        }

        public static decimal getCountLisProductQuery(String searchText, String categoryProduct, String minimarketTarget, bool isViewSponsorship, String sortBy, String sortDir, int startRow, int maxRow)
        {
            string xSQL = @"
            select count(*)
            from Product p with(nolock)
            where 1=1 ";

            bool isIncludeZeroPrice = false;

            if (!isIncludeZeroPrice)
            {
                xSQL += " and p.productPrice != 0";
            }

            if (minimarketTarget != "")
            {
                xSQL += " and p.miniMarketID like " + "'%" + minimarketTarget + "%'";
            }

            if (isViewSponsorship)
            {
                xSQL += " and isSponsorShip = 'T'";
            }

            if (searchText != "")
            {
                xSQL += " and p.productName like " + "'%" + searchText + "%'";
            }

            if (categoryProduct != "")
            {
                xSQL += " and p.productCategoryID like " + "'%" + categoryProduct + "%'";
            }

            return DBUtil.execScalarInt(xSQL);
        }
        #endregion

        #region getListProductWithoutRowNumber
        public static BOProductList getListProductWithoutRowNumber(String searchText, String categoryProduct, bool isViewSponsorship, String sortBy, String sortDir)
        {
            if (sortBy == "")
            {
                sortBy = "productID";
                sortDir = "asc";
            }

            string xSQL = @"
            select * from product p
            left join MiniMarket m with(nolock) on m.miniMarketID = p.miniMarketID
            where 1=1"; // ubah ini

            bool isIncludeZeroPrice = false;

            if (!isIncludeZeroPrice)
            {
                xSQL += " and p.productPrice != 0";
            }

            if (isViewSponsorship)
            {
                xSQL += " and isSponsorShip = 'T'";
            }

            if (searchText != "")
            {
                xSQL += " and p.productName like " + "'%" + searchText + "%'";
            }

            if (categoryProduct != "")
            {
                xSQL += " and p.productCategoryID like " + "'%" + categoryProduct + "%'";
            }
          
            if (sortBy != "")
            {
                xSQL += "  order by " + sortBy + " " + sortDir;
            }

            return getProductListWithoutRowNumberQR(xSQL);
        }
        #endregion
        #endregion

        #region getListProductForAutoComplete
        public static BOProductList getListProductForAutoComplete()
        {
            string xSQL = @"
            select

            productName

            from Product p with(nolock)";

            return getProductListQRForAutoComplete(xSQL);
        }
        #endregion

        #region getProductListWithoutRowNumberQR
        private static BOProductList getProductListWithoutRowNumberQR(string xSQL)
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
                                xBOList.Add(fillDataRecordWithoutRowNumber(myReader));
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

        #region getListQR
        private static BOProductList getProductListQR(string xSQL)
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

        #region getListQR for autocomplete
        private static BOProductList getProductListQRForAutoComplete(string xSQL)
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
                                xBOList.Add(fillDataRecordForAutoComplete(myReader));
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

        #region manage product

        public static BOProcessResult manageProduct(BOProduct xProduct, String flag)
        {
            SqlTransaction myTxn = null;
            SqlConnection myCon;
            myCon = new SqlConnection(DBUtil.conStringdbflashPrice);

            BOProcessResult retVal = new BOProcessResult();

            retVal.xProcessName = "DBProduct.manageProduct";

            try
            {
                myCon.Open();
                if (myCon.State == ConnectionState.Open)
                {
                    myTxn = myCon.BeginTransaction();
                    retVal = manageProductSP(myCon, myTxn, xProduct, flag);

                    if (retVal.isSuccess)
                    {
                        myTxn.Commit();
                    }
                    else
                    {
                        myTxn.Rollback();
                        retVal.isSuccess = false;
                    }

                }

                else
                {
                    if (myTxn.Connection != null) { myTxn.Rollback(); }
                    retVal.xMessage = "EXCEPTION! Failed Open Connection";
                }

            }

            catch (Exception ex)
            {
                if (myTxn.Connection != null) { myTxn.Rollback(); }
                retVal.xMessage = "EXCEPTION! " + ex.Message;
                retVal.isSuccess = false;
            }

            if (myCon.State == ConnectionState.Open) myCon.Close();
            return retVal;
        }

        private static BOProcessResult manageProductSP(SqlConnection nCon, SqlTransaction nTxn, BOProduct myHEAD, String flag)
        {
            BOProcessResult retVal = new BOProcessResult();

            try
            {
                if (nCon.State == ConnectionState.Open)
                {
                    SqlParameter hasiltxnID;
                    SqlCommand cmHead = new SqlCommand();

                    cmHead.Connection = nCon;
                    cmHead.Transaction = nTxn;
                    cmHead.CommandType = CommandType.StoredProcedure;
                    cmHead.Parameters.Clear();

                    cmHead.Parameters.AddWithValue("@productID", (object)myHEAD.productID ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@productName", (object)myHEAD.productName);
                    cmHead.Parameters.AddWithValue("@productPrice", (object)myHEAD.productPrice);
                    cmHead.Parameters.AddWithValue("@isSponsorship", (object)myHEAD.isSponsorship ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@flag", (object)flag ?? DBNull.Value);

                    cmHead.CommandText = "[S_Product_ManageProduct]";
                    cmHead.ExecuteNonQuery();

                    retVal.isSuccess = true;
                }
            }

            catch (Exception ex)
            {
                retVal.xMessage = "EXCEPTION!DBProduct.manageProductSP" + ex.Message;
                retVal.isSuccess = false;
            }

            return retVal;
        }

        #endregion

        #region fillDataRecord

        #region fillDataRecordWithoutRowNumber
        public static BOProduct fillDataRecordWithoutRowNumber(IDataRecord mD)
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

            xBO.isSponsorship = DBHelper.getDBBool(mD, "isSponsorship", false);

            xBO.entryDate = DBHelper.getDBDateTime(mD, "entryDate");
            xBO.lastUpdate = DBHelper.getDBDateTime(mD, "lastUpdate");

            return xBO;
        }
        #endregion

        #region fillDataRecord original
        public static BOProduct fillDataRecord(IDataRecord mD)
        {
            BOProduct xBO = new BOProduct();
            xBO.rowNum = (!mD.IsDBNull(mD.GetOrdinal("rowNum"))) ? mD.GetInt64(mD.GetOrdinal("rowNum")) : 0;
            xBO.productID = (!mD.IsDBNull(mD.GetOrdinal("productID"))) ? mD.GetString(mD.GetOrdinal("productID")) : null;
            xBO.productName = (!mD.IsDBNull(mD.GetOrdinal("productName"))) ? mD.GetString(mD.GetOrdinal("productName")) : null;
            xBO.productDescription = (!mD.IsDBNull(mD.GetOrdinal("productDescription"))) ? mD.GetString(mD.GetOrdinal("productDescription")) : null;
            xBO.productPrice = (!mD.IsDBNull(mD.GetOrdinal("productPrice"))) ? mD.GetInt32(mD.GetOrdinal("productPrice")) : 0;
            xBO.productCategoryID = (!mD.IsDBNull(mD.GetOrdinal("productCategoryID"))) ? mD.GetString(mD.GetOrdinal("productCategoryID")) : null;
            xBO.productImageUrl = (!mD.IsDBNull(mD.GetOrdinal("productImageUrl"))) ? mD.GetString(mD.GetOrdinal("productImageUrl")) : null;

            xBO.miniMarketAddress = (!mD.IsDBNull(mD.GetOrdinal("miniMarketAddress"))) ? mD.GetString(mD.GetOrdinal("miniMarketAddress")) : null;
            xBO.miniMarketName = (!mD.IsDBNull(mD.GetOrdinal("miniMarketName"))) ? mD.GetString(mD.GetOrdinal("miniMarketName")) : null;
            xBO.miniMarketType = (!mD.IsDBNull(mD.GetOrdinal("miniMarketType"))) ? mD.GetString(mD.GetOrdinal("miniMarketType")) : null;

            xBO.isSponsorship = DBHelper.getDBBool(mD, "isSponsorship", false);

            xBO.entryDate = DBHelper.getDBDateTime(mD, "entryDate");
            xBO.lastUpdate = DBHelper.getDBDateTime(mD, "lastUpdate");

            return xBO;
        }
        #endregion

        #region fillDataRecordForAutoComplete
        public static BOProduct fillDataRecordForAutoComplete(IDataRecord mD)
        {
            BOProduct xBO = new BOProduct();

            xBO.productName = (!mD.IsDBNull(mD.GetOrdinal("productName"))) ? mD.GetString(mD.GetOrdinal("productName")) : null;

            return xBO;
        }
        #endregion

        #endregion

    }
}
