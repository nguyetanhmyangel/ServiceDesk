<%@ Page Title="Công việc cần làm" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="TaskExecute.aspx.cs" Inherits="ServiceDesk.WebApp.Issues.TaskExecute" %>

<%@ Import Namespace="ServiceDesk.Utilities" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="widget">
        <div class="widget-header bordered-left bordered-darkorange">
            <span class="widget-caption">Form</span>
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
                        <telerik:RadDatePicker ID="rdpFromDate" OnSelectedDateChanged="FromDateSelectedDateChange" runat="server" Width="100%" AutoPostBack="True" Skin="Office2010Blue" Culture="English (United States)">
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
            </div>

            <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="False"
                CellSpacing="0" GridLines="None" Skin="Office2010Blue" Style="clear: both; margin-top: 10px;" RenderMode="Auto"
                AllowPaging="true" AllowSorting="true" OnNeedDataSource="RadGrid1_NeedDataSource"
                OnUpdateCommand="RadGrid1_UpdateCommand" OnItemDataBound="RadGrid1_ItemDataBound">
                <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                </HeaderContextMenu>
                <PagerStyle Mode="NextPrevAndNumeric" />
                <AlternatingItemStyle BackColor="aliceblue" />
                <MasterTableView Name="Master" TableLayout="Fixed" InsertItemPageIndexAction="ShowItemOnCurrentPage" DataKeyNames="Id" EditMode="PopUp" CommandItemDisplay="Top" NoMasterRecordsText="Không có dòng dữ liệu nào">
                    <CommandItemSettings ShowAddNewRecordButton="false" />
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

                       <%-- <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/Images/excel-btn-26.png"
                            CommandName="ExportCommandColumn" UniqueName="ExportCommandColumn"
                            ConfirmText="Bạn muốn export phiếu nhập?" EnableHeaderContextMenu="false">
                            <ItemStyle Width="50px" HorizontalAlign="Center" />
                            <HeaderStyle Width="50px" />
                        </telerik:GridButtonColumn>--%>

                        <telerik:GridBoundColumn FilterControlAltText="Filter Id column"
                            UniqueName="Id" HeaderText="IssueId" DataField="Id" ReadOnly="true" Display="false">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Width="70px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter TaskId column"
                            UniqueName="TaskId" HeaderText="TaskId" DataField="TaskId" ReadOnly="true" Display="false">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Width="70px" />
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

                        <telerik:GridTemplateColumn HeaderText="Tiến độ" EnableHeaderContextMenu="false"
                            UniqueName="Progress" FilterControlAltText="Filter Progress column"
                            SortExpression="Progress" CurrentFilterFunction="StartsWith"
                            ShowFilterIcon="false" AllowFiltering="true" ReadOnly="true">
                            <ItemTemplate>
                                <telerik:RadProgressBar runat="server" Width="140" ID="ItemProgressBar"
                                    BarType="Value" Value='<%# Helper.ConvertToInt(Eval("Progress")) %>' MinValue="0" MaxValue="100">
                                </telerik:RadProgressBar>
                            </ItemTemplate>
                            <HeaderStyle Width="165px" />
                            <ItemStyle Width="165px" />
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Tiêu đề" EnableHeaderContextMenu="false"
                            FilterControlAltText="Filter Title column"
                            SortExpression="Title" CurrentFilterFunction="StartsWith"
                            ShowFilterIcon="false" AllowFiltering="true" UniqueName="Title">
                            <ItemTemplate>
                                <asp:Label ID="lbTitle" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                                <telerik:RadToolTip RenderMode="Lightweight" ID="RadToolTip23" runat="server"
                                    TargetControlID="lbTitle" Width="300px"
                                    RelativeTo="Element" Position="MiddleRight" BorderColor="OrangeRed" ForeColor="Blue">
                                    <%# DataBinder.Eval(Container, "DataItem.Title") %>
                                </telerik:RadToolTip>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Họ tên" EnableHeaderContextMenu="false"
                            FilterControlAltText="Filter EmployeeName column"
                            SortExpression="EmployeeName" CurrentFilterFunction="StartsWith"
                            ShowFilterIcon="false" AllowFiltering="true" UniqueName="EmployeeName">
                            <ItemTemplate>
                                <asp:Label ID="lbCustomerEmployeeName" runat="server" Text='<%# Eval("EmployeeName") %>'></asp:Label>
                                <telerik:RadToolTip RenderMode="Lightweight" ID="RadToolTip3" runat="server"
                                    TargetControlID="lbCustomerEmployeeName" Width="300px"
                                    RelativeTo="Element" Position="MiddleRight" BorderColor="OrangeRed" ForeColor="Blue">
                                    <%# DataBinder.Eval(Container, "DataItem.EmployeeName") %>
                                </telerik:RadToolTip>
                            </ItemTemplate>
                            <HeaderStyle Width="180px" />
                            <ItemStyle Width="180px" />
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Đơn vị" EnableHeaderContextMenu="false" Display="False"
                            FilterControlAltText="Filter CustomerDivisionName column"
                            SortExpression="CustomerDivisionName" CurrentFilterFunction="StartsWith"
                            ShowFilterIcon="false" AllowFiltering="true" UniqueName="CustomerDivisionName">
                            <ItemTemplate>
                                <asp:Label ID="lbCustomerDivisionName" runat="server" Text='<%# Eval("CustomerDivisionName") %>'></asp:Label>
                                <telerik:RadToolTip RenderMode="Lightweight" ID="RadToolTip2" runat="server"
                                    TargetControlID="lbCustomerDivisionName" Width="200px"
                                    RelativeTo="Element" Position="MiddleRight" BorderColor="OrangeRed" ForeColor="Blue">
                                    <%# DataBinder.Eval(Container, "DataItem.CustomerDivisionName") %>
                                </telerik:RadToolTip>
                            </ItemTemplate>
                            <HeaderStyle Width="200px" />
                            <ItemStyle Width="200px" />
                        </telerik:GridTemplateColumn>

                        <telerik:GridDateTimeColumn DataField="StartDate" HeaderText="Bắt đầu" DataFormatString="{0:dd/MM/yyyy}" ColumnGroupName="ProcessingTime"
                            SortExpression="StartDate" CurrentFilterFunction="StartsWith" ShowFilterIcon="false" AllowFiltering="false"
                            UniqueName="StartDate" ReadOnly="true" Display="true">
                            <HeaderStyle Width="100px" />
                            <ItemStyle Width="100px" />
                            <ColumnValidationSettings></ColumnValidationSettings>
                        </telerik:GridDateTimeColumn>

                        <telerik:GridDateTimeColumn DataField="EndDate" HeaderText="Kết thúc" DataFormatString="{0:dd/MM/yyyy}" ColumnGroupName="ProcessingTime"
                            SortExpression="EndDate" CurrentFilterFunction="StartsWith" ShowFilterIcon="false" AllowFiltering="false"
                            UniqueName="EndDate" ReadOnly="true" Display="true">
                            <HeaderStyle Width="100px" />
                            <ItemStyle Width="100px" />
                            <ColumnValidationSettings></ColumnValidationSettings>
                        </telerik:GridDateTimeColumn>

                        <telerik:GridDateTimeColumn DataField="FinishDate" HeaderText="Hoàn thành" DataFormatString="{0:dd/MM/yyyy}" ColumnGroupName="ProcessingTime"
                            SortExpression="FinishDate" CurrentFilterFunction="StartsWith" ShowFilterIcon="false" AllowFiltering="false"
                            UniqueName="FinishDate" ReadOnly="true" Display="true">
                            <HeaderStyle Width="100px" />
                            <ItemStyle Width="100px" />
                            <ColumnValidationSettings></ColumnValidationSettings>
                        </telerik:GridDateTimeColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <FormTemplate>
                            <div class="page-body">
                                <div class="row">
                                    <div class="col-lg-12 col-sm-12 col-xs-12">
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
                                                                            Text='<%# DataBinder.Eval(Container.DataItem, "CustomerDivisionName") %>'>
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
                                                                <div style="padding-bottom: 4px"></div>
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
                                                                        <%--<telerik:RadComboBox ID="Status" runat="server" Width="100%"
                                                                            HighlightTemplatedItems="true" EnableLoadOnDemand="true" Filter="Contains"
                                                                            DataTextField="StatusName" DataValueField="Id" Skin="Office2010Blue">
                                                                        </telerik:RadComboBox>
                                                                        <asp:RequiredFieldValidator ID="StatusRequire" runat="server" ValidationGroup="SummaryRequire" Display="None"
                                                                            ForeColor="Red" ControlToValidate="Status" ErrorMessage="Tình trạng là bắt buộc">
                                                                        </asp:RequiredFieldValidator>--%>
                                                                        <telerik:RadTextBox ID="Status" Width="100%" runat="server" BackColor="GhostWhite" ReadOnly="true"
                                                                            Text='<%# DataBinder.Eval(Container.DataItem, "StatusName") %>'>
                                                                        </telerik:RadTextBox>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <label for="Users">Ưu tiên</label>
                                                                        <%--<telerik:RadComboBox ID="Priority" runat="server" Width="100%"
                                                                            HighlightTemplatedItems="true" EnableLoadOnDemand="true" Filter="Contains"
                                                                            DataTextField="PriorityName" DataValueField="Id" Skin="Office2010Blue">
                                                                        </telerik:RadComboBox>--%>
                                                                        <telerik:RadTextBox ID="PriorityName" Width="100%" runat="server" BackColor="GhostWhite" ReadOnly="true"
                                                                            Text='<%# DataBinder.Eval(Container.DataItem, "PriorityName") %>'>
                                                                        </telerik:RadTextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-md-6">
                                                                        <label for="StartDate">Ngày bắt đầu</label>
                                                                        <telerik:RadDatePicker ID="StartDate" Width="100%" DbSelectedDate='<%# Eval("StartDate") %>' runat="server"
                                                                            Skin="Office2010Blue" Culture="English (United States)">
                                                                            <Calendar Skin="Office2010Blue" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                                                                ViewSelectorText="x" ShowOtherMonthsDays="False" ShowRowHeaders="False">
                                                                            </Calendar>
                                                                            <DatePopupButton HoverImageUrl="" ImageUrl=""></DatePopupButton>
                                                                            <DateInput BackColor="White" DateFormat="dd/MM/yyyy" EmptyMessage="Ngày bắt đầu" Width="100%">
                                                                                <EmptyMessageStyle CssClass="MyEmptyTextBox"></EmptyMessageStyle>
                                                                            </DateInput>
                                                                        </telerik:RadDatePicker>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <label for="EndDate">Ngày kết thúc</label>
                                                                        <telerik:RadDatePicker ID="EndDate" Width="100%" DbSelectedDate='<%# Eval("EndDate") %>' runat="server" Skin="Office2010Blue" Culture="English (United States)">
                                                                            <Calendar Skin="Office2010Blue" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                                                                ViewSelectorText="x" ShowOtherMonthsDays="False" ShowRowHeaders="False">
                                                                            </Calendar>
                                                                            <DatePopupButton HoverImageUrl="" ImageUrl=""></DatePopupButton>
                                                                            <DateInput BackColor="White" DateFormat="dd/MM/yyyy" EmptyMessage="Ngày kết thúc">
                                                                                <EmptyMessageStyle CssClass="MyEmptyTextBox"></EmptyMessageStyle>
                                                                            </DateInput>
                                                                        </telerik:RadDatePicker>
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-md-6">
                                                                        <label for="FinishDate">Ngày hoàn thành</label>
                                                                        <telerik:RadDatePicker ID="FinishDate" Width="100%" DbSelectedDate='<%# Bind("FinishDate") %>' runat="server" Skin="Office2010Blue" Culture="English (United States)">
                                                                            <Calendar Skin="Office2010Blue" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                                                                ViewSelectorText="x" ShowOtherMonthsDays="False" ShowRowHeaders="False">
                                                                            </Calendar>
                                                                            <DatePopupButton HoverImageUrl="" ImageUrl=""></DatePopupButton>
                                                                            <DateInput BackColor="White" DateFormat="dd/MM/yyyy" EmptyMessage="Ngày hoàn thành">
                                                                                <EmptyMessageStyle CssClass="MyEmptyTextBox"></EmptyMessageStyle>
                                                                            </DateInput>
                                                                        </telerik:RadDatePicker>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <label for="Progress">Tiến độ</label>
                                                                        <telerik:RadSlider RenderMode="Lightweight" ID="Progress" runat="server" MinimumValue="0" MaximumValue="100" Width="100%"
                                                                            ItemType="Tick" TrackPosition="TopLeft" LargeChange="10" SmallChange="5"
                                                                            Value='<%# Helper.ConvertToInt(Eval("Progress")) %>'>
                                                                        </telerik:RadSlider>
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
                <ClientSettings>
                    <Scrolling AllowScroll="True" ScrollHeight="" UseStaticHeaders="True" SaveScrollPosition="true" EnableNextPrevFrozenColumns="true"></Scrolling>
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
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadNotification1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rdpFromDate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rdpToDate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbStatus">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
</asp:Content>
