<%@ Page Language="VB" MasterPageFile="~/Forms/MstInternal.master" AutoEventWireup="false" CodeFile="Pricing.aspx.vb" Inherits="Forms_Pricing" title="Untitled Page" %>

<script language=vb runat="server">

    Dim show As Object
    
Sub buyNowButton(ByRef productName As String, ByRef thePrice As Double, ByRef itemID As Byte)
	
Response.Write("" & vbCrLf)
Response.Write("<form action=""https://www.paypal.com/cgi-bin/webscr"" method=""post"">" & vbCrLf)
Response.Write("<input type=""hidden"" name=""cmd"" value=""_xclick"">" & vbCrLf)
Response.Write("<input type=""hidden"" name=""business"" value=""sales@scalper.co.uk"">" & vbCrLf)
Response.Write("<input type=""hidden"" name=""item_name"" value=""")


Response.Write(productName)

Response.Write(""">" & vbCrLf)
Response.Write("<input type=""hidden"" name=""item_number"" value=""")


Response.Write(itemID)


Response.Write(""">" & vbCrLf)
Response.Write("<input type=""hidden"" name=""amount"" value=""")


Response.Write(thePrice)


Response.Write(""">" & vbCrLf)
        Response.Write("<input type=""hidden"" name=""return"" value=""pricing.aspx"">" & vbCrLf)
Response.Write("<input type=""hidden"" name=""cancel_return"" value=""pricing.aspx"">" & vbCrLf)
Response.Write("<input type=""hidden"" name=""currency_code"" value=""GBP"">" & vbCrLf)
Response.Write("<input type=""hidden"" name=""lc"" value=""GB"">" & vbCrLf)
Response.Write("<input type=""image"" src=""https://www.paypal.com/en_US/i/btn/x-click-but01.gif"" border=""0"" name=""submit"" alt=""Make payments with PayPal - it's fast, free and secure!"">" & vbCrLf)
Response.Write("</form>")

	
End Sub


Sub showPage()
	
Response.Write("" & vbCrLf)
Response.Write("<table border=""0"" width=""500"">" & vbCrLf)
Response.Write("<tr><td width=""600"" valign=""top"" colspan=""3"">" & vbCrLf)
Response.Write("<font class=""size3"">Pricing and Order Information</font>" & vbCrLf)
Response.Write("</td></tr>" & vbCrLf)
Response.Write("<tr><td width=""600"" valign=""top"" colspan=""3"">" & vbCrLf)
Response.Write("<img src=""img/electron.gif"">&nbsp;" & vbCrLf)
Response.Write("<img src=""img/mastercard.gif"">&nbsp;" & vbCrLf)
Response.Write("<img src=""img/delta.gif"">&nbsp;" & vbCrLf)
Response.Write("<img src=""img/amex.gif"">&nbsp;" & vbCrLf)
Response.Write("<img src=""img/switch.gif"">&nbsp;" & vbCrLf)
Response.Write("<img src=""img/solo.gif"">&nbsp;" & vbCrLf)
Response.Write("</td></tr>" & vbCrLf)
Response.Write("<tr><td width=""300"" valign=""top"">" & vbCrLf)
Response.Write("<font class=""size2""><b>Tradestation&reg; Technologies Inc.</b></font>" & vbCrLf)
Response.Write("</td><td width=""200"" valign=""top"">" & vbCrLf)
Response.Write("<font class=""size2""><b>Single License</b></font>" & vbCrLf)
Response.Write("</td><td width=""100"" valign=""top""> </td></tr> " & vbCrLf)
Response.Write("<tr>" & vbCrLf)
Response.Write("<td width=""300"" valign=""top""><font class=""size2""><b><a href=""products.aspx?show=1&id=13"">TradeStation 2000i</a></b></font></td>" & vbCrLf)
Response.Write("<td width=""200"" valign=""top""><font class=""size2""><b>UK £ 1499.00</b></font></td>" & vbCrLf)
Response.Write("<td width=""100"" valign=""top"">")

	Call buyNowButton("TradeStation 2000i", 1499, 1)
Response.Write("</td>" & vbCrLf)
Response.Write("</tr>" & vbCrLf)
Response.Write("<tr>" & vbCrLf)
Response.Write("<td width=""300"" valign=""top""><font class=""size2""><b><a href=""products.aspx?show=2&id=10"">RadarScreen 2000i</a></b></font></td>" & vbCrLf)
Response.Write("<td width=""200"" valign=""top""><font class=""size2""><b>UK £ 1499.00</b></font></td>" & vbCrLf)
Response.Write("<td width=""100"" valign=""top"">")

	Call buyNowButton("RadarScreen 2000i", 1499, 2)
Response.Write("</td>" & vbCrLf)
Response.Write("</tr>" & vbCrLf)
Response.Write("<tr>" & vbCrLf)
Response.Write("<td width=""300"" valign=""top""><font class=""size2""><b><a href=""products.aspx?show=3&id=3"">ProSuite 2000i</a></b></font></td>" & vbCrLf)
Response.Write("<td width=""200"" valign=""top""><font class=""size2""><b>UK £ 3000.00</b></font></td>" & vbCrLf)
Response.Write("<td width=""100"" valign=""top"">")

	Call buyNowButton("ProSuite 2000i", 3000, 3)
Response.Write("</td>" & vbCrLf)
Response.Write("</tr>" & vbCrLf)
Response.Write("<tr>" & vbCrLf)
Response.Write("<td width=""300"" valign=""top""><font class=""size2""><b><a href=""systems.aspx?show=1"">Scalper High and Lows</a></b></font></td>" & vbCrLf)
Response.Write("<td width=""200"" valign=""top""><font class=""size2""><b>UK  £ 499.00</b></font></td>" & vbCrLf)
Response.Write("<td width=""100"" valign=""top"">")

	Call buyNowButton(">Scalper High and Lows", 499, 4)
Response.Write("</td>" & vbCrLf)
Response.Write("</tr>" & vbCrLf)
Response.Write("<tr>" & vbCrLf)
Response.Write("<td width=""300"" valign=""top""><font class=""size2""><b><a href=""systems.aspx?show=2"">Bond 007</a></b></font></td>" & vbCrLf)
Response.Write("<td width=""200"" valign=""top""><font class=""size2""><b>UK  £ 499.00</b></font></td>" & vbCrLf)
Response.Write("<td width=""100"" valign=""top"">")

	Call buyNowButton("Bond 007", 499, 5)
Response.Write("</td>" & vbCrLf)
Response.Write("</tr>" & vbCrLf)
Response.Write("<tr>" & vbCrLf)
Response.Write("<td width=""300"" valign=""top""><font class=""size2""><b>Scalper Indicators package for intraday Trading and manual + 30 day commentary</b></font></td>" & vbCrLf)
Response.Write("<td width=""200"" valign=""top""><font class=""size2""><b>UK  £ 299.00</b></font></td>" & vbCrLf)
Response.Write("<td width=""100"" valign=""top"">")

	Call buyNowButton("Scalper Indicators package", 299, 6)
Response.Write("</td>" & vbCrLf)
Response.Write("</tr>" & vbCrLf)
Response.Write("<tr>" & vbCrLf)
Response.Write("<td width=""300"" valign=""top""><font class=""size2""><b>Scalper Intraday Trading commentary and support for Day trading</b></font></td>" & vbCrLf)
Response.Write("<td width=""200"" valign=""top""><font class=""size2""><b>UK £ 199.00</b></font></td>" & vbCrLf)
Response.Write("<td width=""100"" valign=""top"">")

	Call buyNowButton("Scalper Intraday Trading commentary and support", 199, 7)
Response.Write("</td>" & vbCrLf)
Response.Write("</tr>" & vbCrLf)
Response.Write("<tr><td width=""600"" valign=""top"" colspan=""3"">" & vbCrLf)
Response.Write("<font class=""size2"">Try it risk-free for 30-days. If you're not 100% satisfied, return your purchase within 30 days and receive a full refund (1)." & vbCrLf)
Response.Write("</font>" & vbCrLf)
Response.Write("</td></tr>" & vbCrLf)
Response.Write("<tr>" & vbCrLf)
Response.Write("<td width=""300"" valign=""top"" height=""30""> </td>" & vbCrLf)
Response.Write("<td width=""200"" valign=""top"" height=""30""> </td>" & vbCrLf)
Response.Write("<td width=""100"" valign=""top"" height=""30""> </td>" & vbCrLf)
Response.Write("</tr>" & vbCrLf)
Response.Write("<tr><td width=""600"" valign=""top"" colspan=""3"">" & vbCrLf)
Response.Write("<font class=""size3"">Annual Service Support Price</font>" & vbCrLf)
Response.Write("</td></tr>" & vbCrLf)
Response.Write("<tr>" & vbCrLf)
Response.Write("<td width=""300"" valign=""top""><font class=""size2""><b>TradeStation 2000i</b></font></td>" & vbCrLf)
Response.Write("<td width=""200"" valign=""top""><font class=""size2""><b>UK  £ 499.00</b></font></td>" & vbCrLf)
Response.Write("<td width=""100"" valign=""top"">")

	Call buyNowButton("TradeStation 2000i Annual Service Support", 499, 8)
	
Response.Write("</td>" & vbCrLf)
Response.Write("</tr>" & vbCrLf)
Response.Write("<tr>" & vbCrLf)
Response.Write("<td width=""300"" valign=""top""><font class=""size2""><b>ProSuite 2000i</b></font></td>" & vbCrLf)
Response.Write("<td width=""200"" valign=""top""><font class=""size2""><b>UK  £ 699.00</b></font></td>" & vbCrLf)
Response.Write("<td width=""100"" valign=""top"">")

	Call buyNowButton("ProSuite 2000i Annual Service Support", 699, 9)
Response.Write("</td>" & vbCrLf)
Response.Write("</tr>" & vbCrLf)
Response.Write("<tr><td width=""600"" valign=""top"" colspan=""3"">" & vbCrLf)
Response.Write("<font class=""size2"">Annual Service Support is available to any registered user of TradeStation Technologies (formerly Omega Research) 2000i client software, regardless of where the software was purchased!" & vbCrLf)
Response.Write("</font>" & vbCrLf)
Response.Write("</td></tr>  " & vbCrLf)
Response.Write("<tr>" & vbCrLf)
Response.Write("<td width=""300"" valign=""top"" height=""30""> </td>" & vbCrLf)
Response.Write("<td width=""200"" valign=""top"" height=""30""> </td>" & vbCrLf)
Response.Write("<td width=""100"" valign=""top"" height=""30""> </td>" & vbCrLf)
Response.Write("</tr>" & vbCrLf)
Response.Write("<tr><td width=""600"" valign=""top"" colspan=""3"">" & vbCrLf)
Response.Write("<font class=""size3"">Additional Services Price</font>" & vbCrLf)
Response.Write("</td></tr>" & vbCrLf)
Response.Write("<tr>" & vbCrLf)
Response.Write("<td width=""300"" valign=""top""><font class=""size2""><b>On-Site Technical Support*</b></font></td>" & vbCrLf)
Response.Write("<td width=""200"" valign=""top""><font class=""size2""><b>UK £ 88/Hr</b></font></td>" & vbCrLf)
Response.Write("<td width=""100"" valign=""top""> </td>" & vbCrLf)
Response.Write("</tr>" & vbCrLf)
Response.Write("<tr>" & vbCrLf)
Response.Write("<td width=""300"" valign=""top""><font class=""size2""><b>On-Site TradeStation Training*</b></font></td>" & vbCrLf)
Response.Write("<td width=""200"" valign=""top""><font class=""size2""><b>UK £ 88/Hr</b></font></td>" & vbCrLf)
Response.Write("<td width=""100"" valign=""top""> </td>" & vbCrLf)
Response.Write("</tr>" & vbCrLf)
Response.Write("<tr>" & vbCrLf)
Response.Write("<td width=""300"" valign=""top""><font class=""size2""><b>Advanced EasyLanguage Support**</b></font></td>" & vbCrLf)
Response.Write("<td width=""200"" valign=""top""><font class=""size2""><b>UK £ 95/Hr</b></font></td>" & vbCrLf)
Response.Write("<td width=""100"" valign=""top""> </td>" & vbCrLf)
Response.Write("</tr>" & vbCrLf)
Response.Write("</table>" & vbCrLf)
Response.Write("<br>" & vbCrLf)
Response.Write("<br><font class=""size2"">" & vbCrLf)
Response.Write("<b>* Advanced booking required.</b> Call Scalper Systems  for availability.<br><br>" & vbCrLf)
Response.Write("<b>** Guideline prices ONLY.</b> Price may vary depending on the complexity of EasyLanguage requirements<br><br>" & vbCrLf)
Response.Write("<B>NOTE</B> All prices are exclusive of V.A.T which will be calculated at paypal.<br><br>" & vbCrLf)
Response.Write("</font>" & vbCrLf)
Response.Write("<br><br>" & vbCrLf)
Response.Write("<p align=""center""><hr>" & vbCrLf)
Response.Write("<font class=""size1"">All prices are FOB and exclude local sales taxes and import duty (if applicable) and are subject to change without notice. European Union customers add VAT at 17.5%." & vbCrLf)
Response.Write("<br><br>" & vbCrLf)
Response.Write("(1) As a convenience, Scalper Systems pays for shipping &amp; handling when you purchase TradeStation Technologies client software. If you cancel within 30-days we will deduct £49.00 to recover our shipping & handling costs.  " & vbCrLf)
Response.Write("</font>" & vbCrLf)
Response.Write("</p>" & vbCrLf)
Response.Write(" " & vbCrLf)
Response.Write("")

	
    End Sub
    
    

</script>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server" Height="110px" Width="125px">
        <table style="width: 486px; height: 115px">
            <tr>
                <td style="width: 100px; height: 21px">
                </td>
                <td style="width: 100px; height: 21px">
                </td>
                <td style="width: 100px; height: 21px">
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td style="width: 100px; height: 21px">
                </td>
                <td style="width: 100px; height: 21px">
                </td>
                <td style="width: 100px; height: 21px">
                <%
'badForm = False
'onPage = 53
show = request.Form.Item("show")
If Len(CStr(show)) = 0 Then
	show = 1
End If
%>


<%Select Case show
	Case 1
		showPage()
End Select
%>
                </td>
            </tr>
        </table>
    </asp:Panel>

</asp:Content>

