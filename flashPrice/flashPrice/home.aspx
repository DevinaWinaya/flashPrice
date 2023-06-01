<%@ Page Title="" Language="C#" MasterPageFile="~/default.Master" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="flashPrice.pages.home" UICulture="id-ID" Culture="id-ID" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Flash Price</title>
    <style>
        /*AutoComplete flyout */
        .autoCompleteContainer ul li {
            margin: 0 !important;
            padding: 0 !important;
            border: none !important;
        }

        .autocomplete_completionListElement {
            margin: 0px !important;
            padding: 0px !important;
            background-color: white;
            color: windowtext;
            border: buttonshadow;
            border-width: 1px;
            border-style: solid;
            cursor: 'default';
            overflow: auto;
            z-index: 9999 !important;
            width: 408.6px !important;
            text-align: left;
            list-style-type: none;
        }

        /* AutoComplete highlighted item */

        .autocomplete_highlightedListItem {
            background-color: #007bff;
            color: white;
            padding: 0 !important;
        }

        /* AutoComplete item */

        .autocomplete_listItem {
            background-color: window;
            color: windowtext;
            padding: 1px !important;
        }
    </style>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPh" runat="server">


    <asp:UpdatePanel ID="updAction" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <asp:HiddenField ID="hiddenMyLatitude" runat="server" />
            <asp:HiddenField ID="hiddenMyLongitude" runat="server" />

            <asp:HiddenField ID="hiddenProductID" runat="server" />
            <asp:HiddenField ID="hdnPageIdx" runat="server" />
            <asp:Button ID="productDetailBtn" runat="server" Text="Product Detail" OnClick="productDetailBtn_Click" Style="display: none;" />
        </ContentTemplate>
    </asp:UpdatePanel>



    <asp:HiddenField ID="hdSortEx" runat="server" />
    <asp:HiddenField ID="hdSortDir" runat="server" />


    <nav class="navbar navbar-expand bg-body-tertiary sticky-top text-center" style="background-color: #fbd746;">

        <div class="col-md-3 pl-0 pr-1">
            <asp:TextBox runat="server" ID="navSearchTextBox" CssClass="form-control autocomplete" placeHolder="Temukan produkmu disini . . ."></asp:TextBox>
            <act:AutoCompleteExtender runat="server" ID="dataProduct" TargetControlID="navSearchTextBox"
                ServiceMethod="getListProductCached" ServicePath="~/webService/wsvProduct.asmx"
                MinimumPrefixLength="2" CompletionInterval="100" EnableCaching="true" CompletionSetCount="10"
                CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" DelimiterCharacters=";,:"
                ShowOnlyCurrentWordInCompletionListItem="true">
            </act:AutoCompleteExtender>
        </div>

        <div class="col-md-3 px-1">
            <asp:DropDownList runat="server" ID="categoryProductDD" CssClass="form-control">
                <asp:ListItem Text="Pilih Kategori" Value=""></asp:ListItem>
                <asp:ListItem Text="Makanan" Value="C001"></asp:ListItem>
                <asp:ListItem Text="Minuman" Value="C002"></asp:ListItem>
            </asp:DropDownList>

        </div>
        <div class="col-md-3 px-1">
            <asp:TextBox runat="server" ID="miniMarketSearchTextBox" CssClass="form-control autocomplete" placeHolder="Tentukan Destinasi Minimarketmu Disini"></asp:TextBox>

            <act:AutoCompleteExtender runat="server" ID="dataMiniMarket" TargetControlID="miniMarketSearchTextBox"
                ServiceMethod="getListMiniMarketCached" ServicePath="~/webService/wsvMiniMarket.asmx"
                MinimumPrefixLength="2" CompletionInterval="100" EnableCaching="true" CompletionSetCount="10"
                CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" DelimiterCharacters=";,:"
                ShowOnlyCurrentWordInCompletionListItem="true">
            </act:AutoCompleteExtender>
        </div>

        <div class="col-md-1 px-1 text-left">
            <asp:LinkButton runat="server" ID="navSearchBtn" OnClick="navSearchBtn_Click" CssClass="btn btn-light"><i class="fa fa-search mr-2"> </i>Search</asp:LinkButton>
        </div>
    </nav>


    <asp:Literal runat="server" ID="testLit"></asp:Literal>
    <asp:Literal runat="server" ID="farFromXLit"></asp:Literal>



    <asp:UpdatePanel ID="updError" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="col-md-12 col-xs-12 col-sm-12 mt-4 alert alert-danger" id="errDiv" runat="server" visible="false">
                <asp:Label runat="server" ID="errLbl" CssClass="col-md-12"></asp:Label>
            </div>

        </ContentTemplate>

    </asp:UpdatePanel>


    <div class="col-md-12 col-xs-12 col-sm-12 mt-3">
        <asp:UpdatePanel ID="updGridView" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Literal ID="litErrorLogin" runat="server"></asp:Literal>
                <div class="datagrid" id="dvGrid" runat="server" style="display: none;">
                    <!-- main grid -->
                    <div style="height: 350px; overflow-y: auto; width: auto;" class="mt-4">
                        <asp:GridView ID="gvMain" runat="server" EnableModelValidation="True" AutoGenerateColumns="false"
                            AllowPaging="False" PageSize="100" OnPageIndexChanging="gvMain_PageIndexChanging"
                            OnRowDataBound="gvMain_RowDataBound" CssClass="table table-hover table-bordered">
                            <Columns>
                                <asp:TemplateField HeaderText="#" HeaderStyle-BackColor="#fbd746" HeaderStyle-ForeColor="Black">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# (Container.DataItemIndex)+1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="MiniMarket" ItemStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#fbd746" HeaderStyle-ForeColor="Black">
                                    <ItemTemplate>
                                        <asp:Image ID="imgMiniMarketType" Style="width: 100px; height: auto;" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="miniMarketName" HeaderText="Nama" HeaderStyle-BackColor="#fbd746" HeaderStyle-ForeColor="Black" />
                                <asp:BoundField DataField="miniMarketAddress" HeaderText="Alamat" HeaderStyle-BackColor="#fbd746" HeaderStyle-ForeColor="Black" />
                                <asp:BoundField DataField="distanceFromMe" HeaderText="Jarak (meter)" HeaderStyle-BackColor="#fbd746" HeaderStyle-ForeColor="Black" />
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="4" FirstPageText="First"
                                LastPageText="Last" />
                            <RowStyle CssClass="td" />
                            <SelectedRowStyle CssClass="thspecalt" />
                            <AlternatingRowStyle CssClass="tdalt" />
                        </asp:GridView>
                    </div>
                </div>
                <hr />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <asp:UpdatePanel ID="updatePanelSearchResultRepeater" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="navSearchBtn" />
        </Triggers>
        <ContentTemplate>
            <div id="resultDiv" class="pt-4" runat="server" style="height: auto;">
                <div id="queryResultDiv" class="row mx-auto col-md-8 col-xs-12 col-sm-12 border border-secondary-50" runat="server">


                    <asp:Repeater ID="resultRepeater" runat="server" OnItemDataBound="resultRepeater_ItemDataBound" OnItemCommand="resultRepeater_ItemCommand">
                        <ItemTemplate>
                            <div class="col-md-3 mb-3 d-flex align-items-stretch">
                                <div class="card mt-3 mb-3 col-md-12">
                                    <div class="card-header bg-white">
                                        <asp:Literal ID="litProductImg" runat="server"></asp:Literal>
                                    </div>
                                    <div class="card-body">
                                        <div class="row">
                                            <span class="text-left col-md-12">
                                                <img src="<%#Eval("productImageUrl")%>" onerror="imgError(this)" style="border-radius: 10px; height: auto; width: 280px;" class="img-fluid" />
                                            </span>

                                            <span class="pt-2 pb-2">
                                                <asp:Label ID="productNameLbl" CssClass="h6" Style="color: #ee8000;" runat="server" Text='<%#Eval("productName") %>'></asp:Label>
                                            </span>
                                        </div>
                                        <div class="row pt-2 pb-2">
                                            <span>
                                                <i class="fa fa-money-bill-1 mr-2"></i>
                                                Rp.<asp:Label ID="productPriceLbl" CssClass="ml-1" runat="server" Text='<%#Eval("productPrice")%>'></asp:Label>
                                            </span>
                                        </div>
                                        <div class="row pt-2 pb-2">
                                            <span>
                                                <i class="fa fa-store mr-2"></i>
                                                <asp:Label ID="miniMarketName" runat="server" Text='<%#Eval("miniMarketName") %>'></asp:Label>
                                            </span>
                                        </div>
                                        <div class="row">
                                            <span>
                                                <i class="fa fa-map-location mr-2"></i>
                                                <asp:Label ID="miniMarketAddress" Text='<%#Eval("miniMarketAddress")%>' runat="server"></asp:Label>
                                            </span>
                                        </div>
                                    </div>
                                    <div class="row pt-2 card-footer bg-white">
                                        <asp:LinkButton runat="server" CssClass="btn btn-warning btn-sm align-self-start btn-block text-white" BackColor="#ee8000" OnClientClick='<%# Eval("productID", "productDetail(\"{0}\"); return false;") %>'><i class="fa fa-search mr-2"></i>Detail</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:Repeater>


                    <div class="col-md-12 col-lg-12 col-xs-12 col-sm-12 px-0 mt-2" runat="server" id="paginationDiv">
                        <nav aria-label="Page navigation example">
                            <ul class="pagination justify-content-end flat pagination-primary text-warning">
                                <asp:Repeater ID="rptPager" runat="server">
                                    <ItemTemplate>
                                        <li class="page-item">
                                            <asp:LinkButton ID="lnkPage" CssClass="page-link" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                Enabled='<%# Eval("Enabled") %>' OnClick="Page_Changed"></asp:LinkButton>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </nav>
                    </div>
                </div>

            </div>

            <asp:Literal ID="litError" runat="server"></asp:Literal>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div id="modalDialogProductDetail" class="modal fade modal-dialog-add" role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false" style="overflow-y: auto;">
        <div class="modal-dialog modal-lg" style="max-width: 1080px">
            <div class="modal-content">
                <asp:UpdatePanel ID="updatePanelProductDetail" runat="server" UpdateMode="Conditional"
                    ChildrenAsTriggers="false">
                    <Triggers>
                    </Triggers>
                    <ContentTemplate>
                        <div class="modal-header text-white" style="background-color: #fbd746">
                            <asp:Label ID="headerNameLbl" runat="server"></asp:Label>
                            <a type="button" class="close text-white" data-dismiss="modal">
                                <i class="fa fa-times text-white" aria-hidden="true"></i>
                            </a>
                        </div>
                        <div class="modal-body pt-2" id="modal-body">
                            <div class="card col-md-3">
                                <div class="card-header bg-white">
                                    <asp:Image runat="server" ID="productImageUrlPopup" AlternateText="Product Image" Style="width: 180px; height: auto;" />
                                </div>
                                <div class="card-body">
                                    <div class="row mt-2">
                                        <asp:Image runat="server" ID="miniMarketImageUrlPopup" Style="width: 100px; height: auto;" />
                                    </div>
                                    <div class="row  mt-2">
                                        <asp:Label runat="server" CssClass="h6 mr-2" Style="color: #ee8000" ID="productNamePopupLbl"></asp:Label>
                                    </div>
                                    <div class="row mt-2">
                                        <span class="badge badge-warning">
                                            <span class="h6">Rp.</span>
                                            <asp:Label runat="server" CssClass="h6 ml-1" ID="productPricePopupLbl"></asp:Label>
                                        </span>
                                    </div>

                                </div>
                            </div>
                            <div class="mt-2 col-md-12">
                                <div class="row">
                                    <span class="font-weight-bold">Deskripsi Produk</span>
                                </div>
                                <div class="row">
                                    <asp:Label runat="server" Style="text-align: justify;" ID="productDescPopupLbl"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>



    <script type="text/javascript">

        var _baseUrl = '<%=ResolveUrl("~/")%>';

        Sys.Application.add_init(appl_init);

        function appl_init() {
            var pgRegMgr = Sys.WebForms.PageRequestManager.getInstance();
            pgRegMgr.add_endRequest(EndHandlerGrid);
            pgRegMgr.add_beginRequest(BeginHandlerGrid);
        }

        function BeginHandlerGrid(sender, args) {
            init();
        }

        function EndHandlerGrid(sender, args) {
            init();
        }

        $(document).ready(function () {
            init();
        });

        function imgError(me) {
            // place here the alternative image
            var AlterNativeImg = "/assets/images/Image_not_available.png";

            // to avoid the case that even the alternative fails        
            if (AlterNativeImg != me.src)
                me.src = AlterNativeImg;
        }

        function init() {
            setTimeout(function () { getLocation(); showPosition(); }, 500);
            $(document).keypress(
                function (event) {
                    if (event.which == '13') {
                        event.preventDefault();
                    }
                });
        }

        function productDetail(productID) {
            if (productID != '') {
                $('#<%= hiddenProductID.ClientID %>').val(productID);
            }

            $('#<%= productDetailBtn.ClientID %>').click();
        }

        function getLocation() {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(showPosition);
            } else {
                console.log("Geolocation is not supported by this browser.");
            }
        }

        function showPosition(position) {
            var lat1 = position.coords.latitude;
            var lon1 = position.coords.longitude;



            //-6.2087634,
            //106.845599
            /*
                var lat2 = -6.177437;
                var lon2 = 106.621188;
 
                console.log(getDistanceFromLatLonInKm(lat1, lon1, lat2, lon2) + " km");
            */

            $('#<%= hiddenMyLatitude.ClientID %>').val(lat1);
            $('#<%= hiddenMyLongitude.ClientID %>').val(lon1);
        }

        function getDistanceFromLatLonInKm(lat1, lon1, lat2, lon2) {
            var R = 6371; // Radius of the earth in km
            var dLat = deg2rad(lat2 - lat1);  // deg2rad below
            var dLon = deg2rad(lon2 - lon1);
            var a =
                Math.sin(dLat / 2) * Math.sin(dLat / 2) +
                Math.cos(deg2rad(lat1)) * Math.cos(deg2rad(lat2)) *
                Math.sin(dLon / 2) * Math.sin(dLon / 2)
                ;
            var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
            var d = R * c; // Distance in km
            return d;
        }

        function deg2rad(deg) {
            return deg * (Math.PI / 180)
        }

    </script>
</asp:Content>
