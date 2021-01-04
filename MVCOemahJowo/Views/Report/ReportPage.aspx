<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportPage.aspx.cs" Inherits="MVCOemahJowo.Views.Report.ReportPage" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="telerik" Assembly="Telerik.ReportViewer.WebForms" Namespace="Telerik.ReportViewer.WebForms" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
     <telerik:ReportViewer ID="ReportViewer1"
                Width="100%"
                Height="650px" runat="server">
            
            </telerik:ReportViewer>
    <a href="ReportPage.aspx">ReportPage.aspx</a>
</asp:Content>
