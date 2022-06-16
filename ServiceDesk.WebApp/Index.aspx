<%@ Page Title="<%$ Resources:Resource,Yeucau %>" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ServiceDesk.WebApp.Index" %>

<%@ Import Namespace="ServiceDesk.Utilities" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
    <%--<ul class="breadcrumb">--%>
    <%--<li>
            <i class="fa fa-home"></i>
            <a href="~/Index.aspx"><%= GetGlobalResourceObject("Resource","Yeucau") %></a>
        </li>--%>
    <%--<li>
            <a href="#">Mail</a>
        </li>
        <li class="active">Compose Message</li>--%>
    <%--</ul>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="widget">
        <div class="widget-header bordered-left bordered-darkorange">
            <span class="widget-caption" runat="server"><%= GetGlobalResourceObject("Resource","Yeucau") %></span>
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
            <div class="form-inline" role="form" style="display: none;">
                <div class="row">
                    <div class="col-md-2">
                        <telerik:RadDatePicker ID="rdpFromDate" OnSelectedDateChanged="FromDateSelectedDateChange" runat="server" Width="100%" AutoPostBack="True" Skin="Office2010Blue" Culture="English (United States)">
                            <Calendar Skin="Office2010Blue" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                ViewSelectorText="x" ShowOtherMonthsDays="False" ShowRowHeaders="False">
                            </Calendar>
                            <DatePopupButton HoverImageUrl="" ImageUrl="" CssClass="" ToolTip="<%$ Resources:Resource,Tungay %>"></DatePopupButton>
                            <DateInput BackColor="#FFFFCC" DateFormat="dd/MM/yyyy" ToolTip="<%$ Resources:Resource,Tungay %>"></DateInput>
                        </telerik:RadDatePicker>
                        <div class="horizontal-space"></div>
                    </div>
                    <div class="col-md-2">
                        <telerik:RadDatePicker ID="rdpToDate" OnSelectedDateChanged="ToDateSelectedDateChange" runat="server" Width="100%" AutoPostBack="True" Skin="Office2010Blue" Culture="English (United States)">
                            <Calendar Skin="Office2010Blue" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                ViewSelectorText="x" ShowOtherMonthsDays="False" ShowRowHeaders="False">
                            </Calendar>
                            <DatePopupButton HoverImageUrl="" ImageUrl="" CssClass="" ToolTip="<%$ Resources:Resource,Denngay %>"></DatePopupButton>
                            <DateInput BackColor="#FFFFCC" DateFormat="dd/MM/yyyy" ToolTip="<%$ Resources:Resource,Denngay %>"></DateInput>
                        </telerik:RadDatePicker>
                        <div class="horizontal-space"></div>
                    </div>
                    <div class="col-md-2">
                        <telerik:RadComboBox ID="rcbStatus" runat="server" Width="100%"
                            HighlightTemplatedItems="true" AutoPostBack="true"
                            EnableLoadOnDemand="true" Filter="Contains" AllowCustomText="true" EmptyMessage="<%$ Resources:Resource,Chontinhtrang %>"
                            OnSelectedIndexChanged="rcbStatus_SelectedIndexChanged"
                            DataTextField="StatusName" DataValueField="Id" Skin="Office2010Blue">
                        </telerik:RadComboBox>
                    </div>
                    <div class="col-md-4" style="display: none">
                        <telerik:RadComboBox ID="rcbTag" runat="server" Width="100%"
                            HighlightTemplatedItems="true" AutoPostBack="true"
                            EnableLoadOnDemand="true" Filter="Contains" AllowCustomText="true" EmptyMessage="<%$ Resources:Resource,ChonloaiYC %>"
                            OnSelectedIndexChanged="rcbTag_SelectedIndexChanged"
                            DataTextField="TagName" DataValueField="Id" Skin="Office2010Blue">
                        </telerik:RadComboBox>
                    </div>
                    <div class="col-md-2" style="display: none">
                        <telerik:RadButton ID="btnSearch" Skin="Office2010Blue" runat="server" Text="Tìm kiếm" OnClick="btnSearch_Click" />
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
                AllowPaging="true" OnSelectedIndexChanged="RadGrid1_SelectedIndexChanged" OnItemCommand="RadGrid1_ItemCommand"
                PagerStyle-AlwaysVisible="true" AllowSorting="true" OnNeedDataSource="RadGrid1_NeedDataSource" OnItemCreated="RadGrid1_ItemCreated"
                OnUpdateCommand="RadGrid1_UpdateCommand" OnInsertCommand="RadGrid1_InsertCommand" OnDeleteCommand="RadGrid1_DeleteCommand" OnItemDataBound="RadGrid1_ItemDataBound">
                <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                </HeaderContextMenu>
                <PagerStyle Mode="NextPrevAndNumeric" />
                <%--<AlternatingItemStyle BackColor="aliceblue" />--%>
                <MasterTableView Name="Master" InsertItemPageIndexAction="ShowItemOnCurrentPage" TableLayout="Fixed" DataKeyNames="Id" EditMode="PopUp" CommandItemDisplay="Top"
                    CommandItemStyle-BackColor="LightBlue" CommandItemStyle-Font-Bold="true" CommandItemStyle-ForeColor="White"
                    NoMasterRecordsText="<%$ Resources:Resource,Khongcodulieu %>">
                    <CommandItemSettings AddNewRecordText="<%$ Resources:Resource,Taoyeucaumoi %>" RefreshText="" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="<%$ Resources:Resource,Nguoiyeucau %>" Name="EmployeeGroup" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>

                        <%--<telerik:GridTemplateColumn UniqueName="TemplateColumn">
                            <ItemTemplate>
                                <asp:CheckBox ID="checkColumn" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle Width="50px" HorizontalAlign="Center"/>
                            <ItemStyle Width="50px"  HorizontalAlign="Center"/>
                        </telerik:GridTemplateColumn>--%>


                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn" EditText="Chỉnh sửa yêu cầu"
                            EditImageUrl="~/Images/Grid/grd_Edit.gif" EnableHeaderContextMenu="false" ShowFilterIcon="false">
                            <HeaderStyle HorizontalAlign="Center" Width="40px" />
                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                        </telerik:GridEditCommandColumn>

                        <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/Images/Downloads-icon.png" HeaderText="<%$ Resources:Resource,Congvan %>" HeaderStyle-HorizontalAlign="Center"
                            Text="Dowload File" CommandName="DownloadCommandColumn" UniqueName="DownloadCommandColumn"
                            ConfirmText="<%$ Resources:Resource,Muondownload %>" EnableHeaderContextMenu="false">
                            <ItemStyle Width="80px" HorizontalAlign="Center" />
                            <HeaderStyle Width="80px" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter DispatchPath column"
                            UniqueName="DispatchPath" HeaderText="DispatchPath" DataField="DispatchPath"
                            ReadOnly="true" Display="false">
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter Id column"
                            UniqueName="Id" HeaderText="IssueId" DataField="Id" ReadOnly="true" Display="false">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Width="70px" />
                        </telerik:GridBoundColumn>

                        <%-- Tình trạng --%>
                        <telerik:GridTemplateColumn HeaderText="<%$ Resources:Resource,Tinhtrang %>" EnableHeaderContextMenu="false" HeaderStyle-HorizontalAlign="Center"
                            UniqueName="StatusName" FilterControlAltText="Filter StatusName column"
                            SortExpression="StatusName" CurrentFilterFunction="StartsWith"
                            ShowFilterIcon="false" AllowFiltering="true" ReadOnly="true">
                            <ItemTemplate>
                                <asp:Label ID="lbStatus" Width="90px" runat="server" Text='<%# Eval("StatusName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="120px" HorizontalAlign="Center" />
                            <ItemStyle Width="120px" HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>

                        <%--  Tiêu đề--%>
                        <telerik:GridTemplateColumn HeaderText="<%$ Resources:Resource,Tieude %>" EnableHeaderContextMenu="false" HeaderStyle-HorizontalAlign="Center"
                            FilterControlAltText="Filter Title column"
                            SortExpression="Title" CurrentFilterFunction="StartsWith"
                            ShowFilterIcon="false" AllowFiltering="true" UniqueName="Title">
                            <ItemTemplate>
                                <asp:Label ID="lbTitle" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                                <telerik:RadToolTip RenderMode="Lightweight" ID="RadToolTip23" runat="server"
                                    TargetControlID="lbTitle" Width="500px"
                                    RelativeTo="Element" Position="MiddleRight" BorderColor="OrangeRed" ForeColor="Blue">
                                    <%# DataBinder.Eval(Container, "DataItem.Title") %>
                                </telerik:RadToolTip>
                            </ItemTemplate>
                            <HeaderStyle Width="400px" />
                            <ItemStyle Width="400px" />
                        </telerik:GridTemplateColumn>

                        <%--Người yêu cầu--%>
                        <telerik:GridTemplateColumn HeaderText="<%$ Resources:Resource,Nguoiyeucau %>" EnableHeaderContextMenu="false" HeaderStyle-HorizontalAlign="Center"
                            FilterControlAltText="Filter EmployeeName column"
                            SortExpression="EmployeeName" CurrentFilterFunction="StartsWith"
                            ShowFilterIcon="false" AllowFiltering="true" UniqueName="EmployeeName">
                            <ItemTemplate>
                                <asp:Label ID="lbEmployee" runat="server" Text='<%# Eval("EmployeeName") %>'></asp:Label>
                                <telerik:RadToolTip RenderMode="Lightweight" ID="RadToolTip2" runat="server"
                                    TargetControlID="lbEmployee" Width="300px"
                                    RelativeTo="Element" Position="MiddleRight" BorderColor="OrangeRed" ForeColor="Blue">
                                    <%# DataBinder.Eval(Container, "DataItem.EmployeeName") %>
                                </telerik:RadToolTip>
                            </ItemTemplate>
                            <HeaderStyle Width="180px" HorizontalAlign="Center" />
                            <ItemStyle Width="180px" HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>

                        <%--<telerik:GridTemplateColumn HeaderText="Đơn vị" EnableHeaderContextMenu="false"
                                                    FilterControlAltText="Filter Đơn vị column"
                                                    SortExpression="DivisionName" CurrentFilterFunction="StartsWith"
                                                    ShowFilterIcon="false" AllowFiltering="true" UniqueName="DivisionName">
                            <ItemTemplate>
                                <asp:Label ID="lbDivisionName" runat="server" Text='<%# Eval("DivisionName") %>'></asp:Label>
                                <telerik:RadToolTip RenderMode="Lightweight" ID="RadToolTip3" runat="server"
                                                    TargetControlID="lbDivisionName" Width="200px"
                                                    RelativeTo="Element" Position="MiddleRight" BorderColor="OrangeRed" ForeColor="Blue">
                                    <%# DataBinder.Eval(Container, "DataItem.DivisionName") %>
                                </telerik:RadToolTip>
                            </ItemTemplate>
                            <HeaderStyle Width="180px" />
                            <ItemStyle Width="180px" />
                        </telerik:GridTemplateColumn>--%>

                        <telerik:GridDateTimeColumn DataField="CreateDate" HeaderText="<%$ Resources:Resource,Ngayyeucau %>" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="CreateDate" CurrentFilterFunction="StartsWith" ShowFilterIcon="false" AllowFiltering="false"
                            UniqueName="CreateDate" ReadOnly="true" Display="true" ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle Width="120px" />
                            <ItemStyle Width="120px" />
                            <ColumnValidationSettings></ColumnValidationSettings>
                        </telerik:GridDateTimeColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter StatusId column"
                            UniqueName="StatusId" HeaderText="StatusId" DataField="StatusId" ReadOnly="true" Display="false">
                        </telerik:GridBoundColumn>

                        <%--                        <telerik:GridTemplateColumn HeaderText="Loại yêu cầu" EnableHeaderContextMenu="false"
                                                    FilterControlAltText="Filter TagName column"
                                                    SortExpression="TagName" CurrentFilterFunction="StartsWith"
                                                    ShowFilterIcon="false" AllowFiltering="true" UniqueName="TagName">
                            <ItemTemplate>
                                <asp:Label ID="lbTagName" Font-Bold="True" runat="server"  Text='<%# Eval("TagName") %>'></asp:Label>
                                <telerik:RadToolTip RenderMode="Lightweight" ID="RadToolTip1" runat="server"
                                                    TargetControlID="lbTagName" Width="300px"
                                                    RelativeTo="Element" Position="MiddleRight" BorderColor="OrangeRed" ForeColor="Blue">
                                    <%# DataBinder.Eval(Container, "DataItem.TagName") %>
                                </telerik:RadToolTip>
                            </ItemTemplate>
                            <HeaderStyle Width="180px" />
                            <ItemStyle Width="180px" />
                        </telerik:GridTemplateColumn>
                        --%>

                        <telerik:GridTemplateColumn HeaderText="<%$ Resources:Resource,Danhgia %>" EnableHeaderContextMenu="false" HeaderStyle-HorizontalAlign="Center"
                            UniqueName="StatusName" FilterControlAltText="Filter StatusName column"
                            SortExpression="StatusName" CurrentFilterFunction="StartsWith"
                            ShowFilterIcon="false" AllowFiltering="false">
                            <ItemTemplate>
                                <telerik:RadRating RenderMode="Lightweight" ID="RadRating1" runat="server"
                                    AutoPostBack="true" Value='<%# Convert.ToInt32(Eval("Rating")) %>'
                                    OnRate="RadRating1_Rate" Precision="Item">
                                </telerik:RadRating>
                            </ItemTemplate>
                            <HeaderStyle Width="120px" HorizontalAlign="Center" />
                            <ItemStyle Width="120px" HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="<%$ Resources:Resource,YkienKH %>"
                            EnableHeaderContextMenu="false" HeaderStyle-HorizontalAlign="Center"
                            FilterControlAltText="Filter Title column"
                            SortExpression="Review" CurrentFilterFunction="StartsWith"
                            ShowFilterIcon="false" AllowFiltering="true" UniqueName="Review">
                            <ItemTemplate>
                                <asp:Label ID="lbReview" runat="server" Text='<%# Eval("Review") %>'></asp:Label>
                                <telerik:RadToolTip RenderMode="Lightweight" ID="RadToolTip33" runat="server"
                                    TargetControlID="lbReview" Width="500px"
                                    RelativeTo="Element" Position="MiddleRight" BorderColor="OrangeRed" ForeColor="Blue">
                                    <%# DataBinder.Eval(Container, "DataItem.Review") %>
                                </telerik:RadToolTip>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn UniqueName="TemplateEditColumn">
                            <ItemTemplate>
                                <asp:HyperLink ID="EditLink" runat="server">
                                    <asp:Image runat="server" ImageUrl="~/Images/Grid/vista_Delete.gif"/>
                                </asp:HyperLink>

                            </ItemTemplate>
                            <HeaderStyle Width="40px" />
                            <ItemStyle Width="40px" />
                        </telerik:GridTemplateColumn>

                        <%--<telerik:GridTemplateColumn HeaderText="Chi tiết" EnableHeaderContextMenu="false"
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
                        </telerik:GridTemplateColumn>--%>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <FormTemplate>
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="background-color: transparent !important; clear: both;">
                                <div class="widget flat">
                                    <div class="widget-body" style="background-color: transparent !important;">
                                        <div id="edit-form">
                                            <div class="row">
                                                <div class="col-sm-9">
                                                    <div class="form-group">
                                                        <%--   Tiêu đề--%>
                                                        <label for="Title" runat="server"><%= GetGlobalResourceObject("Resource","Tieude") %> </label>
                                                        <label style="color: red" for="Title">(*)</label>
                                                        <telerik:RadTextBox ID="Title" Width="100%" runat="server" MaxLength="100"
                                                            Text='<%# DataBinder.Eval(Container.DataItem, "Title") %>'>
                                                        </telerik:RadTextBox>
                                                        <asp:RequiredFieldValidator ID="requiredTitle" runat="server" Display="Static"
                                                            ForeColor="Red" ControlToValidate="Title" ErrorMessage="<%$ Resources:Resource,Tieudebatbuoc %>">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator Display="Static" ControlToValidate="Title"
                                                            ID="RegularExpressionValidator1" ValidationExpression="^[\s\S]{0,100}$" runat="server"
                                                            ForeColor="Red" ErrorMessage="<%$ Resources:Resource,TD100kytu %>"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>                                                
                                            </div>
                                            <div class="row" id="twoRow" runat="server">
                                                <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <label for="DivisionName" runat="server" id="lbDivision"><%= GetGlobalResourceObject("Resource","Donvi") %></label>
                                                        <%--<asp:Label ID="lbDivision" runat="server" Text="Đơn vị" for="lbDivision"></asp:Label>--%>
                                                        <telerik:RadTextBox ID="DivisionName" Width="100%" runat="server" BackColor="GhostWhite" ReadOnly="true"
                                                            Text='<%# DataBinder.Eval(Container.DataItem, "DivisionName") %>'>
                                                        </telerik:RadTextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3">
                                                    <div class="form-group">
                                                        <label for="EmployeeName" runat="server" id="lbEmployeeName"><%= GetGlobalResourceObject("Resource","Hoten") %></label>
                                                        <telerik:RadTextBox ID="EmployeeName" Width="100%" runat="server" BackColor="GhostWhite" ReadOnly="true"
                                                            Text='<%# DataBinder.Eval(Container.DataItem, "EmployeeName") %>'>
                                                        </telerik:RadTextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" runat="server">
                                                <%--  Mã nhân viên--%>
                                                <div class="col-sm-3">
                                                    <div class="form-group">

                                                        <label for="Title" runat="server"><%= GetGlobalResourceObject("Resource","MaNV") %> </label>
                                                        <label style="color: red" for="Title">(*)</label>
                                                        <telerik:RadTextBox ID="EmployeeId" Width="100%" runat="server" MaxLength="50"
                                                            Text='<%# DataBinder.Eval(Container.DataItem, "EmployeeId") %>'>
                                                        </telerik:RadTextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldEmployeeId" runat="server" Display="Static"
                                                            ForeColor="Red" ControlToValidate="EmployeeId" ErrorMessage="<%$ Resources:Resource,MNVbatbuoc %>">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:CustomValidator ID="EmployeeIdValidator" runat="server"
                                                            ErrorMessage="<%$ Resources:Resource,MNVkotontai%>" Display="Static"
                                                            ControlToValidate="EmployeeId" ForeColor="Red"
                                                            OnServerValidate="EmployeeIdValidator_ServerValidate">
                                                        </asp:CustomValidator>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3">
                                                    <div class="form-group">
                                                        <%-- Điện thoại liên hệ--%>
                                                        <label for="Mobile" runat="server"><%= GetGlobalResourceObject("Resource","SoDt") %> </label>
                                                        <label style="color: red" for="Title">(*)</label>
                                                        <telerik:RadTextBox ID="Mobile" Width="100%" runat="server" MaxLength="50"
                                                            Text='<%# DataBinder.Eval(Container.DataItem, "Mobile") %>'>
                                                        </telerik:RadTextBox>
                                                        <%--Số điện thoại là bắt buộc--%>
                                                        <asp:RequiredFieldValidator ID="RequiredMobile" runat="server" Display="Static"
                                                            ForeColor="Red" ControlToValidate="Mobile"
                                                            ErrorMessage="<%$ Resources:Resource,SDTbatbuoc %>">
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <%--<div class="col-sm-3">
                                                    <div class="form-group">
                                                        <label for="Tags" runat="server"><%= GetGlobalResourceObject("Resource","LoaiYC") %> </label>
                                                        <label style="color: red" for="Title">(*)</label>
                                                        <telerik:RadComboBox ID="Tags" runat="server" Width="100%" EmptyMessage="<%$ Resources:Resource,ChonloaiYC %>" AllowCustomText="True"
                                                                             HighlightTemplatedItems="true" EnableLoadOnDemand="true" Filter="Contains"
                                                                             DataTextField="TagName" DataValueField="Id" Skin="Office2010Blue">
                                                        </telerik:RadComboBox>
                                                        <asp:RequiredFieldValidator ID="RequiredTag" runat="server" Display="Static"
                                                                                    ForeColor="Red" ControlToValidate="Tags"
                                                                                    ErrorMessage="<%$ Resources:Resource,ChonloaiYC %>">
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>--%>
                                                <div class="col-sm-3">
                                                    <div class="form-group">
                                                        <label for="Email">Email</label>
                                                        <telerik:RadTextBox ID="Email" Width="100%" runat="server" MaxLength="50"
                                                            Text='<%# DataBinder.Eval(Container.DataItem, "Email") %>'>
                                                        </telerik:RadTextBox>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="Email"
                                                            ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                                            Display="Static" ErrorMessage="Email không tồn tại" />
                                                    </div>
                                                </div>
                                                <div class="col-sm-3">
                                                    <label for="Path"><%= GetGlobalResourceObject("Resource","Upload") %></label>
                                                    <telerik:RadAsyncUpload runat="server" ID="Path" Width="100%"
                                                        onvalidatingfile="ValidatingFile" ToolTip="<%$ Resources:Resource,UploadMax100Mb %>"
                                                        MaxFileInputsCount="1" OnFileUploaded="Path_FileUploaded"
                                                        AllowedFileExtensions="jpg,jpeg,PNG,gif,word,pdf,tiff,tif,zip" MaxFileSize="104857600">
                                                    </telerik:RadAsyncUpload>
                                                    <asp:HiddenField ID="hdPath" runat="server" Value='<%# Eval("DispatchPath") %>' />
                                                </div>
                                            </div>
                                            <div class="row" id="fourRow" runat="server">
                                                <div class="col-sm-12">
                                                    <div class="form-group">
                                                        <label for="Description" runat="server" id="lbInsertDescription"><%= GetGlobalResourceObject("Resource","NoidungYC") %> </label>
                                                        <telerik:RadTextBox ID="InsertDescription" runat="server" TextMode="MultiLine" Width="100%"
                                                            MaxLength="450" EnabledStyle-HorizontalAlign="Left" Text='<%# Bind("CustomerDescription") %>'>
                                                        </telerik:RadTextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row" id="fiveRow" runat="server">
                                                <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <label for="Description" runat="server" id="lbUpdateDescription"><%= GetGlobalResourceObject("Resource","NoidungYC") %> </label>
                                                        <telerik:RadTextBox ID="UpdateDescription" runat="server" TextMode="MultiLine" Width="100%"
                                                            MaxLength="450" EnabledStyle-HorizontalAlign="Left" Text='<%# Bind("CustomerDescription") %>'>
                                                        </telerik:RadTextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <%--Ý kiến--%>
                                                        <label for="Review" runat="server" id="lbReView"><%= GetGlobalResourceObject("Resource","YkienKH") %>  </label>
                                                        <telerik:RadTextBox ID="Review" Width="100%" runat="server" TextMode="MultiLine"
                                                            Text='<%# DataBinder.Eval(Container.DataItem, "Review") %>'>
                                                        </telerik:RadTextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">

                                                <div class="col-md-6">
                                                    <div class="form-group">

                                                        <telerik:RadButton VerticalAlign="right" ID="btnUpdate" Primary="true" Text='<%# Container is GridEditFormInsertItem ? "Save" : "Update" %>'
                                                            runat="server" CommandName='<%# Container is GridEditFormInsertItem ? "PerformInsert" : "Update" %>'>
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
                                                        <asp:HiddenField ID="hdCreateDate" runat="server" Value='<%# Eval("CreateDate") %>' />
                                                        <asp:HiddenField ID="hdTagId" runat="server" Value='<%# Eval("TagId") %>' />
                                                        <asp:HiddenField ID="hdTagName" runat="server" Value='<%# Eval("TagName") %>' />
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
                <ClientSettings EnablePostBackOnRowClick="True" Selecting-AllowRowSelect="true">
                    <Scrolling AllowScroll="True" ScrollHeight="" FrozenColumnsCount="4" UseStaticHeaders="True" SaveScrollPosition="true" EnableNextPrevFrozenColumns="true"></Scrolling>
                </ClientSettings>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </telerik:RadGrid>

            <telerik:RadNotification RenderMode="Lightweight" ID="RadNotification1" runat="server" VisibleOnPageLoad="False" Position="BottomRight"
                Title="Notification" OffsetX="0" OffsetY="0" AnimationDuration="500" Opacity="100" AutoCloseDelay="7000"
                ContentScrolling="Default" Pinned="True" EnableRoundedCorners="True" KeepOnMouseOver="True" VisibleTitlebar="True"
                ShowCloseButton="True" ShowSound="None" Width="700" Height="160" Animation="Fade" EnableShadow="True" Style="z-index: 100000">
            </telerik:RadNotification>
        </div>
    </div>

    <!--Cancel Issue Modal Templates-->
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
        Behaviors="Move,Close" EnableShadow="true" Top="10px" OnClientShow="setCustomPosition">
        <Windows>
            <telerik:RadWindow RenderMode="Lightweight" ID="UserListDialog" runat="server" Height="250px"
                Width="800px" ReloadOnShow="true" ShowContentDuringLoad="false"
                Modal="true" VisibleStatusbar="False">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <!--End Cancel Issue Templates-->

    <div class="widget" id="detailWidget" runat="server">
        <div class="widget-header bordered-left bordered-darkorange">
            <%-- Chi tiết--%>
            <span class="widget-caption" runat="server"><%= GetGlobalResourceObject("Resource","Chitiet") %> </span>
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
                    <div class="col-md-12">
                        <div id="WiredWizard" class="wizard wizard-wired">

                            <%-- if owner Issues cancel --%>
                            <ul class="steps" id="OwnerCancel" runat="server">
                                <li data-target="#wiredstep1" class="cancel"><span class="step">1</span><span class="title"><%= GetGlobalResourceObject("Resource","HuyYC") %></span><span class="chevron"></span></li>
                                <li data-target="#wiredstep2"><span class="step">2</span><span class="title"><%= GetGlobalResourceObject("Resource","NhanYC") %></span> <span class="chevron"></span></li>
                                <li data-target="#wiredstep3"><span class="step">3</span><span class="title"><%= GetGlobalResourceObject("Resource","XulyYC") %></span> <span class="chevron"></span></li>
                                <li data-target="#wiredstep4"><span class="step">4</span><span class="title"><%= GetGlobalResourceObject("Resource","Ketthuc") %></span> <span class="chevron"></span></li>
                            </ul>
                            <%-- if all Task is waiting --%>
                            <ul class="steps" id="AllWaiting" runat="server">
                                <li data-target="#wiredstep1" class="complete"><span class="step">1</span><span class="title"><%= GetGlobalResourceObject("Resource","GuiYC") %></span><span class="chevron"></span></li>
                                <li data-target="#wiredstep2" class="active"><span class="step">2</span><span class="title"><%= GetGlobalResourceObject("Resource","NhanYC") %></span> <span class="chevron"></span></li>
                                <li data-target="#wiredstep3"><span class="step">3</span><span class="title"><%= GetGlobalResourceObject("Resource","XulyYC") %></span> <span class="chevron"></span></li>
                                <li data-target="#wiredstep4"><span class="step">4</span><span class="title"><%= GetGlobalResourceObject("Resource","Ketthuc") %></span> <span class="chevron"></span></li>
                            </ul>
                            <%--if all tasks complete --%>
                            <ul class="steps" id="Allcomplete" runat="server">
                                <li data-target="#wiredstep1" class="complete"><span class="step">1</span><span class="title"><%= GetGlobalResourceObject("Resource","GuiYC") %></span><span class="chevron"></span></li>
                                <li data-target="#wiredstep2" class="complete"><span class="step">2</span><span class="title"><%= GetGlobalResourceObject("Resource","NhanYC") %></span> <span class="chevron"></span></li>
                                <li data-target="#wiredstep3" class="complete"><span class="step">3</span><span class="title"><%= GetGlobalResourceObject("Resource","XulyYC") %></span> <span class="chevron"></span></li>
                                <li data-target="#wiredstep4" class="complete"><span class="step">4</span><span class="title"><%= GetGlobalResourceObject("Resource","Ketthuc") %></span> <span class="chevron"></span></li>
                            </ul>
                            <%--if all tasks cancel--%>
                            <ul class="steps" id="AllCancel" runat="server">
                                <li data-target="#wiredstep1" class="complete"><span class="step">1</span><span class="title"><%= GetGlobalResourceObject("Resource","GuiYC") %></span><span class="chevron"></span></li>
                                <li data-target="#wiredstep2" class="complete"><span class="step">2</span><span class="title"><%= GetGlobalResourceObject("Resource","NhanYC") %></span> <span class="chevron"></span></li>
                                <li data-target="#wiredstep3" class="complete"><span class="step">3</span><span class="title"><%= GetGlobalResourceObject("Resource","XulyYC") %></span> <span class="chevron"></span></li>
                                <li data-target="#wiredstep4" class="cancel"><span class="step">4</span><span class="title"><%= GetGlobalResourceObject("Resource","HuyYC") %></span> <span class="chevron"></span></li>
                            </ul>
                            <%--if exist user handle tasks --%>
                            <ul class="steps" id="Handle" runat="server">
                                <li data-target="#wiredstep1" class="complete"><span class="step">1</span><span class="title"><%= GetGlobalResourceObject("Resource","GuiYC") %></span><span class="chevron"></span></li>
                                <li data-target="#wiredstep2" class="complete"><span class="step">2</span><span class="title"><%= GetGlobalResourceObject("Resource","NhanYC") %></span> <span class="chevron"></span></li>
                                <li data-target="#wiredstep3" class="complete"><span class="step">3</span><span class="title"><%= GetGlobalResourceObject("Resource","XulyYC") %></span> <span class="chevron"></span></li>
                                <li data-target="#wiredstep4" class="active"><span class="step">4</span><span class="title"><%= GetGlobalResourceObject("Resource","Ketthuc") %></span> <span class="chevron"></span></li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>

            <telerik:RadGrid ID="RadGrid2" runat="server" AutoGenerateColumns="False"
                CellSpacing="0" GridLines="None" Skin="Office2010Blue" Style="clear: both; margin-top: 10px;" RenderMode="Auto"
                AllowPaging="true" AllowSorting="true" OnNeedDataSource="RadGrid2_NeedDataSource">
                <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                </HeaderContextMenu>
                <PagerStyle Mode="NextPrevAndNumeric" />
                <AlternatingItemStyle BackColor="aliceblue" />
                <MasterTableView Name="Master" TableLayout="Fixed" DataKeyNames="Id" EditMode="PopUp" CommandItemDisplay="Top" NoMasterRecordsText="">
                    <CommandItemSettings ShowAddNewRecordButton="false" RefreshText="" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="<%$ Resources:Resource,Nguoixuly%>" Name="EmployeeGroup" HeaderStyle-HorizontalAlign="Center">
                            <%--Người xử lí--%>
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Thời gian" Name="ProcessingTime" HeaderStyle-HorizontalAlign="Center">
                            <%--Thời gian--%>
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <%-- Tiến độ--%>
                        <telerik:GridTemplateColumn HeaderText="<%$ Resources:Resource,Tiendo%>" EnableHeaderContextMenu="false" HeaderStyle-HorizontalAlign="Center"
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

                        <%--<telerik:GridBoundColumn FilterControlAltText="Filter EmployeeId column" 
                                                 EnableHeaderContextMenu="false" ColumnGroupName="EmployeeGroup"
                                                 UniqueName="EmployeeId" HeaderText="Mã NV" DataField="EmployeeId" ReadOnly="true"
                                                 ShowFilterIcon="false" AllowFiltering="true"
                                                 FilterCheckListEnableLoadOnDemand="true" SortExpression="EmployeeId">
                            <HeaderStyle Width="100px" />
                            <ItemStyle Width="100px" />
                        </telerik:GridBoundColumn>--%>
                        <%--Người xử lí--%>
                        <telerik:GridTemplateColumn HeaderText="<%$ Resources:Resource,Nguoixuly%>" EnableHeaderContextMenu="false" HeaderStyle-HorizontalAlign="Center"
                            FilterControlAltText="Filter EmployeeName column"
                            SortExpression="EmployeeName" CurrentFilterFunction="StartsWith"
                            ShowFilterIcon="false" AllowFiltering="true" UniqueName="EmployeeName">
                            <ItemTemplate>
                                <asp:Label ID="lbRequestEmployeeName" runat="server" Text='<%# Eval("EmployeeName") %>'></asp:Label>
                                <telerik:RadToolTip RenderMode="Lightweight" ID="RadToolTip3" runat="server"
                                    TargetControlID="lbRequestEmployeeName" Width="300px"
                                    RelativeTo="Element" Position="MiddleRight" BorderColor="OrangeRed" ForeColor="Blue">
                                    <%# DataBinder.Eval(Container, "DataItem.EmployeeName") %>
                                </telerik:RadToolTip>
                            </ItemTemplate>
                            <HeaderStyle Width="160px" />
                            <ItemStyle Width="160px" />
                        </telerik:GridTemplateColumn>
                        <%--Số nội bộ--%>
                        <telerik:GridBoundColumn FilterControlAltText="Filter Phone column"
                            EnableHeaderContextMenu="false" HeaderStyle-HorizontalAlign="Center"
                            UniqueName="Phone" HeaderText="<%$ Resources:Resource,Sonoibo %>" DataField="Phone" ReadOnly="true"
                            ShowFilterIcon="false" AllowFiltering="true"
                            FilterCheckListEnableLoadOnDemand="true" SortExpression="Phone">
                            <HeaderStyle Width="120px" />
                            <ItemStyle Width="120px" />
                        </telerik:GridBoundColumn>
                        <%-- Di động--%>
                        <telerik:GridBoundColumn FilterControlAltText="Filter Mobile column"
                            EnableHeaderContextMenu="false" HeaderStyle-HorizontalAlign="Center"
                            UniqueName="Mobile" HeaderText="<%$ Resources:Resource,SoDTDD %>" DataField="Mobile" ReadOnly="true"
                            ShowFilterIcon="false" AllowFiltering="true"
                            FilterCheckListEnableLoadOnDemand="true" SortExpression="Mobile">
                            <HeaderStyle Width="120px" />
                            <ItemStyle Width="120px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter Email column"
                            EnableHeaderContextMenu="false" HeaderStyle-HorizontalAlign="Center"
                            UniqueName="Email" HeaderText="Email" DataField="Email" ReadOnly="true"
                            ShowFilterIcon="false" AllowFiltering="true"
                            FilterCheckListEnableLoadOnDemand="true" SortExpression="Email">
                            <HeaderStyle Width="140px" />
                            <ItemStyle Width="140px" />
                        </telerik:GridBoundColumn>

                        <%--<telerik:GridTemplateColumn HeaderText="Phòng" EnableHeaderContextMenu="false"
                                                    FilterControlAltText="Filter RequestDivisionName column" ColumnGroupName="EmployeeGroup"
                                                    SortExpression="RequestDivisionName" CurrentFilterFunction="StartsWith"
                                                    ShowFilterIcon="false" AllowFiltering="true" UniqueName="RequestDivisionName">
                            <ItemTemplate>
                                <asp:Label ID="lbDepartmentName" runat="server" Text='<%# Eval("DepartmentName") %>'></asp:Label>
                                <telerik:RadToolTip RenderMode="Lightweight" ID="RadToolTip2" runat="server"
                                                    TargetControlID="lbDepartmentName" Width="200px"
                                                    RelativeTo="Element" Position="MiddleRight" BorderColor="OrangeRed" ForeColor="Blue">
                                    <%# DataBinder.Eval(Container, "DataItem.DepartmentName") %>
                                </telerik:RadToolTip>
                            </ItemTemplate>
                            <HeaderStyle Width="200px" />
                            <ItemStyle Width="200px" />
                        </telerik:GridTemplateColumn>--%>
                        <%--Ngày bắt đầu--%>
                        <telerik:GridDateTimeColumn DataField="StartDate" HeaderText="<%$ Resources:Resource,Ngaybatdau %>" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="StartDate" CurrentFilterFunction="StartsWith" ShowFilterIcon="false" AllowFiltering="false"
                            UniqueName="StartDate" ReadOnly="true" Display="true">
                            <HeaderStyle Width="120px" />
                            <ItemStyle Width="120px" />
                            <ColumnValidationSettings></ColumnValidationSettings>
                        </telerik:GridDateTimeColumn>
                        <%--Ngày kết thúc--%>
                        <telerik:GridDateTimeColumn DataField="EndDate" HeaderText="<%$ Resources:Resource,Ngayketthuc%>" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="EndDate" CurrentFilterFunction="StartsWith" ShowFilterIcon="false" AllowFiltering="false"
                            UniqueName="EndDate" ReadOnly="true" Display="true">
                            <HeaderStyle Width="120px" />
                            <ItemStyle Width="120px" />
                            <ColumnValidationSettings></ColumnValidationSettings>
                        </telerik:GridDateTimeColumn>
                        <%--Ngày hoàn thành--%>
                        <telerik:GridDateTimeColumn DataField="FinishDate" HeaderText="<%$ Resources:Resource,Ngayhoanthanh%>" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Center"
                            SortExpression="FinishDate" CurrentFilterFunction="StartsWith" ShowFilterIcon="false" AllowFiltering="false"
                            UniqueName="FinishDate" ReadOnly="true" Display="true">
                            <HeaderStyle Width="140px" />
                            <ItemStyle Width="140px" />
                            <ColumnValidationSettings></ColumnValidationSettings>
                        </telerik:GridDateTimeColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings>
                    <Scrolling AllowScroll="True" ScrollHeight="" FrozenColumnsCount="1" UseStaticHeaders="True" SaveScrollPosition="true" EnableNextPrevFrozenColumns="true"></Scrolling>
                </ClientSettings>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageCssContent" runat="server">
    <%: Styles.Render("~/assets/css/custom.css") %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PageScriptContent" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="gridLoadingPanel"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadNotification1" />
                    <telerik:AjaxUpdatedControl ControlID="detailWidget" />
                    <telerik:AjaxUpdatedControl ControlID="OwnerCancel" />
                    <telerik:AjaxUpdatedControl ControlID="AllWaiting" />
                    <telerik:AjaxUpdatedControl ControlID="Allcomplete" />
                    <telerik:AjaxUpdatedControl ControlID="AllCancel" />
                    <telerik:AjaxUpdatedControl ControlID="Handle" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="dateCompareValidator" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                    <telerik:AjaxUpdatedControl ControlID="detailWidget" />
                    <telerik:AjaxUpdatedControl ControlID="OwnerCancel" />
                    <telerik:AjaxUpdatedControl ControlID="AllWaiting" />
                    <telerik:AjaxUpdatedControl ControlID="Allcomplete" />
                    <telerik:AjaxUpdatedControl ControlID="AllCancel" />
                    <telerik:AjaxUpdatedControl ControlID="Handle" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rdpFromDate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                    <telerik:AjaxUpdatedControl ControlID="detailWidget" />
                    <telerik:AjaxUpdatedControl ControlID="OwnerCancel" />
                    <telerik:AjaxUpdatedControl ControlID="AllWaiting" />
                    <telerik:AjaxUpdatedControl ControlID="Allcomplete" />
                    <telerik:AjaxUpdatedControl ControlID="AllCancel" />

                    <telerik:AjaxUpdatedControl ControlID="Handle" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rdpToDate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                    <telerik:AjaxUpdatedControl ControlID="detailWidget" />
                    <telerik:AjaxUpdatedControl ControlID="OwnerCancel" />
                    <telerik:AjaxUpdatedControl ControlID="AllWaiting" />
                    <telerik:AjaxUpdatedControl ControlID="Allcomplete" />
                    <telerik:AjaxUpdatedControl ControlID="AllCancel" /> 
                    <telerik:AjaxUpdatedControl ControlID="Handle" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbStatus">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="detailWidget" />
                    <telerik:AjaxUpdatedControl ControlID="OwnerCancel" />
                    <telerik:AjaxUpdatedControl ControlID="AllWaiting" />
                    <telerik:AjaxUpdatedControl ControlID="Allcomplete" />
                    <telerik:AjaxUpdatedControl ControlID="AllCancel" />
                    <telerik:AjaxUpdatedControl ControlID="Handle" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbTag">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="detailWidget" />
                    <telerik:AjaxUpdatedControl ControlID="OwnerCancel" />
                    <telerik:AjaxUpdatedControl ControlID="AllWaiting" />
                    <telerik:AjaxUpdatedControl ControlID="Allcomplete" />
                    <telerik:AjaxUpdatedControl ControlID="AllCancel" />
                    <telerik:AjaxUpdatedControl ControlID="Handle" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>

    <script type="text/javascript">
        function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("DownloadCommandColumn") >= 0) {
                args.set_enableAjax(false);
            }
        }

        function ShowEditForm(id, rowIndex) {
            var grid = $find("<%= RadGrid1.ClientID %>");

            var rowControl = grid.get_masterTableView().get_dataItems()[rowIndex].get_element();
            grid.get_masterTableView().selectItem(rowControl, true);

            //var pagePath = '<%= Page.ResolveUrl("~") %>';

            window.radopen("/Issues/CancelIssue.aspx?IssueId=" + id, "UserListDialog");
            return false;
        }

        function refreshGrid(arg) {
            if (!arg) {
                $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Rebind");
            }
            else {
                $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("RebindAndNavigate");
            }
        }

        function setCustomPosition(sender, args) {
            sender.moveTo(sender.get_left(), sender.get_top());
        }

        // Checked only checkbox
        <%--function uncheckOther(chk) {
            var grid = $find("<%=RadGrid1.ClientID %>");

            if (grid) {
                var MasterTable = grid.get_masterTableView();
                var Rows = MasterTable.get_dataItems();
                for (var i = 0; i < Rows.length; i++) {
                    var row = Rows[i];
                    var Chk1 = $(row.get_element()).find("input[id*='checkColumn']").get(0);
                    if (Chk1.id != chk.id) {
                        Chk1.checked = false;
                    }
                }
            }
        }--%>
    </script>
</asp:Content>
