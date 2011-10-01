Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb

Partial Class FrmHLink
    Inherits System.Web.UI.Page

    Private lDataAccess As ClsDatabaseAccess
    Private objProviderFact As ClsProviderFactory

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim LinkId As Integer
        LinkId = Request.QueryString("LinkNum")

        Dim rd As OleDbDataReader

        lDataAccess = New ClsDatabaseAccess
        lDataAccess.ConnectToDatabase()

        objProviderFact = New ClsProviderFactory

        lDataAccess.objCommand = objProviderFact.GetCommandType

        lDataAccess.objCommand.Connection = lDataAccess.objConnection

        '        lDataAccess.objCommand.CommandText = "Select Hits from Links where LinkNum = " & LinkId 

        lDataAccess.objCommand.CommandText = "Update Links SET hits = hits + 1 where LinkNum = " & LinkId

        rd = lDataAccess.objCommand.ExecuteReader()

        lDataAccess.DisconnectFromDatabase()
        lDataAccess = Nothing
        objProviderFact = Nothing

        '********  Open Link *****************************************************

        lDataAccess = New ClsDatabaseAccess
        lDataAccess.ConnectToDatabase()

        objProviderFact = New ClsProviderFactory

        lDataAccess.objCommand = objProviderFact.GetCommandType

        lDataAccess.objCommand.Connection = lDataAccess.objConnection

        lDataAccess.objCommand.CommandText = "Select URL from Links where LinkNum = " & LinkId

        rd = lDataAccess.objCommand.ExecuteReader()

        Dim ab As String

        If rd.Read Then

            ab = rd(0)

        End If

        'TextBox1.Text = ab

        HiddenField1.Value = ab

        lDataAccess.DisconnectFromDatabase()


        '*********************************************************************

        Response.Write("<script language='javascript'>window.close('../FrmHLink.aspx');</script>")

        ' Response.Write("<script language='javascript'>window.open ('http://www.yahoo.com', target='_blank', 'height=400, width=350')</script>")

        '  Response.Write("<script language='javascript'>window.open (ab, target='_blank', 'height=400, width=350')</script>")


    End Sub

End Class
