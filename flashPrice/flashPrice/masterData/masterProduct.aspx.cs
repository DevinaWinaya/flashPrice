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
    public partial class masterProduct : System.Web.UI.Page
    {
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

                bool isSponsorship = (bool)DataBinder.Eval(e.Row.DataItem, "isSponsorship");
                Label isSponsorshipLbl = ((Label)e.Row.FindControl("isSponsorshipLbl"));

                if (isSponsorship)
                {
                    isSponsorshipLbl.Text = "✅";
                    isSponsorshipLbl.CssClass = "text-success";
                }
                else
                {
                    isSponsorshipLbl.Text = "❌";
                    isSponsorshipLbl.CssClass = "text-danger";
                }

                String productID = (String)DataBinder.Eval(e.Row.DataItem, "productID");

                LinkButton detailBtn = ((LinkButton)e.Row.FindControl("detailBtn"));
                detailBtn.Text = "<i class='fa fa-search mr-2' ></i>" + productID;
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
            String categoryProduct = categoryProductDD.SelectedValue;
            String miniMarketTarget = miniMarketSearchTextBox.Text.Trim();

            BOProductList listProduct = BLLProduct.getListProduct(searchText, categoryProduct, "", false, sortBy, sortDir, 0, int.MaxValue);

            gvMain.DataSource = listProduct;
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

        protected void productDetailBtn_Click(object sender, EventArgs e)
        {
            string productID = hiddenProductID.Value;
            loadProduct(productID);
        }

        private void loadProduct(string productID)
        {
            BOProduct xBO = BLLProduct.getContent(productID);

            productNameTb.Text = xBO.productName;
            productPriceTb.Text = xBO.productPrice.ToString();
            isSponsorshipDD.SelectedValue = xBO.isSponsorship == true ? "T" : "F";

            ScriptManager.RegisterClientScriptBlock(updatePanelProductDetail, typeof(UpdatePanel), "OpenModalDialog", "setTimeout(function(){$('#modalDialogProductDetail').modal('show');},500)", true);
            updatePanelProductDetail.Update();
            updAction.Update();
        }

        #endregion

        protected void save_hiddenBtn_Click(object sender, EventArgs e)
        {
            BOProduct xProduct = new BOProduct();
            xProduct.productID = hiddenProductID.Value;
            xProduct.productName = productNameTb.Text.Trim();
            xProduct.productPrice = int.Parse(productPriceTb.Text);
            xProduct.isSponsorship = isSponsorshipDD.SelectedValue == "T" ? true : false;

            BOProcessResult retVal = BLLProduct.manageProduct(xProduct, "update");

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
            BOProduct xProduct = new BOProduct();
            xProduct.productID = hiddenProductID.Value;

            BOProcessResult retVal = BLLProduct.manageProduct(xProduct, "delete");

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
    }
}