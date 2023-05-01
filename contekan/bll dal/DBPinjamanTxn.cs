using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using HCPLUSFx.DAL;
using HCFx;
using KopkariFX.Quota.QuotaHeader;
using HCPLUSFx;
using KopkariFX.Quota.QuotaDetail;
using KopkariFX.PinjamanTxn.PinjamanStatus;
using KopkariFX;

namespace KopkariFX.PinjamanTxn
{
    public class DBPinjamanTxn
    {
        const string defaultFields = @"
select 

p.PinjamanID, 
p.amount,
p.txnDate,
p.txnStatus,
p.PinjamanNo,
p.tenor,
p.cicilanPokok,
p.bunga,
p.typeID,
p.packetID,
p.tujuan,
p.isDocComplete,
p.notes,
p.entryDate,

p.compID,
p.locID,
p.jobTitleID,
p.jobLevelID,
p.orgID,
p.hrdNIP,
p.bpuID,
p.alasanID,
p.txnOrigin,
p.txnRequestFrom,
p.pencairanDate,
p.pelunasanDate,

a.anggotaNIP, 
a.anggotaName,
jt.jobTitleName,
a.userID,
a.empTypeID,

mst.statusName,
mst.statusByAnggota,

pkt.packetName,

co.compName,

lo.locName,

al.description,

cl.classID,

dt.jmlCicil,

hrd.hrdNIP,

p.lastUpdate,

asp.superiorID,
asp.empID,
asp.superSuperiorID

,pkt.isAllowAutoBypassSuperior
,pkt.isAllowAutoBypassAnalystCredit
,pkt.analystCreditMethod
from PNJ_t_Header p with(nolock) 
left join APP_m_Anggota a with(nolock) on p.userID = a.userID 
left join app_m_class cl with(nolock) on cl.classID = a.classID 
left join app_m_company co with(nolock) on a.compID = co.compID 
left join app_m_location lo with(nolock) on a.locID = lo.locID 
left join app_m_joblevel jl with(nolock) on a.jobLevelID = jl.jobLevelID 
left join app_m_jobtitle jt with(nolock) on a.jobtitleID = jt.jobTitleID 
left join PNJ_m_Packet pkt with(nolock) on pkt.packetID = p.packetID 
left join pnj_m_alasan al with(nolock) on p.alasanID = al.alasanID
left join pnj_m_status mst with(nolock) on p.txnStatus = mst.statusCode
left join APP_m_AnggotaSuperior asp with (nolock) on asp.empID = a.anggotaNIP
left join (select pinjamanID , count(1)  as jmlCicil from PNJ_t_Cicilan where txnStatus = 'T' group by pinjamanID) dt on p.pinjamanID = dt.pinjamanID
left join APP_m_HRD hrd with(nolock) on  hrd.userNIP = a.anggotaNIP
 
where 1=1";

        #region getContent

        #region getPinjaman
        public static BOPinjamanTxn getPinjaman(String txnID)
        {
            string xSQL = defaultFields + " and p.pinjamanID='" + DBHelper.cleanParam(txnID) + "' ";

            return getContentQR(xSQL);

        }
        #endregion

        #region getLoadTxn
        public static BOPinjamanTxn getLoadTxn(String txnID)
        {

            string xSQL = defaultFields + "and p.pinjamanID='" + DBHelper.cleanParam(txnID) + "' ";

            return getContentQR(xSQL);

        }
        #endregion

        #region getContentQR
        private static BOPinjamanTxn getContentQR(string xSQL)
        {
            BOPinjamanTxn xPinjaman = null;
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
                            xPinjaman = new BOPinjamanTxn();
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

        #region getLastPinjamanID
        public static BOPinjamanTxn getLastPinjaman()
        {
            BOPinjamanTxn xPinjaman = null;
            string xSQL = "select top 1 * from PNJ_t_Header with(nolock) order by pinjamanID desc";

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
                            xPinjaman = new BOPinjamanTxn();
                            while (myReader.Read())
                            {
                                xPinjaman = fillDataRecordGetLastID(myReader);

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

        #endregion

        #region getList

        #region getListPinjamanAnggota by IDAnggota

        public static BOPinjamanTxnList getListPinjamanAnggota(String userID, String sortBy, String sortDir)
        {

            string xSQL = defaultFields + " and a.userID='" + DBHelper.cleanParam(userID) + "'";

            if (sortBy != "")
            {
                xSQL += "  order by " + sortBy + " " + sortDir;
            }

            return getPinjamanTxnListQR(xSQL);
        }

        #endregion

        #region  getListPinjamanGridAnggota

        public static BOPinjamanTxnList getListPinjamanGridAnggota(String userID, String sortBy, String sortDir)
        {
            string xSQL = defaultFields + " and p.userID = '" + DBHelper.cleanParam(userID) + "'";

            if (sortBy != "")
            {
                xSQL += "  order by " + sortBy + " " + sortDir;
            }

            return getPinjamanTxnListQR(xSQL);
        }

        public static BOPinjamanTxnList getListPinjamanGridAnggota(String userID, String hrdNIP,
           DateTime? periodeStart, DateTime? periodeEnd,
           String sortBy,
           String sortDir)
        {
            string xSQL = defaultFields;

            if (userID != "")
            {
                xSQL += "  and p.userID = '" + DBHelper.cleanParam(userID) + "'";
            }

            if (hrdNIP != "")
            {
                xSQL += "  and hrd.hrdNIP = '" + DBHelper.cleanParam(hrdNIP) + "'";
            }

            if (periodeStart.HasValue)
            {
                xSQL += " and ( p.txnDate between '" + periodeStart.Value.date_to_yearMonthDay() + "' and '" + periodeEnd.Value.date_to_yearMonthDay() + "')";
            }

            if (sortBy != "")
            {
                xSQL += "  order by " + sortBy + " " + sortDir;
            }


            return getPinjamanTxnListQR(xSQL);
        }

        public static BOPinjamanTxnList getListPinjamanGridAnggota(String userID, String hrdNIP,
          DateTime? periodeStart, DateTime? periodeEnd,
          String txnStatus,
          String sortBy,
          String sortDir)
        {
            string xSQL = defaultFields;

            if (userID != "")
            {
                xSQL += "  and p.userID = '" + DBHelper.cleanParam(userID) + "'";
            }

            if (hrdNIP != "")
            {
                xSQL += "  and hrd.hrdNIP = '" + DBHelper.cleanParam(hrdNIP) + "'";
            }

            if (periodeStart.HasValue)
            {
                xSQL += " and ( p.txnDate between '" + periodeStart.Value.date_to_yearMonthDay() + " 00:00:00' and '" + periodeEnd.Value.date_to_yearMonthDay() + " 23:59:59')";
            }

            if (txnStatus != "")
            {
                xSQL += "  and p.txnStatus in ('" + txnStatus + "')";
            }

            if (sortBy != "")
            {
                xSQL += "  order by " + sortBy + " " + sortDir;
            }


            return getPinjamanTxnListQR(xSQL);
        }

        #endregion

        #region main fillGrid daftarPengajuanPinjaman

        public static BOPinjamanTxnList getListxPinjaman(String xCompanyList,
            String xLocationList,
            DateTime? periodeStart, DateTime? periodeEnd,
            String simpType,
            String searchCrit,
            String searchVal,
            String txnStatus,
            String sortBy,
            String sortDir)
        {
            string xSQL = defaultFields;

            if (xCompanyList != "")
            {
                xSQL += " and a.compID in ( " + "'" + xCompanyList + "')";
            }

            if (xLocationList != "")
            {
                xSQL += " and a.locID in ( " + "'" + xLocationList + "')";
            }

            if (simpType != "")
            {
                xSQL += "  and p.packetID ='" + simpType + "'";
            }

            if (periodeStart.HasValue)
            {
                xSQL += " and ( p.txnDate between '" + periodeStart.Value.date_to_yearMonthDay() + " 00:00:00' and '" + periodeEnd.Value.date_to_yearMonthDay() + " 23:59:59')";
            }


            if (searchCrit != null && searchCrit != "")
            {
                xSQL += " and " + searchCrit + "  LIKE '%" + DBHelper.cleanParam(searchVal) + "%' ";

            }

            if (txnStatus != "")
            {
                xSQL += "  and p.txnStatus in ('" + txnStatus + "')";
            }

            if (sortBy != "")
            {
                xSQL += "  order by " + sortBy + " " + sortDir;
            }


            return getPinjamanTxnListQR(xSQL);
        }

        public static BOPinjamanTxnList getListPinjamanforAtasan(String xCompanyList,
            String xLocationList,
            DateTime? periodeStart, DateTime? periodeEnd,
            String simpType,
            String searchCrit,
            String searchVal,
            String txnStatus,
            String sortBy,
            String sortDir,
            String userNIP)
        {
            string xSQL = defaultFields;

            if (xCompanyList != "")
            {
                xSQL += " and a.compID in ( " + "'" + xCompanyList + "')";
            }

            if (xLocationList != "")
            {
                xSQL += " and a.locID in ( " + "'" + xLocationList + "')";
            }

            if (simpType != "")
            {
                xSQL += "  and p.packetID ='" + simpType + "'";
            }

            if (periodeStart.HasValue)
            {
                xSQL += " and ( p.txnDate between '" + periodeStart.Value.date_to_yearMonthDay() + " 00:00:00' and '" + periodeEnd.Value.date_to_yearMonthDay() + " 23:59:59')";
            }


            if (searchCrit != null && searchCrit != "")
            {
                xSQL += " and " + searchCrit + "  LIKE '%" + DBHelper.cleanParam(searchVal) + "%' ";

            }

            if (txnStatus != "")
            {
                xSQL += "  and p.txnStatus in ('" + txnStatus + "')";
            }

            xSQL += " and (asp.superiorID = '" + userNIP + "' OR asp.supersuperiorID = '" + userNIP + "')";


            if (sortBy != "")
            {
                xSQL += "  order by " + sortBy + " " + sortDir;
            }




            return getPinjamanTxnListQR(xSQL);
        }

        public static BOPinjamanTxnList getListMultipleStatus(String xCompanyList,
           String xLocationList,
           DateTime? periodeStart, DateTime? periodeEnd,
           String simpType,
           String searchCrit,
           String searchVal,
           String txnStatus1,
           String txnStatus2,
           String sortBy,
           String sortDir)
        {
            string xSQL = defaultFields;

            if (xCompanyList != "")
            {
                xSQL += " and a.compID in ( " + "'" + xCompanyList + "')";
            }

            if (xLocationList != "")
            {
                xSQL += " and a.locID in ( " + "'" + xLocationList + "')";
            }

            if (simpType != "")
            {
                xSQL += "  and p.packetID ='" + simpType + "'";
            }

            if (periodeStart.HasValue)
            {
                xSQL += " and ( p.txnDate between '" + periodeStart.Value.date_to_yearMonthDay() + " 00:00:00' and '" + periodeEnd.Value.date_to_yearMonthDay() + " 23:59:59')";
            }


            if (searchCrit != null && searchCrit != "")
            {
                xSQL += " and " + searchCrit + "  LIKE '%" + DBHelper.cleanParam(searchVal) + "%' ";

            }

            if (txnStatus1 != "")
            {
                xSQL += "  and (p.txnStatus ='" + txnStatus1 + "'";
            }

            if (txnStatus2 != "")
            {
                xSQL += "  or p.txnStatus ='" + txnStatus2 + "'";
            }

            xSQL += ")";

            if (sortBy != "")
            {
                xSQL += "  order by " + sortBy + " " + sortDir;
            }


            return getPinjamanTxnListQR(xSQL);
        }

        #endregion

        #region getListQR
        private static BOPinjamanTxnList getPinjamanTxnListQR(string xSQL)
        {

            BOPinjamanTxnList xTxnList = null;

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
                            xTxnList = new BOPinjamanTxnList();
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

        #region pinjamanPrint

        public static string generatePinjamanNo(BOPinjamanTxn xBO)
        {
            SqlTransaction myTxn = null;
            SqlConnection myCon;

            string retVal = "";

            myCon = new SqlConnection(DBUtil.conStringdbKopkari);

            try
            {
                myCon.Open();
                if (myCon.State == ConnectionState.Open)
                {
                    myTxn = myCon.BeginTransaction();

                    SqlParameter hasilTXNID;
                    SqlCommand cmHead = new SqlCommand();
                    cmHead.Connection = myCon;
                    cmHead.Transaction = myTxn;
                    cmHead.CommandType = CommandType.StoredProcedure;
                    cmHead.Parameters.Clear();

                    cmHead.Parameters.AddWithValue("@pinjamanID", (object)xBO.pinjamanID ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@lastOperator", (object)xBO.lastOperator ?? DBNull.Value);

                    if (getPinjaman(xBO.pinjamanID) != null)
                    {
                        cmHead.Parameters.AddWithValue("@txnID", xBO.pinjamanID);
                        cmHead.CommandText = "S_PNJ_generatePinjamanNo";
                        cmHead.ExecuteNonQuery();
                        retVal = xBO.pinjamanID;
                    }
                    myTxn.Commit();

                }
            }
            catch (Exception ex)
            {
                if (myTxn.Connection != null) { myTxn.Rollback(); }
                retVal = "EXCEPTION! " + ex.Message;
                if (myCon.State == ConnectionState.Open) myCon.Close();
            }
            if (myCon.State == ConnectionState.Open) myCon.Close();
            return retVal;
        }

        public static DataSet getPinjamanPrint(string pinjamanID, string userID)
        {
            BOPinjamanTxn xTxn = getPinjaman(pinjamanID);
            if (xTxn.pinjamanNo == null)
            {
                string retVal = generatePinjamanNo(xTxn);
            }

            string xSQL = " select hd.pinjamanNo, hd.amount, hd.txnDate, hd.userID, hd.tenor, hd.cicilanPokok, case when pma.alasanID = 'A0023' then hd.tujuan else pma.description end as tujuan, us.anggotaName, us.phone1, us.anggotaNIP, us.KTPNo, us.KTPAddress, us.KTPCountry, us.KTPPostalCode, ci.cityName, co.compName, co.address from PNJ_t_Header hd with(nolock) left join APP_m_Anggota us with(nolock) on hd.userID=us.userID left join pnj_m_alasan pma with(nolock) on pma.alasanID = hd.alasanID left join APP_m_City ci with(nolock) on us.KTPCityID=ci.cityID join APP_m_Company co with(nolock) on us.compID=co.compID where 1=1"
                + "and hd.userID='" + DBHelper.cleanParam(userID) + "'"
                + "and hd.pinjamanID='" + DBHelper.cleanParam(pinjamanID) + "'";

            xSQL += " select distinct (select paramValue from APP_m_Parameter where 1=1 and paramID='pihak1Name') as pihak1Name, (select paramValue from APP_m_Parameter where 1=1 and paramID='pihak1JobTitle') as pihak1Jabatan";

            xSQL += " select distinct (select paramValue from APP_m_Parameter where 1=1 and paramID='pihak3Name') as pihak3Name, (select paramValue from APP_m_Parameter where 1=1 and paramID='pihak3JobTitle') as pihak3Jabatan";

            xSQL += " select lower(dbo.Terbilang(amount)) as amountStr, lower(dbo.Terbilang(tenor)) as tenorStr, bunga/12 as jasa, lower(dbo.Terbilang(bunga/12)) as jasaStr, round((amount*((bunga/12)/100))+cicilanPokok, 0) as cclTotal, lower(dbo.Terbilang(round((amount*((bunga/12)/100))+cicilanPokok, 0))) as cclTotalStr from PNJ_t_Header with(nolock) where 1=1"
                + "and pinjamanID='" + DBHelper.cleanParam(pinjamanID) + "'";

            return DBUtil.fillDasetGeneric("dasetPinjamanPrint", xSQL);
        }

        #endregion // buat print perjanjian dokumen  // ngeprint document

        #region sum total pinjaman

        public static decimal getTotalPinjamanUser(String userID)
        {
            string xSQL = "select sum(hd.amount) as amount from PNJ_t_Header hd with(nolock) where hd.userID = '" + DBHelper.cleanParam(userID) + "' and hd.txnStatus not in ('PSA001', 'PSA004', 'PSA006', 'PSA014', 'PSA017', 'PSA019', 'PSX001', 'PSX004', 'PSX006', 'PSX014', 'PSX016')";

            /*
                PSA0001 - PINJAMAN DITOLAK
                PSA0004 - DITOLAK ATASAN
                PSA0006 - DITOLAK PANITIA KREDIT
                PSA0014 - DITOLAK ADMIN
                PSA0017 - DITOLAK MANAGER KOPERASI
                PSA0019 - DITOLAK KETUA
                PSA0024 - LUNAS
            */
            return DBUtil.execScalarDecimal(xSQL);
        }

        #endregion

        #region fillDataRecord

        public static BOPinjamanTxn fillDataRecord(IDataRecord mD)
        {
            BOPinjamanTxn xTxn = new BOPinjamanTxn();

            xTxn.ID = (!mD.IsDBNull(mD.GetOrdinal("pinjamanID"))) ? mD.GetString(mD.GetOrdinal("pinjamanID")) : null;  //20220705 mike tambahin
            xTxn.pinjamanID = (!mD.IsDBNull(mD.GetOrdinal("pinjamanID"))) ? mD.GetString(mD.GetOrdinal("pinjamanID")) : null;
            xTxn.pinjamanNo = (!mD.IsDBNull(mD.GetOrdinal("pinjamanNo"))) ? mD.GetString(mD.GetOrdinal("pinjamanNo")) : null;
            xTxn.amount = (!mD.IsDBNull(mD.GetOrdinal("amount"))) ? mD.GetDecimal(mD.GetOrdinal("amount")) : 0;
            xTxn.txnStatus = (!mD.IsDBNull(mD.GetOrdinal("txnStatus"))) ? mD.GetString(mD.GetOrdinal("txnStatus")) : null;
            xTxn.statusName = (!mD.IsDBNull(mD.GetOrdinal("statusName"))) ? mD.GetString(mD.GetOrdinal("statusName")) : null;
            xTxn.statusByAnggota = (!mD.IsDBNull(mD.GetOrdinal("statusByAnggota"))) ? mD.GetString(mD.GetOrdinal("statusByAnggota")) : null;
            xTxn.txnDate = DBHelper.getDBDateTime(mD, "txnDate");

            xTxn.userID = (!mD.IsDBNull(mD.GetOrdinal("userID"))) ? mD.GetString(mD.GetOrdinal("userID")) : null;
            xTxn.tenor = (!mD.IsDBNull(mD.GetOrdinal("tenor"))) ? mD.GetInt32(mD.GetOrdinal("tenor")) : 0;
            xTxn.cicilanPokok = (!mD.IsDBNull(mD.GetOrdinal("cicilanPokok"))) ? mD.GetDecimal(mD.GetOrdinal("cicilanPokok")) : 0;
            xTxn.bunga = (!mD.IsDBNull(mD.GetOrdinal("bunga"))) ? mD.GetDecimal(mD.GetOrdinal("bunga")) : 0;
            xTxn.typeID = (!mD.IsDBNull(mD.GetOrdinal("typeID"))) ? mD.GetString(mD.GetOrdinal("typeID")) : null;
            xTxn.packetID = (!mD.IsDBNull(mD.GetOrdinal("packetID"))) ? mD.GetString(mD.GetOrdinal("packetID")) : null;
            xTxn.tujuan = (!mD.IsDBNull(mD.GetOrdinal("tujuan"))) ? mD.GetString(mD.GetOrdinal("tujuan")) : null;
            xTxn.isDocComplete = (!mD.IsDBNull(mD.GetOrdinal("isDocComplete"))) ? mD.GetString(mD.GetOrdinal("isDocComplete")) : null;
            xTxn.notes = (!mD.IsDBNull(mD.GetOrdinal("notes"))) ? mD.GetString(mD.GetOrdinal("notes")) : null;
            xTxn.compID = (!mD.IsDBNull(mD.GetOrdinal("compID"))) ? mD.GetString(mD.GetOrdinal("compID")) : null;
            xTxn.locID = (!mD.IsDBNull(mD.GetOrdinal("locID"))) ? mD.GetString(mD.GetOrdinal("locID")) : null;
            xTxn.compName = (!mD.IsDBNull(mD.GetOrdinal("compName"))) ? mD.GetString(mD.GetOrdinal("compName")) : null;
            xTxn.locName = (!mD.IsDBNull(mD.GetOrdinal("locName"))) ? mD.GetString(mD.GetOrdinal("locName")) : null;

            xTxn.jobTitleID = (!mD.IsDBNull(mD.GetOrdinal("jobTitleID"))) ? mD.GetString(mD.GetOrdinal("jobTitleID")) : null;
            xTxn.jobLevelID = (!mD.IsDBNull(mD.GetOrdinal("jobLevelID"))) ? mD.GetString(mD.GetOrdinal("jobLevelID")) : null;
            xTxn.orgID = (!mD.IsDBNull(mD.GetOrdinal("orgID"))) ? mD.GetString(mD.GetOrdinal("orgID")) : null;

            xTxn.superiorID = (!mD.IsDBNull(mD.GetOrdinal("superiorID"))) ? mD.GetString(mD.GetOrdinal("superiorID")) : null;
            xTxn.superSuperiorID = (!mD.IsDBNull(mD.GetOrdinal("superSuperiorID"))) ? mD.GetString(mD.GetOrdinal("superSuperiorID")) : null;
            xTxn.hrdNIP = (!mD.IsDBNull(mD.GetOrdinal("hrdNIP"))) ? mD.GetString(mD.GetOrdinal("hrdNIP")) : null;
            xTxn.bpuID = (!mD.IsDBNull(mD.GetOrdinal("bpuID"))) ? mD.GetString(mD.GetOrdinal("bpuID")) : null;
            xTxn.alasanID = (!mD.IsDBNull(mD.GetOrdinal("alasanID"))) ? mD.GetString(mD.GetOrdinal("alasanID")) : null;
            xTxn.txnOrigin = (!mD.IsDBNull(mD.GetOrdinal("txnOrigin"))) ? mD.GetString(mD.GetOrdinal("txnOrigin")) : null;
            xTxn.txnRequestFrom = (!mD.IsDBNull(mD.GetOrdinal("txnRequestFrom"))) ? mD.GetString(mD.GetOrdinal("txnRequestFrom")) : null;
            xTxn.pencairanDate = DBHelper.getDBDateTime(mD, "pencairanDate");
            xTxn.pelunasanDate = DBHelper.getDBDateTime(mD, "pelunasanDate");
            xTxn.entryDate = DBHelper.getDBDateTime(mD, "entryDate");
            //xTxn.entryOperator = (!mD.IsDBNull(mD.GetOrdinal("entryOperator"))) ? mD.GetString(mD.GetOrdinal("entryOperator")) : null;
            xTxn.lastUpdate = DBHelper.getDBDateTime(mD, "lastUpdate");
            //xTxn.lastOperator = (!mD.IsDBNull(mD.GetOrdinal("lastOperator"))) ? mD.GetString(mD.GetOrdinal("lastOperator")) : null;

            xTxn.anggotaName = (!mD.IsDBNull(mD.GetOrdinal("anggotaName"))) ? mD.GetString(mD.GetOrdinal("anggotaName")) : null;
            xTxn.anggotaNIP = (!mD.IsDBNull(mD.GetOrdinal("anggotaNIP"))) ? mD.GetString(mD.GetOrdinal("anggotaNIP")) : null;
            xTxn.jobTitleName = (!mD.IsDBNull(mD.GetOrdinal("jobTitleName"))) ? mD.GetString(mD.GetOrdinal("jobTitleName")) : null;
            xTxn.classID = (!mD.IsDBNull(mD.GetOrdinal("classID"))) ? mD.GetString(mD.GetOrdinal("classID")) : null;
            xTxn.compName = (!mD.IsDBNull(mD.GetOrdinal("compName"))) ? mD.GetString(mD.GetOrdinal("compName")) : null;
            xTxn.locName = (!mD.IsDBNull(mD.GetOrdinal("locName"))) ? mD.GetString(mD.GetOrdinal("locName")) : null;
            xTxn.packetName = (!mD.IsDBNull(mD.GetOrdinal("packetName"))) ? mD.GetString(mD.GetOrdinal("packetName")) : null;
            xTxn.description = (!mD.IsDBNull(mD.GetOrdinal("description"))) ? mD.GetString(mD.GetOrdinal("description")) : null;
            xTxn.empTypeID = (!mD.IsDBNull(mD.GetOrdinal("empTypeID"))) ? mD.GetString(mD.GetOrdinal("empTypeID")) : null;

            xTxn.jmlCicil = (!mD.IsDBNull(mD.GetOrdinal("jmlCicil"))) ? mD.GetInt32(mD.GetOrdinal("jmlCicil")) : 0;

            xTxn.isAllowAutoBypassSuperior = DBHelper.getDBBool(mD, "isAllowAutoBypassSuperior", false);
            xTxn.isAllowAutoBypassAnalystCredit = DBHelper.getDBBool(mD, "isAllowAutoBypassAnalystCredit", false);
            xTxn.analystCreditMethod = DBHelper.getDBStringNullable(mD, "analystCreditMethod");

            return xTxn;
        }

        #endregion

        #region fillDataRecordGetLastID

        public static BOPinjamanTxn fillDataRecordGetLastID(IDataRecord mD)
        {
            BOPinjamanTxn xTxn = new BOPinjamanTxn();

            xTxn.pinjamanID = (!mD.IsDBNull(mD.GetOrdinal("pinjamanID"))) ? mD.GetString(mD.GetOrdinal("pinjamanID")) : null;
            xTxn.pinjamanNo = (!mD.IsDBNull(mD.GetOrdinal("pinjamanNo"))) ? mD.GetString(mD.GetOrdinal("pinjamanNo")) : null;
            xTxn.amount = (!mD.IsDBNull(mD.GetOrdinal("amount"))) ? mD.GetDecimal(mD.GetOrdinal("amount")) : 0;
            xTxn.txnStatus = (!mD.IsDBNull(mD.GetOrdinal("txnStatus"))) ? mD.GetString(mD.GetOrdinal("txnStatus")) : null;
            xTxn.txnDate = DBHelper.getDBDateTime(mD, "txnDate");
            xTxn.userID = (!mD.IsDBNull(mD.GetOrdinal("userID"))) ? mD.GetString(mD.GetOrdinal("userID")) : null;
            xTxn.tenor = (!mD.IsDBNull(mD.GetOrdinal("tenor"))) ? mD.GetInt32(mD.GetOrdinal("tenor")) : 0;
            xTxn.cicilanPokok = (!mD.IsDBNull(mD.GetOrdinal("cicilanPokok"))) ? mD.GetDecimal(mD.GetOrdinal("cicilanPokok")) : 0;
            xTxn.bunga = (!mD.IsDBNull(mD.GetOrdinal("bunga"))) ? mD.GetDecimal(mD.GetOrdinal("bunga")) : 0;
            xTxn.typeID = (!mD.IsDBNull(mD.GetOrdinal("typeID"))) ? mD.GetString(mD.GetOrdinal("typeID")) : null;
            xTxn.packetID = (!mD.IsDBNull(mD.GetOrdinal("packetID"))) ? mD.GetString(mD.GetOrdinal("packetID")) : null;
            xTxn.tujuan = (!mD.IsDBNull(mD.GetOrdinal("tujuan"))) ? mD.GetString(mD.GetOrdinal("tujuan")) : null;
            xTxn.notes = (!mD.IsDBNull(mD.GetOrdinal("notes"))) ? mD.GetString(mD.GetOrdinal("notes")) : null;
            xTxn.compID = (!mD.IsDBNull(mD.GetOrdinal("compID"))) ? mD.GetString(mD.GetOrdinal("compID")) : null;
            xTxn.locID = (!mD.IsDBNull(mD.GetOrdinal("locID"))) ? mD.GetString(mD.GetOrdinal("locID")) : null;
            xTxn.jobTitleID = (!mD.IsDBNull(mD.GetOrdinal("jobTitleID"))) ? mD.GetString(mD.GetOrdinal("jobTitleID")) : null;
            xTxn.jobLevelID = (!mD.IsDBNull(mD.GetOrdinal("jobLevelID"))) ? mD.GetString(mD.GetOrdinal("jobLevelID")) : null;
            xTxn.orgID = (!mD.IsDBNull(mD.GetOrdinal("orgID"))) ? mD.GetString(mD.GetOrdinal("orgID")) : null;
            xTxn.superiorID = (!mD.IsDBNull(mD.GetOrdinal("superiorID"))) ? mD.GetString(mD.GetOrdinal("superiorID")) : null;
            xTxn.hrdNIP = (!mD.IsDBNull(mD.GetOrdinal("hrdNIP"))) ? mD.GetString(mD.GetOrdinal("hrdNIP")) : null;
            xTxn.bpuID = (!mD.IsDBNull(mD.GetOrdinal("bpuID"))) ? mD.GetString(mD.GetOrdinal("bpuID")) : null;
            xTxn.alasanID = (!mD.IsDBNull(mD.GetOrdinal("alasanID"))) ? mD.GetString(mD.GetOrdinal("alasanID")) : null;
            xTxn.txnOrigin = (!mD.IsDBNull(mD.GetOrdinal("txnOrigin"))) ? mD.GetString(mD.GetOrdinal("txnOrigin")) : null;
            xTxn.txnRequestFrom = (!mD.IsDBNull(mD.GetOrdinal("txnRequestFrom"))) ? mD.GetString(mD.GetOrdinal("txnRequestFrom")) : null;
            xTxn.pencairanDate = DBHelper.getDBDateTime(mD, "pencairanDate");
            xTxn.pelunasanDate = DBHelper.getDBDateTime(mD, "pelunasanDate");
            xTxn.entryDate = DBHelper.getDBDateTime(mD, "entryDate");
            xTxn.entryOperator = (!mD.IsDBNull(mD.GetOrdinal("entryOperator"))) ? mD.GetString(mD.GetOrdinal("entryOperator")) : null;
            xTxn.lastUpdate = DBHelper.getDBDateTime(mD, "lastUpdate");
            xTxn.lastOperator = (!mD.IsDBNull(mD.GetOrdinal("lastOperator"))) ? mD.GetString(mD.GetOrdinal("lastOperator")) : null;


            return xTxn;
        }

        #endregion // ambil id

        // insert

        #region addDataPinjamanHeader
        public static void addDataPinjamanHeader(BOPinjamanTxn xPinjaman)
        {

            string xSQL = "declare @getdate datetime = (select getdate()) exec S_PNJ_TxnHeaderInsert '"
+ xPinjaman.pinjamanNo + "',"
+ xPinjaman.amount + ",'"
+ xPinjaman.txnStatus + "',"
+ "@getdate" + ",'"
+ xPinjaman.userID + "',"
+ xPinjaman.tenor + ","
+ xPinjaman.cicilanPokok + ","
+ xPinjaman.bunga + ",'"
+ xPinjaman.typeID + "','"
+ xPinjaman.packetID + "','"
+ xPinjaman.tujuan + "','"
+ xPinjaman.notes + "','"
+ xPinjaman.compID + "','"
+ xPinjaman.locID + "','"
+ xPinjaman.jobTitleID + "','"
+ xPinjaman.jobLevelID + "','"
+ xPinjaman.orgID + "','"
+ xPinjaman.superiorID + "',"
+ xPinjaman.hrdNIP + ",'"
+ xPinjaman.bpuID + "','"
+ xPinjaman.alasanID + "',"
+ xPinjaman.txnOrigin + ",'"
+ xPinjaman.txnRequestFrom + "',"
+ "null" + ","
+ "null" + ",'"
+ xPinjaman.lastOperator + "', '1'";

            using (SqlConnection myConnection = new SqlConnection(DBUtil.conStringdbKopkari))
            {
                myConnection.Open();
                if (myConnection.State == ConnectionState.Open)
                {
                    SqlCommand myComm = new SqlCommand();
                    SqlDataAdapter myAdpt = new SqlDataAdapter();
                    myComm.Connection = myConnection;
                    myComm.CommandText = xSQL;
                    myComm.CommandType = CommandType.Text;

                    myAdpt.InsertCommand = new SqlCommand(xSQL, myConnection);
                    myAdpt.InsertCommand.ExecuteNonQuery();


                    myComm.Parameters.Clear();

                    myComm.Dispose();
                }
            }
        }
        #endregion

        private static BOProcessResult SaveHeader(SqlConnection nCon, SqlTransaction nTxn, BOPinjamanTxn myHEAD)
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

                    cmHead.Parameters.AddWithValue("@pinjamanNo", (object)myHEAD.pinjamanNo ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@amount", (object)myHEAD.amount);
                    cmHead.Parameters.AddWithValue("@txnStatus", (object)myHEAD.txnStatus);
                    cmHead.Parameters.AddWithValue("@userID", (object)myHEAD.userID ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@tenor", (object)myHEAD.tenor);
                    cmHead.Parameters.AddWithValue("@cicilanPokok", (object)myHEAD.cicilanPokok);
                    cmHead.Parameters.AddWithValue("@bunga", (object)myHEAD.bunga);
                    cmHead.Parameters.AddWithValue("@typeID", (object)myHEAD.typeID ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@packetID", (object)myHEAD.packetID);
                    cmHead.Parameters.AddWithValue("@tujuan", (object)myHEAD.tujuan);
                    cmHead.Parameters.AddWithValue("@notes", (object)myHEAD.notes ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@compID", (object)myHEAD.compID);
                    cmHead.Parameters.AddWithValue("@locID", (object)myHEAD.locID);
                    cmHead.Parameters.AddWithValue("@jobTitleID", (object)myHEAD.jobTitleID);
                    cmHead.Parameters.AddWithValue("@jobLevelID", (object)myHEAD.jobLevelID);
                    cmHead.Parameters.AddWithValue("@orgID", (object)myHEAD.orgID);
                    cmHead.Parameters.AddWithValue("@superiorID", (object)myHEAD.superiorID ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@hrdNIP", (object)myHEAD.hrdNIP);
                    cmHead.Parameters.AddWithValue("@bpuID", (object)myHEAD.bpuID ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@alasanID", (object)myHEAD.alasanID);
                    cmHead.Parameters.AddWithValue("@txnOrigin", (object)myHEAD.txnOrigin);
                    cmHead.Parameters.AddWithValue("@txnRequestFrom", (object)myHEAD.txnRequestFrom);
                    cmHead.Parameters.AddWithValue("@lastoperator", (object)myHEAD.lastOperator);

                    hasiltxnID = cmHead.Parameters.Add("@txnID", SqlDbType.NVarChar, 100);
                    hasiltxnID.Direction = ParameterDirection.Output;
                    cmHead.CommandText = "S_PNJ_TxnHeaderInsertBatch";
                    cmHead.ExecuteNonQuery();

                    retVal.xDataObject = hasiltxnID.Value.ToString();

                    retVal.xMessage = myHEAD.ID + " sukses insert";
                    retVal.isSuccess = true;
                }
            }

            catch (Exception ex)
            {
                retVal.xMessage = "EXCEPTION!DBPinjamanTxn.SaveHeader" + ex.Message;
                retVal.isSuccess = false;
            }

            return retVal;
        }

        public static BOProcessResult saveTxnPinjaman(BOPinjamanTxn xPinjaman, BOQuotaDetail xTxnDetail, BOQuotaHeader xTxnHeader, BOPinjamanStatus xStatus)
        {
            SqlTransaction myTxn = null;
            SqlConnection myCon;
            myCon = new SqlConnection(DBUtil.conStringdbKopkari);

            BOProcessResult retVal = new BOProcessResult();

            retVal.xProcessName = "DBPinjamanTxn.saveTxnPinjaman";

            try
            {
                myCon.Open();
                if (myCon.State == ConnectionState.Open)
                {
                    myTxn = myCon.BeginTransaction();
                    retVal = SaveHeader(myCon, myTxn, xPinjaman);


                    if (retVal.isSuccess)
                    {
                        xTxnDetail.ID = retVal.xDataObject.ToString();
                        xStatus.ID = retVal.xDataObject.ToString();
                        retVal = DBQuotaDetail.saveQuotaDetail(myCon, myTxn, xTxnDetail);


                        if (retVal.isSuccess)
                        {
                            xTxnHeader.ID = retVal.xDataObject.ToString();
                            retVal = DBQuotaHeader.saveQuotaHeader(myCon, myTxn, xTxnHeader);


                            if (retVal.isSuccess)
                            {

                                retVal = DBPinjamanStatus.saveStatusTxn(myCon, myTxn, xStatus);


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
                                myTxn.Rollback();
                                retVal.isSuccess = false;
                            }
                        }

                        else
                        {
                            myTxn.Rollback();
                            retVal.isSuccess = false;
                        }
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


        #region pengembalian Quota

        public static BOProcessResult QuotaRefund(BOQuotaDetail xTxnDetail, BOQuotaHeader xTxnHeader, BOPinjamanStatus xStatus)
        {
            SqlTransaction myTxn = null;
            SqlConnection myCon;
            myCon = new SqlConnection(DBUtil.conStringdbKopkari);

            BOProcessResult retVal = new BOProcessResult();

            retVal.xProcessName = "DBPinjamanTxn.pengembalianQuota";

            try
            {
                myCon.Open();
                if (myCon.State == ConnectionState.Open)
                {
                    myTxn = myCon.BeginTransaction();

                    xTxnDetail.ID = xTxnHeader.lastPinjamanID; // isinya pnj id
                    xStatus.ID = xTxnHeader.lastPinjamanID; // isinya pnj id
                    retVal = DBQuotaDetail.saveQuotaDetail(myCon, myTxn, xTxnDetail);


                    if (retVal.isSuccess)
                    {
                        xTxnHeader.ID = xTxnHeader.lastPinjamanID; ;
                        retVal = DBQuotaHeader.saveQuotaHeader(myCon, myTxn, xTxnHeader);


                        if (retVal.isSuccess)
                        {

                            retVal = DBPinjamanStatus.saveStatusTxn(myCon, myTxn, xStatus);


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
                            myTxn.Rollback();
                            retVal.isSuccess = false;
                        }
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
        #endregion

        #region isDocComplete

        public static string updateDokumenLengkap(BOPinjamanTxn xBO)
        {
            SqlTransaction myTxn = null;
            SqlConnection myCon;

            string retVal = "";

            myCon = new SqlConnection(DBUtil.conStringdbKopkari);

            try
            {
                myCon.Open();
                if (myCon.State == ConnectionState.Open)
                {
                    myTxn = myCon.BeginTransaction();

                    SqlCommand cmHead = new SqlCommand();
                    cmHead.Connection = myCon;
                    cmHead.Transaction = myTxn;
                    cmHead.CommandType = CommandType.StoredProcedure;
                    cmHead.Parameters.Clear();

                    cmHead.Parameters.AddWithValue("@pinjamanID", (object)xBO.pinjamanID ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@isDocComplete", (object)xBO.isDocComplete ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@description", (object)xBO.notes ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@lastOperator", (object)xBO.lastOperator ?? DBNull.Value);

                    cmHead.Parameters.AddWithValue("@txnID", xBO.pinjamanID);
                    cmHead.CommandText = "S_PNJ_DokumenLengkapUpdate";
                    cmHead.ExecuteNonQuery();
                    retVal = xBO.pinjamanID;

                    myTxn.Commit();

                }
            }
            catch (Exception ex)
            {
                if (myTxn.Connection != null) { myTxn.Rollback(); }
                retVal = "EXCEPTION! " + ex.Message;
                if (myCon.State == ConnectionState.Open) myCon.Close();
            }
            if (myCon.State == ConnectionState.Open) myCon.Close();
            return retVal;
        }

        public static BOProcessResult updateDokumenLengkap(SqlConnection myCon, SqlTransaction myTxn, BOPinjamanTxn xBO)
        {

            BOProcessResult retVal = new BOProcessResult();

            try
            {
                if (myCon.State == ConnectionState.Open)
                {
                    SqlCommand cmHead = new SqlCommand();
                    cmHead.Connection = myCon;
                    cmHead.Transaction = myTxn;
                    cmHead.CommandType = CommandType.StoredProcedure;
                    cmHead.Parameters.Clear();

                    cmHead.Parameters.AddWithValue("@pinjamanID", (object)xBO.pinjamanID ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@isDocComplete", (object)xBO.isDocComplete ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@description", (object)xBO.notes ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@lastOperator", (object)xBO.lastOperator ?? DBNull.Value);

                    cmHead.Parameters.AddWithValue("@txnID", xBO.pinjamanID);
                    cmHead.CommandText = "S_PNJ_DokumenLengkapUpdate";
                    cmHead.ExecuteNonQuery();
                    retVal.xDataObject = xBO.pinjamanID;
                    retVal.isSuccess = true;

                }
            }
            catch (Exception ex)
            {
                retVal.xMessage = "EXCEPTION! " + ex.Message;
                retVal.isSuccess = false;
            }
            return retVal;
        }

        #endregion

        #region pnj ketua koperasi

        public static BOPinjamanTxnList getListOnKetua(String xCompanyList,
         String xLocationList,
         DateTime? periodeStart, DateTime? periodeEnd,
         String simpType,
         String searchCrit,
         String searchVal,
         String txnStatus,
         String packetID,
         String sortBy,
         String sortDir)
        {
            string xSQL = defaultFields;

            if (xCompanyList != "")
            {
                xSQL += " and a.compID in ( " + "'" + xCompanyList + "')";
            }

            if (xLocationList != "")
            {
                xSQL += " and a.locID in ( " + "'" + xLocationList + "')";
            }

            if (simpType != "")
            {
                xSQL += "  and p.packetID ='" + simpType + "'";
            }

            if (periodeStart.HasValue)
            {
                xSQL += " and ( p.txnDate between '" + periodeStart.Value.date_to_yearMonthDay() + " 00:00:00' and '" + periodeEnd.Value.date_to_yearMonthDay() + " 23:59:59')";
            }


            if (searchCrit != null && searchCrit != "")
            {
                xSQL += " and " + searchCrit + "  LIKE '%" + DBHelper.cleanParam(searchVal) + "%' ";

            }

            if (packetID != "")
            {
                xSQL += " and p.packetID in ('" + packetID + "')";
            }

            if (txnStatus != "")
            {
                xSQL += "  and p.txnStatus in ('" + txnStatus + "')";
            }

            if (sortBy != "")
            {
                xSQL += "  order by " + sortBy + " " + sortDir;
            }


            return getPinjamanTxnListQR(xSQL);
        }
        #endregion



        #region save pengajuan pinjaman batch
        public static BOProcessResult savePinjamanKhususBatch(BOPinjamanTxnList xPinjamanList)
        {
            SqlTransaction myTxn = null;
            SqlConnection myCon;
            myCon = new SqlConnection(DBUtil.conStringdbKopkari);

            BOProcessResult retVal = new BOProcessResult();

            retVal.xProcessName = "DBPinjamanTxn.savePinjamanKhususBatch";

            try
            {
                myCon.Open();
                if (myCon.State == ConnectionState.Open)
                {
                    myTxn = myCon.BeginTransaction();

                    foreach (BOPinjamanTxn xPinjaman in xPinjamanList)
                    {
                        retVal = SaveHeader(myCon, myTxn, xPinjaman);
                    }

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
        #endregion


        #region upload

        public static string xxSQL;
        public static string uploadAttribute(DataTable dtUpload, string lastOperator, out DataTable result, string fileName)
        {
            SqlTransaction myTxn = null;
            SqlConnection myCon;
            result = null;

            string retVal = "";
            myCon = new SqlConnection(DBUtil.conStringdbKopkari);

            try
            {
                myCon.Open();
                if (myCon.State == ConnectionState.Open)
                {
                    myTxn = myCon.BeginTransaction();

                    retVal = uploadAttribute(myCon, myTxn, dtUpload, lastOperator, out result, fileName);
                    if (!retVal.Contains("EXCEPTION!"))
                    {
                        myTxn.Commit();
                    }
                    else
                    {
                        if (myTxn.Connection != null) { myTxn.Rollback(); }
                        retVal = "EXCEPTION! FailedHeader: " + retVal;
                    }
                }
                else
                {
                    if (myTxn.Connection != null) { myTxn.Rollback(); }
                    retVal = "EXCEPTION! Failed Open Connection";
                }

            }
            catch (Exception ex)
            {
                if (myTxn.Connection != null) { myTxn.Rollback(); }
                retVal = "EXCEPTION! " + ex.Message;
            }
            if (myCon.State == ConnectionState.Open) myCon.Close();
            return retVal;
        }

        public static string uploadAttribute(SqlConnection nCon, SqlTransaction nTxn
            , DataTable dtUpload, string lastOperator, out DataTable result, string fileName)
        {

            string retVal = "";
            result = null;

            SqlCommand cmHead = new SqlCommand();
            cmHead.Connection = nCon;
            cmHead.Transaction = nTxn;
            cmHead.CommandType = CommandType.StoredProcedure;
            cmHead.Parameters.Clear();


            try
            {

                xxSQL += "truncate table pnj_t_hakPengajuan; ";
                for (int i = 0; i < dtUpload.Rows.Count; i++)
                {

                    xxSQL += " INSERT INTO pnj_t_hakPengajuan select  a.userID"
                          + (!String.IsNullOrEmpty(dtUpload.Rows[i]["Flag"].ToString()) ? ",'" + dtUpload.Rows[i]["Flag"] + "'" : ",NULL")
                        + ", getdate(), "
                       + lastOperator + " 	FROM APP_m_Anggota a left join pnj_t_hakPengajuan hk on hk.userID = a.userID where a.anggotaNIP = "
                    + (!String.IsNullOrEmpty(dtUpload.Rows[i]["anggotaNIP"].ToString()) ? "'" + dtUpload.Rows[i]["anggotaNIP"] + "'" : "NULL");

                }

                using (SqlConnection myConnection = new SqlConnection(DBUtil.conStringdbKopkari))
                {
                    result = new DataTable();
                    myConnection.Open();
                    if (myConnection.State == ConnectionState.Open)
                    {
                        SqlCommand myComm = new SqlCommand();
                        myComm.Connection = nCon;
                        myComm.Transaction = nTxn;
                        myComm.CommandText = xxSQL;
                        myComm.CommandType = CommandType.Text;

                        myComm.Parameters.Clear();

                        SqlDataAdapter myReader = new SqlDataAdapter(myComm);
                        myReader.Fill(result);
                        myReader.Dispose();
                        myComm.Dispose();
                    }
                }

                retVal = "Upload sukses";


            }
            catch (Exception ex)
            {
                retVal = "EXCEPTION!" + ex.Message;
            }
            return retVal;
        }



        #endregion




        #region pengajuan from anggota


        private static BOProcessResult doAjukanFromAnggota(SqlConnection nCon, SqlTransaction nTxn, BOPinjamanTxn myHEAD)
        {
            BOProcessResult xBOReturnResult = new BOProcessResult("DBPinjamanTxn.doAjukanFromAnggota");
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

                    cmHead.Parameters.AddWithValue("@userID", (object)myHEAD.userID ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@packetID", (object)myHEAD.packetID ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@pinjamanAmount", (object)myHEAD.amount ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@tenor", (object)myHEAD.tenor ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@cicilanPokok", (object)myHEAD.cicilanPokok ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@bunga", (object)myHEAD.bunga ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@tujuan", (object)myHEAD.tujuan ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@alasanID", (object)myHEAD.alasanID ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@txnOrigin", (object)myHEAD.txnOrigin ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@txnRequestFrom", (object)myHEAD.txnRequestFrom ?? DBNull.Value);
                    cmHead.Parameters.AddWithValue("@lastOperator", (object)myHEAD.lastOperator ?? DBNull.Value);

                    hasiltxnID = cmHead.Parameters.Add("@txnID", SqlDbType.NVarChar, 100);
                    hasiltxnID.Direction = ParameterDirection.Output;
                    cmHead.CommandText = "S_PNJ_PengajuanPinjamanAnggota";
                    cmHead.ExecuteNonQuery();

                    xBOReturnResult.isSuccess = true;
                    xBOReturnResult.xStatusCode = "200";
                    xBOReturnResult.xDataObject = hasiltxnID.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                xBOReturnResult.xMessageList.Add(ex.Message);
            }

            return xBOReturnResult;
        }

        public static BOProcessResult pengajuanPinjamanFromAnggota(BOPinjamanTxn myHEAD)
        {
            BOProcessResult xBOReturnResult = new BOProcessResult("DBPayrollHeader.pengajuanPinjamanFromAnggota");

            SqlTransaction myTxn = null;
            SqlConnection myCon;
            myCon = new SqlConnection(DBUtil.conStringdbKopkari);
            try
            {
                myCon.Open();
                if (myCon.State == ConnectionState.Open)
                {
                    myTxn = myCon.BeginTransaction();
                    BOProcessResult xResult = doAjukanFromAnggota(myCon, myTxn, myHEAD);
                    xBOReturnResult.xInnerProcessResult = xResult;

                    if (xResult.isSuccess)
                    {
                        myTxn.Commit();
                        xBOReturnResult.isSuccess = true;
                        xBOReturnResult.xStatusCode = "200";
                        xBOReturnResult.xDataObject = xResult.xDataObject;  //tampungan dsini. 
                    }
                    else
                    {                        
                        //xBOReturnResult.xMessageList.AddRange(xResult.xMessageCompleteList); //gk perlu lagi krn udah dikonek via innerResult
                        if (myTxn.Connection != null) { myTxn.Rollback(); }
                    }
                }
                else
                {
                    xBOReturnResult.xMessageList.Add("EXCEPTION! Failed Open Connection");
                    if (myTxn.Connection != null) { myTxn.Rollback(); }
                }
            }
            catch (Exception ex)
            {
                xBOReturnResult.xMessageList.Add(ex.Message);
                if (myTxn.Connection != null) { myTxn.Rollback(); }
            }
            finally
            {
                if (myCon.State == ConnectionState.Open) myCon.Close();
            }
            return xBOReturnResult;
        }


        #endregion


        #region approve reject analis kredit


        public static BOProcessResult approveRejectAnalisKredit(string xPinjamanID, bool xIsApproveReject, string xApproveNotes, string xLastOperator)
        {
            BOProcessResult xBOReturnResult = new BOProcessResult("DBPinjamanTxn.approveRejectAnalisKredit");

            SqlTransaction myTxn = null;
            SqlConnection myCon;
            myCon = new SqlConnection(DBUtil.conStringdbKopkari);

            try
            {
                myCon.Open();
                if (myCon.State == ConnectionState.Open)
                {
                    myTxn = myCon.BeginTransaction();

                    SqlCommand cmHead = new SqlCommand();
                    cmHead.Connection = myCon;
                    cmHead.Transaction = myTxn;
                    cmHead.CommandType = CommandType.StoredProcedure;
                    cmHead.Parameters.Clear();

                    cmHead.Parameters.AddWithValue("@pinjamanID", xPinjamanID);
                    cmHead.Parameters.AddWithValue("@isApproveRejectFlag", DBHelper.setMyBoolean(xIsApproveReject));
                    cmHead.Parameters.AddWithValue("@approveNotes", xApproveNotes);
                    cmHead.Parameters.AddWithValue("@lastoperator", xLastOperator);
                    cmHead.CommandText = "S_PNJ_ApproveRejectAnalisKredit";

                    cmHead.ExecuteNonQuery();
                    myTxn.Commit();

                    xBOReturnResult.isSuccess = true;
                    xBOReturnResult.xStatusCode = "200";
                    xBOReturnResult.xDataObject = xPinjamanID;
                }
                else
                {
                    if (myTxn.Connection != null) { myTxn.Rollback(); }
                    xBOReturnResult.xMessageList.Add("EXCEPTION! Failed Open Connection");
                }
            }
            catch (Exception ex)
            {
                xBOReturnResult.xMessageList.Add("EXCEPTION!" + ex.Message);
                if (myTxn.Connection != null) { myTxn.Rollback(); }
            }

            return xBOReturnResult;
        }



        #endregion








        // gak kepake

        #region fillDataRecordGridAnggota

        public static BOPinjamanTxn fillDataRecordGridAnggota(IDataRecord mD)
        {
            BOPinjamanTxn xTxn = new BOPinjamanTxn();
            xTxn.pinjamanID = (!mD.IsDBNull(mD.GetOrdinal("pinjamanID"))) ? mD.GetString(mD.GetOrdinal("pinjamanID")) : null;
            xTxn.amount = (!mD.IsDBNull(mD.GetOrdinal("amount"))) ? mD.GetDecimal(mD.GetOrdinal("amount")) : 0;
            xTxn.txnStatus = (!mD.IsDBNull(mD.GetOrdinal("txnStatus"))) ? mD.GetString(mD.GetOrdinal("txnStatus")) : null;
            xTxn.txnDate = DBHelper.getDBDateTime(mD, "txnDate");
            xTxn.tenor = (!mD.IsDBNull(mD.GetOrdinal("tenor"))) ? mD.GetInt32(mD.GetOrdinal("tenor")) : 0;
            xTxn.jmlCicil = (!mD.IsDBNull(mD.GetOrdinal("jmlCicil"))) ? mD.GetInt32(mD.GetOrdinal("jmlCicil")) : 0;

            xTxn.packetName = (!mD.IsDBNull(mD.GetOrdinal("packetName"))) ? mD.GetString(mD.GetOrdinal("packetName")) : null;
            xTxn.lastUpdate = DBHelper.getDBDateTime(mD, "lastUpdate");

            return xTxn;
        }

        #endregion

        #region fillDataRecordLoadTxn

        public static BOPinjamanTxn fillDataRecordLoadTxn(IDataRecord mD)
        {
            BOPinjamanTxn xTxn = new BOPinjamanTxn();
            xTxn.pinjamanID = (!mD.IsDBNull(mD.GetOrdinal("pinjamanID"))) ? mD.GetString(mD.GetOrdinal("pinjamanID")) : null;
            xTxn.userID = (!mD.IsDBNull(mD.GetOrdinal("userID"))) ? mD.GetString(mD.GetOrdinal("userID")) : null;
            xTxn.amount = (!mD.IsDBNull(mD.GetOrdinal("amount"))) ? mD.GetDecimal(mD.GetOrdinal("amount")) : 0;
            xTxn.txnDate = DBHelper.getDBDateTime(mD, "txnDate");
            xTxn.tenor = (!mD.IsDBNull(mD.GetOrdinal("tenor"))) ? mD.GetInt32(mD.GetOrdinal("tenor")) : 0;
            xTxn.cicilanPokok = (!mD.IsDBNull(mD.GetOrdinal("cicilanPokok"))) ? mD.GetDecimal(mD.GetOrdinal("cicilanPokok")) : 0;
            xTxn.bunga = (!mD.IsDBNull(mD.GetOrdinal("bunga"))) ? mD.GetDecimal(mD.GetOrdinal("bunga")) : 0;
            xTxn.txnStatus = (!mD.IsDBNull(mD.GetOrdinal("txnStatus"))) ? mD.GetString(mD.GetOrdinal("txnStatus")) : null;
            xTxn.tujuan = (!mD.IsDBNull(mD.GetOrdinal("tujuan"))) ? mD.GetString(mD.GetOrdinal("tujuan")) : null;
            xTxn.notes = (!mD.IsDBNull(mD.GetOrdinal("notes"))) ? mD.GetString(mD.GetOrdinal("notes")) : null;

            xTxn.anggotaName = (!mD.IsDBNull(mD.GetOrdinal("anggotaName"))) ? mD.GetString(mD.GetOrdinal("anggotaName")) : null;
            xTxn.anggotaNIP = (!mD.IsDBNull(mD.GetOrdinal("anggotaNIP"))) ? mD.GetString(mD.GetOrdinal("anggotaNIP")) : null;
            xTxn.compName = (!mD.IsDBNull(mD.GetOrdinal("compName"))) ? mD.GetString(mD.GetOrdinal("compName")) : null;
            xTxn.locName = (!mD.IsDBNull(mD.GetOrdinal("locName"))) ? mD.GetString(mD.GetOrdinal("locName")) : null;
            xTxn.packetName = (!mD.IsDBNull(mD.GetOrdinal("packetName"))) ? mD.GetString(mD.GetOrdinal("packetName")) : null;
            xTxn.description = (!mD.IsDBNull(mD.GetOrdinal("description"))) ? mD.GetString(mD.GetOrdinal("description")) : null;
            xTxn.empTypeID = (!mD.IsDBNull(mD.GetOrdinal("empTypeID"))) ? mD.GetString(mD.GetOrdinal("empTypeID")) : null;

            return xTxn;
        }

        #endregion
    }
}
