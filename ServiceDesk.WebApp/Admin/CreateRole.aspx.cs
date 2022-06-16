using ServiceDesk.Data.Features.Role;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using System;
using Telerik.Web.UI;

namespace ServiceDesk.WebApp.Admin
{
    public partial class CreateRole : System.Web.UI.Page
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IAuthorityRepository _authorityRepository;

        public CreateRole(IRoleRepository roleRepository, IAuthorityRepository authorityRepository)
        {
            _roleRepository = roleRepository;
            _authorityRepository = authorityRepository;
        }

        #region Property

        public string RoleName
        {
            get => txtRoleName.Text.Trim();
            set => txtRoleName.Text = value;
        }

        #endregion Property

        //private const int ItemsPerRequest = 10;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!_authorityRepository.LoggedIn() && !_authorityRepository.IsAdmin())
                    Response.Redirect("~/Account/Login.aspx?returnUrl=" + Server.UrlEncode(Request.Url.AbsolutePath));
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            RadGrid1.Rebind();
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            ((RadGrid)sender).DataSource = _roleRepository.FindByName(RoleName);
        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            switch (e.Item.OwnerTableView.Name)
            {
                case "Master":
                    {
                        //edit mode
                        if (e.Item is GridEditFormItem item && e.Item.IsInEditMode)
                        {
                            var editItem = (GridEditableItem)e.Item;
                            if (!item.OwnerTableView.IsItemInserted)
                            {
                                var roleId = (RadTextBox)editItem.FindControl("RoleId");
                                roleId.ReadOnly = true;
                            }
                        }
                    }
                    break;
            }
        }

        protected void RadGrid1_InsertCommand(object source, GridCommandEventArgs e)
        {
            switch (e.Item.OwnerTableView.Name)
            {
                case "Master":
                    {
                        var roleCommand = new RoleCommand();
                        if (e.Item is GridEditableItem editedItem)
                        {
                            roleCommand.RoleName = ((RadTextBox)editedItem.FindControl("RoleName")).Text.Trim();
                            roleCommand.RoleId = Helper.ConvertToInt(((RadNumericTextBox)editedItem.FindControl("RoleId")).Value);
                            try
                            {
                                _roleRepository.Add(roleCommand);
                                Helper.Notification(RadNotification1, "Insert is successful", "ok");
                            }
                            catch (Exception)
                            {
                                Helper.Notification(RadNotification1, "Insert is failed", "warning");
                            }
                        }
                    }
                    break;
            }
        }

        protected void RadGrid1_UpdateCommand(object source, GridCommandEventArgs e)
        {
            switch (e.Item.OwnerTableView.Name)
            {
                case "Master":
                    {
                        var roleCommand = new RoleCommand();
                        if (e.Item is GridEditableItem editedItem)
                        {
                            roleCommand.RoleName = ((RadTextBox)editedItem.FindControl("RoleName")).Text.Trim();
                            roleCommand.RoleId = (int)editedItem.GetDataKeyValue("RoleId");
                            try
                            {
                                _roleRepository.Update(roleCommand);
                                Helper.Notification(RadNotification1, "Update is successful", "ok");
                            }
                            catch (Exception)
                            {
                                Helper.Notification(RadNotification1, "Update is failed", "warning");
                            }
                        }
                    }
                    break;
            }
        }

        protected void RadGrid1_DeleteCommand(object source, GridCommandEventArgs e)
        {
            switch (e.Item.OwnerTableView.Name)
            {
                case "Master":
                    {
                        if (e.Item is GridEditableItem editedItem)
                        {
                            try
                            {
                                _roleRepository.Remove((int)editedItem.GetDataKeyValue("MenuId"));
                                Helper.Notification(RadNotification1, "Remove is successful", "ok");
                            }
                            catch (Exception)
                            {
                                Helper.Notification(RadNotification1, "Remove is failed", "warning");
                            }
                        }
                    }
                    break;
            }
        }
    }
}