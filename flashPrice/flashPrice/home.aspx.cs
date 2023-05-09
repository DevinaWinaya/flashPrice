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
            fillResult(searchText);
        }
        protected void fillResult(String searchText)
        {
            try
            {
                BOProductList listProduct = BLLProduct.getListProductQuery(searchText);

                if (listProduct == null)
                {
                    litError.Text = "Produk tidak ditemukan";
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
    }
}