<%@ Page Title="My Issue" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ServiceDesk.WebApp.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="widget">
        <div class="widget-header bordered-left bordered-darkorange">
            <span class="widget-caption">My Issue Form</span>
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
                        <telerik:RadDatePicker ID="rdpFromDate"  runat="server" Width="100%" AutoPostBack="True" Skin="Office2010Blue" Culture="English (United States)">
                            <Calendar Skin="Office2010Blue" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                ViewSelectorText="x" ShowOtherMonthsDays="False" ShowRowHeaders="False">
                            </Calendar>
                            <DatePopupButton HoverImageUrl="" ImageUrl="" CssClass="" ToolTip="Từ ngày"></DatePopupButton>
                            <DateInput BackColor="#FFFFCC" DateFormat="dd/MM/yyyy" ToolTip="Từ ngày"></DateInput>
                        </telerik:RadDatePicker>
                        <div class="horizontal-space"></div>
                    </div>
                    <div class="col-md-3">
                        <telerik:RadDatePicker ID="rdpToDate" runat="server" Width="100%" AutoPostBack="True" Skin="Office2010Blue" Culture="English (United States)">
                            <Calendar Skin="Office2010Blue" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                ViewSelectorText="x" ShowOtherMonthsDays="False" ShowRowHeaders="False">
                            </Calendar>
                            <DatePopupButton HoverImageUrl="" ImageUrl="" CssClass="" ToolTip="Đến ngày"></DatePopupButton>
                            <DateInput BackColor="#FFFFCC" DateFormat="dd/MM/yyyy" ToolTip="Đến ngày"></DateInput>
                        </telerik:RadDatePicker>
                        <div class="horizontal-space"></div>
                    </div>
                    <div class="col-md-1">
                        <telerik:RadButton ID="btnSearch" Skin="Office2010Blue" runat="server" Text="Search" OnClick="btnSearch_Click" />
                    </div>
                </div>
                <%--<div class="row">
                    <div class="col-md-12">
                        <asp:CompareValidator ID="dateCompareValidator" runat="server" ControlToValidate="rdpToDate"
                            ControlToCompare="rdpFromDate" Operator="GreaterThan" Type="Date" ErrorMessage="Từ ngày ko được lớn hơn đến ngày">
                        </asp:CompareValidator>
                    </div>
                </div>--%>
            </div>

            <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="False"
                CellSpacing="0" GridLines="None" Skin="Office2010Blue" Style="clear: both; margin-top: 10px;" RenderMode="Auto"
                AllowPaging="true"
                PagerStyle-AlwaysVisible="true" AllowSorting="true" OnNeedDataSource="RadGrid1_NeedDataSource"
                OnUpdateCommand="RadGrid1_UpdateCommand" OnInsertCommand="RadGrid1_InsertCommand" OnDeleteCommand="RadGrid1_DeleteCommand" OnItemDataBound="RadGrid1_ItemDataBound">
                <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                </HeaderContextMenu>
                <PagerStyle Mode="NextPrevAndNumeric" />
                <AlternatingItemStyle BackColor="aliceblue" />
                <MasterTableView Name="Master" TableLayout="Fixed" DataKeyNames="Id" EditMode="PopUp" CommandItemDisplay="Top">
                    <CommandItemSettings />
                    <Columns>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                            EditImageUrl="~/Images/Grid/grd_Edit.gif" EnableHeaderContextMenu="false" ShowFilterIcon="false">
                            <HeaderStyle HorizontalAlign="Center" Width="40px" />
                            <ItemStyle Width="40px" />
                        </telerik:GridEditCommandColumn>

                        <telerik:GridTemplateColumn HeaderText="Chi tiết" EnableHeaderContextMenu="false"
                            UniqueName="Detail" FilterControlAltText="Filter Detail column"
                            SortExpression="Detail" CurrentFilterFunction="StartsWith"
                            ShowFilterIcon="false" AllowFiltering="true" ReadOnly="true">
                            <ItemTemplate>
                                <telerik:RadLinkButton ID="RadLinkButton15" runat="server" Text="Chi tiết" NavigateUrl='<%# "~/Issues/IssueTimeLife.aspx?IssueId=" + Eval("Id") %>' Target="_blank">
                                    <Icon CssClass="rbSearch"></Icon>
                                </telerik:RadLinkButton>
                            </ItemTemplate>
                            <HeaderStyle Width="120px" />
                            <ItemStyle Width="120px" HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter Id column"
                            UniqueName="Id" HeaderText="IssueId" DataField="Id" ReadOnly="true" Display="false">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Width="70px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridTemplateColumn HeaderText="Rating" EnableHeaderContextMenu="false"
                            UniqueName="StatusName" FilterControlAltText="Filter StatusName column"
                            SortExpression="StatusName" CurrentFilterFunction="StartsWith"
                            ShowFilterIcon="false" AllowFiltering="false">
                            <ItemTemplate>
                                <telerik:RadRating RenderMode="Lightweight" ID="RadRating1" runat="server"
                                    AutoPostBack="true" Value='<%# Convert.ToInt32(Eval("Rating")) %>'
                                    OnRate="RadRating1_Rate" Precision="Item">
                                </telerik:RadRating>
                            </ItemTemplate>
                            <HeaderStyle Width="120px" />
                            <ItemStyle Width="120px" HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter StatusId column"
                            UniqueName="StatusId" HeaderText="StatusId" DataField="StatusId" ReadOnly="true" Display="false">
                        </telerik:GridBoundColumn>

                        <telerik:GridTemplateColumn HeaderText="Tình trạng" EnableHeaderContextMenu="false"
                            UniqueName="StatusName" FilterControlAltText="Filter StatusName column"
                            SortExpression="StatusName" CurrentFilterFunction="StartsWith"
                            ShowFilterIcon="false" AllowFiltering="true" ReadOnly="true">
                            <ItemTemplate>
                                <asp:Label ID="lbStatus" Width="90px" runat="server" Text='<%# Eval("StatusName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="120px" />
                            <ItemStyle Width="120px" HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter EmployeeId column"
                            EnableHeaderContextMenu="false"
                            UniqueName="EmployeeId" HeaderText="Mã" DataField="EmployeeId" ReadOnly="true"
                            ShowFilterIcon="false" AllowFiltering="true"
                            FilterCheckListEnableLoadOnDemand="true" SortExpression="UserName">
                            <HeaderStyle Width="120px" />
                            <ItemStyle Width="120px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridTemplateColumn HeaderText="EmployeeName" EnableHeaderContextMenu="false"
                            FilterControlAltText="Filter Họ tên column"
                            SortExpression="EmployeeName" CurrentFilterFunction="StartsWith"
                            ShowFilterIcon="false" AllowFiltering="true" UniqueName="EmployeeName">
                            <ItemTemplate>
                                <asp:Label ID="lbEmployeeName" runat="server" Text='<%# Eval("EmployeeName") %>'></asp:Label>
                                <telerik:RadToolTip RenderMode="Lightweight" ID="RadToolTip3" runat="server"
                                    TargetControlID="lbEmployeeName" Width="300px"
                                    RelativeTo="Element" Position="MiddleRight" BorderColor="OrangeRed" ForeColor="Blue">
                                    <%# DataBinder.Eval(Container, "DataItem.EmployeeName") %>
                                </telerik:RadToolTip>
                            </ItemTemplate>
                            <HeaderStyle Width="160px" />
                            <ItemStyle Width="160px" />
                        </telerik:GridTemplateColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter Title column"
                            UniqueName="Title" HeaderText="Title" DataField="Title" ReadOnly="true"
                            ShowFilterIcon="false" AllowFiltering="true" EnableHeaderContextMenu="false"
                            FilterCheckListEnableLoadOnDemand="true" SortExpression="Title">
                            <HeaderStyle Width="160px" />
                            <ItemStyle Width="160px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter Email column"
                            UniqueName="Email" HeaderText="Email" DataField="Email" ReadOnly="true"
                            ShowFilterIcon="false" AllowFiltering="false" EnableHeaderContextMenu="false"
                            FilterCheckListEnableLoadOnDemand="true" SortExpression="Email">
                            <HeaderStyle Width="120px" />
                            <ItemStyle Width="120px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter Phone column" ShowFilterIcon="true"
                            UniqueName="Phone" HeaderText="Phone" EnableHeaderContextMenu="false" DataField="Phone" ReadOnly="true" Display="false">
                            <HeaderStyle Width="100px" />
                            <ItemStyle Width="100px" />
                        </telerik:GridBoundColumn>


                        <telerik:GridBoundColumn FilterControlAltText="Filter Mobile column"
                            UniqueName="Mobile" HeaderText="Mobile" DataField="Mobile" ReadOnly="true"
                            ShowFilterIcon="false" AllowFiltering="false" EnableHeaderContextMenu="false"
                            FilterCheckListEnableLoadOnDemand="true" SortExpression="Mobile">
                            <HeaderStyle Width="120px" />
                            <ItemStyle Width="120px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridTemplateColumn HeaderText="Description"
                            UniqueName="Description" EnableHeaderContextMenu="false"
                            FilterControlAltText="Filter Description column"
                            SortExpression="Description" CurrentFilterFunction="StartsWith"
                            ShowFilterIcon="false" AllowFiltering="true">
                            <ItemTemplate>
                                <asp:Label ID="lbDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                <telerik:RadToolTip RenderMode="Lightweight" ID="RadToolTip4" runat="server"
                                    TargetControlID="lbDescription" Width="300px"
                                    RelativeTo="Element" Position="MiddleRight" BorderColor="OrangeRed" ForeColor="Blue">
                                    <%# DataBinder.Eval(Container, "DataItem.Description") %>
                                </telerik:RadToolTip>
                            </ItemTemplate>
                            <HeaderStyle Width="200px" />
                            <ItemStyle Width="200px" />
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="DivisionName"
                            UniqueName="DivisionName" EnableHeaderContextMenu="false"
                            FilterControlAltText="Filter StatusName column"
                            SortExpression="StatusName" CurrentFilterFunction="StartsWith"
                            ShowFilterIcon="false" AllowFiltering="true">
                            <ItemTemplate>
                                <asp:Label ID="lbDivisionName" runat="server" Text='<%# Eval("DivisionName") %>'></asp:Label>
                                <telerik:RadToolTip RenderMode="Lightweight" ID="RadToolTip22" runat="server"
                                    TargetControlID="lbDivisionName" Width="300px"
                                    RelativeTo="Element" Position="MiddleRight" BorderColor="OrangeRed" ForeColor="Blue">
                                    <%# DataBinder.Eval(Container, "DataItem.DivisionName") %>
                                </telerik:RadToolTip>
                            </ItemTemplate>
                            <HeaderStyle Width="200px" />
                            <ItemStyle Width="200px" />
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="DepartmentName"
                            UniqueName="DepartmentName" EnableHeaderContextMenu="false"
                            FilterControlAltText="Filter StatusName column"
                            SortExpression="StatusName" CurrentFilterFunction="StartsWith"
                            ShowFilterIcon="false" AllowFiltering="true">
                            <ItemTemplate>
                                <asp:Label ID="lbDepartmentName" runat="server" Text='<%# Eval("DepartmentName") %>'></asp:Label>
                                <telerik:RadToolTip RenderMode="Lightweight" ID="RadToolTip11" runat="server"
                                    TargetControlID="lbDepartmentName" Width="300px"
                                    RelativeTo="Element" Position="MiddleRight" BorderColor="OrangeRed" ForeColor="Blue">
                                    <%# DataBinder.Eval(Container, "DataItem.DepartmentName") %>
                                </telerik:RadToolTip>
                            </ItemTemplate>
                            <HeaderStyle Width="200px" />
                            <ItemStyle Width="200px" />
                        </telerik:GridTemplateColumn>

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
                                                <div class="col-sm-12">
                                                    <div class="form-group">
                                                        <label for="Title">Title</label>
                                                        <telerik:RadTextBox ID="Title" Width="100%" runat="server" MaxLength="50"
                                                            Text='<%# DataBinder.Eval(Container.DataItem, "Title") %>'>
                                                        </telerik:RadTextBox>
                                                        <asp:RequiredFieldValidator ID="requiredTitle" runat="server" Display="None"
                                                            ForeColor="Red" ControlToValidate="Title" ErrorMessage="Title is require" ValidationGroup="SummaryRequire">
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-4">
                                                    <div class="form-group">
                                                        <label for="Email">Email</label>
                                                        <telerik:RadTextBox ID="Email" Width="100%" runat="server" MaxLength="50"
                                                            Text='<%# DataBinder.Eval(Container.DataItem, "Email") %>'>
                                                        </telerik:RadTextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-4">
                                                    <div class="form-group">
                                                        <label for="Mobile">Mobile</label>
                                                        <telerik:RadTextBox ID="Mobile" Width="100%" runat="server" MaxLength="50"
                                                            Text='<%# DataBinder.Eval(Container.DataItem, "Mobile") %>'>
                                                        </telerik:RadTextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredMobile" runat="server" Display="None"
                                                            ForeColor="Red" ControlToValidate="Mobile" ErrorMessage="Mobile is require" ValidationGroup="SummaryRequire">
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="col-sm-4">
                                                    <div class="form-group">
                                                        <label for="Phone">Phone</label>
                                                        <telerik:RadTextBox ID="Phone" Width="100%" runat="server" MaxLength="50"
                                                            Text='<%# DataBinder.Eval(Container.DataItem, "Phone") %>'>
                                                        </telerik:RadTextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-12">
                                                    <div class="form-group">
                                                        <label for="Description">Yêu cầu</label>
                                                        <telerik:RadTextBox ID="Description" runat="server" TextMode="MultiLine" Width="100%"
                                                            MaxLength="450" EnabledStyle-HorizontalAlign="Left" Text='<%# Bind("Description") %>'>
                                                        </telerik:RadTextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <telerik:RadButton ID="btnUpdate" Primary="true" Text='<%# Container is GridEditFormInsertItem ? "Insert" : "Update" %>'
                                                            ValidationGroup="SummaryRequire" runat="server" CommandName='<%# Container is GridEditFormInsertItem ? "PerformInsert" : "Update" %>'>
                                                            <Icon PrimaryIconCssClass="rbOk"></Icon>
                                                        </telerik:RadButton>
                                                        &nbsp;
                                                        <telerik:RadButton ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CommandName="Cancel">
                                                            <Icon PrimaryIconCssClass="rbCancel"></Icon>
                                                        </telerik:RadButton>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <asp:HiddenField ID="hdStatusId" runat="server" Value='<%# Eval("StatusId") %>' />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </FormTemplate>
                        <PopUpSettings Width="99%" Modal="true" KeepInScreenBounds="true" />
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

        .processing {
            background-color: #4CAF50;
            border-radius: 46px;
            padding: 3px
        }
        /* Green */
        .finish {
            background-color: #2196F3;
            border-radius: 46px;
            padding: 3px
        }
        /* Blue */
        .waiting {
            background-color: #ff9800;
            border-radius: 46px;
            padding: 3px
        }
        /* Orange */
        .cancel {
            background-color: #f44336;
            border-radius: 46px;
            padding: 3px
        }
        /* Red */
        .fausing {
            background-color: #e7e7e7;
            color: black;
            border-radius: 46px;
            padding: 3px
        }
        /* Gray */
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
                    <telerik:AjaxUpdatedControl ControlID="dateCompareValidator" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rdpFromDate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rdpToDate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
</asp:Content>

