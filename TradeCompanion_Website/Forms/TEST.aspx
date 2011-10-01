<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TEST.aspx.vb" Inherits="Forms_TEST" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script language="javascript" type="text/javascript">
  javascript:window.history.forward(1);
</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        &nbsp;&nbsp;
        <table style="width: 592px; height: 196px" border="1" bordercolor=black>
            <tr>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                    <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Height="98px" Width="198px"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                    <asp:Button ID="Button1" runat="server" Text="Button" /></td>
            </tr>
            <tr>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Forms/Test2.aspx">HyperLink</asp:HyperLink></td>
            </tr>
            <tr>
                <td style="width: 100px">
                </td>
                <td id=AA runat="server" style="width: 100px">
                </td>
            </tr>
        </table>
    
    </div>
        <asp:Label ID="Label1" runat="server" Height="135px" Text="Label" Width="480px"></asp:Label>
        <asp:Button ID="Button3" runat="server" Text="Change WebConfig" /><br />
        <asp:TextBox ID="TxtEncrypt" runat="server" Width="200px"></asp:TextBox>
        <asp:Button ID="Button2" runat="server" Text="Button" /><br />
        <asp:TextBox ID="Txtresult" runat="server"></asp:TextBox>
    </form>
</body>
</html>
