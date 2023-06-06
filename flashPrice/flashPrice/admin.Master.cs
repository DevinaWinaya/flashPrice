using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace flashPrice
{
    public partial class admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] != null)
            {
                userNameSpan.InnerText = Session["username"].ToString();
            }
            else
            {
                Response.Redirect("~/login.aspx");
            }
        }

        protected void logoutBtn_Click(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("~/login.aspx");
            }
            else
            {
                Session["username"] = null;
                Response.Redirect("~/login.aspx");
            }
        }
    }
}