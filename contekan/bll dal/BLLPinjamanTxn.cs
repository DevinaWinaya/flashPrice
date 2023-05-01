
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using KopkariFX.Quota.QuotaDetail;
using KopkariFX.Quota.QuotaHeader;
using HCPLUSFx;
using KopkariFX.PinjamanTxn.CicilanPinjaman;
using KopkariFX.PinjamanTxn.PinjamanStatus;
using KopkariFX.DokumenNotes;
using KopkariFX.Anggota;
using KopkariFX.Class;
using HCPLUSFx.DAL;
using KopkariFX.EmailFX;
using KopkariFX.PinjamanTxn.AlasanPinjaman;
using KopkariFX;
using KopkariFX.Super;

namespace KopkariFX.PinjamanTxn
{
    public class BLLPinjamanTxn
    {
        #region calc and validation
        //public static decimal calcPinjamanAmount(String userID)
        //{
        //    decimal xAmount = 0;

        //    BOPinjamanTxnList xTxnList = DBPinjamanTxn.getListPinjamanAnggota(userID, "", "");

        //    if (xTxnList == null)
        //    {
        //        return xAmount;
        //    }

        //    for (var i = 0; i < xTxnList.Count; i++)
        //    {
        //        xAmount += xTxnList[i].amount;
        //    }

        //    return xAmount;
        //}

        public static decimal calcSisaPlafon(String userID)
        {
            BOAnggota xAnggota = BLLAnggota.getContent(userID);
            BOClass xClass = BLLClass.getContent(xAnggota.classID);
            decimal xTotal = DBPinjamanTxn.getTotalPinjamanUser(userID);
            decimal xPokokTerbayar = DBCicilanPinjaman.getListCicilanPokokTerbayarAnggota(userID);

            return xClass.plafonPinjaman - (xTotal - xPokokTerbayar);
        }

        public static bool isExceedLimitGol(decimal xAmount, String userID)
        {
            if ((calcSisaPlafon(userID) - xAmount) >= 0)
                return false;

            return true;
        }
        #endregion

        #region getTotalPinjamanUser
        public static decimal getTotalPinjamanUser(String userID)
        {
            return DBPinjamanTxn.getTotalPinjamanUser(userID);
        }
        #endregion

        #region getLastContent

        public static BOPinjamanTxn getLastContent()
        {
            return DBPinjamanTxn.getLastPinjaman();
        }
        #endregion

        #region getListPinjamanAnggota
        public static BOPinjamanTxnList getListPinjamanAnggota(
            String userID,
            String sortBy,
            String sortDir)
        {
            return DBPinjamanTxn.getListPinjamanAnggota(userID, sortBy, sortDir);
        }
        #endregion

        #region getListPinjamanGridAnggota
        public static BOPinjamanTxnList getListPinjamanGridAnggota(
            String userID,
            String sortBy,
            String sortDir)
        {
            return DBPinjamanTxn.getListPinjamanGridAnggota(userID, sortBy, sortDir);
        }
        #endregion


        #region Generate Pinjaman No
        public static string generatePNJNomor(String pinjamanID)
        {
            String pnjNomor;
            BOPinjamanTxn xTxn = DBPinjamanTxn.getPinjaman(pinjamanID);

            if (xTxn.pinjamanNo == null)
            {
                pnjNomor = DBPinjamanTxn.generatePinjamanNo(xTxn);
            }

            else
            {
                pnjNomor = xTxn.pinjamanNo;
            }

            return pnjNomor;
        }

        #endregion


        public static BOPinjamanTxnList getListxPinjamanUser(
               String userID,
               String hrdNIP,
               DateTime? periodeStart, DateTime? periodeEnd,
               String sortBy,
               String sortDir)
        {
            return DBPinjamanTxn.getListPinjamanGridAnggota(userID, hrdNIP, periodeStart, periodeEnd, sortBy, sortDir);
        }

        public static BOPinjamanTxnList getListxPinjamanUser(
               String userID,
               String hrdNIP,
               DateTime? periodeStart, DateTime? periodeEnd,
               String txnStatus,
               String sortBy,
               String sortDir)
        {
            return DBPinjamanTxn.getListPinjamanGridAnggota(userID, hrdNIP, periodeStart, periodeEnd, txnStatus, sortBy, sortDir);
        }

        public static void addDataPinjamanHeader(BOPinjamanTxn xPinjaman)
        {
            DBPinjamanTxn.addDataPinjamanHeader(xPinjaman);
        }

        #region getListPinjaman
        public static BOPinjamanTxnList getListxPinjaman(
            String xCompanyList,
            String xLocationList,
            DateTime? periodeStart, DateTime? periodeEnd,
            String simpType,
            String searchCrit,
            String searchVal,
            String txnStatus,
            String sortBy,
            String sortDir)
        {
            return DBPinjamanTxn.getListxPinjaman(xCompanyList, xLocationList, periodeStart, periodeEnd, simpType, searchCrit, searchVal, txnStatus, sortBy, sortDir);
        }

        public static BOPinjamanTxnList getListPinjamanforAtasan(
           String xCompanyList,
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
            return DBPinjamanTxn.getListPinjamanforAtasan(xCompanyList, xLocationList, periodeStart, periodeEnd, simpType, searchCrit, searchVal, txnStatus, sortBy, sortDir, userNIP);
        }


        public static BOPinjamanTxnList getListMultipleStatus(
            String xCompanyList,
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
            return DBPinjamanTxn.getListMultipleStatus(xCompanyList, xLocationList, periodeStart, periodeEnd, simpType, searchCrit, searchVal, txnStatus1, txnStatus2, sortBy, sortDir);
        }
        #endregion

        #region getContent
        public static BOPinjamanTxn getContent(string txnID)
        {
            return DBPinjamanTxn.getPinjaman(txnID);
        }

        #endregion

        #region getLoadTxn
        public static BOPinjamanTxn getLoadTxn(string txnID)
        {
            return DBPinjamanTxn.getLoadTxn(txnID);
        }

        #endregion

        public static DataSet getPinjamanPrint(string pinjamanID, string userID)
        {
            return DBPinjamanTxn.getPinjamanPrint(pinjamanID, userID);
        }

        #region savePinjaman
        public static BOProcessResult savePinjaman(BOPinjamanTxn xPinjaman, BOQuotaDetail xTxnDetail, BOQuotaHeader xTxnHeader, BOPinjamanStatus xStatus)
        {
            return DBPinjamanTxn.saveTxnPinjaman(xPinjaman, xTxnDetail, xTxnHeader, xStatus);
        }


        public static BOProcessResult savePinjamanKhususBatch(BOPinjamanTxnList xPinjaman)
        {
            return DBPinjamanTxn.savePinjamanKhususBatch(xPinjaman);
        }

        #endregion


        #region pengembalian quota setelah penolakan
        public static BOProcessResult QuotaRefund(BOQuotaDetail xTxnDetail, BOQuotaHeader xTxnHeader, BOPinjamanStatus xStatus)
        {
            return DBPinjamanTxn.QuotaRefund(xTxnDetail, xTxnHeader, xStatus);
        }

        #endregion

        #region update dokumen lengkap
        public static string updateDokumenLengkap(BOPinjamanTxn xTxn)
        {
            string retVal = "";

            if (retVal == "")
            {
                retVal = DBPinjamanTxn.updateDokumenLengkap(xTxn);
                if (retVal.Contains("EXCEPTION!"))
                {
                    retVal = "Error saat updateDokumenLengkap " + retVal;
                }
            }
            else
            {
                retVal = "Error saat Saving updateDokumenLengkap " + retVal;
            }
            return retVal;
        }
        #endregion

        #region emailing

        public static string[] paramMailDokumenBelumLengkap = {
                "[@empID]","[@anggotaName]","[@tanggal]","[@pinjamanNo]","[@amount]","[@tenor]"
                ,"[@txnDate]","[@KKNotes]","[@KTPNotes]","[@KTPPenjaminNotes]","[@NameTagNotes]"
                ,"[@SJPNotes]","[@SPPHal1Notes]","[@SPPHal2Notes]","[@adminNotes]"
                ,"[@kopkariURL]"
        };


        #region email dokumen belum lengkap (dari halaman cek dokumen)
        public static string emailDokumenBelumLengkap(BOPinjamanTxn xTxn, BODokumenNotesList xNotes, String url, bool reminder)
        {

            string retVal = "";
            string xMailTemplateID = "PNJ_mailDokumenBelumLengkap";
            object[] paramValueDokumenBelumLengkap;
            BOAnggota xAnggota = BLLAnggota.getContent(xTxn.userID);


            BOEmail xMail = null;
            StringBuilder tableFormat = new StringBuilder();
            try
            {
                paramValueDokumenBelumLengkap = new object[] {
                      xTxn.anggotaNIP ,xTxn.anggotaName, DateTime.Now.ToString(), xTxn.pinjamanNo, xTxn.amount, xTxn.tenor, xTxn.txnDate, xNotes[0].lastFeedBack, xNotes[1].lastFeedBack, xNotes[2].lastFeedBack, xNotes[3].lastFeedBack, xNotes[4].lastFeedBack, xNotes[5].lastFeedBack, xNotes[6].lastFeedBack, xTxn.notes, url
                        };

                xMail = BLLEmail.getMailContent(xMailTemplateID, paramMailDokumenBelumLengkap, paramValueDokumenBelumLengkap);
                if (xMail != null)
                {
                    xMail.mailFrom = System.Configuration.ConfigurationManager.AppSettings["defaultEmailSender"];

                    xMail.mailTo = "try.hartono@kawanlamaretail.com";
                    /*BLLEmail.addEmailAddress(xMail.mailTo, xAnggota.email);*/

                    xMail.mailCC = xMail.defaultCC;
                    xMail.mailBCC = xMail.defaultBCC;

                    //////xMail.mailBody += "<br />" + xMail.mailTo
                    //////    + "<br />" + xMail.mailCC
                    //////    + "<br />" + xMail.mailBCC;
                    //////xMail.mailTo = "michael.widjaja@kawanlamaretail.com";
                    //xMail.mailCC = "michael.widjaja@kawanlamaretail.com";
                    //////xMail.mailBCC = "michael.widjaja@kawanlamaretail.com";

                    if (reminder == true)
                    {
                        xMail.mailSubject = "Reminder: " + xMail.mailSubject;
                    }
                    else
                    {
                        xMail.mailSubject = xMail.mailSubject;
                    }

                    xMail.mailAttach = null;
                    xMail.mailAttachName = "";
                    xMail.scheduleSend = DateTime.Now;
                    xMail.failDelay = 120;
                    xMail.maxTry = 5;
                    xMail.isAddIDInSubject = true;
                    xMail.lastOperator = "BLLPinjamanTxn";

                    retVal = BLLEmail.postMailQueueToServer(xMail);//queue email nya                            
                }
                else
                {
                    retVal = "EXCEPTION:BLLBonInsPotImport:emailToApprover:Mail Template:" + xMailTemplateID + " tidak ditemukan ";
                }
            }
            catch (Exception ex)
            {
                retVal = "EXCEPTION:BLLBonInsPotImport:emailToApprover:" + ex.Message;
            }

            return retVal;
        }
        #endregion


        #region email approval analis kredit inform ke atasan (dari halaman approval analis kredit)
        public static BOProcessResult informPinjamanToAtasanEmail(string xTxn, bool reminder)
        {
            BOProcessResult retVal = new BOProcessResult();

            BOPinjamanTxn xPNJ = BLLPinjamanTxn.getContent(xTxn);

            string xMailTemplateID = "PNJ_emailInformasiKeAtasan";


            retVal = emailPNJToDestination(xPNJ, xMailTemplateID, reminder);

            return retVal;
        }


        #endregion



        #region general method


        private static BOEmail emailGetTemplateAndContent(
            BOPinjamanTxn xPinjamanHeader
            , string xMailTemplateID
            )
        {
            BOEmail xMail = null;

            string[] paramEmail = {
                "[@anggotaNIP]",
                "[@anggotaName]",

                "[@KTPAddress]",
                "[@phone1]",
                "[@locName]",
                "[@pinjamanNo]",
                "[@txnDate]",
                "[@amount]",
                "[@tenor]",
                "[@lastUpdate]",
                "[@tujuan]",
                "[@kopkariURL]"
                //buat tampungan tanggal berakhir aktivitas. 

        };

            BOAnggota xAnggota = BLLAnggota.getContent(xPinjamanHeader.userID);
            BOAlasan xAlasan = BLLAlasan.getContent(xPinjamanHeader.alasanID);
            BOSuper xSuperior = BLLSuper.getContent(xAnggota.anggotaNIP);
            BOAnggota xSuperiorAng = BLLAnggota.getContentByNIP(xSuperior.superiorID);

            DateTime xLastUpdateConverted = xPinjamanHeader.lastUpdate ?? DateTime.Now;

            object[] paramValue = new object[] {
                xPinjamanHeader.anggotaNIP,
                xPinjamanHeader.anggotaName,
                xSuperiorAng.anggotaName,
                xAnggota.KTPAddress,
                xAnggota.phone1,
                xAnggota.locName,
                xPinjamanHeader.pinjamanNo,
                DateTimeExtender.toLongIndoDateTime(xPinjamanHeader.txnDate),
                xPinjamanHeader.amount.ToString("#,0"),
                xPinjamanHeader.tenor,
                DateTimeExtender.toLongIndoDateTime(xLastUpdateConverted),
                (xAlasan.alasanID == "A0023" ? xPinjamanHeader.tujuan : xAlasan.description),
                "kopkari-no-reply@klgsys.com"
                
                //,(xBODetail!=null ? ( xBODetail.endDate.HasValue? xBODetail.endDate.Value.toIndoDate() :"" ) : "") --> kalo null begini caranya
        };

            xMail = BLLEmail.getMailContent(xMailTemplateID, paramEmail, paramValue);
            return xMail;
        }




        private static BOProcessResult emailPNJToDestination( // inform ke atasan dari analis kredit
            BOPinjamanTxn xTxn
            , string xMailTemplateID
            , bool reminder)
        {

            BOProcessResult xBOReturnResult = new BOProcessResult("BLLPinjamanTxn.emailPNJToDestination." + xMailTemplateID);

            BOEmail xMail = null;
            StringBuilder tableFormat = new StringBuilder();


            try
            {
                xMail = emailGetTemplateAndContent(xTxn, xMailTemplateID);

                if (xMail != null)
                {
                    xMail.mailFrom = System.Configuration.ConfigurationManager.AppSettings["defaultEmailSender"];
                    xMail.mailCC = xMail.defaultCC;
                    xMail.mailBCC = xMail.defaultBCC;

                    BOAnggota xAnggota = BLLAnggota.getContent(xTxn.userID);

                    xMail.mailTo = xAnggota.email;

                    /* gk ngerti buat apa
                    foreach (BOEmployee xDesti in xAdditionalRecipient ?? new BOEmployeeList())
                    {
                        xMail.mailCC = BLLEmail.addEmailAddress(xMail.mailCC, xDesti.EmpEmail);
                    }
                    */


                    bool xIsDebug = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["isThisServerIsDebugging"]);

                    if (xIsDebug)
                    {
                        xMail.mailBody += "<br />To:" + xMail.mailTo
                            + "<br />CC:" + xMail.mailCC
                            + "<br />BCC:" + xMail.mailBCC;


                        xMail.mailTo = "try.hartono@kawanlamaretail.com , michael.widjaja@kawanlamaretail.com";
                        xMail.mailCC = "";
                        xMail.mailBCC = "";
                    }

                    if (reminder == true)
                    {
                        xMail.queueType = BOEmail.emailQueueType.Reminder;
                        xMail.mailSubject = "Reminder: " + xMail.mailSubject;
                    }
                    else
                    {
                        //paling cepet priority2
                        xMail.queueType = BOEmail.emailQueueType.Priority2;
                        xMail.mailSubject = xMail.mailSubject;
                    }

                    xMail.mailAttach = null;
                    xMail.mailAttachName = "";
                    xMail.scheduleSend = DateTime.Now;
                    xMail.failDelay = 120;
                    xMail.maxTry = 5;
                    xMail.isAddIDInSubject = true;
                    xMail.lastOperator = "KOPKARI_BLLPinjamanTxn";

                    string xRetVal = BLLEmail.postMailQueueToServer(xMail);//queue email nya  

                    if (!xRetVal.Contains("EXCEPTION"))
                    {
                        xBOReturnResult.isSuccess = true;
                        xBOReturnResult.xStatusCode = "200";
                        xBOReturnResult.xDataObject = xRetVal;  //dapetin mailID nya. 
                    }
                }
                else
                {
                    xBOReturnResult.xMessage = ("Mail Template:" + xMailTemplateID + " tidak ditemukan ");
                }
            }
            catch (Exception ex)
            {
                xBOReturnResult.xMessage = ex.Message;
            }
            return xBOReturnResult;

        }

        #endregion


        #endregion

        #region getListPrint
        public static DataTable getListPrint(
            String xCompanyList,
            String xLocationList,
            DateTime? periodeStart, DateTime? periodeEnd,
            String simpType,
            String searchCrit,
            String searchVal,
            String txnStatus,
            String sortBy,
            String sortDir
            )
        {
            BOPinjamanTxnList xList = getListxPinjaman(xCompanyList, xLocationList, periodeStart, periodeEnd, simpType, searchCrit, searchVal, txnStatus, sortBy, sortDir);

            if (xList == null)
            {
                // kalo kosong
                xList = new BOPinjamanTxnList();
            }

            DataTable dt = DataTableExtender.ToDataTable(xList);

            dt.Columns["pinjamanID"].SetOrdinal(0);
            dt.Columns["PinjamanID"].ColumnName = "ID Pinjaman";

            dt.Columns["anggotaNIP"].SetOrdinal(1);
            dt.Columns["anggotaNIP"].ColumnName = "NIP Anggota";

            dt.Columns["anggotaName"].SetOrdinal(2);
            dt.Columns["anggotaName"].ColumnName = "Nama Anggota";

            dt.Columns["packetName"].SetOrdinal(3);
            dt.Columns["packetName"].ColumnName = "Nama Paket";

            dt.Columns["amount"].SetOrdinal(4);
            dt.Columns["amount"].ColumnName = "Nominal";

            dt.Columns.Add("txnDateProcessed").SetOrdinal(5);
            dt.Columns["txnDateProcessed"].ColumnName = "Tanggal Pengajuan";
            dt.Columns["Tanggal Pengajuan"].DataType = typeof(String);

            foreach (DataRow dr in dt.Rows)
            {
                string dateTime = DateTime.Parse((dr["txnDate"].ToString())).ToString("dd/MM/yyyy");
                dr["Tanggal Pengajuan"] = dateTime;
            }

            dt.Columns["jobTitleName"].SetOrdinal(6);
            dt.Columns["jobTitleName"].ColumnName = "Jabatan";

            dt.Columns["statusName"].SetOrdinal(7);
            dt.Columns["statusName"].ColumnName = "Status Transaksi";

            dt.Columns.Remove("pinjamanNo");
            dt.Columns.Remove("userID");
            dt.Columns.Remove("classID");
            dt.Columns.Remove("cicilanPokok");
            dt.Columns.Remove("typeID");
            dt.Columns.Remove("tujuan");
            dt.Columns.Remove("compID");
            dt.Columns.Remove("locID");
            dt.Columns.Remove("jobTitleID");
            dt.Columns.Remove("jmlCicil");
            dt.Columns.Remove("orgID");
            dt.Columns.Remove("hrdID");
            dt.Columns.Remove("alasanID");
            dt.Columns.Remove("txnOrigin");
            dt.Columns.Remove("pencairanDate");
            dt.Columns.Remove("ID");
            dt.Columns.Remove("name_ID");
            dt.Columns.Remove("lastOperator");
            dt.Columns.Remove("entryDate");
            dt.Columns.Remove("isMarkToDelete");
            dt.Columns.Remove("historyData");
            dt.Columns.Remove("isLocked");
            dt.Columns.Remove("lockedDate");
            dt.Columns.Remove("deletedDate");
            dt.Columns.Remove("deletedReason");
            dt.Columns.Remove("urutanID");
            dt.Columns.Remove("txnStatus");
            dt.Columns.Remove("empTypeID");
            dt.Columns.Remove("tenor");
            dt.Columns.Remove("bunga");
            dt.Columns.Remove("packetID");
            dt.Columns.Remove("notes");
            dt.Columns.Remove("compName");
            dt.Columns.Remove("locName");
            dt.Columns.Remove("jobLevelID");
            dt.Columns.Remove("superiorID");
            dt.Columns.Remove("bpuID");
            dt.Columns.Remove("description");
            dt.Columns.Remove("txnRequestFrom");
            dt.Columns.Remove("pelunasanDate");
            dt.Columns.Remove("Name");
            dt.Columns.Remove("entryOperator");
            dt.Columns.Remove("lastUpdate");
            dt.Columns.Remove("isNewRecord");
            dt.Columns.Remove("isSavedRecord");
            dt.Columns.Remove("isActive");
            dt.Columns.Remove("lockedBy");
            dt.Columns.Remove("isDeleted");
            dt.Columns.Remove("deletedOperator");
            dt.Columns.Remove("isDocComplete");
            dt.Columns.Remove("hrdNIP");
            dt.Columns.Remove("txnStatusPNJStyle");
            dt.Columns.Remove("superiorName");
            dt.Columns.Remove("statusByAnggota");
            dt.Columns.Remove("superSuperiorID");
            dt.Columns.Remove("superSuperiorName");
            dt.Columns.Remove("cicilanTotal");
            dt.Columns.Remove("isAllowAutoBypassSuperior");
            dt.Columns.Remove("isAllowAutoBypassAnalystCredit");
            dt.Columns.Remove("analystCreditMethod");
            dt.Columns.Remove("amountPaid");
            dt.Columns.Remove("tenorPaid");
            dt.Columns.Remove("sisaPokok");
            dt.Columns.Remove("amountMustPay");
            dt.Columns.Remove("txnDate");

            return dt;
        }
        #endregion


        #region ketuaKoperasi
        public static BOPinjamanTxnList getListOnKetua(
           String xCompanyList,
           String xLocationList,
           DateTime? periodeStart, DateTime? periodeEnd,
           String simpType,
           String searchCrit,
           String searchVal,
           String txnStatus,
           String packetName,
           String sortBy,
           String sortDir)
        {
            return DBPinjamanTxn.getListOnKetua(xCompanyList, xLocationList, periodeStart, periodeEnd, simpType, searchCrit, searchVal, txnStatus, packetName, sortBy, sortDir);
        }

        #region getListPrint
        public static DataTable getListPrintOnKetua(
            String xCompanyList,
            String xLocationList,
            DateTime? periodeStart, DateTime? periodeEnd,
            String simpType,
            String searchCrit,
            String searchVal,
            String txnStatus,
            String packetID,
            String sortBy,
            String sortDir
            )
        {
            BOPinjamanTxnList xList = getListOnKetua(xCompanyList, xLocationList, periodeStart, periodeEnd, simpType, searchCrit, searchVal, txnStatus, packetID, sortBy, sortDir);

            if (xList == null)
            {
                // kalo kosong
                xList = new BOPinjamanTxnList();
            }

            DataTable dt = DataTableExtender.ToDataTable(xList);

            dt.Columns["pinjamanID"].SetOrdinal(0);
            dt.Columns["PinjamanID"].ColumnName = "ID Pinjaman";

            dt.Columns["anggotaNIP"].SetOrdinal(1);
            dt.Columns["anggotaNIP"].ColumnName = "NIP Anggota";

            dt.Columns["anggotaName"].SetOrdinal(2);
            dt.Columns["anggotaName"].ColumnName = "Nama Anggota";

            dt.Columns["packetName"].SetOrdinal(3);
            dt.Columns["packetName"].ColumnName = "Nama Paket";

            dt.Columns["amount"].SetOrdinal(4);
            dt.Columns["amount"].ColumnName = "Nominal";

            dt.Columns.Add("txnDateProcessed").SetOrdinal(5);
            dt.Columns["txnDateProcessed"].ColumnName = "Tanggal Pengajuan";
            dt.Columns["Tanggal Pengajuan"].DataType = typeof(String);

            foreach (DataRow dr in dt.Rows)
            {
                string dateTime = DateTime.Parse((dr["txnDate"].ToString())).ToString("dd/MM/yyyy");
                dr["Tanggal Pengajuan"] = dateTime;
            }

            dt.Columns["jobTitleName"].SetOrdinal(6);
            dt.Columns["jobTitleName"].ColumnName = "Jabatan";

            dt.Columns["statusName"].SetOrdinal(7);
            dt.Columns["statusName"].ColumnName = "Status Transaksi";

            dt.Columns.Remove("pinjamanNo");
            dt.Columns.Remove("userID");
            dt.Columns.Remove("classID");
            dt.Columns.Remove("cicilanPokok");
            dt.Columns.Remove("typeID");
            dt.Columns.Remove("tujuan");
            dt.Columns.Remove("compID");
            dt.Columns.Remove("locID");
            dt.Columns.Remove("jobTitleID");
            dt.Columns.Remove("jmlCicil");
            dt.Columns.Remove("orgID");
            dt.Columns.Remove("hrdID");
            dt.Columns.Remove("alasanID");
            dt.Columns.Remove("txnOrigin");
            dt.Columns.Remove("pencairanDate");
            dt.Columns.Remove("ID");
            dt.Columns.Remove("name_ID");
            dt.Columns.Remove("lastOperator");
            dt.Columns.Remove("entryDate");
            dt.Columns.Remove("isMarkToDelete");
            dt.Columns.Remove("historyData");
            dt.Columns.Remove("isLocked");
            dt.Columns.Remove("lockedDate");
            dt.Columns.Remove("deletedDate");
            dt.Columns.Remove("deletedReason");
            dt.Columns.Remove("urutanID");
            dt.Columns.Remove("txnStatus");
            dt.Columns.Remove("empTypeID");
            dt.Columns.Remove("tenor");
            dt.Columns.Remove("bunga");
            dt.Columns.Remove("packetID");
            dt.Columns.Remove("notes");
            dt.Columns.Remove("compName");
            dt.Columns.Remove("locName");
            dt.Columns.Remove("jobLevelID");
            dt.Columns.Remove("superiorID");
            dt.Columns.Remove("bpuID");
            dt.Columns.Remove("description");
            dt.Columns.Remove("txnRequestFrom");
            dt.Columns.Remove("pelunasanDate");
            dt.Columns.Remove("Name");
            dt.Columns.Remove("entryOperator");
            dt.Columns.Remove("lastUpdate");
            dt.Columns.Remove("isNewRecord");
            dt.Columns.Remove("isSavedRecord");
            dt.Columns.Remove("isActive");
            dt.Columns.Remove("lockedBy");
            dt.Columns.Remove("isDeleted");
            dt.Columns.Remove("deletedOperator");
            dt.Columns.Remove("isDocComplete");
            dt.Columns.Remove("hrdNIP");
            dt.Columns.Remove("txnStatusPNJStyle");
            dt.Columns.Remove("superiorName");
            dt.Columns.Remove("statusByAnggota");
            dt.Columns.Remove("superSuperiorID");
            dt.Columns.Remove("superSuperiorName");
            dt.Columns.Remove("cicilanTotal");
            dt.Columns.Remove("isAllowAutoBypassSuperior");
            dt.Columns.Remove("isAllowAutoBypassAnalystCredit");
            dt.Columns.Remove("analystCreditMethod");
            dt.Columns.Remove("amountPaid");
            dt.Columns.Remove("tenorPaid");
            dt.Columns.Remove("sisaPokok");
            dt.Columns.Remove("amountMustPay");
            dt.Columns.Remove("txnDate");

            return dt;
        }
        #endregion



        #endregion


        #region upload attribute dari excel
        public static string uploadAttribute(DataTable dtUpload, string lastOperator, out DataTable result, string fileName)
        {
            return DBPinjamanTxn.uploadAttribute(dtUpload, lastOperator, out result, fileName);
        }
        #endregion



        #region pengajuan from anggota


        public static BOProcessResult pengajuanPinjamanFromAnggota(BOPinjamanTxn myHEAD)
        {
            return DBPinjamanTxn.pengajuanPinjamanFromAnggota(myHEAD);
        }

        #endregion


        #region analis kredit

        public static BOProcessResult approveRejectAnalisKredit(string xPinjamanID, bool xIsApproveReject, string xApproveNotes, string xLastOperator)
        {
            ///logic:
            ///get datanya. klo statusnya valid baru lanjut. 
            ///approve = PSA005
            ///reject = PSA006
            ///klo reject harus balikin quota. 

            BOProcessResult xBOReturnResult = new BOProcessResult("BLLPinjamanTxn.approveRejectAnalisKredit");

            xBOReturnResult.xInnerProcessResult = DBPinjamanTxn.approveRejectAnalisKredit(xPinjamanID, xIsApproveReject, xApproveNotes, xLastOperator);

            if (xBOReturnResult.xInnerProcessResult.isSuccess)
            {
                xBOReturnResult.isSuccess = true;
                xBOReturnResult.xStatusCode = "200";
                xBOReturnResult.xDataObject = xPinjamanID;

                BOProcessResult xResultEmail = null;
                //klo approve..harus kirim email ke atasan
                if (xIsApproveReject == true)
                {
                    xResultEmail = BLLPinjamanTxn.emailWhenAnalisApproveToSuperior(xPinjamanID, false);
                }
                else
                {
                    xResultEmail = BLLPinjamanTxn.emailWhenAnalisRejectToAnggota(xPinjamanID, false);
                }
            }


            return xBOReturnResult;



        }



        #endregion





        #region emailing lengkap




        #region general method

        private static BOEmail emailGetTemplateAndContent(
            BOPinjamanTxn xBOHeader
            , string xMailTemplateID
            , BOAnggotaList xAdditionalRecipient  //buat tambahan orang diluar superior & supersuperior.             
            )
        {
            BOEmail xMail = null;

            string xRecipientID = "", xRecipientName = "";
            string xCCID = "", xCCName;

            BOAnggota xAnggota = BLLAnggota.getContent(xBOHeader.userID);

            xRecipientID = xAnggota.anggotaNIP;
            xRecipientName = xAnggota.anggotaName;

            if (xAdditionalRecipient != null && xAdditionalRecipient.Count > 0)
            {
                xCCID = xAdditionalRecipient[0].anggotaNIP;
                xCCName = xAdditionalRecipient[0].anggotaName;
            }

            string[] paramMailApprover = {
"[@pinjamanID]",
"[@pinjamanNo]",
"[@amount]",
"[@txnStatus]",
"[@statusName]",
"[@txnDate]",
"[@userID]",
"[@anggotaNIP]",
"[@anggotaName]",
"[@empTypeID]",
"[@classID]",
"[@tenor]",
"[@cicilanPokok]",
"[@bunga]",
"[@typeID]",
"[@packetID]",
"[@tujuan]",
"[@isDocComplete]",
"[@notes]",
"[@compID]",
"[@compName]",
"[@locID]",
"[@locName]",
"[@jobTitleID]",
"[@jobTitleName]",
"[@jmlCicil]",
"[@jobLevelID]",
"[@orgID]",
"[@superiorID]",
"[@superiorName]",
"[@hrdID]",
"[@bpuID]",
"[@alasanID]",
"[@description]",
"[@txnOrigin]",
"[@txnRequestFrom]",
"[@pencairanDate]",
"[@pelunasanDate]",
"[@hrdNIP]",
"[@packetName]",
"[@statusByAnggota]",
"[@superSuperiorID]",
"[@superSuperiorName]",
"[@entryDate]",
"[@entryOperator]",
"[@lastUpdate]",
"[@lastOperator]"
,"[@recipientID]"
,"[@recipientName]"
        };
            object[] paramValue = new object[] {
                xBOHeader.pinjamanID,
xBOHeader.pinjamanNo,
xBOHeader.amount,
xBOHeader.txnStatus,
xBOHeader.statusName,
xBOHeader.txnDate,
xBOHeader.userID,
xBOHeader.anggotaNIP,
xBOHeader.anggotaName,
xBOHeader.empTypeID,
xBOHeader.classID,
xBOHeader.tenor,
xBOHeader.cicilanPokok,
xBOHeader.bunga,
xBOHeader.typeID,
xBOHeader.packetID,
xBOHeader.tujuan,
xBOHeader.isDocComplete,
xBOHeader.notes,
xBOHeader.compID,
xBOHeader.compName,
xBOHeader.locID,
xBOHeader.locName,
xBOHeader.jobTitleID,
xBOHeader.jobTitleName,
xBOHeader.jmlCicil,
xBOHeader.jobLevelID,
xBOHeader.orgID,
xBOHeader.superiorID,
xBOHeader.superiorName,
xBOHeader.hrdID,
xBOHeader.bpuID,
xBOHeader.alasanID,
xBOHeader.description,
xBOHeader.txnOrigin,
xBOHeader.txnRequestFrom,
xBOHeader.pencairanDate,
xBOHeader.pelunasanDate,
xBOHeader.hrdNIP,
xBOHeader.packetName,
xBOHeader.statusByAnggota,
xBOHeader.superSuperiorID,
xBOHeader.superSuperiorName,
xBOHeader.entryDate,
xBOHeader.entryOperator,
xBOHeader.lastUpdate,
xBOHeader.lastOperator,
xRecipientID ,
xRecipientName
                        };

            xMail = BLLEmail.getMailContent(xMailTemplateID, paramMailApprover, paramValue);

            return xMail;
        }


        private static BOProcessResult emailPNJToDestination(
            BOPinjamanTxn xBOHeader
            , string xMailTemplateID
            , BOAnggota xPrimaryRecipient
            , BOAnggotaList xAdditionalRecipient
            , bool reminder)
        {
            BOProcessResult xBOReturnResult = new BOProcessResult("BLLPinjamanTxn.emailIDPToDestination." + xMailTemplateID);


            BOEmail xMail = null;
            StringBuilder tableFormat = new StringBuilder();
            try
            {
                xMail = emailGetTemplateAndContent(xBOHeader, xMailTemplateID, xAdditionalRecipient);
                if (xMail != null)
                {
                    xMail.mailFrom = System.Configuration.ConfigurationManager.AppSettings["defaultEmailSender"];
                    xMail.mailCC = xMail.defaultCC;
                    xMail.mailBCC = xMail.defaultBCC;


                    xMail.mailTo = xPrimaryRecipient.email;

                    foreach (BOAnggota xDesti in xAdditionalRecipient ?? new BOAnggotaList())
                    {
                        xMail.mailCC = BLLEmail.addEmailAddress(xMail.mailCC, xDesti.email);
                    }

                    bool xIsDebug = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["isThisServerIsDebugging"]);

                    if (xIsDebug)
                    {
                        xMail.mailBody += "<br />To:" + xMail.mailTo
                            + "<br />CC:" + xMail.mailCC
                            + "<br />BCC:" + xMail.mailBCC;
                        xMail.mailTo = "michael.widjaja@kawanlamaretail.com , try.hartono@kawanlamaretail.com ";
                        xMail.mailCC = "michael.widjaja@kawanlamaretail.com , try.hartono@kawanlamaretail.com ";
                        xMail.mailBCC = "michael.widjaja@kawanlamaretail.com , try.hartono@kawanlamaretail.com ";
                    }

                    if (reminder == true)
                    {
                        xMail.queueType = BOEmail.emailQueueType.Reminder;
                        xMail.mailSubject = "Reminder: " + xMail.mailSubject;
                    }
                    else
                    {
                        xMail.queueType = BOEmail.emailQueueType.Notification1;
                        xMail.mailSubject = xMail.mailSubject;
                    }

                    xMail.mailAttach = null;
                    xMail.mailAttachName = "";
                    xMail.scheduleSend = DateTime.Now;
                    xMail.failDelay = 120;
                    xMail.maxTry = 5;
                    xMail.isAddIDInSubject = true;
                    xMail.lastOperator = "BLLPinjamanTxn";

                    string xRetVal = BLLEmail.postMailQueueToServer(xMail);//queue email nya  

                    if (!xRetVal.Contains("EXCEPTION"))
                    {
                        xBOReturnResult.isSuccess = true;
                        xBOReturnResult.xStatusCode = "200";
                        xBOReturnResult.xDataObject = xRetVal;  //dapetin mailID nya. 
                    }
                }
                else
                {
                    xBOReturnResult.xMessageList.Add("Mail Template:" + xMailTemplateID + " tidak ditemukan ");
                }
            }
            catch (Exception ex)
            {
                xBOReturnResult.xMessageList.Add(ex.Message);
            }
            return xBOReturnResult;
        }

        #endregion





        #region reminder logic


        #endregion

        /*
        girls , memastikan email di modul pinjaman : 
        analis kredit :
        -. approve ke atasan
        -. reject ke anggota

        atasan:
        -. approve ke anggota utk lengkapin dokument
        -. reject ke anggota 

        admin:
        -. approve -- nothin
        -. reject ke anggota utk lengkapin ulang dokument
        -. drop -- nothin

        BPU pencairan
        -. ke anggota ketika cair.
        */


        #region email on analis kredit

        public static BOProcessResult emailWhenAnalisApproveToSuperior(string xTxnID, bool reminder)
        {
            BOProcessResult xBOReturnResult = new BOProcessResult("BLLPinjamanTxn.emailWhenAnalisApproveToSuperior");

            BOPinjamanTxn xHeader = getContent(xTxnID);
            if (xHeader != null)
            {
                string xTemplateID = "PNJ_emailWhenAnalisApproveToSuperior";

                BOAnggota xPrimaryDesti = BLLAnggota.getContentByNIP(xHeader.superiorID);

                BOAnggotaList xAdditionalList = new BOAnggotaList();
                xAdditionalList.Add(xPrimaryDesti);

                xBOReturnResult.xInnerProcessResult = emailPNJToDestination(xHeader, xTemplateID, xPrimaryDesti, xAdditionalList, false);
                if (xBOReturnResult.xInnerProcessResult.isSuccess)
                {
                    xBOReturnResult.isSuccess = true;
                    xBOReturnResult.xStatusCode = "200";
                    xBOReturnResult.xDataObject = xBOReturnResult.xInnerProcessResult.xDataObject;  //dapetin mailID nya. 
                }
            }
            else
            {
                xBOReturnResult.xMessageList.Add("Txn" + xTxnID + " not found");
            }
            return xBOReturnResult;
        }

        public static BOProcessResult emailWhenAnalisRejectToAnggota(string xTxnID, bool reminder)
        {
            BOProcessResult xBOReturnResult = new BOProcessResult("BLLPinjamanTxn.emailWhenAnalisRejectToAnggota");

            BOPinjamanTxn xHeader = getContent(xTxnID);
            if (xHeader != null)
            {
                string xTemplateID = "PNJ_emailWhenAnalisRejectToAnggota";

                BOAnggota xPrimaryDesti = BLLAnggota.getContentByNIP(xHeader.anggotaNIP);

                xBOReturnResult.xInnerProcessResult = emailPNJToDestination(xHeader, xTemplateID, xPrimaryDesti, null, false);
                if (xBOReturnResult.xInnerProcessResult.isSuccess)
                {
                    xBOReturnResult.isSuccess = true;
                    xBOReturnResult.xStatusCode = "200";
                    xBOReturnResult.xDataObject = xBOReturnResult.xInnerProcessResult.xDataObject;  //dapetin mailID nya. 
                }
            }
            else
            {
                xBOReturnResult.xMessageList.Add("Txn" + xTxnID + " not found");
            }
            return xBOReturnResult;
        }

        #endregion


        #region email on atasan

        public static BOProcessResult emailWhenAtasanApproveToAnggota(string xTxnID, bool reminder)
        {
            BOProcessResult xBOReturnResult = new BOProcessResult("BLLPinjamanTxn.emailWhenAtasanApproveToAnggota");

            BOPinjamanTxn xHeader = getContent(xTxnID);
            if (xHeader != null)
            {
                string xTemplateID = "PNJ_emailWhenAtasanApproveToAnggota";

                BOAnggota xPrimaryDesti = BLLAnggota.getContentByNIP(xHeader.anggotaNIP);

                xBOReturnResult.xInnerProcessResult = emailPNJToDestination(xHeader, xTemplateID, xPrimaryDesti, null, false);
                if (xBOReturnResult.xInnerProcessResult.isSuccess)
                {
                    xBOReturnResult.isSuccess = true;
                    xBOReturnResult.xStatusCode = "200";
                    xBOReturnResult.xDataObject = xBOReturnResult.xInnerProcessResult.xDataObject;  //dapetin mailID nya. 
                }
            }
            else
            {
                xBOReturnResult.xMessageList.Add("Txn" + xTxnID + " not found");
            }
            return xBOReturnResult;
        }


        public static BOProcessResult emailWhenAtasanRejectToAnggota(string xTxnID, bool reminder)
        {
            BOProcessResult xBOReturnResult = new BOProcessResult("BLLPinjamanTxn.emailWhenAtasanRejectToAnggota");

            BOPinjamanTxn xHeader = getContent(xTxnID);
            if (xHeader != null)
            {
                string xTemplateID = "PNJ_emailWhenAtasanRejectToAnggota";

                BOAnggota xPrimaryDesti = BLLAnggota.getContentByNIP(xHeader.anggotaNIP);

                xBOReturnResult.xInnerProcessResult = emailPNJToDestination(xHeader, xTemplateID, xPrimaryDesti, null, false);
                if (xBOReturnResult.xInnerProcessResult.isSuccess)
                {
                    xBOReturnResult.isSuccess = true;
                    xBOReturnResult.xStatusCode = "200";
                    xBOReturnResult.xDataObject = xBOReturnResult.xInnerProcessResult.xDataObject;  //dapetin mailID nya. 
                }
            }
            else
            {
                xBOReturnResult.xMessageList.Add("Txn" + xTxnID + " not found");
            }
            return xBOReturnResult;
        }

        #endregion


        #region email on admin kopkari

        public static BOProcessResult emailWhenAdminRejectToAnggotaLengkapinDokumen(string xTxnID, bool reminder)
        {
            BOProcessResult xBOReturnResult = new BOProcessResult("BLLPinjamanTxn.emailWhenAdminRejectToAnggotaLengkapinDokumen");

            BOPinjamanTxn xHeader = getContent(xTxnID);
            if (xHeader != null)
            {
                string xTemplateID = "PNJ_emailWhenAdminRejectToAnggotaLengkapinDokumen";

                BOAnggota xPrimaryDesti = BLLAnggota.getContentByNIP(xHeader.anggotaNIP);

                xBOReturnResult.xInnerProcessResult = emailPNJToDestination(xHeader, xTemplateID, xPrimaryDesti, null, false);
                if (xBOReturnResult.xInnerProcessResult.isSuccess)
                {
                    xBOReturnResult.isSuccess = true;
                    xBOReturnResult.xStatusCode = "200";
                    xBOReturnResult.xDataObject = xBOReturnResult.xInnerProcessResult.xDataObject;  //dapetin mailID nya. 
                }
            }
            else
            {
                xBOReturnResult.xMessageList.Add("Txn" + xTxnID + " not found");
            }
            return xBOReturnResult;
        }

        #endregion


        #region email on BPU
        public static BOProcessResult emailWhenBPUCairToAnggota(string xTxnID, bool reminder)
        {
            BOProcessResult xBOReturnResult = new BOProcessResult("BLLPinjamanTxn.emailWhenBPUCairToAnggota");

            BOPinjamanTxn xHeader = getContent(xTxnID);
            if (xHeader != null)
            {
                string xTemplateID = "PNJ_emailWhenBPUCairToAnggota";

                BOAnggota xPrimaryDesti = BLLAnggota.getContentByNIP(xHeader.anggotaNIP);

                xBOReturnResult.xInnerProcessResult = emailPNJToDestination(xHeader, xTemplateID, xPrimaryDesti, null, false);
                if (xBOReturnResult.xInnerProcessResult.isSuccess)
                {
                    xBOReturnResult.isSuccess = true;
                    xBOReturnResult.xStatusCode = "200";
                    xBOReturnResult.xDataObject = xBOReturnResult.xInnerProcessResult.xDataObject;  //dapetin mailID nya. 
                }
            }
            else
            {
                xBOReturnResult.xMessageList.Add("Txn" + xTxnID + " not found");
            }
            return xBOReturnResult;
        }


        #endregion

        #endregion


        #region others

        public static void setTxnStatusLabelStyle(System.Web.UI.WebControls.Label label, String statusCode)
        {
            switch (statusCode)
            {
                case "PSA001":
                    label.CssClass = "badge badge-pill badge-danger  col-md-12"; break;

                case "PSA002":
                    label.CssClass = "badge badge-pill badge-secondary  col-md-12"; break;

                case "PSA003":
                    label.CssClass = "badge badge-pill badge-success  col-md-12"; break;

                case "PSA004":
                    label.CssClass = "badge badge-pill badge-danger  col-md-12"; break;

                case "PSA005":
                    label.CssClass = "badge badge-pill badge-success  col-md-12"; break;

                case "PSA006":
                    label.CssClass = "badge badge-pill badge-danger  col-md-12"; break;

                case "PSA007":
                    label.CssClass = "badge badge-pill badge-success  col-md-12"; break;

                case "PSA008":
                    label.CssClass = "badge badge-pill badge-primary  col-md-12"; break;

                case "PSA009":
                    label.CssClass = "badge badge-pill badge-primary  col-md-12"; break;

                case "PSA010":
                    label.CssClass = "badge badge-pill badge-success  col-md-12"; break;

                case "PSA011":
                    label.CssClass = "badge badge-pill badge-warning  col-md-12"; break;

                case "PSA012":
                    label.CssClass = "badge badge-pill badge-warning  col-md-12"; break;

                case "PSA013":
                    label.CssClass = "badge badge-pill badge-success  col-md-12"; break;

                case "PSA014":
                    label.CssClass = "badge badge-pill badge-danger  col-md-12"; break;

                case "PSA015":
                    label.CssClass = "badge badge-pill badge-warning  col-md-12"; break;

                case "PSA016":
                    label.CssClass = "badge badge-pill badge-success  col-md-12"; break;

                case "PSA017":
                    label.CssClass = "badge badge-pill badge-danger  col-md-12"; break;

                case "PSA018":
                    label.CssClass = "badge badge-pill badge-primary  col-md-12"; break;

                case "PSA019":
                    label.CssClass = "badge badge-pill badge-primary  col-md-12"; break;

                case "PSA020":
                    label.CssClass = "badge badge-pill badge-success  col-md-12"; break;

                case "PSA021":
                    label.CssClass = "badge badge-pill badge-success  col-md-12"; break;

                case "PSA022":
                    label.CssClass = "badge badge-pill badge-success  col-md-12"; break;
            }
        }

        #endregion











    }
}
