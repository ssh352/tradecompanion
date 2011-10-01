<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FrmSendToFrd.aspx.vb" Inherits="FrmSendToFrd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Scalper Systems - Send To A Friend</title>
    <link rel="stylesheet" type="text/css" href="../CSS/InternalFormStyles.css">
    <link rel="stylesheet" type="text/css" href="../CSS/style.css">
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="450px">
            <tr>
                <td colspan="3" id="Inform" runat="server" visible="true" style="color: #0000ff" class="text5">
                    <strong>
                    To send your friend an email linking to this page on
                        <br />
                        tradecompanion.co.uk, simply fill
                    in the following form              
                    </strong>.</td>
            </tr>
            <tr>
                <td colspan="3" id="Sorry" runat="server" visible="false" style="color: #ff0066" class="text5">
                    <strong>
                    Sorry, there was an error with your form. Please fill
                        <br />
                         in all required 
                    fields correctly
                    ensure both yours
                        <br />
                         and your friend's email addresses are written correctly
                    </strong>.</td>
            </tr>
            <tr>
                <td colspan="3" id="Thanks" runat="server" visible="false" style="color: #009900" class="text5">
                    <strong>
                    Thankyou for using our Send To A Friend system. An 
                        <br />
                        email has been sent to
                    with a link to
                    the page you are 
                        <br />
                        currently on.
                    </strong></td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;
                    <asp:Label ID="Label1" runat="server" Text="Label" Width="250px" Visible="False"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 157px">
                </td>
                <td style="width: 100px">
                </td>
                <td style="width: 84px">
                </td>
            </tr>
            <tr>
                <td style="width: 157px">
                    <strong>
                    Your Name :</strong></td>
                <td style="width: 100px">
                    <asp:TextBox ID="TxtName" runat="server" CssClass="TEXTBOX"></asp:TextBox></td>
                <td style="width: 84px">
                </td>
            </tr>
            <tr>
                <td style="width: 157px">
                    <strong>Your EMail Address :</strong></td>
                <td style="width: 100px">
                    <asp:TextBox ID="TxtFrom" runat="server" CssClass="TEXTBOX"></asp:TextBox></td>
                <td style="width: 84px">
                </td>
            </tr>
            <tr>
                <td style="width: 157px">
                    <strong>Send to EMail Address :</strong></td>
                <td style="width: 100px">
                    <asp:TextBox ID="TxtSendTo" runat="server" CssClass="TEXTBOX"></asp:TextBox></td>
                <td style="width: 84px">
                </td>
            </tr>
            <tr>
                <td style="width: 157px; height: 100px">
                    <strong>
                    Message (Optional) :</strong></td>
                <td style="width: 100px; height: 100px">
                    <asp:TextBox ID="TxtMsg" runat="server" Height="84px" TextMode="MultiLine" CssClass="TEXTBOX"></asp:TextBox></td>
                <td style="width: 84px; height: 100px">
                </td>
            </tr>
            <tr>
                <td style="width: 157px">
                </td>
                <td style="width: 100px">
                    <asp:Button ID="BttnSend" runat="server" Text="Send" CssClass="BUTTON" Width="91px" /></td>
                <td style="width: 84px">
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    &nbsp;
                    </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
