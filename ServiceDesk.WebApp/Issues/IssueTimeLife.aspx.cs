using ServiceDesk.Data.Interfaces;
using ServiceDesk.Data.ViewModels;
using ServiceDesk.Utilities;
using ServiceDesk.WebApp.Culture;
using System;
using System.Text.RegularExpressions;
using Telerik.Web.UI;

namespace ServiceDesk.WebApp.Issues
{
    public partial class IssueTimeLife : LanguagePage
    {
        private readonly IAuthorityRepository _authorityRepository;

        public IssueTimeLife(IAuthorityRepository authorityRepository)
        {
            _authorityRepository = authorityRepository;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            _authorityRepository.LoadPrivilege();
            if (!IsPostBack)
            {
                if (!_authorityRepository.LoggedIn())
                    Response.Redirect("~/Account/Login.aspx?returnUrl=" + Server.UrlEncode(Request.Url.AbsolutePath));
            }

            if (Request.QueryString["IssueId"] == null)
            {
                Response.Redirect("~/Account/Authority.aspx");
            }
            else
            {
                var number = new Regex(@"^\d+$");
                if (!number.Match(Request.QueryString["IssueId"]).Success)
                    Response.Redirect("~/Account/Authority.aspx");
                else
                    Page.Items.Add("IssueId", Request.QueryString["IssueId"]);
            }
        }


    }
}