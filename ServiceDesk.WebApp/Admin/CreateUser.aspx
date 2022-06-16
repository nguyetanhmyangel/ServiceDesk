<%@ Page Title="Create User" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="CreateUser.aspx.cs" Inherits="ServiceDesk.WebApp.Admin.CreateUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="widget">
        <div class="widget-header bordered-left bordered-darkorange">
            <span class="widget-caption">User Form</span>
            <div class="widget-buttons">
                <a href="#" data-toggle="maximize">
                    <i class="fa fa-expand"></i>
                </a>
                <a href="#" data-toggle="collapse">
                    <i class="fa fa-minus"></i>
                </a>
            </div>
        </div>
        <div class="widget-body bordered-left">
            <div class="form-inline" role="form">
                <div class="row">
                    <div class="col-md-3">
                        <telerik:RadTextBox Skin="Office2010Blue" ID="txtUserName" Width="100%"
                            runat="server" EmptyMessage=" Input User Name">
                        </telerik:RadTextBox>
                    </div>
                    <div class="col-md-1">
                        <telerik:RadButton ID="btnSearch" Skin="Office2010Blue" runat="server" Text="Search" OnClick="btnSearch_Click" CausesValidation="False" />
                    </div>
                </div>
            </div>

            <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="False"
                CellSpacing="0" GridLines="None" Skin="Office2010Blue" Style="clear: both; margin-top: 10px;" RenderMode="Auto"
                AllowPaging="true" EnableHeaderContextMenu="true" FilterMenu-EnableEmbeddedBaseStylesheet="true"
                FilterType="HeaderContext" GroupingEnabled="false" EnableHeaderContextFilterMenu="true"
                PagerStyle-AlwaysVisible="true" AllowSorting="true" AllowFilteringByColumn="true" OnNeedDataSource="RadGrid1_NeedDataSource"
                OnUpdateCommand="RadGrid1_UpdateCommand" OnInsertCommand="RadGrid1_InsertCommand" OnDeleteCommand="RadGrid1_DeleteCommand" OnItemDataBound="RadGrid1_ItemDataBound">
                <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                </HeaderContextMenu>
                <PagerStyle Mode="NextPrevAndNumeric" />
                <AlternatingItemStyle BackColor="aliceblue" />
                <MasterTableView Name="Master" TableLayout="Fixed" DataKeyNames="UserId" EditMode="PopUp" CommandItemDisplay="Top">
                    <CommandItemSettings />
                    <Columns>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                            EditImageUrl="~/Images/Grid/grd_Edit.gif" EnableHeaderContextMenu="false" ShowFilterIcon="false">
                            <HeaderStyle HorizontalAlign="Center" Width="40px" />
                            <ItemStyle Width="40px" />
                        </telerik:GridEditCommandColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter UserId column"
                            UniqueName="UserId" HeaderText="UserId" DataField="UserId" ReadOnly="true" Display="false">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Width="70px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter UserName column"
                            UniqueName="UserName" HeaderText="UserName" DataField="UserName" ReadOnly="true"
                            ShowFilterIcon="false" AllowFiltering="false"
                            FilterCheckListEnableLoadOnDemand="true" SortExpression="UserName">
                            <HeaderStyle Width="120px" />
                            <ItemStyle Width="120px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridTemplateColumn HeaderText="FullName" EnableHeaderContextMenu="false"
                            UniqueName="FullName" FilterControlAltText="Filter FullName column"
                            SortExpression="FullName" CurrentFilterFunction="StartsWith" ShowFilterIcon="false" AllowFiltering="false">
                            <ItemTemplate>
                                <asp:Label ID="lbFullName" runat="server" Text='<%# Eval("FullName") %>'></asp:Label>
                                <telerik:RadToolTip RenderMode="Lightweight" ID="RadToolTip3" runat="server"
                                    TargetControlID="lbFullName" Width="300px"
                                    RelativeTo="Element" Position="MiddleRight" BorderColor="OrangeRed" ForeColor="Blue">
                                    <%# DataBinder.Eval(Container, "DataItem.FullName") %>
                                </telerik:RadToolTip>
                            </ItemTemplate>
                            <HeaderStyle Width="200px" />
                            <ItemStyle Width="200px" />
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridBoundColumn FilterControlAltText="Filter DepartmentId column"
                                                 UniqueName="DepartmentId" HeaderText="DepartmentId" DataField="DepartmentId" ReadOnly="true" Display="false">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Width="70px" />
                        </telerik:GridBoundColumn>
                        
                        <telerik:GridBoundColumn FilterControlAltText="Filter DepartmentName column"
                                                 UniqueName="DepartmentName" HeaderText="DepartmentName" DataField="DepartmentName" ReadOnly="true" Display="false">
                            
                        </telerik:GridBoundColumn>

                        <telerik:GridTemplateColumn HeaderText="DepartmentName" EnableHeaderContextMenu="false"
                            UniqueName="DepartmentNameExt" FilterControlAltText="Filter DepartmentName column"
                            SortExpression="DepartmentName" CurrentFilterFunction="StartsWith" ShowFilterIcon="false" AllowFiltering="false">
                            <ItemTemplate>
                                <asp:Label ID="lbDepartmentName" runat="server" Text='<%# Eval("DepartmentName") %>'></asp:Label>
                                <telerik:RadToolTip RenderMode="Lightweight" ID="RadToolTip1" runat="server"
                                    TargetControlID="lbDepartmentName" Width="300px"
                                    RelativeTo="Element" Position="MiddleRight" BorderColor="OrangeRed" ForeColor="Blue">
                                    <%# DataBinder.Eval(Container, "DataItem.DepartmentName") %>
                                </telerik:RadToolTip>
                            </ItemTemplate>
                            <HeaderStyle Width="200px" />
                            <ItemStyle Width="200px" />
                        </telerik:GridTemplateColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter RoleId column"
                            UniqueName="RoleId" HeaderText="RoleId" DataField="RoleId" ReadOnly="true" Display="false">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Width="70px" />
                        </telerik:GridBoundColumn>


                        <telerik:GridBoundColumn FilterControlAltText="Filter RoleName column"
                            UniqueName="RoleName" HeaderText="RoleName" DataField="RoleName" ReadOnly="true"
                            ShowFilterIcon="false" AllowFiltering="false"
                            FilterCheckListEnableLoadOnDemand="true" SortExpression="RoleName">
                            <HeaderStyle Width="120px" />
                            <ItemStyle Width="120px" />
                        </telerik:GridBoundColumn>
                        
                        <telerik:GridBoundColumn FilterControlAltText="Filter PositionId column"
                                                 UniqueName="PositionId" HeaderText="PositionId" DataField="PositionId" ReadOnly="true" Display="false">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Width="70px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter PositionName column"
                            UniqueName="PositionName" HeaderText="PositionName" DataField="PositionName" ReadOnly="true"
                            ShowFilterIcon="false" AllowFiltering="false"
                            FilterCheckListEnableLoadOnDemand="true" SortExpression="PositionName">
                            <HeaderStyle Width="140px" />
                            <ItemStyle Width="140px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter Email column"
                            UniqueName="Email" HeaderText="Email" DataField="Email" ReadOnly="true"
                            ShowFilterIcon="false" AllowFiltering="false"
                            FilterCheckListEnableLoadOnDemand="true" SortExpression="Email">
                            <HeaderStyle Width="240px" />
                            <ItemStyle Width="240px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridCheckBoxColumn HeaderText="Active" UniqueName="Active" DataField="Active" Display="true" ReadOnly="true">
                            <ItemStyle Width="120px" HorizontalAlign="Center"/>
                            <HeaderStyle Width="120px" HorizontalAlign="Center"/>
                        </telerik:GridCheckBoxColumn>

                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="B&#7841;n có ch&#7855;c ch&#7855;n mu&#7889;n xóa không?"
                            UniqueName="DeleteCommandColumn" ImageUrl="~/Images/Grid/vista_Delete.gif" EnableHeaderContextMenu="false">
                            <HeaderStyle HorizontalAlign="Center" Width="40px" />
                            <ItemStyle Width="40px" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <FormTemplate>
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="background-color: transparent !important; clear: both;">
                                <div class="widget flat">
                                    <div class="widget-body" style="background-color: transparent !important;">
                                        <div id="edit-form">
                                            <div class="row">
                                                <div class="col-sm-12">
                                                    <div class="form-group">
                                                        <asp:ValidationSummary ForeColor="Red" ID="rqSummary" runat="server" ValidationGroup="SummaryRequire" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-3">
                                                    <div class="form-group">
                                                        <label for="UserName">User Name</label>
                                                        <telerik:RadTextBox ID="UserName" Width="100%" runat="server"
                                                            Text='<%# DataBinder.Eval(Container.DataItem, "UserName") %>'>
                                                        </telerik:RadTextBox>
                                                        <asp:RequiredFieldValidator ID="requiredUserName" runat="server" Display="None"
                                                            ForeColor="Red" ControlToValidate="UserName" ErrorMessage="User Name is require" ValidationGroup="SummaryRequire">
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                               <div class="col-sm-3">
                                                    <div class="form-group">
                                                        <label for="Role">Role</label>
                                                        <telerik:RadComboBox ID="rcbRole" runat="server" Width="100%"
                                                            HighlightTemplatedItems="true"
                                                            EnableLoadOnDemand="true" Filter="Contains"
                                                            DataTextField="RoleName" DataValueField="RoleId" EnableItemCaching="true"
                                                            OnItemsRequested="rcbRole_ItemsRequested" AppendDataBoundItems="true"
                                                            ItemsPerRequest="10" ShowMoreResultsBox="true" EnableVirtualScrolling="true" Skin="Office2010Blue">
                                                            <HeaderTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td style="color: Black; width: 80px;">RoleId
                                                                        </td>
                                                                        <td style="color: Black; width: 300px;">RoleName
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td style="color: Black; width: 80px;">
                                                                            <%# DataBinder.Eval(Container.DataItem, "RoleId") %>
                                                                        </td>
                                                                        <td style="color: Black;">
                                                                            <%# DataBinder.Eval(Container.DataItem, "RoleName") %>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </telerik:RadComboBox>
                                                        <asp:RequiredFieldValidator ID="RequiredRole" runat="server" Display="None"
                                                            ForeColor="Red" ControlToValidate="rcbRole" ErrorMessage="Role is require" ValidationGroup="SummaryRequire">
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3">
                                                    <div class="form-group">
                                                        <label for="Position">Position</label>
                                                        <telerik:RadComboBox ID="rcbPosition" runat="server" Width="100%"
                                                            HighlightTemplatedItems="true"
                                                            EnableLoadOnDemand="true" Filter="Contains"
                                                            DataTextField="PositionName" DataValueField="Id" EnableItemCaching="true"
                                                            OnItemsRequested="rcbPosition_ItemsRequested" AppendDataBoundItems="true"
                                                            ItemsPerRequest="10" ShowMoreResultsBox="true" EnableVirtualScrolling="true" Skin="Office2010Blue">
                                                            <HeaderTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td style="color: Black; width: 80px;">Id
                                                                        </td>
                                                                        <td style="color: Black; width: 300px;">PositionName
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td style="color: Black; width: 80px;">
                                                                            <%# DataBinder.Eval(Container.DataItem, "Id") %>
                                                                        </td>
                                                                        <td style="color: Black;">
                                                                            <%# DataBinder.Eval(Container.DataItem, "PositionName") %>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </telerik:RadComboBox>
                                                        <asp:RequiredFieldValidator ID="RequiredPosition" runat="server" Display="None"
                                                            ForeColor="Red" ControlToValidate="rcbPosition" ErrorMessage="Position is require" ValidationGroup="SummaryRequire">
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3">
                                                    <div class="form-group">
                                                        <label for="Active">Active</label>
                                                        <div class="checkbox">
                                                            <label>
                                                                <asp:CheckBox ID="Active" runat="server" Checked='<%# Eval("Active") %>'></asp:CheckBox>
                                                            </label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-3">
                                                    <div class="form-group">
                                                        <label for="Email" id ="lbEmail" runat="server">Email</label>
                                                        <telerik:RadTextBox ID="Email" Width="100%" EmptyMessageStyle-ForeColor="Red" runat="server"
                                                            Text='<%# DataBinder.Eval(Container.DataItem, "Email") %>'>
                                                        </telerik:RadTextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3">
                                                    <div class="form-group">
                                                        <label for="Password" id ="lbPassword" runat="server">Password</label>
                                                        <telerik:RadTextBox ID="Password" Width="100%" EmptyMessageStyle-ForeColor="Red" runat="server">
                                                        </telerik:RadTextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <label for="Department" id ="lbDepartment" runat="server">Department</label>
                                                        <telerik:RadComboBox ID="rcbDepartment" runat="server" Width="100%"
                                                            HighlightTemplatedItems="true"
                                                            EnableLoadOnDemand="true" Filter="Contains"
                                                            DataTextField="DepartmentName" DataValueField="DepartmentId" EnableItemCaching="true"
                                                            OnItemsRequested="rcbDepartment_ItemsRequested" AppendDataBoundItems="true"
                                                            ItemsPerRequest="10" ShowMoreResultsBox="true" EnableVirtualScrolling="true" Skin="Office2010Blue">
                                                            <HeaderTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td style="color: Black; width: 80px;">DepartmentId
                                                                        </td>
                                                                        <td style="color: Black; width: 300px;">DepartmentName
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td style="color: Black; width: 80px;">
                                                                            <%# DataBinder.Eval(Container.DataItem, "DepartmentId") %>
                                                                        </td>
                                                                        <td style="color: Black;">
                                                                            <%# DataBinder.Eval(Container.DataItem, "DepartmentName") %>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </telerik:RadComboBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <telerik:RadButton ID="btnUpdate" Text='<%# Container is GridEditFormInsertItem ? "Insert" : "Update" %>'
                                                            ValidationGroup="SummaryRequire" runat="server" CommandName='<%# Container is GridEditFormInsertItem ? "PerformInsert" : "Update" %>'>
                                                            <Icon PrimaryIconCssClass="rbOk"></Icon>
                                                        </telerik:RadButton>
                                                        &nbsp;
                                                        <telerik:RadButton ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CommandName="Cancel">
                                                            <Icon PrimaryIconCssClass="rbCancel"></Icon>
                                                        </telerik:RadButton>
                                                        
                                                        <asp:HiddenField ID="hdPassword" runat="server" Value='<%# Eval("Password") %>' />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </FormTemplate>
                        <PopUpSettings Width="100%" Modal="true" KeepInScreenBounds="true"/>
                    </EditFormSettings>
                </MasterTableView>
                <ClientSettings>
                    <Scrolling AllowScroll="True" ScrollHeight="" FrozenColumnsCount="4" UseStaticHeaders="True" SaveScrollPosition="true" EnableNextPrevFrozenColumns="true"></Scrolling>
                </ClientSettings>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </telerik:RadGrid>

            <telerik:RadNotification RenderMode="Lightweight" ID="RadNotification1" runat="server" VisibleOnPageLoad="False" Position="BottomRight"
                Title="Notification" OffsetX="0" OffsetY="0" AnimationDuration="500" Opacity="100" AutoCloseDelay="3000"
                ContentScrolling="Default" Pinned="True" EnableRoundedCorners="True" KeepOnMouseOver="True" VisibleTitlebar="True"
                ShowCloseButton="True" ShowSound="None" Width="330" Height="160" Animation="Fade" EnableShadow="True" Style="z-index: 100000">
            </telerik:RadNotification>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageCssContent" runat="server">
    <style>
        div.RadComboBoxDropDown .rcbSlide {
            display: block !important;
        }

        .marginText {
            margin-top: 3px;
        }

        .RadGrid_Office2010Blue .rgMasterTable, .RadGrid_Office2010Blue .rgDetailTable, .RadGrid_Office2010Blue .rgGroupPanel table,
        .RadGrid_Office2010Blue .rgCommandRow table, .RadGrid_Office2010Blue .rgEditForm table, .RadGrid_Office2010Blue .rgPager table {
            line-height: 20px;
        }

        .RadGrid .rgInput, .RadGrid .rgEditRow > td > [type="text"], .RadGrid .rgEditForm td > [type="text"], .RadGrid .rgBatchContainer > [type="text"], .RadGrid .rgFilterBox, .RadGrid .rgFilterApply, .RadGrid .rgFilterCancel {
            border-width: 0;
            padding: 0;
        }

        .rgEditForm td {
            padding-left: 3px;
            padding-top: 3px;
        }

        .RadGrid_Office2010Blue .rgEditForm {
            border: none;
            padding: 0;
        }

        caption {
            padding-bottom: 0;
            padding-top: 0;
        }

        .GridDataDiv_Default {
            overflow-y: hidden !important;
        }

        .rmItem #ctl00_MainContent_RadGrid1_rghcMenu_i3_i1_chkctl00_MainContent_RadGrid1_ctl00SupplierId,
        label[for="ctl00_MainContent_RadGrid1_rghcMenu_i3_i1_chkctl00_MainContent_RadGrid1_ctl00SupplierId"] {
            border: 0;
            clip: rect(0 0 0 0);
            display: none;
            height: 1px;
            margin: -1px;
            padding: 0;
            visibility: hidden;
            width: 1px;
        }

        #ctl00_MainContent_RadGrid1_ctl00 input[type="checkbox"], input[type=radio] {
            -webkit-appearance: checkbox;
            border-radius: 0;
            left: 0;
            opacity: 1;
            position: relative;
        }

        ctl00_MainContent_RadGrid1_ctl00_ctl04_ctl00

        .rmVertical .rmFirst, .rmVertical .rmLast {
            border: 0;
            clip: rect(0 0 0 0);
            display: none;
            height: 1px;
            margin: -1px;
            padding: 0;
            visibility: hidden;
            width: 1px;
        }

        #edit-form input[type="checkbox"], input[type=radio] {
            -webkit-appearance: checkbox;
            border-radius: 0;
            left: 20px;
            opacity: 1;
            position: relative;
        }

        #edit-form .checkbox {
            margin-top: 0 !important
        }

        .rgMasterTable thead tr:nth-child(2) {
            background-color: white !important;
            border-top: none;
        }

        .rgMasterTable tbody tr td {
            border-bottom: solid 1px #aaa;
        }

        .rgMasterTable .widget {
            margin: 0 !important;
        }

        /*#ctl00_MainContent_txtName {
            width: 100%;
        }

        @media only screen and (min-width: 768px) {
            #ctl00_MainContent_txtName {
                width: 472px;
            }
        }*/

        #ctl00_MainContent_RadGrid1_GridData {
            overflow-y: hidden !important;
            width: 100%;
            height: auto !important;
        }

        .rddtSlide input[type="checkbox"], input[type=radio] {
            -webkit-appearance: checkbox;
            border-radius: 0;
            left: 0;
            opacity: 1;
            position: relative;
            margin-left: 0
        }

        .checkbox {
            margin: 0;
        }

        .RadGrid .rgEditPopup {
            top: -70px !important;
            left: 0 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PageScriptContent" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                    <telerik:AjaxUpdatedControl ControlID="RadNotification1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
</asp:Content>
