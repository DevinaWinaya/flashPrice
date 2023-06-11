<%@ Page Title="" Language="C#" MasterPageFile="~/admin.Master" AutoEventWireup="true" CodeBehind="masterProduct.aspx.cs" Inherits="flashPrice.masterData.masterProduct" %>

<asp:Content ID="Content2" ContentPlaceHolderID="pageNamePh" runat="server">
    Master Data Product

</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPh" runat="server">
    <div id="content">
        <asp:UpdatePanel ID="updAction" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:HiddenField ID="hiddenProductID" runat="server" />
                <asp:Button ID="productDetailBtn" runat="server" Text="Product Detail" OnClick="productDetailBtn_Click" Style="display: none;" />
                <asp:Button ID="save_hiddenBtn" runat="server" Text="save" OnClick="save_hiddenBtn_Click" Style="display: none;" />
                <asp:Button ID="delete_hiddenBtn" runat="server" Text="delete" OnClick="delete_hiddenBtn_Click" Style="display: none;" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="row col-md-12 col-sm-12 col-xs-12">
            <div class="col-md-3 col-sm-12 col-xs-12">
                <asp:TextBox runat="server" ID="searchTextBox" CssClass="form-control autocomplete" placeHolder="Cari Produk Berdasarkan Nama Produk"></asp:TextBox>
                <act:AutoCompleteExtender runat="server" ID="dataProduct" TargetControlID="searchTextBox"
                    ServiceMethod="getListProductCached" ServicePath="~/webService/wsvProduct.asmx"
                    MinimumPrefixLength="2" CompletionInterval="100" EnableCaching="true" CompletionSetCount="10"
                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" DelimiterCharacters=";,:"
                    ShowOnlyCurrentWordInCompletionListItem="true">
                </act:AutoCompleteExtender>
            </div>

            <div class="col-md-3 col-sm-12 col-xs-12">
                <asp:DropDownList runat="server" ID="categoryProductDD" CssClass="form-control">
                    <asp:ListItem Text="Pilih Kategori" Value=""></asp:ListItem>
                    <asp:ListItem Text="Makanan" Value="C001"></asp:ListItem>
                    <asp:ListItem Text="Minuman" Value="C002"></asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="col-md-3 col-sm-12 col-xs-12" style="display:none;">
                <asp:TextBox runat="server" ID="miniMarketSearchTextBox" CssClass="form-control autocomplete" placeHolder="Cari Destinasi Minimarketmu Disini"></asp:TextBox>

                <act:AutoCompleteExtender runat="server" ID="dataMiniMarket" TargetControlID="miniMarketSearchTextBox"
                    ServiceMethod="getListMiniMarketCached" ServicePath="~/webService/wsvMiniMarket.asmx"
                    MinimumPrefixLength="2" CompletionInterval="100" EnableCaching="true" CompletionSetCount="10"
                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" DelimiterCharacters=";,:"
                    ShowOnlyCurrentWordInCompletionListItem="true">
                </act:AutoCompleteExtender>

            </div>

            <div class="col-md-3 col-sm-12 col-xs-12">
                <asp:LinkButton runat="server" ID="searchBtn" OnClick="searchBtn_Click" CssClass="btn btn-primary"><i class="fa fa-search mr-2"> </i>Search</asp:LinkButton>
            </div>
        </div>


        <asp:HiddenField ID="hdSortEx" runat="server" />
        <asp:HiddenField ID="hdSortDir" runat="server" />

        <asp:UpdatePanel ID="updGridView" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Literal ID="litErrorLogin" runat="server"></asp:Literal>
                <div class="datagrid col-md-12 col-sm-12 col-xs-12" id="dvGrid" runat="server">
                    <!-- main grid -->
                    <div style="overflow-y: auto; width: auto; overflow-x: auto;" class="mt-4 table table-bordered dataTable">
                        <asp:GridView ID="gvMain" runat="server" EnableModelValidation="True" AutoGenerateColumns="false"
                            AllowPaging="true" AllowSorting="true" PageSize="25" OnPageIndexChanging="gvMain_PageIndexChanging" OnSorting="gvMain_Sorting" EmptyDataText="Data tidak ditemukan"
                            OnRowDataBound="gvMain_RowDataBound" CssClass="table table-hover table-bordered">
                            <Columns>

                                <asp:TemplateField HeaderText="#" HeaderStyle-BackColor="#4e73df" HeaderStyle-ForeColor="White">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# (Container.DataItemIndex)+1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Pilihan" HeaderStyle-BackColor="#4e73df" HeaderStyle-ForeColor="White">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="detailBtn" runat="server" Text="Detail" CssClass="btn btn-block btn-primary"
                                            CommandName="detail" OnClientClick='<%# Eval("productID", "productDetail(\"{0}\"); return false;") %>' />

                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="productName" HeaderText="Nama" HeaderStyle-BackColor="#4e73df" HeaderStyle-ForeColor="White" />

                                <asp:BoundField DataField="productPrice" HeaderText="Harga" HeaderStyle-BackColor="#4e73df" HeaderStyle-ForeColor="White" />

                                <asp:TemplateField HeaderText="Minimarket" ItemStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#4e73df" HeaderStyle-ForeColor="White">
                                    <ItemTemplate>
                                        <asp:Image ID="imgMiniMarketType" Style="width: 100px; height: auto;" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Sponsorship" ItemStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#4e73df" HeaderStyle-ForeColor="White">
                                    <ItemTemplate>
                                        <asp:Label ID="isSponsorshipLbl" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>

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

    <div id="modalDialogProductDetail" class="modal fade modal-dialog-add" role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false" style="overflow-y: auto;">
        <div class="modal-dialog modal-lg" style="max-width: 1080px">
            <div class="modal-content">
                <asp:UpdatePanel ID="updatePanelProductDetail" runat="server" UpdateMode="Conditional"
                    ChildrenAsTriggers="false">
                    <Triggers>
                    </Triggers>
                    <ContentTemplate>
                        <div class="modal-header text-white" style="background-color: #4e73df">
                            <asp:Label ID="headerNameLbl" runat="server"></asp:Label>
                            <a type="button" class="close text-white" data-dismiss="modal">
                                <i class="fa fa-times text-white" aria-hidden="true"></i>
                            </a>
                        </div>
                        <div class="modal-body pt-2" id="modal-body">

                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="row mt-3">
                                    <div class="col-md-3 col-xs-12">
                                        Nama Produk
                                    </div>
                                    <div class="col-md-9 col-xs-12">
                                        <asp:TextBox runat="server" ID="productNameTb" CssClass="form form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row mt-3">
                                    <div class="col-md-3 col-xs-12">
                                        Harga Produk
                                    </div>
                                    <div class="col-md-9 col-xs-12">
                                        <asp:TextBox runat="server" ID="productPriceTb" CssClass="form form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row mt-3 mb-3">
                                    <div class="col-md-3 col-xs-12">
                                        Sponsorship
                                    </div>
                                    <div class="col-md-9 col-xs-12">
                                        <asp:DropDownList runat="server" ID="isSponsorshipDD" CssClass="form form-control">
                                            <asp:ListItem Value="T" Text="True"></asp:ListItem>
                                            <asp:ListItem Value="F" Text="False"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:LinkButton runat="server" ID="deleteBtn" CssClass="btn btn-outline-danger" OnClientClick="deletePrompt(); return false;"><i class="fa fa-trash mr-2"></i> Delete</asp:LinkButton>
                                <asp:LinkButton runat="server" ID="saveBtn" CssClass="btn btn-outline-primary" OnClientClick="savePrompt(); return false;"><i class="fa fa-save mr-2"></i> Save</asp:LinkButton>
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
            //$(document).keypress(
            //    function (event) {
            //        if (event.which == '13') {
            //            event.preventDefault();
            //        }
            //    });
        }

        function productDetail(productID) {
            if (productID != '') {
                $('#<%= hiddenProductID.ClientID %>').val(productID);
            }

            $('#<%= productDetailBtn.ClientID %>').click();
        }

        function deletePrompt() {
            Swal.fire({
                title: 'Apakah anda yakin ?',
                text: "Data produk akan terhapus",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#4e73df',
                cancelButtonColor: '#e74a3b',
                confirmButtonText: 'Ya, saya yakin'
            }).then((result) => {
                if (result.isConfirmed) {
                    $('#<%= delete_hiddenBtn.ClientID%>').click();
                }
            });
        }

        function savePrompt() {
            if (
                $('#<%= productNameTb.ClientID %>').val() != "" &&
                $('#<%= productPriceTb.ClientID %>').val() != ""
            ) {
                Swal.fire({
                    title: 'Apakah anda yakin ?',
                    text: "Data produk akan terupdate",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#4e73df',
                    cancelButtonColor: '#e74a3b',
                    confirmButtonText: 'Ya, saya yakin'
                }).then((result) => {
                    if (result.isConfirmed) {
                        $('#<%= save_hiddenBtn.ClientID%>').click();
                    }
                });
            }
            else {
                Swal.fire({
                    icon: 'error',
                    title: 'error',
                    text: 'Dimohon untuk mengisi field yang kosong',
                });
            }
        }

        function swalSuccess() {
            Swal.fire({
                icon: 'success',
                title: 'success',
                text: 'Data berhasil tersimpan',
            });
        }

        function swalFailed() {
            Swal.fire({
                icon: 'error',
                title: 'failed',
                text: 'Data gagal tersimpan',
            });
        }

    </script>


</asp:Content>
