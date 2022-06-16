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
    public partial class HandleIssue : LanguagePage
    {
        private readonly IAuthorityRepository _authorityRepository;
        private readonly ITaskRepository _departmentIssuesRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDivisionRepository _divisionRepository;
        private readonly ISendMailRepository _sendMailRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly ITaskExecuteRepository _taskRepository;

        public HandleIssue(IAuthorityRepository authorityRepository, IStatusRepository statusRepository,
            IDepartmentRepository departmentRepository, ISendMailRepository sendMailRepository,
            ITaskRepository departmentIssuesRepository, IUserRepository userRepository,
            IDivisionRepository divisionRepository, ITaskExecuteRepository taskRepository)
        {
            _authorityRepository = authorityRepository;
            _departmentRepository = departmentRepository;
            _sendMailRepository = sendMailRepository;
            _userRepository = userRepository;
            _departmentIssuesRepository = departmentIssuesRepository;
            _statusRepository = statusRepository;
            _divisionRepository = divisionRepository;
            _taskRepository = taskRepository;
        }

        #region Property

        public DateTime FromDate => Convert.ToDateTime(rdpFromDate.SelectedDate);
        public DateTime ToDate => Convert.ToDateTime(rdpToDate.SelectedDate);

        private int DivisionId => !string.IsNullOrEmpty(rcbDivision.SelectedValue) ?
            Helper.ConvertToInt(rcbDivision.SelectedValue) : -1;

        private int StatusId => !string.IsNullOrEmpty(rcbStatus.SelectedValue) ?
            Helper.ConvertToInt(rcbStatus.SelectedValue) : -1;

        #endregion Property

        protected void Page_Load(object sender, EventArgs e)
        {
            _authorityRepository.LoadPrivilege();
            if (!IsPostBack)
            {
                if (!_authorityRepository.LoggedIn() && !_authorityRepository.CanView)
                    Response.Redirect("~/Account/Login.aspx?returnUrl=" + Server.UrlEncode(Request.Url.AbsoluteUri));
                rdpFromDate.SelectedDate = DateTime.Today.AddDays(-15);
                rdpToDate.SelectedDate = DateTime.Now;
                InitStatusCombobox();
                InitDivisionCombobox();
                //RadGrid1.MasterTableView.Items[0].Expanded = true;
            }
        }

        private void InitDivisionCombobox()
        {
            var list = _divisionRepository.FindAll();
            rcbDivision.Items.Clear();
            rcbDivision.DataSource = list;
            rcbDivision.DataBind();
        }

        private void InitStatusCombobox()
        {
            var list = _statusRepository.FindAll();
            rcbStatus.Items.Clear();
            rcbStatus.DataSource = list;
            rcbStatus.DataBind();
        }

        //private void AddDepartmentToCombo()
        //{
        //    var userId = Claim.Session[Config.UserId] != null ? Claim.Session[Config.UserId].ToString() : string.Empty;
        //    var userDepartmentId =  Helper.ConvertToInt(Claim.Session[Config.DepartmentId]) ;
        //    var canAllView =  _userRepository.DepartmentView(userId);
        //    rcbDepartment.Items.Clear();
        //    if (!canAllView)
        //    {
        //        rcbDepartment.DataSource = _departmentRepository.FindById(userDepartmentId);
        //        rcbDepartment.SelectedValue = Claim.Session[Config.DepartmentId].ToString();
        //    }
        //    else
        //        rcbDepartment.DataSource = _departmentRepository.FindAllIT();
        //    rcbDepartment.DataBind();
        //}

        protected void rcbStatus_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }

        protected void rcbDivision_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
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
                var languageId = Claim.Session[Config.LanguageId] != null ? Claim.Session[Config.LanguageId].ToString() : "vi-VN";
                var departmentId = 0;
                if (Claim.Session[Config.RoleId] != null)
                {
                    departmentId = Helper.ConvertToInt(Claim.Session[Config.DepartmentId]);
                }
                ((RadGrid)sender).DataSource = _departmentIssuesRepository.FindByDepartmentId(departmentId, StatusId,  FromDate, ToDate);
            }
        }

        protected void RadGrid1_DetailTableDataBind(object source, GridDetailTableDataBindEventArgs e)
        {
            var dataItem = (GridDataItem)e.DetailTableView.ParentItem;
            switch (e.DetailTableView.Name)
            {
                case "Detail":
                    {
                        var departmentIssueId = Helper.ConvertToInt(dataItem.GetDataKeyValue("Id").ToString());
                        e.DetailTableView.DataSource = _taskRepository.FindByTaskId(departmentIssueId);
                        break;
                    }
            }
        }

        //protected void RadGrid1_PreRender(object sender, EventArgs e)
        //{
        //    if (!Page.IsPostBack)
        //    {
        //        // Child level 1
        //        RadGrid1.MasterTableView.Items[0].Expanded = RadGrid1.MasterTableView.Items[0].HasChildItems;

        //        // Child level 2
        //        //RadGrid1.MasterTableView.Items[0].ChildItem.NestedTableViews[0].Items[0].Expanded = true;
        //    }
        //}

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            switch (e.Item.OwnerTableView.Name)
            {
                case "Master":
                    {
                        if (e.Item is GridDataItem dataItem)
                        {
                            // Limited text
                            Helper.TextLimit("lbRequestDepartmentName", 20, e);
                            Helper.TextLimit("lbRequestDivisionName", 20, e);
                            Helper.TextLimit("lbRequestDescription", 20, e);
                            Helper.TextLimit("lbRequestEmployeeName", 20, e);

                            // set css for status
                            var label = (Label)dataItem.FindControl("lbStatus");
                            switch (dataItem["StatusId"].Text)
                            {
                                case "1":
                                    if (label != null) label.CssClass = "waiting";
                                    break;

                                case "2":
                                    if (label != null) label.CssClass = "processing";
                                    break;

                                case "3":
                                    if (label != null) label.CssClass = "finish";
                                    break;

                                case "4":
                                    if (label != null) label.CssClass = "fausing";
                                    break;

                                case "5":
                                    if (label != null) label.CssClass = "cancel";
                                    break;
                            }
                        }
                        //edit mode
                        if (e.Item is GridEditFormItem item && e.Item.IsInEditMode)
                        {
                            var editItem = (GridEditableItem)e.Item;
                            // Load combobox
                            var status = (RadComboBox)e.Item.FindControl("Status");
                            status.DataSource = _statusRepository.FindAll();
                            status.DataBind();
                            // Load MultiSelect
                            var departmentId = Helper.ConvertToInt(editItem["DepartmentId"].Text);//access boundcolumn
                            var users = (RadMultiSelect)e.Item.FindControl("Users");
                            users.DataSource = _userRepository.FindUserByDepartmentId(departmentId);
                            users.DataBind();

                            // Edit mode
                            if (!item.OwnerTableView.IsItemInserted)
                            {
                                var departmentIssueId = Helper.ConvertToInt(item.GetDataKeyValue("Id").ToString());
                                // set value MultiSelect
                                var userValues = _userRepository.FindUserByTaskId(departmentIssueId);
                                var userResponse = userValues as UserResponse[] ?? userValues.ToArray();
                                if (userResponse.Any())
                                {
                                    users.Value = userResponse.ToList();
                                }
                            }
                            //else
                            //{
                            //var Active = (CheckBox)item.FindControl("Active");
                            //Active.Checked = true;
                            //}
                        }
                    }
                    break;

                case "Detail":
                    {
                        if (e.Item is GridDataItem eItem)
                        {
                            // Limited text
                            Helper.TextLimit("lbDetailDescription", 100, e);
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
                            var createUserId = Claim.Session[Config.UserId] != null ? Claim.Session[Config.UserId].ToString() : string.Empty;
                            var model = new TaskCommand()
                            {
                                Id = (int)editedItem.GetDataKeyValue("Id"),
                                StatusId = !string.IsNullOrEmpty(((RadComboBox)editedItem.FindControl("Status")).SelectedValue) ?
                                    Helper.ConvertToInt(((RadComboBox)editedItem.FindControl("Status")).SelectedValue) : 2,
                                StartDate = ((RadDatePicker)editedItem.FindControl("StartDate")).SelectedDate,
                                EndDate = ((RadDatePicker)editedItem.FindControl("EndDate")).SelectedDate,
                                Description = ((RadTextBox)editedItem.FindControl("Description")).Text.Trim()
                            };
                            var multiSelect = (RadMultiSelect)editedItem.FindControl("Users");
                            if (multiSelect.Value.Any())
                            {
                                if (_departmentIssuesRepository.Update(createUserId, model, multiSelect.Value))
                                {
                                    //send mail
                                    try
                                    {
                                        // get mail of user of issue
                                        var listSendMail = _userRepository.FindPersonnelEmail(model.Id);

                                        const string subject = "Xử lí yêu cầu";
                                        var content = "Một yêu cầu đã được gửi tới ông/bà.</br>";
                                        content += "Đăng nhập vào chương trình <strong><a href='#'>It Help Desk</a></strong> để xử lí.</br></br>";
                                        content += "<i>Vui lòng không trả lời Mail này!</i>";

                                        _sendMailRepository.SendEmail(listSendMail, subject, content);
                                    }
                                    catch (Exception ex)
                                    {
                                        //
                                    }

                                    Helper.Notification(RadNotification1, "Update is successful", "ok");
                                }
                                else
                                    Helper.Notification(RadNotification1, "Update is failed.", "warning");
                            }
                        }
                    }
                    break;
            }
        }
    }
}