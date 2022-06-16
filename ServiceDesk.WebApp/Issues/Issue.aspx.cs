using ServiceDesk.Data.Interfaces;
using ServiceDesk.Data.Features;
using ServiceDesk.Utilities;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceDesk.WebApp.Culture;
using Telerik.Web.UI;

namespace ServiceDesk.WebApp.Issues
{
    public partial class Issue : LanguagePage
    {
        private readonly IAuthorityRepository _authorityRepository;
        private readonly IIssuesRepository _issuesRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDivisionRepository _divisionRepository;
        private readonly ISendMailRepository _sendMailRepository;
        private readonly IStatusRepository _statusRepository;

        public Issue(IAuthorityRepository authorityRepository, IIssuesRepository issuesRepository,
            IDepartmentRepository departmentRepository, ISendMailRepository sendMailRepository,
            IUserRepository userRepository, IDivisionRepository divisionRepository, IStatusRepository statusRepository)
        {
            _authorityRepository = authorityRepository;
            _issuesRepository = issuesRepository;
            _departmentRepository = departmentRepository;
            _sendMailRepository = sendMailRepository;
            _userRepository = userRepository;
            _divisionRepository = divisionRepository;
            _statusRepository = statusRepository;
        }

        #region Property

        public DateTime FromDate => Convert.ToDateTime(rdpFromDate.SelectedDate);
        public DateTime ToDate => Convert.ToDateTime(rdpToDate.SelectedDate);
        private int DivisionId => !string.IsNullOrEmpty(rcbDivision.SelectedValue) ?
            Helper.ConvertToInt(rcbDivision.SelectedValue) : 0;
        private int StatusId => !string.IsNullOrEmpty(rcbStatus.SelectedValue) ?
            Helper.ConvertToInt(rcbStatus.SelectedValue) : 0;

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
                AddDivisonToCombo();
                AddStatusToCombo();
            }
        }

        private void AddDivisonToCombo()
        {
            var list = _divisionRepository.FindAll();
            rcbDivision.Items.Clear();
            rcbDivision.DataSource = list;
            rcbDivision.DataBind();
        }

        private void AddStatusToCombo()
        {
            var list = _statusRepository.FindAll();
            rcbStatus.Items.Clear();
            rcbStatus.DataSource = list;
            rcbStatus.DataBind();
        }

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
            var languageId = Claim.Session[Config.LanguageId] != null ? Claim.Session[Config.LanguageId].ToString() : "vi-VN"; ;
            ((RadGrid)sender).DataSource = _issuesRepository.FindByDivision(DivisionId, StatusId, languageId, FromDate, ToDate);
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
                            Helper.TextLimit("lbDepartmentName", 20, e);
                            Helper.TextLimit("lbDivisionName", 20, e);
                            Helper.TextLimit("lbDescription", 20, e);
                            Helper.TextLimit("lbEmployeeName", 20, e);

                            // set css for status
                            var dataItem = (GridDataItem)e.Item;
                            var label = (Label)e.Item.FindControl("lbStatus");
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
                            // Load MultiSelect
                            var departments = (RadMultiSelect)e.Item.FindControl("Department");
                            departments.DataSource = _departmentRepository.FindByDivisionId();
                            departments.DataBind();

                            // Edit mode
                            if (!item.OwnerTableView.IsItemInserted)
                            {
                                var issueId = Helper.ConvertToInt(item.GetDataKeyValue("Id").ToString());
                                // set value MultiSelect
                                var departmentValues = _departmentRepository.FindByIssueId(issueId);
                                if (departmentValues.Any())
                                {
                                    departments.Value = departmentValues.ToList();
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
                            var id = (int)editedItem.GetDataKeyValue("Id");
                            var userId = Claim.Session[Config.UserId] != null ? Claim.Session[Config.UserId].ToString() : string.Empty;
                            var multiSelect = (RadMultiSelect)editedItem.FindControl("Department");
                            if (multiSelect.Value.Any())
                            {
                                if (_issuesRepository.Update(userId, id, multiSelect.Value))
                                {
                                    //send mail
                                    try
                                    {
                                        string listSendMail = string.Empty;
                                        foreach (var value in multiSelect.Value)
                                        {
                                            // get mail of header department
                                            var departmentId = Helper.ConvertToInt(value);
                                            listSendMail += _userRepository.FindLeaderEmail(departmentId) + ",";
                                        }
                                        string subject = "Xử lí yêu cầu";
                                        string content = "Một yêu cầu đã được gửi tới ông/bà.</br>";
                                        content += "Đăng nhập vào chương trình <strong><a href='#'>It Help Desk</a></strong>.</br></br>";
                                        content += "<i>Vui lòng không trả lời Mail này!</i>";

                                        _sendMailRepository.SendEmail(listSendMail.Remove(listSendMail.Length - 1, 1), subject, content);
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

        //protected async Task GetDataFromServicesAsync()
        //{
        //    return await _sendMailRepository.SendEmailAsync("","","");
        //}
    }
}