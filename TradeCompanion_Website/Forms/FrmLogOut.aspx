<%@ Page Language="VB" MasterPageFile="~/Forms/MstForContents.master" AutoEventWireup="false" CodeFile="FrmLogOut.aspx.vb" Inherits="FrmLogOut" title="Scalper - Log Out !!!" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server" Height="50px" Width="425px">
        <table style="width: 486px">
            <tr>
                <td style="width: 85px">
                </td>
                <td style="width: 160px">&nbsp;
                </td>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td align="center" colspan="3">
                    <strong style="color: #0066ff"><span style="font-family: Arial">You have successfully logged out.!!!</span></strong></td>
            </tr>
            <tr>
                <td style="width: 85px; height: 21px;">
                </td>
                <td style="width: 160px; height: 21px;">&nbsp;
                </td>
                <td style="width: 100px; height: 21px;">
                </td>
            </tr>
            <tr>
                <td style="width: 85px">
                </td>
                <td style="width: 160px">
                    </td>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td align="center" colspan="3">
                    <asp:HyperLink ID="HyperLink1" runat="server" CssClass="LinkButton" Font-Bold="True"
                        NavigateUrl="~/Forms/FrmLogIn.aspx" Width="115px">Click Here To Login Again</asp:HyperLink></td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>

