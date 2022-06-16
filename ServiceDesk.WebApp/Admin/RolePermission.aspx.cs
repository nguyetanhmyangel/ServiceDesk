using ServiceDesk.Data.Features.RolePermission;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ServiceDesk.WebApp.Admin
{
    public partial class RolePermission : Page
    {
        private readonly IAuthorityRepository _authorityRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;

        public RolePermission(IAuthorityRepository authorityRepository,
            IRolePermissionRepository rolePermissionRepository, IRoleRepository roleRepository)
        {
            _authorityRepository = authorityRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _roleRepository = roleRepository;
        }

        #region Property

        private int RoleId => !string.IsNullOrEmpty(rcbRole.SelectedValue) ?
            Helper.ConvertToInt(rcbRole.SelectedValue) : 0;

        #endregion Property

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!_authorityRepository.LoggedIn() && !_authorityRepository.IsAdmin())
                    Response.Redirect("~/Account/Login.aspx?returnUrl=" + Server.UrlEncode(Request.Url.AbsolutePath));
                AddRoleToCombo();
            }
        }

        private void AddRoleToCombo()
        {
            var list = _roleRepository.FindAll();
            rcbRole.Items.Clear();
            rcbRole.DataSource = list;
            rcbRole.DataBind();
        }

        protected void rcbRole_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            ((RadGrid)sender).DataSource = _rolePermissionRepository.FindByRoleId(RoleId);
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "UpdateCommandTemplate")
            {
                var result = true;
                for (var i = 0; i < RadGrid1.MasterTableView.Items.Count; i++)
                {
                    var chkAdd = (CheckBox)RadGrid1.MasterTableView.Items[i].Cells[2].FindControl("chkAdd");
                    var chkEdit = (CheckBox)RadGrid1.MasterTableView.Items[i].Cells[3].FindControl("chkEdit");
                    var chkDelete = (CheckBox)RadGrid1.MasterTableView.Items[i].Cells[4].FindControl("chkDelete");
                    var chkView = (CheckBox)RadGrid1.MasterTableView.Items[i].Cells[5].FindControl("chkView");
                    //var lbMenuId = (Label)RadGrid1.MasterTableView.Items[i].Cells[1].FindControl("lbMenuId");
                    if (chkAdd != null & chkEdit != null & chkDelete != null & chkView != null)
                    {
                        var canAdd = chkAdd.Checked ? Config.AllowAdd : Config.Default;
                        var canDelete = chkDelete.Checked ? Config.AllowDelete : Config.Default;
                        var canEdit = chkEdit.Checked ? Config.AllowEdit : Config.Default;
                        var canView = chkView.Checked ? Config.AllowView : Config.Default;

                        //GridDataItem item = (GridDataItem)e.Item;
                        var model = new RolePermissionCommand
                        {
                            MenuId = (int)RadGrid1.MasterTableView.Items[i].GetDataKeyValue("MenuId"),
                            RoleId = RoleId,
                            RolePermission = canAdd + canEdit + canDelete + canView
                        };
                        if (model.RolePermission < 2) continue;
                        if (_rolePermissionRepository.ChangePermission(model)) continue;
                        result = false;
                        break;
                    }
                }
                //RadGrid1.DataBind();
                if (result)
                    Helper.Notification(RadNotification1, "Change permission is successful", "ok");
                else
                    Helper.Notification(RadNotification1, "Change permission are corrupted", "warning");
            }
        }

        public bool CheckedAdd(int eval)
        {
            return (eval & Config.AllowAdd) == Config.AllowAdd;
        }

        public bool CheckedEdit(int eval)
        {
            return (eval & Config.AllowEdit) == Config.AllowEdit;
        }

        public bool CheckedDelete(int eval)
        {
            return (eval & Config.AllowDelete) == Config.AllowDelete;
        }

        public bool CheckedView(int eval)
        {
            return (eval & Config.AllowView) == Config.AllowView;
        }

        protected void chkAllAdd_CheckedChanged(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;
            var header = (GridHeaderItem)chk.NamingContainer;
            var chkAllAdd = (CheckBox)header.FindControl("chkAllAdd");
            if (chkAllAdd.Checked)
                foreach (GridDataItem grdRow in RadGrid1.MasterTableView.Items)
                {
                    var chkAdd =
                        (CheckBox)grdRow.FindControl("chkAdd");
                    chkAdd.Checked = true;
                }
            else
                foreach (GridDataItem grdRow in RadGrid1.MasterTableView.Items)
                {
                    var chkAdd = (CheckBox)grdRow.FindControl("chkAdd");
                    chkAdd.Checked = false;
                }
        }

        protected void chkAllEdit_CheckedChanged(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;
            var header = (GridHeaderItem)chk.NamingContainer;
            var chkAllEdit = (CheckBox)header.FindControl("chkAllEdit");
            if (chkAllEdit.Checked)
                foreach (GridDataItem grdRow in RadGrid1.MasterTableView.Items)
                {
                    var chkAdd =
                        (CheckBox)grdRow.FindControl("chkEdit");
                    chkAdd.Checked = true;
                }
            else
                foreach (GridDataItem grdRow in RadGrid1.MasterTableView.Items)
                {
                    var chkAdd = (CheckBox)grdRow.FindControl("chkEdit");
                    chkAdd.Checked = false;
                }
        }

        protected void chkAllDelete_CheckedChanged(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;
            var header = (GridHeaderItem)chk.NamingContainer;
            var chkAllDelete = (CheckBox)header.FindControl("chkAllDelete");
            if (chkAllDelete.Checked)
                foreach (GridDataItem grdRow in RadGrid1.MasterTableView.Items)
                {
                    var chkAdd =
                        (CheckBox)grdRow.FindControl("chkDelete");
                    chkAdd.Checked = true;
                }
            else
                foreach (GridDataItem grdRow in RadGrid1.MasterTableView.Items)
                {
                    var chkAdd = (CheckBox)grdRow.FindControl("chkDelete");
                    chkAdd.Checked = false;
                }
        }

        protected void chkAllView_CheckedChanged(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;
            var header = (GridHeaderItem)chk.NamingContainer;
            var chkAllView = (CheckBox)header.FindControl("chkAllView");
            if (chkAllView.Checked)
                foreach (GridDataItem grdRow in RadGrid1.MasterTableView.Items)
                {
                    var chkAdd =
                        (CheckBox)grdRow.FindControl("chkView");
                    chkAdd.Checked = true;
                }
            else
                foreach (GridDataItem grdRow in RadGrid1.MasterTableView.Items)
                {
                    var chkAdd = (CheckBox)grdRow.FindControl("chkView");
                    chkAdd.Checked = false;
                }
        }
    }
}