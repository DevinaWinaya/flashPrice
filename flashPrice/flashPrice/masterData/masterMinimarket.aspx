<%@ Page Title="" Language="C#" MasterPageFile="~/admin.Master" AutoEventWireup="true" CodeBehind="masterMinimarket.aspx.cs" Inherits="flashPrice.masterData.masterMinimarket" %>

<asp:Content ID="Content2" ContentPlaceHolderID="pageNamePh" runat="server">
    Master Data Minimarket
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPh" runat="server">
    <div id="content">
        <asp:UpdatePanel ID="updAction" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:HiddenField ID="hiddenDataID" runat="server" />
                <asp:Button ID="dataDetail_hiddenBtn" runat="server" Text="Data Detail" OnClick="dataDetail_hiddenBtn_Click" Style="display: none;" />
                <asp:Button ID="save_hiddenBtn" runat="server" Text="save" OnClick="save_hiddenBtn_Click" Style="display: none;" />
                <asp:Button ID="delete_hiddenBtn" runat="server" Text="delete" OnClick="delete_hiddenBtn_Click" Style="display: none;" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="row col-md-12 col-sm-12 col-xs-12">
            <div class="col-md-4 col-sm-12 col-xs-12">
                <asp:TextBox runat="server" ID="searchTextBox" CssClass="form-control autocomplete" placeHolder="Cari Minimarket"></asp:TextBox>
                <act:AutoCompleteExtender runat="server" ID="dataMiniMarket" TargetControlID="searchTextBox"
                    ServiceMethod="getListMiniMarketCached" ServicePath="~/webService/wsvMiniMarket.asmx"
                    MinimumPrefixLength="2" CompletionInterval="100" EnableCaching="true" CompletionSetCount="10"
                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" DelimiterCharacters=";,:"
                    ShowOnlyCurrentWordInCompletionListItem="true">
                </act:AutoCompleteExtender>
            </div>

            <div class="col-md-4 col-sm-12 col-xs-12">
                <asp:LinkButton runat="server" ID="searchBtn" OnClick="searchBtn_Click" CssClass="btn btn-primary"><i class="fa fa-search mr-2"> </i>Search</asp:LinkButton>
                <asp:LinkButton runat="server" ID="addBtn" OnClick="addBtn_Click" CssClass="btn btn-success"><i class="fa fa-plus mr-2"> </i>Add Data</asp:LinkButton>

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

                                <asp:TemplateField HeaderText="Pilihan" HeaderStyle-BackColor="#4e73df" HeaderStyle-ForeColor="White" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="detailBtn" runat="server" Text="Detail" CssClass="btn btn-block btn-primary"
                                            CommandName="detail" OnClientClick='<%# Eval("minimarketID", "dataDetail(\"{0}\"); return false;") %>' />

                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="miniMarketName" HeaderText="Nama" HeaderStyle-BackColor="#4e73df" HeaderStyle-ForeColor="White" ItemStyle-Wrap="false" />

                                <asp:TemplateField HeaderText="Tipe" ItemStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#4e73df" HeaderStyle-ForeColor="White" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Image ID="imgMiniMarketType" Style="width: 100px; height: auto;" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="miniMarketAddress" HeaderText="Alamat" HeaderStyle-BackColor="#4e73df" HeaderStyle-ForeColor="White" />

                                <asp:BoundField DataField="miniMarketLattitude" HeaderText="Minimarket Lattitude" HeaderStyle-BackColor="#4e73df" HeaderStyle-ForeColor="White" ItemStyle-Wrap="false" />

                                <asp:BoundField DataField="miniMarketLongitude" HeaderText="Minimarket Longitude" HeaderStyle-BackColor="#4e73df" HeaderStyle-ForeColor="White" ItemStyle-Wrap="false" />

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
                                        Nama Minimarket
                                    </div>
                                    <div class="col-md-9 col-xs-12">
                                        <asp:TextBox runat="server" ID="minimarketNameTb" CssClass="form form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row mt-3">
                                    <div class="col-md-3 col-xs-12">
                                        Alamat Minimarket
                                    </div>
                                    <div class="col-md-9 col-xs-12">
                                        <asp:TextBox runat="server" ID="minimarketAddressTb" CssClass="form form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row mt-3">
                                    <div class="col-md-3 col-xs-12">
                                        Minimarket Lattitude
                                    </div>
                                    <div class="col-md-9 col-xs-12">
                                        <asp:TextBox runat="server" ID="minimarketLattitudeTb" CssClass="form form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row mt-3">
                                    <div class="col-md-3 col-xs-12">
                                        Minimarket Longitude
                                    </div>
                                    <div class="col-md-9 col-xs-12">
                                        <asp:TextBox runat="server" ID="minimarketLongitudeTb" CssClass="form form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row mt-3 mb-3">
                                    <div class="col-md-3 col-xs-12">
                                        Tipe Minimarket 
                                    </div>
                                    <div class="col-md-9 col-xs-12">
                                        <asp:DropDownList runat="server" ID="minimarketTypeDD" CssClass="form form-control">
                                            <asp:ListItem Value="Alfamaret" Text="Alfamart"></asp:ListItem>
                                            <asp:ListItem Value="Indomaret" Text="Indomaret"></asp:ListItem>
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

        function dataDetail(dataID) {
            if (dataID != '') {
                $('#<%= hiddenDataID.ClientID %>').val(dataID);
            }

            $('#<%= dataDetail_hiddenBtn.ClientID %>').click();
        }

        function deletePrompt() {
            Swal.fire({
                title: 'Apakah anda yakin ?',
                text: "Data akan terhapus",
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
                $('#<%= minimarketNameTb.ClientID %>').val() != "" &&
                $('#<%= minimarketAddressTb.ClientID %>').val() != "" &&
                $('#<%= minimarketLattitudeTb.ClientID %>').val() != "" &&
                $('#<%= minimarketLongitudeTb.ClientID %>').val() != ""
            ) {

                Swal.fire({
                    title: 'Apakah anda yakin ?',
                    text: "Data akan tersimpan",
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
