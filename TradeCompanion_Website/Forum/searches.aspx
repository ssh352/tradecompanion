<%@ Reference Control="~/Forum/CCPager.ascx" %>
<%@ Page language="c#" MasterPageFile="MstForContents.master" Inherits="Forum.searches" CodeFile="~/Forum/searches.cs" %>
<%@ Register TagPrefix="CC" TagName="Header" Src="~/Forum/header.ascx" %>
<%@Register TagPrefix="CC" TagName="Pager" Src="~/Forum/CCPager.ascx"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<%--<html>
  <head>
	<title>SearchRes</title>
	<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0">
	<meta name="GENERATOR" content="YesSoftware CodeCharge v.2.0.5 using 'ASP.NET C#.ccp' build 9/27/2001">
	<meta name="CODE_LANGUAGE" Content="C#">
	<meta http-equiv="pragma" content="no-cache">
<meta http-equiv="expires" content="0"/>
<meta http-equiv="cache-control" content="no-cache"/>
<meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1"/></head>
  <body style="">--%>

 
<CC:Header id="Header" runat="server"/>
	<input type="hidden" id="p_Messages_message_id" runat="server" />
<div align=left>	
<table><tr><td valign="top" >


    <table id="Search_holder" runat="Server" style="width: 100%">
	
	<tr>
      <td style="background-color: #f2f2f2; width: 150px"><font style="font-family: Arial; font-size: 13px; font-weight: bold; color: #0033cc"></font></td>
      <td style="">
	<asp:TextBox
	id="Search_s_topic"
 Columns="35" MaxLength="50"
	runat="server"/>
	
	  </td>
      <td >
	  <asp:Button
		id="Search_search_button"
		Text="Search"
		runat="server"/>
	</td>
    </tr>
	</table>

</td></tr></table><table><tr><td valign="top" >


	<table   id="Messages_holder" runat="Server" style=""  border="0" cellspacing="1" cellpadding="3" width="100%"">
	<tr><td colspan="6"
         bgcolor="#c2c2c2"
><font face="arial"  style="font:bold" color="#0033cc"><asp:label id="MessagesForm_Title" runat="server">Messages</asp:label></font></td></tr>
<tr>
<td style="" bgcolor="#D7D7FF">
<asp:Label id="Messages_Column_icon" Text=""   face="arial" size="2" style="font:bold" color="#0033cc" runat="server"/></td>
	
<td style="" bgcolor="#D7D7FF">
<asp:LinkButton id="Messages_Column_topic" Text="Topic" CommandArgument="m.topic" onClick="Messages_SortChange" size="2" style="font:bold" color="#0033cc" runat="server"/></td>
	
<td style="" bgcolor="#D7D7FF">
<asp:LinkButton id="Messages_Column_author" Text="Author" CommandArgument="m.author" onClick="Messages_SortChange"  size="2" style="font:bold" color="#0033cc" runat="server"/></td>
	
<td style="" bgcolor="#D7D7FF">
<asp:LinkButton id="Messages_Column_date_entered" Text="Date Entered" CommandArgument="m.date_entered" onClick="Messages_SortChange" size="2" style="font:bold" color="#0033cc" runat="server"/></td>
	
</tr><tr id="Messages_no_records" runat="server"><td style="" colspan="6"><font size="2">No records</font></td></tr>
	<tr><td><asp:Repeater id="Messages_Repeater" onitemdatabound="Messages_Repeater_ItemDataBound" runat="server">
	<HeaderTemplate>
	</td></tr>
	</HeaderTemplate>
	<ItemTemplate>

	<tr>

<td style="">

 <asp:Label id="Messages_icon" size="2" runat="server">  </asp:Label>&nbsp;
</td>
<td style="">

<asp:HyperLink id="Messages_topic" NavigateUrl='<%# "viewthread.aspx"+"?"+"mid="+Server.UrlEncode(DataBinder.Eval(Container.DataItem, "m_message_id").ToString()) +"&" +""%>'  size="2" runat="server"> <%#DataBinder.Eval(Container.DataItem, "m_topic") %> </asp:HyperLink>&nbsp;
</td>
<td style="">

 <asp:Label id="Messages_author"  size="2" runat="server">  <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "m_author").ToString()) %> </asp:Label>&nbsp;
</td>
<td style="">

 
<input type="hidden" id="Messages_smiley_id" runat="server" value=<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "m_smiley_id").ToString()) %>>
 
<input type="hidden" id="Messages_message_id" runat="server" value=<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "m_message_id").ToString()) %>>
 <asp:Label id="Messages_date_entered"  size="2" runat="server">  <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "m_date_entered").ToString().Replace('T', ' ')) %> </asp:Label>&nbsp;
</td></tr>
</ItemTemplate>

	<FooterTemplate>
	<tr><td>
	</FooterTemplate>
	</asp:Repeater></td></tr>

    <tr><td
        style= "" bgcolor="#f2f2f2"
        colspan=6>


<font  size="2" style="font:bold" color="#0033cc"">
<CC:Pager id="Messages_Pager" 
	 size="2" style="font:bold" color="#0033cc""
	ShowFirst=False
	showLast=False
	showprev=True
	shownext=True
	ShowFirstCaption=""
	showLastCaption=""
	showtotal="False"
	showtotalstring="of"
	shownextCaption="Next"
	showprevCaption="Previous"
	PagerStyle="1"
	NumberOfPages="10"
	runat="server"/>
</font></td></tr>
	</table>


</td>
    </tr></table>


</div>
<%--</body>
</html>--%>

</asp:Content>