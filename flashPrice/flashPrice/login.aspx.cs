using flashPriceFx.User;
using flashPriceFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace flashPrice
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void loginBtn_Click(object sender, EventArgs e)
        {
            String username = usernameTextBox.Text;
            String password = passwordTextBox.Text;

            BOProcessResult xLogin = BLLUser.tryLogin(username, password);

            if (xLogin.isSuccess)
            {
                loginResultLbl.Text = xLogin.xMessage;
                loginResultLbl.CssClass = "alert alert-success";

                Session["username"] = username;

                Response.Redirect("~/home.aspx");
            }
            else
            {
                loginResultLbl.Text = xLogin.xMessage;
                loginResultLbl.CssClass = "alert alert-danger";
                loginResultLbl.Visible = true;
            }

            updatePanelErrorLogin.Update();
        }
    }
}