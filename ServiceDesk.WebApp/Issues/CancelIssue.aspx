<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CancelIssue.aspx.cs" Inherits="ServiceDesk.WebApp.Issues.CancelIssue" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
        <%: Styles.Render("~/css/bootstrap") %>
    </asp:PlaceHolder>
    <title></title>
</head>
<body>
    
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"/>
        <div class="container" style="padding-top: 10px">
            <div class="row">
                <div class="col-md-12">
                    <asp:Label runat="server" ID="FirstNameLabel" Text="Nhập lí do hủy yêu cầu"></asp:Label>
                    <telerik:RadTextBox runat="server" ID="txtReason" Rows="5"
                                        TextMode="MultiLine" Width="100%" />
                    <asp:RequiredFieldValidator ForeColor="Red" runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtReason"
                                                ErrorMessage="Lí do hủy yêu cầu không để trống!"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <telerik:RadButton runat="server" ID="btnUpdate" Primary="true" Text="Update" OnClick="btnUpdate_Click">
                                <Icon PrimaryIconCssClass="rbOk"></Icon>
                    </telerik:RadButton>
                    <telerik:RadButton runat="server" ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click"
                                       CausesValidation="false" >
                        <Icon PrimaryIconCssClass="rbCancel"></Icon>
                    </telerik:RadButton>
                </div>
            </div>
        </div>
        

        <script type="text/javascript">
            function CloseAndRebind(args) {
                GetRadWindow().BrowserWindow.refreshGrid(args);
                GetRadWindow().close();
            }
 
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; //IE (and Moz as well)
 
                return oWindow;
            }
 
            function CancelEdit() {
                GetRadWindow().close();
            }
        </script>
    </form>
</body>
</html>
