using Microsoft.AspNet.Identity;
using ServiceDesk.Utilities;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceDesk.WebApp
{
    public partial class Layout : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) spUserId.InnerHtml = Session["UserName"]?.ToString() ?? string.Empty;
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        public void ImgVN_Click(object sender, EventArgs e)
        {
            Claim.Session[Config.LanguageId] = "vi-VN";
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        protected void ImgRu_Click(object sender, ImageClickEventArgs e)
        {
            Claim.Session[Config.LanguageId] = "ru-Ru";
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        //protected override void Render(HtmlTextWriter writer)
        //{
        //    //html minifier & JS at bottom
        //    // not tested
        //    if (this.Request.Headers["X-MicrosoftAjax"] != "Delta=true")
        //    {
        //        var reg = new Regex(@"<script[^>]*>[\w|\t|\r|\W]*?</script>");
        //        var sb = new System.Text.StringBuilder();
        //        var sw = new System.IO.StringWriter(sb);
        //        var hw = new HtmlTextWriter(sw);
        //        base.Render(hw);
        //        var html = sb.ToString();
        //        var myMatch = reg.Matches(html);
        //        html = reg.Replace(html, string.Empty);
        //        reg = new Regex(@"(?<=[^])\t{2,}|(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,11}(?=[<])|(?=[\n])\s{2,}|(?=[\r])\s{2,}");
        //        html = reg.Replace(html, string.Empty);
        //        reg = new Regex(@"</body>");
        //        var str = myMatch.Cast<Match>().Aggregate(string.Empty, (current, match) => current + match.ToString());
        //        html = reg.Replace(html, str + "</body>");
        //        writer.Write(html);
        //    }
        //    else
        //        base.Render(writer);
        //}
    }
}