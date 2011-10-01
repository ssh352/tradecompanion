<%@ Page Language="VB" CodeFile="chart.aspx.vb" masterpagefile="~/Forms/ChartMaster.master" Inherits="chart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


    <table style="width:500px; font-family: Arial;" > 
        <tr>
            <td colspan="2" style="font-size: 12pt; color:#64829A; font-family: Arial; text-align: center; height: 25px;">
                <strong>Trader Details</strong></td>
        </tr>
        <tr style="font-family: Arial">
            <td valign="top" style="width:20%;text-align:center" >
                <asp:Image ID="LogoImage"  runat="server" BorderWidth="1px" ImageUrl="~/Forms/Logo/LOGO1.gif" Width="130px" /></td>
            <td >
                <table style="width:100%; font-size: 9pt; font-family: Arial;">
                    <tr>
                        <td align="right" valign="top" style="width: 217px; color:#808080;">
                            <strong>
                        Login ID</strong>&nbsp;
                        </td>
                        <td align="left" valign="top">
                            <asp:Label ID="Label1" runat="server" Text="Label" Font-Bold="False" ForeColor="#808080"></asp:Label></td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" style="width: 217px; color:#808080;" >
                            <strong>
                        Trader Name</strong>&nbsp;
                        </td>
                        <td align="left" valign="top">
                            <asp:Label ID="Label2" runat="server" Text="Label" Font-Bold="False" ForeColor="#808080"></asp:Label></td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" style="width: 217px; color:#808080; font-weight: bold;" >
                        Contact No.&nbsp;
                        </td>
                        <td align="left" valign="top">
                            <asp:Label ID="Label3" runat="server" Text="Label" Font-Bold="False" ForeColor="#808080"></asp:Label></td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" style="width: 217px; color:#808080; height: 17px;" >
                            <strong>
                        E-mail ID</strong>&nbsp;
                        </td>
                        <td align="left" valign="top" style="height: 17px">
                            <asp:Label ID="Label4" runat="server" Text="Label" Font-Bold="False" ForeColor="#808080"></asp:Label></td>
                    </tr>
                    <tr>
                        <td  align="right" valign="top" style="width: 217px; color:#808080;" >
                            <strong>
                        Address</strong>&nbsp;
                        </td>
                        <td  align="left" valign="top" style="height: 21px">
                            <asp:Label ID="Label5" runat="server" Text="Label" Font-Bold="False" ForeColor="#808080"></asp:Label></td>
                    </tr>
                    <tr>
                        <td  align="right" valign="top" style="width: 217px; color:#808080;" >
                            <strong>
                        Trader Description</strong>&nbsp;
                        </td>
                        <td  align="left" valign="top">
                            <asp:Label ID="Label6" runat="server" Text="Label" Font-Bold="False" ForeColor="#808080"></asp:Label></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr style="font-size: 12pt; font-family: Arial">
            <td colspan="2" >
            <hr />
            </td>
        </tr>
        <tr style="font-size: 12pt; font-family: Arial">
            <td colspan="2" style="font-size: 12pt; color:#64829A; font-family: Arial; text-align: center; height: 31px;">
            <strong>Chart Analysis</strong></td>
        </tr>
        <tr style="font-size: 12pt; font-family: Arial">
            <td colspan="2" align="center" >
                <div id="DATACELL"  style="overflow: auto; width: 750px; height: 600px; text-align:center; border:2px,black"><asp:Image ID="ChartImage"  runat="server" /></div></td>
        </tr>
    </table>
 </asp:Content>