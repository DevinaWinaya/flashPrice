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
                //testingProductBFS();
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
                priceDifference resultItem = new priceDifference();
                resultItem.node = x;
                resultItem.priceGap = curr_min.Item1;
                resultProduct.Add(resultItem);

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
        public class priceDifference
        {
            public int node { get; set; }
            public string productID { get; set; }
            public string productName { get; set; }
            public int productPrice { get; set; }
            public int priceGap { get; set; } // selisih harga antar barang

            public int nodeSource { get; set; } // tempat A
            public string productIDSource { get; set; }
            public string productIDTarget { get; set; }
            public int nodeTarget { get; set; } // tempat B

            public string categoryPrice { get; set; }
        }

        // buat nampung hasil
        public List<priceDifference> whiceOneIsIt = new List<priceDifference>();
        public List<priceDifference> resultProduct = new List<priceDifference>();
        public BOProductList resultProductBFS = new BOProductList();

        public class priceCategory
        {
            public int node { get; set; }
            public string productName { get; set; }
            public string producCategoryPrice { get; set; }
            public int productPrice { get; set; }
        }

        // build category nya
        public List<priceCategory> catA = new List<priceCategory>();
        public List<priceCategory> catB = new List<priceCategory>();
        public List<priceCategory> catC = new List<priceCategory>();
        public List<priceCategory> catD = new List<priceCategory>();
        public List<priceCategory> catE = new List<priceCategory>();
        public List<priceCategory> catALL = new List<priceCategory>();

        public List<priceCategory> minCatA = new List<priceCategory>();
        public List<priceCategory> minCatB = new List<priceCategory>();
        public List<priceCategory> minCatC = new List<priceCategory>();
        public List<priceCategory> minCatD = new List<priceCategory>();
        public List<priceCategory> minCatE = new List<priceCategory>();
        public List<priceCategory> minCatAll = new List<priceCategory>();


        /*
            kategori A <= 10000 
            kategori B 10000 m < jarak dari kita <= 25000 m
            kategori C 25000 m < jarak dari kita <= 50000 m
            Kategori D 75000 m < jarak dari kita <= 100000 m
            kategori E 100000 m < jarak dari kita
        */

        #endregion

        protected void navSearchBtn_Click(object sender, EventArgs e)
        {
            //fillResult(1, pageSize, "productPrice", "asc");
            testingProductBFS();
        }

        private void testingProductBFS()
        {
            try
            {
                String searchText = navSearchTextBox.Text;
                String categoryProduct = categoryProductDD.SelectedValue;


                string sortBy = "";
                string sortDir = "";

                BOProductList productList = BLLProduct.getListProductWithoutRowNumber(searchText, categoryProduct, false, sortBy, sortDir);
                BOProduct mostExpensiveProduct = BLLProduct.getMostExpensiveProduct(searchText, categoryProduct, false);

                // buat list buat nampung product yg mau diitung bfs  
                List<priceDifference> listX = new List<priceDifference>();
                listX.Clear();

                foreach (BOProduct xProduct in productList)
                {
                    priceDifference x = new priceDifference();
                    x.node = int.Parse(xProduct.productID.Remove(0, 1));
                    x.productName = xProduct.productName;
                    int priceGap = xProduct.productPrice;

                    x.priceGap = priceGap;

                    listX.Add(x);

                    // tentuin masuk mana dia berdasarkan raneg harga

                    if (x.priceGap <= 10000) // dibawah 10k
                    {
                        priceCategory xCategory = new priceCategory();

                        xCategory.node = int.Parse(xProduct.productID.Remove(0, 1));
                        xCategory.productName = xProduct.productName;
                        xCategory.productPrice = xProduct.productPrice;
                        xCategory.producCategoryPrice = "Sangat Murah";

                        catA.Add(xCategory);
                        catALL.Add(xCategory);
                    }
                    else if (x.priceGap > 10000 && x.priceGap <= 25000)
                    {
                        priceCategory xCategory = new priceCategory();

                        xCategory.node = int.Parse(xProduct.productID.Remove(0, 1));
                        xCategory.productName = xProduct.productName;
                        xCategory.productPrice = xProduct.productPrice;
                        xCategory.producCategoryPrice = "Murah";

                        catB.Add(xCategory);
                        catALL.Add(xCategory);
                    }
                    else if (x.priceGap > 25000 && x.priceGap <= 50000)
                    {
                        priceCategory xCategory = new priceCategory();

                        xCategory.node = int.Parse(xProduct.productID.Remove(0, 1));
                        xCategory.productName = xProduct.productName;
                        xCategory.productPrice = xProduct.productPrice;
                        xCategory.producCategoryPrice = "Normal";

                        catC.Add(xCategory);
                        catALL.Add(xCategory);
                    }
                    else if (x.priceGap > 50000 && x.priceGap <= 100000)
                    {
                        priceCategory xCategory = new priceCategory();

                        xCategory.node = int.Parse(xProduct.productID.Remove(0, 1));
                        xCategory.productName = xProduct.productName;
                        xCategory.productPrice = xProduct.productPrice;
                        xCategory.producCategoryPrice = "Mahal";

                        catD.Add(xCategory);
                        catALL.Add(xCategory);
                    }
                    else if (x.priceGap > 100000)
                    {
                        priceCategory xCategory = new priceCategory();

                        xCategory.node = int.Parse(xProduct.productID.Remove(0, 1));
                        xCategory.productName = xProduct.productName;
                        xCategory.productPrice = xProduct.productPrice;
                        xCategory.producCategoryPrice = "Sangat Mahal";

                        catE.Add(xCategory);
                        catALL.Add(xCategory);
                    }
                }

                int limit = productList.Count();

                // find max v on first looping
                for (int i = 0; i < limit; i++)
                {
                    for (int j = i + 1; j < limit - 1; j++)
                    {
                        findMaxV.Add(Math.Abs(productList[i].productPrice - productList[j].productPrice));
                    }
                }

                errLbl.Text = findMaxV.ToString();
                graph = new LinkedList<Tuple<int, int>>[findMaxV.Max()];

                // buat lokasi awalnya pake 0 

                graph[0] = new LinkedList<Tuple<int, int>>();

                for (int i = 0; i < limit; ++i)
                {
                    graph[int.Parse(productList[i].productID.Remove(0, 1))] = new LinkedList<Tuple<int, int>>();
                }

                /*                
                    step 2
                    gw urutin dulu yg yg paling kecil dijadiin kepala si kategori                

                    step 3
                    nyambung lokasi kita ke kepala kategori masing2, yg pasti koordinat kita sama kepala-kepalanya
                   
                    step 4
                    nyambungin kepala kategori ke anakannya
                 */


                // step 2
                // tentuin minimumnya node nya
                int minimumA = catA.Count == 0 ? 0 : catA[0].node;
                int minimumB = catB.Count == 0 ? 0 : catB[0].node;
                int minimumC = catC.Count == 0 ? 0 : catC[0].node;
                int minimumD = catD.Count == 0 ? 0 : catD[0].node;
                int minimumE = catE.Count == 0 ? 0 : catE[0].node;
                int minCatAll = catALL.Count == 0 ? 0 : catALL[0].node;


                string edges = "";

                // step 3
                // buat edge antara lokasi kita dengan lokasi kepala masing-masing category
                if (minimumA != 0)
                {
                    // checking
                    int priceGap = 0 - catA[0].productPrice;

                    edges += @"addedge(0, " + catA[0].node + ", " + priceGap + "); \n";

                    priceDifference x = new priceDifference();
                    x.nodeSource = 0;
                    x.nodeTarget = catA[0].node;
                    x.priceGap = priceGap;
                    x.categoryPrice = "Hemat";
                    whiceOneIsIt.Add(x);

                    addedge(
                            0,
                            catA[0].node,
                            priceGap
                        );
                }

                if (minimumB != 0)
                {
                    // checking
                    int priceGap = 0 - catB[0].productPrice;

                    edges += @"addedge(0, " + catB[0].node + ", " + priceGap + "); \n";

                    priceDifference x = new priceDifference();
                    x.nodeSource = 0;
                    x.nodeTarget = catB[0].node;
                    x.priceGap = priceGap;
                    x.categoryPrice = "Murah";
                    whiceOneIsIt.Add(x);

                    addedge(
                            0,
                            catB[0].node,
                            priceGap
                        );
                }

                if (minimumC != 0)
                {
                    // checking
                    int priceGap = 0 - catC[0].productPrice;

                    edges += @"addedge(0, " + catC[0].node + ", " + priceGap + "); \n";

                    priceDifference x = new priceDifference();
                    x.nodeSource = 0;
                    x.nodeTarget = catC[0].node;
                    x.priceGap = priceGap;
                    x.categoryPrice = "Normal";
                    whiceOneIsIt.Add(x);

                    addedge(
                            0,
                            catC[0].node,
                            priceGap
                        );
                }

                if (minimumD != 0)
                {
                    // checking
                    int priceGap = 0 - catD[0].productPrice;

                    edges += @"addedge(0, " + catD[0].node + ", " + priceGap + "); \n";

                    priceDifference x = new priceDifference();
                    x.nodeSource = 0;
                    x.nodeTarget = catD[0].node;
                    x.priceGap = priceGap;
                    x.categoryPrice = "Mahal";
                    whiceOneIsIt.Add(x);

                    addedge(
                            0,
                            catD[0].node,
                            priceGap
                        );
                }

                if (minimumE != 0)
                {
                    // checking
                    int priceGap = 0 - catE[0].productPrice;

                    edges += @"addedge(0, " + catE[0].node + ", " + priceGap + "); \n";

                    priceDifference x = new priceDifference();
                    x.nodeSource = 0;
                    x.nodeTarget = catE[0].node;
                    x.priceGap = priceGap;
                    x.categoryPrice = "Exclusive";
                    whiceOneIsIt.Add(x);

                    addedge(
                            0,
                            catE[0].node,
                            priceGap
                        );
                }

                string newEdges = "";

                // step 4
                // nyambungin kepala kategori ke anakannya

                if (minimumA != 0)
                {
                    for (int i = 0; i < catA.Count; i++)
                    {
                        for (int j = i + 1; j < catA.Count; j++)
                        {
                            int priceGap = catA[0].productPrice - catA[j].productPrice;

                            newEdges = @"addedge(" + minimumA + "," + catA[j].node + ", " + priceGap + "); \n";

                            if (!edges.Contains(newEdges))
                            {
                                priceDifference x = new priceDifference();
                                x.nodeSource = catA[0].node;
                                x.priceGap = priceGap;
                                x.nodeTarget = catA[j].node;
                                x.categoryPrice = "Hemat";
                                whiceOneIsIt.Add(x);

                                edges += newEdges;
                                addedge(
                                    minimumA,
                                    catA[j].node,
                                    priceGap
                                );
                            }
                        }
                    }
                }

                if (minimumB != 0)
                {
                    for (int i = 0; i < catB.Count; i++)
                    {
                        for (int j = i + 1; j < catB.Count; j++)
                        {
                            int priceGap = catB[0].productPrice - catB[j].productPrice;

                            newEdges = @"addedge(" + minimumB + "," + catB[j].node + ", " + priceGap + "); \n";

                            if (!edges.Contains(newEdges))
                            {
                                priceDifference x = new priceDifference();
                                x.nodeSource = catB[0].node;
                                x.nodeTarget = catB[j].node;
                                x.priceGap = priceGap;
                                x.categoryPrice = "Murah";
                                whiceOneIsIt.Add(x);

                                edges += newEdges;
                                addedge(
                                    minimumB,
                                    catB[j].node,
                                    priceGap
                                );
                            }
                        }
                    }
                }

                if (minimumC != 0)
                {
                    for (int i = 0; i < catC.Count; i++)
                    {
                        for (int j = i + 1; j < catC.Count; j++)
                        {
                            int priceGap = catC[0].productPrice - catC[j].productPrice;

                            newEdges = @"addedge(" + minimumC + "," + catC[j].node + ", " + priceGap + "); \n";

                            if (!edges.Contains(newEdges))
                            {
                                priceDifference x = new priceDifference();
                                x.nodeSource = catC[0].node;
                                x.nodeTarget = catC[j].node;
                                x.priceGap = priceGap;
                                x.categoryPrice = "Normal";
                                whiceOneIsIt.Add(x);

                                edges += newEdges;

                                addedge(
                                    minimumC,
                                    catC[j].node,
                                    priceGap
                                );
                            }
                        }
                    }
                }

                if (minimumD != 0)
                {
                    for (int i = 0; i < catD.Count; i++)
                    {
                        for (int j = i + 1; j < catD.Count; j++)
                        {
                            int priceGap = catD[0].productPrice - catD[j].productPrice;

                            newEdges = @"addedge(" + minimumD + "," + catD[j].node + ", " + priceGap + "); \n";

                            if (!edges.Contains(newEdges))
                            {
                                edges += newEdges;

                                priceDifference x = new priceDifference();
                                x.nodeSource = catD[0].node;
                                x.nodeTarget = catD[j].node;
                                x.priceGap = priceGap;
                                x.categoryPrice = "Mahal";
                                whiceOneIsIt.Add(x);

                                addedge(
                                    minimumD,
                                    catD[j].node,
                                    priceGap
                                );
                            }
                        }
                    }
                }

                if (minimumE != 0)
                {
                    for (int i = 0; i < catE.Count; i++)
                    {
                        for (int j = i + 1; j < catE.Count; j++)
                        {
                            int priceGap = catE[0].productPrice - catE[j].productPrice;

                            newEdges = @"addedge(" + minimumE + "," + catE[j].node + ", " + priceGap + "); \n";

                            if (!edges.Contains(newEdges))
                            {
                                priceDifference x = new priceDifference();
                                x.nodeSource = catE[0].node;
                                x.nodeTarget = catE[j].node;
                                x.priceGap = priceGap;
                                x.categoryPrice = "Exclusive";
                                whiceOneIsIt.Add(x);

                                edges += newEdges;
                                addedge(
                                    minimumE,
                                    catE[j].node,
                                    priceGap
                                );
                            }
                        }
                    }
                }

                // tempat test output
                //testLit.Text = edges;
                //updError.Update();

                // Function call
                int source = 0;

                if (catALL.Count != 0) { catALL = catALL.AsQueryable().OrderByDescending(c => c.productPrice).ToList(); } // ambil harga paling mahal

                // tentuin target
                //int target = 25000;
                int target = int.Parse(mostExpensiveProduct.productID.Remove(0, 1));

                best_first_search(source, target, findMaxV.Max());

                foreach (priceDifference x in resultProduct)
                {
                    if (x.node == 0) continue;

                    x.productID = "P" + x.node.ToString("00000").PadLeft(5);

                    BOProduct xBO = BLLProduct.getContent(x.productID);
                    foreach (priceDifference y in whiceOneIsIt)
                    {
                        if (y.priceGap == x.priceGap && y.nodeTarget == x.node) // harusnya price gap atau product id ya ini
                        {
                            x.productIDSource = "P" + y.nodeSource.ToString("00000").PadLeft(5);
                            if (y.nodeSource == 0)
                            {
                                xBO.sourceProduct = "You";
                                xBO.categoryPrice = y.categoryPrice;
                                xBO.productName = xBO.productID + "-" + xBO.productName;
                                xBO.compareTo = "You";
                            }
                            else
                            {
                                BOProduct xSource = BLLProduct.getContent(x.productIDSource);
                                xBO.sourceProduct = xSource.productName;
                                xBO.categoryPrice = y.categoryPrice;
                                xBO.productName = xBO.productID + "-" + xBO.productName;
                                xBO.compareTo = x.productIDSource;
                            }
                            continue;
                        }
                    }

                    xBO.priceGap = x.priceGap;
                    resultProductBFS.Add(xBO);
                }

                resultRepeater.DataSource = resultProductBFS;//.OrderBy(BOProduct => BOProduct.productPrice);
                resultRepeater.DataBind();

                litError.Text = "";

                hdSortEx.Value = sortBy;
                hdSortDir.Value = sortDir;
                updatePanelSearchResultRepeater.Update();

                paginationDiv.Visible = false;

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



        #region result repeater normal product
        protected void resultRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //ScriptManager scriptMng = ScriptManager.GetCurrent(this);
                //Label docIDLbl = e.Item.FindControl("docIDLbl") as Label;
                //Label pinjamanIDLbl = e.Item.FindControl("pinjamanIDLbl") as Label;
                //BODokumenPersyaratan xDoc = BLLDokumenPersyaratan.getContent(pinjamanIDLbl.Text, docIDLbl.Text);
                //Label fileNameLbl = e.Item.FindControl("fileNameLbl") as Label;
                //Label lastFeedBackLbl = e.Item.FindControl("lastFeedBackLbl") as Label;
                //CheckBox checkCB = e.Item.FindControl("checkCB") as CheckBox;
                //scriptMng.RegisterAsyncPostBackControl(checkCB);
                //LinkButton btnDownload = e.Item.FindControl("btnDownloadCheck") as LinkButton;
                //LinkButton btnPreview = e.Item.FindControl("btnPreviewCheck") as LinkButton;

                //BODokumenNotesList xApprovedDocList = BLLDokumenNotes.getList(hidTxnID.Value, "Approved", "dn.docID", "asc");
                //BODokumenNotesList xDocList = BLLDokumenNotes.getListDocStatus(hidTxnID.Value, "dn.docID", "asc");

            }
        }

        protected void resultRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void Page_Changed(object sender, EventArgs e)
        {
            //int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
            //fillResult(pageIndex, pageSize, hdSortEx.Value, hdSortDir.Value);
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

                BOProductList listProduct = BLLProduct.getListProduct(searchText, categoryProduct, "", false, sortBy, sortDir, startRow, maxRow);
                int jmlBaris = int.Parse(BLLProduct.getCountListProduct(searchText, categoryProduct, "", false, sortBy, sortDir, startRow, maxRow).ToString());

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

                PopulatePager(jmlBaris, pageIndex);
                paginationDiv.Visible = false;
                updGridView.Update();
                updatePanelSearchResultRepeater.Update();
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
            string compareID = hiddenProductCompareID.Value;

            loadProduct(productID, compareID);
        }

        private void loadProduct(string productID, string compareID)
        {
            BOProduct xBO = BLLProduct.getContent(productID);

            productNamePopupLbl.Text = xBO.productName;
            productPricePopupLbl.Text = xBO.productPrice.ToString("#0");
            //productDescPopupLbl.Text = xBO.productDescription == "" || xBO.productDescription == null ? "Tidak ada deskripsi produk" : xBO.productDescription;
            productImageUrlPopup.ImageUrl = xBO.productImageUrl;
            miniMarketImageUrlPopup.ImageUrl = xBO.miniMarketType == "Indomaret" ? @"~\assets\images\indomaret_logo.png" : @"~\assets\images\alfamart_logo.png";

            if (compareID != "You")
            {
                BOProduct xCompare = BLLProduct.getContent(compareID);
                productCompareNameLbl.Text = xCompare.productName;
                productComparePricePopupLbl.Text = xCompare.productPrice.ToString("#0");
                //productDescPopupLbl.Text = xBO.productDescription == "" || xBO.productDescription == null ? "Tidak ada deskripsi produk" : xBO.productDescription;
                imageProductCompare.ImageUrl = xCompare.productImageUrl;
                miniMarketCompareImageUrlPopup.ImageUrl = xCompare.miniMarketType == "Indomaret" ? @"~\assets\images\indomaret_logo.png" : @"~\assets\images\alfamart_logo.png";

                if (xCompare.productPrice < xBO.productPrice)
                {
                    compareLbl.Text = "Rekomendasi Produk Terkait yang Lebih Murah";
                    compareLbl.CssClass = "col-md-12 col-xs-12 col-sm-12 alert alert-success";
                }
                else
                {
                    compareLbl.Text = "Rekomendasi Produk Terkait yang Lebih Mahal";
                    compareLbl.CssClass = "col-md-12 col-xs-12 col-sm-12 alert alert-danger";
                }

                compareDIV.Visible = true;
            }

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