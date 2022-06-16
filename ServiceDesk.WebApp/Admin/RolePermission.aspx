<%@ Page Title="Role Permission" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="RolePermission.aspx.cs" Inherits="ServiceDesk.WebApp.Admin.RolePermission" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="widget">
        <div class="widget-header bordered-left bordered-darkorange">
            <span class="widget-caption">Role Permission</span>
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
                        <telerik:RadComboBox ID="rcbRole" runat="server" Width="100%"
                            HighlightTemplatedItems="true" AutoPostBack="true" 
                            EnableLoadOnDemand="true" Filter="Contains" OnSelectedIndexChanged="rcbRole_SelectedIndexChanged"
                            DataTextField="RoleName" DataValueField="RoleId" EnableItemCaching="true"
                            ItemsPerRequest="10" ShowMoreResultsBox="true" EnableVirtualScrolling="true" Skin="Office2010Blue">
                            <HeaderTemplate>
                                <table>
                                    <tr>
                                        <td style="color: Black; width: 70px;">RoleId
                                        </td>
                                        <td style="color: Black;">RoleName
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td style="color: Black; width: 70px;">
                                            <%# DataBinder.Eval(Container.DataItem, "RoleId") %>
                                        </td>
                                        <td style="color: Black;">
                                            <%# DataBinder.Eval(Container.DataItem, "RoleName") %>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:RadComboBox>
                    </div>
                </div>
            </div>

            <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="False"
                CellSpacing="0" GridLines="None" Skin="Office2010Blue" Style="clear: both; margin-top: 10px;" RenderMode="Auto"
                AllowPaging="true" EnableHeaderContextMenu="true" FilterMenu-EnableEmbeddedBaseStylesheet="true"
                FilterType="HeaderContext" GroupingEnabled="false" EnableHeaderContextFilterMenu="true"
                PagerStyle-AlwaysVisible="true" AllowSorting="true" AllowFilteringByColumn="true" OnNeedDataSource="RadGrid1_NeedDataSource"
                             OnItemCommand="RadGrid1_ItemCommand">
                <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                </HeaderContextMenu>
                <PagerStyle Mode="NextPrevAndNumeric" />
                <AlternatingItemStyle BackColor="aliceblue" />
                <MasterTableView Name="Master" TableLayout="Fixed" DataKeyNames="MenuId" EditMode="PopUp" CommandItemDisplay="Top">
                <CommandItemTemplate>
                    <div style="padding: 5px 5px;">
                        <asp:LinkButton ID="btnUpdateCommand" runat="server" CommandName="UpdateCommandTemplate">
                            <img style="border: 0px; vertical-align: middle;" alt="" src="../Images/Grid/grd_Edit.gif"/>
                            <strong>Update</strong>
                        </asp:LinkButton>
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridBoundColumn FilterControlAltText="Filter MenuId column"
                                             UniqueName="MenuId" HeaderText="MenuId" DataField="MenuId" ReadOnly="true" Display="false">
                    </telerik:GridBoundColumn>

                    <telerik:GridBoundColumn FilterControlAltText="Filter MenuName column" UniqueName="MenuName" HeaderText="MenuName" DataField="MenuName">
                        <ItemStyle Width="200px"/>
                        <HeaderStyle Width="200px"/>
                    </telerik:GridBoundColumn>

                    <telerik:GridTemplateColumn FilterControlAltText="Filter Add column"
                                   EnableHeaderContextMenu="false" ShowFilterIcon="false" UniqueName="Add">
                        <HeaderTemplate>
                            <div class="checkbox">
                                <label>
                                    <asp:CheckBox id="chkAllAdd" runat="server" AutoPostBack="true" OnCheckedChanged="chkAllAdd_CheckedChanged"></asp:CheckBox>
                                    <span class="text">Add</span>
                                </label>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="checkbox">
                                <label>
                                    <asp:CheckBox id="chkAdd" runat="server" Checked='<%# CheckedAdd(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "RolePermission"))) %>'></asp:CheckBox>
                                    <span class="text"></span>
                                </label>
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="90px" HorizontalAlign="Center"/>
                        <HeaderStyle Width="90px" HorizontalAlign="Center"/>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn FilterControlAltText="Filter Edit column" ShowFilterIcon="false" 
                        EnableHeaderContextMenu="false" UniqueName="Edit">
                        <HeaderTemplate>
                            <div class="checkbox">
                                <label>
                                    <asp:CheckBox id="chkAllEdit" runat="server" AutoPostBack="true" OnCheckedChanged="chkAllEdit_CheckedChanged"></asp:CheckBox>
                                    <span class="text">Edit</span>
                                </label>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="checkbox">
                                <label>
                                    <asp:CheckBox id="chkEdit" runat="server" Checked='<%# CheckedEdit(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "RolePermission"))) %>'></asp:CheckBox>
                                    <span class="text"></span>
                                </label>
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="90px" HorizontalAlign="Center"/>
                        <HeaderStyle Width="90px" HorizontalAlign="Center"/>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn FilterControlAltText="Filter Delete column"
                                                EnableHeaderContextMenu="false" ShowFilterIcon="false" UniqueName="Delete">
                        <HeaderTemplate>
                            <div class="checkbox">
                                <label>
                                    <asp:CheckBox id="chkAllDelete" runat="server" AutoPostBack="true" OnCheckedChanged="chkAllDelete_CheckedChanged"></asp:CheckBox>
                                    <span class="text">Delete</span>
                                </label>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="checkbox">
                                <label>
                                    <asp:CheckBox id="chkDelete" runat="server" Checked='<%# CheckedDelete(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "RolePermission"))) %>'></asp:CheckBox>
                                    <span class="text"></span>
                                </label>
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="90px" HorizontalAlign="Center"/>
                        <HeaderStyle Width="90px" HorizontalAlign="Center"/>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn FilterControlAltText="Filter View column"
                                        EnableHeaderContextMenu="false" ShowFilterIcon="false" UniqueName="View">
                        <HeaderTemplate>
                            <div class="checkbox">
                                <label>
                                    <asp:CheckBox id="chkAllView" runat="server" AutoPostBack="true" OnCheckedChanged="chkAllView_CheckedChanged"></asp:CheckBox>
                                    <span class="text">View</span>
                                </label>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="checkbox">
                                <label>
                                    <asp:CheckBox id="chkView" runat="server" Checked='<%# CheckedView(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "RolePermission"))) %>'></asp:CheckBox>
                                    <span class="text"></span>
                                </label>
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="90px" HorizontalAlign="Center"/>
                        <HeaderStyle Width="90px" HorizontalAlign="Center"/>
                    </telerik:GridTemplateColumn>
                </Columns>
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

        .rgMasterTable tbody input[type=checkbox] ~ .text, tbody input[type=radio] ~ .text {
            display: none;
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
            <telerik:AjaxSetting AjaxControlID="rcbRole">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
</asp:Content>
