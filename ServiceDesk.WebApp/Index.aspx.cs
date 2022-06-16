using ServiceDesk.Data.Features.Issue;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using ServiceDesk.WebApp.Culture;
using System;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.Calendar;

namespace ServiceDesk.WebApp
{
    public partial class Index : LanguagePage
    {
        private readonly IAuthorityRepository _authorityRepository;
        private readonly IIssuesRepository _issuesRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ISendMailService _sendMailService;
        private readonly IStatusRepository _statusRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ITaskExecuteRepository _taskExecuteRepository;
        private readonly IUserRepository _userRepository;

        public Index(IAuthorityRepository authorityRepository,
            IIssuesRepository issuesRepository,
            IStatusRepository statusRepository,
            ITagRepository tagRepository,
            IEmployeeRepository employeeRepository,
            ITaskExecuteRepository taskExecuteRepository, ISendMailService sendMailService, IUserRepository userRepository)
        {
            _authorityRepository = authorityRepository;
            _issuesRepository = issuesRepository;
            _statusRepository = statusRepository;
            _tagRepository = tagRepository;
            _employeeRepository = employeeRepository;
            _taskExecuteRepository = taskExecuteRepository;
            _sendMailService = sendMailService;
            _userRepository = userRepository;
        }

        #region Property

        public DateTime FromDate => Convert.ToDateTime(rdpFromDate.SelectedDate);

        public DateTime ToDate => Convert.ToDateTime(rdpToDate.SelectedDate);

        private int StatusId => !string.IsNullOrEmpty(rcbStatus.SelectedValue) ?
            Helper.ConvertToInt(rcbStatus.SelectedValue) : 0;

        private int TagId => !string.IsNullOrEmpty(rcbTag.SelectedValue) ?
            Helper.ConvertToInt(rcbTag.SelectedValue) : 0;

        public bool? IsRadAsyncValid
        {
            get
            {
                if (Session["IsRadAsyncValid"] == null) Session["IsRadAsyncValid"] = true;

                return Convert.ToBoolean(Session["IsRadAsyncValid"].ToString());
            }
            set => Session["IsRadAsyncValid"] = value;
        }

        #endregion Property

        private const int MaxTotalBytes = 20971520; // 20MB

        protected void Page_Load(object sender, EventArgs e)
        {
            _authorityRepository.LoadPrivilege();
            if (!IsPostBack)
            {
                if (!_authorityRepository.LoggedIn())
                    Response.Redirect("~/Account/Login.aspx?returnUrl=" + Server.UrlEncode(Request.Url.AbsolutePath));
                rdpFromDate.SelectedDate = DateTime.Today.AddDays(-365);
                rdpToDate.SelectedDate = DateTime.Now;
                InitStatusCombobox();
                InitTagCombobox();
                HideWizard();
                Session["IssueId"] = "0";
            }
        }

        private void InitTagCombobox()
        {
            var languageId = Claim.Session[Config.LanguageId] != null ? Claim.Session[Config.LanguageId].ToString() : "vi-VN";
            var list = _tagRepository.FindAll(languageId);
            rcbTag.Items.Clear();
            rcbTag.DataSource = list;
            rcbTag.DataBind();
        }

        private void InitStatusCombobox()
        {
            var list = _statusRepository.FindAll();
            rcbStatus.Items.Clear();
            rcbStatus.DataSource = list;
            rcbStatus.DataBind();
        }

        protected void rcbStatus_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            HideWizard();
            RadGrid1.Rebind();
        }

        protected void rcbTag_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //HideWizard();
            RadGrid1.Rebind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            HideWizard();
            RadGrid1.Rebind();
        }

        protected void FromDateSelectedDateChange(object sender, SelectedDateChangedEventArgs e)
        {
            HideWizard();
            RadGrid1.Rebind();
        }

        protected void ToDateSelectedDateChange(object sender, SelectedDateChangedEventArgs e)
        {
            HideWizard();
            RadGrid1.Rebind();
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var employeeId = Claim.Session[Config.UserId] != null ? Claim.Session[Config.UserId].ToString() : string.Empty;
            //var departmentId = Claim.Session[Config.DepartmentId] != null ? Helper.ConvertToInt(Claim.Session[Config.DepartmentId].ToString()) : 0;
            var languageId = Claim.Session[Config.LanguageId] != null ? Claim.Session[Config.LanguageId].ToString() : "vi-VN";
            //var roleId = Claim.Session[Config.RoleId] != null ? Helper.ConvertToInt(Claim.Session[Config.RoleId].ToString()) : 0;
            ((RadGrid)sender).DataSource = _issuesRepository.FindByOwner(employeeId, languageId, FromDate, ToDate);
        }

        protected void RadGrid2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var languageId = Claim.Session[Config.LanguageId] != null ? Claim.Session[Config.LanguageId].ToString() : "vi-VN";
            var issueId = Session["IssueId"] != null ? Helper.ConvertToInt(Session["IssueId"].ToString()) : 0;
            ((RadGrid)sender).DataSource = _taskExecuteRepository.FindByIssueId(issueId, languageId);
        }

        protected void RadRating1_Rate(object sender, EventArgs e)
        {
            var oRating = (RadRating)sender;
            var dataItem = (GridDataItem)oRating.Parent.Parent;
            var id = (int)dataItem.GetDataKeyValue("Id");
            var value = Helper.ConvertToInt(oRating.DbValue);
            try
            {
                _issuesRepository.Update(id, value);
            }
            catch (Exception)
            {
                //RadGrid1.Controls.Add(new LiteralControl("Unable to update Ratings. Reason: " + ex.Message));
            }
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                RadGrid1.MasterTableView.SortExpressions.Clear();
                RadGrid1.MasterTableView.GroupByExpressions.Clear();
                RadGrid1.Rebind();
            }
            else if (e.Argument == "RebindAndNavigate")
            {
                RadGrid1.MasterTableView.SortExpressions.Clear();
                RadGrid1.MasterTableView.GroupByExpressions.Clear();
                RadGrid1.MasterTableView.CurrentPageIndex = RadGrid1.MasterTableView.PageCount - 1;
                RadGrid1.Rebind();
            }
        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            switch (e.Item.OwnerTableView.Name)
            {
                case "Master":
                    {
                        if (e.Item is GridDataItem dataItem)
                        {
                            // Limited text
                            //Helper.TextLimit("lbTagName", 18, e);
                            Helper.TextLimit("lbTitle", 50, e);
                            Helper.TextLimit("lbReview", 50, e);
                            //Helper.TextLimit("lbDivisionName", 18, e);
                            //Helper.TextLimit("lbDescription", 18, e);
                            Helper.TextLimit("lbEmployee", 22, e);

                            // set css for status
                            var label = (Label)dataItem.FindControl("lbStatus");
                            switch (Helper.ConvertToInt(dataItem["StatusId"].Text))
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

                            // add function javascript to checkbox
                            // CheckBox checkColumn = dataItem.FindControl("checkColumn") as CheckBox;
                            // if (checkColumn != null) checkColumn.Attributes.Add("onclick", "uncheckOther(this);");
                        }
                        //edit mode
                        if (e.Item is GridEditFormItem item && e.Item.IsInEditMode)
                        {
                            var editItem = (GridEditableItem)e.Item;
                            // Load combobox
                            //var tagCombo = (RadComboBox)e.Item.FindControl("Tags");
                            //var languageId = Claim.Session[Config.LanguageId] != null ? Claim.Session[Config.LanguageId].ToString() : "vi-VN";
                            //tagCombo.DataSource = _tagRepository.FindAll(languageId);

                            var txtEmployeeId = (RadTextBox)item.FindControl("EmployeeId");
                            var hdStatus = (HiddenField)editItem.FindControl("hdStatusId");
                            var txtUpdateDescription = (RadTextBox)item.FindControl("UpdateDescription");

                            var txtTitle = (RadTextBox)item.FindControl("Title");
                            var txtEmail = (RadTextBox)item.FindControl("Email");
                            //var txtPhone = (RadTextBox)item.FindControl("Phone");
                            var txtReason = (RadTextBox)item.FindControl("Reason");
                            var txtMobile = (RadTextBox)item.FindControl("Mobile");
                            //var txtDispatchNumber = (RadTextBox)item.FindControl("DispatchNumber");
                            var cancelCheckbox = (CheckBox)item.FindControl("CancelCheckbox");
                            //var tagId = (HiddenField)editItem.FindControl("hdTagId");
                            //var tagName = (HiddenField)editItem.FindControl("hdTagName");
                            //var tags = (RadComboBox)editItem.FindControl("Tags");
                            var commandButton = (RadButton)editItem.FindControl("btnUpdate");

                            if (!item.OwnerTableView.IsItemInserted)
                            {
                                //tags.SelectedValue = tagId.Value;
                                //tags.Text = tagName.Value;
                                //tagCombo.DataBind();

                                var fourRow = (HtmlControl)item.FindControl("fourRow");
                                fourRow.Visible = false;

                                //if status not is waiting then set readonly
                                if (Helper.ConvertToInt(hdStatus.Value) != Config.Waiting)
                                {
                                    //Helper.SetComboboxReadOnly(tags);
                                    Helper.SetControlReadOnly(txtEmployeeId);
                                    Helper.SetControlReadOnly(txtUpdateDescription);
                                    Helper.SetControlReadOnly(txtTitle);
                                    Helper.SetControlReadOnly(txtEmail);
                                    Helper.SetControlReadOnly(txtMobile);
                                    //Helper.SetControlReadOnly(txtPhone);
                                    //Helper.SetControlReadOnly(txtDispatchNumber);
                                    //Helper.SetControlReadOnly(commandButton);
                                }
                            }
                            else
                            {
                                //tagCombo.DataBind();

                                var employeeId = Claim.Session[Config.UserId] != null ? Claim.Session[Config.UserId].ToString() : string.Empty;
                                var infoList = _employeeRepository.FindInformation(employeeId);
                                foreach (var info in infoList)
                                {
                                    txtEmail.Text = info.Email;
                                    //txtPhone.Text = info.Phone;
                                    txtMobile.Text = info.Mobile;
                                    txtEmployeeId.Text = employeeId;
                                }

                                var txtEmployeeName = (RadTextBox)item.FindControl("EmployeeName");
                                txtEmployeeName.Visible = false;

                                //Label lbEmployeeName =(Label)e.Item.FindControl("lbEmployeeName");
                                var lbEmployeeName = (HtmlControl)item.FindControl("lbEmployeeName");
                                lbEmployeeName.Visible = false;

                                // Label lbDivisionName = (Label)e.Item.FindControl("lbDivision");
                                var lbDivisionName = (HtmlControl)item.FindControl("lbDivision");
                                lbDivisionName.Visible = false;

                                var txtDivisionName = (RadTextBox)item.FindControl("DivisionName");
                                txtDivisionName.Visible = false;

                                var twoRow = (HtmlControl)item.FindControl("twoRow");
                                twoRow.Visible = false;
                                var fiveRow = (HtmlControl)item.FindControl("fiveRow");
                                fiveRow.Visible = false;
                            }
                        }
                    }
                    break;
            }
        }

        protected void RadGrid2_ItemDataBound(object sender, GridItemEventArgs e)
        {
            switch (e.Item.OwnerTableView.Name)
            {
                case "Master":
                    {
                        if (e.Item is GridDataItem dataItem)
                        {
                            // Limited text
                            Helper.TextLimit("lbDepartmentName", 20, e);
                            Helper.TextLimit("lbEmployeeName", 20, e);
                        }
                    }
                    break;
            }
        }

        protected void EmployeeIdValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            try
            {
                var exist = _employeeRepository.ExistEmployeeId(args.Value.Trim());
                args.IsValid = 0 != exist;
            }
            catch (Exception)
            {
                args.IsValid = false;
            }
        }

        protected void ReasonValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            var cancel = ((CheckBox)(((GridEditFormItem)((CustomValidator)source).NamingContainer)).FindControl("CancelCheckbox")).Checked;
            try
            {
                if (cancel && string.IsNullOrEmpty(args.Value))
                {
                    args.IsValid = false;
                }
            }
            catch (Exception)
            {
                args.IsValid = false;
            }

            if (!args.IsValid)
            {
                var textBox = ((RadTextBox)(((GridEditFormItem)((CustomValidator)source).
                    NamingContainer)).FindControl("Reason"));
                textBox.BorderColor = Color.FromArgb(255, 0, 0);
            }
        }

        protected void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                var editLink = (HyperLink)e.Item.FindControl("EditLink");
                editLink.Attributes["href"] = "javascript:void(0);";
                editLink.Attributes["onclick"] =
                  $"return ShowEditForm('{e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"]}','{e.Item.ItemIndex}');";
            }

            //if (!(e.Item is GridEditableItem) || !e.Item.IsInEditMode) return;
            //var editItem = e.Item as GridEditFormItem;
            //if (editItem == null) return;
            //var upload  = ((GridEditableItem)e.Item)["Upload"].FindControl("Path") as RadAsyncUpload;

            //var cell = (TableCell)upload.Parent;
            //var validator = new CustomValidator
            //{
            //  ErrorMessage = @"File upload quá lớn hoặc không đúng định dạng",
            //  Display = ValidatorDisplay.Dynamic,
            //  ForeColor = Color.Red
            //};
            //cell.Controls.Add(validator);
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName != "DownloadCommandColumn") return;
            {
                var item = e.Item as GridDataItem;
                //var ReceiptId = item.GetDataKeyValue("ReceiptId").ToString();
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

        protected void RadGrid1_InsertCommand(object source, GridCommandEventArgs e)
        {
            switch (e.Item.OwnerTableView.Name)
            {
                case "Master":
                    {
                        if (e.Item is GridEditableItem editedItem)
                        {
                            var employeeId = ((RadTextBox)editedItem.FindControl("EmployeeId")).Text.Trim();
                            var departmentId = _employeeRepository.FindByEmployeeId(employeeId);
                            var title = ((RadTextBox)editedItem.FindControl("Title")).Text.Trim();
                            //var phone = ((RadTextBox)editedItem.FindControl("Phone")).Text.Trim();
                            var mobile = ((RadTextBox)editedItem.FindControl("Mobile")).Text.Trim();
                            var email = ((RadTextBox)editedItem.FindControl("Email")).Text.Trim();
                            var description = ((RadTextBox)editedItem.FindControl("InsertDescription")).Text.Trim();
                            var createUser = Claim.Session[Config.UserId] != null
                                ? Claim.Session[Config.UserId].ToString() : string.Empty;
                            //var tagId = Helper.ConvertToInt(((RadComboBox)editedItem.FindControl("Tags")).SelectedValue);
                            //var dispatchNumber = ((RadTextBox)editedItem.FindControl("DispatchNumber")).Text.Trim();
                            var review = ((RadTextBox)editedItem.FindControl("Review")).Text.Trim();

                            var upload = (RadAsyncUpload)editedItem.FindControl("Path");
                            string dispatchPath;
                            if (upload.UploadedFiles.Count > 0)
                            {
                                if (IsRadAsyncValid != null && !IsRadAsyncValid.Value)
                                {
                                    e.Canceled = true;
                                    return;
                                }

                                var target = HttpContext.Current.Server.MapPath("~/DocumentFiles/");
                                dispatchPath = upload.UploadedFiles[0].GetNameWithoutExtension() + "_" + DateTime.Now.Day +
                                                   DateTime.Now.Month + DateTime.Now.Year + "_" +
                                                   Guid.NewGuid() + upload.UploadedFiles[0].GetExtension();
                                upload.UploadedFiles[0].SaveAs(Path.Combine(target, dispatchPath));
                            }
                            else
                            {
                                dispatchPath = string.Empty;
                            }

                            var issueCommand = new IssuesCommand()
                            {
                                EmployeeId = employeeId,
                                Title = title,
                                //Phone = phone,
                                Mobile = mobile,
                                Email = email,
                                CustomerDescription = description,
                                DepartmentId = departmentId,
                                CreateUser = createUser,
                                //TagId = tagId > 0 ? tagId : 4,                                
                                StatusId = Config.Waiting,
                                //DispatchNumber = dispatchNumber,
                                DispatchPath = dispatchPath,
                                Review = review
                            };

                            if (_issuesRepository.Add(issueCommand))
                            {
                                //SendMail(tagId);
                                Helper.Notification(RadNotification1, "Yêu cầu đã được gửi. Chúng tôi sẽ liên hệ với bạn trong thời gian sớm nhất.", "ok");
                            }
                            else
                                Helper.Notification(RadNotification1, "Một lỗi đã xảy ra. Không thể gửi yêu cầu.", "warning");
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
                            //userViewModel.Id = (int)editedItem.GetDataKeyValue("UserId");
                            var employeeId = ((RadTextBox)editedItem.FindControl("EmployeeId")).Text.Trim();
                            var issueId = (int)editedItem.GetDataKeyValue("Id");
                            //var cancelCheckbox = ((CheckBox)editedItem.FindControl("CancelCheckbox")).Checked;
                            //if (cancelCheckbox)
                            //{
                            //  var reason = ((RadTextBox)editedItem.FindControl("Reason")).Text.Trim();
                            //  if (_issuesRepository.CancelIssue(issueId, reason))
                            //    Helper.Notification(RadNotification1, "Cancel Issue is successful", "ok");
                            //  else
                            //    Helper.Notification(RadNotification1, "Cancel Issue is failed.", "warning");
                            //}

                            var currentDepartmentId = _employeeRepository.FindByEmployeeId(employeeId);
                            var title = ((RadTextBox)editedItem.FindControl("Title")).Text.Trim();
                            //var phone = ((RadTextBox)editedItem.FindControl("Phone")).Text.Trim();
                            var mobile = ((RadTextBox)editedItem.FindControl("Mobile")).Text.Trim();
                            var email = ((RadTextBox)editedItem.FindControl("Email")).Text.Trim();
                            var description = ((RadTextBox)editedItem.FindControl("UpdateDescription")).Text.Trim();
                            var updateUser = Claim.Session[Config.UserId] != null
                                ? Claim.Session[Config.UserId].ToString() : string.Empty;
                            //var tagId = Helper.ConvertToInt(((RadComboBox)editedItem.FindControl("Tags")).SelectedValue);
                            //var dispatchNumber = ((RadTextBox)editedItem.FindControl("DispatchNumber")).Text.Trim();
                            var updateDate = (HiddenField)editedItem.FindControl("hdCreateDate");
                            var review = ((RadTextBox)editedItem.FindControl("Review")).Text.Trim();
                            var upload = (RadAsyncUpload)editedItem.FindControl("Path");
                            var hdPath = (HiddenField)editedItem.FindControl("hdPath");
                            //var lastTagId = Helper.ConvertToInt(((HiddenField)editedItem.FindControl("hdTagId")).Value);

                            string dispatchPath;
                            if (upload.UploadedFiles.Count > 0)
                            {
                                if (IsRadAsyncValid != null && !IsRadAsyncValid.Value)
                                {
                                    e.Canceled = true;
                                    return;
                                }

                                var target = HttpContext.Current.Server.MapPath("~/DocumentFiles/");
                                dispatchPath = upload.UploadedFiles[0].GetNameWithoutExtension() + "_" + DateTime.Now.Day +
                                                DateTime.Now.Month + DateTime.Now.Year + "_" + Guid.NewGuid() +
                                                upload.UploadedFiles[0].GetExtension();
                                upload.UploadedFiles[0].SaveAs(Path.Combine(target, dispatchPath));
                            }
                            else
                            {
                                dispatchPath = hdPath.Value;
                            }

                            var issueCommand = new IssuesCommand()
                            {
                                Id = issueId,
                                EmployeeId = employeeId,
                                Title = title,
                                // Phone = "",
                                //Phone = phone,
                                Mobile = mobile,
                                Email = email,
                                CustomerDescription = description,
                                DepartmentId = currentDepartmentId,
                                UpdateUser = updateUser,
                                //DispatchNumber = dispatchNumber,
                                //TagId = tagId > 0 ? tagId : 4,
                                UpdateDate = Convert.ToDateTime(updateDate.Value.Trim()),
                                DispatchPath = dispatchPath,
                                Review = review
                            };

                            if (_issuesRepository.Update(issueCommand))
                            {
                                //if (tagId != lastTagId)
                                //{
                                //    //Send Email
                                //    SendMail(tagId);
                                //}
                                Helper.Notification(RadNotification1, "Chỉnh sửa yêu cầu thành công.", "ok");
                            }
                            else
                                Helper.Notification(RadNotification1, "Không thể chỉnh sửa yêu cầu.", "warning");
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
                            var status = Helper.ConvertToInt(((Label)e.Item.FindControl("lbStatus")).Text);
                            if (status == Config.Waiting)
                            {
                                try
                                {
                                    _issuesRepository.Remove((int)editedItem.GetDataKeyValue("Id"));
                                    Helper.Notification(RadNotification1, "Remove is successful", "ok");
                                }
                                catch (Exception ex)
                                {
                                    Helper.Notification(RadNotification1, "Remove is failed. Reason: " + ex.Message, "warning");
                                }
                            }
                            else
                                Helper.Notification(RadNotification1, "This issue can't deleted because it's processing.", "warning");
                        }
                    }
                    break;
            }
        }

        private void HideWizard()
        {
            detailWidget.Visible = OwnerCancel.Visible = AllWaiting.Visible =
            Allcomplete.Visible = AllCancel.Visible = Handle.Visible = false;
        }

        private void ShowWizard(int issueId)
        {
            var result = _issuesRepository.ProcessedStatus(issueId);
            switch (result)
            {
                case 1:
                    detailWidget.Visible = OwnerCancel.Visible = true;
                    AllWaiting.Visible = Allcomplete.Visible =
                    AllCancel.Visible = Handle.Visible = false;
                    RadGrid2.Rebind();
                    break;

                case 2:
                    detailWidget.Visible = AllWaiting.Visible = true;
                    OwnerCancel.Visible = Allcomplete.Visible =
                    AllCancel.Visible = Handle.Visible = false;
                    RadGrid2.Rebind();
                    break;

                case 3:
                    detailWidget.Visible = Allcomplete.Visible = true;
                    OwnerCancel.Visible = AllWaiting.Visible =
                    AllCancel.Visible = Handle.Visible = false;
                    RadGrid2.Rebind();
                    break;

                case 4:
                    detailWidget.Visible = AllCancel.Visible = true;
                    OwnerCancel.Visible = AllWaiting.Visible =
                    Allcomplete.Visible = Handle.Visible = false;
                    RadGrid2.Rebind();
                    break;

                case 5:
                    detailWidget.Visible = Handle.Visible = true;
                    OwnerCancel.Visible = AllWaiting.Visible = 
                    AllCancel.Visible = Allcomplete.Visible = false;
                    RadGrid2.Rebind();
                    break;
            }
        }

        protected void RadGrid1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridDataItem item in RadGrid1.SelectedItems)
            {
                //change the code here according to the value you need
                var issueId = Helper.ConvertToInt(item["Id"].Text.Trim());
                Session["IssueId"] = item["Id"].Text.Trim();
                ShowWizard(issueId);
            }
        }

        protected void Path_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            if (e.File.ContentLength < MaxTotalBytes)
            {
                var extFile = e.File.GetExtension();
                if (Helper.CheckExtenstion(extFile))
                {
                    e.IsValid = true;
                    IsRadAsyncValid = true;
                }
                else
                {
                    e.IsValid = false;
                    IsRadAsyncValid = false;
                }
            }
            else
            {
                e.IsValid = false;
                IsRadAsyncValid = false;
            }
        }

        private void SendMail(int tagId)
        {
            const string subject = "Xử lí yêu cầu";
            var content = "Một yêu cầu đã được gửi tới ông/bà.</br>";
            content += "Đăng nhập vào chương trình <strong><a href='#'>support.vietsov.com.vn</a></strong> để xử lí.</br></br>";
            content += "<i>Vui lòng không trả lời Mail này!</i>";
            // send mail to user of your departments

            try
            {
                if (tagId != 4 && tagId > 0)
                {
                    int requestDepartmentId;
                    switch (tagId)
                    {
                        case 1:
                            requestDepartmentId = Config.RepairDepartment;
                            break;

                        case 2:
                            requestDepartmentId = Config.NetworkDepartment;
                            break;

                        default:
                            requestDepartmentId = Config.SoftwareDepartment;
                            break;
                    }
                    // get mail of user of issue
                    var userList = _userRepository.FindLeaderEmail(requestDepartmentId);
                    _sendMailService.SendEmail(userList, subject, content);
                }
            }
            catch (Exception ex)
            {
                //
            }
        }
    }
}