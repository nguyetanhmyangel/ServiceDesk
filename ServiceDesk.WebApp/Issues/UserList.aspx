<%@ Page Title="Danh sách nhân viên" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="ServiceDesk.WebApp.Issues.UserList" %>
<%@ Import Namespace="ServiceDesk.Utilities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="widget">
        <div class="widget-header bordered-left bordered-darkorange">
            <span class="widget-caption">Danh sách nhân viên</span>
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
                    <div class="col-md-4">
                        <telerik:RadComboBox ID="rcbDepartment" runat="server" Width="100%"
                            HighlightTemplatedItems="true" AutoPostBack="true" AllowCustomText="true" EmptyMessage="Select one department"
                            EnableLoadOnDemand="true" Filter="Contains"
                            OnSelectedIndexChanged="rcbDepartment_SelectedIndexChanged"
                            DataTextField="DepartmentName" DataValueField="DepartmentId" Skin="Office2010Blue">
                        </telerik:RadComboBox>
                    </div>
                    
                    <div class="col-md-1">
                        <telerik:RadButton ID="btnSearch" Skin="Office2010Blue" runat="server" Text="Search" OnClick="btnSearch_Click" />
                    </div>
                </div>
            </div>

            <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="False"
                CellSpacing="0" GridLines="None" Skin="Office2010Blue" Style="clear: both; margin-top: 10px;" RenderMode="Auto"
                AllowPaging="true" AllowSorting="true" OnNeedDataSource="RadGrid1_NeedDataSource" OnDetailTableDataBind="RadGrid1_DetailTableDataBind">
                <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                </HeaderContextMenu>
                <PagerStyle Mode="NextPrevAndNumeric" PageSizeLabelText="Cỡ trang" PagerTextFormat="{4} trang {0}/{1}, dòng {2} đến {3}/{5}"/>
                <MasterTableView Name="Master" TableLayout="Fixed" InsertItemPageIndexAction ="ShowItemOnCurrentPage" DataKeyNames="UserId" EditMode="PopUp" CommandItemDisplay="Top" 
                                 NoMasterRecordsText="Không có dòng dữ liệu nào" NoDetailRecordsText="Không có dòng dữ liệu nào">
                    <CommandItemSettings ShowAddNewRecordButton="false" RefreshText="Làm mới"/>
                    <DetailTables>
                        <telerik:GridTableView DataKeyNames="UserId" Name="Detail" Width="100%">
                            <Columns>
                                <telerik:GridBoundColumn FilterControlAltText="Filter UserId column"
                                    UniqueName="UserId" HeaderText="EmployeeId" DataField="UserId" ReadOnly="true" Display="false">
                                    <HeaderStyle Width="70px" />
                                    <ItemStyle Width="70px" />
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn FilterControlAltText="Filter TaskId column"
                                                         UniqueName="TaskId" HeaderText="TaskId" DataField="TaskId" ReadOnly="true" Display="false">
                                    <HeaderStyle Width="70px" />
                                    <ItemStyle Width="70px" />
                                </telerik:GridBoundColumn>

                                <telerik:GridTemplateColumn HeaderText="Tiến độ" EnableHeaderContextMenu="false"
                                    UniqueName="Progress" FilterControlAltText="Filter Progress column"
                                    SortExpression="Progress" CurrentFilterFunction="StartsWith"
                                    ShowFilterIcon="false" AllowFiltering="true" ReadOnly="true">
                                    <ItemTemplate>
                                        <telerik:RadProgressBar runat="server" Width="140" ID="ItemProgressBar" 
                                              BarType="Value" Value='<%# Helper.ConvertToInt(Eval("Progress")) %>' MinValue="0" MaxValue="100">
                                        </telerik:RadProgressBar>
                                    </ItemTemplate>
                                    <HeaderStyle Width="170px" />
                                    <ItemStyle Width="170px" />
                                </telerik:GridTemplateColumn>
                                
                                <telerik:GridDateTimeColumn DataField="StartDate" HeaderText="Ngày bắt đầu" DataFormatString="{0:dd/MM/yyyy}"
                                                SortExpression="StartDate" CurrentFilterFunction="StartsWith" ShowFilterIcon="false" AllowFiltering="false"
                                                UniqueName="StartDate" ReadOnly="true" Display="true">
                                    <HeaderStyle Width="120px"/>
                                    <ItemStyle Width="120px"/>
                                    <ColumnValidationSettings></ColumnValidationSettings>
                                </telerik:GridDateTimeColumn>

                                <telerik:GridDateTimeColumn DataField="EndDate" HeaderText="Ngày kết thúc" DataFormatString="{0:dd/MM/yyyy}"
                                        SortExpression="EndDate" CurrentFilterFunction="StartsWith" ShowFilterIcon="false" AllowFiltering="false"
                                        UniqueName="EndDate" ReadOnly="true" Display="true">
                                    <HeaderStyle Width="120px"/>
                                    <ItemStyle Width="120px"/>
                                    <ColumnValidationSettings></ColumnValidationSettings>
                                </telerik:GridDateTimeColumn>

                                <telerik:GridTemplateColumn HeaderText="Ghi chú"
                                    UniqueName="DetailDescription" EnableHeaderContextMenu="false"
                                    FilterControlAltText="Filter DetailDescription column"
                                    SortExpression="DetailDescription" CurrentFilterFunction="StartsWith"
                                    ShowFilterIcon="false" AllowFiltering="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lbDetailDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                        <telerik:RadToolTip RenderMode="Lightweight" ID="RadToolTip4" runat="server"
                                            TargetControlID="lbDetailDescription" Width="300px"
                                            RelativeTo="Element" Position="MiddleRight" BorderColor="OrangeRed" ForeColor="Blue">
                                            <%# DataBinder.Eval(Container, "DataItem.Description") %>
                                        </telerik:RadToolTip>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </telerik:GridTableView>
                    </DetailTables>
                    <Columns>
                        <telerik:GridBoundColumn FilterControlAltText="Filter UserId column"
                                                 UniqueName="UserId" HeaderText="EmployeeId" DataField="UserId" ReadOnly="true" Display="false">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Width="70px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter numExecute column"
                                                 UniqueName="numExecute" HeaderText="Số việc đang làm" DataField="NumExecute" ReadOnly="true">
                            <HeaderStyle Width="130px" HorizontalAlign="Center"/>
                            <ItemStyle Width="130px"  HorizontalAlign="Center"/>
                        </telerik:GridBoundColumn>
                        
                        <telerik:GridBoundColumn FilterControlAltText="Filter EmployeeId column"
                                                 UniqueName="EmployeeId" HeaderText="EmployeeId" DataField="EmployeeId" ReadOnly="true">
                            <HeaderStyle Width="120px" />
                            <ItemStyle Width="120px" />
                        </telerik:GridBoundColumn>
                                
                        <telerik:GridBoundColumn FilterControlAltText="Filter EmployeeName column"
                                                 UniqueName="EmployeeName" HeaderText="Họ tên" DataField="EmployeeName" ReadOnly="true">
                            <HeaderStyle Width="300px" />
                        </telerik:GridBoundColumn>
                                
                        <telerik:GridBoundColumn FilterControlAltText="Filter DepartmentName column"
                                                 UniqueName="DepartmentName" HeaderText="Phòng ban" DataField="DepartmentName" ReadOnly="true">
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings>
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
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadNotification1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbDepartment">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
</asp:Content>


