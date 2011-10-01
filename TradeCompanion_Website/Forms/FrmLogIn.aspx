<%@ Page Language="VB"  MasterPageFile="~/Forms/MstForContents.master" AutoEventWireup="false" CodeFile="FrmLogIn.aspx.vb" Inherits="FrmLogIn" title="Scalper - LogIn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div align=center>

    <asp:Panel ID="Panel_LogIn" runat="server" Height="500px"
        Width="525px" DefaultButton="BttnLogIN">
        <br />
        <br />
        <br />
        <br />
        
    <table style="width: 461px; font-family: Arial;" id="TABLE1" align=center bgcolor="#64829a">
        <tr>
            <td colspan="2" style="height: 21px; text-align: center">
                <strong><span style="color: white">LOGIN FORM</span></strong></td>
        </tr>
    
        <tr>
            <td style="width: 124px; height: 55px;">
            </td>
            <td style="width: 157px; height: 55px;">&nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 124px; color: #000000; height: 18px;" align="right">
                <strong><span style="font-size: 9pt; color: white">
                EMail Address / User Name :</span></strong></td>
            <td style="width: 157px; height: 18px;" align="center">
                <asp:TextBox ID="TxtEMailID" runat="server" CssClass="TEXTBOX"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="width: 124px; height: 21px;" align="right">
                <strong><span style="color: #ffffff">&nbsp;<span style="font-size: 9pt"> Password :</span></span></strong></td>
            <td style="width: 157px; height: 21px;" align="center">
                <asp:TextBox ID="TxtPswd" runat="server" TextMode="Password" CssClass=TEXTBOX></asp:TextBox></td>
        </tr>
        <tr>
            <td style="width: 124px; height: 20px;">
            </td>
            <td align="center" style="width: 157px; height: 20px;">&nbsp;
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Button CssClass=BUTTON ID="BttnLogIN" runat="server" Text="Log In" BackColor="DarkGray" Font-Bold="True" ForeColor="White" Width="90px" /></td>
        </tr>
        <tr>
            <td style="height: 34px;" colspan="2" align="center">
                <asp:Label ID="LblMsg" runat="server" ForeColor="Red" Width="251px" Font-Size="9pt"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 21px;" colspan="2">
                <asp:LinkButton ID="LBttnForgotPswd" runat="server" Width="203px" Font-Bold="True" CssClass="LinkButton" Visible="False" Font-Size="9pt">Forgot your Password ?</asp:LinkButton></td>
        </tr>
    </table>
    </asp:Panel>
    </div>  
</asp:Content>

