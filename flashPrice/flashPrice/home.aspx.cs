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

        #region best-first-search 
        public LinkedList<Tuple<int, int>>[] graph;
        public List<int> resultX = new List<int>();
        public List<int> farFromX = new List<int>();

 

        // maksimum v dibuat listnya v itu adalah nilai terbesar dari jarak yang paling jauh minimarketnya
        public List<int> findMaxV = new List<int>();

        // Function for adding edges to graph
        public void addedge(int x, int y, int cost)
        {
            graph[x].AddLast(new Tuple<int, int>(cost, y));
            graph[y].AddLast(new Tuple<int, int>(cost, x));
        }

        // Function for finding the minimum weight element.
        public Tuple<int, int> get_min(LinkedList<Tuple<int, int>> pq)
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

        // Function For Implementing Best First Search
        // Gives output path having lowest cost

        public void best_first_search(int actual_Src, int target, int n)
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

                // tambahin result nya biar bisa ditampilin
                distance resultItem = new distance();
                resultItem.node = x;
                resultItem.distanceFromMe = curr_min.Item1;
                resultMinimarket.Add(resultItem);

                // Displaying the path having lowest cost
                if (x == target)
                    break;

                LinkedList<Tuple<int, int>> list = graph[x];
                foreach (var val in list)
                {
                    if (visited[val.Item2] == 0)
                    {
                        visited[val.Item2] = 1;
                        pq.AddLast(new Tuple<int, int>(val.Item1, val.Item2));
                    }
                }
            }
        }
        #endregion

        #region calculate distance from coordinates
        public class distance
        {
            public int node { get; set; }            
            public string MiniMarketID { get; set; }
            public string MiniMarketName { get; set; }
            public int distanceFromMe { get; set; }
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

        public List<category> minCatA = new List<category>();
        public List<category> minCatB = new List<category>();
        public List<category> minCatC = new List<category>();
        public List<category> minCatD = new List<category>();
        public List<category> minCatE = new List<category>();

    

        /*
            kategori A <= 1000 m
            kategori B 1000 m < jarak dari kita <= 5000 m
            kategori C 5000 m < jarak dari kita <= 10000 m
            Kategori D 10000 m < jarak dari kita <= 25000 m
            kategori E 25000 m < jarak dari kita
        */

        private void testingLocation()
        {
            try
            {
                farFromXLit.Text = string.Empty;
                testLit.Text = string.Empty;

                double lat1 = -6.1756452;
                double lon1 = 106.6202711;

                // simpen dulu letak coordinate kitanya  
                //double lat1 = Convert.ToDouble(hiddenMyLatitude.Value.Replace(".", ","));
                //double lon1 = Convert.ToDouble(hiddenMyLongitude.Value.Replace(".", ","));

                // ambil data minimarketnya
                BOMiniMarketList miniMarketList = BLLMiniMarket.getListAllMiniMarket();

                // buat list buat nampung minimarketnya yg mau diitung bfs 
                List<distance> listX = new List<distance>();

                // masukin masing-masing data minimarket ke list string untuk diolah
                foreach (BOMiniMarket xMinimarket in miniMarketList)
                {
                    distance x = new distance();
                    x.node = int.Parse(xMinimarket.miniMarketID.Remove(0, 2));
                    x.MiniMarketName = xMinimarket.miniMarketName;
                    double before = getDistanceFromLatLonInKm(lat1, lon1, double.Parse(xMinimarket.miniMarketLattitude.ToString()), double.Parse(xMinimarket.miniMarketLongitude.ToString())) * 1000;

                    x.distanceFromMe = int.Parse(Math.Round(before).ToString());

                    listX.Add(x);

                    // tentuin masuk mana dia 

                    if (x.distanceFromMe <= 1000)
                    {
                        category xCategory = new category();

                        xCategory.node = int.Parse(xMinimarket.miniMarketID.Remove(0, 2));
                        xCategory.MiniMarketName = xMinimarket.miniMarketName;
                        xCategory.categoryLattitude = double.Parse(xMinimarket.miniMarketLattitude.ToString());
                        xCategory.categoryLongitude = double.Parse(xMinimarket.miniMarketLongitude.ToString());
                        xCategory.distanceFromMe = x.distanceFromMe;

                        catA.Add(xCategory);
                    }
                    else if (x.distanceFromMe > 1000 && x.distanceFromMe <= 5000)
                    {
                        category xCategory = new category();

                        xCategory.node = int.Parse(xMinimarket.miniMarketID.Remove(0, 2));
                        xCategory.MiniMarketName = xMinimarket.miniMarketName;
                        xCategory.categoryLattitude = double.Parse(xMinimarket.miniMarketLattitude.ToString());
                        xCategory.categoryLongitude = double.Parse(xMinimarket.miniMarketLongitude.ToString());
                        xCategory.distanceFromMe = x.distanceFromMe;

                        catB.Add(xCategory);
                    }
                    else if (x.distanceFromMe > 5000 && x.distanceFromMe <= 10000)
                    {
                        category xCategory = new category();

                        xCategory.node = int.Parse(xMinimarket.miniMarketID.Remove(0, 2));
                        xCategory.MiniMarketName = xMinimarket.miniMarketName;
                        xCategory.categoryLattitude = double.Parse(xMinimarket.miniMarketLattitude.ToString());
                        xCategory.categoryLongitude = double.Parse(xMinimarket.miniMarketLongitude.ToString());
                        xCategory.distanceFromMe = x.distanceFromMe;

                        catC.Add(xCategory);
                    }
                    else if (x.distanceFromMe > 10000 && x.distanceFromMe <= 25000)
                    {
                        category xCategory = new category();

                        xCategory.node = int.Parse(xMinimarket.miniMarketID.Remove(0, 2));
                        xCategory.MiniMarketName = xMinimarket.miniMarketName;
                        xCategory.categoryLattitude = double.Parse(xMinimarket.miniMarketLattitude.ToString());
                        xCategory.categoryLongitude = double.Parse(xMinimarket.miniMarketLongitude.ToString());
                        xCategory.distanceFromMe = x.distanceFromMe;

                        catD.Add(xCategory);
                    }
                    else if (x.distanceFromMe > 25000)
                    {
                        category xCategory = new category();

                        xCategory.node = int.Parse(xMinimarket.miniMarketID.Remove(0, 2));
                        xCategory.MiniMarketName = xMinimarket.miniMarketName;
                        xCategory.categoryLattitude = double.Parse(xMinimarket.miniMarketLattitude.ToString());
                        xCategory.categoryLongitude = double.Parse(xMinimarket.miniMarketLongitude.ToString());
                        xCategory.distanceFromMe = x.distanceFromMe;

                        catE.Add(xCategory);
                    }
                }

                /*
                    data yg udah kebentuk itu kita bagi 5 kategori

                    kategori A <= 1000 m
                    kategori B 1000 m < jarak dari kita <= 5000 m
                    kategori C 5000 m < jarak dari kita <= 10000 m
                    Kategori D 10000 m < jarak dari kita <= 25000 m
                    kategori E 25000 m < jarak dari kita

                    4   Indomaret Kenaiban                  0,223599270639309
                    14  Indomaret Indomaret SPBU Otista     0,559619608257842
                    6   Indomaret M Toha                    0,631695782614107
                    2   Indomaret Pasar Baru                0,677222805695068
                    10  Indomaret Merdeka                   0,701830814070184
                    8   Indomaret Pabuaran Karawaci         1,00084398524016
                    16  Indomaret Aria Santika 2            1,03645316913408
                    15  Alfamart Proklamasi                 1,40643255925764
                    19  Indomaret Mt Haryono                1,49341018895008
                    12  Indomaret Pondok Arum               1,50723202577663
                    17  Alfamart Untung Suropati            1,52079015494239
                    18  Indomaret Ahmad Yani                1,60462016720897
                    3   Alfamart Aria Wasangkara            1,66774828762806
                    21  Indomaret Soleh Ali                 1,7140132219309
                    22  Indomaret Cimone Permai             1,98470150887079
                    20  Indomaret Veteran Tangerang         2,01710981778311
                    11  Alfamart Duta Raya                  2,54583670276194
                    13  Alfamart Dipatiunus Raya            2,59224467711972
                    1   Alfamart Villa Grand Tomang         2,60032655725977
                    5   Alfamart Taman Cibodas Raya         2,65063129906387
                    23  Indomaret Gebang Rusunawa           2,88330431499922
                    9   Alfamart Prabu Kian Santang 2       3,24709277815486
                    7   Alfamart Subur                      3,25115374987773
                    24  Indomaret Raya Kampung Pisang       3,33894571680443

                */

                //gvMain.DataSource = listX.OrderBy(distance => distance.distanceFromMe).ToList();
                //gvMain.DataBind();
                //updGridView.Update();


                int limit = miniMarketList.Count();

                // find max v on first looping
                for (int i = 0; i < limit; i++)
                {
                    for (int j = i + 1; j < limit - 1; j++)
                    {
                        findMaxV.Add((int)getDistanceFromLatLonInKm(double.Parse(miniMarketList[i].miniMarketLattitude.ToString()), double.Parse(miniMarketList[i].miniMarketLongitude.ToString()), double.Parse(miniMarketList[j].miniMarketLattitude.ToString()), double.Parse(miniMarketList[j].miniMarketLongitude.ToString())) * 1000);
                    }
                }

                errLbl.Text = findMaxV.ToString();
                graph = new LinkedList<Tuple<int, int>>[findMaxV.Max()];

                // buat lokasi awalnya pake 0 

                graph[0] = new LinkedList<Tuple<int, int>>();

                for (int i = 0; i < limit; ++i)
                {
                    graph[int.Parse(miniMarketList[i].miniMarketID.Remove(0, 2))] = new LinkedList<Tuple<int, int>>();
                }

                /*                
                    step 1
                    urutin Categorynya secara ascending

                    step 2
                    gw urutin dulu yg yg paling kecil dijadiin kepala si kategori                

                    step 3
                    nyambung lokasi kita ke kepala kategori masing2, yg pasti koordinat kita sama kepala-kepalanya
                   
                    step 4
                    nyambungin kepala kategori ke anakannya
                 */

                // step 1

                // urutin Categorynya secara ascending
                if (catA.Count != 0) { catA = catA.AsQueryable().OrderBy(c => c.distanceFromMe).ToList(); }
                if (catB.Count != 0) { catB = catB.AsQueryable().OrderBy(c => c.distanceFromMe).ToList(); }
                if (catC.Count != 0) { catC = catC.AsQueryable().OrderBy(c => c.distanceFromMe).ToList(); }
                if (catD.Count != 0) { catD = catD.AsQueryable().OrderBy(c => c.distanceFromMe).ToList(); }
                if (catE.Count != 0) { catE = catE.AsQueryable().OrderBy(c => c.distanceFromMe).ToList(); }

                // step 2
                // tentuin minimumnya node nya
                int minimumA = catA.Count == 0 ? 0 : catA[0].node;
                int minimumB = catB.Count == 0 ? 0 : catB[0].node;
                int minimumC = catC.Count == 0 ? 0 : catC[0].node;
                int minimumD = catD.Count == 0 ? 0 : catD[0].node;
                int minimumE = catE.Count == 0 ? 0 : catE[0].node;

                // step 3
                // buat edge antara lokasi kita dengan lokasi kepala masing-masing category
                if (minimumA != 0)
                {
                    // checking
                    double before = getDistanceFromLatLonInKm(lat1, lon1, catA[0].categoryLattitude, catA[0].categoryLongitude) * 1000;
                    int value = int.Parse(Math.Round(before).ToString());

                    addedge(
                            0,
                            catA[0].node,
                            value
                        );
                }

                if (minimumB != 0)
                {
                    double before = getDistanceFromLatLonInKm(lat1, lon1, catB[0].categoryLattitude, catB[0].categoryLongitude) * 1000;
                    int value = int.Parse(Math.Round(before).ToString());

                    addedge
                        (
                            0,
                            catB[0].node,
                            value
                        );
                }

                if (minimumC != 0)
                {
                    double before = getDistanceFromLatLonInKm(lat1, lon1, catC[0].categoryLattitude, catC[0].categoryLongitude) * 1000;
                    int value = int.Parse(Math.Round(before).ToString());

                    addedge(
                            0,
                            catC[0].node,
                            value
                        );
                }

                if (minimumD != 0)
                {
                    double before = getDistanceFromLatLonInKm(lat1, lon1, catD[0].categoryLattitude, catD[0].categoryLongitude) * 1000;
                    int value = int.Parse(Math.Round(before).ToString());

                    addedge(
                            0,
                            catD[0].node,
                            value
                        );

                }

                if (minimumE != 0)
                {
                    double before = getDistanceFromLatLonInKm(lat1, lon1, catE[0].categoryLattitude, catE[0].categoryLongitude) * 1000;
                    int value = int.Parse(Math.Round(before).ToString());

                    addedge(
                            0,
                            catE[0].node,
                            value
                        );
                }

                // step 4
                // nyambungin kepala kategori ke anakannya

                if (minimumA != 0)
                {
                    for (int i = 0; i < catA.Count; i++)
                    {
                        for (int j = i + 1; j < catA.Count; j++)
                        {
                            double before = getDistanceFromLatLonInKm(catA[0].categoryLattitude, catA[0].categoryLongitude, catA[j].categoryLattitude, catA[j].categoryLongitude) * 1000;
                            int value = int.Parse(Math.Round(before).ToString());

                            addedge(
                                    minimumA,
                                    catA[j].node,
                                    value
                                );
                        }
                    }
                }

                if (minimumB != 0)
                {
                    for (int i = 0; i < catB.Count; i++)
                    {
                        for (int j = i + 1; j < catB.Count; j++)
                        {
                            double before = getDistanceFromLatLonInKm(catB[0].categoryLattitude, catB[0].categoryLongitude, catB[j].categoryLattitude, catB[j].categoryLongitude) * 1000;
                            int value = int.Parse(Math.Round(before).ToString());

                            addedge(
                                    minimumB,
                                    catB[j].node,
                                    value
                                );
                        }
                    }
                }

                if (minimumC != 0)
                {
                    for (int i = 0; i < catC.Count; i++)
                    {
                        for (int j = i + 1; j < catC.Count; j++)
                        {
                            double before = getDistanceFromLatLonInKm(catC[0].categoryLattitude, catC[0].categoryLongitude, catC[j].categoryLattitude, catC[j].categoryLongitude) * 1000;
                            int value = int.Parse(Math.Round(before).ToString());

                            addedge(
                                    minimumC,
                                    catC[j].node,
                                    value
                                );
                        }
                    }
                }

                if (minimumD != 0)
                {
                    for (int i = 0; i < catD.Count; i++)
                    {
                        for (int j = i + 1; j < catD.Count; j++)
                        {
                            double before = getDistanceFromLatLonInKm(catD[0].categoryLattitude, catD[0].categoryLongitude, catD[j].categoryLattitude, catD[j].categoryLongitude) * 1000;
                            int value = int.Parse(Math.Round(before).ToString());

                            addedge(
                                    minimumD,
                                    catD[j].node,
                                    value
                                );
                        }
                    }
                }

                if (minimumE != 0)
                {
                    for (int i = 0; i < catE.Count; i++)
                    {
                        for (int j = i + 1; j < catE.Count; j++)
                        {
                            double before = getDistanceFromLatLonInKm(catE[0].categoryLattitude, catE[0].categoryLongitude, catE[j].categoryLattitude, catE[j].categoryLongitude) * 1000;
                            int value = int.Parse(Math.Round(before).ToString());

                            addedge(
                                    minimumE,
                                    catE[j].node,
                                    value
                                );
                        }
                    }
                }

                testLit.Text = string.Empty; // reset hasilnya

                // Function call
                int source = 0;
                BOMiniMarket miniMarketTarget = BLLMiniMarket.getIDMiniMarketByMiniMarketName(miniMarketSearchTextBox.Text.Trim());
                int target = int.Parse(miniMarketTarget.miniMarketID.Remove(0, 2));

                best_first_search(source, target, findMaxV.Max());

                foreach (distance x in resultMinimarket)
                {
                    if (x.node == 0) continue;

                    x.MiniMarketID = "MM" + x.node.ToString("000").PadLeft(3);
                    BOMiniMarket xBO = BLLMiniMarket.getContentByID(x.MiniMarketID);
                    xBO.distanceFromMe = x.distanceFromMe;
                    resultMinimarketBFS.Add(xBO);
                }

                gvMain.DataSource = resultMinimarketBFS;
                gvMain.DataBind();

                updGridView.Update();
                /* eof bfs area */
            }
            catch (Exception x)
            {
                errLbl.Visible = true;
                errLbl.CssClass = "alert alert-danger";
                errLbl.Text = x.Message;

                updatePanelSearchResultRepeater.Update();
            }

            updatePanelSearchResultRepeater.Update();
        }

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
            testingLocation();
            fillResult(1, pageSize, "productPrice", "asc");
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
                // urusan logo minimarket
                Image imgMiniMarketType = ((Image)e.Row.FindControl("imgMiniMarketType"));
                String miniMarketType = (String)DataBinder.Eval(e.Row.DataItem, "miniMarketType");
                imgMiniMarketType.ImageUrl = miniMarketType == "Indomaret" ? @"~\assets\images\indomaret_logo.png" : @"~\assets\images\alfamart_logo.png";
            }
        }

        protected void gvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMain.PageIndex = e.NewPageIndex;
            testingLocation();
        }


        protected void FillGrid(BOMiniMarketList xBO)
        {
        }
        #endregion
    }
}