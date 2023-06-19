<%@ Page Title="" Language="C#" MasterPageFile="~/default.Master" AutoEventWireup="true" CodeBehind="product.aspx.cs" Inherits="flashPrice.pages.product" UICulture="id-ID" Culture="id-ID" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Flash Price</title>
    <style>
    </style>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPh" runat="server">
    <asp:UpdatePanel ID="updAction" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <asp:HiddenField ID="hiddenMyLatitude" runat="server" />
            <asp:HiddenField ID="hiddenMyLongitude" runat="server" />

            <asp:HiddenField ID="hiddenProductCompareID" runat="server" />
            <asp:HiddenField ID="hiddenProductID" runat="server" />
            <asp:HiddenField ID="hdnPageIdx" runat="server" />
            <asp:Button ID="productDetailBtn" runat="server" Text="Product Detail" OnClick="productDetailBtn_Click" Style="display: none;" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:HiddenField ID="hdSortEx" runat="server" />
    <asp:HiddenField ID="hdSortDir" runat="server" />


    <nav class="navbar navbar-expand bg-body-tertiary sticky-top text-center mt-3" style="background-color: white;">


        <div class="col-md-3 pl-0 pr-1">
            <asp:TextBox runat="server" ID="navSearchTextBox" CssClass="form-control autocomplete" placeHolder="Temukan produkmu disini . . ."></asp:TextBox>
<%--            <act:AutoCompleteExtender runat="server" ID="dataProduct" TargetControlID="navSearchTextBox"
                ServiceMethod="getListProductCached" ServicePath="~/webService/wsvProduct.asmx"
                MinimumPrefixLength="2" CompletionInterval="100" EnableCaching="true" CompletionSetCount="10"
                CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" DelimiterCharacters=";,:"
                ShowOnlyCurrentWordInCompletionListItem="true">
            </act:AutoCompleteExtender>--%>
        </div>

        <div class="col-md-3 px-1">
            <asp:DropDownList runat="server" ID="categoryProductDD" CssClass="form-control">
                <asp:ListItem Text="Pilih Kategori" Value=""></asp:ListItem>
                <asp:ListItem Text="Makanan" Value="C001"></asp:ListItem>
                <asp:ListItem Text="Minuman" Value="C002"></asp:ListItem>
            </asp:DropDownList>

        </div>
        <div class="col-md-8 px-1 text-left">
            <asp:LinkButton runat="server" ID="navSearchBtn" OnClick="navSearchBtn_Click" CssClass="btn btn-green" Style=""><i class="fa fa-search mr-2"> </i>Search</asp:LinkButton>
        </div>
    </nav>

    <asp:Literal runat="server" ID="farFromXLit"></asp:Literal>

    <asp:UpdatePanel ID="updError" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="col-md-12 col-xs-12 col-sm-12 mt-4 alert alert-danger" id="errDiv" runat="server" visible="false">
                <asp:Label runat="server" ID="errLbl" CssClass="col-md-12"></asp:Label>
            </div>

            <asp:Literal runat="server" ID="testLit"></asp:Literal>

        </ContentTemplate>

    </asp:UpdatePanel>

    <div class="col-md-12 col-xs-12 col-sm-12 mt-3">
        <asp:UpdatePanel ID="updGridView" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Literal ID="myLocationLit" runat="server"></asp:Literal>
                <asp:Literal ID="litErrorLogin" runat="server"></asp:Literal>
                <div class="datagrid" id="dvGrid" runat="server" style="display: none;">
                    <!-- main grid -->
                    <div style="height: 350px; overflow-y: auto; width: auto;" class="mt-4">
                        <asp:GridView ID="gvMain" runat="server" EnableModelValidation="True" AutoGenerateColumns="false"
                            AllowPaging="False" PageSize="100" OnPageIndexChanging="gvMain_PageIndexChanging"
                            OnRowDataBound="gvMain_RowDataBound" CssClass="table table-hover table-bordered">
                            <Columns>
                                <asp:TemplateField HeaderText="#" HeaderStyle-BackColor="#406C1C" HeaderStyle-ForeColor="#f7f89f">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# (Container.DataItemIndex)+1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="MiniMarket" ItemStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#406C1C" HeaderStyle-ForeColor="#f7f89f">
                                    <ItemTemplate>
                                        <asp:Image ID="imgMiniMarketType" Style="width: 100px; height: auto;" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="miniMarketName" HeaderText="Nama" HeaderStyle-BackColor="#406C1C" HeaderStyle-ForeColor="#f7f89f" />
                                <asp:BoundField DataField="miniMarketAddress" HeaderText="Alamat" HeaderStyle-BackColor="#406C1C" HeaderStyle-ForeColor="#f7f89f" />
                                <asp:BoundField DataField="distanceFromMe" HeaderText="Jarak (meter)" HeaderStyle-BackColor="#406C1C" HeaderStyle-ForeColor="#f7f89f" />
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
            <div id="resultDiv" class="pt-2" runat="server" style="height: auto;">
                <p style="color: #c1c27d" class="h4 pl-0 mx-auto col-md-8 col-xs-12 col-sm-12">Daftar List Produk yang tersedia</p>

                <div id="queryResultDiv" class="row mx-auto col-md-8 col-xs-12 col-sm-12 border border-secondary-50" runat="server">


                    <asp:Repeater ID="resultRepeater" runat="server" OnItemDataBound="resultRepeater_ItemDataBound" OnItemCommand="resultRepeater_ItemCommand">
                        <ItemTemplate>

                            <div class="col-md-3 mb-3 d-flex align-items-stretch">
                                <div class="card mt-3 mb-3 col-md-12">
                                    <div class="card-header bg-white">
                                        <asp:Literal ID="litProductImg" runat="server"></asp:Literal>
                                    </div>
                                    <div class="card-body">
                                        <div class="row pt-2 pb-2">
                                            <span class="text-left col-md-12">
                                                <img src="<%#Eval("productImageUrl")%>" onerror="imgError(this)" style="border-radius: 10px; height: auto; width: 280px;" class="img-fluid" />
                                            </span>

                                            <span class="pt-2 pb-2">
                                                <asp:Label ID="productNameLbl" CssClass="h6 text-green-leaf" runat="server" Text='<%#Eval("productName") %>'></asp:Label>
                                                <asp:Label ID="categoryPriceLbl" CssClass="badge badge-primary" runat="server" Text='<%#Eval("categoryPrice") %>'></asp:Label>
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
                                        <asp:LinkButton ID="detailBtn" runat="server" CssClass="btn btn-yellow btn-sm align-self-start btn-block text-green-leaf" OnClientClick='<%# String.Format("productDetail(\"{0}\",\"{1}\"); return false;", Eval("productID"), Eval("compareTo")) %>'><i class="fa fa-search mr-2"></i>Detail</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:Repeater>


                    <div class="col-md-12 col-lg-12 col-xs-12 col-sm-12 px-0 mt-2" runat="server" id="paginationDiv">
                        <nav aria-label="Page navigation example">
                            <ul class="pagination justify-content-end flat pagination-primary text-green-leaf">
                                <asp:Repeater ID="rptPager" runat="server">
                                    <ItemTemplate>
                                        <li class="page-item">
                                            <asp:LinkButton ID="lnkPage" CssClass="page-link text-green-leaf" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
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
                        <div class="modal-header text-white" style="background-color: #f7f89f">
                            <asp:Label ID="headerNameLbl" runat="server"></asp:Label>
                            <a type="button" class="close text-white" data-dismiss="modal">
                                <i class="fa fa-times text-green-leaf" aria-hidden="true"></i>
                            </a>
                        </div>
                        <div class="modal-body pt-2" id="modal-body">

                            <div class="card mb-3 col-md-12">
                                <div class="row no-gutters">
                                    <div class="col-md-4 text-center my-auto">
                                        <asp:Image runat="server" ID="productImageUrlPopup" onerror="imgError(this)" AlternateText="Product Image" Style="width: 200px; height: auto;" />
                                    </div>
                                    <div class="col-md-8">
                                        <div class="card-body">

                                            <asp:Image runat="server" ID="miniMarketImageUrlPopup" CssClass="mr-2" Style="width: 55px; height: auto;" />
                                            <asp:Label runat="server" CssClass="h5 card-title mr-2 text-green-leaf" ID="productNamePopupLbl"></asp:Label>


                                            <span>
                                                <span class="text-green-leaf font-weight-bold">Rp.</span>
                                                <asp:Label runat="server" CssClass="ml-1 text-green-leaf font-weight-bold" ID="productPricePopupLbl"></asp:Label>
                                            </span>

                                            <p class="card-text mt-3">
                                                We would like to express our sincerest apologies for any inconvenience this may cause. Regrettably, we find ourselves in a situation where we are unable to present the product description at this time. We understand the frustration and disappointment this may bring, and we assure you that we are actively working to rectify this issue. We appreciate your understanding and patience as we strive to provide the best possible service. Once again, please accept our apologies for any inconvenience caused, and we thank you for your continued support.
                                            </p>

                                            <p class="card-text">
                                                <small class="text-muted">Regards, Admin Flash Price
                                                </small>
                                            </p>

                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="pt-2 pb-2 h5">
                                <asp:Label runat="server" ID="compareLbl"></asp:Label>
                            </div>

                            <div class="card mb-3 col-md-12" id="compareDIV" visible="false" runat="server">
                                <div class="row no-gutters">
                                    <div class="col-md-4 text-center my-auto">
                                        <asp:Image runat="server" ID="imageProductCompare" onerror="imgError(this)" AlternateText="Product Image" Style="width: 200px; height: auto;" />
                                    </div>
                                    <div class="col-md-8">
                                        <div class="card-body">

                                            <asp:Image runat="server" ID="miniMarketCompareImageUrlPopup" CssClass="mr-2" Style="width: 55px; height: auto;" />
                                            <asp:Label runat="server" CssClass="h5 card-title mr-2 text-green-leaf" ID="productCompareNameLbl"></asp:Label>


                                            <span>
                                                <span class="text-green-leaf font-weight-bold">Rp.</span>
                                                <asp:Label runat="server" CssClass="ml-1 text-green-leaf font-weight-bold" ID="productComparePricePopupLbl"></asp:Label>
                                            </span>

                                            <p class="card-text mt-3">
                                                We would like to express our sincerest apologies for any inconvenience this may cause. Regrettably, we find ourselves in a situation where we are unable to present the product description at this time. We understand the frustration and disappointment this may bring, and we assure you that we are actively working to rectify this issue. We appreciate your understanding and patience as we strive to provide the best possible service. Once again, please accept our apologies for any inconvenience caused, and we thank you for your continued support.
                                            </p>

                                            <p class="card-text">
                                                <small class="text-muted">Regards, Admin Flash Price
                                                </small>
                                            </p>

                                        </div>
                                    </div>
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
            loading_start();
        }

        function loading_start() {
            $(document).ready(function () {
                $.blockUI({ message: "<h1 class='text-success p-2' style='font-size:x-large'><i class='fa fa-spin fa-circle-o-notch mr-2'></i><span >Please Wait</span></h1>", baseZ: 5000 });
            });
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

        function productDetail(productID, productCompareID) {
            if (productID != '' && productCompareID != '') {
                console.log(productID);
                $('#<%= hiddenProductID.ClientID %>').val(productID);
                $('#<%= hiddenProductCompareID.ClientID %>').val(productCompareID);
            }

            $('#<%= productDetailBtn.ClientID %>').click();
        }

        function getLocation() {

            const options = {
                enableHighAccuracy: true,
                timeout: 5000,
                maximumAge: 0,
            };

            if (navigator.geolocation) {
                navigator.geolocation.watchPosition(showPosition);
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
