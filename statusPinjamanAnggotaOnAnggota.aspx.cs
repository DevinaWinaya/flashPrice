using AjaxControlToolkit;
using CrystalDecisions.CrystalReports.Engine;
using HCFx.Other;
using HCPLUSFx;
using KopkariFX.Anggota;
using KopkariFX.DokumenNotes;
using KopkariFX.DokumenPersyaratan;
using KopkariFX.General;
using KopkariFX.MasterDokumen;
using KopkariFX.PinjamanTxn;
using KopkariFX.PinjamanTxn.AlasanDokumen;
using KopkariFX.PinjamanTxn.CicilanPinjaman;
using KopkariFX.PinjamanTxn.PinjamanStatus;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KopkariFX;
using System.Linq;

namespace NEWKOPKARI.anggota
{
    public partial class statusPinjamanAnggotaOnAnggota : System.Web.UI.Page
    {
        internal const string pageAccessID = "PNJ1.2";
        public string downloadTemp;
        private string xUserID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (clsUserAuthen.cekPageSession(Session))
            {
                xUserID = Session["kopkariUserID"].ToString();
                if (!Page.IsPostBack) // yg cuma ke load sekali
                {
                    this.initForm();
                }
            }
        }

        #region validasi akses


        #endregion

        #region init

        private void initForm()
        {
            //confirmBtn.Visible = false;
            //notesTB.Visible = false;
            //txtSearch.Enabled = false;
            txtFilterDateFrom.Text = DateTime.Now.FirstDayOfMonth().toIndoDate();
            txtFilterDateTo.Text = DateTime.Now.LastDayOfMonth().toIndoDate();
            FillGrid(hdSortEx.Value, hdSortDir.Value);
            LoadDDLFilterStatus();
        }

        #endregion

        #region filter
        private void LoadDDLFilterStatus()
        {
            listboxFilterStatus.DataSource = BLLPinjamanStatus.GetAllPinjamanStatus().OrderBy(s => s.entryDate);
            listboxFilterStatus.DataValueField = "statusCode";
            listboxFilterStatus.DataTextField = "statusByAnggota";
            listboxFilterStatus.DataBind();

        }
        protected void txtFilterDateFrom_TextChanged(object sender, EventArgs e)
        {
            FillGrid(hdSortEx.Value, hdSortDir.Value);
        }

        protected void txtFilterDateTo_TextChanged(object sender, EventArgs e)
        {
            FillGrid(hdSortEx.Value, hdSortDir.Value);
        }
        #endregion


        #region update status 
        protected void saveStatusSuratPerjanjianStatus()
        {
            //string xStatusCode = "PSA008";
            printPerjanjian();

            //BOProcessResult retVal = updatePinjamanStatus(xStatusCode);

            //if (retVal.isSuccess)
            //{
            //}

            //else
            //{

            //}
        }
        #endregion


        #region btn click
        protected void closeBtn_click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(updatePanelFormData, typeof(UpdatePanel), "OpenModalDialog", "$('#modelDialogEditData').modal('hide');", true);

            updatePanelFormData.Update();
        }

        protected void showNotesCloseBtn_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(updatePanelShowNotes, typeof(UpdatePanel), "OpenModalDialog", "$('#modelDialogShowNotes').modal('hide');", true);

            updatePanelShowNotes.Update();
        }

        protected void btnDetail_click(object sender, EventArgs e)
        {
            string xTxnID = hidTxnID.Value;

            loadTxn(xTxnID);
        }


        #region print surat pernyataan penjamin

        protected void sppDownloadBtn_click(object sender, EventArgs e)
        {
            printSPP();
        }

        protected void printSPP()
        {
            string pinjamanID = hidTxnID.Value.ToString();
            string userID = Session["kopkariUserID"].ToString();
            DataSet xDataSetToPrint = BLLPinjamanTxn.getPinjamanPrint(pinjamanID, userID);
            BOPinjamanTxn xTxn = BLLPinjamanTxn.getContent(pinjamanID);

            string dayToStr = DateTimeExtender.toDayNameIndo(DateTime.Now);
            string monthToStr = DateTimeExtender.toMonthNameIndo(DateTime.Now);
            DateTime dateNow = DateTime.Now;
            //string amountToStr = DecimalExtender.convertDecimalToString(Math.Round(xTxn.amount));
            //string tenorToStr = DecimalExtender.convertDecimalToString(Math.Round((decimal)xTxn.tenor));
            //decimal jasa = xTxn.bunga / 12;
            //string jasaToStr = DecimalExtenderNEW2.ConvertToWords(jasa.ToString());
            //decimal cclTotal = (Math.Round(xTxn.amount) * jasa / 100) + xTxn.cicilanPokok;
            //string cclTotalToStr = DecimalExtender.convertDecimalToString(Math.Round(cclTotal));

            DataTable dataTambahan = new DataTable();

            dataTambahan.Columns.Add("dayStr", typeof(string));
            dataTambahan.Columns.Add("monthStr", typeof(string));
            dataTambahan.Columns.Add("dateNow", typeof(DateTime));
            //dataTambahan.Columns.Add("amountStr", typeof(string));
            //dataTambahan.Columns.Add("tenorStr", typeof(string));
            //dataTambahan.Columns.Add("jasa", typeof(decimal));
            //dataTambahan.Columns.Add("jasaStr", typeof(string));
            //dataTambahan.Columns.Add("cclTotal", typeof(decimal));
            //dataTambahan.Columns.Add("cclTotalStr", typeof(string));

            dataTambahan.Rows.Add(dayToStr, monthToStr, dateNow);

            xDataSetToPrint.Tables.Add(dataTambahan);

            ReportDocument xDoc = new ReportDocument();


            string reportURL = ("~/PNJ/crSuratPernyataanPenjamin.rpt"); //disesuaikan pathnya
            string xFileName = "SuratPernyataanPenjamin" + "_" + userID + "_" + pinjamanID + "_" +
            System.DateTime.Now.date_to_yearMonthDayTimeString() + ".pdf";

            string reportPath = Server.MapPath(reportURL);
            xDoc.Load(reportPath);

            //disesuaikan pathnya -- pas pertama kali aja ya kalo update atau buat baru
            xDataSetToPrint.WriteXmlSchema(Server.MapPath("~/PNJ/perjanjianKopkari.xsd"));

            foreach (CrystalDecisions.CrystalReports.Engine.Table tbl in xDoc.Database.Tables)
            {
                tbl.SetDataSource(xDataSetToPrint);
            }

            System.IO.Stream oStream = null;
            byte[] byteArray = null;

            oStream = xDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            byteArray = new byte[oStream.Length];
            oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));


            Response.Expires = 0;
            Response.Buffer = true;
            Response.ClearContent();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + xFileName);
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(byteArray);
            oStream.Close();
            Response.End();

        }

        #endregion

        //protected void btnPrintLetter_click(object sender, EventArgs e)
        //{
        //    //Session["kopkariLastTxnToDo"] = hidTxnID.Value;
        //    //printPerjanjian();
        //    ScriptManager.RegisterClientScriptBlock(updatePanelPrintLetter, typeof(UpdatePanel), "OpenModalDialog", "$('#modelDialogPrintLetter').modal('show');", true);
        //    updatePanelPrintLetter.Update();
        //    //Response.Redirect("~/PNJ/cetakPerjanjian.aspx");
        //}

        protected void printBtn_onClick(object sender, EventArgs e)
        {
            printPerjanjian();

            updatePanelPrintLetter.Update();
        }

        protected void printCloseBtn_onClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(updatePanelPrintLetter, typeof(UpdatePanel), "OpenModalDialog", "$('#modelDialogPrintLetter').modal('hide');", true);
            updatePanelPrintLetter.Update();
        }

        protected void btnUploadDoc_click(object sender, EventArgs e)
        {
            //Session["kopkariLastTxnToDo"] = hidTxnID.Value;

            showUploadDocPopUp();
            //Response.Redirect("~/PNJ/uploadDokumen.aspx");
        }

        protected void nextBtn_click(object sender, EventArgs e)
        {
            BOPinjamanTxn xTxn = BLLPinjamanTxn.getContent(hidTxnID.Value);

            if (xTxn.txnStatus == "PSA007")
            {
                saveStatusSuratPerjanjianStatus();
                showUploadDocPopUp();
            }
        }

        protected void backBtn_click(object sender, EventArgs e)
        {
            showProcessDocPopUp();
        }

        protected void btnProcessDoc_click(object sender, EventArgs e)
        {
            //Session["kopkariLastTxnToDo"] = hidTxnID.Value;

            BOPinjamanTxn xTxn = BLLPinjamanTxn.getContent(hidTxnID.Value);
            if (xTxn.txnStatus == "PSA008" || xTxn.txnStatus == "PSA012") // mau buat upload
            {
                showUploadDocPopUp();
            }

            else if (xTxn.txnStatus == "PSA007") // mau ttd
            {
                showProcessDocPopUp();
            }

            //Response.Redirect("~/PNJ/uploadDokumen.aspx");
        }

        #region bagian detail pnj
        protected void btnViewPHP_click(object sender, EventArgs e)
        {
            viewSuratPHP();
        }

        protected void viewSuratPHP()
        {
            // init perjanjian
            viewDocDiv.Visible = true;

            // pinjaman keterangan
            BOPinjamanTxn xPinjaman = BLLPinjamanTxn.getContent(hidTxnID.Value);

            string xPinjamanID = xPinjaman.pinjamanID.ToString();

            BOAnggota xAnggota = BLLAnggota.getContent(xPinjaman.userID);
            BOPinjamanStatusList xStatusList = BLLPinjamanStatus.getListNotes(xPinjamanID);

            DateTime? suratDate = null;
            DateTime? ketuaApproveDate = null;

            foreach (BOPinjamanStatus xStatus in xStatusList) // buat ambil date pas cetak surat perjanjian
            {
                if (xStatus.statusCode == "PSA008")
                {
                    suratDate = xStatus.entryDate;
                }
            }

            foreach (BOPinjamanStatus xStatus in xStatusList) // buat ambil date pas ketua approve
            {
                if (xStatus.statusCode == "PSA018")
                {
                    ketuaApproveDate = xStatus.entryDate;
                }
            }

            // datetime perjanjian
            DateTime notNullSuratDate = suratDate ?? DateTime.Now;
            dayViewDocLit.Text = notNullSuratDate.toDayNameIndo();
            dateViewDocLit.Text = notNullSuratDate.ToString("dd");
            monthViewDocLit.Text = notNullSuratDate.toMonthNameIndo();
            yearViewDocLit.Text = notNullSuratDate.ToString("yyyy");

            tglPihak2ViewDocLit.Text = "Dokumen ini ditandatangani secara elektronik pada <br/>" + notNullSuratDate.toDayNameIndo() + ", " + DateTimeExtender.toLongIndoDateTime(notNullSuratDate);

            //tglPihak2ViewDocLit.Text = notNullSuratDate.ToString("dd") + ' ' + notNullSuratDate.toMonthNameIndo() + ' ' + notNullSuratDate.ToString("yyyy");

            // biodata peminjam

            anggotaNameViewDocLit.Text = xAnggota.anggotaName;
            anggotaNameViewDocLit2.Text = xAnggota.anggotaName;
            userIDViewDocLit.Text = xAnggota.userID;
            KTPViewDocLit.Text = xAnggota.KTPNo;
            alamatViewDocLit.Text = xAnggota.KTPAddress;

            pasal1ViewDocCheckBox.Enabled = false;
            pasal1ViewDocCheckBox.Checked = true;

            pasal2ViewDocCheckBox.Enabled = false;
            pasal2ViewDocCheckBox.Checked = true;

            pasal3ViewDocCheckBox.Enabled = false;
            pasal3ViewDocCheckBox.Checked = true;

            pasal4ViewDocCheckBox.Enabled = false;
            pasal4ViewDocCheckBox.Checked = true;

            DateTime notNullKetuaApproveDate = ketuaApproveDate ?? DateTime.Now;

            if (notNullKetuaApproveDate != DateTime.Now)
            {
                tglPihak1ViewDocLit.Text = "Dokumen ini ditandatangani secara elektronik pada <br/>" + notNullKetuaApproveDate.toDayNameIndo() + ", " + DateTimeExtender.toLongIndoDateTime(notNullKetuaApproveDate);
            }

            else
            {
                tglPihak1ViewDocLit.Text = "";
            }


            noPinjamanViewDocLit.Text = xPinjaman.pinjamanNo;

            amountAngkaViewDocLit.Text = xPinjaman.amount.ToString("###,###,###");
            amountTerbilangViewDocLit.Text = Terbilang(long.Parse(xPinjaman.amount.ToString("#0")));

            tenorViewDocLit.Text = xPinjaman.tenor.ToString();
            tenorTerbilangViewDocLit.Text = Terbilang(xPinjaman.tenor);

            //fromMonthYearViewDocLit.Text = xPinjaman.txnDate.toMonthNameYearIndo();
            //toMonthYearViewDocLit.Text = xPinjaman.txnDate.AddMonths(xPinjaman.tenor - 1).toMonthNameYearIndo();

            bungaViewDocLit.Text = xPinjaman.bunga.ToString("###,###,###");
            bungaTerbilangViewDocLit.Text = Terbilang(long.Parse(xPinjaman.bunga.ToString("#0")));

            cilPokViewDocLit.Text = xPinjaman.cicilanPokok.ToString("###,###,###");
            cilPokTerbilangViewDocLit.Text = Terbilang(long.Parse(xPinjaman.cicilanPokok.ToString("#0")));

            ScriptManager.RegisterClientScriptBlock(updatePanelViewDoc, typeof(UpdatePanel), "OpenModalDialog", "$('#modelDialogViewDoc').modal('show');", true);

            updatePanelViewDoc.Update();

        }

        protected void viewDocCloseBtn_click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(updatePanelViewDoc, typeof(UpdatePanel), "OpenModalDialog", "$('#modelDialogViewDoc').modal('hide');", true);

            updatePanelViewDoc.Update();
        }

        #endregion


        protected void uploadDocCloseBtn_click(object sender, EventArgs e)
        {
            showProcessDocPopUp();
        }

        protected void processDocCloseBtn_click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(updatePanelUploadDoc, typeof(UpdatePanel), "OpenModalDialog", "$('#modelDialogProcessDoc').modal('hide');", true);

            Response.Redirect(Request.RawUrl);
            updatePanelUploadDoc.Update();
        }

        protected void uploadDocSaveBtn_click(object sender, EventArgs e)
        {
            saveDoc();
        }

        protected void uploadDocSubmitBtn_click(object sender, EventArgs e)
        {
            submitDoc();
        }
        protected void searchBtn_Click(object sender, EventArgs e)
        {
            FillGrid(hdSortEx.Value, hdSortDir.Value);
        }


        protected void btnAddPinjaman_Click(object sender, EventArgs e)
        {
            Response.Redirect(clsConstant.redirectURLPengajuanPinjamanAnggota, true);
        }


        #region preview download yg upload ini
        protected void btnDownloadHide_click(object sender, EventArgs e)
        {
            string fileName = "";
            string xDocID = hidDocIDDownload.Value;

            BODokumenPersyaratan xDoc = BLLDokumenPersyaratan.getContent(hidTxnID.Value, xDocID);

            fileName = xDoc.docName;
            Response.Expires = 0;
            Response.Buffer = true;
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            Response.ContentType = mimeHelper.getMimeType(xDoc.docType);
            Response.BinaryWrite(xDoc.docContent);

            Response.End();
        }

        protected void btnPreviewHide_click(object sender, EventArgs e)
        {
            try
            {
                string xDocID = hidDocIDPreview.Value;
                BODokumenPersyaratan xDoc = BLLDokumenPersyaratan.getContent(hidTxnID.Value, xDocID);

                if (xDoc != null)
                {


                    ketLbl.Text = "Keterangan : " + xDoc.docPinjamanKet;

                    byte[] fileByte = xDoc.docContent;
                    string xFileName = xDoc.docPinjamanNameOrigin;

                    String base64File = "";

                    try
                    {
                        base64File = Convert.ToBase64String(fileByte);
                    }
                    catch (Exception)
                    {
                    }

                    litPreviewAttachment.Text = "<object id=\"previewAttach\" style=\"width:100%;min-height:500px;border=solid 2px; border-color=black\" data=\"data:" + mimeHelper.getMimeType(xFileName) + ";base64," + base64File + "\" type=\"" + mimeHelper.getMimeType(xFileName) + "\"></object>";
                }

                else
                {
                    litPreviewAttachment.Text = "Dokumen Tidak Ditemukan";
                }

                ScriptManager.RegisterClientScriptBlock(updPreviewAttachment, typeof(UpdatePanel), "OpenModalDialog", "$('#modalPreviewAttachment').modal('show');", true);
                updPreviewAttachment.Update();
            }
            catch (Exception ex)
            {
                uploadDocLit.Text = "Error Previewing:" + ex.Message;
                updatePanelUploadLit.Update();
            }

            ScriptManager.RegisterClientScriptBlock(updPreviewAttachment, typeof(UpdatePanel), "OpenModalDialog", "$('#modalPreviewAttachment').modal('show');", true);
            updPreviewAttachment.Update();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "", "gridRBValidation()", true);

        }
        #endregion

        #region check preview or download yang buat di detail
        protected void btnDownloadCheckHide_click(object sender, EventArgs e)
        {
            string fileName = "";
            string xDocID = hidDocIDDownload.Value;

            BODokumenPersyaratan xDoc = BLLDokumenPersyaratan.getContent(hidTxnID.Value, xDocID);

            if (xDoc != null)
            {
                fileName = xDoc.docName;
                Response.Expires = 0;
                Response.Buffer = true;
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                Response.ContentType = mimeHelper.getMimeType(xDoc.docType);
                Response.BinaryWrite(xDoc.docContent);

                Response.End();
            }

            else
            {
                string msg = "Dokumen Tidak Ditemukan";
                string xjs = @"setTimeout(function() {txnFailed('" + msg + "'); });";

                //ScriptManager.RegisterClientScriptBlock(updatePanelFormData, typeof(UpdatePanel), "OpenModalDialog", xjs, true);
                //updatePanelFormData.Update();
            }

        }

        protected void btnPreviewCheckHide_click(object sender, EventArgs e)
        {
            try
            {
                string xDocID = hidDocIDPreview.Value;
                BODokumenPersyaratan xDoc = BLLDokumenPersyaratan.getContent(hidTxnID.Value, xDocID);
                if (xDoc == null)
                {
                    litPreviewAttachment.Text = "Dokumen Tidak Ditemukan";
                }

                else
                {
                    byte[] fileByte = xDoc.docContent;
                    string xFileName = xDoc.docPinjamanNameOrigin;

                    String base64File = "";

                    try
                    {
                        base64File = Convert.ToBase64String(fileByte);
                        litPreviewAttachment.Text = "<object id=\"previewAttach\" style=\"width:100%;min-height:500px;border=solid 2px; border-color=black\" data=\"data:" + mimeHelper.getMimeType(xFileName) + ";base64," + base64File + "\" type=\"" + mimeHelper.getMimeType(xFileName) + "\"></object>";

                        ScriptManager.RegisterClientScriptBlock(updPreviewAttachment, typeof(UpdatePanel), "OpenModalDialog", "$('#modalPreviewAttachment').modal('show');", true);
                        updPreviewAttachment.Update();
                    }
                    catch (Exception ex)
                    {
                        uploadDocLit.Text = "Error Previewing:" + ex.Message;
                        updatePanelUploadLit.Update();
                    }
                }

                ScriptManager.RegisterClientScriptBlock(updPreviewAttachment, typeof(UpdatePanel), "OpenModalDialog", "$('#modalPreviewAttachment').modal('show');", true);
                updPreviewAttachment.Update();
            }
            catch (Exception ex)
            {
                uploadDocLit.Text = "Error Previewing:" + ex.Message;
                updPreviewAttachment.Update();
            }

            ScriptManager.RegisterClientScriptBlock(updPreviewAttachment, typeof(UpdatePanel), "OpenModalDialog", "$('#modalPreviewAttachment').modal('show');", true);
            updPreviewAttachment.Update();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "", "gridRBValidation()", true);


        }
        #endregion


        #region document status
        protected void FillDocStatus()
        {
            BODokumenNotesList xDocList = BLLDokumenNotes.getListDocStatus(hidTxnID.Value, "dn.docID", "asc");
            BODokumenNotesList xApprovedDocList = BLLDokumenNotes.getList(hidTxnID.Value, "Approved", "dn.docID", "asc");
            BODokumenNotesList xNeedApprovalDocList = BLLDokumenNotes.getList(hidTxnID.Value, "Need Approval", "dn.docID", "asc");
            BOPinjamanTxn xPinjaman = BLLPinjamanTxn.getContent(hidTxnID.Value);


            //BODokumenPersyaratanList xDocList = BLLDokumenPersyaratan.getList(xTxn.pinjamanID, "docID", "asc");
            checkDocRepeater.DataSource = xDocList;
            checkDocRepeater.DataBind();

            Boolean isWait = true;

            foreach (RepeaterItem item in checkDocRepeater.Items)
            {
                string strScript = "CBValidation(" + ((CheckBox)item.FindControl("checkCB")).ClientID + ", " + item.ItemIndex + ");";
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Click" + item.ItemIndex, strScript, true);
            }

            foreach (BODokumenNotes xDoc in xDocList)
            {
                String xStatus = xDoc.docStatus;

                if (xStatus != "Need Approval")
                {
                    isWait = false;
                    break;
                }
            }


            //if (xApprovedDocList != null && xDocList != null)
            //{
            //    if (xApprovedDocList.Count == xDocList.Count)
            //    {
            //        statusDocLbl.Text = "Dokumen Lengkap";
            //        statusDocLbl.CssClass = "text-success";
            //    }

            //    else if (xNeedApprovalDocList.Count == xDocList.Count && isWait == true && xPinjaman.txnStatus == "PSA009")
            //    {
            //        statusDocLbl.Text = "Menunggu Verifikasi Admin";
            //        statusDocLbl.CssClass = "text-primary";
            //    }

            //    else if (xNeedApprovalDocList.Count == xDocList.Count && isWait == true && xPinjaman.txnStatus != "PSA009")
            //    {
            //        statusDocLbl.Text = "Dokumen Masih Belum Dikirim";
            //        statusDocLbl.CssClass = "text-warning";
            //    }

            //    else
            //    {
            //        statusDocLbl.Text = "Dokumen Belum Lengkap";
            //        statusDocLbl.CssClass = "text-danger";
            //    }
            //}

            //else
            //{
            //    statusDocLbl.Text = "Dokumen Belum Lengkap";
            //    statusDocLbl.CssClass = "text-danger";
            //}
        }

        protected void checkDocRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ScriptManager scriptMng = ScriptManager.GetCurrent(this);
                Label docIDLbl = e.Item.FindControl("docIDLbl") as Label;
                Label pinjamanIDLbl = e.Item.FindControl("pinjamanIDLbl") as Label;
                BODokumenPersyaratan xDoc = BLLDokumenPersyaratan.getContent(pinjamanIDLbl.Text, docIDLbl.Text);
                Label fileNameLbl = e.Item.FindControl("fileNameLbl") as Label;
                Label lastFeedBackLbl = e.Item.FindControl("lastFeedBackLbl") as Label;
                CheckBox checkCB = e.Item.FindControl("checkCB") as CheckBox;
                scriptMng.RegisterAsyncPostBackControl(checkCB);
                LinkButton btnDownload = e.Item.FindControl("btnDownloadCheck") as LinkButton;
                LinkButton btnPreview = e.Item.FindControl("btnPreviewCheck") as LinkButton;

                BODokumenNotesList xApprovedDocList = BLLDokumenNotes.getList(hidTxnID.Value, "Approved", "dn.docID", "asc");
                BODokumenNotesList xDocList = BLLDokumenNotes.getListDocStatus(hidTxnID.Value, "dn.docID", "asc");

                btnDownload.Attributes.Add("onclick", "buttonDownloadCheckClick('" + docIDLbl.Text + "','" + pinjamanIDLbl.Text + "'); return false;");
                btnPreview.Attributes.Add("onclick", "buttonPreviewCheckClick('" + docIDLbl.Text + "','" + pinjamanIDLbl.Text + "'); return false;");

                if (xApprovedDocList != null && xDocList != null)
                {
                    if (xApprovedDocList.Count == xDocList.Count)
                    {
                        btnDownload.Visible = true;
                        btnPreview.Visible = true;
                    }
                    else
                    {
                        btnDownload.Visible = false;
                        btnPreview.Visible = false;
                    }
                }
                else
                {
                    btnDownload.Visible = false;
                    btnPreview.Visible = false;
                }

                if (xDoc == null) // kalo datanya gk ada yauda dihide dlu aja ya
                {
                    btnDownload.Visible = false;
                    btnPreview.Visible = false;
                }

                string docID = docIDLbl.Text.ToString();

                if (xDoc != null)
                {
                    //entryIDLbl.Text = xDoc.entryID.ToString();
                    fileNameLbl.Text = xDoc.docPinjamanNameOrigin.ToString();
                    btnDownload.CommandArgument = xDoc.docPinjamanNameOrigin.ToString();
                    btnPreview.CommandArgument = xDoc.docPinjamanNameOrigin.ToString();
                }

                BODokumenNotes notesTemp = (BODokumenNotes)e.Item.DataItem;

                if (notesTemp != null && String.Compare(notesTemp.docStatus, "Need Approval", true) == 0)
                {
                    checkCB.Checked = false;
                    lastFeedBackLbl.Visible = false;
                    //isChecked.Text = "false";
                    // txtNotes.Visible = false;
                }
                else if (notesTemp != null && String.Compare(notesTemp.docStatus, "Approved", true) == 0)
                {
                    checkCB.Checked = true;
                    lastFeedBackLbl.Visible = false;
                    //isChecked.Text = "true";
                    //txtNotes.Visible = false;
                }
                else if (notesTemp != null && String.Compare(notesTemp.docStatus, "Rejected", true) == 0)
                {
                    checkCB.Checked = false;
                    lastFeedBackLbl.Visible = true;
                    lastFeedBackLbl.Text = notesTemp.lastFeedBack;
                    //isChecked.Text = "false";
                    // txtNotes.Visible = true;
                }
                else if (notesTemp != null && notesTemp.docStatus == null)
                {
                    checkCB.Checked = false;
                    lastFeedBackLbl.Visible = false;
                    //isChecked.Text = "false";
                    // txtNotes.Visible = false;
                }
            }
        }

        protected void checkDocRepeater_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            //if (e.CommandName == "downloadCheckDoc")
            //{
            //    txtFileNameCheckDownload.Text = e.CommandArgument.ToString();
            //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Click", "buttonDownloadCheckClick();", true);
            //}
            //else if (e.CommandName == "previewCheckDoc")
            //{
            //    //hidFileNameDownload.Value = e.CommandArgument.ToString();
            //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Click", "buttonPreviewCheckClick('" + e.CommandArgument.ToString() + "');", true);
            //}
        }
        #endregion


        //#region document status (code lama)
        //protected void docCheckBox_OnCheckedChanged(object sender, EventArgs e)
        //{
        //    updatePanelCheckDoc.Update();
        //    updatePanelAction.Update();
        //}

        //protected void initCheckBox()
        //{
        //    string xTxnID = hidTxnID.Value;
        //    BODokumenNotesList xList = BLLDokumenNotes.getList(xTxnID, "", "entryID", "asc");
        //    int flag = 1;

        //    if (xList == null)
        //    {
        //        statusDocLbl.Text = "Dokumen Belum Lengkap";
        //        statusDocLbl.CssClass = "text-danger";
        //        return;
        //    }

        //    if (xList[0].docStatus == "Approved")
        //    {
        //        checkKK.Checked = true;
        //    }

        //    else flag = 0;


        //    if (xList[1].docStatus == "Approved")
        //    {
        //        checkKTP.Checked = true;
        //    }

        //    else flag = 0;

        //    if (xList[2].docStatus == "Approved")
        //    {
        //        checkKTPPenjamin.Checked = true;
        //    }

        //    else flag = 0;

        //    if (xList[3].docStatus == "Approved")
        //    {
        //        checkNameTag.Checked = true;
        //    }

        //    else flag = 0;

        //    if (xList[4].docStatus == "Approved")
        //    {
        //        checkSJP.Checked = true;
        //    }

        //    else flag = 0;

        //    if (xList[5].docStatus == "Approved")
        //    {
        //        checkSPPHal1.Checked = true;
        //    }

        //    else flag = 0;

        //    if (xList[6].docStatus == "Approved")
        //    {
        //        checkSPPHal2.Checked = true;
        //    }

        //    else flag = 0;

        //    if (flag == 0)
        //    {
        //        statusDocLbl.Text = "Dokumen Belum Lengkap";
        //        statusDocLbl.CssClass = "text-danger";
        //    }

        //    else
        //    {
        //        statusDocLbl.Text = "Dokumen Lengkap";
        //        statusDocLbl.CssClass = "text-success";
        //    }

        //    //showHideTxtNotes();
        //}

        //#endregion



        #region repeater upload dokumen
        protected void FillRepeater()
        {
            BOPinjamanTxn xTxn = BLLPinjamanTxn.getContent(hidTxnID.Value);
            BOAlasanDokumenList xAlasanList = BLLAlasanDokumen.getListRepeater(xTxn.pinjamanID);
            uploadDocRepeater.DataSource = xAlasanList;
            uploadDocRepeater.DataBind();
        }

        protected void uploadDocRepeater_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            //if (e.CommandName == "downloadDoc")
            //{
            //    txtFileNameDownload.Text = e.CommandArgument.ToString();
            //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Click", "buttonDownloadClick();", true);
            //}
            //else if (e.CommandName == "previewDoc")
            //{
            //    //hidFileNameDownload.Value = e.CommandArgument.ToString();
            //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Click", "buttonPreviewClick('" + e.CommandArgument.ToString() + "');", true);
            //}
        }

        protected void uploadDocRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label docIDLbl = e.Item.FindControl("docIDLbl") as Label;
                Label pinjamanIDLblDoc = e.Item.FindControl("pinjamanIDLblDoc") as Label;
                BODokumenPersyaratan xDoc = BLLDokumenPersyaratan.getContent(pinjamanIDLblDoc.Text, docIDLbl.Text);
                LinkButton btnUpload = e.Item.FindControl("btnUpload") as LinkButton;
                LinkButton btnDownload = e.Item.FindControl("btnDownload") as LinkButton;
                LinkButton btnPreview = e.Item.FindControl("btnPreview") as LinkButton;
                TextBox txtKet = e.Item.FindControl("txtKet") as TextBox;
                Label notesLbl = e.Item.FindControl("notesLbl") as Label;
                TextBox txtFileName = e.Item.FindControl("txtFileName") as TextBox;
                TextBox txtIsEdit = e.Item.FindControl("txtIsEdit") as TextBox;
                TextBox txtIsUploaded = e.Item.FindControl("txtIsUploaded") as TextBox;
                TextBox txtIsApproved = e.Item.FindControl("txtIsApproved") as TextBox;
                Label entryIDLbl = (Label)e.Item.FindControl("entryIDLbl");
                HtmlTableCell tdUpload = (HtmlTableCell)e.Item.FindControl("tdUpload");
                LinkButton btnNotes = e.Item.FindControl("notesBtn") as LinkButton;
                Label notesSpan = (Label)e.Item.FindControl("notesSpan");

                //Label docTypeLbl = e.Item.FindControl("docTypeLbl") as Label;

                if (xDoc != null)
                {
                    entryIDLbl.Text = xDoc.entryID.ToString();
                    txtFileName.Text = xDoc.docPinjamanNameOrigin.ToString();
                    btnDownload.CommandArgument = xDoc.docPinjamanNameOrigin.ToString();
                    btnPreview.CommandArgument = xDoc.docPinjamanNameOrigin.ToString();
                }

                string docID = docIDLbl.Text.ToString();
                //string typeFile = docTypeLbl.Text.ToString();

                string strScript = "buttonUploadClick('" + ((TextBox)e.Item.FindControl("txtFileName")).ClientID + "', '" + ((TextBox)e.Item.FindControl("txtIsEdit")).ClientID + "', '" + ((Label)e.Item.FindControl("docIDLbl")).Text + "', '" + ((TextBox)e.Item.FindControl("txtIsUploaded")).ClientID + "');";

                ((LinkButton)e.Item.FindControl("btnUpload")).Attributes.Add("onclick", strScript);

                btnDownload.Attributes.Add("onclick", "buttonDownloadClick('" + docIDLbl.Text + "','" + pinjamanIDLblDoc.Text + "'); return false;");
                btnPreview.Attributes.Add("onclick", "buttonPreviewClick('" + docIDLbl.Text + "','" + pinjamanIDLblDoc.Text + "'); return false;");

                string filePathName;
                byte[] fileattach;

                BODokumenPersyaratan temp = xDoc;
                BODokumenNotes notesTemp = new BODokumenNotes();

                if (docIDLbl.Text == "DOC0001")
                {
                    txtKet.Attributes.Add("placeholder", "Dimohon untuk memasukkan nomor KTP");
                    txtKet.CssClass += " txtNoKTP";
                }

                else
                {
                    txtKet.Attributes.Add("placeholder", "Masukan Keterangan Disini");
                }

                if (temp != null)
                {
                    notesTemp = BLLDokumenNotes.getContent(temp.pinjamanID, temp.entryID, temp.docID);
                }
                else
                {
                    notesTemp = null;
                }

                if (temp != null)
                {
                    filePathName = null;
                    fileattach = null;

                    if (xDoc.docPinjamanKet == "")
                    {
                        txtKet.Text = "-";
                    }
                    else
                    {
                        txtKet.Text = xDoc.docPinjamanKet;
                    }

                    txtFileName.Text = temp.docPinjamanNameOrigin.ToString();
                    filePathName = Server.MapPath("~/tempUpload/") + txtFileName.Text;
                    fileattach = temp.docContent;
                    //txtIsEdit.Text = "true";

                    using (var fs = new FileStream(filePathName, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(fileattach, 0, fileattach.Length);
                    }
                }

                BOPinjamanTxn xPinjaman = BLLPinjamanTxn.getContent(pinjamanIDLblDoc.Text);

                if (notesTemp == null && temp == null)
                {
                    btnUpload.Visible = true;
                    tdUpload.Visible = true;
                    btnDownload.Visible = false;
                    //btnDownload.Attributes["style"] = "margin-left: 10px;";
                    btnPreview.Visible = false;
                    //btnNotes.Visible = false;
                    txtKet.Enabled = true;
                    fakeUploadDocSaveBtn.Visible = true;
                    fakeUploadDocSubmitBtn.Visible = true;
                    txtIsUploaded.Text = "true";
                    notesSpan.Visible = false;


                }
                else if (xPinjaman.txnStatus == "PSA008" && notesTemp != null)
                {
                    btnUpload.Visible = true;
                    tdUpload.Visible = false;
                    //btnDownload.Attributes["style"] = "margin-left: 10px;";
                    btnDownload.Visible = true;
                    btnPreview.Visible = true;
                    txtKet.Enabled = true;
                    //btnNotes.Visible = false;
                    txtIsUploaded.Text = "true";
                    fakeUploadDocSaveBtn.Visible = true;
                    fakeUploadDocSubmitBtn.Visible = true;
                    notesSpan.Visible = false;

                }
                else if (notesTemp != null && String.Compare(notesTemp.docStatus, "Need Approval", true) == 0 && (xPinjaman.txnStatus != "PSA008" || xPinjaman.txnStatus != "PSA012"))
                {
                    //btnDownload.Attributes["style"] = "margin-left: 60px;";
                    btnUpload.Visible = false;
                    tdUpload.Visible = false;
                    btnDownload.Visible = true;
                    btnPreview.Visible = true;
                    //btnNotes.Visible = false;
                    txtKet.Enabled = false;
                    txtIsUploaded.Text = "true";
                    fakeUploadDocSaveBtn.Visible = false;
                    fakeUploadDocSubmitBtn.Visible = false;
                    notesSpan.Visible = false;

                }
                else if (notesTemp != null && String.Compare(notesTemp.docStatus, "Approved", true) == 0)
                {
                    btnUpload.Visible = false;
                    tdUpload.Visible = false;
                    btnDownload.Visible = true;
                    //btnDownload.Attributes["style"] = "margin-left: 60px;";
                    btnPreview.Visible = true;
                    txtKet.Enabled = false;
                    //btnNotes.Visible = false;
                    txtIsUploaded.Text = "true";
                    txtIsApproved.Text = "true";
                    fakeUploadDocSaveBtn.Visible = false;
                    fakeUploadDocSubmitBtn.Visible = false;
                    notesSpan.Visible = false;
                }
                else if (notesTemp != null && String.Compare(notesTemp.docStatus, "Rejected", true) == 0)
                {
                    btnUpload.Enabled = true;
                    btnDownload.Visible = true;
                    //btnDownload.Attributes["style"] = "margin-left: 10px;";

                    btnPreview.Visible = true;
                    txtKet.Enabled = true;

                    //btnNotes.Visible = true;

                    tdUpload.Visible = true;
                    notesLbl.Text = notesTemp.lastFeedBack;
                    notesLbl.CssClass = "text-danger";
                    txtIsApproved.Text = "";
                    fakeUploadDocSaveBtn.Visible = true;
                    fakeUploadDocSubmitBtn.Visible = true;

                    notesSpan.Visible = true;
                    notesTB.Text = notesTemp.lastFeedBack;
                }

                updatePanelShowNotes.Update();
                updatePanelUploadDoc.Update();
            }
        }
        #endregion


        #region grid view
        protected void gvAnggota_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int xTenor = (int)DataBinder.Eval(e.Row.DataItem, "tenor");
                int xCicil = (int)DataBinder.Eval(e.Row.DataItem, "jmlCicil");

                Label tenorGridLb = ((Label)e.Row.FindControl("tenorGridLb"));

                tenorGridLb.Text = xCicil + "/" + xTenor;

                String xID = (String)DataBinder.Eval(e.Row.DataItem, "pinjamanID");

                Label txnStatusGVAnggotaLbl = ((Label)e.Row.FindControl("txnStatusGVAnggotaLbl"));

                BOPinjamanTxn xPNJ = BLLPinjamanTxn.getContent(xID);

                txnStatusGVAnggotaLbl.Text = xPNJ.statusName;
                txnStatusGVAnggotaLbl.CssClass = xPNJ.txnStatusPNJStyle;

            }
        }

        protected void gvNotes_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DateTime xDate = (DateTime)DataBinder.Eval(e.Row.DataItem, "entryDate");


                Label txnDateLogStatusLb = ((Label)e.Row.FindControl("txnDateLogStatusLb"));


                String xStatusByLog = (String)DataBinder.Eval(e.Row.DataItem, "statusByLog");

                if (xStatusByLog == null)
                {
                    e.Row.Visible = false;
                }


                txnDateLogStatusLb.Text = xDate.toLongIndoDateTime();

            }
        }

        protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label cclJasaLbl = ((Label)e.Row.FindControl("cclJasaLbl"));
                Label totalCclLbl = ((Label)e.Row.FindControl("totalCclLbl"));
                LinkButton printLetterBtn = ((LinkButton)e.Row.FindControl("printLetterBtn"));
                LinkButton uploadDocBtn = ((LinkButton)e.Row.FindControl("uploadDocBtn"));
                LinkButton processDocBtn = ((LinkButton)e.Row.FindControl("processDocBtn"));

                string txnStatus = (string)DataBinder.Eval(e.Row.DataItem, "txnStatus");

                decimal xAmount = (decimal)DataBinder.Eval(e.Row.DataItem, "amount");
                decimal xBunga = (decimal)DataBinder.Eval(e.Row.DataItem, "bunga");
                decimal xCclPokok = (decimal)DataBinder.Eval(e.Row.DataItem, "cicilanPokok");

                decimal xJasa = xBunga / 12;
                decimal xCclJasa = xAmount * xJasa / 100;
                decimal xTotalCcl = xCclPokok + xCclJasa;

                cclJasaLbl.Text = xCclJasa.ToString("0,#,0");
                //cclJasaLbl.Text = DecimalExtender.formatCurrency(xCclJasa.ToString(), "en-US");

                totalCclLbl.Text = xTotalCcl.ToString("0#,0");
                //totalCclLbl.Text = DecimalExtender.formatCurrency(totalCclLbl.ToString(), "en-US");

                //if (txnStatus == "PSA003" || txnStatus == "PSA005" || txnStatus == "PSA008" || txnStatus == "PSA009" || txnStatus == "PSA010" || txnStatus == "PSA011" || txnStatus == "PSA012")
                //{
                //    printLetterBtn.Visible = true;
                //}

                //if (txnStatus == "PSA008" || txnStatus == "PSA009" || txnStatus == "PSA010" || txnStatus == "PSA011")
                //{
                //    uploadDocBtn.Visible = true;
                //}

                /*
                 * PSA007 = Surat Perjanjian
                 * PSA008 = Upload Dokumen
                 * PSA012 = 
                 * 
                 */

                if (txnStatus == "PSA007" || txnStatus == "PSA008" || txnStatus == "PSA012")
                {
                    processDocBtn.Visible = true;
                }
                else
                {
                    processDocBtn.Visible = false;
                }

                String xID = (String)DataBinder.Eval(e.Row.DataItem, "pinjamanID");

                LinkButton detailBtn = ((LinkButton)e.Row.FindControl("detailBtn"));
                detailBtn.Text = "<i class='fa fa-search mr - 2' ></i>" + xID;

                Label txnStatusLbl = ((Label)e.Row.FindControl("txnStatusLbl"));

                BOPinjamanTxn xPNJ = BLLPinjamanTxn.getContent(xID);

                txnStatusLbl.Text = xPNJ.statusByAnggota;
                txnStatusLbl.CssClass = xPNJ.txnStatusPNJStyle;

                /* Document Notes */

                BODokumenNotesList checkDraft = BLLDokumenNotes.getList(xID, "Draft", "", "");

                if (checkDraft != null)
                {
                    processDocBtn.Text = "<i class='fa fa-upload'></i> Perlu Submit Dokumen";
                }

                else
                {
                    processDocBtn.Text = "<i class='fa fa-upload'></i> Lengkapi Dokumen Disini";
                }
            }
        }

        protected void gvList_Sorting(object sender, GridViewSortEventArgs e)
        {
            hdSortEx.Value = e.SortExpression;
            if (hdSortDir.Value == "desc")
            {
                hdSortDir.Value = "asc";
            }
            else
            {
                hdSortDir.Value = "desc";
            }

            FillGrid(hdSortEx.Value, hdSortDir.Value);
        }

        protected void gvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMain.PageIndex = e.NewPageIndex;
            FillGrid(hdSortEx.Value, hdSortDir.Value);
        }

        protected void gvAnggota_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            BOPinjamanTxn xBO = BLLPinjamanTxn.getLoadTxn(hidTxnID.Value);
            gvAnggota.PageIndex = e.NewPageIndex;
            FillGridAnggota(xBO.userID.ToString(), hdSortEx.Value, hdSortDir.Value);
            updatePanelGvAnggota.Update();
            //updatePanelFormData.Update();
        }

        protected void gvAnggota_Sorting(object sender, GridViewSortEventArgs e)
        {
            hdSortExAng.Value = e.SortExpression;
            if (hdSortDirAng.Value == "desc")
            {
                hdSortDirAng.Value = "asc";
            }
            else
            {
                hdSortDirAng.Value = "desc";
            }


            BOPinjamanTxn xBO = BLLPinjamanTxn.getLoadTxn(hidTxnID.Value);

            FillGridAnggota(xBO.userID, hdSortExAng.Value, hdSortDirAng.Value);
        }

        protected DataTable createDataTable(GridView gv)
        {
            DataTable xdt = new DataTable();

            //Add columns to DataTable.
            foreach (BoundField col in gv.Columns)
            {
                xdt.Columns.Add(col.HeaderText);
            }

            //Loop through the GridView and copy rows.
            foreach (GridViewRow row in gv.Rows)
            {
                xdt.Rows.Add();
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    xdt.Rows[row.RowIndex][i] = row.Cells[i].Text;
                }
            }

            return xdt;
        }

        protected void FillGridNotes(string sortEx, string sortDir, string xTxnID) // ngebind gridview
        {

            BOPinjamanStatusList xNotes = BLLPinjamanStatus.getListNotes(xTxnID);


            gvNotes.DataSource = xNotes;
            gvNotes.DataBind();

            updatePanelNotes.Update();

        }

        protected void FillGrid(string sortEx, string sortDir) // ngebind gridview
        {
            DateTime? fromDate = null, toDate = null;

            fromDate = DateTime.ParseExact(txtFilterDateFrom.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
            toDate = DateTime.ParseExact(txtFilterDateTo.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);

            //BOPinjamanTxnList xTxnList = BLLPinjamanTxn.getListxPinjamanUser(
            //    Session["kopkariUsername"].ToString(),
            //    fromDate, toDate,
            //    sortEx, sortDir
            //    );

            if (sortEx == "")
            {
                sortEx = "pinjamanID";
                sortDir = "desc";
            }
            String status = String.Join(",", listboxFilterStatus.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.Value).ToArray());

            BOPinjamanTxnList xTxnList = BLLPinjamanTxn.getListxPinjamanUser(
             Session["kopkariUserID"].ToString(),
             "",
             fromDate, toDate, status,
             sortEx, sortDir
             );

            //BOCicilanPinjamanList xCicilanList = BLLCicilanPinjaman



            gvMain.DataSource = xTxnList;
            gvMain.DataBind();

            updatePanelPinjaman.Update();


        }

        protected void FillGridAnggota(string userID, string sortEx, string sortDir) // ngebind gridview
        {


            BOPinjamanTxnList xHasil = BLLPinjamanTxn.getListPinjamanGridAnggota(userID, sortEx, sortDir);

            gvAnggota.DataSource = xHasil;
            gvAnggota.DataBind();

            updatePanelGvAnggota.Update();


        }
        #endregion

        #region ajax

        #region button upload doc
        protected void btnUpload_click(object sender, EventArgs e)
        {
            string docID = hidDocID.Value;

            Session["fileType"] = docID.ToString();

        }

        #endregion


        protected void ajaxFileUpAttach_OnUploadComplete(object sender, AjaxFileUploadEventArgs file)
        {
            string filename = System.IO.Path.GetFileName(file.FileName);

            string xPNJID = (string)Session["pinjamanID"];

            string xDocID = (string)Session["fileType"];

            ajaxFileUpAttach.SaveAs(MapPath("~/tempUpload/" + xPNJID + "_" + xDocID + "_" + filename));

            file.PostedUrl = xPNJID + "_" + xDocID + "_" + filename;

            updatePanelUploadDoc.Update();
            updatePanelProcessDoc.Update();
        }


        #endregion


        #region main function

        private void loadTxn(string txnID)
        {
            BOPinjamanTxn xBO = BLLPinjamanTxn.getLoadTxn(txnID);
            BOCicilanPinjaman xCicilan = BLLCicilanPinjaman.getCicilan(txnID);
            BOPinjamanStatus xStatus = BLLPinjamanStatus.getContent(txnID);
            BOPinjamanStatusList xNotes = BLLPinjamanStatus.getListNotes(txnID);

            FillDocStatus();
            FillGridNotes(null, null, txnID);

            if (xNotes == null)
            {
                string err = "tidak ada history notes";
                LitErrHistory.Text = "<div class='alert alert-info text-center' role='alert'>" + err + "</div>";
                updatePanelFormData.Update();
            }

            else
            {
                LitErrHistory.Text = "";
                updatePanelFormData.Update();
            }

            pinjamanIDLb.Text = xBO.pinjamanID.ToString();

            headerNameLb.Text = "Detail Pinjaman " + xBO.anggotaName.ToString();
            //pinjamanDateLb.Text = DateTimeExtender.toLongIndoDateTime(xBO.txnDate);

            // detail header

            decimal bunga = decimal.Parse(xBO.bunga.ToString());
            decimal jasa = bunga / 12;
            decimal angsuranJasa = bunga * jasa / 100;
            decimal agJasa = xBO.amount * jasa / 100;
            decimal angsuranPokok = xBO.amount / xBO.tenor;
            decimal angsuranTotal = angsuranPokok + agJasa;

            anggotaNameLb.Text = xBO.anggotaName;
            tipekaryawanLb.Text = xBO.empTypeID;
            companyLb.Text = xBO.compName;
            lokasiLb.Text = xBO.locName;
            tujuanLb.Text = xBO.tujuan;
            alasanLb.Text = xBO.description;

            if (xBO.pinjamanNo != null)
            {
                noPinjamanLb.Text = xBO.pinjamanNo.ToString();
            }
            else
            {
                noPinjamanLb.Text = "-";
            }

            amountLb.Text = "Rp. " + xBO.amount.ToString("#,0");

            packetLb.Text = xBO.packetName.ToString();
            tenorLb.Text = xBO.tenor.ToString() + " Bulan";
            jasaLb.Text = Math.Round(jasa, 2).ToString() + " %";

            angsuranPokokLb.Text = "Rp. " + xBO.cicilanPokok.ToString("#,0");
            angsuranJasaLb.Text = "Rp. " + agJasa.ToString("#,0");

            decimal trbyr = xBO.jmlCicil * xBO.cicilanPokok;

            decimal agpk = xBO.amount - trbyr;

            saldoPokokLb.Text = "Rp. " + agpk.ToString("#,0");
            angsuranPokokTerbayarLb.Text = "Rp. " + trbyr.ToString("#,0");
            totalAngsuranLb.Text = "Rp. " + angsuranTotal.ToString("#,0");



            if (xCicilan == null) // ini error kalo gk ada datanya di cicilan
            {
                angsurankeLb.Text = "0/" + xBO.tenor.ToString();
            }

            else
            {
                angsurankeLb.Text = xCicilan.jmlCicil.ToString() + '/' + xBO.tenor.ToString();
            }


            if (xBO.jmlCicil != 0)
            {
                tanggalPembayaranTerakhirLb.Text = xBO.txnDate.ToString();
            }

            else
            {
                tanggalPembayaranTerakhirLb.Text = "Cicilan belum berjalan";
            }

            FillGridAnggota(xBO.userID.ToString(), hdSortEx.Value, hdSortDir.Value);

            if (xBO.txnStatus == "PSA002")
            {
                infoDoc.Visible = false;
            }

            else
            {
                infoDoc.Visible = true;
            }

            if (xBO.txnStatus == "PSA001" || xBO.txnStatus == "PSA002" || xBO.txnStatus == "PSA003" || xBO.txnStatus == "PSA004" || xBO.txnStatus == "PSA005" || xBO.txnStatus == "PSA006" || xBO.txnStatus == "PSA007")
            {
                viewPHPBtn.Visible = false;
            }
            else
            {
                viewPHPBtn.Visible = true;
            }

            ScriptManager.RegisterClientScriptBlock(updatePanelFormData, typeof(UpdatePanel), "OpenModalDialog", "$('#modelDialogEditData').modal('show');", true);

            updatePanelFormData.Update();
            updAction.Update();
        }

        protected void printPerjanjian()
        {
            string pinjamanID = hidTxnID.Value.ToString();
            string userID = Session["kopkariUserID"].ToString();
            DataSet xDataSetToPrint = BLLPinjamanTxn.getPinjamanPrint(pinjamanID, userID);
            BOPinjamanTxn xTxn = BLLPinjamanTxn.getContent(pinjamanID);

            string dayToStr = DateTimeExtender.toDayNameIndo(DateTime.Now);
            string monthToStr = DateTimeExtender.toMonthNameIndo(DateTime.Now);
            DateTime dateNow = DateTime.Now;

            string txnDateFromStr = DateTimeExtender.toMonthNameYearIndo(xTxn.txnDate);
            string txnDateToStr = DateTimeExtender.toMonthNameYearIndo(xTxn.txnDate.AddMonths(xTxn.tenor - 1));

            DateTime? suratDate = null;
            DateTime? ketuaApproveDate = null;

            BOPinjamanStatusList xStatusList = BLLPinjamanStatus.getListNotes(pinjamanID);

            foreach (BOPinjamanStatus xStatus in xStatusList) // buat ambil date pas cetak surat perjanjian
            {
                if (xStatus.statusCode == "PSA008")
                {
                    suratDate = xStatus.entryDate;
                }
            }

            foreach (BOPinjamanStatus xStatus in xStatusList) // buat ambil date pas ketua approve
            {
                if (xStatus.statusCode == "PSA018")
                {
                    ketuaApproveDate = xStatus.entryDate;
                }
            }

            string tglPihakKeduaStr;
            string tglPihakPertamaStr;

            // datetime perjanjian
            DateTime notNullSuratDate = suratDate ?? DateTime.Now;
            dayViewDocLit.Text = notNullSuratDate.toDayNameIndo();
            dateViewDocLit.Text = notNullSuratDate.ToString("dd");
            monthViewDocLit.Text = notNullSuratDate.toMonthNameIndo();
            yearViewDocLit.Text = notNullSuratDate.ToString("yyyy");

            tglPihakKeduaStr = "Dokumen ini ditandatangani secara elektronik pada " + notNullSuratDate.toDayNameIndo() + ", " + DateTimeExtender.toLongIndoDateTime(notNullSuratDate);

            DateTime notNullKetuaApproveDate = ketuaApproveDate ?? DateTime.Now;

            if (notNullKetuaApproveDate != DateTime.Now)
            {
                tglPihakPertamaStr = "Dokumen ini ditandatangani secara elektronik pada " + notNullKetuaApproveDate.toDayNameIndo() + ", " + DateTimeExtender.toLongIndoDateTime(notNullKetuaApproveDate);
            }

            else
            {
                tglPihakPertamaStr = "";
            }

            DataTable dataTambahan = new DataTable();

            dataTambahan.Columns.Add("dayStr", typeof(string));
            dataTambahan.Columns.Add("monthStr", typeof(string));
            dataTambahan.Columns.Add("txnDateFromStr", typeof(string));
            dataTambahan.Columns.Add("txnDateToStr", typeof(string));
            dataTambahan.Columns.Add("tglPihakPertamaStr", typeof(string));
            dataTambahan.Columns.Add("tglPihakKeduaStr", typeof(string));
            dataTambahan.Columns.Add("dateNow", typeof(DateTime));

            dataTambahan.Rows.Add(dayToStr, monthToStr, txnDateFromStr, txnDateToStr, tglPihakPertamaStr, tglPihakKeduaStr, dateNow);
            xDataSetToPrint.Tables.Add(dataTambahan);

            ReportDocument xDoc = new ReportDocument();

            string reportURL = ("~/PNJ/crPerjanjianHutangPiutang.rpt"); //disesuaikan pathnya
            string xFileName = "pinjaman" + "_" + userID + "_" + pinjamanID + "_" +
            System.DateTime.Now.date_to_yearMonthDayTimeString() + ".pdf";

            string reportPath = Server.MapPath(reportURL);
            xDoc.Load(reportPath);

            //xDataSetToPrint.WriteXmlSchema(Server.MapPath("~/PNJ/perjanjianKopkari.xsd")); //disesuaikan pathnya
            foreach (CrystalDecisions.CrystalReports.Engine.Table tbl in xDoc.Database.Tables)
            {
                tbl.SetDataSource(xDataSetToPrint);
            }

            System.IO.Stream oStream = null;
            byte[] byteArray = null;

            oStream = xDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            byteArray = new byte[oStream.Length];
            oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));

            BODokumenPersyaratanList docList = new BODokumenPersyaratanList();
            BODokumenNotesList notesList = new BODokumenNotesList();
            BODokumenNotesList xNotes = BLLDokumenNotes.getList(hidTxnID.Value.ToString(), "Rejected", "", "");

            BODokumenPersyaratan docTemp = new BODokumenPersyaratan();
            BODokumenNotes notesTemp = new BODokumenNotes();

            string fileName = pinjamanID + "_DOC0000.pdf";

            docTemp.pinjamanID = hidTxnID.Value.ToString();
            docTemp.docPinjamanURL = fileName; // Server.MapPath("~/tempUpload/") + fileName;

            docTemp.docPinjamanNameOrigin = fileName;
            docTemp.docPinjamanKet = "-";
            docTemp.docID = "DOC0000";
            docTemp.docType = ".pdf"; // System .IO.Path.GetExtension(Server.MapPath("~/tempUpload/") + fileName);
            docTemp.docSize = byteArray.Length;
            docTemp.docContent = byteArray;
            docTemp.lastOperator = Session["kopkariUserID"].ToString();

            notesTemp.pinjamanID = hidTxnID.Value.ToString();
            notesTemp.docID = "DOC0000";
            notesTemp.docStatus = "Need Approval";
            notesTemp.lastOperator = Session["kopkariUserID"].ToString();

            docTemp.entryID = 1;
            notesTemp.entryID = 1;

            docList.Add(docTemp);
            notesList.Add(notesTemp);

            string xStatusCode = "";

            xStatusCode = "PSA008";

            BOPinjamanStatus xUpdateStatus = new BOPinjamanStatus();

            xUpdateStatus.pinjamanID = hidTxnID.Value.ToString();
            xUpdateStatus.lastOperator = Session["kopkariUserID"].ToString(); ;
            xUpdateStatus.statusCode = xStatusCode;
            xUpdateStatus.description = "";

            string retVal = BLLDokumenPersyaratan.Save(docList, notesList, xUpdateStatus);

            //Response.Expires = 0;
            //Response.Buffer = true;
            //Response.ClearContent();
            //Response.AddHeader("Content-Disposition", "attachment; filename=" + xFileName);
            //Response.ContentType = "application/pdf";
            //Response.BinaryWrite(byteArray);
            //Response.End();

            oStream.Close();
        }

        protected BOProcessResult updatePinjamanStatus(string xStatusCode)
        {
            string lastOperator = xUserID;

            BOPinjamanStatus xUpdateStatus = new BOPinjamanStatus();

            xUpdateStatus.lastOperator = lastOperator;
            xUpdateStatus.statusCode = xStatusCode;
            xUpdateStatus.pinjamanID = hidTxnID.Value;

            BOProcessResult retVal = BLLPinjamanStatus.saveStatus(xUpdateStatus);

            updatePanelPinjaman.Update();

            return retVal;
        }

        #region function terbilang

        public static string Terbilang(long a)
        {
            string[] bilangan = new string[] { "", "Satu", "Dua", "Tiga", "Empat", "Lima", "Enam", "Tujuh", "Delapan", "Sembilan", "Sepuluh", "Sebelas" };
            var kalimat = "";
            // 1 - 11
            if (a < 12)
            {
                kalimat = bilangan[a];
            }
            // 12 - 19
            else if (a < 20)
            {
                kalimat = bilangan[a - 10] + " Belas";
            }
            // 20 - 99
            else if (a < 100)
            {
                var utama = a / 10;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 1));
                var belakang = a % 10;
                kalimat = bilangan[depan] + " Puluh " + bilangan[belakang];
            }
            // 100 - 199
            else if (a < 200)
            {
                kalimat = "Seratus " + Terbilang(a - 100);
            }
            // 200 - 999
            else if (a < 1000)
            {
                var utama = a / 100;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 1));
                var belakang = a % 100;
                kalimat = bilangan[depan] + " Ratus " + Terbilang(belakang);
            }
            // 1,000 - 1,999
            else if (a < 2000)
            {
                kalimat = "Seribu " + Terbilang(a - 1000);
            }
            // 2,000 - 9,999
            else if (a < 10000)
            {
                var utama = a / 1000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 1));
                var belakang = a % 1000;
                kalimat = bilangan[depan] + " Ribu " + Terbilang(belakang);
            }
            // 10,000 - 99,999
            else if (a < 100000)
            {
                var utama = a / 100;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 2));
                var belakang = a % 1000;
                kalimat = Terbilang(depan) + " Ribu " + Terbilang(belakang);
            }
            // 100,000 - 999,999
            else if (a < 1000000)
            {
                var utama = a / 1000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 3));
                var belakang = a % 1000;
                kalimat = Terbilang(depan) + " Ribu " + Terbilang(belakang);
            }
            // 1,000,000 - 	99,999,999
            else if (a < 100000000)
            {
                var utama = a / 1000000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 1));//Substring(0, 4));
                var belakang = a % 1000000;
                kalimat = Terbilang(depan) + " Juta " + Terbilang(belakang);
            }
            else if (a < 1000000000)
            {
                var utama = a / 1000000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 3));
                var belakang = a % 1000000;
                kalimat = Terbilang(depan) + " Juta " + Terbilang(belakang);
            }
            else if (a < 10000000000)
            {
                var utama = a / 1000000000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 1));
                var belakang = a % 1000000000;
                kalimat = Terbilang(depan) + " Milyar " + Terbilang(belakang);
            }
            else if (a < 100000000000)
            {
                var utama = a / 1000000000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 2));
                var belakang = a % 1000000000;
                kalimat = Terbilang(depan) + " Milyar " + Terbilang(belakang);
            }
            else if (a < 1000000000000)
            {
                var utama = a / 1000000000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 3));
                var belakang = a % 1000000000;
                kalimat = Terbilang(depan) + " Milyar " + Terbilang(belakang);
            }
            else if (a < 10000000000000)
            {
                var utama = a / 10000000000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 1));
                var belakang = a % 10000000000;
                kalimat = Terbilang(depan) + " Triliun " + Terbilang(belakang);
            }
            else if (a < 100000000000000)
            {
                var utama = a / 1000000000000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 2));
                var belakang = a % 1000000000000;
                kalimat = Terbilang(depan) + " Triliun " + Terbilang(belakang);
            }

            else if (a < 1000000000000000)
            {
                var utama = a / 1000000000000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 3));
                var belakang = a % 1000000000000;
                kalimat = Terbilang(depan) + " Triliun " + Terbilang(belakang);
            }

            else if (a < 10000000000000000)
            {
                var utama = a / 1000000000000000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 1));
                var belakang = a % 1000000000000000;
                kalimat = Terbilang(depan) + " Kuadriliun " + Terbilang(belakang);
            }

            var pisah = kalimat.Split(' ');
            List<string> full = new List<string>();// = [];
            for (var i = 0; i < pisah.Length; i++)
            {
                if (pisah[i] != "") { full.Add(pisah[i]); }
            }
            return CombineTerbilang(full.ToArray());// full.Concat(' '); .join(' ');
        }

        static string CombineTerbilang(string[] arr)
        {
            return string.Join(" ", arr);
        }

        #endregion

        #region processDoc
        protected void showProcessDocPopUp()
        {
            // init perjanjian
            uploadDIV.Visible = false;
            perjanjianDIV.Visible = true;
            backBtn.Visible = false;
            fakeUploadDocSubmitBtn.Visible = false;
            fakeUploadDocSaveBtn.Visible = false;
            nextBtn.Visible = true;

            BOAnggota xAnggota = BLLAnggota.getContent(Session["kopkariUserID"].ToString());

            // datetime perjanjian
            DateTime timeStamp = DateTime.Now;
            dayLit.Text = timeStamp.toDayNameIndo();
            dateLit.Text = timeStamp.ToString("dd");
            monthLit.Text = timeStamp.toMonthNameIndo();
            yearLit.Text = timeStamp.ToString("yyyy");

            //tglPihak2Lit.Text = timeStamp.ToString("dd") + ' ' + timeStamp.toMonthNameIndo() + ' ' + timeStamp.ToString("yyyy");

            // biodata peminjam

            anggotaNameLit.Text = xAnggota.anggotaName;
            anggotaNameLit2.Text = xAnggota.anggotaName;
            userIDLit.Text = xAnggota.userID;
            KTPLit.Text = xAnggota.KTPNo;
            alamatLit.Text = xAnggota.KTPAddress;

            // pinjaman keterangan
            BLLPinjamanTxn.generatePNJNomor(hidTxnID.Value);
            BOPinjamanTxn xPinjaman = BLLPinjamanTxn.getContent(hidTxnID.Value);


            noPinjamanLit.Text = xPinjaman.pinjamanNo;

            amountAngkaLit.Text = xPinjaman.amount.ToString("###,###,###");
            amountTerbilangLit.Text = Terbilang(long.Parse(xPinjaman.amount.ToString("#0")));

            tenorLit.Text = xPinjaman.tenor.ToString();
            tenorTerbilangLit.Text = Terbilang(xPinjaman.tenor);

            //fromMonthYearLit.Text = xPinjaman.txnDate.toMonthNameYearIndo();
            //toMonthYearLit.Text = xPinjaman.txnDate.AddMonths(xPinjaman.tenor - 1).toMonthNameYearIndo();

            bungaLit.Text = xPinjaman.bunga.ToString("###,###,###");

            if (bungaLit.Text == "")
            {
                bungaLit.Text = "0";
            }

            bungaTerbilangLit.Text = Terbilang(long.Parse(xPinjaman.bunga.ToString("#0")));

            if (bungaTerbilangLit.Text == "")
            {
                bungaTerbilangLit.Text = "Nol";
            }

            cilPokLit.Text = xPinjaman.cicilanPokok.ToString("###,###,###");
            cilPokTerbilangLit.Text = Terbilang(long.Parse(xPinjaman.cicilanPokok.ToString("#0")));

            ScriptManager.RegisterClientScriptBlock(updatePanelProcessDoc, typeof(UpdatePanel), "OpenModalDialog", "$('#modelDialogProcessDoc').modal('show');", true);

            updatePanelProcessDoc.Update();
        }

        #endregion

        #region notes penolakan

        protected void notesBtn_click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(updatePanelShowNotes, typeof(UpdatePanel), "OpenModalDialog", "$('#modelDialogShowNotes').modal('show');", true);

            updatePanelShowNotes.Update();
        }

        #endregion

        #region upload dokumen
        protected void showUploadDocPopUp()
        {
            Session["pinjamanID"] = hidTxnID.Value;

            BOPinjamanTxn xTxn = BLLPinjamanTxn.getContent(hidTxnID.Value);

            if (xTxn.txnStatus == "PSA008" || xTxn.txnStatus == "PSA012") // surat perjanjian || dokumen tidak sesuai
            {
                backBtn.Visible = false;
                perjanjianDIV.Visible = false;
            }


            FillRepeater();

            foreach (RepeaterItem item in uploadDocRepeater.Items)
            {
                //Label docNameLbl = item.FindControl("docNameLbl") as Label;
                Panel pnl = item.FindControl("repeaterPanel") as Panel;
                //AjaxControlToolkit.AjaxFileUpload af = (AjaxControlToolkit.AjaxFileUpload)pnl.FindControl("AjaxFileUpload1");

                //docNameLbl.Text = xADList.ElementAt(idx).docName;
                //idx++;
            }

            // init upload
            uploadDIV.Visible = true;
            fakeUploadDocSubmitBtn.Visible = true;
            fakeUploadDocSaveBtn.Visible = true;
            nextBtn.Visible = false;


            ScriptManager.RegisterClientScriptBlock(updatePanelProcessDoc, typeof(UpdatePanel), "OpenModalDialog", "$('#modelDialogProcessDoc').modal('show');", true);

            //updatePanelUploadDoc.Update();
            updatePanelProcessDoc.Update();
        }

        public void handleError(string error)
        {
            uploadDocLit.Text = "<div class='alert alert-danger text-left col-md-12' role='alert'>ERROR: " + error + "</div>";
            uploadDocLit.Visible = true;
            updatePanelUploadLit.Update();
        }

        protected void submitDoc() //save seluruhnya
        {
            try
            {
                bool isAllUploaded = false;

                bool isFilledNIK = false;

                foreach (RepeaterItem item in uploadDocRepeater.Items)
                {
                    TextBox txtIsUploaded = (TextBox)item.FindControl("txtIsUploaded");
                    TextBox txtKet = (TextBox)item.FindControl("txtKet");


                    if (txtKet.Text.Trim() == "")
                    {
                        if (item.ItemIndex == 0)
                        {
                            isFilledNIK = true;
                            continue;
                        }

                        else
                        {
                            isFilledNIK = false;
                            break;
                        }
                    }

                    if (txtIsUploaded.Text == "true")
                        isAllUploaded = true;
                    else
                    {
                        isAllUploaded = false;
                        break;
                    }
                }

                //if (isAllUploaded == true)
                //{
                string retVal = "";
                BODokumenPersyaratanList docList = new BODokumenPersyaratanList();
                BODokumenNotesList notesList = new BODokumenNotesList();
                BODokumenNotesList xNotes = BLLDokumenNotes.getList(hidTxnID.Value.ToString(), "Rejected", "", "");
                int no = 1;
                string xmsg = "";
                Boolean isSafeToSave = true;

                foreach (RepeaterItem item in uploadDocRepeater.Items)
                {
                    BODokumenPersyaratan docTemp = new BODokumenPersyaratan();
                    BODokumenNotes notesTemp = new BODokumenNotes();

                    Label docIDLbl = (Label)item.FindControl("docIDLbl");
                    Label docNameLbl = (Label)item.FindControl("docNameLbl");
                    Label entryIDLbl = (Label)item.FindControl("entryIDLbl");
                    TextBox fileName = (TextBox)item.FindControl("txtFileName");
                    TextBox txtIsApproved = (TextBox)item.FindControl("txtIsApproved");
                    TextBox ket = (TextBox)item.FindControl("txtKet");
                    Label notesLbl = (Label)item.FindControl("notesLbl");

                    if (txtIsApproved.Text != "true")
                    {
                        docTemp.pinjamanID = hidTxnID.Value.ToString();
                        string docURL = Server.MapPath("~/tempUpload/") + fileName.Text;
                        docTemp.docPinjamanURL = docURL;

                        BOMasterDokumen xDoc = BLLMasterDokumen.getContent(docIDLbl.Text);

                        if (System.IO.File.Exists(docURL))
                        {
                            docTemp.docPinjamanNameOrigin = fileName.Text;
                            docTemp.docPinjamanKet = ket.Text;
                            docTemp.docID = docIDLbl.Text;
                            docTemp.docType = System.IO.Path.GetExtension(docURL);
                            docTemp.docSize = System.IO.File.ReadAllBytes(docURL).Length;
                            docTemp.docContent = System.IO.File.ReadAllBytes(docURL);
                            docTemp.lastOperator = Session["kopkariUserID"].ToString();
                        }

                        else
                        {
                            xmsg += "- Dimohon untuk mengupload kembali " + docNameLbl.Text + "<br />";
                            handleError(xmsg);
                            isSafeToSave = false;
                        }

                        notesTemp.pinjamanID = hidTxnID.Value.ToString();
                        notesTemp.docID = docIDLbl.Text;
                        notesTemp.docStatus = "Need Approval";
                        notesTemp.lastOperator = Session["kopkariUserID"].ToString();
                        notesTemp.lastFeedBack = notesLbl.Text;

                        BODokumenNotes searchNotes = BLLDokumenNotes.getContent(docTemp.pinjamanID, no, docIDLbl.Text);

                        if (searchNotes != null)
                        {
                            notesTemp.feedBackLog = searchNotes.feedBackLog;
                        }

                        else
                        {
                            notesTemp.feedBackLog = "-";
                        }

                        if (entryIDLbl.Text != "")
                        {
                            docTemp.entryID = Convert.ToInt32(entryIDLbl.Text);
                            notesTemp.entryID = Convert.ToInt32(entryIDLbl.Text);
                        }

                        docList.Add(docTemp);
                        notesList.Add(notesTemp);
                        no++;
                    }

                }

                if (isSafeToSave)
                {
                    string xStatusCode = "";

                    if (xNotes != null)//upload ulang
                    {
                        xStatusCode = "PSA011";
                    }
                    else//upload pertama kali
                    {
                        xStatusCode = "PSA009";
                    }

                    BOPinjamanStatus xUpdateStatus = new BOPinjamanStatus();

                    xUpdateStatus.pinjamanID = hidTxnID.Value.ToString();
                    xUpdateStatus.lastOperator = Session["kopkariUserID"].ToString(); ;
                    xUpdateStatus.statusCode = xStatusCode;
                    xUpdateStatus.description = "";

                    retVal = BLLDokumenPersyaratan.Save(docList, notesList, xUpdateStatus);

                    if (retVal.Contains("EXCEPTION"))
                    {
                        uploadDocLit.Text = "<div class='alert alert-danger text-center' style='width:375px;' role='alert'>" + retVal + "</div>";
                        uploadDocLit.Visible = true;
                        updatePanelUploadLit.Update();
                    }
                    else
                    {
                        string xjs = @"setTimeout(function() {updateSuccess(); });";
                        ScriptManager.RegisterClientScriptBlock(updatePanelUploadDoc, typeof(UpdatePanel), "OpenModalDialog", xjs, true);

                        uploadDocLit.Text = "";
                        uploadDocLit.Visible = false;
                        ScriptManager.RegisterClientScriptBlock(updatePanelUploadDoc, typeof(UpdatePanel), "OpenModalDialog", "$('#modelDialogUploadDoc').modal('hide');", true);
                        //FillRepeater();
                        updatePanelUploadDoc.Update();
                        FillGrid(hdSortEx.Value, hdSortDir.Value);
                    }
                }
               
            }
            catch (Exception ex)
            {
                handleError(ex.Message);
            }
            //}
            //else
            //{
            //    uploadDocLit.Text = "<div class='alert alert-warning text-center mt-2 col-md-12' role='alert'>Semua Dokumen Harus Diupload / Diperbaharui</div>";
            //    uploadDocLit.Visible = true;
            //    updatePanelUploadLit.Update();
            //}

        }

        protected void saveDoc() //save sebagian
        {
            bool isEdit = false;
            foreach (RepeaterItem item in uploadDocRepeater.Items)
            {
                TextBox txtIsEdit = (TextBox)item.FindControl("txtIsEdit");

                if (txtIsEdit.Text == "true")
                {
                    isEdit = true;
                    break;
                }
            }

            bool isAllUploaded = false;
            foreach (RepeaterItem item in uploadDocRepeater.Items)
            {
                TextBox txtIsUploaded = (TextBox)item.FindControl("txtIsUploaded");

                if (txtIsUploaded.Text == "true")
                    isAllUploaded = true;
                else
                {
                    isAllUploaded = false;
                    break;
                }

            }

            //if(isAllUploaded == true)
            //{
            //    submitDoc();
            //}
            //else
            //{

            isEdit = true;

            //if (isEdit == true)
            //{
                string retVal = "";
                BODokumenPersyaratanList docList = new BODokumenPersyaratanList();
                BODokumenNotesList notesList = new BODokumenNotesList();

                foreach (RepeaterItem item in uploadDocRepeater.Items)
                {
                    BODokumenPersyaratan docTemp = new BODokumenPersyaratan();
                    BODokumenNotes notesTemp = new BODokumenNotes();

                    Label docIDLbl = (Label)item.FindControl("docIDLbl");
                    Label entryIDLbl = (Label)item.FindControl("entryIDLbl");
                    TextBox fileName = (TextBox)item.FindControl("txtFileName");
                    TextBox txtIsApproved = (TextBox)item.FindControl("txtIsApproved");
                    TextBox ket = (TextBox)item.FindControl("txtKet");
                    TextBox txtIsEdit = (TextBox)item.FindControl("txtIsEdit");

                    if (fileName.Text != "")
                    {
                        docTemp.pinjamanID = hidTxnID.Value.ToString();
                        docTemp.docPinjamanURL = Server.MapPath("~/tempUpload/") + fileName.Text;
                        docTemp.docPinjamanNameOrigin = fileName.Text;
                        docTemp.docPinjamanKet = ket.Text;
                        docTemp.docID = docIDLbl.Text;
                        docTemp.docType = System.IO.Path.GetExtension(Server.MapPath("~/tempUpload/") + fileName.Text);
                        docTemp.docSize = System.IO.File.ReadAllBytes(Server.MapPath("~/tempUpload/") + fileName.Text).Length;
                        docTemp.docContent = System.IO.File.ReadAllBytes(Server.MapPath("~/tempUpload/") + fileName.Text);
                        docTemp.lastOperator = Session["kopkariUserID"].ToString();

                        notesTemp.pinjamanID = hidTxnID.Value.ToString();
                        notesTemp.docID = docIDLbl.Text;
                        notesTemp.docStatus = "Draft"; // belom perlu kirim
                        notesTemp.lastOperator = Session["kopkariUserID"].ToString();

                        if (entryIDLbl.Text != "")
                        {
                            docTemp.entryID = Convert.ToInt32(entryIDLbl.Text);
                            notesTemp.entryID = Convert.ToInt32(entryIDLbl.Text);
                        }

                        docList.Add(docTemp);
                        notesList.Add(notesTemp);
                    }

                }

                try
                {
                    retVal = BLLDokumenPersyaratan.Save(docList, notesList, null);

                    if (retVal.Contains("EXCEPTION"))
                    {
                        uploadDocLit.Text = "<div class='alert alert-danger text-center' style='width:375px;' role='alert'>" + retVal + "</div>";
                        uploadDocLit.Visible = true;
                        updatePanelUploadLit.Update();
                    }
                    else
                    {
                        string xjs = @"setTimeout(function() {updateSuccess(); });";
                        ScriptManager.RegisterClientScriptBlock(updatePanelUploadDoc, typeof(UpdatePanel), "OpenModalDialog", xjs, true);

                        uploadDocLit.Text = "";
                        uploadDocLit.Visible = false;
                        FillRepeater();
                        updatePanelUploadDoc.Update();
                    }
                }
                catch (Exception ex)
                {
                    uploadDocLit.Text = "<div class='alert alert-danger text-center' style='width:375px;' role='alert'>ERROR: " + ex.Message + "</div>";
                    uploadDocLit.Visible = true;
                    updatePanelUploadLit.Update();
                }
            //}
            //else
            //{
            //    uploadDocLit.Text = "<div class='alert alert-warning text-center mt-2 col-md-12' role='alert'>Minimal satu Dokumen Harus Diupload/Diperbaharui</div>";
            //    uploadDocLit.Visible = true;
            //    updatePanelUploadLit.Update();
            //}
            //}          
        }
        #endregion
        //protected string updatePinjamanStatus(string xStatusCode, string xTxn)
        //{
        //    BOProcessResult retVal = new BOProcessResult();
        //    BOPinjamanStatus xUpdateStatus = new BOPinjamanStatus();

        //    xUpdateStatus.lastOperator = Session["kopkariUserID"].ToString(); ;
        //    xUpdateStatus.statusCode = xStatusCode;
        //    xUpdateStatus.description = "";
        //    if (xTxn == null)
        //    {
        //        xUpdateStatus.pinjamanID = hidTxnID.Value;
        //    }
        //    else
        //    {
        //        xUpdateStatus.pinjamanID = xTxn;
        //    }

        //    retVal = BLLPinjamanStatus.saveStatus(xUpdateStatus);

        //    if(retVal.xMessage == null)
        //    {
        //        return "success";
        //    }

        //    return retVal.xMessage;

        //}
        #endregion
        #endregion
    }
}