using flashPriceFx.MiniMarket;
using flashPriceFx.Product;
using flashPriceFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace flashPrice.masterData
{
    public partial class masterMinimarket : System.Web.UI.Page
    {
        BOProcessResult retVal = new BOProcessResult();
        protected void Page_Load(object sender, EventArgs e)
        {
            FillGrid(hdSortEx.Value, hdSortDir.Value);
        }

        #region gvmain

        protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // urusan logo minimarket
                Image imgMiniMarketType = ((Image)e.Row.FindControl("imgMiniMarketType"));
                String miniMarketType = (String)DataBinder.Eval(e.Row.DataItem, "miniMarketType");
                imgMiniMarketType.ImageUrl = miniMarketType == "Indomaret" ? @"~\assets\images\indomaret_logo.png" : @"~\assets\images\alfamart_logo.png";

                String minimarketID = (String)DataBinder.Eval(e.Row.DataItem, "minimarketID");

                LinkButton detailBtn = ((LinkButton)e.Row.FindControl("detailBtn"));
                detailBtn.Text = "<i class='fa fa-search mr-2' ></i>" + minimarketID;
            }
        }

        protected void gvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMain.PageIndex = e.NewPageIndex;
            FillGrid(hdSortEx.Value, hdSortDir.Value);
        }


        protected void FillGrid(string sortBy, string sortDir)
        {
            String searchText = searchTextBox.Text;

            BOMiniMarketList listMinimarket = BLLMiniMarket.getListMinimarketQueries(searchText, sortBy, sortDir);

            gvMain.DataSource = listMinimarket;
            gvMain.DataBind();

            updGridView.Update();

        }

        protected void gvMain_Sorting(object sender, GridViewSortEventArgs e)
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

        #endregion

        protected void searchBtn_Click(object sender, EventArgs e)
        {
            FillGrid(hdSortEx.Value, hdSortDir.Value);
        }


        #region product detail

        protected void dataDetail_hiddenBtn_Click(object sender, EventArgs e)
        {
            string dataID = hiddenDataID.Value;
            loadData(dataID);
        }

        private void loadData(string dataID)
        {
            BOMiniMarket xBO = BLLMiniMarket.getContentByID(dataID);

            minimarketNameTb.Text = xBO.miniMarketName;
            minimarketAddressTb.Text = xBO.miniMarketAddress;
            minimarketTypeDD.SelectedValue = xBO.miniMarketType;
            minimarketLattitudeTb.Text = xBO.miniMarketLattitude.ToString();
            minimarketLongitudeTb.Text = xBO.miniMarketLongitude.ToString();

            deleteBtn.Visible = true;


            isUpdateOrInsertHid.Value = "updateData";

            ScriptManager.RegisterClientScriptBlock(updatePanelProductDetail, typeof(UpdatePanel), "OpenModalDialog", "setTimeout(function(){$('#modalDialogProductDetail').modal('show');},500)", true);
            updatePanelProductDetail.Update();
            updAction.Update();
        }

        private void newData()
        {
            minimarketNameTb.Text = string.Empty;
            minimarketAddressTb.Text = string.Empty;
            minimarketTypeDD.SelectedValue = string.Empty;
            minimarketLattitudeTb.Text = string.Empty;
            minimarketLongitudeTb.Text = string.Empty;

            deleteBtn.Visible = false;

            isUpdateOrInsertHid.Value = "newData";

            ScriptManager.RegisterClientScriptBlock(updatePanelProductDetail, typeof(UpdatePanel), "OpenModalDialog", "setTimeout(function(){$('#modalDialogProductDetail').modal('show');},500)", true);
            updatePanelProductDetail.Update();
            updAction.Update();
        }

        #endregion

        protected void save_hiddenBtn_Click(object sender, EventArgs e)
        {
            BOMiniMarket xMinimarket = new BOMiniMarket();
            xMinimarket.miniMarketID = hiddenDataID.Value;
            xMinimarket.miniMarketName = minimarketNameTb.Text.Trim();
            xMinimarket.miniMarketAddress = minimarketAddressTb.Text.Trim();
            xMinimarket.miniMarketLattitude = decimal.Parse(minimarketLattitudeTb.Text);
            xMinimarket.miniMarketLongitude = decimal.Parse(minimarketLongitudeTb.Text);
            xMinimarket.miniMarketType = minimarketTypeDD.SelectedValue;

            if (isUpdateOrInsertHid.Value == "updateData")
            {
                retVal = BLLMiniMarket.manageMinimarket(xMinimarket, "update");
            }

            else if (isUpdateOrInsertHid.Value == "newData")
            {
                retVal = BLLMiniMarket.manageMinimarket(xMinimarket, "insert");
            }

            if (retVal.isSuccess)
            {
                FillGrid(hdSortEx.Value, hdSortDir.Value);
                string xjs = @"setTimeout(function() {swalSuccess(); $('#modalDialogProductDetail').modal('hide');}, 500);";
                ScriptManager.RegisterClientScriptBlock(this, typeof(UpdatePanel), "OpenModalDialog", xjs, true);

                updatePanelProductDetail.Update();
            }

            else
            {
                string xjs = @"setTimeout(function() {swalFailed(); }, 500);";
                ScriptManager.RegisterClientScriptBlock(this, typeof(UpdatePanel), "OpenModalDialog", xjs, true);

                updatePanelProductDetail.Update();
            }
        }

        protected void delete_hiddenBtn_Click(object sender, EventArgs e)
        {
            BOMiniMarket xMinimarket = new BOMiniMarket();
            xMinimarket.miniMarketID = hiddenDataID.Value;

            BOProcessResult retVal = BLLMiniMarket.manageMinimarket(xMinimarket, "delete");

            if (retVal.isSuccess)
            {
                FillGrid(hdSortEx.Value, hdSortDir.Value);
                string xjs = @"setTimeout(function() {swalSuccess(); $('#modalDialogProductDetail').modal('hide');}, 500);";
                ScriptManager.RegisterClientScriptBlock(this, typeof(UpdatePanel), "OpenModalDialog", xjs, true);

                updatePanelProductDetail.Update();
            }

            else
            {
                string xjs = @"setTimeout(function() {swalFailed(); }, 500);";
                ScriptManager.RegisterClientScriptBlock(this, typeof(UpdatePanel), "OpenModalDialog", xjs, true);

                updatePanelProductDetail.Update();
            }
        }

        protected void addBtn_Click(object sender, EventArgs e)
        {
            newData();
        }
    }
}