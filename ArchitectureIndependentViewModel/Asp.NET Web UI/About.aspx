<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="Asp.NET_Web_UI.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Dynamic Dialog</h3>
    <p>&nbsp;</p>
    <asp:Panel ID="Row1" runat="server" Direction="LeftToRight">
        <asp:Label ID="Label1" runat="server" Text="Label 1" Width="100px"></asp:Label>
        <asp:TextBox ID="TextBox1" runat="server">my textbox 1</asp:TextBox>
    </asp:Panel>
    <asp:Panel ID="Row2" runat="server" Direction="LeftToRight">
        <asp:Label ID="Label2" runat="server" Text="Label 2" Width="100px"></asp:Label>
        <asp:TextBox ID="TextBox2" runat="server">my textbox 2</asp:TextBox>
    </asp:Panel>
    <asp:Panel ID="Row3" runat="server" Direction="LeftToRight">
    </asp:Panel>
    <asp:Panel ID="Row4" runat="server" Direction="LeftToRight">
        <asp:Button ID="Button1" runat="server" Text="Save" OnClick="Button1_Click" Width="70px" />
        <asp:Button ID="Button2" runat="server" Text="Cancel" Width="70px" OnClick="Button2_Click" />
    </asp:Panel>
</asp:Content>
