<%@ Page Title="" Language="C#" MasterPageFile="~/default.Master" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="flashPrice.pages.home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPh" runat="server">
    <div class="s004" id="searchDiv" runat="server">
        <form runat="server">
            <fieldset>
                <legend>WHAT ARE YOU LOOKING FOR?</legend>
                <div class="inner-form">
                    <div class="input-field btn-group">
                        <asp:TextBox runat="server" ID="searchTextBox" CssClass="form-control" placeHolder="Type to search . . ."></asp:TextBox>
                        <asp:LinkButton runat="server" ID="searchBtn" OnClick="searchBtn_Click" CssClass="btn btn-primary"><i class="fa fa-search"></i></asp:LinkButton>
                    </div>
                </div>
                <div class="suggestion-wrap text-center text-white">
                    <span>flashprice.com</span>
                </div>
            </fieldset>
        </form>
    </div>

    <div id="searchResultDiv">
        <asp:Repeater ID="resultRepeater" runat="server" OnItemDataBound="resultRepeater_ItemDataBound" OnItemCommand="resultRepeater_ItemCommand">
            <HeaderTemplate>
                <table id="Table1">
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="repeaterRow">
                    <td class='col-md-1 pl-0'>
                        <asp:LinkButton ID="detailBtn" runat="server" CssClass="px-3 btn btn-outline-success text-center" CommandName="btnDetail"><i class="fa fa-search"></i></asp:LinkButton>
                        <asp:Label ID="productNameLbl" runat="server" Text='<%#Eval("productName") %>'></asp:Label>
                        <asp:Literal ID="litProductImg" runat="server"></asp:Literal>
                        <asp:Label ID="productPriceLbl" runat="server" Text='<%#Eval("productPrice") %>'></asp:Label>
                        <asp:Label ID="miniMarketName" runat="server" Text='<%#Eval("miniMarketName") %>'></asp:Label>
                        <asp:Label ID="miniMarketAddress" Text='<%#Eval("miniMarketAddress")%>' runat="server"></asp:Label>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>





</asp:Content>
