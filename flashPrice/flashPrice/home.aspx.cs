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
    public partial class home : System.Web.UI.Page
    {
        private static int pageSize = 12;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillResult(1, pageSize, "productID", "ASC");

            }
        }

        #region selenium

        private IWebDriver _webDriver;

        [SetUp]
        public void SetUp()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            _webDriver = new ChromeDriver();
        }

        [TearDown]
        public void TearDown()
        {
            _webDriver.Quit();
        }

        [Test]
        public void Test()
        {
            _webDriver.Navigate().GoToUrl("https://alfagift.id/c/makanan-602f8240898b4705ec586ab3");
            //#__layout > div > div:nth-child(2) > div.content > div > div:nth-child(2) > div:nth-child(2) > div.row.list-product-catalog.pt-3 > div > p.text-lg.fw7.mb-1

            _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            var x = _webDriver.FindElement(By.CssSelector("div > div:nth-child(2) > div.content > div > div:nth-child(2) > div:nth-child(2) > div.row.list-product-catalog.pt-3 > div > p.text-lg.fw7.mb-1']"));
        }
        #endregion


        #region best-first-search double 
        //public static LinkedList<Tuple<double, int>>[] graph;
        //public static List<int> resultX = new List<int>();

        //// Function for adding edges to graph
        //public static void addedge(int x, int y, double cost)
        //{
        //    graph[x].AddLast(new Tuple<double, int>(cost, y));
        //    graph[y].AddLast(new Tuple<double, int>(cost, x));
        //}

        //// Function for finding the minimum weight element.
        //public static Tuple<double, int> get_min(LinkedList<Tuple<double, int>> pq)
        //{
        //    // Assuming the maximum wt can be of 1e5.
        //    Tuple<double, int> curr_min = new Tuple<double, int>(100000, 100000);
        //    foreach (var ele in pq)
        //    {
        //        if (ele.Item1 == curr_min.Item1)
        //        {
        //            if (ele.Item2 < curr_min.Item2)
        //            {
        //                curr_min = ele;
        //            }
        //        }
        //        else
        //        {
        //            if (ele.Item1 < curr_min.Item1)
        //            {
        //                curr_min = ele;
        //            }
        //        }
        //    }

        //    return curr_min;
        //}


        // Function For Implementing Best First Search
        // Gives output path having lowest cost

        //public static void best_first_search(int actual_Src, int target, int n)
        //{
        //    int[] visited = new int[n];
        //    for (int i = 0; i < n; i++)
        //    {
        //        visited[i] = 0;
        //    }

        //    // MIN HEAP priority queue
        //    LinkedList<Tuple<double, int>> pq = new LinkedList<Tuple<double, int>>();

        //    // sorting in pq gets done by first value of pair
        //    pq.AddLast(new Tuple<double, int>(0, actual_Src));
        //    int s = actual_Src;
        //    visited[s] = 1;
        //    while (pq.Count > 0)
        //    {
        //        Tuple<double, int> curr_min = get_min(pq);
        //        int x = curr_min.Item2;
        //        pq.Remove(curr_min);
        //        resultX.Add(x);
        //        // Displaying the path having lowest cost
        //        if (x == target)
        //            break;

        //        LinkedList<Tuple<double, int>> list = graph[x];
        //        foreach (var val in list)
        //        {
        //            if (visited[val.Item2] == 0)
        //            {
        //                visited[val.Item2] = 1;
        //                pq.AddLast(new Tuple<double, int>(val.Item1, val.Item2));
        //            }
        //        }
        //    }
        //}
        #endregion



        #region best-first search
        public static LinkedList<Tuple<int, int>>[] graph;
        public static List<int> resultX = new List<int>();
        public static List<int> findMaxV = new List<int>();


        // Function for adding edges to graph
        public static void addedge(int x, int y, int cost)
        {
            graph[x].AddLast(new Tuple<int, int>(cost, y));
            graph[y].AddLast(new Tuple<int, int>(cost, x));

        }

        // Function for finding the minimum weight element.
        public static Tuple<int, int> get_min(LinkedList<Tuple<int, int>> pq)
        {
            // Assuming the maximum wt can be of 1e5.
            Tuple<int, int> curr_min = new Tuple<int, int>(100000, 100000);
            foreach (var ele in pq)
            {
                if (ele.Item1 == curr_min.Item1)
                {
                    if (ele.Item2 < curr_min.Item2)
                    {
                        curr_min = ele;
                    }
                }
                else
                {
                    if (ele.Item1 < curr_min.Item1)
                    {
                        curr_min = ele;
                    }
                }
            }

            return curr_min;
        }

        public static void best_first_search(int actual_Src, int target, int n)
        {
            try
            {

                int[] visited = new int[n];
                for (int i = 0; i < n; i++)
                {
                    visited[i] = 0;
                }

                // MIN HEAP priority queue
                LinkedList<Tuple<int, int>> pq = new LinkedList<Tuple<int, int>>();

                // sorting in pq gets done by first value of pair
                pq.AddLast(new Tuple<int, int>(0, actual_Src));
                int s = actual_Src;
                visited[s] = 1;
                while (pq.Count > 0)
                {

                    Tuple<int, int> curr_min = get_min(pq);
                    int x = curr_min.Item2;
                    pq.Remove(curr_min);

                    resultX.Add(x);
                    // Displaying the path having lowest cost
                    if (x == target)
                        break;

                    // jagain buat x nya gk null
                    //if (graph[x] != null)
                    //{
                    LinkedList<Tuple<int, int>> list = graph[x];

                    foreach (var val in list)
                    {
                        if (visited[val.Item2] == 0)
                        {
                            visited[val.Item2] = 1;
                            pq.AddLast(new Tuple<int, int>(val.Item1, val.Item2));
                        }
                    }

                    //}

                    //else
                    //{
                    //    break;
                    //}

                }
            }
            catch (Exception x)
            {
                string error = x.Message;
            }

        }
        public static void Main()
        {

        }
        #endregion

        #region calculate distance from coordinates

        //public class distance
        //{
        //    public int node { get; set; }
        //    public string MiniMarketName { get; set; }
        //    public double distanceFrom { get; set; }
        //}

        //private void testingLocation()
        //{
        //    double lat1 = Convert.ToDouble(hiddenMyLatitude.Value.Replace(".", ","));
        //    double lon1 = Convert.ToDouble(hiddenMyLongitude.Value.Replace(".", ","));

        //    BOMiniMarketList miniMarketList = BLLMiniMarket.getListAllMiniMarket();
        //    List<distance> listX = new List<distance>();
        //    int xx = 0;

        //    foreach (BOMiniMarket xMinimarket in miniMarketList)
        //    {
        //        distance x = new distance();
        //        x.node = xx + 1;
        //        x.MiniMarketName = xMinimarket.miniMarketName;
        //        x.distanceFrom = getDistanceFromLatLonInKm(lat1, lon1, double.Parse(xMinimarket.miniMarketLattitude.ToString()), double.Parse(xMinimarket.miniMarketLongitude.ToString()));
        //        listX.Add(x);
        //    }

        //    gvMain.DataSource = listX.OrderBy(distance => distance.distanceFrom).Take(5).ToList();
        //    gvMain.DataBind();

        //    // No. of Nodes
        //    int v = 5;

        //    graph = new LinkedList<Tuple<int, int>>[v];
        //    for (int i = 0; i < graph.Length; ++i)
        //    {
        //        graph[i] = new LinkedList<Tuple<int, int>>();
        //    }

        //    //foreach (distance d in listX)
        //    //{
        //    //    addedge(0, d.node, );
        //    //}

        //    //List<distance> listX = new List<distance>();

        //    //for (int i = 0; i < v; i++)
        //    //{
        //    //    for (int j = 0; j < v; i++)
        //    //    {

        //    //    }
        //    //}



        //    // The nodes shown in above example(by alphabets) are
        //    // implemented using integers addedge(x,y,cost);
        //    //addedge(0, 1, 3);
        //    //addedge(0, 2, 6);
        //    //addedge(0, 3, 5);
        //    //addedge(1, 4, 9);
        //    //addedge(1, 5, 8);
        //    //addedge(2, 6, 12);
        //    //addedge(2, 7, 14);
        //    //addedge(3, 8, 7);
        //    //addedge(8, 9, 5);
        //    //addedge(8, 10, 6);
        //    //addedge(9, 11, 1);
        //    //addedge(9, 12, 10);
        //    //addedge(9, 13, 2);

        //    int source = 0;
        //    int target = 5;

        //    // Function call
        //    best_first_search(source, target, v);

        //    //foreach (int x in resultX)
        //    //{
        //    //    testLit.Text += x + " ";
        //    //}

        //    updGridView.Update();
        //    updatePanelSearchResultRepeater.Update();
        //}

        protected double getDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371; // Radius of the earth in km
            double dLat = deg2rad(lat2 - lat1);  // deg2rad below
            double dLon = deg2rad(lon2 - lon1);

            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c; // Distance in km

            return d;
        }

        protected double deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }

        #endregion
        protected void navSearchBtn_Click(object sender, EventArgs e)
        {
            //testingLocation();
            fillResult(1, pageSize, "productID", "asc");
            fillGrid();
        }

        protected void Page_Changed(object sender, EventArgs e)
        {
            int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
            fillResult(pageIndex, pageSize, hdSortEx.Value, hdSortDir.Value);
        }

        protected void fillGrid()
        {
            try
            {
                /* bfs area */

                String searchText = navSearchTextBox.Text;
                String categoryProduct = categoryProductDD.SelectedValue;
                String sortBy = "", sortDir = "";

                sortBy = "productPrice";
                sortDir = "desc";

                BOProductList listProduct = BLLProduct.getListProduct(searchText, categoryProduct, sortBy, sortDir, 0, 999999);

                int limit = listProduct.Count();

                gvMain.DataSource = listProduct.ToList();
                gvMain.DataBind();
                updGridView.Update();

                // find max v on first looping

                for (int i = 0; i < limit; i++)
                {
                    for (int j = i + 1; j < limit - 1; j++)
                    {
                        findMaxV.Add(listProduct[i].productPrice - listProduct[j].productPrice);
                    }
                }

                errLbl.Text = findMaxV.ToString();
                graph = new LinkedList<Tuple<int, int>>[findMaxV.Max()];

                for (int i = 0; i < limit; ++i)
                {
                    graph[int.Parse(listProduct[i].productID.Remove(0, 1))] = new LinkedList<Tuple<int, int>>();
                }

                for (int i = 0; i < limit; i++)
                {
                    for (int j = i + 1; j <= limit - 1; j++)
                    {
                        addedge(
                            int.Parse(listProduct[i].productID.Remove(0, 1)),
                            int.Parse(listProduct[j].productID.Remove(0, 1)),
                            Math.Abs(listProduct[i].productPrice - listProduct[j].productPrice)
                            );
                    }
                }

                //addedge(2067, 1267, 20500);
                //addedge(2067, 2073, 45700);
                //addedge(1267, 2073, 25200);

                testLit.Text = string.Empty;

                // Function call
                int source = int.Parse(listProduct[0].productID.Remove(0, 1));
                int target = int.Parse(listProduct[limit - 1].productID.Remove(0, 1));

                //int source = 2067;
                //int target = 2073;
                //v = 45700;

                best_first_search(source, target, findMaxV.Max());

                /* 
                 * int.Parse(listProduct[limit-1]
                 * -1 karena mulainya dari 0 */

                foreach (int x in resultX)
                {
                    testLit.Text += x + " ";
                }

                testLit.Text += "<=== hasil bfsnya";
                /* eof bfs area */
            }
            catch (Exception x)
            {
                errLbl.Visible = true;
                errLbl.CssClass = "alert alert-danger";
                errLbl.Text = x.Message;

                updatePanelSearchResultRepeater.Update();
            }

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
                    sortDir = "asc";
                }

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

        #region gvmain

        protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
        }

        protected void gvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMain.PageIndex = e.NewPageIndex;
        }

        #endregion
    }
}