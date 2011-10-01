<%@ Page Language="VB" MasterPageFile="~/Forms/MstInternal.master" AutoEventWireup="false" CodeFile="FrmSignUp.aspx.vb" Inherits="FrmSignUp" title="Scalper - SignUp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server" Height="458px" Width="477px">
        <table id="TblSignUp" language="javascript" runat="server"  style="width: 461px">
            <tr>
                <td style="width: 124px">
                </td>
                <td style="width: 157px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 124px; color: #000000; height: 18px">
                    <strong>EMail Addres :</strong></td>
                <td align="center" style="width: 157px; height: 18px">
                    <asp:TextBox ID="TxtEMailID" runat="server" CssClass="TEXTBOX"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 124px">
                </td>
                <td align="center" style="width: 157px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 124px">
                </td>
                <td align="center" style="width: 157px">
                    <asp:Button ID="BttnSignUp" runat="server" BackColor="DarkGray" CssClass="BUTTON"
                        Font-Bold="True" ForeColor="White" Text="Sign Up" Width="90px" /></td>
            </tr>
            <tr>
                <td colspan="2" style="height: 61px">
                    <asp:Label ID="LblMsg" runat="server" ForeColor="Red" Width="251px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 124px; height: 21px">
                </td>
                <td style="width: 157px; height: 21px">
                </td>
            </tr>
        </table>
        <table id="TblTrue" style="width: 460px" runat="server" visible="false">
            <tr>
                <td style="width: 100px">&nbsp;
                </td>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td colspan="2" class="TDFONT">
                    Thankyou for signing up. An email confirming your registration and containing your
                    login details have been sent to your ID.</td>
            </tr>
            <tr>
                <td style="width: 100px; height: 21px">
                </td>
                <td style="width: 100px; height: 21px">
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>

