<%@ Page Title="" Language="C#" MasterPageFile="~/templateAnggota.Master" AutoEventWireup="true"
    CodeBehind="statusPinjamanAnggotaOnAnggota.aspx.cs" Inherits="NEWKOPKARI.anggota.statusPinjamanAnggotaOnAnggota" UICulture="id-ID" Culture="id-ID" %>

<asp:Content ID="titleContent" ContentPlaceHolderID="menuTitlePH" runat="server">
    Halaman Status Pinjaman Anggota
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPageContent" runat="server">
    <asp:Button ID="btnPrintLetter" runat="server" Text="printLetter" CssClass="invisible"
        OnClick="printBtn_onClick" Style="display: none" />


    <!-- updatePanel hidden field disini !-->
    <asp:UpdatePanel ID="updAction" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" style="display: none">
        <ContentTemplate>
            <asp:HiddenField ID="hidTxnID" runat="server" />
            <%--<asp:HiddenField ID="hidFileNameDownload" runat="server" />--%>
            <asp:HiddenField ID="hidDocIDDownload" runat="server" />
            <asp:HiddenField ID="hidDocIDPreview" runat="server"></asp:HiddenField>
            <asp:Button ID="btnDetail" runat="server" Text="editData" CssClass="invisible" OnClick="btnDetail_click" />
            <asp:Button ID="btnUploadDoc" runat="server" Text="uploadDoc" CssClass="invisible"
                OnClick="btnUploadDoc_click" />
            <asp:Button ID="btnProcessDoc" runat="server" Text="processDoc" CssClass="invisible"
                OnClick="btnProcessDoc_click" />

            <asp:Button ID="fakeBtnUpload" runat="server" Text="uploadDoc" CssClass="invisible"
                OnClick="btnUpload_click" />

            <asp:Button ID="nextBtn_hide" runat="server" Text="processDoc" CssClass="invisible"
                OnClick="nextBtn_click" />
            <asp:Button ID="btnPreviewCheckHide" runat="server" Text="Preview" CssClass="invisible" Style="padding: 5px" OnClick="btnPreviewCheckHide_click" />
            <asp:Button ID="btnPreviewHide" runat="server" Text="Preview" CssClass="invisible" Style="padding: 5px" OnClick="btnPreviewHide_click" />
            <asp:Button runat="server" ID="uploadDocSaveBtn" CssClass="btn btn-primary no-mn-bottom"
                Text="Simpan Sebagian" OnClick="uploadDocSaveBtn_click" />
            <asp:Button runat="server" ID="uploadDocSubmitBtn" CssClass="btn btn-success no-mn-bottom"
                Text="Submit Dokumen" OnClick="uploadDocSubmitBtn_click" />
            <asp:Button ID="btnViewPHP" runat="server" Text="viewPHP" CssClass="visible"
                OnClick="btnViewPHP_click" />
            <%--            <asp:Button ID="printBtn" runat="server" Text="printLetter" CssClass="invisible"
                OnClick="printBtn_onClick" />--%>
        </ContentTemplate>
    </asp:UpdatePanel>


    <!-- updatePanel hidden field disini !-->
    <!--------------------------------------------------------------->
    <!-- ini awal bagian header filter -->
    <div class="col-md-12 col-sm-12 col-xs-12 no-margin no-padding" style="margin-left: -16px; margin-top: 0px;">
        <div class="x_panel">
            <div class="x_content">
                <!-- pencarian -->
                <div class="row no-margin">
                    <div class="col-md-3 col-sm-12 col-xs-12 splay_inputx has-feedback">
                        <div class="form-group">
                            <label class="control-labelc">
                                Periode Mulai</label>
                            <asp:TextBox ID="txtFilterDateFrom" CssClass="form-control datepicker has-feedback-left bg-white"
                                runat="server" />

                        </div>
                    </div>
                    <div class="col-md-3 col-sm-12 col-xs-12 xdisplay_inputx has-feedback">
                        <div class="form-group">
                            <label class="control-label">
                                Sampai</label>
                            <asp:TextBox ID="txtFilterDateTo" CssClass="form-control datepicker has-feedback-left bg-white"
                                runat="server" />

                        </div>
                    </div>
                    <div class="col-md-3 col-sm-2 col-xs-1disa2 xdisplay_inputx has-feedback">
                        <div class="form-group">
                            <label class="control-label">
                                Status</label>
                            <asp:ListBox ID="listboxFilterStatus" CssClass="form-control bg-white multiple-select" SelectionMode="Multiple"
                                runat="server"></asp:ListBox>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-2 col-xs-12 xdisplay_inputx has-feedback">
                        <div class="form-group">
                            <label class="control-label">
                                Action</label>
                            <div class="text-left">
                                <asp:LinkButton type="button" runat="server" class="btn btn-primary" ID="searchBtn" OnClick="searchBtn_Click"
                                    Text="Search"><i class="fa fa-search"></i> Search</asp:LinkButton>

                                <asp:LinkButton type="button" runat="server" class="btn btn-success" ID="btnAddPinjaman" OnClick="btnAddPinjaman_Click"
                                    Text="Search"><i class="fa fa-plus"></i> Add</asp:LinkButton>
                            </div>
                        </div>
                    </div>

                </div>
                <!-- ./pencarian -->
            </div>
        </div>
        <!-- EOF filtering-->
    </div>
    <!-- ini akhir bagian header filter 
    <!--------------------------------------------------------------->

    <asp:UpdatePanel ID="updatePanelPinjaman" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Literal ID="litErrorLogin" runat="server"></asp:Literal>
            <div class="datagrid" id="dvGrid">
                <asp:HiddenField ID="hdSortEx" runat="server" />
                <asp:HiddenField ID="hdSortDir" runat="server" />
                <!-- main grid -->
                <div style="overflow-x: auto; width: auto;">
                    <asp:GridView ID="gvMain" runat="server" EnableModelValidation="True" AutoGenerateColumns="False"
                        AllowPaging="True" PageSize="50" AllowSorting="true" OnPageIndexChanging="gvMain_PageIndexChanging"
                        OnSorting="gvList_Sorting" OnRowDataBound="gvMain_RowDataBound" CssClass="table table-hover table-bordered" HeaderStyle-ForeColor="White">
                        <Columns>
                            <asp:TemplateField HeaderText="No." HeaderStyle-ForeColor="White">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# (Container.DataItemIndex)+1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Pilihan" HeaderStyle-ForeColor="White">
                                <ItemTemplate>
                                    <asp:LinkButton ID="detailBtn" runat="server" Text="Detail" CssClass="btn btn-block btn-inverse-success"
                                        CommandName="detail" OnClientClick='<%# Eval("PinjamanID", "dataDetail(\"{0}\"); return false;") %>' />
                                    <asp:LinkButton ID="printLetterBtn" runat="server"
                                        CssClass="btn btn-block btn-inverse-primary" CommandName="print" OnClientClick='<%# Eval("PinjamanID", "printLetter(\"{0}\"); return false;") %>' Visible="false"><i class="fa fa-print"></i>Cetak Surat Perjanjian</asp:LinkButton>
                                    <asp:LinkButton ID="uploadDocBtn" runat="server" CssClass="btn btn-block btn-inverse-warning"
                                        CommandName="upload" OnClientClick='<%# Eval("PinjamanID", "uploadDoc(\"{0}\"); return false;") %>' Visible="false"><i class="icon-upload"></i>Upload Dokumen</asp:LinkButton>
                                    <asp:LinkButton ID="processDocBtn" runat="server" CssClass="btn btn-block btn-inverse-primary" ViewStateMode="Disabled"
                                        CommandName="processDocBtn" OnClientClick='<%# Eval("PinjamanID", "processDoc(\"{0}\"); return false;") %>' Visible="true"><i class="fa fa-upload"></i>Lengkapi Dokumen Disini</asp:LinkButton>

                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Status Pinjaman" HeaderStyle-ForeColor="White" SortExpression="statusByAnggota" HeaderStyle-Width="50%">
                                <ItemTemplate>
                                    <asp:Label ID="txnStatusLbl" runat="server" Text="Status XXX"
                                        CommandName="statusByAnggota" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="pinjamanNo" HeaderText="Nomor Pinjaman " SortExpression="pinjamanNo" />
                            <%-- <asp:BoundField DataField="pinjamanID" HeaderText="ID Pinjaman " SortExpression="pinjamanID" />--%>

                            <asp:BoundField DataField="txnDate" HeaderText="Tanggal Pengajuan" DataFormatString="{0:dd-MM-yyyy HH:mm:ss}" ItemStyle-CssClass="text-nowrap" SortExpression="txnDate" />

                            <asp:BoundField DataField="amount" HeaderText="Jumlah Pinjaman" SortExpression="amount"
                                ItemStyle-HorizontalAlign="Right" DataFormatString="{0:###,###,###}" />
                            <asp:BoundField DataField="packetName" HeaderText="Nama Paket" SortExpression="packetName" />
                            <asp:BoundField DataField="jmlCicil" HeaderText="Cicilan Ke" SortExpression="jmlCicil" />
                            <asp:BoundField DataField="cicilanPokok" HeaderText="Cicilan Pokok (Perbulan)" SortExpression="cicilanPokok" DataFormatString="{0:###,###,###}" />

                            <asp:TemplateField HeaderText="Jasa Perbulan" SortExpression="cicilanPokok">
                                <ItemStyle HorizontalAlign="Right" Wrap="false" />
                                <ItemTemplate>
                                    <asp:Label ID="cclJasaLbl" runat="server"></asp:Label>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Right" Wrap="false" />
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Total Angsuran (Bulan)" SortExpression="cicilanPokok">
                                <ItemStyle HorizontalAlign="Right" Wrap="false" />
                                <ItemTemplate>
                                    <asp:Label ID="totalCclLbl" runat="server"></asp:Label>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Right" Wrap="false" />
                            </asp:TemplateField>

                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="4" FirstPageText="First"
                            LastPageText="Last" />
                        <RowStyle CssClass="td" />
                        <SelectedRowStyle CssClass="thspecalt" />
                        <AlternatingRowStyle CssClass="tdalt" />
                        <HeaderStyle CssClass="th thead-light text-center" />
                    </asp:GridView>
                </div>
                <!-- end main grid -->
                <!-- modal dialog edit data (ini popup) -->
                <div id="modelDialogEditData" class="modal fade modal-dialog-add" role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false" style="overflow-y: auto;">
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content">
                            <!-- ini update panel buat popupnya -->
                            <asp:UpdatePanel ID="updatePanelFormData" runat="server" UpdateMode="Conditional"
                                ChildrenAsTriggers="false">
                                <Triggers>
                                </Triggers>
                                <ContentTemplate>
                                    <div class="modal-header bg-success text-white">
                                        <asp:Label ID="headerNameLb" runat="server"></asp:Label>
                                        <a type="button" class="close text-white" data-dismiss="modal">
                                            <i class="fa fa-times text-white" aria-hidden="true"></i>
                                        </a>
                                    </div>
                                    <div class="modal-body pt-2" id="modal-body">

                                        <!-- start informasi karyawan -->
                                        <div id="infoKaryawan">
                                            <div class="card">
                                                <div class="card-header-pills bg-success text-white p-1">

                                                    <asp:Table ID="Table5" runat="server" Style="width: 100%;" CssClass="pl-0">
                                                        <asp:TableRow>
                                                            <asp:TableCell Style="width: 100%; text-align: left;" CssClass="font-weight-bold">
                                                <asp:Label runat="server">Informasi Karyawan</asp:Label>
                                                    <a class="text-white pull-right" data-toggle="collapse" data-target="#collapseOne" aria-controls="collapseOne" aria-expanded="true" style="cursor:pointer">
                                                        <i class="fa fa-chevron-up text-white" aria-hidden="true"></i>
                                                        <i class="fa fa-chevron-down text-white" aria-hidden="true"></i>
                                                    </a>
                                                            </asp:TableCell>

                                                        </asp:TableRow>
                                                    </asp:Table>
                                                </div>
                                                <div id="collapseOne" class="collapse show" aria-labelledby="headingOne" data-parent="#infoKaryawan">
                                                    <div class="card-body rounded p-3 show nav-link">
                                                        <div class="col-md-12 col-sm-12 col-xs-12 no-margin pl-0">
                                                            <asp:Table ID="iAnggotaTbl" runat="server" CssClass="" CellPadding="5" CellSpacing="0">
                                                                <asp:TableRow>
                                                                    <asp:TableCell>
                                                                        <asp:Label ID="Label4" runat="server" Text="Nama Anggota"></asp:Label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <asp:Label ID="Label5" runat="server" Text=":"></asp:Label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <asp:Label ID="anggotaNameLb" runat="server"></asp:Label>
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow>
                                                                    <asp:TableCell>
                                                                        <asp:Label ID="Label7" runat="server" CssClass="mt-3" Text="Company"></asp:Label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <asp:Label ID="Label8" runat="server" Text=":"></asp:Label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <asp:Label ID="companyLb" runat="server"></asp:Label>
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow>
                                                                    <asp:TableCell>
                                                                        <asp:Label ID="Label10" runat="server" Text="Tipe Karyawan"></asp:Label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <asp:Label ID="Label11" runat="server" Text=":"></asp:Label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <asp:Label ID="tipekaryawanLb" runat="server"></asp:Label>
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                                <asp:TableRow>
                                                                    <asp:TableCell>
                                                                        <asp:Label ID="Label6" runat="server" Text="Lokasi"></asp:Label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <asp:Label ID="Label9" runat="server" Text=":"></asp:Label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell>
                                                                        <asp:Label ID="lokasiLb" runat="server"></asp:Label>
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                            </asp:Table>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- end informasi karyawan -->

                                        <!-- start informasi pinjaman -->
                                        <div id="infoPinjaman">
                                            <div class="card mt-4">
                                                <div class="card-header-pills bg-success text-white p-1">
                                                    <asp:Table ID="Table6" runat="server" Style="width: 100%;" CssClass="">
                                                        <asp:TableRow>
                                                            <asp:TableCell Style="width: 100%; text-align: left;" CssClass="font-weight-bold">
                                                    <asp:Label runat="server">Informasi Pinjaman</asp:Label>
                                                    <a class="text-white pull-right" data-toggle="collapse" data-target="#collapseTwo" aria-controls="collapseTwo" aria-expanded="true" style="cursor:pointer">
                                                        <i class="fa fa-chevron-up text-white" aria-hidden="true"></i>
                                                        <i class="fa fa-chevron-down text-white" aria-hidden="true"></i>
                                                    </a>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                    </asp:Table>
                                                </div>
                                                <div id="collapseTwo" class="collapse show" aria-labelledby="headingOne" data-parent="#infoPinjaman">
                                                    <div class="card body p-3 rounded">
                                                        <div class="row">
                                                            <div class="col-md-6 col-sm-12 col-xs-12 no-margin no-padding">
                                                                <asp:Table ID="tableKiri" runat="server" CellPadding="5" CellSpacing="0">
                                                                    <asp:TableRow>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="pktLbl" runat="server" Text="ID Transaksi"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="pktColon" runat="server" Text=":"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="pinjamanIDLb" runat="server">TEST</asp:Label>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label1" runat="server" Text="No Perjanjian/Transaksi"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label2" runat="server" Text=":"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="noPinjamanLb" runat="server"></asp:Label>

                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label13" runat="server" Text="Tujuan Pinjaman"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label14" runat="server" Text=":"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="tujuanLb" runat="server"></asp:Label>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label12" runat="server" Text="Alasan Pinjaman"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label15" runat="server" Text=":"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="alasanLb" runat="server"></asp:Label>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label16" runat="server" Text="Jumlah Pinjaman"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label17" runat="server" Text=":"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="amountLb" runat="server"></asp:Label>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label18" runat="server" Text="Paket Pinjaman"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label19" runat="server" Text=":"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="packetLb" runat="server"></asp:Label>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label20" runat="server" Text="Tenor"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label21" runat="server" Text=":"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="tenorLb" runat="server"></asp:Label>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label22" runat="server" Text="Jasa Perbulan"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label23" runat="server" Text=":"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="jasaLb" runat="server"></asp:Label>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                </asp:Table>
                                                            </div>

                                                            <div class="col-md-6 col-sm-12 col-xs-12 no-margin no-padding">
                                                                <asp:Table ID="tableKanan" runat="server" CssClass="" CellPadding="5" CellSpacing="0">
                                                                    <asp:TableRow>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label24" runat="server" Text="Angsuran Pokok"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label25" runat="server" Text=":"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="angsuranPokokLb" runat="server"></asp:Label>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label26" runat="server" Text="Angsuran Jasa"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label27" runat="server" Text=":"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="angsuranJasaLb" runat="server"></asp:Label>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label28" runat="server" Text="Total Angsuran"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label29" runat="server" Text=":"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="totalAngsuranLb" runat="server"></asp:Label>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label30" runat="server" Text="Angsuran ke"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label31" runat="server" Text=":"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="angsurankeLb" runat="server"></asp:Label>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label32" runat="server" Text="Total Angsuran Pokok Terbayar"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label33" runat="server" Text=":"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="angsuranPokokTerbayarLb" runat="server"></asp:Label>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label34" runat="server" Text="Saldo Pokok"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label35" runat="server" Text=":"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="saldoPokokLb" runat="server"></asp:Label>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                    <asp:TableRow>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label36" runat="server" Text="Tanggal Pembayaran Terakhir"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="Label37" runat="server" Text=":"></asp:Label>
                                                                        </asp:TableCell>
                                                                        <asp:TableCell>
                                                                            <asp:Label ID="tanggalPembayaranTerakhirLb" runat="server"></asp:Label>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>
                                                                </asp:Table>
                                                            </div>


                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- end informasi pinjaman -->


                                        <!-- end informasi pinjaman -->


                                        <!-- start table doc -->
                                        <div id="infoDoc" runat="server">
                                            <div class="card mt-4">
                                                <div class="card-header-pills bg-success text-white p-1">
                                                    <asp:Table ID="Table7" runat="server" Style="width: 100%;" CssClass="">
                                                        <asp:TableRow>
                                                            <asp:TableCell Style="width: 45%; text-align: left;" CssClass="font-weight-bold">
                                                    <asp:Label runat="server">Kelengkapan Dokumen</asp:Label>
                                                     <a class="text-white pull-right" data-toggle="collapse" data-target="#collapseThree" aria-controls="collapseThree" aria-expanded="false" style="cursor:pointer">
                                                        <i class="fa fa-chevron-up text-white" aria-hidden="true"></i>
                                                        <i class="fa fa-chevron-down text-white" aria-hidden="true"></i>
                                                    </a>
                                                            </asp:TableCell>
                                                        </asp:TableRow>

                                                    </asp:Table>
                                                </div>
                                                <div id="collapseThree" class="collapse" aria-labelledby="headingOne" data-parent="#infoDoc">
                                                    <div class="card-body pt-3">
                                                        <div id="repeaterDv">
                                                            <asp:Repeater ID="checkDocRepeater" runat="server" OnItemDataBound="checkDocRepeater_ItemDataBound" OnItemCommand="checkDocRepeater_ItemCommand">
                                                                <HeaderTemplate>
                                                                    <table id="Table1">
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <tr class="repeaterRow">
                                                                        <td class='col-md-1 pl-0'>
                                                                            <asp:LinkButton ID="btnDownloadCheck" runat="server" CssClass="px-3 btn btn-outline-primary text-center" CommandName="downloadCheckDoc"><i class="icon-download"></i></asp:LinkButton>
                                                                            <asp:LinkButton ID="btnPreviewCheck" runat="server" CssClass="px-3 btn btn-outline-success text-center" Style="margin-left: 5px;" CommandName="previewCheckDoc"><i class="fa fa-search"></i></asp:LinkButton>

                                                                            <asp:CheckBox ID="checkCB" runat="server" class="checkCB ml-1" type="checkbox"
                                                                                AutoPostBack="false" Enabled="false" />

                                                                            <asp:Label ID="fileNameLbl" runat="server" Style='display: none;'></asp:Label>
                                                                            <asp:Label ID="docNameLbl" runat="server" Text='<%#Eval("docName") %>'></asp:Label>
                                                                            <span class="text-danger">Notes dari admin : </span> <asp:Label ID="lastFeedBackLbl" runat="server" ForeColor="OrangeRed"></asp:Label>
                                                                            <asp:Label ID="docIDLbl" runat="server" Text='<%#Eval("docID") %>' Style='display: none;'></asp:Label>
                                                                            <asp:Label ID="entryIDLbl" runat="server" Text='<%#Eval("entryID") %>' Style='display: none;'></asp:Label>
                                                                            <asp:Label ID="pinjamanIDLbl" runat="server" Text='<%#Eval("pinjamanID") %>' Style='display: none;'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    </table>
                                                                </FooterTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                        <%--<asp:UpdatePanel ID="updatePanelCheckDoc" runat="server" UpdateMode="Conditional"
                                                            ChildrenAsTriggers="false">
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="checkKK" EventName="CheckedChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="checkKTP" EventName="CheckedChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="checkKTPPenjamin" EventName="CheckedChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="checkNameTag" EventName="CheckedChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="checkSJP" EventName="CheckedChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="checkSPPHal1" EventName="CheckedChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="checkSPPHal2" EventName="CheckedChanged" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                                <asp:Table ID="docTable" runat="server" Style="width: 75%; margin-top: 5px;">
                                                                    <asp:TableRow ID="KKRow">
                                                                        <asp:TableCell HorizontalAlign="Left">
                                                                            <asp:CheckBox Enabled="false" ID="checkKK" runat="server" OnCheckedChanged="docCheckBox_OnCheckedChanged"
                                                                                AutoPostBack="true" />
                                                                        </asp:TableCell>
                                                                        <asp:TableCell HorizontalAlign="Left">
                                                                            <asp:Label ID="KKLbl" runat="server" Text="FC Kartu Keluarga "></asp:Label>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>

                                                                    <asp:TableRow ID="KTPRow">
                                                                        <asp:TableCell HorizontalAlign="Left">
                                                                            <asp:CheckBox Enabled="false" ID="checkKTP" runat="server" OnCheckedChanged="docCheckBox_OnCheckedChanged"
                                                                                AutoPostBack="true" />
                                                                        </asp:TableCell>
                                                                        <asp:TableCell HorizontalAlign="Left">
                                                                            <asp:Label ID="KTPLbl" runat="server" Text="FC KTP "></asp:Label>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>

                                                                    <asp:TableRow ID="KTPPenjaminRow">
                                                                        <asp:TableCell HorizontalAlign="Left">
                                                                            <asp:CheckBox Enabled="false" ID="checkKTPPenjamin" runat="server" OnCheckedChanged="docCheckBox_OnCheckedChanged"
                                                                                AutoPostBack="true" />
                                                                        </asp:TableCell>
                                                                        <asp:TableCell HorizontalAlign="Left">
                                                                            <asp:Label ID="KTPPenjaminLbl" runat="server" Text="FC KTP Penjamin "></asp:Label>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>

                                                                    <asp:TableRow ID="NameTagRow">
                                                                        <asp:TableCell HorizontalAlign="Left">
                                                                            <asp:CheckBox Enabled="false" ID="checkNameTag" runat="server" OnCheckedChanged="docCheckBox_OnCheckedChanged"
                                                                                AutoPostBack="true" />
                                                                        </asp:TableCell>
                                                                        <asp:TableCell HorizontalAlign="Left">
                                                                            <asp:Label ID="NameTagLbl" runat="server" Text="FC Name Tag  "></asp:Label>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>

                                                                    <asp:TableRow ID="SJPRow">
                                                                        <asp:TableCell HorizontalAlign="Left">
                                                                            <asp:CheckBox Enabled="false" ID="checkSJP" runat="server" OnCheckedChanged="docCheckBox_OnCheckedChanged"
                                                                                AutoPostBack="true" />
                                                                        </asp:TableCell>
                                                                        <asp:TableCell HorizontalAlign="Left">
                                                                            <asp:Label ID="SJPLbl" runat="server" Text="Surat Jaminan Peminjaman Kopkari  	"></asp:Label>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>

                                                                    <asp:TableRow ID="SPPHal1Row">
                                                                        <asp:TableCell HorizontalAlign="Left">
                                                                            <asp:CheckBox Enabled="false" ID="checkSPPHal1" runat="server" OnCheckedChanged="docCheckBox_OnCheckedChanged"
                                                                                AutoPostBack="true" />
                                                                        </asp:TableCell>
                                                                        <asp:TableCell HorizontalAlign="Left">
                                                                            <asp:Label ID="SPPHal1Lbl" runat="server" Text="Surat Perjanjian Pinjaman Halaman 1  "></asp:Label>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>

                                                                    <asp:TableRow ID="SPPHal2Row">
                                                                        <asp:TableCell HorizontalAlign="Left">
                                                                            <asp:CheckBox Enabled="false" ID="checkSPPHal2" runat="server" OnCheckedChanged="docCheckBox_OnCheckedChanged"
                                                                                AutoPostBack="true" />
                                                                        </asp:TableCell>
                                                                        <asp:TableCell HorizontalAlign="Left">
                                                                            <asp:Label ID="SPPHal2Lbl" runat="server" Text="Surat Perjanjian Pinjaman Halaman 2  "></asp:Label>
                                                                        </asp:TableCell>
                                                                    </asp:TableRow>

                                                                </asp:Table>
                                                                <asp:UpdatePanel ID="updatePanelAction" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:HiddenField ID="hidKKFileName" runat="server" EnableViewState="true" />
                                                                        <asp:HiddenField ID="hidKTPFileName" runat="server" EnableViewState="true" />
                                                                        <asp:HiddenField ID="hidKTPPenjaminFileName" runat="server" EnableViewState="true" />
                                                                        <asp:HiddenField ID="hidNameTagFileName" runat="server" EnableViewState="true" />
                                                                        <asp:HiddenField ID="hidSJPFileName" runat="server" EnableViewState="true" />
                                                                        <asp:HiddenField ID="hidSPPHal1FileName" runat="server" EnableViewState="true" />
                                                                        <asp:HiddenField ID="hidSPPHal2FileName" runat="server" EnableViewState="true" />
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>--%>

                                                        <div class="col-md-12 px-0">
                                                            <asp:LinkButton runat="server" ID="viewPHPBtn" CssClass="btn btn-success mt-3" OnClientClick="viewPHP(); return false;"><i class="fa fa-search"></i> Lihat Perjanjian Hutang Piutang</asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <!-- end table doc -->

                                        <!-- Log status proses peminjaman start -->
                                        <div id="infoLog">
                                            <div class="card mt-4">
                                                <div class="card-header-pills text-white bg-success p-1">
                                                    <asp:Table ID="Table2" runat="server" Style="width: 100%;" CssClass="">
                                                        <asp:TableRow>
                                                            <asp:TableCell Style="width: 100%; text-align: left;" CssClass="font-weight-bold">
                                                                <asp:Label runat="server">Log Status Proses Peminjaman</asp:Label>
                                                                <asp:Literal ID="LitErrHistory" runat="server"></asp:Literal>
                                                                <a class="text-white pull-right" data-toggle="collapse" data-target="#collapseFour" aria-controls="collapseFour" aria-expanded="false" style="cursor: pointer">
                                                                    <i class="fa fa-chevron-up text-white" aria-hidden="true"></i>
                                                                    <i class="fa fa-chevron-down text-white" aria-hidden="true"></i>
                                                                </a>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                    </asp:Table>
                                                </div>
                                                <div id="collapseFour" class="collapse" aria-labelledby="headingOne" data-parent="#infoLog">
                                                    <div class="card-body rounded p-3">

                                                        <asp:UpdatePanel ID="updatePanelNotes" runat="server" UpdateMode="Conditional"
                                                            ChildrenAsTriggers="true">
                                                            <ContentTemplate>

                                                                <div class="datagrid" id="Div3">
                                                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                                                    <asp:HiddenField ID="HiddenField2" runat="server" />
                                                                    <div style="width: auto;">
                                                                        <asp:GridView ID="gvNotes" runat="server" EnableModelValidation="True" AutoGenerateColumns="False"
                                                                            CssClass="table table-hover table-bordered" OnRowDataBound="gvNotes_RowDataBound">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Tanggal" SortExpression="entryDate" HeaderStyle-ForeColor="White">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="txnDateLogStatusLb" runat="server" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField DataField="statusByLog" HeaderText="Status" SortExpression="statusByLog" HeaderStyle-ForeColor="White" />
                                                                                <asp:BoundField DataField="lastOperator" HeaderText="Operator" SortExpression="lastOperator" HeaderStyle-ForeColor="White" />
                                                                                <asp:BoundField DataField="description" HeaderText="Notes" SortExpression="description" HeaderStyle-ForeColor="White" />
                                                                            </Columns>
                                                                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="4" FirstPageText="First"
                                                                                LastPageText="Last" />
                                                                            <RowStyle CssClass="td" />
                                                                            <SelectedRowStyle CssClass="thspecalt" />
                                                                            <AlternatingRowStyle CssClass="tdalt" />
                                                                            <HeaderStyle CssClass="th thead-light text-center" />
                                                                        </asp:GridView>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>

                                                    </div>

                                                </div>
                                            </div>
                                        </div>

                                        <!-- Log status proses peminjaman stop -->

                                        <!-- history peminjaman -->
                                        <div id="infoHis" runat="server" class="invisible">
                                            <div class="card mt-4">
                                                <div class="card-header-pills text-white bg-success p-1">

                                                    <asp:Table ID="Table4" runat="server" Style="width: 100%;" CssClass="">
                                                        <asp:TableRow>
                                                            <asp:TableCell Style="width: 100%; text-align: left;" CssClass="font-weight-bold">
                                                    <asp:Label runat="server">History Pinjaman</asp:Label>
                                                     <a class="text-white pull-right" data-toggle="collapse" data-target="#collapseFive" aria-controls="collapseFive" aria-expanded="false" style="cursor:pointer">
                                                        <i class="fa fa-chevron-up text-white" aria-hidden="true"></i>
                                                        <i class="fa fa-chevron-down text-white" aria-hidden="true"></i>
                                                    </a>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                    </asp:Table>

                                                </div>
                                                <div id="collapseFive" class="collapse" aria-labelledby="headingOne" data-parent="#infoHis">
                                                    <div class="card-body rounded p-3">

                                                        <asp:UpdatePanel ID="updatePanelGvAnggota" runat="server" UpdateMode="Conditional"
                                                            ChildrenAsTriggers="true">
                                                            <ContentTemplate>
                                                                <div class="datagrid " id="Div1">
                                                                    <asp:HiddenField ID="hdSortDirAng" runat="server" />
                                                                    <asp:HiddenField ID="hdSortExAng" runat="server" />
                                                                    <div style="overflow: auto; width: auto;">
                                                                        <asp:GridView ID="gvAnggota" runat="server" EnableModelValidation="True" AutoGenerateColumns="False"
                                                                            AllowPaging="True" PageSize="5" AllowSorting="true" OnPageIndexChanging="gvAnggota_PageIndexChanging"
                                                                            OnRowDataBound="gvAnggota_RowDataBound" OnSorting="gvAnggota_Sorting" CssClass="table table-hover table-bordered" HeaderStyle-ForeColor="White">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="No." HeaderStyle-ForeColor="White">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label runat="server" Text='<%# (Container.DataItemIndex)+1 %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField DataField="pinjamanID" HeaderText="ID Transaksi " SortExpression="pinjamanID" />
                                                                                <asp:BoundField DataField="txnDate" HeaderText="Tanggal Aplikasi Pinjaman" SortExpression="txnDate"
                                                                                    DataFormatString="{0:dd-MM-yyyy}" />
                                                                                <asp:BoundField DataField="packetName" HeaderText="Paket Pinjaman" SortExpression="packetName" />
                                                                                <asp:BoundField DataField="amount" HeaderText="Jumlah Pinjaman" SortExpression="amount"
                                                                                    ItemStyle-CssClass="text-right" DataFormatString="{0:###,###,###}" />
                                                                                <asp:TemplateField HeaderText="Tenor" SortExpression="Tenor">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="tenorGridLb" runat="server" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField DataField="txnDate" HeaderText="Tanggal Pembayaran Terakhir" SortExpression="txnDate"
                                                                                    DataFormatString="{0:dd-MM-yyyy}" />
                                                                                <asp:TemplateField HeaderText="Status" HeaderStyle-ForeColor="White" SortExpression="statusByAnggota">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="txnStatusGVAnggotaLbl" runat="server" Text="Status XXX"
                                                                                            CommandName="txnStatus" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="4" FirstPageText="First"
                                                                                LastPageText="Last" />
                                                                            <RowStyle CssClass="td" />
                                                                            <SelectedRowStyle CssClass="thspecalt" />
                                                                            <AlternatingRowStyle CssClass="tdalt" />
                                                                            <HeaderStyle CssClass="th thead-light text-center" />
                                                                        </asp:GridView>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                        <!-- end history peminjaman -->

                                    </div>
                                    <div class="modal-footer">
                                        <asp:LinkButton runat="server" ID="closeBtn" CssClass="btn btn-inverse-dark no-margin-bottom"
                                            OnClick="closeBtn_click"> <i class="fa fa-arrow-circle-left"></i>Kembali</asp:LinkButton>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <!-- ini update panel buat popupnya -->
                        </div>
                    </div>
                </div>
                <!-- akhir modal dialog edit data (ini popup) -->

                <!-- modal dialog cetak surat (ini popup) -->
                <div id="modelDialogPrintLetter" class="modal fade modal-dialog-add" role="dialog" data-backdrop="static" data-keyboard="false" aria-hidden="true"
                    style="overflow-y: auto; width: 50%; top: 25%; left: 25%">
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content">
                            <!-- ini update panel buat popupnya -->
                            <asp:UpdatePanel ID="updatePanelPrintLetter" runat="server" UpdateMode="Conditional"
                                ChildrenAsTriggers="false">
                                <Triggers>
                                </Triggers>
                                <ContentTemplate>
                                    <div class="modal-header bg-success text-white">
                                        <asp:Label runat="server">Cetak Surat Perjanjian</asp:Label>
                                    </div>
                                    <div class="modal-body bg-white" id="modal-body">
                                        <asp:LinkButton ID="btnPrintFake" runat="server" Text="Cetak" CssClass="btn btn-primary btn-block " OnClientClick="print()"></asp:LinkButton>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button runat="server" ID="printCloseBtn" CssClass="btn btn-primary no-margin-bottom"
                                            Text="Close" OnClick="printCloseBtn_onClick" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <!-- ini update panel buat popupnya -->
                        </div>
                    </div>
                </div>
                <!-- akhir modal dialog cetak surat (ini popup) -->

                <!-- modal notes (ini popup) -->
                <div id="modelDialogShowNotes" class="modal fade modal-dialog-add" role="dialog" data-backdrop="static" data-keyboard="false" aria-hidden="true"
                    style="overflow-y: auto; width: 50%; top: 25%; left: 25%">
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content">
                            <!-- ini update panel buat popupnya -->
                            <asp:UpdatePanel ID="updatePanelShowNotes" runat="server" UpdateMode="Conditional"
                                ChildrenAsTriggers="false">
                                <Triggers>
                                </Triggers>
                                <ContentTemplate>
                                    <div class="modal-header bg-success text-white">
                                        <asp:Label runat="server">Notes Penolakan Dokumen</asp:Label>
                                        <a type="button" class="close text-white" data-dismiss="modal">
                                            <i class="fa fa-times text-white" aria-hidden="true"></i>
                                        </a>
                                    </div>
                                    <div class="modal-body bg-white" id="modal-body">
                                        <span>Berikut Informasi Penolakan Dokumen</span>
                                        <asp:TextBox Enabled="false" CssClass="form-control bg-white" runat="server" Rows="10" ID="notesTB" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:LinkButton runat="server" ID="snCloseBtn" CssClass="btn btn-outline-dark no-margin-bottom"
                                            Text="Close" OnClick="showNotesCloseBtn_Click"><i class="fa fa-times-circle"></i> Close</asp:LinkButton>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <!-- ini update panel buat popupnya -->
                        </div>
                    </div>
                </div>
                <!-- akhir modal dialog notes (ini popup) -->



                <!------------------------------------------------>




                <div id="Div2" class="uploadArea" runat="server" style="display: none">
                    <%--<asp:Button runat="server" ID="btnUploadAttach"  />--%>

                    <asp:Label runat="server" ID="ajaxFileUpThrobber" Style="display: none;">
                                 <img align="absmiddle" alt="" src="~/Assets/images/ajax-loader.gif"/></asp:Label>

                    <act:AjaxFileUpload ID="ajaxFileUpAttach" runat="server" padding-bottom="4" padding-left="2"
                        padding-right="1" padding-top="4" ThrobberID="ajaxFileUpThrobber" OnClientUploadStart="ajaxFileUp_ClientUploadStart" OnClientUploadComplete="ajaxFileUp_onClientUploadComplete"
                        OnUploadComplete="ajaxFileUpAttach_OnUploadComplete" AutoStartUpload="true"
                        AllowedFileTypes="pdf,jpg,jpeg,png" MaximumNumberOfFiles="10" MaxFileSize="5000"
                        ClearFileListAfterUpload="true" />
                </div>
            </div>
            </div>
             
        </ContentTemplate>
    </asp:UpdatePanel>

    <!-- modal dialog upload dokumen (ini popup) -->
    <div id="modelDialogUploadDoc" class="modal fade modal-dialog-add" role="dialog" data-backdrop="static" data-keyboard="false" aria-hidden="true"
        style="overflow-y: auto;">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <!-- ini update panel buat popupnya -->
                <asp:UpdatePanel ID="updatePanelUploadDoc" runat="server" UpdateMode="Conditional"
                    ChildrenAsTriggers="false">
                    <Triggers>
                        <%--<asp:PostBackTrigger ControlID="btnDownloadHide" />--%>
                    </Triggers>
                    <ContentTemplate>
                        <div class="modal-header bg-success text-white">
                            <asp:Label ID="Label39" runat="server">Upload Dokumen Persyaratan Pinjaman</asp:Label>
                            <a type="button" class="close text-white" data-dismiss="modal">
                                <i class="fa fa-times text-white" aria-hidden="true"></i>
                            </a>
                        </div>


                        <div class="modal-footer">
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!-- ini update panel buat popupnya -->
            </div>
        </div>
    </div>
    <!-- akhir modal dialog upload dokumen (ini popup) -->



    <!-- modal dialog process dokumen (ini popup) -->
    <div id="modelDialogProcessDoc" class="modal fade modal-dialog-add" role="dialog" data-backdrop="static" data-keyboard="false" aria-hidden="true"
        style="overflow-y: auto;">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <!-- ini update panel buat popupnya -->
                <asp:UpdatePanel ID="updatePanelProcessDoc" runat="server" UpdateMode="Conditional"
                    ChildrenAsTriggers="false">
                    <Triggers>
                        <%--<asp:PostBackTrigger ControlID="btnDownloadHide" />--%>
                    </Triggers>
                    <ContentTemplate>
                        <div class="modal-header bg-success text-white">
                            <asp:Label ID="headerProccessLbl" runat="server">Kelengkapan Dokumen Pinjaman</asp:Label>
                            <a type="button" class="close text-white" data-dismiss="modal">
                                <i class="fa fa-times text-white" aria-hidden="true"></i>
                            </a>
                        </div>

                        <div id="perjanjianDIV" runat="server" class="modal-content col-md-11 col-sm-12 col-xs-12">
                            <table class="col-md-12 col-sm-12 col-xs-12 px-4 mx-5">

                                <tr>
                                    <td class="text-center">
                                        <b><u>PERJANJIAN HUTANG PIUTANG</u></b>
                                    </td>

                                </tr>

                                <tr>

                                    <td class="text-center pb-3">
                                        <asp:Literal runat="server" ID="noPinjamanLit"></asp:Literal>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="pb-2 ">Perjanjian ini dibuat di  Jakarta pada hari ini,
                                            <asp:Literal ID="dayLit" runat="server"></asp:Literal>
                                        tanggal
                                            <asp:Literal ID="dateLit" runat="server"></asp:Literal>
                                        bulan
                                            <asp:Literal ID="monthLit" runat="server"></asp:Literal>
                                        tahun
                                            <asp:Literal ID="yearLit" runat="server"></asp:Literal>, oleh dan antara :
                                    </td>
                                </tr>

                                <tr>
                                    <tr>
                                        <td>
                                            <div class="row">
                                                <div class="col-md-1 col-fix">
                                                    <b>Nama</b>
                                                </div>
                                                <div class="col-md-10">
                                                    Rudy Prakoso
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="row">
                                                <div class="col-md-1 col-fix">
                                                    <b>Jabatan</b>
                                                </div>
                                                <div class="col-md-10">
                                                    Ketua Koperasi
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </tr>

                                <tr>
                                    <td class="pt-2 pb-2" style="text-align: justify">Dalam hal ini bertindak untuk dan atas nama <b>Koperasi Karyawan Kawan Lama Mandiri (KOPKARI)</b> yang berkedudukan di Jakarta dengan alamat Jl. Puri Kencana No.1, Kembangan yang selanjutnya disebut sebagai <b>Pihak Pertama</b>.
                                    </td>
                                </tr>

                                <tr>
                                    <tr>
                                        <td>
                                            <div class="row">
                                                <div class="col-md-1 col-fix">
                                                    <b>Nama</b>
                                                </div>
                                                <div class="col-md-10">
                                                    <asp:Literal runat="server" ID="anggotaNameLit"></asp:Literal>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="row">
                                                <div class="col-fix col-md-1">
                                                    <b>No. Anggota</b>
                                                </div>
                                                <div class="col-md-10">
                                                    <asp:Literal runat="server" ID="userIDLit"></asp:Literal>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="row">
                                                <div class="col-md-1 col-fix">
                                                    <b>No. KTP</b>
                                                </div>
                                                <div class="col-md-10">
                                                    <asp:Literal runat="server" ID="KTPLit"></asp:Literal>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <div class="row">
                                                <div class="col-md-1 col-fix">
                                                    <b>Alamat</b>
                                                </div>
                                                <div class="col-md-10">
                                                    <asp:Literal runat="server" ID="alamatLit"></asp:Literal>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </tr>

                                <tr>
                                    <td class="pt-2 pb-2">Dalam hal ini bertindak untuk dan atas namanya sendiri yang selanjutnya disebut sebagai <b>Pihak Kedua</b>.
                                    </td>
                                </tr>

                                <tr>
                                    <td class="pt-2 pb-2">Secara bersama - sama kedua disebut : <b>"Para Pihak"</b> dan secara sendiri disebut <b>Pihak</b>, menerangkan terlebih dahulu hal-hal sebagai berikut :
                                    </td>
                                </tr>

                                <tr>
                                    <td>

                                        <ul>
                                            <li style="list-style-type: circle; text-align: justify">Bahwa <b>Pihak Pertama</b> adalah Koperasi yang bergerak di bidang Simpan Pinjam
                                            </li>
                                            <li style="list-style-type: circle">Bahwa <b>Pihak Kedua</b>, adalah anggota koperasi <b>Pihak Pertama yang sekaligus merupakan karyawan
                                                    <asp:Literal runat="server" ID="compNameLit"></asp:Literal></b></li>

                                            <li style="list-style-type: circle; text-align: justify">
                                                <b>Pihak Kedua</b> dengan ini menyatakan telah mengajukan permohonan pinjaman dana tunai kepada <b>Pihak Pertama</b>
                                            </li>

                                            <li style="list-style-type: circle; text-align: justify">
                                                <b>Pihak Pertama</b> telah setuju untuk memberikan pinjaman dana tunai kepada <b>Pihak Kedua</b>

                                            </li>
                                        </ul>
                                        <hr />
                                    </td>

                                </tr>

                                <tr>
                                    <td class="pt-2 pb-2">Sehubungan dengan hal tersebut di atas, <b>Para Pihak</b> sepakat untuk mengikatkan diri dalam <b>Perjanjian Hutang-Piutang</b>, selanjutnya disebut <b>“Perjanjian”</b>, dengan syarat-syarat dan ketentuan-ketentuan sebagai berikut:
                                    </td>
                                </tr>

                                <%--Pasal 1--%>

                                <div class="col-md-12 row">
                                    <div class="col-md-10">
                                        <tr>
                                            <td class="pb-2">
                                                <div class="row">
                                                    <div class="col-md-12 font-weight-bold text-center">Pasal 1</div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12 font-weight-bold text-center">JUMLAH PINJAMAN DAN JANGKA WAKTU</div>
                                                </div>

                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <ul>
                                                    <li style="list-style-type: decimal; text-align: justify">
                                                        <b>Pihak Kedua</b> dengan ini menyatakan telah mengajukan pinjaman dana tunai dari <b>Pihak Pertama</b> sebesar <b>Rp.
                                                        <asp:Literal runat="server" ID="amountAngkaLit"></asp:Literal></b>  <b>( Terbilang :
                                                            <asp:Literal runat="server" ID="amountTerbilangLit"></asp:Literal>
                                                            Rupiah )</b> untuk selanjutnya disebut sebagai <b>“Hutang”</b>
                                                    </li>

                                                    <li style="list-style-type: decimal; text-align: justify">
                                                        <b>Pihak Pertama</b> telah setuju untuk memberikan pinjaman kepada <b>Pihak Kedua</b> dengan mentransfer secara cash dan sekaligus ke rekening <b>Pihak Kedua</b>.
                                                    </li>

                                                    <li style="list-style-type: decimal; text-align: justify">
                                                        <b>Para Pihak</b> sepakat akan menyelesaikan Hutang selama
                                                    <asp:Literal ID="tenorLit" runat="server"></asp:Literal>
                                                        (<asp:Literal runat="server" ID="tenorTerbilangLit"></asp:Literal>) bulan yang dilakukan melalui pemotongan gaji oleh Payroll <b>Pihak Kedua</b>, untuk selanjutnya disebut “Jangka Waktu Perjanjian”. 
                                                    </li>
                                                </ul>
                                            </td>
                                        </tr>
                                    </div>

                                    <div class="col-md-2">
                                        <tr>
                                            <td class="pull-right font-weight-bold text-success">
                                                <asp:CheckBox ID="pasal1CheckBox" runat="server" />
                                                <span>Saya mengetahui dan menyetujui pasal 1 ini</span>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <hr />
                                            </td>
                                        </tr>


                                    </div>
                                </div>


                                <%--PASAL 2--%>

                                <div class="col-md-12 row">
                                    <div class="col-md-10">

                                        <tr>
                                            <td>
                                                <div class="row">
                                                    <div class="col-md-12 font-weight-bold text-center">Pasal 2</div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12 font-weight-bold text-center">BESARAN ANGSURAN DAN CARA PEMBAYARAN</div>
                                                </div>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <ul>
                                                    <li style="list-style-type: decimal; text-align: justify">
                                                        <b>Pihak Kedua</b> wajib melunasi kembali Hutang tersebut kepada <b>Pihak Pertama</b>  dengan cara mencicil setiap bulannya sebagaimana diatur dalam Pasal 1 ayat 3 Perjanjian ini dengan cara pemotongan upah oleh pihak Payroll <b>Pihak Kedua</b> berdasarkan Surat Kuasa yang telah dibuat oleh <b>Pihak Kedua</b>
                                                    </li>

                                                    <li style="list-style-type: decimal; text-align: justify">Atas peminjaman tersebut <b>Pihak Kedua</b> akan dikenakan bunga tetap sebesar
                                                    <asp:Literal runat="server" ID="bungaLit"></asp:Literal>%
                                                (
                                                    <asp:Literal ID="bungaTerbilangLit" runat="server"></asp:Literal>
                                                        persen ) setiap bulannya
                                                    </li>

                                                    <li style="list-style-type: decimal; text-align: justify">Sehingga besaran cicilan setiap bulannya dalam penyelesaian Hutang ini adalah sebesar Rp.
                                                    <asp:Literal runat="server" ID="cilPokLit"> </asp:Literal>
                                                        (terbilang
                                                    <asp:Literal runat="server" ID="cilPokTerbilangLit"></asp:Literal>), untuk selanjutnya disebut <b>“Angsuran”</b>.
                                                    </li>
                                                </ul>
                                            </td>
                                        </tr>
                                    </div>
                                    <div class="col-md-2">
                                        <tr>
                                            <td class="pull-right font-weight-bold text-success">
                                                <asp:CheckBox ID="pasal2CheckBox" runat="server" />
                                                <span>Saya mengetahui dan menyetujui pasal 2 ini</span>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <hr />
                                            </td>
                                        </tr>
                                    </div>
                                </div>

                                <%--PASAL 3--%>

                                <div class="col-md-12 row">
                                    <div class="col-md-10">
                                        <tr>
                                            <td>
                                                <div class="row">
                                                    <div class="col-md-12 font-weight-bold text-center">Pasal 3</div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12 font-weight-bold text-center">HAL-HAL YANG MENGAKIBATKAN BERAKHIRNYA PERJANJIAN</div>
                                                </div>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <ul>
                                                    <li style="list-style-type: decimal; text-align: justify">Perjanjian akan berakhir dengan sendirinya apabila Jangka Waktu Perjanjian berakhir dan Hutang telah dinyatakan lunas oleh <b>Pihak Pertama</b>.
                                                    </li>


                                                    <li style="list-style-type: decimal; text-align: justify">Apabila terjadi pemutusan hubungan kerja <b>Pihak Kedua</b> oleh karena sebab apapun, maka <b>Pihak Kedua</b> wajib melunasi Hutang dan bunga berjalan kepada <b>Pihak Pertama</b> selambat-lambatnya 7 (tujuh) hari sebelum tanggal efektif berakhirnya hubungan kerja <b>Pihak Kedua</b>
                                                    </li>

                                                    <li style="list-style-type: decimal; text-align: justify">Pelunasan terhadap sisa kurang sebagaimana dimaksud ayat 2, maka <b>Pihak Kedua</b> setuju untuk diperhitungkan dengan hak-haknya yang ada di <b>Pihak Pertama</b> seperti Simpanan Wajib, Simpanan Sukarela dan Simpanan Berjangka (jika ada).
                                                    </li>

                                                    <li style="list-style-type: decimal; text-align: justify">Apabila hak-hak tersebut pada Ayat 3 diatas tidak mencukupi untuk melunasi kewajiban, maka <b>Pihak Kedua</b> sepakat untuk diperhitungkan dari Upah terakhir, Uang Pisah, Uang Pesangon, Uang Penghargaan Masa Kerja dan/atau Uang Penggantian Hak <b>Pihak Kedua</b> yang akan diterimanya karena pemutusan hubungan kerja
                                                    </li>

                                                    <li style="list-style-type: decimal; text-align: justify">Apabila hak-haknya sebagaimana dimaksud ayat 3 dan ayat 4, ternyata belum mencukupi, maka <b>Pihak Kedua</b> diwajibkan menyelesaikan kekurangan kewajiban tersebut selambat-lambatnya pada hari terakhir <b>Pihak Kedua</b> bekerja
                                                    </li>

                                                </ul>
                                            </td>
                                        </tr>

                                    </div>
                                    <div class="col-md-2">
                                        <tr>
                                            <td class="pull-right font-weight-bold text-success">
                                                <asp:CheckBox ID="pasal3CheckBox" runat="server" />
                                                <span>Saya mengetahui dan menyetujui pasal 3 ini</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <hr />
                                            </td>
                                        </tr>
                                    </div>
                                </div>




                                <%--PASAL 4--%>

                                <div class="col-md-12 row">
                                    <div class="col-md-10">
                                        <tr>
                                            <td>
                                                <div class="row">
                                                    <div class="col-md-12 font-weight-bold text-center">Pasal 4</div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12 font-weight-bold text-center">PENYELESAIAN PERSELISIHAN</div>
                                                </div>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <ul>
                                                    <li style="list-style-type: decimal; text-align: justify">Apabila ada hal-hal yang tidak atau belum diatur dalam Perjanjian ini, dan juga jika terjadi perbedaan penafsiran atas seluruh atau sebagian dari Perjanjian ini, maka kedua belah pihak sepakat untuk menyelesaikannya secara musyawarah untuk mufakat.
                                                    </li>

                                                    <li style="list-style-type: decimal; text-align: justify">Jika penyelesaian secara musyawarah untuk mufakat tidak tercapai, maka perselisihan tersebut akan diselesaikan sesuai hukum yang berlaku di Indonesia, dan oleh karena itu kedua belah pihak memilih domisili hukum di Pengadilan Negeri Jakarta Barat sesuai domisili hukum Pihak Pertama. 
                                                    </li>
                                                </ul>
                                            </td>
                                        </tr>

                                    </div>

                                    <div class="col-md-2">
                                        <tr>
                                            <td class="pull-right font-weight-bold text-success">
                                                <asp:CheckBox ID="pasal4CheckBox" runat="server" />
                                                <span>Saya mengetahui dan menyetujui pasal 4 ini</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <hr />
                                            </td>
                                        </tr>
                                    </div>
                                </div>


                                <div class="col-md-12 px-4 mx-5 row">
                                    <tr>
                                        <td class="pb-3">Demikian Perjanjian ini dibuat dan ditandatangani oleh para pihak secara elektronik.
                                        </td>
                                    </tr>
                                </div>


                            </table>
                            <table class="col-md-12 px-4 mx-5">
                                <div class="">

                                    <tr>
                                        <div class="row">
                                            <td class="text-center pb-lg-5 font-weight-bold">Pihak Pertama,</td>
                                        </div>

                                        <div class="row">
                                            <td class="text-center pb-lg-5 font-weight-bold">Pihak Kedua,</td>
                                        </div>

                                    </tr>


                                    <tr>
                                        <div class="row">
                                            <td class="text-center">Rudy Prakoso</td>
                                        </div>
                                        <div class="row">
                                            <td class="text-center">
                                                <asp:Literal runat="server" ID="anggotaNameLit2"></asp:Literal>
                                            </td>
                                        </div>
                                    </tr>

                                    <tr>
                                        <td class="col-md-3">
                                            <hr />
                                        </td>
                                        <td class="col-md-3">
                                            <hr />
                                        </td>
                                    </tr>

                                    <tr>
                                        <div class="row">
                                            <td class="text-center">
                                                <b>Ketua KOPKARI</b>
                                            </td>
                                        </div>
                                        <div class="row">
                                            <td class="text-center">
                                                <b>Anggota</b>
                                            </td>

                                        </div>
                                    </tr>

                                    <tr>
                                        <div class="row">
                                            <td class="text-left px-3"></td>

                                        </div>
                                        <div class="row">
                                            <td class="text-left px-3">
                                                <asp:Literal runat="server" ID="tglPihak2Lit" />
                                            </td>

                                        </div>
                                    </tr>

                                </div>
                            </table>

                        </div>

                        <div id="uploadDIV" runat="server" class="modal-body bg-white">
                            <asp:HiddenField ID="hidDocID" runat="server" />
                            <asp:HiddenField ID="hidFileName" runat="server" />
                            <asp:HiddenField ID="hidFilIsEdit" runat="server" />
                            <asp:HiddenField ID="hidFilIsUploaded" runat="server" />
                            <asp:HiddenField ID="hidIsSubmit" runat="server" />
                            <asp:HiddenField ID="hidAttachList" runat="server" />
                            <asp:HiddenField ID="hidClientTextBox" runat="server" />
                            <asp:HiddenField ID="hidFileType" runat="server" />
                            <div>
                                <asp:Repeater ID="uploadDocRepeater" runat="server" OnItemCommand="uploadDocRepeater_ItemCommand" OnItemDataBound="uploadDocRepeater_ItemDataBound">
                                    <HeaderTemplate>
                                        <table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <asp:Label ID="pinjamanIDLblDoc" runat="server" Text='<%#Eval("pinjamanID") %>' Style='display: none;'></asp:Label>
                                            <asp:Label ID="entryIDLbl" runat="server" Text='<%#Eval("entryID") %>' Style='display: none;'></asp:Label>
                                            <asp:Label ID="docIDLbl" runat="server" Text='<%#Eval("docID") %>' Style='display: none;'></asp:Label>
                                            <asp:TextBox ID="txtIsEdit" runat="server" Enabled="false" Style="display: none;"></asp:TextBox>
                                            <asp:TextBox ID="txtIsUploaded" runat="server" Enabled="false" Style="display: none;"></asp:TextBox>
                                            <asp:TextBox ID="txtIsApproved" runat="server" Enabled="false" Style="display: none;"></asp:TextBox>

                                            <td class="col-md-2 px-0">
                                                <asp:Label runat="server" CssClass="mr-2" Text='<%# (Container.ItemIndex)+1 %>'></asp:Label>
                                                <asp:Label ID="docNameLbl" runat="server" Text='<%#Eval("docName") %>' CssClass="text-left"></asp:Label>
                                            </td>

                                        </tr>


                                        <tr>
                                            <td class="col-md-4 px-0">
                                                <asp:TextBox ID="txtKet" runat="server" CssClass=" form-control bg-white col-12 txtKet"></asp:TextBox>
                                            </td>

                                            <td class="col-md-6 px-1">
                                                <asp:TextBox ID="txtFileName" runat="server" Enabled="false" CssClass="form-control bg-white col-12"></asp:TextBox>
                                            </td>

                                            <td class="col-md-6 px-1" id="tdUpload" runat="server">
                                                <asp:LinkButton ID="btnUpload" runat="server" Text="Pilih File" CssClass="btn btn-outline-success col-12"><i class="fa fa-cloud-upload"></i>Pilih File</asp:LinkButton>
                                            </td>


                                            <td class="col-md-6 px-1">
                                                <asp:LinkButton ID="btnDownload" runat="server" CssClass="btn btn-outline-primary" CommandName="downloadDoc"><i class="icon-download"></i>Download</asp:LinkButton>
                                            </td>

                                            <td class="col-md-6 px-1">
                                                <asp:LinkButton ID="btnPreview" runat="server" CssClass="btn btn-outline-primary " CommandName="previewDoc"><i class="fa fa-search"></i>Preview</asp:LinkButton>

                                            </td>

                                            <td class="col-md-6 px-1" style="display: none;">
                                                <asp:LinkButton ID="notesBtn" OnClick="notesBtn_click" runat="server" CssClass="btn btn-outline-danger text-center" Style="margin-left: 5px;" CommandName="notesBtn"><i class="fa fa-exclamation"></i>Notes</asp:LinkButton>
                                            </td>


                                        </tr>
                                        <tr>
                                            <td class="col-md-6 px-1 pb-3">
                                                <asp:Label class="text-danger" runat="server" ID="notesSpan"> Notes dari admin : </asp:Label>
                                                <asp:Label ID="notesLbl" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <asp:UpdatePanel ID="updatePanelUploadLit" runat="server" UpdateMode="Conditional"
                                ChildrenAsTriggers="false">
                                <ContentTemplate>
                                    <div class="row" style="text-align: center;">
                                        <asp:Literal ID="uploadDocLit" runat="server" Visible="false" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <asp:LinkButton runat="server" ID="sppDownloadBtn" CssClass="btn btn-inverse-warning no-mn-bottom"
                                OnClick="sppDownloadBtn_click"><i class="fa fa-download"></i>Download Surat Pernyataan Penjamin</asp:LinkButton>

                        </div>

                        <div class="modal-footer">
                            <asp:LinkButton runat="server" ID="processDocCloseBtn" CssClass="btn btn-secondary no-mn-bottom"
                                OnClick="processDocCloseBtn_click"><i class="fa fa-times-circle"></i>Close</asp:LinkButton>

                            <asp:LinkButton runat="server" ID="backBtn" CssClass="btn btn-warning no-mn-bottom"
                                OnClick="backBtn_click"><i class="fa fa-arrow-circle-left"></i>Kembali</asp:LinkButton>

                            <asp:LinkButton runat="server" OnClientClick="checkScript(); return false;" ID="nextBtn" CssClass="btn btn-success no-mn-bottom" Text="">Selanjutnya <i class="fa fa-arrow-circle-right"></i></asp:LinkButton>

                            <asp:LinkButton runat="server" ID="fakeUploadDocSaveBtn" CssClass="btn btn-inverse-primary no-mn-bottom"
                                Text="" OnClientClick="saveScript(); return false;"><i class="fa fa-save"></i>Simpan Sebagian</asp:LinkButton>

                            <asp:LinkButton runat="server" ID="fakeUploadDocSubmitBtn" CssClass="btn btn-inverse-success no-mn-bottom"
                                OnClientClick="submitScript(); return false;"><i class="fa fa-save"></i>Submit</asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!-- ini update panel buat popupnya -->
            </div>
        </div>
    </div>
    <!-- akhir modal dialog process dokumen (ini popup) -->

    <!-- modal dialog view php (ini popup) -->
    <div id="modelDialogViewDoc" class="modal fade modal-dialog-add" role="dialog" data-backdrop="static" data-keyboard="false" aria-hidden="true"
        style="overflow-y: auto;">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <!-- ini update panel buat popupnya -->
                <asp:UpdatePanel ID="updatePanelViewDoc" runat="server" UpdateMode="Conditional"
                    ChildrenAsTriggers="false">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="sppDownloadBtn" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="modal-header bg-success text-white">
                            <asp:Label ID="Label41" runat="server">Kelengkapan Dokumen Pinjaman</asp:Label>
                            <a type="button" class="close text-white" data-dismiss="modal">
                                <i class="fa fa-times text-white" aria-hidden="true"></i>
                            </a>
                        </div>

                        <div id="viewDocDiv" runat="server" class="modal-content col-md-11 col-sm-12 col-xs-12">
                            <table class="col-md-12 col-sm-12 col-xs-12 px-4 mx-5">

                                <tr>
                                    <td class="text-center">
                                        <b><u>PERJANJIAN HUTANG PIUTANG</u></b>
                                    </td>

                                </tr>

                                <tr>

                                    <td class="text-center pb-3">
                                        <asp:Literal runat="server" ID="noPinjamanViewDocLit"></asp:Literal>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="pb-2 ">Perjanjian ini dibuat di  Jakarta pada hari ini,
                                            <asp:Literal ID="dayViewDocLit" runat="server"></asp:Literal>
                                        tanggal
                                            <asp:Literal ID="dateViewDocLit" runat="server"></asp:Literal>
                                        bulan
                                            <asp:Literal ID="monthViewDocLit" runat="server"></asp:Literal>
                                        tahun
                                            <asp:Literal ID="yearViewDocLit" runat="server"></asp:Literal>, oleh dan antara :
                                    </td>
                                </tr>

                                <tr>
                                    <tr>
                                        <td>
                                            <div class="row">
                                                <div class="col-md-1 col-fix">
                                                    <b>Nama</b>
                                                </div>
                                                <div class="col-md-10">
                                                    Rudy Prakoso
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="row">
                                                <div class="col-md-1 col-fix">
                                                    <b>Jabatan</b>
                                                </div>
                                                <div class="col-md-10">
                                                    Ketua Koperasi
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </tr>

                                <tr>
                                    <td class="pt-2 pb-2" style="text-align: justify">Dalam hal ini bertindak untuk dan atas nama <b>Koperasi Karyawan Kawan Lama Mandiri (KOPKARI)</b> yang berkedudukan di Jakarta dengan alamat Jl. Puri Kencana No.1, Kembangan yang selanjutnya disebut sebagai <b>Pihak Pertama</b>.
                                    </td>
                                </tr>

                                <tr>
                                    <tr>
                                        <td>
                                            <div class="row">
                                                <div class="col-md-1 col-fix">
                                                    <b>Nama</b>
                                                </div>
                                                <div class="col-md-10">
                                                    <asp:Literal runat="server" ID="anggotaNameViewDocLit"></asp:Literal>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="row">
                                                <div class="col-fix col-md-1">
                                                    <b>No. Anggota</b>
                                                </div>
                                                <div class="col-md-10">
                                                    <asp:Literal runat="server" ID="userIDViewDocLit"></asp:Literal>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="row">
                                                <div class="col-md-1 col-fix">
                                                    <b>No. KTP</b>
                                                </div>
                                                <div class="col-md-10">
                                                    <asp:Literal runat="server" ID="KTPViewDocLit"></asp:Literal>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <div class="row">
                                                <div class="col-md-1 col-fix">
                                                    <b>Alamat</b>
                                                </div>
                                                <div class="col-md-10">
                                                    <asp:Literal runat="server" ID="alamatViewDocLit"></asp:Literal>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </tr>

                                <tr>
                                    <td class="pt-2 pb-2">Dalam hal ini bertindak untuk dan atas namanya sendiri yang selanjutnya disebut sebagai <b>Pihak Kedua</b>.
                                    </td>
                                </tr>

                                <tr>
                                    <td class="pt-2 pb-2">Secara bersama - sama kedua disebut : <b>"Para Pihak"</b> dan secara sendiri disebut <b>Pihak</b>, menerangkan terlebih dahulu hal-hal sebagai berikut :
                                    </td>
                                </tr>

                                <tr>
                                    <td>

                                        <ul>
                                            <li style="list-style-type: circle; text-align: justify">Bahwa <b>Pihak Pertama</b> adalah Koperasi yang bergerak di bidang Simpan Pinjam
                                            </li>
                                            <li style="list-style-type: circle">Bahwa <b>Pihak Kedua</b>, adalah anggota koperasi <b>Pihak Pertama yang sekaligus merupakan karyawan
                                                    <asp:Literal runat="server" ID="compNameViewDocLit"></asp:Literal></b></li>

                                            <li style="list-style-type: circle; text-align: justify">
                                                <b>Pihak Kedua</b> dengan ini menyatakan telah mengajukan permohonan pinjaman dana tunai kepada <b>Pihak Pertama</b>
                                            </li>

                                            <li style="list-style-type: circle; text-align: justify">
                                                <b>Pihak Pertama</b> telah setuju untuk memberikan pinjaman dana tunai kepada <b>Pihak Kedua</b>

                                            </li>
                                        </ul>
                                        <hr />
                                    </td>

                                </tr>

                                <tr>
                                    <td class="pt-2 pb-2">Sehubungan dengan hal tersebut di atas, <b>Para Pihak</b> sepakat untuk mengikatkan diri dalam <b>Perjanjian Hutang-Piutang</b>, selanjutnya disebut <b>“Perjanjian”</b>, dengan syarat-syarat dan ketentuan-ketentuan sebagai berikut:
                                    </td>
                                </tr>

                                <%--Pasal 1--%>

                                <div class="col-md-12 row">
                                    <div class="col-md-10">
                                        <tr>
                                            <td class="pb-2">
                                                <div class="row">
                                                    <div class="col-md-12 font-weight-bold text-center">Pasal 1</div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12 font-weight-bold text-center">JUMLAH PINJAMAN DAN JANGKA WAKTU</div>
                                                </div>

                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <ul>
                                                    <li style="list-style-type: decimal; text-align: justify">
                                                        <b>Pihak Kedua</b> dengan ini menyatakan telah mengajukan pinjaman dana tunai dari <b>Pihak Pertama</b> sebesar <b>Rp.
                                                        <asp:Literal runat="server" ID="amountAngkaViewDocLit"></asp:Literal></b>  <b>( Terbilang :
                                                            <asp:Literal runat="server" ID="amountTerbilangViewDocLit"></asp:Literal>
                                                            Rupiah )</b> untuk selanjutnya disebut sebagai <b>“Hutang”</b>
                                                    </li>

                                                    <li style="list-style-type: decimal; text-align: justify">
                                                        <b>Pihak Pertama</b> telah setuju untuk memberikan pinjaman kepada <b>Pihak Kedua</b> dengan mentransfer secara cash dan sekaligus ke rekening <b>Pihak Kedua</b>.
                                                    </li>

                                                    <li style="list-style-type: decimal; text-align: justify">
                                                        <b>Para Pihak</b> sepakat akan menyelesaikan Hutang selama
                                                    <asp:Literal ID="tenorViewDocLit" runat="server"></asp:Literal>
                                                        (<asp:Literal runat="server" ID="tenorTerbilangViewDocLit"></asp:Literal>) bulan yang dilakukan melalui pemotongan gaji oleh Payroll <b>Pihak Kedua</b>, untuk selanjutnya disebut “Jangka Waktu Perjanjian”. 
                                                    </li>
                                                </ul>
                                            </td>
                                        </tr>
                                    </div>

                                    <div class="col-md-2">
                                        <tr>
                                            <td class="pull-right font-weight-bold text-success">
                                                <asp:CheckBox ID="pasal1ViewDocCheckBox" runat="server" />
                                                <span>Saya mengetahui dan menyetujui pasal 1 ini</span>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <hr />
                                            </td>
                                        </tr>


                                    </div>
                                </div>


                                <%--PASAL 2--%>

                                <div class="col-md-12 row">
                                    <div class="col-md-10">

                                        <tr>
                                            <td>
                                                <div class="row">
                                                    <div class="col-md-12 font-weight-bold text-center">Pasal 2</div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12 font-weight-bold text-center">BESARAN ANGSURAN DAN CARA PEMBAYARAN</div>
                                                </div>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <ul>
                                                    <li style="list-style-type: decimal; text-align: justify">
                                                        <b>Pihak Kedua</b> wajib melunasi kembali Hutang tersebut kepada <b>Pihak Pertama</b>  dengan cara mencicil setiap bulannya sebagaimana diatur dalam Pasal 1 ayat 3 Perjanjian ini dengan cara pemotongan upah oleh pihak Payroll <b>Pihak Kedua</b> berdasarkan Surat Kuasa yang telah dibuat oleh <b>Pihak Kedua</b>
                                                    </li>

                                                    <li style="list-style-type: decimal; text-align: justify">Atas peminjaman tersebut <b>Pihak Kedua</b> akan dikenakan bunga tetap sebesar
                                                    <asp:Literal runat="server" ID="bungaViewDocLit"></asp:Literal>%
                                                (
                                                    <asp:Literal ID="bungaTerbilangViewDocLit" runat="server"></asp:Literal>
                                                        persen ) setiap bulannya
                                                    </li>

                                                    <li style="list-style-type: decimal; text-align: justify">Sehingga besaran cicilan setiap bulannya dalam penyelesaian Hutang ini adalah sebesar Rp.
                                                    <asp:Literal runat="server" ID="cilPokViewDocLit"> </asp:Literal>
                                                        (terbilang
                                                    <asp:Literal runat="server" ID="cilPokTerbilangViewDocLit"></asp:Literal>), untuk selanjutnya disebut <b>“Angsuran”</b>.
                                                    </li>
                                                </ul>
                                            </td>
                                        </tr>
                                    </div>
                                    <div class="col-md-2">
                                        <tr>
                                            <td class="pull-right font-weight-bold text-success">
                                                <asp:CheckBox ID="pasal2ViewDocCheckBox" runat="server" />
                                                <span>Saya mengetahui dan menyetujui pasal 2 ini</span>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <hr />
                                            </td>
                                        </tr>
                                    </div>
                                </div>

                                <%--PASAL 3--%>

                                <div class="col-md-12 row">
                                    <div class="col-md-10">
                                        <tr>
                                            <td>
                                                <div class="row">
                                                    <div class="col-md-12 font-weight-bold text-center">Pasal 3</div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12 font-weight-bold text-center">HAL-HAL YANG MENGAKIBATKAN BERAKHIRNYA PERJANJIAN</div>
                                                </div>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <ul>
                                                    <li style="list-style-type: decimal; text-align: justify">Perjanjian akan berakhir dengan sendirinya apabila Jangka Waktu Perjanjian berakhir dan Hutang telah dinyatakan lunas oleh Pihak Pertama.
                                                    </li>


                                                    <li style="list-style-type: decimal; text-align: justify">Apabila terjadi pemutusan hubungan kerja <b>Pihak Kedua</b> oleh karena sebab apapun, maka <b>Pihak Kedua</b> wajib melunasi Hutang dan bunga berjalan kepada <b>Pihak Pertama</b> selambat-lambatnya 7 (tujuh) hari sebelum tanggal efektif berakhirnya hubungan kerja <b>Pihak Kedua</b>
                                                    </li>

                                                    <li style="list-style-type: decimal; text-align: justify">Pelunasan terhadap sisa kurang sebagaimana dimaksud ayat 2, maka <b>Pihak Kedua</b> setuju untuk diperhitungkan dengan hak-haknya yang ada di <b>Pihak Pertama</b> seperti Simpanan Wajib, Simpanan Sukarela dan Simpanan Berjangka (jika ada).
                                                    </li>

                                                    <li style="list-style-type: decimal; text-align: justify">Apabila hak-hak tersebut pada Ayat 3 diatas tidak mencukupi untuk melunasi kewajiban, maka <b>Pihak Kedua</b> sepakat untuk diperhitungkan dari Upah terakhir, Uang Pisah, Uang Pesangon, Uang Penghargaan Masa Kerja dan/atau Uang Penggantian Hak <b>Pihak Kedua</b> yang akan diterimanya karena pemutusan hubungan kerja
                                                    </li>

                                                    <li style="list-style-type: decimal; text-align: justify">Apabila hak-haknya sebagaimana dimaksud ayat 3 dan ayat 4, ternyata belum mencukupi, maka <b>Pihak Kedua</b> diwajibkan menyelesaikan kekurangan kewajiban tersebut selambat-lambatnya pada hari terakhir <b>Pihak Kedua</b> bekerja
                                                    </li>

                                                </ul>
                                            </td>
                                        </tr>

                                    </div>
                                    <div class="col-md-2">
                                        <tr>
                                            <td class="pull-right font-weight-bold text-success">
                                                <asp:CheckBox ID="pasal3ViewDocCheckBox" runat="server" />
                                                <span>Saya mengetahui dan menyetujui pasal 3 ini</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <hr />
                                            </td>
                                        </tr>
                                    </div>
                                </div>




                                <%--PASAL 4--%>

                                <div class="col-md-12 row">
                                    <div class="col-md-10">
                                        <tr>
                                            <td>
                                                <div class="row">
                                                    <div class="col-md-12 font-weight-bold text-center">Pasal 4</div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12 font-weight-bold text-center">PENYELESAIAN PERSELISIHAN</div>
                                                </div>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <ul>
                                                    <li style="list-style-type: decimal; text-align: justify">Apabila ada hal-hal yang tidak atau belum diatur dalam Perjanjian ini, dan juga jika terjadi perbedaan penafsiran atas seluruh atau sebagian dari Perjanjian ini, maka kedua belah pihak sepakat untuk menyelesaikannya secara musyawarah untuk mufakat.
                                                    </li>

                                                    <li style="list-style-type: decimal; text-align: justify">Jika penyelesaian secara musyawarah untuk mufakat tidak tercapai, maka perselisihan tersebut akan diselesaikan sesuai hukum yang berlaku di Indonesia, dan oleh karena itu kedua belah pihak memilih domisili hukum di Pengadilan Negeri Jakarta Barat sesuai domisili hukum Pihak Pertama. 
                                                    </li>
                                                </ul>
                                            </td>
                                        </tr>

                                    </div>

                                    <div class="col-md-2">
                                        <tr>
                                            <td class="pull-right font-weight-bold text-success">
                                                <asp:CheckBox ID="pasal4ViewDocCheckBox" runat="server" />
                                                <span>Saya mengetahui dan menyetujui pasal 4 ini</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <hr />
                                            </td>
                                        </tr>
                                    </div>
                                </div>


                                <div class="col-md-12 px-4 mx-5 row">
                                    <tr>
                                        <td class="pb-3">Demikian Perjanjian ini dibuat dan ditandatangani oleh para pihak secara elektronik.
                                        </td>
                                    </tr>
                                </div>


                            </table>
                            <table class="col-md-12 px-4 mx-5">
                                <div class="">

                                    <tr>
                                        <div class="row">
                                            <td class="text-center pb-lg-5 font-weight-bold">Pihak Pertama,</td>
                                        </div>

                                        <div class="row">
                                            <td class="text-center pb-lg-5 font-weight-bold">Pihak Kedua,</td>
                                        </div>

                                    </tr>


                                    <tr>
                                        <div class="row">
                                            <td class="text-center">Rudy Prakoso</td>
                                        </div>
                                        <div class="row">
                                            <td class="text-center">
                                                <asp:Literal runat="server" ID="anggotaNameViewDocLit2"></asp:Literal>
                                            </td>
                                        </div>
                                    </tr>

                                    <tr>
                                        <td class="col-md-3">
                                            <hr />
                                        </td>
                                        <td class="col-md-3">
                                            <hr />
                                        </td>
                                    </tr>

                                    <tr>
                                        <div class="row">
                                            <td class="text-center">
                                                <b>Ketua KOPKARI</b>
                                            </td>
                                        </div>
                                        <div class="row">
                                            <td class="text-center">
                                                <b>Anggota</b>
                                            </td>

                                        </div>
                                    </tr>

                                    <tr>
                                        <div class="row">
                                            <td class="text-center px-3">
                                                <asp:Literal runat="server" ID="tglPihak1ViewDocLit">
                                                </asp:Literal>
                                            </td>

                                        </div>
                                        <div class="row">
                                            <td class="text-center px-3">
                                                <asp:Literal runat="server" ID="tglPihak2ViewDocLit">
                                                </asp:Literal>
                                            </td>

                                        </div>
                                    </tr>

                                </div>
                            </table>

                        </div>

                        <div class="modal-footer">
                            <asp:LinkButton runat="server" ID="viewDocCloseBtn" CssClass="btn btn-secondary no-mn-bottom"
                                OnClick="viewDocCloseBtn_click"><i class="fa fa-arrow-circle-left"></i>Kembali</asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!-- ini update panel buat popupnya -->
            </div>
        </div>
    </div>
    <!-- akhir modal dialog view PHP (ini popup) -->

    <asp:TextBox ID="txtFileNameDownload" runat="server" Enabled="false" Style="display: none;"></asp:TextBox>
    <asp:TextBox ID="txtFileNameCheckDownload" runat="server" Enabled="false" Style="display: none;"></asp:TextBox>
    <asp:Button ID="btnDownloadHide" runat="server" Text="Download" CssClass="invisible" Style="padding: 5px" OnClick="btnDownloadHide_click" />
    <asp:Button ID="btnDownloadCheckHide" runat="server" Text="Download" CssClass="invisible" Style="padding: 5px" OnClick="btnDownloadCheckHide_click" />

    <!-- modal dialog preview attach-->
    <div id="modalPreviewAttachment" class="modal fade modal-dialog-add" tabindex="-1"
        role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-fullscreen">
            <div class="modal-content">
                <asp:UpdatePanel ID="updPreviewAttachment" runat="server" UpdateMode="Conditional"
                    ChildrenAsTriggers="false">
                    <Triggers>
                    </Triggers>
                    <ContentTemplate>
                        <div class="modal-header bg-success text-white">
                            <asp:Label ID="Label40" runat="server">Preview Attachment</asp:Label>
                            <a type="button" class="close text-white" data-dismiss="modal">
                                <i class="fa fa-times text-white" aria-hidden="true"></i>
                            </a>
                        </div>
                        <div class="modal-body" id="Div2">
                            <div class="x_panel">
                                <div class="x_content">
                                    <asp:Label ID="ketLbl" runat="server" CssClass="alert alert-fill-light text-dark" BorderWidth="2px" BorderStyle="Solid"></asp:Label>
                                    <asp:Literal ID="litPreviewAttachment" runat="server"></asp:Literal>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <!-- EOF modal preview attach-->

    <script type="text/javascript">
        var _baseUrl = '<%=ResolveUrl("~/")%>';
        Sys.Application.add_init(appl_init);

        function appl_init() {
            var pgRegMgr = Sys.WebForms.PageRequestManager.getInstance();
            pgRegMgr.add_endRequest(EndHandlerGrid);
            pgRegMgr.add_beginRequest(BeginHandlerGrid);
        }

        function BeginHandlerGrid(sender, args) {
            loading_start();
        }

        function EndHandlerGrid(sender, args) {
            $.unblockUI();
            EditStart();
            init();
        }

        function EditStart() {

            isEdit = true;
            if ($('#<%= hidTxnID.ClientID %>').val().trim() == "") {
                isLoadEmp = "0";
            } else {
                isLoadEmp = "1";
            }
        }

        function loading_start() {
            $(document).ready(function () {
                $.blockUI({ message: "<h1 class='text-success p-2' style='font-size:x-large'><i class='fa fa-spin fa-circle-o-notch mr-2'></i><span >Please Wait</span></h1>", baseZ: 5000 });
            });
        }

        $(document).ready(function () {
            init();
        });

        function dataDetail(pnjID) {
            if (pnjID != '') {
                $('#<%= hidTxnID.ClientID %>').val(pnjID);
            }
            $('#<%= btnDetail.ClientID %>').click();
        }

        function printLetter(pnjID) {
            if (pnjID != '') {
                $('#<%= hidTxnID.ClientID %>').val(pnjID);
            }
            $('#<%= btnPrintLetter.ClientID %>').click();
        }
        <%--function printLetter(packetID) {
            $('#<%= printBtn.ClientID %>').click();
        }--%>

        function uploadDoc(pnjID) {
            if (pnjID != '') {
                $('#<%= hidTxnID.ClientID %>').val(pnjID);
            }
            $('#<%= btnUploadDoc.ClientID %>').click();
        }


        function processDoc(pnjID) {
            if (pnjID != '') {
                $('#<%= hidTxnID.ClientID %>').val(pnjID);
            }
            $('#<%= btnProcessDoc.ClientID %>').click();
        }

        function viewPHP() {
            $('#<%= btnViewPHP.ClientID %>').click();
        }

        function init() {

            $('.datepicker').daterangepicker({
                singleDatePicker: true,
                locale: { format: 'DD-MM-YYYY', firstDay: 1 }
            });
        }

        function buttonUploadClick(clientTextBox, txtIsEdit, docID, txtIsUploaded) {

            $('#<%= hidClientTextBox.ClientID %>').val(clientTextBox);
            $('#<%= hidFilIsEdit.ClientID %>').val(txtIsEdit);
            $('#<%= hidFilIsUploaded.ClientID %>').val(txtIsUploaded);
            $('#<%= hidDocID.ClientID %>').val(docID);

            $('#<%= fakeBtnUpload.ClientID %>').click();

            $('#<%= ajaxFileUpAttach.ClientID %>').children().children().find('input[type=file][multiple=multiple]').click();
        }

        /* bagian check download / preview yang di detail*/

        function buttonPreviewCheckClick(docID, pinjamanID) {
            console.log("kepanggil");
            if (docID != '') {
                $('#<%= hidDocIDPreview.ClientID %>').val(docID);
            }

            if (pinjamanID != '') {
                $('#<%= hidTxnID.ClientID %>').val(pinjamanID);
            }

            $('#<%= btnPreviewCheckHide.ClientID %>').click();
        }

        function buttonDownloadCheckClick(docID, pinjamanID) {
            if (docID != '') {
                $('#<%= hidDocIDDownload.ClientID %>').val(docID);
            }

            if (pinjamanID != '') {
                $('#<%= hidTxnID.ClientID %>').val(pinjamanID);
            }
            $('#<%= btnDownloadCheckHide.ClientID %>').click();
        }


        /* bagian check download / preview yang di upload ni*/

        function buttonDownloadClick(docID, pinjamanID) {
            if (docID != '') {
                $('#<%= hidDocIDDownload.ClientID %>').val(docID);
            }
            if (pinjamanID != '') {
                $('#<%= hidTxnID.ClientID %>').val(pinjamanID);
            }
            $('#<%= btnDownloadHide.ClientID %>').click();
        }


        function buttonPreviewClick(docID, pinjamanID) {
            if (docID != '') {
                $('#<%= hidDocIDPreview.ClientID %>').val(docID);
            }
            if (pinjamanID != '') {
                $('#<%= hidTxnID.ClientID %>').val(pinjamanID);
            }
            $('#<%= btnPreviewHide.ClientID %>').click();
        }


        function openPreviewModal() {
            $('#modalPreviewAttachment').modal({
                backdrop: 'static',
                keyboard: false
            });
        }

        function ajaxFileUp_onClientUploadComplete(sender, e) {
            <%--var docID = $('#<%= hidDocID.ClientID %>').val();--%>

            var fName = e.get_fileName();
            var clientTextBox = $('#<%= hidClientTextBox.ClientID %>').val();
            var clientHidFileName = $('#<%= hidFileName.ClientID %>').val();

            //var fNameX = fName.replace(/ /g, "").replace(/'/g, "");
            var isEdit = 'true';
            var isUploaded = 'true';
            var txtIsEdit = $('#<%= hidFilIsEdit.ClientID %>').val();
            var txtIsUploaded = $('#<%= hidFilIsUploaded.ClientID %>').val();

            var pnjID = $('#<%= hidTxnID.ClientID %>').val();
            var docID = $('#<%= hidDocID.ClientID %>').val();

            $('#' + txtIsEdit).val(isEdit);
            $('#' + txtIsUploaded).val(isUploaded);
            <%--$('#<%= hidFileName.ClientID %>').val(fName);--%>
            <%-- $('#<%= hidTxnID.ClientID %>').val(fName);--%>

            var fileName = pnjID + '_' + docID + '_' + fName;

            $('#' + clientTextBox).val(fileName);

        }

        function ajaxFileUp_ClientUploadStart(sender, e) {

        }

        function submitScript() {


            var noKTPExist = $(".txtNoKTP").length;


            if (noKTPExist == 0) { // gk ada
                Swal.fire({
                    text: "Anda yakin akan memproses ?",
                    showCancelButton: true,
                    cancelButtonColor: '#808080',
                    confirmButtonColor: '#09b76b',
                    reverseButtons: true,
                    timerProgressBar: true,
                    icon: 'question'
                }).then((result) => {
                    if (result.isConfirmed) {
                        $('#<%= uploadDocSubmitBtn.ClientID %>').click();
                    } else if (result.isDenied) {
                        Swal.fire({
                            title: 'Cancelled',
                            text: 'Transaksi Dibatalkan',
                            icon: 'error',
                        })
                    }
                });
            }

            else {
                var noKTPVal = $(".txtNoKTP").val();

                if (noKTPVal == null) {
                    Swal.fire({
                        title: 'Warning!',
                        text: 'Dimohon untuk mengisi NIK',
                        icon: 'warning',
                    })
                }

                else if (noKTPVal.length != 16 || isNaN(noKTPVal)) { // mesti 16 digit + harus angka semua
                    Swal.fire({
                        title: 'Warning!',
                        text: 'Dimohon untuk memasukkan nomor KTP (16 digit)',
                        icon: 'warning',
                    })
                }

                else {
                    Swal.fire({
                        text: "Anda yakin akan memproses ?",
                        showCancelButton: true,
                        cancelButtonColor: '#808080',
                        confirmButtonColor: '#09b76b',
                        reverseButtons: true,
                        timerProgressBar: true,
                        icon: 'question'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            $('#<%= uploadDocSubmitBtn.ClientID %>').click();
                        } else if (result.isDenied) {
                            Swal.fire({
                                title: 'Cancelled',
                                text: 'Transaksi Dibatalkan',
                                icon: 'error',
                            })
                        }
                    });
                }

            }
        }

        function aktifinMulti() {
            $('.multiple-select').multiselect({
                numberDisplayed: 1,
                buttonWidth: '100%',
                maxHeight: 300,
                enableFiltering: true,
                filterBehavior: 'text',
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
            });
        }

        function pageLoadRoutines() {
            aktifinMulti();
        }

        function checkScript() {
            var cb1 = document.getElementById('<%= pasal1CheckBox.ClientID %>').checked;
            var cb2 = document.getElementById('<%= pasal2CheckBox.ClientID %>').checked;
            var cb3 = document.getElementById('<%= pasal3CheckBox.ClientID %>').checked;
            var cb4 = document.getElementById('<%= pasal4CheckBox.ClientID %>').checked;

            const d = new Date();

            const weekday = new Array(7);
            weekday[0] = "Minggu";
            weekday[1] = "Senin";
            weekday[2] = "Selasa";
            weekday[3] = "Rabu";
            weekday[4] = "Kamis";
            weekday[5] = "Jumat";
            weekday[6] = "Sabtu";

            let day = weekday[d.getDay()];

            const month = new Array();
            month[0] = "Januari";
            month[1] = "Februari";
            month[2] = "Maret";
            month[3] = "April";
            month[4] = "Mei";
            month[5] = "Juni";
            month[6] = "Juli";
            month[7] = "Agustus";
            month[8] = "September";
            month[9] = "Oktober";
            month[10] = "November";
            month[11] = "Desember";

            let monthName = month[d.getMonth()];

            let date = d.getDate();

            let yearName = d.getFullYear();

            var content = document.createElement('div');
            content.innerHTML = 'Demikian Perjanjian ini dibuat dan ditandatangani <br> pada hari ' + day + ', ' + date + ' ' + monthName + ' ' + yearName;

            if (cb1 & cb2 & cb3 & cb4) {
                Swal.fire({
                    title: "Konfirmasi",
                    html: content,
                    showCancelButton: true,
                    cancelButtonColor: '#808080',
                    confirmButtonColor: '#09b76b',
                    reverseButtons: true,
                    timerProgressBar: true,
                    icon: 'question'
                }).then((result) => {
                    if (result.isConfirmed) {
                        document.getElementById('<%= nextBtn.ClientID %>').disabled = false;
                        $('#<%= nextBtn_hide.ClientID %>').click();
                    } else if (result.isDenied) {
                        Swal.fire({
                            title: 'Cancelled',
                            text: 'Transaksi Dibatalkan',
                            icon: 'error',
                        })
                    }
                })
            }

            else {

                var content2 = document.createElement('div');
                content2.innerHTML += 'Anda belum menyetujui <br>'
                if (!cb1) {
                    content2.innerHTML += '- Pasal 1<br>'
                }
                if (!cb2) {
                    content2.innerHTML += '- Pasal 2<br>'
                }
                if (!cb3) {
                    content2.innerHTML += '- Pasal 3<br>'
                }
                if (!cb4) {
                    content2.innerHTML += '- Pasal 4<br><br>'
                }

                content2.innerHTML += 'Dimohon untuk menyetujui pasal tersebut'

                swal.fire({
                    title: 'Warning',
                    html: content2,
                    icon: 'warning'
                });
            }


        }

        function saveScript() {
            Swal.fire({
                text: "Anda yakin akan memproses ?",
                showCancelButton: true,
                cancelButtonColor: '#808080',
                confirmButtonColor: '#09b76b',
                reverseButtons: true,
                timerProgressBar: true,
                icon: 'question'
            }).then((result) => {
                if (result.isConfirmed) {
                    $('#<%= uploadDocSaveBtn.ClientID %>').click();
                } else if (result.isDenied) {
                    Swal.fire({
                        title: 'Cancelled',
                        text: 'Transaksi Dibatalkan',
                        icon: 'error',
                    })
                }
            })

        }

        function updateSuccess() {
            Swal.fire({
                title: 'Upload Dokumen Sukses!',
                text: 'Dokumen anda telah tersimpan',
                icon: 'success',
                confirmButtonColor: '#09b76b',
            }).then(function (value) { window.location = 'statusPinjamanAnggotaOnAnggota.aspx'; });
        }

        function txnFailed(msg) {
            Swal.fire({
                title: 'Failed',
                text: msg,
                icon: 'error',
                confirmButtonColor: '#09b76b'
            });
        }

    </script>
</asp:Content>
