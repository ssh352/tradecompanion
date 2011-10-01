<%@ Page Language="VB" MasterPageFile="~/Forms/MstInternal.master" AutoEventWireup="false" CodeFile="FrmForgetPswd.aspx.vb" Inherits="FrmForgetPswd" title="Scalper - Forgotten Password?" %>
<asp:Content ID="InternalForwardPswd" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
        <asp:Panel ID="Panel1" runat="server" DefaultButton="BttnSend" Height="500px" Width="429px">
    <table style="width: 461px" id="TABLE2">
        <tr>
            <td colspan="2" align="center">
                <asp:Label ID="LblSorry" runat="server" ForeColor="Red" Visible="False" Width="396px" Font-Bold="True"></asp:Label>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Label ID="LblSend" runat="server" ForeColor="Blue" Visible="False" Width="433px" Font-Bold="True"></asp:Label></td>
        </tr>
        <tr>
            <td colspan="2">
            </td>
        </tr>
        <tr>
            <td colspan="2" align=center>
                <strong>FORGOTTEN YOUR PASSWORD ?</strong></td>
        </tr>
        <tr>
            <td align="center" class="TDFONT" colspan="2">&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2" class="TDFONT" align=center>
                Simply enter the EMail Address you signed up with into the box </td>
        </tr>
        <tr>
            <td align="center" class="TDFONT" colspan="2">
                below and Click Send</td>
        </tr>
        <tr>
            <td align="center" class="TDFONT" colspan="2">&nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 175px" class="TDFONT">
                EMail Address</td>
            <td align="center" style="width: 157px">
                <asp:TextBox ID="TxtEMailID" runat="server" Width="215px" CssClass="TEXTBOX"></asp:TextBox>&nbsp;
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2" align=center>
                &nbsp; &nbsp;
                <asp:Button ID="BttnSend" runat="server" Text="Send" CssClass="BUTTON" Height="25px" Width="75px" /></td>
        </tr>
        <tr>
            <td style="width: 175px">
            </td>
            <td style="width: 157px">
                </td>
        </tr>
    </table>
        </asp:Panel>
        
</asp:Content>

