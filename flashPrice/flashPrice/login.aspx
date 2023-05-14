<%@ Page Title="" Language="C#" MasterPageFile="~/login.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="flashPrice.login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPh" runat="server">


    <div class="d-flex align-items-center py-5">
        <div class="container">
            <div class="row align-items-center py-5">
                <div class="col-5 col-lg-7 mx-auto mb-5 mb-lg-0">

                    <div class="pr-lg-5">
                        <img src="assets/images/flashPriceLogo.png" style="border-radius:25px;" alt="not found" class="img-fluid" />
                    </div>
                </div>
                <div class="col-lg-5 px-lg-4">
                    <h5 class="mb-4">Login as Admin</h5>

                    <div class="form-group mb-4">
                        <asp:TextBox required="true" CssClass="form-control border-0 shadow form-control-lg text-base" placeholder="Username" ID="userNameTextBox" runat="server"></asp:TextBox>
                    </div>

                    <div class="form-group mb-4">
                        <asp:TextBox required="true" TextMode="Password" CssClass="form-control border-0 shadow form-control-lg text-base " placeholder="Password" ID="passwordTextBox" runat="server"></asp:TextBox>

                    </div>
                    <div class="form-group mb-4">
                        <div class="custom-control custom-checkbox">
                            <asp:CheckBox Text="&nbsp&nbsp&nbspRemember Me" runat="server" />
                        </div>
                    </div>
                    <asp:LinkButton CssClass="btn btn-primary form-control" runat="server"><i class="fa fa-sign-in mr-2"></i> Login</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
