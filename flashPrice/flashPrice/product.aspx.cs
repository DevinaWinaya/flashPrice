using flashPriceFx.MiniMarket;
using flashPriceFx.Product;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace flashPrice.pages
{
    public partial class product : System.Web.UI.Page
    {
        private static int pageSize = 12;
        private static int pageSizeSponsorship = 4;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {             
                fillResult(1, pageSize, "productID", "ASC");
            }
        }

   

        #region calculate distance from coordinates
        public class distance
        {
            public int node { get; set; }
            public string MiniMarketID { get; set; }
            public string MiniMarketName { get; set; }
            public int distanceFromMe { get; set; }
            public bool isItMeWithMiniMarket { get; set; } // true distance between me and minimarket if its not 
            public List<distance> Level1 { get; set; }
            public List<distance> Level2 { get; set; }

        }

        // buat nampung hasil
        public List<distance> resultMinimarket = new List<distance>();
        public BOMiniMarketList resultMinimarketBFS = new BOMiniMarketList();

        public class category
        {
            public int node { get; set; }
            public string MiniMarketName { get; set; }
            public double categoryLattitude { get; set; }
            public double categoryLongitude { get; set; }
            public int distanceFromMe { get; set; }

        }

        // build category nya
        public List<category> catA = new List<category>();
        public List<category> catB = new List<category>();
        public List<category> catC = new List<category>();
        public List<category> catD = new List<category>();
        public List<category> catE = new List<category>();
        public List<category> catALL = new List<category>();


        public List<category> minCatA = new List<category>();
        public List<category> minCatB = new List<category>();
        public List<category> minCatC = new List<category>();
        public List<category> minCatD = new List<category>();
        public List<category> minCatE = new List<category>();
        public List<category> minCatAll = new List<category>();


        /*
            kategori A <= 1000 m
            kategori B 1000 m < jarak dari kita <= 5000 m
            kategori C 5000 m < jarak dari kita <= 10000 m
            Kategori D 10000 m < jarak dari kita <= 25000 m
            kategori E 25000 m < jarak dari kita
        */

        #endregion

        protected void navSearchBtn_Click(object sender, EventArgs e)
        {
                fillResult(1, pageSize, "productPrice", "asc");
        }

        #region result repeater normal product
        protected void resultRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }

        protected void resultRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void Page_Changed(object sender, EventArgs e)
        {
            int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
            fillResult(pageIndex, pageSize, hdSortEx.Value, hdSortDir.Value);
        }

        protected void fillResult(int pageIndex, int PageSize, string sortBy, string sortDir)
        {
            try
            {
                String searchText = navSearchTextBox.Text;
                String categoryProduct = categoryProductDD.SelectedValue;

                int startRow = (pageSize * (pageIndex - 1));

                int maxRow = pageSize;

                if (startRow == 1) maxRow = 12;

                if (pageIndex > 1) startRow = startRow + 1;

                if (sortBy == "")
                {
                    sortBy = "productID";
                    sortDir = " ";
                }

                BOProductList listProduct = BLLProduct.getListProduct(searchText, categoryProduct, false, sortBy, sortDir, startRow, maxRow);
                int jmlBaris = int.Parse(BLLProduct.getCountListProduct(searchText, categoryProduct, false, sortBy, sortDir, startRow, maxRow).ToString());

                if (listProduct == null)
                {
                    litError.Text = "<div class='alert alert-warning text-center mt-4' style='margin-bottom:500px;'> Produk tidak ditemukan </div>";
                    resultRepeater.DataSource = null;
                    resultRepeater.DataBind();
                }
                else
                {
                    double dblPageCount = (double)((decimal)jmlBaris / pageSize);
                    int pageCount = (int)Math.Ceiling(dblPageCount);

                    if (pageIndex != 1 && pageIndex != pageCount)
                    {
                        listProduct.RemoveAt(listProduct.Count - 1); // buat buang element terakhir yang gk kepake
                    }

                    resultRepeater.DataSource = listProduct;
                    resultRepeater.DataBind();

                    litError.Text = "";

                    hdSortEx.Value = sortBy;
                    hdSortDir.Value = sortDir;
                    updatePanelSearchResultRepeater.Update();
                }

                updGridView.Update();
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
            //productDescPopupLbl.Text = xBO.productDescription == "" || xBO.productDescription == null ? "Tidak ada deskripsi produk" : xBO.productDescription;
            productImageUrlPopup.ImageUrl = xBO.productImageUrl;
            miniMarketImageUrlPopup.ImageUrl = xBO.miniMarketType == "Indomaret" ? @"~\assets\images\indomaret_logo.png" : @"~\assets\images\alfamart_logo.png";

            ScriptManager.RegisterClientScriptBlock(updatePanelProductDetail, typeof(UpdatePanel), "OpenModalDialog", "setTimeout(function(){$('#modalDialogProductDetail').modal('show');},500)", true);
            updatePanelProductDetail.Update();
            updAction.Update();
        }

        #endregion

        #region gvmain

        protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // urusan logo minimarket
                Image imgMiniMarketType = ((Image)e.Row.FindControl("imgMiniMarketType"));
                String miniMarketType = (String)DataBinder.Eval(e.Row.DataItem, "miniMarketType");
                imgMiniMarketType.ImageUrl = miniMarketType == "Indomaret" ? @"~\assets\images\indomaret_logo.png" : @"~\assets\images\alfamart_logo.png";

                //  kategori A <= 1000 m
                //  kategori B 1000 m < jarak dari kita <= 5000 m
                //  kategori C 5000 m < jarak dari kita <= 10000 m
                //  Kategori D 10000 m < jarak dari kita <= 25000 m
                //  kategori E 25000 m < jarak dari kita

                int distanceFromMe = (int)DataBinder.Eval(e.Row.DataItem, "distanceFromMe");

                if (distanceFromMe <= 1000)
                {
                    e.Row.BackColor = System.Drawing.Color.Honeydew;
                }
                
                if (distanceFromMe > 1000 && distanceFromMe <= 5000)
                {
                    e.Row.BackColor = System.Drawing.Color.LightYellow;
                }

                if (distanceFromMe > 5000 && distanceFromMe <= 10000)
                {
                    e.Row.BackColor = System.Drawing.Color.LightCyan;
                }

                if (distanceFromMe > 10000 && distanceFromMe <= 25000)
                {
                    e.Row.BackColor = System.Drawing.Color.LightGray;
                }

                if (distanceFromMe > 25000)
                {
                    e.Row.BackColor = System.Drawing.Color.LightPink;
                }
            }
        }

        protected void gvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMain.PageIndex = e.NewPageIndex;
        }


        protected void FillGrid(BOMiniMarketList xBO)
        {
        }
        #endregion


        #region result repeater sponsorship product
        protected void sponsorRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }

        protected void sponsorRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void sponsorRepeater_Page_Changed(object sender, EventArgs e)
        {
            int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        }

        #region pagination
        //private void PopulatePagerSponsporRepeater(int recordCount, int currentPage)
        //{
        //    double dblPageCount = (double)((decimal)recordCount / pageSizeSponsorship);
        //    int pageCount = (int)Math.Ceiling(dblPageCount);
        //    List<ListItem> pages = new List<ListItem>();
        //    string displ = "";

        //    if (pageCount > 0)
        //    {
        //        paginationDiv.Visible = true;
        //        pages.Add(new ListItem("<b>First</b>", "1", currentPage > 1));
        //        for (int i = 1; i <= pageCount; i++)
        //        {
        //            if (i <= 5 || (i >= currentPage - 5 && i <= currentPage + 5) || i > pageCount - 5)
        //            {
        //                displ = i == currentPage ? "<b>" + i.ToString() + "</b>" : i.ToString();
        //                pages.Add(new ListItem(displ, i.ToString(), i != currentPage));
        //            }
        //        }
        //        pages.Add(new ListItem("<b>Last</b>", pageCount.ToString(), currentPage < pageCount));
        //    }

        //    else
        //    {
        //        paginationDiv.Visible = false;
        //    }

        //    sponsorRepeaterPager.DataSource = pages;
        //    sponsorRepeaterPager.DataBind();
        //    hdnPageIdx.Value = currentPage.ToString();
        //}
        #endregion

        #endregion

    }
}