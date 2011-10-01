<%@ Page Language="VB" AspCompat="true" MasterPageFile="~/Forms/ChartMaster.master" AutoEventWireup="false" CodeFile="FrmChart.aspx.vb" Inherits="Forms_FrmChart" title="Scalper - Trader Performance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<% 
	
    'TLogo = "No Logo"
    Dim name
    name = Request.QueryString("name") + ".aspx"
    
    'Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim lDataAccess As ClsDatabaseAccess
    Dim objProviderFact As ClsProviderFactory
            
    Dim rd As System.Data.OleDb.OleDbDataReader
    lDataAccess = New ClsDatabaseAccess
    lDataAccess.ConnectToDatabase()

    objProviderFact = New ClsProviderFactory

    lDataAccess.objCommand = objProviderFact.GetCommandType

    lDataAccess.objCommand.Connection = lDataAccess.objConnection

    lDataAccess.objCommand.CommandText = "Select * from PageRef where Trader = '" & Request.QueryString("name") & "'"
    

    rd = lDataAccess.objCommand.ExecuteReader()
    
    Dim Trader, TName, TAddress, TContact, TEmail, TDesc, TLogo
    
    If rd.Read Then

        Trader = rd(1)
        TName = rd(6)
        TAddress = rd(7)
        TContact = rd(8)
        TEmail = rd(9)
        TDesc = rd(10)
        TLogo = rd(11)

    End If

    lDataAccess.DisconnectFromDatabase()
    lDataAccess = Nothing
    objProviderFact = Nothing
    'End Sub
%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head><title></title></head>
<body>
<table width="100%">
	<tr>
		<td colspan="2" align="center">
			<h2 style="color:Brown">Trader Details</h2>
		</td>
		<td>
		<hr/>		
		</td>
	</tr>
	<tr>
		<td id="myLogo" width="34%" valign="top" align="center">
		</td>
		<td align="center">
			<table align="left">
				<tr>
					<td style="color:Brown" valign="top" align="right">
					Login ID
					</td>
					<td style="color:Brown" valign="top" align="left">
					<b>: <%=trader%></b>
					</td>
				</tr>
				<tr>
					<td style="color:Brown" valign="top" align="right">
						Trader Name 
					</td>
					<td style="color:Brown" valign="top" align="left">
						<b>: <%=TName%></b>
					</td>
				</tr>
				<tr>
					<td style="color:Brown" valign="top" align="right">
						Contact No. 
					</td>
					<td style="color:Brown" valign="top" align="left">
						<b>: <%=TContact %></b>
					</td>
				</tr>
				<tr>
					<td style="color:Brown" valign="top" align="right">
						E-mail ID 
					</td>
					<td style="color:Brown" valign="top" align="left">
						<b>: <%=TEmail %></b>
					</td>
				</tr>
				<tr>
					<td style="color:Brown" valign="top" align="right">
						Address 
					</td>
					<td style="color:Brown" valign="top" align="left">
						<b>: <%=TAddress%></b>
					</td>
				</tr>
				<tr>
				
				<td style="color:Brown" valign="top" align="right">
						Trader Description 
					</td>
					<td style="color:Brown" valign="top" align="left">
						<b>: <%=TDesc %></b>
					</td>
					
					<%--<td style="color:#6699FF" valign="top" align="right">
						Trader Description 
					</td>
					<td style="color:#3333FF" valign="top" align="left">
						<b>: <%=TDesc %></b>
					</td>--%>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td colspan="3">
			<hr/>
		</td>
	</tr>
	<tr>
		<td colspan="3" align=center>
			<h2 style="color:Brown">Chart Analysis</h2>
		</td>
	</tr>
	<tr>
		<td  colspan="2">
		<div id="DATACELL" style="overflow: auto; width: 750px; height: 600px"></div>
		</td>
	</tr>
</table>
<script type="text/javascript">

document.getElementById("myLogo").innerHTML ="<img border=\"1\" src=\"<%=TLogo%>\" alt=\"No Logo\">";

document.getElementById("DATACELL").innerHTML="<img src=\"<%=name%>\">";
setTimeout("window.location.reload()",180000); 
</script>
</body>
</html>
</asp:Content>

