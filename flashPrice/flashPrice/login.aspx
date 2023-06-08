<%@ Page Title="" Language="C#" MasterPageFile="~/login.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="flashPrice.login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPh" runat="server">


    <div class="d-flex align-items-center py-lg-5">
        <div class="container">
            <div class="row align-items-center py-5">
                <div class="col-5 col-lg-7 mx-auto mb-5 mb-lg-0">
                    <div class="pr-lg-5">
                        <a href="home.aspx">



                        <img src="assets/images/flashPriceLogo.png" style="border-radius: 25px;" alt="not found" class="img-fluid" />
                            </a>
                    </div>
                </div>
                <div class="col-lg-5 px-lg-4">
                    <h5 class="mb-4">Login as <span class="text-green-leaf">Admin</span></h5>

                    <div class="form-group mb-4">
                        <asp:TextBox required="true" CssClass="form-control border-0 shadow form-control-lg text-base" placeholder="Username" ID="usernameTextBox" runat="server"></asp:TextBox>
                    </div>

                    <div class="form-group mb-4">
                        <asp:TextBox required="true" TextMode="Password" CssClass="form-control border-0 shadow form-control-lg text-base " placeholder="Password" ID="passwordTextBox" runat="server"></asp:TextBox>
                    </div>

                    <asp:LinkButton CssClass="btn btn-green btn-block mb-4" ID="loginBtn" runat="server" OnClick="loginBtn_Click"><i class="fa fa-sign-in mr-2"></i> Login</asp:LinkButton>
                    
                    <asp:UpdatePanel ID="updatePanelErrorLogin" runat="server" UpdateMode="Conditional"
                        ChildrenAsTriggers="false">
                        <ContentTemplate>
                            <asp:Label runat="server" ID="loginResultLbl" Visible="false"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
