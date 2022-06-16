using ServiceDesk.Data.Features.Task;
using ServiceDesk.Data.Features.User;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using ServiceDesk.WebApp.Culture;
using System;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ServiceDesk.WebApp.Issues
{
    public partial class UserList : LanguagePage
    {
        private readonly IAuthorityRepository _authorityRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public UserList(IAuthorityRepository authorityRepository, IDepartmentRepository departmentRepository,
            IEmployeeRepository employeeRepository)
        {
            _authorityRepository = authorityRepository;
            _departmentRepository = departmentRepository; 
            _employeeRepository = employeeRepository;
        }

        #region Property

        private int DepartmentId => !string.IsNullOrEmpty(rcbDepartment.SelectedValue) ?
            Helper.ConvertToInt(rcbDepartment.SelectedValue) : -1;


        #endregion Property

        protected void Page_Load(object sender, EventArgs e)
        {
            _authorityRepository.LoadPrivilege();
            if (!IsPostBack)
            {
                if (!_authorityRepository.LoggedIn() && !_authorityRepository.CanView)
                    Response.Redirect("~/Account/Login.aspx?returnUrl=" + Server.UrlEncode(Request.Url.AbsolutePath));
                InitDepartmentCombobox();
                //RadGrid1.MasterTableView.Items[0].Expanded = true;
            }
        }

        private void InitDepartmentCombobox()
        {
            var roleId = Claim.Session[Config.RoleId] != null ? Helper.ConvertToInt(Claim.Session[Config.RoleId].ToString()) : 0;
            var departmentId = Helper.ConvertToInt(Claim.Session[Config.DepartmentId]);
            rcbDepartment.Items.Clear();
            rcbDepartment.DataSource = _departmentRepository.FindByUser(roleId, departmentId);
            if (roleId != 4)
            {
                rcbDepartment.AllowCustomText = true;
                //rcbDepartment.EmptyMessage = "Chọn một phòng ban";
            }
            else
                rcbDepartment.SelectedValue = Claim.Session[Config.DepartmentId].ToString();
            rcbDepartment.DataBind();
        }

        protected void rcbDepartment_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            RadGrid1.Rebind();
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!e.IsFromDetailTable)
            {
                var roleId = Claim.Session[Config.RoleId] != null ? Helper.ConvertToInt(Claim.Session[Config.RoleId].ToString()) : 0;
                ((RadGrid)sender).DataSource = _employeeRepository.FindByEmployeeExecuting(DepartmentId,roleId);
            }
        }

        protected void RadGrid1_DetailTableDataBind(object source, GridDetailTableDataBindEventArgs e)
        {
            var dataItem = e.DetailTableView.ParentItem;
            switch (e.DetailTableView.Name)
            {
                case "Detail":
                    {
                        var userId = Helper.ConvertToInt(dataItem.GetDataKeyValue("UserId").ToString());
                        e.DetailTableView.DataSource = _employeeRepository.FindByDetail(userId);
                        break;
                    }
            }
        }
    }
}