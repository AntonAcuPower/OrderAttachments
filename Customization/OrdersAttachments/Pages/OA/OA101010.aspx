<%@ Page Language="C#" MasterPageFile="~/MasterPages/ListView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="OA101010.aspx.cs" Inherits="Page_OA101010" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/ListView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="OrderAttachments.OrderAttachmentsMaint"
        PrimaryView="UploadedFiles">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phL" runat="Server">
    <px:PXGrid ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Primary" AllowAutoHide="false">
        <Levels>
            <px:PXGridLevel DataMember="UploadedFiles">
                <Columns>
	<px:PXGridColumn DataField="OrderType" Width="70" />
                    <px:PXGridColumn LinkCommand="GetFile" CommitChanges="True" DataField="FileName" Width="200"></px:PXGridColumn>
	                <px:PXGridColumn DataField="OrderNbr" Width="70" CommitChanges="True" ></px:PXGridColumn></Columns>
                <Mode AllowAddNew="false" AllowDelete="false" AllowUpdate="false" ></Mode>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Container="Window" Enabled="True" MinHeight="150" ></AutoSize>
        <ActionBar>
        
	<Actions>
		<AddNew Enabled="False" /></Actions>
	<Actions>
		<Delete Enabled="False" /></Actions></ActionBar>
    </px:PXGrid>
</asp:Content>
