<%@ Page Title="Create Menu" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="CreateMenu.aspx.cs" Inherits="ServiceDesk.WebApp.Admin.CreateMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="widget">
        <div class="widget-header bordered-left bordered-darkorange">
            <span class="widget-caption">Menu Form</span>
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
                    <div class="col-md-6">
                        <telerik:RadTextBox Skin="Office2010Blue" ID="txtMenuName" Width="100%"
                            runat="server" EmptyMessage=" Input MenuName">
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
                PagerStyle-AlwaysVisible="true" AllowSorting="true" AllowFilteringByColumn="true"
                OnNeedDataSource="RadGrid1_NeedDataSource"  OnItemDataBound="RadGrid1_ItemDataBound"
                OnUpdateCommand="RadGrid1_UpdateCommand" OnDeleteCommand="RadGrid1_DeleteCommand" OnInsertCommand="RadGrid1_InsertCommand">
                <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                </HeaderContextMenu>
                <PagerStyle Mode="NextPrevAndNumeric" />
                <AlternatingItemStyle BackColor="aliceblue" />
                <MasterTableView Name="Master" TableLayout="Fixed" DataKeyNames="MenuId" EditMode="PopUp" CommandItemDisplay="Top">
                    <CommandItemSettings />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Tên Menu" Name="GroupMenuName" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                            EditImageUrl="~/Images/Grid/grd_Edit.gif" EnableHeaderContextMenu="false" ShowFilterIcon="false">
                            <HeaderStyle HorizontalAlign="Center" Width="40px" />
                            <ItemStyle Width="40px" />
                        </telerik:GridEditCommandColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter MenuId column"
                            UniqueName="MenuId" HeaderText="MenuId" DataField="MenuId" ReadOnly="true" Display="false">
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter MenuName column" ColumnGroupName="GroupMenuName"
                            UniqueName="MenuName" HeaderText="VietNamese" DataField="MenuName" ReadOnly="true"
                            ShowFilterIcon="false" AllowFiltering="false"
                            FilterCheckListEnableLoadOnDemand="true" SortExpression="MenuName">
                            <HeaderStyle Width="200px" />
                            <ItemStyle Width="200px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter RussianMenuName column" ColumnGroupName="GroupMenuName"
                            UniqueName="RussianMenuName" HeaderText="Russian" DataField="RussianMenuName" ReadOnly="true"
                            ShowFilterIcon="false" AllowFiltering="false"
                            FilterCheckListEnableLoadOnDemand="true" SortExpression="RussianMenuName">
                            <HeaderStyle Width="200px" />
                            <ItemStyle Width="200px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter Url column"
                            UniqueName="Url" HeaderText="Url" DataField="Url" ReadOnly="true"
                            ShowFilterIcon="false" AllowFiltering="false"
                            FilterCheckListEnableLoadOnDemand="true" SortExpression="Url">
                            <HeaderStyle Width="200px" />
                            <ItemStyle Width="200px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter FriendlyUrl column"
                            UniqueName="FriendlyUrl" HeaderText="Friendly Url" DataField="FriendlyUrl" ReadOnly="true"
                            ShowFilterIcon="false" AllowFiltering="false"
                            FilterCheckListEnableLoadOnDemand="true" SortExpression="FriendlyUrl">
                            <HeaderStyle Width="150px" />
                            <ItemStyle Width="150px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter ParentId column"
                            UniqueName="ParentId" HeaderText="ParentId" DataField="ParentId" ReadOnly="true"
                            ShowFilterIcon="false" AllowFiltering="false" Display="False"
                            FilterCheckListEnableLoadOnDemand="true" SortExpression="ParentId">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Width="70px" />
                        </telerik:GridBoundColumn>
                        
                        <telerik:GridBoundColumn FilterControlAltText="Filter MenuIconId column"
                            UniqueName="MenuIconId" HeaderText="MenuIconId" DataField="MenuIconId" ReadOnly="true" Display="false">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Width="70px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter IconName column"
                            UniqueName="IconName" HeaderText="Icon" DataField="IconName" ReadOnly="true"
                            ShowFilterIcon="false" AllowFiltering="false"
                            FilterCheckListEnableLoadOnDemand="true" SortExpression="IconName">
                            <HeaderStyle Width="100px" />
                            <ItemStyle Width="100px" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn FilterControlAltText="Filter Sort column"
                            UniqueName="Sort" HeaderText="Sort" DataField="Sort" ReadOnly="true"
                            ShowFilterIcon="false" AllowFiltering="false"
                            FilterCheckListEnableLoadOnDemand="true" SortExpression="Sort">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Width="70px" />
                        </telerik:GridBoundColumn>

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
                                                <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <label for="MenuName">Menu Name</label>
                                                        <telerik:RadTextBox ID="MenuName" Height="25px" Width="100%" EmptyMessageStyle-ForeColor="Red" runat="server"
                                                            Text='<%# DataBinder.Eval(Container.DataItem, "MenuName") %>'>
                                                        </telerik:RadTextBox>
                                                        <asp:RequiredFieldValidator ID="requiredMenuName" runat="server" Display="None"
                                                            ForeColor="Red" ControlToValidate="MenuName" ErrorMessage="Menu Name là bắt buộc" ValidationGroup="SummaryRequire">
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <label for="MenuRussianName">Menu Russian Name</label>
                                                        <telerik:RadTextBox ID="RussianMenuName" Height="25px" Width="100%" EmptyMessageStyle-ForeColor="Red" runat="server"
                                                            Text='<%# DataBinder.Eval(Container.DataItem, "RussianMenuName") %>'>
                                                        </telerik:RadTextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <label for="Url">Url</label>
                                                        <telerik:RadTextBox ID="Url" Height="25px" Width="100%" runat="server" EmptyMessageStyle-ForeColor="Red" EmptyMessage="Url"
                                                            Text='<%# DataBinder.Eval(Container.DataItem, "Url") %>'>
                                                        </telerik:RadTextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="None"
                                                            ForeColor="Red" ControlToValidate="Url" ErrorMessage="Url là bắt buộc" ValidationGroup="SummaryRequire">
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <label for="Url">Friendly Url</label>
                                                        <telerik:RadTextBox ID="FriendlyUrl" Height="25px" Width="100%" EmptyMessage="Friendly Url" runat="server"
                                                            Text='<%# DataBinder.Eval(Container.DataItem, "FriendlyUrl") %>'>
                                                        </telerik:RadTextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <label for="Icon">Icon</label>
                                                        <telerik:RadComboBox ID="rcbIcon" runat="server" Width="100%"
                                                            DropDownWidth="345" EmptyMessage="Chọn Icon" HighlightTemplatedItems="true"
                                                            EnableLoadOnDemand="true" Filter="Contains"
                                                            DataTextField="IconName" DataValueField="Id" EnableItemCaching="true"
                                                            OnItemsRequested="rcbIcon_ItemsRequested"
                                                            ItemsPerRequest="10" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                                                            OnSelectedIndexChanged="OnSelectedIndexChangedHandler" Skin="Office2010Blue">
                                                            <HeaderTemplate>
                                                                <table style="width: 345px">
                                                                    <tr>
                                                                        <td style="color: Black; width: 40px;">Id
                                                                        </td>
                                                                        <td style="color: Black; width: 300px;">Icon
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table style="width: 345px">
                                                                    <tr>
                                                                        <td style="color: Black; width: 40px;">
                                                                            <%# DataBinder.Eval(Container.DataItem, "Id") %>
                                                                        </td>
                                                                        <td style="color: Black; width: 300px;">
                                                                            <%# DataBinder.Eval(Container.DataItem, "IconName") %>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </telerik:RadComboBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <label for="Sort">Parent</label>
                                                        <telerik:RadNumericTextBox ID="Sort" runat="server" MinValue="0" Width="100%" 
                                                             DbValue='<%# Bind("Sort") %>' Culture="en-US">
                                                            <NumberFormat GroupSeparator="" DecimalDigits="0" /> 
                                                        </telerik:RadNumericTextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <label for="Tree">Parent</label>
                                                        <telerik:RadDropDownTree ID="Tree" runat="server" Width="100%" Skin="Office2010Blue" DefaultValue="NULL"
                                                            DefaultMessage="Select Parent Menu" ExpandNodeOnSingleClick="true" CheckBoxes="SingleCheck"
                                                            FilterSettings-EmptyMessage="Select Parent Menu"
                                                            DataFieldID="MenuId" DataFieldParentID="ParentId" DataTextField="MenuName"
                                                            DataValueField="MenuId" 
                                                            SelectedValue='<%# DataBinder.Eval(Container.DataItem, "ParentId") %>'>
                                                            <DropDownSettings OpenDropDownOnLoad="false" Height="300px" />
                                                            <FilterSettings EmptyMessage="true" />
                                                        </telerik:RadDropDownTree>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <label for="Active">Active</label>
                                                        <div class="checkbox">
                                                            <label>
                                                                <asp:CheckBox id="Active" runat="server" Checked='<%# Eval("Active") %>'>
                                                                </asp:CheckBox>
                                                            </label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <telerik:RadButton ID="btnUpdate" Text='<%# Container is GridEditFormInsertItem ? "Insert" : "Update" %>'
                                                            ValidationGroup="SummaryRequire" runat="server" CommandName='<%# Container is GridEditFormInsertItem ? "PerformInsert" : "Update" %>'>
                                                            <Icon PrimaryIconCssClass="rbOk"></Icon>
                                                        </telerik:RadButton>
                                                        &nbsp;
                                                        <telerik:RadButton ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CommandName="Cancel">
                                                            <Icon PrimaryIconCssClass="rbCancel"></Icon>
                                                        </telerik:RadButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </FormTemplate>
                        <PopUpSettings Width="100%" Height="" Modal="true" OverflowPosition="Center" />
                    </EditFormSettings>
                </MasterTableView>
                <ClientSettings>
                    <%--<ClientEvents OnPopUpShowing="PopUpShowing" />--%>
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="4" EnableNextPrevFrozenColumns="true"></Scrolling>
                </ClientSettings>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </telerik:RadGrid>
        
        <telerik:RadNotification RenderMode="Lightweight" ID="RadNotification1" runat="server" VisibleOnPageLoad="False" Position="BottomRight"
                Title="Notification" OffsetX="0" OffsetY="0"  AnimationDuration="500" Opacity ="100" AutoCloseDelay="3000"
                ContentScrolling="Default" Pinned="True" EnableRoundedCorners ="True" KeepOnMouseOver ="True" VisibleTitlebar ="True"
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
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            var popUp;
            function PopUpShowing(sender, eventArgs) {
                popUp = eventArgs.get_popUp();
                //var gridWidth = sender.get_element().offsetWidth;
                //var gridHeight = sender.get_element().offsetHeight;
                var popUpWidth = popUp.style.width.substr(0, popUp.style.width.indexOf("px"));
                var popUpHeight = popUp.style.height.substr(0, popUp.style.height.indexOf("px"));
                //popUp.style.left = ((window - popUpWidth) / 2 + sender.get_element().offsetLeft).toString() + "px";
                //popUp.style.top = ((window - popUpHeight) / 2 + sender.get_element().offsetTop).toString() + "px";
                popUp.style.left = "0 !importal";
                popUp.style.top = "0";
            }
            //function PopUpShowing(sender, args) {
            //    var popUp = args.get_popUp();
            //    var screenSize = $telerik.getViewPortSize();
            //    setTimeout(function () {
            //        popUp.style.left = ((screenSize.width - popUp.offsetWidth) / 2) + "px";
            //        popUp.style.top = ((screenSize.height - popUp.offsetHeight) / 2) + "px";
            //    }, 1)
            //}
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
