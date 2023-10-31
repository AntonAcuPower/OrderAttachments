<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="RW101010.aspx.cs" Inherits="Page_RW101010" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" PrimaryView="Setup" TypeName="ReportViewer.FormViewerSetupMaint">
		<CallbackCommands>
			<px:PXDSCallbackCommand Name="Insert" PostData="Self" />
			<px:PXDSCallbackCommand CommitChanges="True" Name="Save" />
			<px:PXDSCallbackCommand Name="First" PostData="Self" StartNewGroup="true" />
			<px:PXDSCallbackCommand Name="Last" PostData="Self" />
		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" 
		Width="100%" Height="100px" DataMember="Setup">
        <Template>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="M" ControlSize="XM" />
            <px:PXSelector CommitChanges="True" ID="edUserName" runat="server" DataField="UserName"/>
            <px:PXTextEdit CommitChanges="True" ID="edPassword" runat="server" DataField="Password" TextMode="Password"/>
            <px:PXTextEdit CommitChanges="True" ID="edConfirmPassword" runat="server" DataField="ConfirmPassword" TextMode="Password"/>
            
            </Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
	<px:PXGrid ID="grid" runat="server" DataSourceID="ds" Height="150px" Width="100%" ActionsPosition="top" AllowSearch="true" SkinID="Details" BorderWidth="0">
		<Levels>
			<px:PXGridLevel DataMember="Details">
                           
                <Columns>
                <px:PXGridColumn DataField="CustomerID" Width="180" CommitChanges="true"></px:PXGridColumn>
               <px:PXGridColumn DataField="IPAddress" Width="180" CommitChanges="true" ></px:PXGridColumn>
                </Columns>

                 <%-- <RowTemplate>
                     <px:PXSelector ID="edWareHouseID" runat="server" DataField="WarehouseID"  />
                     <px:PXSelector ID="edLocationID" runat="server" DataField="LocationID" />
                 </RowTemplate>--%>

			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
		<%--<ActionBar ActionsText="True">
		</ActionBar>--%>
	</px:PXGrid>
</asp:Content>
