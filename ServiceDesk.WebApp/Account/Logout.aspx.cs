using ServiceDesk.Utilities;
using System;

namespace ServiceDesk.WebApp.Account
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            Response.Redirect(Config.LoginUrl);
        }
    }
}