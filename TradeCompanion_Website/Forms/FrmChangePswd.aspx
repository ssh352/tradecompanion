<%@ Page Language="VB" MasterPageFile="~/Forms/MstForContents.master" AutoEventWireup="false" CodeFile="FrmChangePswd.aspx.vb" Inherits="FrmChangePswd" title="Scalper - Change Password" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <asp:Panel ID="Panel_ChangePswd" runat="server" DefaultButton="BttnChange" Height="500px"
            Width="525px">
            <table id="TABLE1" style="width: 461px">
                <tr>
                    <td style="width: 171px">
                    </td>
                    <td style="width: 157px">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="right" class="TDFONT" style="width: 171px">
                        </td>
                    <td align="center" style="width: 157px; height: 18px">
                        </td>
                </tr>
                <tr>
                    <td align="right"  class="TDFONT" style="width: 171px; height: 24px;">
                        <strong>&nbsp; Old Password :</strong></td>
                    <td align="center" style="width: 157px; height: 24px">
                        <asp:TextBox ID="TxtOldPswd" runat="server" CssClass="TEXTBOX" TextMode="Password" Height="22px" Width="155px"></asp:TextBox></td>
                </tr>
                <tr>
                   <td align="right" class="TDFONT" style="height: 21px; width: 171px;">
                        New Password :</td>
                    <td align="center" style="width: 157px; height: 21px">
                        <asp:TextBox ID="TxtNewPswd" runat="server" CssClass="TEXTBOX" TextMode="Password" Height="22px" Width="155px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="TDFONT" style="width: 171px; height: 21px">
                        Confirm Password :
                    </td>
                    <td align="center" style="width: 157px; height: 21px">
                        <asp:TextBox ID="TxtConfPswd" runat="server" CssClass="TEXTBOX" TextMode="Password" Height="22px" Width="155px"></asp:TextBox>
                    </td>                    
                </tr>
                <tr>
                    <td align="right" class="TDFONT" style="width: 171px">
                    </td>
                    <td align="center" style="width: 157px">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 171px">
                    </td>
                    <td align="left" style="width: 157px">
                        <asp:Button ID="BttnChange" runat="server" BackColor="DarkGray" CssClass="BUTTON"
                            Font-Bold="True" ForeColor="White" Text="Change" Width="90px" /></td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td style="height: 31px;" colspan="2" align="center">
                        </td>
                </tr>
            </table>
        </asp:Panel>
</asp:Content>

