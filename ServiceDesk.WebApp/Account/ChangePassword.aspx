<%@ Page Title="Đổi Password" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="ServiceDesk.WebApp.Account.ChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

            <div class="widget-body">
                <div class="form-horizontal" role="form">
                    <div class="form-group">
                        <label for="UserName" class="col-sm-2 control-label no-padding-right">Mã nhân viên</label>
                        <div class="col-sm-10">
                            <telerik:RadTextBox ID="txtUserName" Width="90%" height="34px" class="form-control" EnabledStyle-HorizontalAlign="Left"
                                                runat="server" ReadOnly="True">
                            </telerik:RadTextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                                        ControlToValidate="txtUserName" ErrorMessage="required" ForeColor="Red">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="OldPassword" class="col-sm-2 control-label no-padding-right">Password cũ</label>
                        <div class="col-sm-10">
                            <telerik:RadTextBox ID="txtOldPassword" Width="90%" height="34px" EnabledStyle-HorizontalAlign="Left"
                                                runat="server" TextMode="Password">
                            </telerik:RadTextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                        ControlToValidate="txtOldPassword" ErrorMessage="required" ForeColor="Red">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="NewPassword" class="col-sm-2 control-label no-padding-right">Password mới</label>
                        <div class="col-sm-10">
                            <telerik:RadTextBox ID="txtNewPassword" MaxLength="15" Width="90%" height="34px" EnabledStyle-HorizontalAlign="Left" runat="server" TextMode="Password">
                            </telerik:RadTextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                        ControlToValidate="txtNewPassword" ErrorMessage="required" ForeColor="Red">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="ConfirmPassWord" class="col-sm-2 control-label no-padding-right">xác nhận Password mới</label>
                        <div class="col-sm-10">
                            <telerik:RadTextBox ID="txtConfirmPassWord" MaxLength="15" Width="90%" height="34px" EnabledStyle-HorizontalAlign="Left" runat="server" TextMode="Password">
                            </telerik:RadTextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                        ControlToValidate="txtConfirmPassWord" ErrorMessage="required" ForeColor="Red">
                            </asp:RequiredFieldValidator>

                            <asp:CompareValidator ID="CompareValidator1" runat="server"
                                                  ControlToCompare="txtConfirmPassWord" ControlToValidate="txtNewPassword"
                                                  ErrorMessage="Does not match Password" ForeColor="Red">
                            </asp:CompareValidator>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-offset-2 col-sm-10">
                            <telerik:RadButton ID="btnSubmit" runat="server" Text="Cập nhật"
                                               Skin="Office2007" onclick="btnSubmit_Click">
                            </telerik:RadButton>
                        </div>
                    </div>
                </div>
            </div>

            <telerik:RadNotification RenderMode="Lightweight" ID="RadNotification1" runat="server" VisibleOnPageLoad="False" Position="BottomRight"
                Title="Notification" OffsetX="0" OffsetY="0" AnimationDuration="500" Opacity="100" AutoCloseDelay="3000"
                ContentScrolling="Default" Pinned="True" EnableRoundedCorners="True" KeepOnMouseOver="True" VisibleTitlebar="True"
                ShowCloseButton="True" ShowSound="None" Width="330" Height="160" Animation="Fade" EnableShadow="True" Style="z-index: 100000">
            </telerik:RadNotification>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageCssContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PageScriptContent" runat="server">
</asp:Content>
