using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using ServiceDesk.WebApp.Culture;

namespace ServiceDesk.WebApp.Issues
{
  public partial class CancelIssue : LanguagePage
  {
    private readonly IAuthorityRepository _authorityRepository;
    private readonly IIssuesRepository _issuesRepository;

    public CancelIssue(IAuthorityRepository authorityRepository,
      IIssuesRepository issuesRepository)
    {
      _authorityRepository = authorityRepository;
      _issuesRepository = issuesRepository;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
      _authorityRepository.LoadPrivilege();
      if (!IsPostBack)
      {
        if (!_authorityRepository.LoggedIn() || Request.QueryString["IssueId"] == null)
          Response.Redirect("~/Account/Login.aspx?returnUrl=" + Server.UrlEncode(Request.Url.AbsolutePath));
      }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
      {
        var issueId = Helper.ConvertToInt(Request.QueryString["IssueId"]);
        if (_issuesRepository.CanCancelIssue(issueId) > 0)
        {
          var reason = txtReason.Text.Trim();
          _issuesRepository.CancelIssue(issueId, reason);
        }
      }

      ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CloseAndRebind();", true);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
      ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CancelEdit();", true);
    }
  }
}