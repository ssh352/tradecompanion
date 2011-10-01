<%@ Page Language="VB" MasterPageFile="~/Forms/MstForContents.master" AutoEventWireup="false" CodeFile="FrmThanks.aspx.vb" Inherits="FrmThanks" title="Scalper - Thanks For Login" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div align=left style="width: 502px; height: 1px">
    <table style="height: 149px" id="TABLE1" language="javascript">
        <tr>
            <td style="width: 100px" align="left">&nbsp;
            </td>
            <td style="width: 276px" align="left">
            </td>
            <td style="width: 80px">
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center" style="font-size: 20px; color: #0066ff">
                <span style="font-size: 12pt; font-family: Arial; color: royalblue;"><strong>
                Welcome</strong></span>
                <asp:Label ID="LblUseName" runat="server" Font-Bold="False" ForeColor="RoyalBlue"
                    Width="118px" Font-Size="9pt" Font-Names="Arial"></asp:Label></td>
        </tr>
        <tr>
        <td style="width:80px" colspan="3">
            <br />
        </td>
        </tr>
        <tr>
            <td align="left" colspan="3" style="height: 23px" valign="top">
                <span style="font-size: 9pt">
                <strong><span style="color: steelblue; font-family: Arial"><span style="color: steelblue">
                Download TradeCompanion &nbsp;
                    <br />
                    <br />
                    [ Version - 1.0 / Build - 35 ]</span><asp:HyperLink ID="HyperLink2" runat="server" ImageUrl="~/Images/index2_25.jpg" NavigateUrl="~/Download/Products/TC_Setup.zip">HyperLink</asp:HyperLink><br />
                    [ Version - 1.0 / Build - 39 ]&nbsp;
                    <asp:HyperLink ID="HyperLink3" runat="server" ImageUrl="~/Images/index2_25.jpg" NavigateUrl="~/Download/Products/TC1039.zip">HyperLink</asp:HyperLink></span></strong></span></td>
        </tr>
        <tr>
        <td style="width:100px; height: 40px;" colspan="3">
            <br />
        </td>
        </tr>
        <tr>
            <td style="height: 23px; text-align: left;" colspan="3" align="center">
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="FrmChangePswd.aspx"
                    Width="149px" CssClass="LinkButton" Font-Bold="True" Font-Size="9pt" ForeColor="SteelBlue" Font-Names="Arial">Change Your Password</asp:HyperLink></td>
        </tr>
        <tr>
            <td id="p1" runat=server align="center" colspan="3" style="height: 23px; text-align: right;">
            </td>
        </tr>
        <tr>
            <td align="center" colspan="3" style="height: 15px; text-align: left; color: #0066ff; font-family: Arial;">
                <strong><span style="font-size: 9pt; color: steelblue">
            How are other traders performing ? </span></strong>
            </td>
        </tr>
        <tr>
            <td id="p2" runat=server align="left" colspan="3" style="height: 23px">
            </td>
        </tr>
        
    </table>   
    
    </div>
    
</asp:Content>

