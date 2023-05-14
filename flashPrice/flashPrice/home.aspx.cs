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
        private static int pageSize = 12;
        protected void Page_Load(object sender, EventArgs e)
        {


        }


        protected void navSearchBtn_Click(object sender, EventArgs e)
        {
            fillResult(1, pageSize);
        }

        protected void Page_Changed(object sender, EventArgs e)
        {
            int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
            fillResult(pageIndex, pageSize);
        }

        protected void fillResult(int pageIndex, int PageSize)
        {
            try
            {
                String searchText = navSearchTextBox.Text;
                String categoryProduct = categoryProductDD.SelectedValue;

                int startRow = (pageSize * (pageIndex - 1));

                int maxRow = pageSize;

                if (startRow == 1)  maxRow = 12;

                if (pageIndex > 1) startRow = startRow + 1;

                String sortBy = "productID";
                String sortDir = "asc";

                BOProductList listProduct = BLLProduct.getListProduct(searchText, categoryProduct, sortBy, sortDir, startRow, maxRow);
                int jmlBaris = int.Parse(BLLProduct.getCountListProduct(searchText, categoryProduct, sortBy, sortDir, startRow, maxRow).ToString());

                if (listProduct == null)
                {
                    litError.Text = "Produk tidak ditemukan";
                    resultRepeater.DataSource = null;
                    resultRepeater.DataBind();
                }
                else
                {
                    
                    double dblPageCount = (double)((decimal)jmlBaris / pageSize);
                    int pageCount = (int)Math.Ceiling(dblPageCount);

                    if (pageIndex != 1 && pageIndex != pageCount) { 
                        listProduct.RemoveAt(listProduct.Count - 1); // buat buang element terakhir yang gk kepake
                    }


                    resultRepeater.DataSource = listProduct;
                    resultRepeater.DataBind();

                    litError.Text = "";
                    updatePanelSearchResultRepeater.Update();
                }

                PopulatePager(jmlBaris, pageIndex);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        #region pagination
        private void PopulatePager(int recordCount, int currentPage)
        {
            double dblPageCount = (double)((decimal)recordCount / pageSize);
            int pageCount = (int)Math.Ceiling(dblPageCount);
            List<ListItem> pages = new List<ListItem>();
            string displ = "";

            if (pageCount > 0)
            {
                paginationDiv.Visible = true;
                pages.Add(new ListItem("<b>First</b>", "1", currentPage > 1));
                for (int i = 1; i <= pageCount; i++)
                {
                    if (i <= 5 || (i >= currentPage - 5 && i <= currentPage + 5) || i > pageCount - 5)
                    {
                        displ = i == currentPage ? "<b>" + i.ToString() + "</b>" : i.ToString();
                        pages.Add(new ListItem(displ, i.ToString(), i != currentPage));
                    }
                }
                pages.Add(new ListItem("<b>Last</b>", pageCount.ToString(), currentPage < pageCount));
            }

            else
            {
                paginationDiv.Visible = false;
            }

            rptPager.DataSource = pages;
            rptPager.DataBind();
            hdnPageIdx.Value = currentPage.ToString();
        }
        #endregion

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
            string productID = hiddenProductID.Value;
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