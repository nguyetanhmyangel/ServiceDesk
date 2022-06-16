using ServiceDesk.Data.Features.Task;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using ServiceDesk.WebApp.Culture;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.Calendar;
using Image = System.Web.UI.WebControls.Image;


namespace ServiceDesk.WebApp.Issues
{
    public partial class DepartmentTask : LanguagePage
    {
        private readonly IAuthorityRepository _authorityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISendMailService _sendMailService;
        private readonly IStatusRepository _statusRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IPriorityRepository _priorityRepository;
        private readonly ITaskExecuteRepository _taskExecuteRepository;

        public DepartmentTask(IAuthorityRepository authorityRepository, IStatusRepository statusRepository,
            ISendMailService sendMailService, IDepartmentRepository departmentRepository,
            IUserRepository userRepository, ITaskRepository taskRepository, ITransferTaskRepository transferTaskRepository,
            ITaskExecuteRepository taskExecuteRepository, IPriorityRepository priorityRepository)
        {
            _authorityRepository = authorityRepository;
            _sendMailService = sendMailService;
            _userRepository = userRepository;
            _statusRepository = statusRepository;
            _departmentRepository = departmentRepository;
            _taskRepository = taskRepository;
            _priorityRepository = priorityRepository;
            _taskExecuteRepository = taskExecuteRepository;
        }

        #region Property

        public DateTime FromDate => Convert.ToDateTime(rdpFromDate.SelectedDate);
        public DateTime ToDate => Convert.ToDateTime(rdpToDate.SelectedDate);

        private int StatusId => !string.IsNullOrEmpty(rcbStatus.SelectedValue) ?
            Helper.ConvertToInt(rcbStatus.SelectedValue) : -1;

        private int DepartmentId => !string.IsNullOrEmpty(rcbDepartment.SelectedValue) ?
            Helper.ConvertToInt(rcbDepartment.SelectedValue) : -1;

        private string DepartmentName => !string.IsNullOrEmpty(rcbDepartment.SelectedValue) ?
            " - " + rcbDepartment.Text : string.Empty;

        private int UserDepartmentId => Helper.ConvertToInt(Claim.Session[Config.DepartmentId]);

        private int RoleId => Helper.ConvertToInt(Claim.Session[Config.RoleId]);

        #endregion Property

        protected void Page_Load(object sender, EventArgs e)
        {
            _authorityRepository.LoadPrivilege();
            if (!IsPostBack)
            {
                if (!_authorityRepository.LoggedIn() && !_authorityRepository.CanView)
                    Response.Redirect("~/Account/Login.aspx?returnUrl=" + Server.UrlEncode(Request.Url.AbsolutePath));
                rdpFromDate.SelectedDate = DateTime.Today.AddDays(-90);
                rdpToDate.SelectedDate = DateTime.Now;
                InitStatusCombobox();
                InitDepartmentCombo();
                Session["TaskId"] = "0";
                //RadGrid1.MasterTableView.Items[0].Expanded = true;
            }
        }

        private void InitStatusCombobox()
        {
            var list = _statusRepository.FindAll();
            rcbStatus.Items.Clear();
            rcbStatus.DataSource = list;
            rcbStatus.DataBind();
        }

        private void InitDepartmentCombo()
        {
            rcbDepartment.Items.Clear();
            rcbDepartment.DataSource = _departmentRepository.FindByUser(RoleId, UserDepartmentId);
            if (RoleId == -1 )
            {
                rcbDepartment.AllowCustomText = false;
                rcbDepartment.SelectedValue = Claim.Session[Config.DepartmentId].ToString();
            }
            else
            {
                //rcbDepartment.AllowCustomText = false;
                Helper.SetControlReadOnly(rcbDepartment);
                rcbDepartment.SelectedValue = Claim.Session[Config.DepartmentId].ToString();
            }

            rcbDepartment.DataBind();
        }


        protected void rcbStatus_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }

        protected void rcbDepartment_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            RadGrid1.Rebind();
        }

        protected void FromDateSelectedDateChange(object sender, SelectedDateChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }

        protected void ToDateSelectedDateChange(object sender, SelectedDateChangedEventArgs e)
        {
            RadGrid1.Rebind();
        }

        public string CustomizeEval()
        {
            return DepartmentName;
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!e.IsFromDetailTable)
            {
                //var roleId = Claim.Session[Config.RoleId] != null ? Helper.ConvertToInt(Claim.Session[Config.RoleId].ToString()) : 0;
                ((RadGrid)sender).DataSource = _taskRepository.FindByDepartmentId(DepartmentId,StatusId, FromDate, ToDate);
                //RadGrid1.MasterTableView.Items[0].Expanded = true;
            }
        }

        protected void RadGrid1_DetailTableDataBind(object source, GridDetailTableDataBindEventArgs e)
        {
            var dataItem = e.DetailTableView.ParentItem;
            switch (e.DetailTableView.Name)
            {
                case "Detail":
                    {
                        var issueId = Helper.ConvertToInt(dataItem.GetDataKeyValue("Id").ToString());
                        e.DetailTableView.DataSource = _taskRepository.FindById(issueId);
                        break;
                    }
                case "ChildDetail":
                    {
                        var taskId = Helper.ConvertToInt(dataItem.GetDataKeyValue("Id").ToString());
                        e.DetailTableView.DataSource = _taskExecuteRepository.FindByTaskId(taskId);
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

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName != "DownloadCommandColumn") return;
            {
                var item = e.Item as GridDataItem;
                //if (item != null)
                //{
                //    var issueId = item.GetDataKeyValue("Id").ToString();
                //}
                if (item == null) return;
                var documentName = item["DispatchPath"].Text.Replace("&nbsp;", "").Trim();
                if (!string.IsNullOrEmpty(documentName))
                {
                    var target = HttpContext.Current.Server.MapPath("~/DocumentFiles/");
                    var fullPath = Path.Combine(target, documentName);
                    if (!File.Exists(fullPath)) return;
                    var binaryData = File.ReadAllBytes(fullPath);
                    Response.Clear();
                    Response.ContentType = "application/octet-stream";
                    Response.AddHeader("content-disposition", "attachment; filename=" + documentName);
                    Response.BinaryWrite(binaryData);
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    RadAjaxManager1.Alert("Không tồn tại file");
                }
            }
        }

        // set attribute for user multi select
        protected void Users_ItemDataBound(object sender, RadMultiSelectItemEventArgs e)
        {
            //e.Item.Attributes.Add("CountExecuting", "Attribute " + e.Item.Value);
            //var select = (RadMultiSelect)sender;
            var item = (GridEditableItem)((RadMultiSelect)sender).NamingContainer;
            var departmentId = item["DepartmentId"].Text != null ? Helper.ConvertToInt(item["DepartmentId"].Text.Trim()) : 0;
            var userList = _userRepository.FindExecutingList(departmentId).ToList();
            var userId = e.Item.Value != null ? Helper.ConvertToInt(e.Item.Value) : 0;

            var attributeValue = userList.Where(u => u.UserId == userId).Select(c => c.CountExecuting).FirstOrDefault();

            e.Item.Attributes.Add("CountExecuting", attributeValue.ToString());
            //e.Item.Attributes.Add("CountExecuting", _userRepository.FindCountExecuting(userId).ToString());
        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            switch (e.Item.OwnerTableView.Name)
            {
                case "Master":
                    {
                        if (e.Item is GridDataItem dataItem)
                        {
                            var priorityId = Helper.ConvertToInt(((HiddenField)dataItem.FindControl("hdPriorityId")).Value);
                            if (priorityId > 1)
                            {
                                var img = (Image)dataItem.FindControl("imgFlag");
                                img.ImageUrl = "~/Images/RedFlag.PNG";
                            }

                            // Limited text
                            //Helper.TextLimit("lbRequireDivisionName", 30, e);
                            //Helper.TextLimit("lbRequireDescription", 20, e);
                            Helper.TextLimit("lbCustomerDescription", 20, e);
                            Helper.TextLimit("lbRequireEmployeeName", 20, e);
                            //Helper.TextLimit("lbDepartmentName", 14, e);
                            Helper.TextLimit("lbTitle", 20, e);

                            // set css for status
                            var label = (Label)dataItem.FindControl("lbStatus");
                            switch (Helper.ConvertToInt(dataItem["IssueStatusId"].Text))
                            {
                                case Config.Waiting:
                                    if (label != null) label.CssClass = "waiting";
                                    break;

                                case Config.Processing:
                                    if (label != null) label.CssClass = "processing";
                                    break;

                                case Config.Complete:
                                    if (label != null) label.CssClass = "Complete";
                                    break;

                                case Config.Pausing:
                                    if (label != null) label.CssClass = "fausing";
                                    break;

                                case Config.Cancel:
                                    if (label != null) label.CssClass = "cancel";
                                    break;

                                case Config.Transfer:
                                    if (label != null) label.CssClass = "transfer";
                                    break;
                            }

                            //var lbDepartmentExcute = (Label)dataItem.FindControl("lbDepartmentExcute");
                            //switch (Helper.ConvertToInt(dataItem["DepartmentId"].Text))
                            //{
                            //    case Config.RepairDepartment:
                            //        if (lbDepartmentExcute != null) lbDepartmentExcute.Text = "SC";
                            //        break;

                            //    case Config.NetworkDepartment:
                            //        if (lbDepartmentExcute != null) lbDepartmentExcute.Text = "KTM";
                            //        break;

                            //    case Config.SoftwareDepartment:
                            //        if (lbDepartmentExcute != null) lbDepartmentExcute.Text = "PM";
                            //        break;

                            //    default:
                            //        lbDepartmentExcute.Text = string.Empty;
                            //        break;
                            //}
                        }
                        //edit and create mode
                        if (e.Item is GridEditFormItem item && e.Item.IsInEditMode)
                        {
                            var editItem = (GridEditableItem)e.Item;
                            // Load combobox
                            var status = (RadComboBox)e.Item.FindControl("Status");
                            status.DataSource = _statusRepository.FindAll();
                            status.DataBind();

                            //
                            var priority = (RadComboBox)e.Item.FindControl("Priority");
                            priority.DataSource = _priorityRepository.FindAll();
                            priority.DataBind();

                            //var departmentId = Helper.ConvertToInt(editItem["DepartmentId"].Text) = 0 ? Helper.ConvertToInt(Claim.Session[Config.DepartmentId]) : Helper.ConvertToInt(editItem["DepartmentId"].Text);

                            //var departmentId = Helper.ConvertToInt(editItem["DepartmentId"].Text) > 0
                            //    ? Helper.ConvertToInt(editItem["DepartmentId"].Text)
                            //    : UserDepartmentId;

                            // Load Users Multi Select, if is Admin then load all user else only load user of department
                            var userCombo = (RadMultiSelect)e.Item.FindControl("Users");
                            var userList = _userRepository.FindUserByDepartmentId(UserDepartmentId);
                            userCombo.DataSource = userList;
                            userCombo.DataBind();

                            // Load Departments Multi Select
                            var departmentCombo = (RadMultiSelect)e.Item.FindControl("OtherDepartments");
                            var departmentList = _departmentRepository.FindByDivisionId(Config.InfoTechDivision);
                            departmentCombo.DataSource = departmentList;
                            departmentCombo.DataBind();

                            var statusId = (HiddenField)editItem.FindControl("hdStatusId");
                            var statusName = (HiddenField)editItem.FindControl("hdStatusName");
                            var statusCombo = (RadComboBox)editItem.FindControl("Status");

                            var priorityId = (HiddenField)editItem.FindControl("hdPriorityId");
                            var priorityName = (HiddenField)editItem.FindControl("hdPriorityName");
                            var priorityComboBox = (RadComboBox)editItem.FindControl("Priority");

                            var txtDescription = (RadTextBox)editItem.FindControl("Description");

                            var commandButton = (RadButton)editItem.FindControl("btnUpdate");

                            var startPicker = (RadDatePicker)editItem.FindControl("StartDate");
                            var endPicker = (RadDatePicker)editItem.FindControl("EndDate");

                            // Edit mode
                            if (!item.OwnerTableView.IsItemInserted)
                            {
                                var issueId = Helper.ConvertToInt(item.GetDataKeyValue("Id").ToString());

                                // set value Departments MultiSelect
                                var departmentValues = _departmentRepository.FindByIssueId(issueId).ToList();
                                if (departmentValues.Any())
                                {
                                    departmentCombo.Value = departmentValues;
                                }

                                // set value Users MultiSelect
                                var userValues = _userRepository.FindUserByIssueId(issueId, UserDepartmentId).ToList();

                                if (userValues.Any())
                                {
                                    userCombo.Value = userValues;
                                }

                                // set value statusCombo
                                statusCombo.SelectedValue = statusId.Value;
                                statusCombo.Text = statusName.Value;

                                // set value priorityComboBox
                                priorityComboBox.SelectedValue = priorityId.Value;
                                priorityComboBox.Text = priorityName.Value;

                                // set attribute readonly
                                if (Helper.ConvertToInt(statusId.Value) == Config.Cancel ||
                                    Helper.ConvertToInt(statusId.Value) == Config.Complete)
                                {
                                    Helper.SetControlReadOnly(priorityComboBox);
                                    Helper.SetControlReadOnly(statusCombo);
                                    Helper.SetControlReadOnly(departmentCombo);
                                    Helper.SetControlReadOnly(txtDescription);
                                    Helper.SetControlReadOnly(startPicker);
                                    Helper.SetControlReadOnly(endPicker);
                                    Helper.SetControlReadOnly(userCombo);
                                    Helper.SetControlReadOnly(commandButton);
                                }


                                //if (expr)
                                //{
                                //    var txtDescription = (RadTextBox)editItem.FindControl("Description");
                                //}
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
                            Helper.TextLimit("lbDetailDescription", 400, e);
                            // set css for status
                            var label = (Label)eItem.FindControl("lbDetailStatus");
                            switch (Helper.ConvertToInt(eItem["DetailStatusId"].Text))
                            {
                                case Config.Waiting:
                                    if (label != null) label.CssClass = "waiting";
                                    break;

                                case Config.Processing:
                                    if (label != null) label.CssClass = "processing";
                                    break;

                                case Config.Complete:
                                    if (label != null) label.CssClass = "Complete";
                                    break;

                                case Config.Pausing:
                                    if (label != null) label.CssClass = "fausing";
                                    break;

                                case Config.Cancel:
                                    if (label != null) label.CssClass = "cancel";
                                    break;

                                case Config.Transfer:
                                    if (label != null) label.CssClass = "transfer";
                                    break;
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
                        if (e.Item is GridEditableItem editedItem)
                        {
                            var model = new TaskCommand()
                            {
                                //Id = (int)editedItem.GetDataKeyValue("Id"),
                                StatusId = !string.IsNullOrEmpty(((RadComboBox)editedItem.FindControl("Status")).SelectedValue) ?
                                            Helper.ConvertToInt(((RadComboBox)editedItem.FindControl("Status")).SelectedValue) : Config.Processing,
                                Description = ((RadTextBox)editedItem.FindControl("Description")).Text.Trim(),
                                PriorityId = !string.IsNullOrEmpty(((RadComboBox)editedItem.FindControl("Priority")).SelectedValue) ?
                                            Helper.ConvertToInt(((RadComboBox)editedItem.FindControl("Priority")).SelectedValue) : 1,
                                //IssueId = Helper.ConvertToInt(((HiddenField)editedItem.FindControl("hdIssueId")).Value),
                                IssueId = (int)editedItem.GetDataKeyValue("Id"),
                                TaskId = Helper.ConvertToInt(((HiddenField)editedItem.FindControl("hdTaskId")).Value),
                                DepartmentId = DepartmentId,
                                StartDate = ((RadDatePicker)editedItem.FindControl("StartDate")).SelectedDate,
                                EndDate = ((RadDatePicker)editedItem.FindControl("EndDate")).SelectedDate,
                                CreateUser = Claim.Session[Config.UserId] != null ? Claim.Session[Config.UserId].ToString() : string.Empty,
                                UpdateUser = Claim.Session[Config.UserId] != null ? Claim.Session[Config.UserId].ToString() : string.Empty
                            };

                            var title = (RadTextBox)editedItem.FindControl("Title");

                            // Access the MultiSelect
                            //var departmentCombo = editedItem.FindControl("OtherDepartments") as RadMultiSelect;
                            var multiUsers = (RadMultiSelect)editedItem.FindControl("Users");
                            var departmentCombo = (RadMultiSelect)editedItem.FindControl("OtherDepartments");

                            if (_taskRepository.Update(model, departmentCombo.Value, multiUsers.Value))
                            {
                                #region Send mail

                                const string subject = "Xử lí yêu cầu";
                                var content = "Một yêu cầu tên: <strong>" + title.Text + "</strong> đã được gửi tới ông/bà.</br>";
                                content += "Vui lòng ăng nhập vào chương trình <strong><a href='support.vietsov.com.vn'>It Support</a></strong> để xử lí.</br></br>";
                                content += "<i>Không trả lời lại Mail này!</i>";

                                var leaderEmailList = "";
                                var personEmailList = "";
                                var userIds = "";
                                var departmentIds = "";

                                // get User 
                                if (multiUsers.Value.Any())
                                {
                                    // get mail of user of issue
                                    userIds = multiUsers.Value.Aggregate(userIds, (current, item) => current + (item + ","));
                                    userIds = userIds.Remove(userIds.Length - 1, 1);
                                    personEmailList = _userRepository.FindPersonnelEmail(userIds, (int)editedItem.GetDataKeyValue("Id"));

                                }

                                if (departmentCombo.Value.Any())
                                {
                                    // get mail of user of issue
                                    departmentIds = departmentCombo.Value.Aggregate(departmentIds, (current, item) => current + (item + ","));
                                    departmentIds = departmentIds.Remove(departmentIds.Length - 1, 1);
                                    leaderEmailList = _userRepository.FindLeaderEmail(departmentIds, (int)editedItem.GetDataKeyValue("Id"));
                                }

                                var totalEmailList = personEmailList + leaderEmailList;
                                if (totalEmailList.Length > 0)
                                {
                                    try
                                    {
                                        _sendMailService.SendEmail(totalEmailList, subject, content);
                                        _taskRepository.Update(departmentIds, userIds, (int)editedItem.GetDataKeyValue("Id"));
                                    }
                                    catch (Exception ex)
                                    {
                                        // throw;
                                    }
                                }

                                #endregion Send mail
                                Helper.Notification(RadNotification1, "Update is successful", "ok");
                            }
                            else
                                Helper.Notification(RadNotification1, "Update is failed.", "warning");
                        }
                    }
                    break;
            }
        }

        protected void RadGrid1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridDataItem item in RadGrid1.SelectedItems)
            {
                //var taskId = Helper.ConvertToInt(item["Id"].Text.Trim());
                Session["TaskId"] = item["Id"].Text.Trim();
            }
        }

        protected void RadMultiSelect1_DataBinding(object sender, EventArgs e)
        {
            var multiSelect = (RadMultiSelect)sender;
            var departmentList = _departmentRepository.FindByDivisionId(Config.InfoTechDivision);
            multiSelect.DataSource = departmentList;
            multiSelect.DataTextField = "DepartmentName";
            multiSelect.DataValueField = "DepartmentId";
        }
    }
}