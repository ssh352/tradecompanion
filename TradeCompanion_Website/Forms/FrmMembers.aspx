<%@ Page Language="VB" MasterPageFile="~/Forms/MstForContents.master" AutoEventWireup="false" CodeFile="FrmMembers.aspx.vb" Inherits="Forms_FrmMembers" title="Scalper Systems - Members" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server" Height="566px" Width="444px">
        <table style="width: 470px; height: 125px; font-size: 9pt; font-family: Arial;">
                       
            <tr>
                <td colspan="3" style="font-weight: bold; font-size: 20pt; color: #3333ff; font-family: 'Book Antiqua';
                    height: 5px" align="center">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Members/MembersArea.gif" /></td>
            </tr>
            
            <% If SignInFlag = False Then%>   
            
            <tr>
                <td colspan="3" style="font-weight: bold; height: 12px;">YOU ARE NOT LOGGED IN - 
                    <asp:HyperLink ID="HyperLink1" runat="server" CssClass="LinkButton" Font-Bold="True"
                        NavigateUrl="~/Forms/FrmLogIn.aspx" Width="115px">Log In</asp:HyperLink>&nbsp;<br />
                    You must be registered and logged in to access the members area.<br />
                </td>
            </tr>
            
            <%End If%>
        </table>
        <table style="width: 464px; height: 281px; font-size: 9pt; font-family: Arial;">
            <tr>
                <td align="center" style="width: 45px; height: 67px" valign="bottom">
                </td>
                <td style="width: 100px; height: 67px" align="center" valign="bottom">
                   <%-- <br />
                    <strong>--%>
                    <% If SignInFlag = True Then%>  &nbsp; <a href="FrmDownloads.aspx"><%End If%><asp:Image ID="Img_CommentryFiles" runat="server" ImageUrl="~/Images/Members/membersIcon3.gif" />
                        <%--</strong>--%>
                        </td>
                <td style="width: 100px; height: 67px" align="center" valign="bottom">
                    
                    
                    <% If SignInFlag = True Then%>  
                    <a href="FrmDownloads.aspx">
                    <%End If%>
                    <asp:Image ID="Img_Tools" runat="server" ImageUrl="~/Images/Members/membersIcon1.gif" />
                    <%--</strong>--%>
                    </td>
            </tr>
            <tr>
                <td align="center" style="width: 45px; color: royalblue; height: 32px" valign="middle">
                </td>
                <td align="center" style="width: 100px; height: 32px; color: royalblue;" valign="middle">
                        Commentry Files</td>
                <td align="center" style="width: 100px; height: 32px; color: royalblue;" valign="middle">
                        Tools and Indicators</td>
            </tr>
            <tr>
                <td align="center" style="width: 45px; height: 72px" valign="middle">
                </td>
                <td style="width: 100px; height: 72px" align="center" valign="middle">
                    <br />
                    <% If SignInFlag = True Then%>
                    <%End If%>
                </td>
                <td style="width: 100px; height: 72px" align="center" valign="middle">
                    <br />
                    <strong>
                    
                    <% If SignInFlag = True Then%>  
                    <a href="FrmDownloads.aspx">
                    <%End If%>
                    
                    <asp:Image ID="Img_Data" runat="server" ImageUrl="~/Images/Members/membersIcon2.gif" />
                    </strong></td>
                    
            </tr>
            <tr>
                <td align="center" style="width: 45px; color: royalblue; height: 28px" valign="middle">
                </td>
                <td align="center" style="width: 100px; color: royalblue; height: 28px" valign="middle">
                    Trade Station</td>
                <td align="center" style="width: 100px; color: royalblue; height: 28px" valign="middle">
                    Data</td>
            </tr>
        </table>
        
        <table style="width: 464px; font-size: 9pt; font-family: Arial;">
            <tr>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    If you are a current Client and should have access but are having problems or can't
                    remember your ID and password, let <a href="mailto:admin@scalper.co.uk"><b>admin@scalper.co.uk</b></a>
                    know. If you don't get a quick response, or have any other access problems here at the website, let 
                   <a href="mailto:sales@scalper.co.uk"><b>Franco</b></a>
                    Know
                    <%--at the website, let <a href="#"><b></b>Franco</a> know.</B>--%>
                    </td>
            </tr>
            <tr>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>

