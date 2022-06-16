using ServiceDesk.Data.Features.Login;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using System;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;

namespace ServiceDesk.WebApp.Account
{
    public partial class Login : Page
    {
        private readonly IAuthorityRepository _authorityRepository;

        //private AuthorityService authorityService = new AuthorityService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HideCaptcha(reCaptchar);
            }
        }

        public Login(IAuthorityRepository authorityRepository)
        {
            _authorityRepository = authorityRepository;
        }

        public const int ResetFailedAttempts = 4;

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrEmpty(txtPassword.Text))
                    Helper.DisplayMessage(true, "UserNam and Password is required.", lbNotice);
                else
                {
                    //var passWord = Helper.Encrypt(txtPassword.Text.Trim());
                    var loginResponse = new LoginResponse
                    {
                        UserName = txtUserName.Text.Trim(),
                        HrPassword = txtPassword.Text.Trim(),
                        Remember = Remember.Checked,
                        Password = Helper.Encrypt(txtPassword.Text.Trim())
                    };
                    //var result = authorityService.UserAuthority(loginModel);
                    var result = _authorityRepository.UserAuthority(loginResponse);
                    if (!result)
                    {
                        Helper.DisplayMessage(true, "UserName or Password is not authorized.", lbNotice);
                        FailedAttempts++;
                        if (ResetFailedAttempts < FailedAttempts)
                        {
                            reCaptchar.Visible = true;
                        }
                    }
                    else
                    {
                        var role = Claim.Session[Config.RoleId] != null ? Helper.ConvertToInt(Claim.Session[Config.RoleId]) : 0;
                        var returnUrl = HttpUtility.UrlEncode(Request.QueryString["returnUrl"]);
                        Helper.RedirectToReturnUrl(returnUrl, role, Response);
                        //Response.Redirect("~/Account/Login.aspx?returnUrl=" + Server.UrlEncode(Request.Url.AbsoluteUri));
                    }
                }
            }
        }

        public void HideCaptcha(RadCaptcha captcha)
        {
            FailedAttempts = 0;
            captcha.Visible = false;
        }

        public int FailedAttempts
        {
            get
            {
                var count = 0;
                try
                {
                    if (null != Claim.Session[Config.FailedAttempts])
                    {
                        count = (int)Claim.Session[Config.FailedAttempts];
                    }
                }
                catch (InvalidCastException) { /* ignore cast errors */ }
                return count;
            }
            set => Claim.Session[Config.FailedAttempts] = value;
        }
    }
}