using ServiceDesk.Utilities;
using System;
using System.Web;

namespace ServiceDesk.WebApp.Account
{
    public partial class Authority : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Claim.Cookie[Config.LastUrl] = string.Empty;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Redirect(Config.LoginUrl);
        }
    }
}