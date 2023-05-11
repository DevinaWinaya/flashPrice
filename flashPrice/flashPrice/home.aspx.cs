using flashPriceFx.Product;
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


        protected void navSearchBtn_Click(object sender, EventArgs e)
        {
            String searchText = navSearchTextBox.Text;
            String categoryProduct = categoryProductDD.SelectedValue; 
            fillResult(searchText, categoryProduct);
        }
        protected void fillResult(String searchText, String categoryProduct)
        {
            try
            {
                BOProductList listProduct = BLLProduct.getListProductQuery(searchText, categoryProduct);

                if (listProduct == null)
                {
                    litError.Text = "Produk tidak ditemukan";
                    resultRepeater.DataSource = null;
                    resultRepeater.DataBind();
                }
                else
                {
                    resultRepeater.DataSource = listProduct;
                    resultRepeater.DataBind();

                    litError.Text = "";
                    updatePanelSearchResultRepeater.Update();
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                
            }
        }

        #region result repeater
        protected void resultRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }

        protected void resultRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        #endregion

        #region product detail

        protected void productDetailBtn_Click(object sender, EventArgs e)
        {
            string productID= hiddenProductID.Value;
            loadProduct(productID);
        }

        private void loadProduct(string productID)
        {
            BOProduct xBO = BLLProduct.getContent(productID);

            productNamePopupLbl.Text = xBO.productName;
            productPricePopupLbl.Text = xBO.productPrice.ToString("#0");
            productDescPopupLbl.Text = xBO.productDescription == "" || xBO.productDescription == null ? "Tidak ada deskripsi produk" : xBO.productDescription;
            productImageUrlPopup.ImageUrl = xBO.productImageUrl;
            miniMarketImageUrlPopup.ImageUrl = xBO.miniMarketType == "Indomaret" ? @"~\assets\images\indomaret_logo.png" : @"~\assets\images\alfamart_logo.png";

            ScriptManager.RegisterClientScriptBlock(updatePanelProductDetail, typeof(UpdatePanel), "OpenModalDialog", "setTimeout(function(){$('#modalDialogProductDetail').modal('show');},500)", true);
            updatePanelProductDetail.Update();
            updAction.Update();
        }

        #endregion
    }
}