using flashPriceFX;
using HCFx;
using HCPLUSFx.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flashPriceFx.MiniMarket
{
    public class DBMiniMarket
    {
        const string defaultFields = @"
        select *
        from miniMarket m with(nolock)
        where 1=1";


        #region getContent

        #region getIDMiniMarketByMiniMarketName
        public static BOMiniMarket getIDMiniMarketByMiniMarketName(String miniMarketName)
        {
            string xSQL = defaultFields + " and m.miniMarketName like '%" + DBHelper.cleanParam(miniMarketName) + "%' ";

            return getContentQR(xSQL);

        }
        #endregion

        #region getIDMiniMarketByMiniMarketName
        public static BOMiniMarket getContentByID(String miniMarketID)
        {
            string xSQL = defaultFields + " and m.MiniMarketID = '" + DBHelper.cleanParam(miniMarketID) + "' ";

            return getContentQR(xSQL);

        }
        #endregion

        #region getContentQR
        private static BOMiniMarket getContentQR(string xSQL)
        {
            BOMiniMarket xMiniMarket = null;
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
                            xMiniMarket = new BOMiniMarket();
                            while (myReader.Read())
                            {
                                xMiniMarket = fillDataRecord(myReader);
                            }
                        }
                        myReader.Close();
                    }
                    myComm.Dispose();
                }
            }

            return xMiniMarket;
        }
        #endregion

        #endregion


        #region getList

        #region getListAllMiniMarket
        public static BOMiniMarketList getListAllMiniMarket()
        {
            string xSQL = defaultFields;

            return getMiniMarketListQR(xSQL);
        }

        public static BOMiniMarketList getListMinimarketQueries(String searchText, String sortBy, String sortDir)
        {
            string xSQL = defaultFields;

            if (searchText != "")
            {
                xSQL += " and m.miniMarketName like " + "'%" + searchText + "%'";
            }

            if (sortBy != "")
            {
                xSQL += "  order by " + sortBy + " " + sortDir;
            }

            return getMiniMarketListQR(xSQL);
        }

        #endregion

        #region getListMiniMarketForAutoComplete
        public static BOMiniMarketList getListMiniMarketForAutoComplete()
        {
            string xSQL = @"

            select
            m.miniMarketName
            from miniMarket m with(nolock)";

            return getMiniMarketListQRForAutoComplete(xSQL);
        }
        #endregion


        #region manage product

        public static BOProcessResult manageMinimarket(BOMiniMarket xMinimarket, String flag)
        {
            SqlTransaction myTxn = null;
            SqlConnection myCon;
            myCon = new SqlConnection(DBUtil.conStringdbflashPrice);

            BOProcessResult retVal = new BOProcessResult();

            retVal.xProcessName = "DBProduct.manageMinimarket";

            try
            {
                myCon.Open();
                if (myCon.State == ConnectionState.Open)
                {
                    myTxn = myCon.BeginTransaction();
                    retVal = manageProductSP(myCon, myTxn, xMinimarket, flag);

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

        private static BOProcessResult manageProductSP(SqlConnection nCon, SqlTransaction nTxn, BOMiniMarket myHEAD, String flag)
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

                    cmHead.Parameters.AddWithValue("@miniMarketID", (object)myHEAD.miniMarketID ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@miniMarketName", (object)myHEAD.miniMarketName);
                    cmHead.Parameters.AddWithValue("@miniMarketAddress", (object)myHEAD.miniMarketAddress);
                    cmHead.Parameters.AddWithValue("@miniMarketType", (object)myHEAD.miniMarketType ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@miniMarketLattitude", (object)myHEAD.miniMarketLattitude ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@miniMarketLongitude", (object)myHEAD.miniMarketLongitude ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@flag", (object)flag ?? DBNull.Value);

                    cmHead.CommandText = "[S_MiniMarket_ManageMinimarket]";
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


        #region getListQR for autocomplete
        private static BOMiniMarketList getMiniMarketListQRForAutoComplete(string xSQL)
        {
            BOMiniMarketList xBOList = null;

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
                            xBOList = new BOMiniMarketList();
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


        #region fillDataRecordForAutoComplete
        public static BOMiniMarket fillDataRecordForAutoComplete(IDataRecord mD)
        {
            BOMiniMarket xBO = new BOMiniMarket();

            xBO.miniMarketName = (!mD.IsDBNull(mD.GetOrdinal("miniMarketName"))) ? mD.GetString(mD.GetOrdinal("miniMarketName")) : null;

            return xBO;
        }
        #endregion


        #region getListQR
        private static BOMiniMarketList getMiniMarketListQR(string xSQL)
        {
            BOMiniMarketList xBOList = null;

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
                            xBOList = new BOMiniMarketList();
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

        #region fillDataRecord original
        public static BOMiniMarket fillDataRecord(IDataRecord mD)
        {
            BOMiniMarket xBO = new BOMiniMarket();
            xBO.miniMarketID = (!mD.IsDBNull(mD.GetOrdinal("miniMarketID"))) ? mD.GetString(mD.GetOrdinal("miniMarketID")) : null;
            xBO.miniMarketName = (!mD.IsDBNull(mD.GetOrdinal("miniMarketName"))) ? mD.GetString(mD.GetOrdinal("miniMarketName")) : null;
            xBO.miniMarketAddress = (!mD.IsDBNull(mD.GetOrdinal("miniMarketAddress"))) ? mD.GetString(mD.GetOrdinal("miniMarketAddress")) : null;
            xBO.miniMarketLattitude = (!mD.IsDBNull(mD.GetOrdinal("miniMarketLattitude"))) ? mD.GetDecimal(mD.GetOrdinal("miniMarketLattitude")) : 0;
            xBO.miniMarketLongitude = (!mD.IsDBNull(mD.GetOrdinal("miniMarketLongitude"))) ? mD.GetDecimal(mD.GetOrdinal("miniMarketLongitude")) : 0;
            xBO.miniMarketAddress = (!mD.IsDBNull(mD.GetOrdinal("miniMarketAddress"))) ? mD.GetString(mD.GetOrdinal("miniMarketAddress")) : null;
            xBO.miniMarketName = (!mD.IsDBNull(mD.GetOrdinal("miniMarketName"))) ? mD.GetString(mD.GetOrdinal("miniMarketName")) : null;
            xBO.miniMarketType = (!mD.IsDBNull(mD.GetOrdinal("miniMarketType"))) ? mD.GetString(mD.GetOrdinal("miniMarketType")) : null;

            xBO.entryDate = DBHelper.getDBDateTime(mD, "entryDate");
            xBO.lastUpdate = DBHelper.getDBDateTime(mD, "lastUpdate");

            return xBO;
        }
        #endregion
        #endregion
    }
}
