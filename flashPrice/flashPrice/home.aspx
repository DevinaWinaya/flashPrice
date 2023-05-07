<%@ Page Title="" Language="C#" MasterPageFile="~/default.Master" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="flashPrice.pages.home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPh" runat="server">
    <asp:UpdatePanel ID="updatePanelSearchResultRepeater" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="resultDiv" runat="server">

                <nav class="navbar navbar-expand-lg navbar-light" style="background-color: navy;">
                    <a class="navbar-brand text-white" href="#">Flash Price</a>
                    <div class="navbar-nav mr-auto col-md-4 mx-auto">
                        <asp:TextBox runat="server" ID="navSearchTextBox" CssClass="form-control mr-2" placeHolder="Type to search . . ."></asp:TextBox>
                        <asp:LinkButton runat="server" ID="navSearchBtn" OnClick="navSearchBtn_Click" CssClass="btn btn-light"><i class="fa fa-search"></i></asp:LinkButton>
                    </div>

                </nav>

                <div id="queryResultDiv" class="row mx-auto col-md-6 mt-3 col-xs-12 col-sm-12 border border-secondary-50" runat="server">
                    <asp:Literal ID="litError" runat="server"></asp:Literal>
                    <asp:Repeater ID="resultRepeater" runat="server" OnItemDataBound="resultRepeater_ItemDataBound" OnItemCommand="resultRepeater_ItemCommand">
                        <ItemTemplate>
                            <div class="col-md-4 col-sm-12 col-xs-12">
                                <div class="card mt-3 mb-3 col-md-12">
                                    <div class="card-header bg-white">
                                        <asp:Literal ID="litProductImg" runat="server"></asp:Literal>
                                        <img src="assets/images/Searchs_004.png" style="border-radius: 10px;height:auto;width:320px;" class="img-fluid" />
                                    </div>
                                    <div class="card-body">
                                        <div class="row">
                                            <span>
                                                <asp:Label ID="productNameLbl" CssClass="h5 text-warning" runat="server" Text='<%#Eval("productName") %>'></asp:Label>
                                            </span>
                                        </div>
                                        <div class="row">
                                            <span>
                                                <i class="fa fa-money-bill-1 mr-2"></i>
                                                Rp<asp:Label ID="productPriceLbl" CssClass="ml-1" runat="server" Text='<%#Eval("productPrice") %>'></asp:Label>
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
                                </div>

                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>

            </div>

        </ContentTemplate>
    </asp:UpdatePanel>




</asp:Content>
