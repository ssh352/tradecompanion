<%@ Page Language="VB" MasterPageFile="~/Forms/MstForContents.master" AutoEventWireup="false" CodeFile="FrmCallBack.aspx.vb" Inherits="Forms_FrmCallBack" title="Scalper - Request Call Back" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server" Height="500px" Width="525px" DefaultButton="BttnSend">
        <table style="width: 494px; height: 140px" id="TblCallReq" runat=server>
            <tr>
                <td style="width: 142px">
                </td>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td style="width: 142px">&nbsp;
                </td>
                <td style="width: 100px">&nbsp;
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2" style="height: 30px" class="TDFONT" valign="bottom">
                    Simply enter your name and telephone number into the form below and 
                </td>
            </tr>
            <tr>
            <td align="left" colspan="2" style="height: 30px" class="TDFONT">
                    we will give you a ring.
                </td>
            </tr>
            <tr>
                <td align="right" class="TDFONT" style="width: 142px">&nbsp;
                </td>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td style="width: 142px" class="TDFONT" align="right">
                    &nbsp;Your Name&nbsp;</td>
                <td style="width: 100px">
                    <asp:TextBox ID="TxtName" runat="server" CssClass="TEXTBOX" Width="183px"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 142px"  class="TDFONT" align="right">
                    &nbsp;Telephone No&nbsp;
                </td>
                <td style="width: 100px">
                    <asp:TextBox ID="TxtTelephone" runat="server" CssClass="TEXTBOX" Width="183px"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right" class="TDFONT" style="width: 142px">
                </td>
                <td style="width: 100px">&nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" class="TDFONT" colspan="2">
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtTelephone"
                        ErrorMessage="Telephone No should be Numeric" ValidationExpression="^([0-9]*|\d*\d{1}?\d*)$"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align="right" class="TDFONT" style="width: 142px">
                </td>
                <td align="left" style="width: 100px">
                    <asp:Button ID="BttnSend" runat="server" CssClass="BUTTON" Text="Send" Width="89px" /></td>
            </tr>
            <tr>
                <td align="center" class="TDFONT" colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" class="TDFONT" colspan="2">
                    </td>
            </tr>
        </table>
        <table style="width: 490px" id="TblConfirm" runat=server visible="false" >
            <tr>
                <td style="width: 100px">&nbsp;
                </td>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <strong style="color: royalblue">
                    EMAIL&nbsp; SENT</strong></td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="Label1" runat="server" Text="Thank you for your Enquiry " Font-Bold="True"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 100px; height: 21px;">
                </td>
                <td style="width: 100px; height: 21px;">
                </td>
            </tr>
        </table>
    </asp:Panel>

</asp:Content>

