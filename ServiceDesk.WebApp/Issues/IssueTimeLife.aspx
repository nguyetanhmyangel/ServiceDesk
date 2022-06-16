<%@ Page Title="Time Life" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="IssueTimeLife.aspx.cs" Inherits="ServiceDesk.WebApp.Issues.IssueTimeLife" %>

<%@ Register Src="~/Issues/TimeLife.ascx" TagPrefix="uc1" TagName="TimeLife" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server"  LoadingPanelID="RadAjaxLoadingPanel1">
        <ul class="timeline">
            <uc1:TimeLife runat="server" id="TimeLife" />
            <%--<li class="timeline-node">
                <a class="btn btn-info">Kết Thúc</a>
            </li>--%>
        </ul>
    </telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" ></telerik:RadAjaxLoadingPanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageCssContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PageScriptContent" runat="server">
</asp:Content>
