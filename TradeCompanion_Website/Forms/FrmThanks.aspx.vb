
'** For Encrypt *****
Imports System.Configuration
Imports System.Web.Security

Imports System.Web.Configuration.WebConfigurationManager

Imports System.Web

Partial Class FrmThanks
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'If Session("UserName") = "" And Session("Access") < 5 Then
        'Response.Write("<script language='javascript'>window.open ('frmLogin.aspx', '_top')</script>")
        'End If

        Response.Expires = 0
        LblUseName.Text = Session("UserName")

        Dim lDataAccess As ClsDatabaseAccess
        Dim objProviderFact As ClsProviderFactory
        Dim DataReader As System.Data.OleDb.OleDbDataReader
        'Dim Access As Integer
        lDataAccess = New ClsDatabaseAccess
        lDataAccess.ConnectToDatabase()
        objProviderFact = New ClsProviderFactory
        lDataAccess.objCommand = objProviderFact.GetCommandType
        lDataAccess.objCommand.Connection = lDataAccess.objConnection
        lDataAccess.objCommand.CommandText = "select Trader, Description from PageRef"
        DataReader = lDataAccess.objCommand.ExecuteReader()
        'If DataReader.Read Then
        Dim sb As New StringBuilder
        Dim sb1 As New StringBuilder
        sb1.Append("<Table  style='font-size: 12px;font-family: Arial; color: #808080' width=100%>")
        sb.Append("<Table style='font-size: 12px;font-family: Arial; color: #808080'>")
        While DataReader.Read

            '****************************************************************************************

            If (Session("UserName").ToString() = DataReader.Item(0).ToString()) Then
                sb1.Append("<tr align=right>")
                sb1.Append("<td align=right>")
                sb1.Append("<a href=Chart.aspx?name=" + DataReader.Item(0) + "><u>My Performance</u></a>")
                sb1.Append("</td>")
                sb1.Append("</tr>")

                ' 2 Row
                Dim StrDesc As String
                StrDesc = DataReader.Item(1)
                StrDesc = StrDesc.Replace("@", " ")
                StrDesc = StrDesc.Replace(",", " ")
                sb.Append("<tr>")
                sb.Append("<td colspan=3>")
                sb.Append(StrDesc)
                sb.Append("</td>")
                sb.Append("</tr>")
            Else
                sb.Append("<tr>")
                sb.Append("<td valign=top><b>")
                sb.Append(DataReader.Item(0) & "    ")
                sb.Append("</b></td>")
                sb.Append("<td>")
                sb.Append("</td>")

                sb.Append("<td align=right>")
                sb.Append("<a href=Chart.aspx?name=" + DataReader.Item(0) + "><u>View</u></a>")
                sb.Append("</td>")
                sb.Append("</tr>")

                ' 2 Row
                Dim StrDesc As String
                StrDesc = DataReader.Item(1)
                StrDesc = StrDesc.Replace("@", " ")
                StrDesc = StrDesc.Replace(",", " ")
                sb.Append("<tr>")
                sb.Append("<td colspan=3>")
                sb.Append(StrDesc)
                sb.Append("</td>")
                sb.Append("</tr>")
            End If

            

            ' Third Row (Blank)
            sb.Append("<tr>")
            sb.Append("<td colspan=3>")
            sb.Append("</td>")
            sb.Append("</tr>")
            '*************************************************************************************
        End While
        sb.Append("</Table>")
        sb1.Append("</Table>")
        p1.InnerHtml = sb1.ToString()
        p2.InnerHtml = sb.ToString()
        lDataAccess.DisconnectFromDatabase()
    End Sub

    '************************** Encrypt data *********************
    Public Sub EncryptConnStr(ByVal protectionProvider As String)

        '---open the web.config file
        'Dim config As Configuration = ConfigurationManager.OpenWebConfiguration(Request.ApplicationPath)

        Dim config As Configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath)

        'Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(Request.ApplicationPath)

        '---indicate the section to protect
        'Dim section As ConfigurationSection = config.Sections("connectionStrings")
        Dim section As ConfigurationSection = config.Sections("appSettings")
        'appSettings


        '---specify the protection provider
        section.SectionInformation.ProtectSection(protectionProvider)

        '---Apple the protection and update
        config.Save()
    End Sub

    Public Sub DecryptConnStr()

        Dim config As Configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath)

        Dim section As ConfigurationSection = config.Sections("connectionStrings")
        section.SectionInformation.UnProtectSection()
        config.Save()

    End Sub

End Class
