using ServiceDesk.Data.Features.TaskExecuted;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using ServiceDesk.WebApp.Culture;
using System;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.Calendar;

namespace ServiceDesk.WebApp.Issues
{
    public partial class TaskExecute : LanguagePage
    {
        private readonly IAuthorityRepository _authorityRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly ITaskExecuteRepository _taskExecuteRepository;

        public TaskExecute(IAuthorityRepository authorityRepository, IStatusRepository statusRepository, ITaskExecuteRepository taskRepository)
        {
            _authorityRepository = authorityRepository;
            _statusRepository = statusRepository;
            _taskExecuteRepository = taskRepository;
        }

        #region Property

        public DateTime FromDate => Convert.ToDateTime(rdpFromDate.SelectedDate);
        public DateTime ToDate => Convert.ToDateTime(rdpToDate.SelectedDate);

        private int StatusId => !string.IsNullOrEmpty(rcbStatus.SelectedValue) ?
            Helper.ConvertToInt(rcbStatus.SelectedValue) : -1;

        #endregion Property

        protected void Page_Load(object sender, EventArgs e)
        {
            _authorityRepository.LoadPrivilege();
            if (!IsPostBack)
            {
                if (!_authorityRepository.LoggedIn() && !_authorityRepository.CanView)
                    Response.Redirect("~/Account/Login.aspx?returnUrl=" + Server.UrlEncode(Request.Url.AbsolutePath));
                rdpFromDate.SelectedDate = DateTime.Today.AddDays(-15);
                rdpToDate.SelectedDate = DateTime.Now;
                InitStatusCombobox();
                //InitDivisionCombobox();
            }
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            RadGrid1.Rebind();
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var languageId = Claim.Session[Config.LanguageId] != null ? Claim.Session[Config.LanguageId].ToString() : "vi-VN";
            var userName = Claim.Session[Config.UserName] != null ? Claim.Session[Config.UserId].ToString() : "All";
            if (!e.IsFromDetailTable)
            {
                ((RadGrid)sender).DataSource = _taskExecuteRepository.FindByUserId(userName, StatusId, languageId, FromDate, ToDate);
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
                            //Helper.TextLimit("lbRequestDepartmentName", 20, e);
                           // Helper.TextLimit("lbCustomerDivisionName", 20, e);
                            //Helper.TextLimit("lbRequestDescription", 20, e);
                            Helper.TextLimit("lbCustomerEmployeeName", 18, e);

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
                        }

                        //edit mode
                        if (e.Item is GridEditFormItem item && e.Item.IsInEditMode)
                        {
                            //var editItem = (GridEditableItem)e.Item;

                            var editItem = (GridEditableItem)e.Item;

                            //var statusCombox = (RadComboBox)e.Item.FindControl("Status");
                            //var priorityComboBox = (RadComboBox)editItem.FindControl("Priority");

                            var statusId = (HiddenField)editItem.FindControl("hdStatusId");
                            var txtDescription = (RadTextBox)editItem.FindControl("Description");
                            var progress = (RadSlider)editItem.FindControl("Progress");
                            var finishDatePicker = (RadDatePicker)editItem.FindControl("FinishDate");
                            var startDatePicker = (RadDatePicker)editItem.FindControl("StartDate");
                            var endDatePicker = (RadDatePicker)editItem.FindControl("EndDate");

                            var commandButton = (RadButton)editItem.FindControl("btnUpdate");
                            //Helper.SetControlReadOnly(statusCombox);
                            //Helper.SetControlReadOnly(priorityComboBox);
                            // Edit mode
                            if (!item.OwnerTableView.IsItemInserted)
                            {
                                // set attribute readonly
                                if (Helper.ConvertToInt(statusId.Value) == Config.Cancel ||
                                    Helper.ConvertToInt(statusId.Value) == Config.Complete)
                                {
                                    
                                    Helper.SetControlReadOnly(txtDescription);
                                    Helper.SetControlReadOnly(finishDatePicker);
                                    Helper.SetControlReadOnly(startDatePicker);
                                    Helper.SetControlReadOnly(endDatePicker);
                                    Helper.SetControlReadOnly(progress);
                                    Helper.SetControlReadOnly(commandButton);
                                }
                            }
                        }

                        break;
                    }
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
                            var model = new TaskExecuteCommand()
                            {
                                Id = (int)editedItem.GetDataKeyValue("Id"),
                                FinishDate = ((RadDatePicker)editedItem.FindControl("FinishDate")).SelectedDate,
                                TaskId = Helper.ConvertToInt(editedItem["TaskId"].Text),
                                Progress = Helper.ConvertToInt(((RadSlider)editedItem.FindControl("Progress")).Value),
                                Description = ((RadTextBox)editedItem.FindControl("Description")).Text.Trim()
                            };
                            if (_taskExecuteRepository.Update(model))
                            {
                                Helper.Notification(RadNotification1, "Update is successful", "ok");
                            }
                            else
                                Helper.Notification(RadNotification1, "Update is failed.", "warning");
                        }
                    }
                    break;
            }
        }
    }
}