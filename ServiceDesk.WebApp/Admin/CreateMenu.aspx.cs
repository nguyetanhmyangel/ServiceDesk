using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using System;
using System.Linq;
using System.Web.UI.WebControls;
using ServiceDesk.Data.Features.Menu;
using ServiceDesk.Data.Features.MenuIcon;
using Telerik.Web.UI;
using static System.String;

namespace ServiceDesk.WebApp.Admin
{
    public partial class CreateMenu : System.Web.UI.Page
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IAuthorityRepository _authorityRepository;
        private readonly IMenuIconRepository _menuIconRepository;

        public CreateMenu(IMenuRepository menuRepository, IAuthorityRepository authorityRepository, IMenuIconRepository menuIconRepository)
        {
            _menuRepository = menuRepository;
            _authorityRepository = authorityRepository;
            _menuIconRepository = menuIconRepository;
        }

        #region Property

        public string MenuName
        {
            get => txtMenuName.Text.Trim();
            set => txtMenuName.Text = value;
        }

        #endregion Property

        //private const int ItemsPerRequest = 10;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (_menuService.LoggedIn() && _menuService.IsAdmin())
                if (!_authorityRepository.LoggedIn() && !_authorityRepository.IsAdmin())
                    Response.Redirect("~/Account/Login.aspx?returnUrl=" + Server.UrlEncode(Request.Url.AbsolutePath));
            }
        }

        //protected void Page_Init(object sender, EventArgs e)
        //{
        //    RadGrid1.NeedDataSource += new GridNeedDataSourceEventHandler(RadGrid1_NeedDataSource);
        //}

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            RadGrid1.Rebind();
        }

        protected void rcbIcon_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            var allIcons = _menuIconRepository.FindAll();

            if (!IsNullOrEmpty(e.Text))
            {
                allIcons = allIcons.Where(i => i.IconName.Contains(e.Text.Trim()))
                  .OrderBy(i => i.Id);
            }

            var list = allIcons.Skip(e.NumberOfItems).Take(10);
            var comboBox = (RadComboBox)sender;
            comboBox.DataSource = list;
            comboBox.DataBind();

            var menuIconViewModels = list as MenuIconResponse[] ?? list.ToArray();
            var endOffset = e.NumberOfItems + menuIconViewModels.Count();
            var totalCount = menuIconViewModels.Count();

            if (endOffset == totalCount)
                e.EndOfItems = true;

            e.Message = $"Items <b>1</b>-<b>{endOffset}</b> out of <b>{totalCount}</b>";
        }

        protected void OnSelectedIndexChangedHandler(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Claim.Session["MenuIconId"] = e.Value;
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
                            var comboTree = (RadDropDownTree)editItem.FindControl("Tree");
                            comboTree.DataSource = _menuRepository.FindAll();
                            comboTree.DataBind();
                        }
                    }
                    break;
            }
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            ((RadGrid)sender).DataSource = _menuRepository.FindByName(MenuName);
        }

        protected void RadGrid1_InsertCommand(object source, GridCommandEventArgs e)
        {
            switch (e.Item.OwnerTableView.Name)
            {
                case "Master":
                    {
                        var menuCommand = new MenuCommand();
                        //SqlDataSourceGrid.InsertParameters["ProjectId"].DefaultValue =
                        //    Claim.Session["ProjectId"] != null ? Claim.Session["ProjectId"].ToString() : "0";
                        if (e.Item is GridEditableItem editedItem)
                        {
                            menuCommand.MenuName = ((RadTextBox)editedItem.FindControl("MenuName")).Text.Trim();
                            menuCommand.RussianMenuName = !IsNullOrEmpty(((RadTextBox)editedItem.FindControl("RussianMenuName")).Text) ?
                                ((RadTextBox)editedItem.FindControl("RussianMenuName")).Text.Trim() : ((RadTextBox)editedItem.FindControl("MenuName")).Text.Trim();
                            menuCommand.Url = ((RadTextBox)editedItem.FindControl("Url")).Text.Trim();
                            menuCommand.FriendlyUrl = ((RadTextBox)editedItem.FindControl("FriendlyUrl")).Text.Trim();
                            menuCommand.MenuIconId = !IsNullOrEmpty(((RadComboBox)editedItem.FindControl("rcbIcon")).SelectedValue) ?
                                Helper.ConvertToInt(((RadComboBox)editedItem.FindControl("rcbIcon")).SelectedValue) : (int?)null;
                            menuCommand.Sort = Helper.ConvertToInt(((RadNumericTextBox)editedItem.FindControl("Sort")).Value);
                            menuCommand.Active = ((CheckBox)editedItem.FindControl("Active")).Checked;
                            menuCommand.ParentId = !IsNullOrEmpty(((RadDropDownTree)editedItem.FindControl("Tree")).SelectedValue) ?
                                Helper.ConvertToInt(((RadDropDownTree)editedItem.FindControl("Tree")).SelectedValue) : (int?)null;
                            try
                            {
                                _menuRepository.Add(menuCommand);
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
                        var menuCommand = new MenuCommand();
                        //SqlDataSourceGrid.InsertParameters["ProjectId"].DefaultValue =
                        //    Claim.Session["ProjectId"] != null ? Claim.Session["ProjectId"].ToString() : "0";
                        if (e.Item is GridEditableItem editedItem)
                        {
                            menuCommand.MenuId = (int)editedItem.GetDataKeyValue("MenuId");
                            menuCommand.MenuName = ((RadTextBox)editedItem.FindControl("MenuName")).Text.Trim();
                            menuCommand.RussianMenuName = !IsNullOrEmpty(((RadTextBox)editedItem.FindControl("RussianMenuName")).Text) ?
                                ((RadTextBox)editedItem.FindControl("RussianMenuName")).Text.Trim() : ((RadTextBox)editedItem.FindControl("MenuName")).Text.Trim();
                            menuCommand.Url = ((RadTextBox)editedItem.FindControl("Url")).Text.Trim();
                            menuCommand.FriendlyUrl = ((RadTextBox)editedItem.FindControl("FriendlyUrl")).Text.Trim();
                            menuCommand.MenuIconId = !IsNullOrEmpty(((RadComboBox)editedItem.FindControl("rcbIcon")).SelectedValue) ?
                                Helper.ConvertToInt(((RadComboBox)editedItem.FindControl("rcbIcon")).SelectedValue) : (int?)null;
                            menuCommand.Sort = Helper.ConvertToInt(((RadNumericTextBox)editedItem.FindControl("Sort")).Value);
                            menuCommand.Active = ((CheckBox)editedItem.FindControl("Active")).Checked;
                            menuCommand.ParentId = !IsNullOrEmpty(((RadDropDownTree)editedItem.FindControl("Tree")).SelectedValue) ?
                                Helper.ConvertToInt(((RadDropDownTree)editedItem.FindControl("Tree")).SelectedValue) : (int?)null;
                            try
                            {
                                _menuRepository.Update(menuCommand);
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
                                _menuRepository.Remove((int)editedItem.GetDataKeyValue("MenuId"));
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