using ServiceDesk.Data.Features.Issue;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using ServiceDesk.WebApp.Culture;
using System;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ServiceDesk.WebApp
{
    public partial class Default : LanguagePage
    {
        private readonly IAuthorityRepository _authorityRepository;
        private readonly IIssuesRepository _issuesRepository;

        public Default(IAuthorityRepository authorityRepository, IIssuesRepository issuesRepository)
        {
            _authorityRepository = authorityRepository;
            _issuesRepository = issuesRepository;
        }

        #region Property

        public DateTime FromDate => Convert.ToDateTime(rdpFromDate.SelectedDate);
        //public DateTime FromDate
        //{
        //    get
        //}

        public DateTime ToDate => Convert.ToDateTime(rdpToDate.SelectedDate);
        //public int DepartmentId => Claim.Session[Config.DepartmentId] != null ? Helper.ConvertToInt(Claim.Session[Config.DepartmentId].ToString()) : 0;

        //public string LanguageId = Claim.Session[Config.LanguageId] != null ? Claim.Session[Config.LanguageId].ToString() : "vi-VN";

        //public string EmployeeId = Claim.Session[Config.UserId] != null ? Claim.Session[Config.UserId].ToString() : string.Empty;

        #endregion Property

        protected void Page_Load(object sender, EventArgs e)
        {
            _authorityRepository.LoadPrivilege();
            if (!IsPostBack)
            {
                if (!_authorityRepository.LoggedIn())
                    Response.Redirect("~/Account/Login.aspx?returnUrl=" + Server.UrlEncode(Request.Url.AbsoluteUri));
                rdpFromDate.SelectedDate = DateTime.Today.AddDays(-15);
                rdpToDate.SelectedDate = DateTime.Now;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            RadGrid1.Rebind();
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var employeeId = Claim.Session[Config.UserId] != null ? Claim.Session[Config.UserId].ToString() : string.Empty;
            var languageId = Claim.Session[Config.LanguageId] != null ? Claim.Session[Config.LanguageId].ToString() : "vi-VN"; ;
            ((RadGrid)sender).DataSource = _issuesRepository.FindByOwner(employeeId, languageId, FromDate, ToDate);
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

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            switch (e.Item.OwnerTableView.Name)
            {
                case "Master":
                    {
                        if (e.Item is GridDataItem dataItem)
                        {
                            // Limited text
                            Helper.TextLimit("lbDepartmentName", 20, e);
                            Helper.TextLimit("lbDivisionName", 20, e);
                            Helper.TextLimit("lbDescription", 20, e);
                            Helper.TextLimit("lbEmployeeName", 20, e);

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
                            if (!item.OwnerTableView.IsItemInserted)
                            {
                                var status = (HiddenField)editItem.FindControl("hdStatusId");
                                if (status.Value == "1")
                                {
                                    var txtDescription = (RadTextBox)item.FindControl("Description");
                                    txtDescription.ReadOnly = true;
                                    var txtTitle = (RadTextBox)item.FindControl("Title");
                                    txtTitle.ReadOnly = true;
                                    var txtEmail = (RadTextBox)item.FindControl("Email");
                                    txtEmail.ReadOnly = true;
                                    var txtMobile = (RadTextBox)item.FindControl("Mobile");
                                    txtMobile.ReadOnly = true;
                                    var txtPhone = (RadTextBox)item.FindControl("Phone");
                                    txtPhone.ReadOnly = true;
                                    var updateButton = (RadButton)item.FindControl("btnUpdate");
                                    updateButton.Enabled = false;
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

        protected void RadGrid1_InsertCommand(object source, GridCommandEventArgs e)
        {
            switch (e.Item.OwnerTableView.Name)
            {
                case "Master":
                    {
                        if (e.Item is GridEditableItem editedItem)
                        {
                            var issueCommand = new IssuesCommand()
                            {
                                Title = ((RadTextBox)editedItem.FindControl("Title")).Text.Trim(),
                                Phone = ((RadTextBox)editedItem.FindControl("Phone")).Text.Trim(),
                                Mobile = ((RadTextBox)editedItem.FindControl("Mobile")).Text.Trim(),
                                Email = ((RadTextBox)editedItem.FindControl("Email")).Text.Trim(),
                                Description = ((RadTextBox)editedItem.FindControl("Description")).Text.Trim(),
                                EmployeeId = Claim.Session[Config.UserId] != null ? Claim.Session[Config.UserId].ToString() : string.Empty,
                                DepartmentId = Claim.Session[Config.DepartmentId] != null ? Helper.ConvertToInt(Claim.Session[Config.DepartmentId].ToString()) : 0,
                                StatusId = 1 // set wating
                            };
                            if (_issuesRepository.Add(issueCommand))
                                Helper.Notification(RadNotification1, "Insert is successful", "ok");
                            else
                                Helper.Notification(RadNotification1, "Insert is failed", "warning");
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
                            var issueCommand = new IssuesCommand()
                            {
                                Id = (int)editedItem.GetDataKeyValue("Id"),
                                Title = ((RadTextBox)editedItem.FindControl("Title")).Text.Trim(),
                                Phone = ((RadTextBox)editedItem.FindControl("Phone")).Text.Trim(),
                                Mobile = ((RadTextBox)editedItem.FindControl("Mobile")).Text.Trim(),
                                Email = ((RadTextBox)editedItem.FindControl("Email")).Text.Trim(),
                                Description = ((RadTextBox)editedItem.FindControl("Description")).Text.Trim()
                            };

                            if (_issuesRepository.Update(issueCommand))
                                Helper.Notification(RadNotification1, "Update is successful", "ok");
                            else
                                Helper.Notification(RadNotification1, "Update is failed.", "warning");
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
                            var label = (Label)e.Item.FindControl("lbStatus");
                            if (label.Text != "1")
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
    }
}