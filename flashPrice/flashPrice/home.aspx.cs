using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace flashPrice.pages
{
    public partial class home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void searchBtn_Click(object sender, EventArgs e)
        {

        }

        protected void FillDocStatus()
        {
            BODokumenNotesList xDocList = BLLDokumenNotes.getListDocStatus(hidTxnID.Value, "dn.docID", "asc");

            //BODokumenPersyaratanList xDocList = BLLDokumenPersyaratan.getList(xTxn.pinjamanID, "docID", "asc");
            resultRepeater.DataSource = xDocList;
            resultRepeater.DataBind();
        }

        protected void resultRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void resultRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
    }
}