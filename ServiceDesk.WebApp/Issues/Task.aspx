<%@ Page Title="Xử lí yêu cầu" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="Task.aspx.cs" Inherits="ServiceDesk.WebApp.Issues.Task" %>

<%@ Import Namespace="ServiceDesk.Utilities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="widget">
        <div class="widget-header bordered-left bordered-darkorange">
            <span class="widget-caption">Yêu cầu cần xử lí</span>
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
                    <div class="col-md-2">
                        <telerik:RadDatePicker ID="rdpFromDate" runat="server" OnSelectedDateChanged="FromDateSelectedDateChange" Width="100%" AutoPostBack="True" Skin="Office2010Blue" Culture="English (United States)">
                            <Calendar Skin="Office2010Blue" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                ViewSelectorText="x" ShowOtherMonthsDays="False" ShowRowHeaders="False">
                            </Calendar>
                            <DatePopupButton HoverImageUrl="" ImageUrl="" CssClass="" ToolTip="Từ ngày"></DatePopupButton>
                            <DateInput BackColor="#FFFFCC" DateFormat="dd/MM/yyyy" ToolTip="Từ ngày"></DateInput>
                        </telerik:RadDatePicker>
                        <div class="horizontal-space"></div>
                    </div>
                    <div class="col-md-2">
                        <telerik:RadDatePicker ID="rdpToDate" runat="server" OnSelectedDateChanged="ToDateSelectedDateChange" Width="100%" AutoPostBack="True" Skin="Office2010Blue" Culture="English (United States)">
                            <Calendar Skin="Office2010Blue" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                ViewSelectorText="x" ShowOtherMonthsDays="False" ShowRowHeaders="False">
                            </Calendar>
                            <DatePopupButton HoverImageUrl="" ImageUrl="" CssClass="" ToolTip="Đến ngày"></DatePopupButton>
                            <DateInput BackColor="#FFFFCC" DateFormat="dd/MM/yyyy" ToolTip="Đến ngày"></DateInput>
                        </telerik:RadDatePicker>
                        <div class="horizontal-space"></div>
                    </div>
                    <div class="col-md-2">
                        <telerik:RadComboBox ID="rcbStatus" runat="server" Width="100%"
                            HighlightTemplatedItems="true" AutoPostBack="true"
                            EnableLoadOnDemand="true" Filter="Contains" AllowCustomText="true" EmptyMessage="Select one status"
                            OnSelectedIndexChanged="rcbStatus_SelectedIndexChanged"
                            DataTextField="StatusName" DataValueField="Id" Skin="Office2010Blue">
                        </telerik:RadComboBox>
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
                AllowPaging="true" AllowSorting="true" OnNeedDataSource="RadGrid1_NeedDataSource"
                OnUpdateCommand="RadGrid1_UpdateCommand" OnItemDataBound="RadGrid1_ItemDataBound" OnDetailTableDataBind="RadGrid1_DetailTableDataBind">
                <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                </HeaderContextMenu>
                <PagerStyle Mode="NextPrevAndNumeric" PageSizeLabelText="Cỡ trang" PagerTextFormat="{4} trang {0}/{1}, dòng {2} đến {3}/{5}" />
                <%--<AlternatingItemStyle BackColor="aliceblue" />--%>
                <MasterTableView Name="Master" TableLayout="Fixed" InsertItemPageIndexAction="ShowItemOnCurrentPage" DataKeyNames="Id" EditMode="PopUp" CommandItemDisplay="Top"
                    NoMasterRecordsText="Không có dòng dữ liệu nào" NoDetailRecordsText="Không có dòng dữ liệu nào">
                    <CommandItemSettings ShowAddNewRecordButton="false" RefreshText="Làm mới" />
                    <DetailTables>
                        <telerik:GridTableView CommandItemDisplay="None" DataKeyNames="IssueId" Name="Detail" Width="100%">
                            <ParentTableRelation>
                                <telerik:GridRelationFields DetailKeyField="IssueId" MasterKeyField="Id" />
                            </ParentTableRelation>
                            <HeaderStyle CssClass="detailHeaderGrid" />
                            <ColumnGroups>
                                <telerik:GridColumnGroup HeaderText="Số lượng" Name="soluong" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridColumnGroup>
                                <telerik:GridColumnGroup HeaderText="Mã vật tư" Name="mavattu" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridColumnGroup>
                            </ColumnGroups>
                            <Columns>
                                <telerik:GridBoundColumn FilterControlAltText="Filter Id column"
                                    UniqueName="Id" HeaderText="Id" DataField="Id" ReadOnly="true" Display="false">
                                    <HeaderStyle Width="70px" />
                                    <ItemStyle Width="70px" />
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn FilterControlAltText="Filter IssueId column"
                                    UniqueName="IssueId" HeaderText="IssueId" DataField="Id" ReadOnly="true" Display="false">
                                    <HeaderStyle Width="70px" />
                                    <ItemStyle Width="70px" />
                                </telerik:GridBoundColumn>

                                <telerik:GridTemplateColumn HeaderText="Phòng ban xử lí" EnableHeaderContextMenu="false"
                                    FilterControlAltText="Filter DetailEmployeeName column"
                                    SortExpression="DetailEmployeeName" CurrentFilterFunction="StartsWith"
                                    ShowFilterIcon="false" AllowFiltering="true" UniqueName="DetailEmployeeName">
                                    <ItemTemplate>
                                        <asp:Label ID="lbDetailEmployeeName" runat="server" Text='<%# Eval("DepartmentName") %>'></asp:Label>
                                        <telerik:RadToolTip RenderMode="Lightweight" ID="RadToolTipEmployeeName" runat="server"
                                            TargetControlID="lbDetailEmployeeName" Width="300px"
                                            RelativeTo="Element" Position="MiddleRight" BorderColor="OrangeRed" ForeColor="Blue">
                                            <%# DataBinder.Eval(Container, "DataItem.DepartmentName") %>
                                        </telerik:RadToolTip>
                                    </ItemTemplate>
                                    <HeaderStyle Width="400px" />
                                    <ItemStyle Width="400px" />
                                </telerik:GridTemplateColumn>

                                <telerik:GridBoundColumn FilterControlAltText="Filter DetailStatusId column"
                                    UniqueName="DetailStatusId" HeaderText="DetailStatusId" DataField="StatusId" ReadOnly="true" Display="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridTemplateColumn HeaderText="Tình trạng" EnableHeaderContextMenu="false"
                                    UniqueName="DetailStatusName" FilterControlAltText="Filter DetailStatusName column"
                                    SortExpression="DetailStatusName" CurrentFilterFunction="StartsWith"
                                    ShowFilterIcon="false" AllowFiltering="true" ReadOnly="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lbDetailStatus" Width="90px" runat="server" Text='<%# Eval("StatusName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="120px" HorizontalAlign="Center" />
                                    <ItemStyle Width="120px" HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>

                                <telerik:GridDateTimeColumn DataField="StartDate" HeaderText="Bắt đầu" DataFormatString="{0:dd/MM/yyyy}" 
                                    SortExpression="StartDate" CurrentFilterFunction="StartsWith" ShowFilterIcon="false" AllowFiltering="false"
                                    UniqueName="StartDate" ReadOnly="true" Display="true">
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle Width="100px" />
                                    <ColumnValidationSettings></ColumnValidationSettings>
                                </telerik:GridDateTimeColumn>

                                <telerik:GridDateTimeColumn DataField="EndDate" HeaderText="Kết thúc" DataFormatString="{0:dd/MM/yyyy}" 
                                    SortExpression="EndDate" CurrentFilterFunction="StartsWith" ShowFilterIcon="false" AllowFiltering="false"
                                    UniqueName="EndDate" ReadOnly="true" Display="true">
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle Width="100px" />
                                    <ColumnValidationSettings></ColumnValidationSettings>
                                </telerik:GridDateTimeColumn>

                                <telerik:GridTemplateColumn HeaderText="Ghi chú" EnableHeaderContextMenu="false"
                                    FilterControlAltText="Filter DetailDescription column"
                                    SortExpression="Title" CurrentFilterFunction="StartsWith"
                                    ShowFilterIcon="false" AllowFiltering="true" UniqueName="Title">
                                    <ItemTemplate>
                                        <asp:Label ID="lbDetailDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                        <telerik:RadToolTip RenderMode="Lightweight" ID="RadToolTipDetailDescription" runat="server"
                                            TargetControlID="lbDetailDescription" Width="400px"
                                            RelativeTo="Element" Position="MiddleRight" BorderColor="OrangeRed" ForeColor="Blue">
                                            <%# DataBinder.Eval(Container, "DataItem.Description") %>
                                        </telerik:RadToolTip>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </telerik:GridTableView>
                    </DetailTables>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Người yêu cầu" Name="EmployeeGroup" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Thời gian" Name="ProcessingTime" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                            EditImageUrl="~/Images/edit-find-replace.png" EnableHeaderContextMenu="false" ShowFilterIcon="false">
                            <HeaderStyle HorizontalAlign="Center" Width="50px" />
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </telerik:GridEditCommandColumn>

                        <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/Images/Downloads-icon.png" HeaderText="Công văn"
                            Text="Export" CommandName="ExportCommandColumn" UniqueName="ExportCommandColumn"
                            ConfirmText="Bạn muốn export phiếu nhập?" EnableHeaderContextMenu="false">
                            <ItemStyle Width="70px" HorizontalAlign="Center" />
                            <HeaderStyle Width="70px" />
                        </telerik:GridButtonColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter Id column"
                            UniqueName="Id" HeaderText="Id" DataField="Id" ReadOnly="true" Display="false">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Width="70px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter DispatchPath column"
                            UniqueName="DispatchPath" HeaderText="DispatchPath" DataField="DispatchPath" ReadOnly="true" Display="false">
                        </telerik:GridBoundColumn>


                        <telerik:GridBoundColumn FilterControlAltText="Filter DepartmentId column"
                            UniqueName="DepartmentId" HeaderText="DepartmentId" DataField="DepartmentId" ReadOnly="true" Display="false">
                            <HeaderStyle Width="60px" />
                            <ItemStyle Width="60px" />
                        </telerik:GridBoundColumn>

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
                            <HeaderStyle Width="120px" HorizontalAlign="Center" />
                            <ItemStyle Width="120px" HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Ưu tiên" EnableHeaderContextMenu="false"
                            FilterControlAltText="Filter PriorityId column"
                            SortExpression="PriorityId" CurrentFilterFunction="StartsWith"
                            ShowFilterIcon="false" AllowFiltering="true" UniqueName="PriorityId">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdPriorityId" runat="server" Value='<%# Eval("PriorityId") %>' />
                                <asp:Image ID="imgFlag" runat="server" ImageUrl="~/Images/BlueFlag.PNG" />
                            </ItemTemplate>
                            <HeaderStyle Width="80px" HorizontalAlign="Center" />
                            <ItemStyle Width="80px" HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Tiêu đề" EnableHeaderContextMenu="false"
                            FilterControlAltText="Filter Title column"
                            SortExpression="Title" CurrentFilterFunction="StartsWith"
                            ShowFilterIcon="false" AllowFiltering="true" UniqueName="Title">
                            <ItemTemplate>
                                <asp:Label ID="lbTitle" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                                <telerik:RadToolTip RenderMode="Lightweight" ID="RadToolTipTitle" runat="server"
                                    TargetControlID="lbTitle" Width="300px"
                                    RelativeTo="Element" Position="MiddleRight" BorderColor="OrangeRed" ForeColor="Blue">
                                    <%# DataBinder.Eval(Container, "DataItem.Title") %>
                                </telerik:RadToolTip>
                            </ItemTemplate>
                            <HeaderStyle Width="300px" />
                            <ItemStyle Width="300px" />
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Nội dung" EnableHeaderContextMenu="false"
                            FilterControlAltText="Filter CustomerDescription column"
                            SortExpression="Title" CurrentFilterFunction="StartsWith"
                            ShowFilterIcon="false" AllowFiltering="true" UniqueName="Title">
                            <ItemTemplate>
                                <asp:Label ID="lbCustomerDescription" runat="server" Text='<%# Eval("CustomerDescription") %>'></asp:Label>
                                <telerik:RadToolTip RenderMode="Lightweight" ID="RadToolTipCustomerDescription" runat="server"
                                    TargetControlID="lbCustomerDescription" Width="300px"
                                    RelativeTo="Element" Position="MiddleRight" BorderColor="OrangeRed" ForeColor="Blue">
                                    <%# DataBinder.Eval(Container, "DataItem.CustomerDescription") %>
                                </telerik:RadToolTip>
                            </ItemTemplate>
                            <HeaderStyle Width="400px" />
                            <ItemStyle Width="400px" />
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Người yêu cầu" EnableHeaderContextMenu="false"
                            FilterControlAltText="Filter EmployeeName column"
                            SortExpression="EmployeeName" CurrentFilterFunction="StartsWith"
                            ShowFilterIcon="false" AllowFiltering="true" UniqueName="EmployeeName">
                            <ItemTemplate>
                                <asp:Label ID="lbCustomerEmployeeName" runat="server" Text='<%# Eval("EmployeeName") %>'></asp:Label>
                                <telerik:RadToolTip RenderMode="Lightweight" ID="RadToolTipEmployeeName" runat="server"
                                    TargetControlID="lbCustomerEmployeeName" Width="300px"
                                    RelativeTo="Element" Position="MiddleRight" BorderColor="OrangeRed" ForeColor="Blue">
                                    <%# DataBinder.Eval(Container, "DataItem.EmployeeName") %>
                                </telerik:RadToolTip>
                            </ItemTemplate>
                            <HeaderStyle Width="140px" />
                            <ItemStyle Width="140px" />
                        </telerik:GridTemplateColumn>

                        <telerik:GridDateTimeColumn DataField="CreateDate" HeaderText="Ngày YC" DataFormatString="{0:dd/MM/yyyy}"
                            SortExpression="CreateDate" CurrentFilterFunction="StartsWith" ShowFilterIcon="false" AllowFiltering="false"
                            UniqueName="CreateDate" ReadOnly="true" Display="true" DataType="System.DateTime">
                            <HeaderStyle Width="90px" />
                            <ItemStyle Width="90px" />
                            <ColumnValidationSettings></ColumnValidationSettings>
                        </telerik:GridDateTimeColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <FormTemplate>
                            <div class="page-body">
                                <div class="row">
                                    <div class="col-lg-12 col-sm-12 col-xs-12">
                                        <div class="row">
                                            <div class="row">

                                                <div class="col-lg-6 col-sm-6 col-xs-12">
                                                    <div class="widget">
                                                        <div class="widget-header bordered-bottom bordered-palegreen">
                                                            <span class="widget-caption">Chi tiết yêu cầu</span>
                                                        </div>
                                                        <div class="widget-body">
                                                            <div>
                                                                <div class="form-horizontal">
                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <label for="Title">Tiêu đề</label>
                                                                            <telerik:RadTextBox ID="Title" Width="100%" runat="server" BackColor="GhostWhite" ReadOnly="true"
                                                                                Text='<%# DataBinder.Eval(Container.DataItem, "Title") %>'>
                                                                            </telerik:RadTextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <label for="CustomerDivisionName">Đơn vị</label>
                                                                            <telerik:RadTextBox ID="CustomerDivisionName" Width="100%" runat="server" BackColor="GhostWhite" ReadOnly="true"
                                                                                Text='<%# DataBinder.Eval(Container.DataItem, "DivisionName") %>'>
                                                                            </telerik:RadTextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-6">
                                                                            <label for="EmployeeName">Người yêu cầu</label>
                                                                            <telerik:RadTextBox ID="EmployeeName" Width="100%" runat="server" BackColor="GhostWhite" ReadOnly="true"
                                                                                Text='<%# DataBinder.Eval(Container.DataItem, "EmployeeName") %>'>
                                                                            </telerik:RadTextBox>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <label for="Mobile">Điện thoại liên lạc</label>
                                                                            <telerik:RadTextBox ID="Mobile" Width="100%" runat="server" BackColor="GhostWhite" ReadOnly="true"
                                                                                Text='<%# DataBinder.Eval(Container.DataItem, "Mobile") %>'>
                                                                            </telerik:RadTextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-6">
                                                                            <label for="Phone">Số điện thoại khác</label>
                                                                            <telerik:RadTextBox ID="Phone" Width="100%" runat="server" BackColor="GhostWhite" ReadOnly="true"
                                                                                Text='<%# DataBinder.Eval(Container.DataItem, "Phone") %>'>
                                                                            </telerik:RadTextBox>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <label for="Email">Email</label>
                                                                            <telerik:RadTextBox ID="Email" Width="100%" runat="server" BackColor="GhostWhite" ReadOnly="true"
                                                                                Text='<%# DataBinder.Eval(Container.DataItem, "Email") %>'>
                                                                            </telerik:RadTextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <label for="CustomerDescription">Ghi chú yêu cầu</label>
                                                                            <telerik:RadTextBox ID="CustomerDescription" runat="server" ReadOnly="true" BackColor="GhostWhite" TextMode="MultiLine" Rows="2" Width="100%"
                                                                                EnabledStyle-HorizontalAlign="Left" Text='<%# Eval("CustomerDescription") %>'>
                                                                            </telerik:RadTextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6 col-sm-6 col-xs-12">
                                                    <div class="widget">
                                                        <div class="widget-header bordered-bottom bordered-pink">
                                                            <span class="widget-caption">Xử lí yêu cầu</span>
                                                        </div>
                                                        <div class="widget-body">
                                                            <div id="horizontal-form">
                                                                <div class="form-horizontal">
                                                                    <div class="row">

                                                                        <div class="col-md-6">
                                                                            <label for="Status">Tình trạng</label>
                                                                            <telerik:RadComboBox ID="Status" runat="server" Width="100%"
                                                                                HighlightTemplatedItems="true" EnableLoadOnDemand="true" Filter="Contains"
                                                                                DataTextField="StatusName" DataValueField="Id" Skin="Office2010Blue">
                                                                            </telerik:RadComboBox>
                                                                            <asp:RequiredFieldValidator ID="StatusRequire" runat="server" ValidationGroup="SummaryRequire" Display="None"
                                                                                ForeColor="Red" ControlToValidate="Status" ErrorMessage="Tình trạng là bắt buộc">
                                                                            </asp:RequiredFieldValidator>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <label for="Users">Ưu tiên</label>
                                                                            <telerik:RadComboBox ID="Priority" runat="server" Width="100%"
                                                                                HighlightTemplatedItems="true" EnableLoadOnDemand="true" Filter="Contains"
                                                                                DataTextField="PriorityName" DataValueField="Id" Skin="Office2010Blue">
                                                                            </telerik:RadComboBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <label for="OtherDepartments">Các phòng xử lí</label>
                                                                            <telerik:RadMultiSelect ID="OtherDepartments" runat="server" RenderMode="Lightweight" Filter="Contains" Height="80px" Width="100%" AutoPostBack="true"
                                                                                DataTextField="DepartmentName" DataValueField="DepartmentId">
                                                                            </telerik:RadMultiSelect>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <label for="Description">Ghi chú</label>
                                                                            <telerik:RadTextBox ID="Description" runat="server" TextMode="MultiLine" Rows="5" Width="100%"
                                                                                EnabledStyle-HorizontalAlign="Left" Text='<%# Eval("Description") %>'>
                                                                            </telerik:RadTextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="row">
                                            <div class="horizontal-space"></div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <telerik:RadButton ID="btnUpdate" Primary="true" Text='<%# Container is GridEditFormInsertItem ? "Create" : "Update" %>'
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
                                                    <asp:HiddenField ID="hdDepartmentId" runat="server" Value='<%# Eval("DepartmentId") %>' />
                                                    <asp:HiddenField ID="hdIssueId" runat="server" Value='<%# Eval("IssueId") %>' />
                                                    <asp:HiddenField ID="hdStatusId" runat="server" Value='<%# Eval("StatusId") %>' />
                                                    <asp:HiddenField ID="hdStatusName" runat="server" Value='<%# Eval("StatusName") %>' />
                                                    <%--                                                        <asp:HiddenField ID="hdOtherDepartmentId" runat="server" Value='<%# Eval("TransferDepartmentId") %>' />
                                                        <asp:HiddenField ID="hdOtherDepartmentName" runat="server" Value='<%# Eval("TransferDepartmentName") %>' />--%>
                                                    <asp:HiddenField ID="hdPriorityId" runat="server" Value='<%# Eval("PriorityId") %>' />
                                                    <asp:HiddenField ID="hdPriorityName" runat="server" Value='<%# Eval("PriorityName") %>' />
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
                <ClientSettings >
                    <Scrolling AllowScroll="True" ScrollHeight="" FrozenColumnsCount="4" UseStaticHeaders="True" SaveScrollPosition="true" EnableNextPrevFrozenColumns="true"></Scrolling>
                </ClientSettings>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </telerik:RadGrid>

            <telerik:RadNotification RenderMode="Lightweight" ID="RadNotification1" runat="server" VisibleOnPageLoad="False" Position="BottomRight"
                Title="Notification" OffsetX="0" OffsetY="0" AnimationDuration="1000" Opacity="100" AutoCloseDelay="3000"
                ContentScrolling="Default" Pinned="True" EnableRoundedCorners="True" KeepOnMouseOver="True" VisibleTitlebar="True"
                ShowCloseButton="True" ShowSound="None" Width="330" Height="100" Animation="Fade" EnableShadow="True" Style="z-index: 100000">
            </telerik:RadNotification>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageCssContent" runat="server">
    <%: Styles.Render("~/assets/css/custom.min.css") %>
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
            <telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
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
            <telerik:AjaxSetting AjaxControlID="rcbStatus">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
</asp:Content>
