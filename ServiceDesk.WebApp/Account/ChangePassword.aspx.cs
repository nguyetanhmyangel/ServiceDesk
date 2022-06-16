using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceDesk.Data.Features.Login;
using ServiceDesk.Data.Features.User;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using ServiceDesk.WebApp.Culture;

namespace ServiceDesk.WebApp.Account
{
    public partial class ChangePassword : LanguagePage
    {

        private readonly IUserRepository _userRepository;
        private readonly IAuthorityRepository _authorityRepository;

        public ChangePassword(IUserRepository userRepository, IAuthorityRepository authorityRepository)
        {
            _userRepository = userRepository;
            _authorityRepository = authorityRepository;
        }

        #region Property

        public string UserName
        {
            get => txtUserName.Text.Trim();
            set => txtUserName.Text = value;
        }

        public string OldPassword
        {
            get => txtOldPassword.Text.Trim();
            set => txtOldPassword.Text = value;
        }

        public string NewPassword
        {
            get => txtNewPassword.Text.Trim();
            set => txtNewPassword.Text = value;
        }

        public string ConfirmPassWord
        {
            get => txtConfirmPassWord.Text.Trim();
            set => txtConfirmPassWord.Text = value;
        }

        #endregion Property

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!_authorityRepository.LoggedIn() && !_authorityRepository.IsAdmin())
                    Response.Redirect("~/Account/Login.aspx?returnUrl=" + Server.UrlEncode(Request.Url.AbsolutePath));
                txtUserName.Text = Claim.Session[Config.UserName] != null ? Claim.Session[Config.UserId].ToString() : "All";
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var loginResponse = new LoginResponse()
            {
                UserName = txtUserName.Text,
                Password = Helper.Encrypt(txtNewPassword.Text),
                HrPassword = txtOldPassword.Text
            };
            
            if (!_authorityRepository.HrPasswordSignIn(loginResponse))
                Helper.Notification(RadNotification1, "Nhập password cũ ko chính xác", "warning");
            else if (_authorityRepository.ChangePassword(loginResponse))
                Helper.Notification(RadNotification1, "Thay đổi password thành công", "ok");
            else
                Helper.Notification(RadNotification1, "Thay đổi password thất bại", "warning");
        }
    }
}