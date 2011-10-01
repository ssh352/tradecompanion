<%@ Page Language="VB" MasterPageFile="~/Forms/MstForContents.master" AutoEventWireup="false" CodeFile="FrmUserInfo.aspx.vb" Inherits="FrmUserInfo" title="Scalper - Registration Form" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="BttnSend" Height="700px" Width="425px">
    
    <table style="width: 503px">
    
        <tr>
            <td id=td2  align="left" colspan="3"  runat="server" visible="true">
                <span style="font-size: 12pt; font-family: Arial; color:#4682b4"><strong>
                If you are already a registered user
                <a href="FrmLogIn.aspx">Click Here</a> to log in.</strong></span></td>
        </tr>
        
        
        <tr>
            <td align="right" style="width: 264px; color: darkgray">&nbsp;
            </td>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 264px; color: darkgray;">
                <strong>
                User Name(Email Address) *</strong></td>
            <td style="width: 100px">
                <asp:TextBox ID="TxtEMailAdd" runat="server" MaxLength="50" CssClass="TEXTBOX"></asp:TextBox></td>
            <td style="width: 100px">
            </td>
        </tr>
        <tr>
            <td class="TDFONT" align="right" style="width: 264px">
            
                Name *</td>
            <td style="width: 100px">
                <asp:TextBox ID="TxtFirstName" runat="server" CssClass="TEXTBOX" MaxLength="50"></asp:TextBox></td>
            <td style="width: 100px">
            </td>
        </tr>
        <tr>
            <td class="TDFONT" style="width: 264px; height: 26px;" align="right">
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                Company (If Applicable)&nbsp;&nbsp;&nbsp;</td>
            <td style="width: 100px; height: 26px;">
                <asp:TextBox ID="TxtCompany" runat="server" CssClass="TEXTBOX"></asp:TextBox></td>
            <td style="width: 100px; height: 26px;">
            </td>
        </tr>
        <tr>
            <td class="TDFONT" align="right" style="width: 264px">
                Address 1 *</td>
            <td style="width: 100px">
                <asp:TextBox ID="TxtAddress1" runat="server" MaxLength="50" CssClass="TEXTBOX"></asp:TextBox></td>
            <td style="width: 100px">
            </td>
        </tr>
        <tr>
            <td class="TDFONT" align="right" style="width: 264px; height: 26px;">
                Town/City *</td>
            <td style="width: 100px; height: 26px;">
                <asp:TextBox ID="TxtTown_City" runat="server" MaxLength="50" CssClass="TEXTBOX"></asp:TextBox></td>
            <td style="width: 100px; height: 26px;">
            </td>
        </tr>
        <tr>
            <td class="TDFONT" align="right" style="width: 264px">
                Province&nbsp;&nbsp;&nbsp;</td>
            <td style="width: 100px">
                <asp:TextBox ID="TxtProvince" runat="server" CssClass="TEXTBOX"></asp:TextBox></td>
            <td style="width: 100px">
            </td>
        </tr>
        <tr>
            <td class="TDFONT" align="right" style="width: 264px">
                Postal Code *</td>
            <td style="width: 100px">
                <asp:TextBox ID="TxtPostalCode" runat="server" MaxLength="10" CssClass="TEXTBOX"></asp:TextBox></td>
            <td style="width: 100px">
            </td>
        </tr>
        <tr>
            <td class="TDFONT" align="right" style="width: 264px">
                Country&nbsp; *</td>
            <td style="width: 100px">
                <asp:DropDownList ID="DDLCountry" runat="server" Width="157px">
                </asp:DropDownList></td>
            <td style="width: 100px">
                </td>
        </tr>
        <tr>
            <td class="TDFONT" align="right" style="width: 264px">
                Contact phone Number *
            </td>
            <td style="width: 100px">
                <asp:TextBox ID="TxtContactPhoneNo" runat="server" MaxLength="15" CssClass="TEXTBOX"></asp:TextBox></td>
            <td style="width: 100px">
            </td>
        </tr>
        <tr>
            <td class="TDFONT" align="right" style="width: 264px">
                Fax number&nbsp;&nbsp;&nbsp;</td>
            <td style="width: 100px">
                <asp:TextBox ID="TxtFaxNo" runat="server" CssClass="TEXTBOX"></asp:TextBox></td>
            <td style="width: 100px">
            </td>
        </tr>
        <tr>
            <td class="TDFONT" align="right" style="width: 264px">
                Where did you first hear about us?&nbsp;&nbsp;&nbsp;</td>
            <td style="width: 100px">
                <asp:DropDownList ID="DDLHearAbtUs" runat="server" Width="155px">
                </asp:DropDownList></td>
            <td style="width: 100px">
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 264px">&nbsp;
            </td>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 264px">&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 264px">
            </td>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 264px">
            </td>
            <td style="width: 100px">
                <asp:Button ID="BttnSend" CssClass=BUTTON runat="server" Text="Send" Font-Bold="True" Width="93px" /></td>
            <td style="width: 100px">
            </td>
        </tr></table>
     
    </asp:Panel>
</asp:Content>





