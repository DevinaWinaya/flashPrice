﻿<%@ Page Title="" Language="C#" MasterPageFile="~/default.Master" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="flashPrice.pages.home" UICulture="id-ID" Culture="id-ID" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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


    <asp:UpdatePanel ID="updatePanelSearchResultRepeater" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="navSearchBtn" />
        </Triggers>
        <ContentTemplate>

            <asp:HiddenField ID="hdSortEx" runat="server" />
            <asp:HiddenField ID="hdSortDir" runat="server" />

            

            <div id="resultDiv" runat="server">
                <nav class="navbar navbar-expand-lg navbar-light sticky-top" style="background-color: #fbd746;">
                    <a class="navbar-brand text-white" href="home.aspx">
                        <img src="assets/images/flashPriceLogo.png" style="width: 150px;" /></a>
                    <span></span>
                    <div class="navbar-nav col-md-8 mx-auto">
                        <asp:TextBox runat="server" ID="navSearchTextBox" CssClass="form-control autocomplete mr-2 col-md-5" placeHolder="Temukan produkmu disini . . ."></asp:TextBox>

                        <act:AutoCompleteExtender runat="server" ID="dataProduct" TargetControlID="navSearchTextBox"
                            ServiceMethod="getListProductCached" ServicePath="~/webService/wsvProduct.asmx"
                            MinimumPrefixLength="2" CompletionInterval="100" EnableCaching="true" CompletionSetCount="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" DelimiterCharacters=";,:"
                            ShowOnlyCurrentWordInCompletionListItem="true">
                        </act:AutoCompleteExtender>

                        <asp:DropDownList runat="server" ID="categoryProductDD" CssClass="form-control col-md-2 mr-2">
                            <asp:ListItem Text="Pilih Kategori" Value=""></asp:ListItem>
                            <asp:ListItem Text="Makanan" Value="C001"></asp:ListItem>
                            <asp:ListItem Text="Minuman" Value="C002"></asp:ListItem>
                        </asp:DropDownList>

                        <asp:LinkButton runat="server" ID="navSearchBtn" OnClick="navSearchBtn_Click" CssClass="btn btn-light"><i class="fa fa-search"></i></asp:LinkButton>
                    </div>

                    <span class="navbar-text font-weight-bold" style="font-size: small;">
                        <i class="fa fa-scale-balanced mr-1"></i>Bandingkan dan temukan produk pilihanmu dengan FlashPrice
                    </span>
                </nav>

                <asp:Label runat="server" ID="errLbl" Visible="false"></asp:Label>

                <asp:Literal runat="server" ID="testLit"></asp:Literal>

                <div id="queryResultDiv" class="row mx-auto col-md-8 mt-3 col-xs-12 col-sm-12 border border-secondary-50" runat="server">
                    <asp:Literal ID="litError" runat="server"></asp:Literal>
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
                                                <img src="<%#Eval("productImageUrl") %>" style="border-radius: 10px; height: auto; width: 280px;" class="img-fluid" alt="Product Image" />
                                            </span>

                                            <span class="mt-3 mb-2">
                                                <asp:Label ID="productNameLbl" CssClass="h6" Style="color: #ee8000;" runat="server" Text='<%#Eval("productName") %>'></asp:Label>
                                            </span>
                                        </div>
                                        <div class="row">
                                            <span>
                                                <i class="fa fa-money-bill-1 mr-2"></i>
                                                Rp.<asp:Label ID="productPriceLbl" CssClass="ml-1" runat="server" Text='<%#Eval("productPrice")%>'></asp:Label>
                                            </span>
                                        </div>
                                        <div class="row">
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
                                    <div class="row mt-3 card-footer bg-white">
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
                                            <asp:Label runat="server" CssClass="h6 ml-1 " ID="productPricePopupLbl"></asp:Label>
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


    <asp:UpdatePanel ID="updGridView" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Literal ID="litErrorLogin" runat="server"></asp:Literal>
            <div class="datagrid" id="dvGrid">
                <!-- main grid -->
                <div style="overflow-x: auto; width: auto;" class="mt-4">
                    <asp:GridView ID="gvMain" runat="server" EnableModelValidation="True" AutoGenerateColumns="true"
                        AllowPaging="True" PageSize="100" AllowSorting="true" OnPageIndexChanging="gvMain_PageIndexChanging"
                        OnRowDataBound="gvMain_RowDataBound" CssClass="table table-hover table-bordered" HeaderStyle-ForeColor="White">
                        <Columns>
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="4" FirstPageText="First"
                            LastPageText="Last" />
                        <RowStyle CssClass="td" />
                        <SelectedRowStyle CssClass="thspecalt" />
                        <AlternatingRowStyle CssClass="tdalt" />
                        <HeaderStyle CssClass="th thead-light text-center" />
                    </asp:GridView>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>


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
