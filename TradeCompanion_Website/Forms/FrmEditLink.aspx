<%@ Page Language="VB" MasterPageFile="~/Forms/MstInternal.master" AutoEventWireup="false" CodeFile="FrmEditLink.aspx.vb" Inherits="FrmEditLink" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server" Height="50px" Width="125px">
        <table id="TblAddLink" runat="server" style="width: 508px" visible="true">
            <tr>
                <td style="width: 127px">
                    &nbsp;
                </td>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <strong></strong>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:LinkButton ID="LBttnDelete" runat="server" CssClass="LinkButton" Font-Bold="True">Delete</asp:LinkButton>
                    &nbsp;&nbsp;
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="|"></asp:Label>
                    <asp:LinkButton ID="LBttnDeActive" runat="server" CssClass="LinkButton" Font-Bold="True">De-Active</asp:LinkButton>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" class="TDFONT" style="width: 127px">
                    &nbsp;Site Name &nbsp;
                </td>
                <td style="width: 100px">
                    <asp:TextBox ID="TxtsiteName" runat="server" CssClass="TEXTBOX" Width="213px"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right" class="TDFONT" style="width: 127px">
                    &nbsp;URL &nbsp;</td>
                <td style="width: 100px" align="left">
                    <asp:TextBox ID="TxtURL" runat="server" CssClass="TEXTBOX" ForeColor="Blue" Width="213px">http://</asp:TextBox></td>
            </tr>
            <tr>
                <td align="right" class="TDFONT" style="width: 127px">
                    &nbsp; Link&nbsp; Section&nbsp;</td>
                <td style="width: 100px">
                    <asp:DropDownList ID="DDLLinkSection" runat="server" Width="167px">
                        <asp:ListItem Value="0">  -   Please Select   -</asp:ListItem>
                        <asp:ListItem Value="1">Indices Forex</asp:ListItem>
                        <asp:ListItem Value="2">Intraday Charts</asp:ListItem>
                        <asp:ListItem Value="3">Stock Market</asp:ListItem>
                        <asp:ListItem Value="4">Other</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td align="right" class="TDFONT" style="width: 127px">
                    &nbsp; &nbsp;
                </td>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td style="width: 127px">
                </td>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td style="width: 127px">
                </td>
                <td style="width: 100px">
                    <asp:Button ID="BttnSend" runat="server" CssClass="BUTTON" Text="Send" Width="69px" /></td>
            </tr>
            <tr>
                <td style="width: 127px">
                </td>
                <td style="width: 100px">
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>

