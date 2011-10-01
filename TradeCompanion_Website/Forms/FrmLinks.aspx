<%@ Page Language="VB" MasterPageFile="~/Forms/MstForContents.master" AutoEventWireup="false" CodeFile="FrmLinks.aspx.vb" Inherits="FrmLinks" title="Recommended Links" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="width: 508px">
        <tr>
            <td style="width: 245px">&nbsp;
            </td>
            <td style="width: 70px">
            </td>
        </tr>
        <tr>
            <td align="left" colspan="2" style="height: 25px">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Links/RecmdLinks.gif" /></td>
        </tr>
        <tr>
            <td style="height: 25px" align="left" colspan="2">
                <asp:LinkButton ID="LBttnIndicesForex" runat="server"  
                    BackColor="RoyalBlue" Font-Bold="True"
                    ForeColor="AliceBlue" Width="125px" CssClass="LinkButton" Height="22px">Indices and Forex</asp:LinkButton>&nbsp;
                <asp:LinkButton ID="LBttnIntradayCharts" runat="server" BackColor="RoyalBlue" Font-Bold="True" 
                ForeColor="AliceBlue" Width="115px" CssClass="LinkButton" Height="22px">Intraday Charts</asp:LinkButton>&nbsp;
                <asp:LinkButton ID="LBttnStockMkt" runat="server" BackColor="RoyalBlue" Font-Bold="True"
                    ForeColor="AliceBlue" Width="115px" CssClass="LinkButton" Height="22px">Stock Market</asp:LinkButton>&nbsp;
                <asp:LinkButton ID="LBttnOther" runat="server" BackColor="RoyalBlue" Font-Bold="True"
                    ForeColor="AliceBlue" Width="115px" CssClass="LinkButton" Height="22px">Other</asp:LinkButton></td>
        </tr>
        <tr>
            <td style="width: 245px; height: 21px">
            </td>
            <td style="width: 70px; height: 21px">
            </td>
        </tr>
        <tr>
            <td align="left" colspan="2">
                <asp:LinkButton ID="LBttnSuggestLink" runat="server" Font-Bold="True" 
                CssClass="LinkButton">SUGGEST A LINK</asp:LinkButton>
                
                
                <%--<% If Session("Access") = 5 Then%>
                <asp:LinkButton ID="LBttnViewSuggestLink" runat="server"  Font-Bold="True" 
                CssClass="LinkButton">| VIEW SUGGESTED LINKS</asp:LinkButton>
                <% End If%>--%>
                
                
             </td>
        </tr>
        <tr>
            <td style="width: 245px; height: 21px;">
                &nbsp;
            </td>
            <td style="width: 70px; height: 21px;">
            </td>
        </tr>
     </table>
    
     <table id="TblGrid" runat="server">
    
                    <tr>
                        <td style="width: 103px" align="left">
                        
                          <%--Load Grid For G User --%>
                        
                          <% If Session("Access") < 5 Or Session("Access") = Nothing Then%> 
                <asp:GridView ID="GViewIndicesForex" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Width="372px">
                <Columns>
                    <asp:HyperLinkField Target="_blank" HeaderText="Indices And Forex"  
                    DataNavigateUrlFields="LinkNum" DataTextField="sitename" 
                    DataNavigateUrlFormatString="~/Forms/FrmHLink.aspx?LinkNum={0}" />
               </Columns>  
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="Red" />
                    <RowStyle BackColor="#EFF3FB" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
            
                </asp:GridView>
                
                 <% End If%>
                 
                 <%--End of G User Grid--%>
                
                            &nbsp;
                        </td>
                        <td style="width: 123px">
                        </td>
                    </tr>
         <tr>
             <td align="left" style="width: 103px">
                 &nbsp;</td>
             <td style="width: 123px">
             </td>
         </tr>
         <tr>
             <td align="left" style="width: 103px">
                 
                <%--Load Grid For Admin--%>
                
                <% If Session("Access") = 5 Then%> 
                 
                <asp:GridView ID="GViewIndicesForex_Admin" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Width="372px" Visible="False">
                <Columns>
                    <asp:HyperLinkField Target="_blank" HeaderText="Indices And Forex"  
                    DataNavigateUrlFields="LinkNum" DataTextField="sitename" 
                    DataNavigateUrlFormatString="~/Forms/FrmHLink.aspx?LinkNum={0}" />
                                        
                    <asp:HyperLinkField DataNavigateUrlFields="LinkNum" DataNavigateUrlFormatString="~/Forms/FrmEditLink.aspx?LinkNum={0}"
                        Target="_parent" Text="Edit" />
                    <asp:HyperLinkField />
                  
               </Columns>   
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="Red" />
                    <RowStyle BackColor="#EFF3FB" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
            
                </asp:GridView>
                
                
                    <% End If%>
                
                <%--End of Admin Grid--%>
                
                
             </td>
             <td style="width: 123px">
             </td>
         </tr>
                    <tr>
                        <td style="width: 103px" align="left">
                        
                          <% If Session("Access") < 5 Or Session("Access") = Nothing Then%> 
                <asp:GridView ID="GViewIntradayCharts" AutoGenerateColumns="False"  runat="server" CellPadding="4" ForeColor="#333333"
                    GridLines="None" Width="373px">
                    <RowStyle BackColor="#EFF3FB" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" /><FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:HyperLinkField Target="_blank" HeaderText="Intraday Charts"  DataNavigateUrlFields="url" DataTextField="sitename" />
                    </Columns>
                </asp:GridView>
                    <% End If%>

                
                
                  <% If Session("Access") = 5 Then%> 
                <asp:GridView ID="GViewIntradayCharts_Admin" AutoGenerateColumns="False"  runat="server" CellPadding="4" ForeColor="#333333"
                    GridLines="None" Width="373px" Visible="False">
                    <RowStyle BackColor="#EFF3FB" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" /><FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:HyperLinkField Target="_blank" HeaderText="Intraday Charts"  DataNavigateUrlFields="url" DataTextField="sitename" />
                   
                    <asp:HyperLinkField DataNavigateUrlFields="LinkNum" DataNavigateUrlFormatString="~/Forms/FrmEditLink.aspx?LinkNum={0}"
                        Target="_parent" Text="Edit" />
                    <asp:HyperLinkField />
                    
                    </Columns>
                </asp:GridView>
                    <% End If%>
                
                
                
                
                        </td>
                        <td style="width: 123px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 103px" align="left">
                        
            <% If Session("Access") < 5 Or Session("Access") = Nothing Then%>              
                        
            <asp:GridView ID="GViewStockMkt" AutoGenerateColumns="False"  runat="server" 
            CellPadding="4" ForeColor="#333333"
                    GridLines="None" Width="373px">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:HyperLinkField Target="_blank" HeaderText="Stock Market"  
                    DataNavigateUrlFields="url" DataTextField="sitename" />
                </Columns>
                <RowStyle BackColor="#EFF3FB" />
                <EditRowStyle BackColor="#2461BF" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
            <% End If%>
            
            
            <% If Session("Access") = 5 Then%> 
            <asp:GridView ID="GViewStockMkt_Admin" AutoGenerateColumns="False"  runat="server" 
            CellPadding="4" ForeColor="#333333"
                    GridLines="None" Width="373px" Visible="False">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:HyperLinkField Target="_blank" HeaderText="Stock Market"  
                    DataNavigateUrlFields="url" DataTextField="sitename" />
                    
                     <asp:HyperLinkField DataNavigateUrlFields="LinkNum" DataNavigateUrlFormatString="~/Forms/FrmEditLink.aspx?LinkNum={0}"
                        Target="_parent" Text="Edit" />
                    <asp:HyperLinkField />
                    
                </Columns>
                <RowStyle BackColor="#EFF3FB" />
                <EditRowStyle BackColor="#2461BF" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
            <% End If%>
            
            
            
                        </td>
                        <td style="width: 123px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 103px; height: 183px;" align="left">
                        
                        
                  <% If Session("Access") < 5 Or Session("Access") = Nothing Then%>            
                <asp:GridView ID="GViewOther" AutoGenerateColumns="False"  runat="server" 
                            CellPadding="4" ForeColor="#333333"
                    GridLines="None" Width="373px">
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:HyperLinkField Target="_blank" HeaderText="Other"  
                    DataNavigateUrlFields="url" DataTextField="sitename" />
                    </Columns>
                    <RowStyle BackColor="#EFF3FB" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                 <% End If%>
                
                
                 <% If Session("Access") = 5 Then%>
                <asp:GridView ID="GViewOther_Admin" AutoGenerateColumns="False"  runat="server" 
                            CellPadding="4" ForeColor="#333333"
                    GridLines="None" Width="373px" Visible="False">
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:HyperLinkField Target="_blank" HeaderText="Other"  
                    DataNavigateUrlFields="url" DataTextField="sitename" />
                    
                     <asp:HyperLinkField DataNavigateUrlFields="LinkNum" DataNavigateUrlFormatString="~/Forms/FrmEditLink.aspx?LinkNum={0}"
                        Target="_parent" Text="Edit" />
                    <asp:HyperLinkField />
                    
                    </Columns>
                    <RowStyle BackColor="#EFF3FB" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                 <% End If%>
                
                
                
                
                        </td>
                        <td style="width: 123px; height: 183px;">
                        </td>
                    </tr>
         <tr>
             <td align="left" style="width: 103px;">
             </td>
             <td style="width: 123px; height: 10px">
             </td>
         </tr>
                
   </table>
    
    <%--End Of Grid Table   --%>
    
    <table id="TblAddLink" runat="server" style="width: 508px" visible="false">
        <tr>
            <td style="width: 174px"> 
            </td>
            <td style="width: 100px">
            </td>
        </tr>
        <tr>
            <td colspan="2" align="left">
                <strong>
                To suggest a link to be added to these pages, fill in the following form. It will
                be checked before being added, and itd be nice if we could have a link back too.</strong></td>
        </tr>
        <tr>
            <td colspan="2">
            </td>
        </tr>
        <tr>
            <td colspan="2">
            </td>
        </tr>
        <tr>
            <td style="width: 174px" class="TDFONT" align="right">
                Site Name
            </td>
            <td style="width: 100px" align="left">
                <asp:TextBox ID="TxtsiteName" runat="server"  CssClass=TEXTBOX></asp:TextBox></td>
        </tr>
        <tr>
            <td style="width: 174px" align="right" class="TDFONT">
                URL</td>
            <td style="width: 100px" align="left">
                <asp:TextBox ID="TxtURL" runat="server"  CssClass=TEXTBOX ForeColor="Blue"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="width: 174px" align="right"  class="TDFONT">
                Place Link in Section</td>
            <td style="width: 100px">
                <asp:DropDownList ID="DDLLinkSection" runat="server" Width="157px">
                <asp:ListItem Value="0">  -   Please Select   -</asp:ListItem>
                    <asp:ListItem Value="1">Indices Forex</asp:ListItem>
                    <asp:ListItem Value="2">Intraday Charts</asp:ListItem>
                     <asp:ListItem Value="3">Stock Market</asp:ListItem>
                      <asp:ListItem Value="4">Other</asp:ListItem>
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td style="width: 174px"  class="TDFONT" align="right">
                &nbsp; &nbsp; &nbsp;
                Description &nbsp;</td>
            <td style="width: 100px">
                <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="width: 174px">
            </td>
            <td style="width: 100px">
            </td>
        </tr>
        <tr>
            <td style="width: 174px">
            </td>
            <td style="width: 100px">
                <asp:Button ID="BttnSend" runat="server" Text="Send" CssClass="BUTTON" Width="69px"/></td>
        </tr>
        <tr>
            <td style="width: 174px">
            </td>
            <td style="width: 100px">
            </td>
        </tr>
    </table>
</asp:Content>

