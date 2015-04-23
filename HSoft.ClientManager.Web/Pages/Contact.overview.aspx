<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Contact.overview.aspx.cs" Inherits="Pages_Contact_overview" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Grid" Assembly="obout_Grid_NET" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Interface" Assembly="obout_Interface" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <asp:PlaceHolder runat="server" ID="contactsForm">

<asp:Table ID="tblCustomer" runat="server" Width="100%" GridLines="Both" BorderWidth="1" BorderStyle="Solid" > 
    <asp:TableHeaderRow>
        <asp:TableCell Width="1px">Id</asp:TableCell>
        <asp:TableCell Width="1px">eMail</asp:TableCell>
        <asp:TableCell Width="1px">Phone</asp:TableCell>
        <asp:TableCell Width="1px">Name</asp:TableCell>
        <asp:TableCell Width="1px">Entry Date</asp:TableCell>
        <asp:TableCell Width="120px">Call Later</asp:TableCell>
        <asp:TableCell Width="120px">Send Mails</asp:TableCell>
        <asp:TableCell Width="250px">Comment</asp:TableCell>
        <asp:TableCell Width="250px">Notes</asp:TableCell>
    </asp:TableHeaderRow>
</asp:Table> 


<obout:Grid id="gridCustomer" runat="server" AutoGenerateColumns="false" AllowAddingRecords="false" AllowColumnResizing="true" AllowGrouping="false">
  <Columns>
    <obout:Column DataField="Id" HeaderText="Number" Width="80" runat="server" AllowFilter="false" AllowEdit="false" AllowSorting="false" />
    <obout:Column DataField="eMail" HeaderText="EMail" Width="120" runat="server" ShowFilterCriterias="true" />
    <obout:Column DataField="Name" HeaderText="Name" Width="120" runat="server" />
    <obout:Column DataField="Entry Date" HeaderText="Entry Date" Width="100" runat="server" />
    <obout:Column DataField="Call Later" HeaderText="Call Later" Width="100" runat="server" />
    <obout:Column DataField="AssignedTo" HeaderText="Assigned To" Width="100" runat="server" />
    <obout:Column DataField="SendMails" HeaderText="Senr Mails" Width="80" runat="server" AllowFilter="false" AllowSorting="false" />
  </Columns>
</obout:Grid>


    </asp:PlaceHolder>

</asp:Content>