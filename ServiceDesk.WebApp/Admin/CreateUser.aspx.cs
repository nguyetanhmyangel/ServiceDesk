using ServiceDesk.Data.Features.Department;
using ServiceDesk.Data.Features.Position;
using ServiceDesk.Data.Features.Role;
using ServiceDesk.Data.Features.User;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using static System.String;

namespace ServiceDesk.WebApp.Admin
{
    public partial class CreateUser : Page
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthorityRepository _authorityRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IPositionRepository _positionRepository;

        public CreateUser(IUserRepository userRepository, IAuthorityRepository authorityRepository,
            IRoleRepository roleRepository,
            IDepartmentRepository departmentRepository, IPositionRepository positionRepository)
        {
            _userRepository = userRepository;
            _authorityRepository = authorityRepository;
            _roleRepository = roleRepository;
            _departmentRepository = departmentRepository;
            _positionRepository = positionRepository;
        }

        #region Property

        public string UserName
        {
            get => txtUserName.Text.Trim();
            set => txtUserName.Text = value;
        }

        //private int DivisionId => !IsNullOrEmpty(rcbDivision.SelectedValue) ?
        //    Helper.ConvertToInt(rcbDivision.SelectedValue) : 0;

        #endregion Property

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!_authorityRepository.LoggedIn() && !_authorityRepository.IsAdmin())
                    Response.Redirect("~/Account/Login.aspx?returnUrl=" + Server.UrlEncode(Request.Url.AbsolutePath));
                //LoadDivision();
            }
        }

        //private void LoadDivision()Request.Url.PathAndQuery
        //{
        //    var list = _divisionRepository.FindAll();
        //    rcbDivision.Items.Clear();
        //    rcbDivision.DataSource = list;
        //    rcbDivision.DataBind();
        //}

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            RadGrid1.Rebind();
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            ((RadGrid)sender).DataSource = _userRepository.FindByName(UserName);
        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            switch (e.Item.OwnerTableView.Name)
            {
                case "Master":
                    {
                        if (e.Item is GridDataItem)
                        {
                            // Limited text
                            Helper.TextLimit("lbFullName", 20, e);
                            Helper.TextLimit("lbDivisionName", 20, e);
                            Helper.TextLimit("lbDepartmentName", 20, e);
                        }
                        //edit mode
                        if (e.Item is GridEditFormItem item && e.Item.IsInEditMode)
                        {
                            var editItem = (GridEditableItem)e.Item;
                            //Session["divisionId"] = ((RadComboBox)editItem.FindControl("rcbDivision")).SelectedValue;
                            if (!item.OwnerTableView.IsItemInserted)
                            {
                                //var userId = (int)item.GetDataKeyValue("UserId");

                                var departmentId = Helper.ConvertToInt(editItem["DepartmentId"].Text);
                                var departmentName = editItem["DepartmentName"].Text;
                                var rcbDepartment = (RadComboBox)item.FindControl("rcbDepartment");
                                Helper.SetComboBoxValue(rcbDepartment, departmentName, departmentId);

                                var roleId = Helper.ConvertToInt(editItem["RoleId"].Text);
                                var roleName = editItem["RoleName"].Text;
                                var rcbRole = (RadComboBox)item.FindControl("rcbRole");
                                Helper.SetComboBoxValue(rcbRole, roleName, roleId);

                                var positionId = Helper.ConvertToInt(editItem["PositionId"].Text);
                                var positionName = editItem["PositionName"].Text;
                                var rcbPosition = (RadComboBox)item.FindControl("rcbPosition");
                                Helper.SetComboBoxValue(rcbPosition, positionName, positionId);

                                var userName = (RadTextBox)item.FindControl("UserName");
                                userName.ReadOnly = true;
                            }
                            else
                            {
                                var active = (CheckBox)item.FindControl("Active");
                                active.Checked = true;

                                var txtEmail = (RadTextBox)item.FindControl("Email");
                                txtEmail.Visible = false;

                                var lbEmail = (HtmlControl)item.FindControl("lbEmail");
                                lbEmail.Visible = false;

                                var txtPassword = (RadTextBox)item.FindControl("Password");
                                txtPassword.Visible = false;

                                var lbPassword = (HtmlControl)item.FindControl("lbPassword");
                                lbPassword.Visible = false;

                                var rcbDepartment = (RadComboBox)item.FindControl("rcbDepartment");
                                rcbDepartment.Visible = false;

                                var lbDepartment = (HtmlControl)item.FindControl("lbDepartment");
                                lbDepartment.Visible = false;
                            }
                        }
                    }
                    break;
            }
        }

        protected void rcbRole_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            var list = _roleRepository.FindAll();

            if (!IsNullOrEmpty(e.Text))
            {
                list = list.Where(i => i.RoleName.Contains(e.Text.Trim()))
                  .OrderBy(i => i.RoleId).Skip(e.NumberOfItems).Take(10);
            }
            var comboBox = (RadComboBox)sender;
            comboBox.DataSource = list;
            comboBox.DataBind();

            var roleResponse = list as RoleResponse[] ?? list.ToArray();
            var endOffset = e.NumberOfItems + roleResponse.Count();
            var totalCount = roleResponse.Count();

            if (endOffset == totalCount)
                e.EndOfItems = true;

            e.Message = $"Items <b>1</b>-<b>{endOffset}</b> out of <b>{totalCount}</b>";
        }

        protected void rcbPosition_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            var list = _positionRepository.FindAll();

            if (!IsNullOrEmpty(e.Text))
            {
                list = list.Where(i => i.PositionName.Contains(e.Text.Trim()))
                  .OrderBy(i => i.Id).Skip(e.NumberOfItems).Take(10);
            }
            var comboBox = (RadComboBox)sender;
            comboBox.DataSource = list;
            comboBox.DataBind();

            var positionResponse = list as PositionResponse[] ?? list.ToArray();
            var endOffset = e.NumberOfItems + positionResponse.Count();
            var totalCount = positionResponse.Count();

            if (endOffset == totalCount)
                e.EndOfItems = true;

            e.Message = $"Items <b>1</b>-<b>{endOffset}</b> out of <b>{totalCount}</b>";
        }

        protected void rcbDepartment_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            //GridEditableItem editItem = (GridEditableItem)(sender as RadComboBox)?.NamingContainer;
            //if (editItem != null)
            //{
            //var divisionId = Helper.ConvertToInt(((RadComboBox)editItem.FindControl("rcbDivision")).SelectedValue);

            var list = _departmentRepository.FindByDivisionId();
            if (!IsNullOrEmpty(e.Text))
            {
                list = list.Where(i => i.DepartmentName.Contains(e.Text.Trim()))
                    .OrderBy(i => i.DepartmentId).Skip(e.NumberOfItems).Take(10).ToList();
            }
            var comboBox = (RadComboBox)sender;
            comboBox.Items.Clear();
            comboBox.DataSource = list;
            comboBox.DataBind();

            var departmentResponse = list as DepartmentResponse[] ?? list.ToArray();
            var endOffset = e.NumberOfItems + departmentResponse.Count();
            var totalCount = departmentResponse.Count();

            if (endOffset == totalCount)
                e.EndOfItems = true;

            e.Message = $"Items <b>1</b>-<b>{endOffset}</b> out of <b>{totalCount}</b>";

            //}
        }

        protected void RadGrid1_InsertCommand(object source, GridCommandEventArgs e)
        {
            switch (e.Item.OwnerTableView.Name)
            {
                case "Master":
                    {
                        //var userName =
                        //    var user = HrFindByUserName(loginParameters);

                        if (e.Item is GridEditableItem editedItem)
                        {
                            var userName = ((RadTextBox)editedItem.FindControl("UserName")).Text.Trim();
                            var employees = _authorityRepository.EmployeList(userName);
                            if (employees != null)
                            {
                                var userCommand = new UserCommand();
                                foreach (var employee in employees)
                                {
                                    userCommand.FullName = employee.EmployeeName ;
                                    userCommand.UserName = employee.EmployeeId;
                                    userCommand.DepartmentId = employee.DepartmentId;
                                    userCommand.Email = employee.Email ?? "Unknown";
                                    userCommand.Password = Helper.Encrypt(employee.EmployeeId);
                                }
                                //userCommand.Email = ((RadTextBox)editedItem.FindControl("Email")).Text.Trim();
                                userCommand.RoleId = Helper.ConvertToInt(((RadComboBox)editedItem.FindControl("rcbRole")).SelectedValue);
                                userCommand.Active = true;
                                userCommand.PositionId = Helper.ConvertToInt(((RadComboBox)editedItem.FindControl("rcbPosition")).SelectedValue);

                                try
                                {
                                    _userRepository.Add(userCommand);
                                    Helper.Notification(RadNotification1, "Insert is successful", "ok");
                                }
                                catch (Exception ex)
                                {
                                    Helper.Notification(RadNotification1, "Insert is failed. Error: " + ex.Message, "warning");
                                }
                            }
                            else
                                Helper.Notification(RadNotification1, "User not exist.Insert is failed", "warning");
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
                        if (e.Item is GridEditableItem editedItem)
                        {
                            var passWord = ((RadTextBox)editedItem.FindControl("Password")).Text.Trim();
                            var oldPassWord = ((HiddenField)editedItem.FindControl("hdPassword")).Value.Trim();
                            var userCommand = new UserCommand
                            {
                                UserId = (int)editedItem.GetDataKeyValue("UserId"),
                                UserName = ((RadTextBox)editedItem.FindControl("UserName")).Text.Trim(),
                                RoleId = Helper.ConvertToInt(((RadComboBox)editedItem.FindControl("rcbRole")).SelectedValue),
                                DepartmentId = Helper.ConvertToInt(((RadComboBox)editedItem.FindControl("rcbDepartment")).SelectedValue),
                                Email = ((RadTextBox)editedItem.FindControl("Email")).Text.Trim(),
                                Active = ((CheckBox)editedItem.FindControl("Active")).Checked,
                                PositionId = Helper.ConvertToInt(((RadComboBox)editedItem.FindControl("rcbPosition")).SelectedValue),
                                Password = !IsNullOrEmpty(passWord)? Helper.Encrypt(passWord): oldPassWord
                            };
                            try
                            {
                                _userRepository.Update(userCommand);
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
                                _userRepository.Remove((int)editedItem.GetDataKeyValue("UserId"));
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