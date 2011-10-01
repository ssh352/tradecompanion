<%@ Page Language="VB" CodeFile="trader.aspx.vb"  EnableEventValidation="false" masterpagefile="~/Forms/MstForContents.master" Inherits="Trader" %>

<%@ Register Assembly="SilkWebware.DateTimeSelector" Namespace="SilkWebware.DateTimeSelector"
    TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
            <table style="width:500px; background-color:#64829A" >
                 <tr>
                    <td colspan="3" align=center>
                        <span style="color: #ffffff; font-family:Arial;font-size: 18pt">Chart Configuration for Traders</span></td>
                </tr>
                <tr>
                <td colspan="3" style="text-align: left">
                    <hr style="color: #b0b38a" /></td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: left">
                        <table >
                            <tr>
                                <td>
                                <asp:DropDownList ID="ExistPageList" Width="115px" runat="server" AutoPostBack="FALSE" EnableViewState="true">
                                </asp:DropDownList></td>
                                <td>
                                <asp:button id="ViewPage" Width="110px" text="Show Chart"  runat="server"/></td>
                                <td>
                                <asp:Button ID="DeletePage" Width="110px" runat="server" Text="Delete Chart" /></td>
                            </tr>
                        </table>
                     </td>
                 </tr>
                 
                <tr>
                    <td colspan="3">
                        <hr style="color: #b0b38a" /></td>
                </tr>
                <tr>
                    <td style="text-align: left" colspan="3">
                        <table >
                            <tr>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="TraderList" Width="230px" AutoPostBack="false" runat="server" OnSelectedIndexChanged="TraderList_SelectedIndexChanged"  EnableViewState="true">
                                    </asp:DropDownList></td>
                                <td style="text-align: left">
                                    <asp:button id="CreateChart"  text="Create Chart" onclick="CreatePage_click"  runat="server"/></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="3" style="height: 416px">
                        <table style="width: 100%">
                            <tr style="background-color:#DCDFFC">
                                <td style="vertical-align:top;text-align: right; margin-right:10; font-family:Arial; font-size:9pt">
                                    Selected Trader:</td>
                                <td style="vertical-align:top;text-align: left; margin-left:10">
                                    <asp:TextBox ID="SltTrader" runat="server" Width="180px"></asp:TextBox></td>
                            </tr>
                            <tr style="background-color:#DAE0E9">
                                <td style="vertical-align:top;text-align: right; font-family:Arial; font-size:9pt" >
                                    Trader Name:</td>
                                <td style="vertical-align:top;text-align: left; margin-left:10; font-family:Arial; font-size:9pt">
                                    <asp:TextBox ID="txtTraderName" runat="server" Width="180px"></asp:TextBox></td>
                            </tr>
                            <tr style="background-color:#DCDFFC">
                                <td style="vertical-align:top;text-align: right; font-family:Arial; font-size:9pt; height: 106px;" >
                                    Select Trading ID:<br /><asp:ListBox ID="SenderID" runat="server" SelectionMode="Multiple" Width="180px"  OnSelectedIndexChanged="SenderID_Selected" AutoPostBack="True" EnableViewState="true" Height="88px"></asp:ListBox></td>
                                <td style="vertical-align:top;text-align: left; margin-left:10; font-family:Arial; font-size:9pt; height: 106px;">
                                    Selected Trading ID:<br/><asp:TextBox ID="SltSenderID" runat="server" Height="81px" TextMode="MultiLine" Width="182px"></asp:TextBox></td>
                            </tr>
                        <tr style="background-color:#DAE0E9">
                            <td style="vertical-align:top;text-align: right; font-family:Arial; font-size:9pt">
                                Select Symbols:<br /><asp:ListBox ID="Symbols" runat="server" SelectionMode="Multiple" Width="180px"  OnSelectedIndexChanged="Symbols_Selected" AutoPostBack="TRUE" EnableViewState="true" Height="87px"></asp:ListBox></td>
                            <td style="vertical-align:top;text-align: left; margin-left:10; font-family:Arial; font-size:9pt">
                                Selected Symbols:<br/><asp:TextBox ID="SltSymbols" runat="server" Height="81px" TextMode="MultiLine" Width="180px" AutoPostBack="FALSE"></asp:TextBox></td>
                        </tr>
                        <%--<tr style="background-color:#DAE0E9">
                        <td style="vertical-align:top;text-align: left; margin-left:10; font-family:Arial; font-size:9pt">
                                    <asp:Button ID="Button" runat="server" Width="180px"></asp:Button> </td>
                                <td style="vertical-align:top;text-align: right; font-family:Arial; font-size:9pt" >
                                    </td>
                                
                                   
                            </tr>--%>
                        <tr style="background-color:#DCDFFC">
                            <td style="vertical-align:top;text-align: right; font-family:Arial; font-size:9pt">
                                Chart Start Date:</td>
                            <td style="vertical-align:top;text-align: left; margin-left:10">
                                <cc1:DateTimeSelector ID="stDate" ClientDateValidation="false" runat="server" /></td>
                        </tr>
                        <tr style="background-color:#DAE0E9">
                            <td style="vertical-align:top;text-align: right; font-family:Arial; font-size:9pt">
                                Chart End Date:</td>
                            <td style="vertical-align:top;text-align: left; margin-left:10">
                                <cc1:DateTimeSelector ID="edDate" ClientDateValidation="false" runat="server" /></td>
                        </tr>
                        <tr style="background-color:#DCDFFC">
                            <td style="vertical-align:top;text-align: right; font-family:Arial; font-size:9pt">
                                Address:</td>
                            <td style="vertical-align:top;text-align: left; margin-left:10">
                                <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Width="180px"></asp:TextBox></td>
                        </tr>
                        <tr style="background-color:#DAE0E9">
                            <td style="vertical-align:top;text-align: right; font-family:Arial; font-size:9pt">
                                Contact:</td>
                            <td style="vertical-align:top;text-align: left; margin-left:10">
                                <asp:TextBox ID="txtContact" runat="server" Width="180px"></asp:TextBox></td>
                        </tr>
                        <tr style="background-color:#DCDFFC">
                            <td style="vertical-align:top;text-align: right; font-family:Arial; font-size:9pt">
                                Email:</td>
                            <td style="vertical-align:top;text-align: left; margin-left:10">
                                <asp:TextBox ID="TxtEmail" runat="server" AutoCompleteType="Disabled" Width="180px"></asp:TextBox></td>
                        </tr>
                        <tr style="background-color:#DAE0E9">
                            <td style="vertical-align:top;text-align: right; font-family:Arial; font-size:9pt">
                                Description:</td>
                            <td style="vertical-align:top;text-align: left; margin-left:10">
                                <asp:TextBox ID="txtDescription" runat="server" AutoCompleteType="Disabled" TextMode="MultiLine"
                                    Width="180px"></asp:TextBox></td>
                        </tr>
                        <tr style="background-color:#DCDFFC">
                            <td style="vertical-align:top;text-align: right; height: 60px; font-family:Arial; font-size:9pt">
                                Select Logo:<br/><asp:DropDownList ID="LogoImage" runat=server Width="180px" OnSelectedIndexChanged="Logo_Selected" AutoPostBack="true" EnableViewState="true">
                                    </asp:DropDownList></td>
                            <td style="vertical-align:top;text-align: left; margin-left:10; height: 60px; font-family:Arial; font-size:9pt">
                                Logo Preview:<br/><asp:Image ID="Preview" runat=server BorderColor="Black" BorderWidth="1px" Height="37px" Width="130px" /></td>
                        </tr>
                        <tr style="background-color:#DAE0E9">
                            <td style="text-align: right" >
                                <asp:Button ID="btnReset" runat="server" Text="Reset Page" /></td>            
                            <td style="vertical-align:top;text-align: left; margin-left:10">
                                <asp:Button ID="btnSubmit" runat="server" Text="Save Page" /></td>
                        </tr>
                   </table>
                </td>
            </tr>
         </table>
      
 </asp:Content>
