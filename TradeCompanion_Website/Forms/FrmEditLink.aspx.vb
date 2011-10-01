
' ****************************  For Admin ***********************************
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb

Partial Class FrmEditLink
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

        lDataAccess.objCommand.CommandText = "Select * from Links where LinkNum = " & LinkId

        rd = lDataAccess.objCommand.ExecuteReader()

        If rd.Read Then

            '  DDLLinkSection.SelectedItem.Value = rd(1)


            If rd(1) = "1" Then
                DDLLinkSection.SelectedItem.Text = "Indices and Forex"
            ElseIf rd(1) = "2" Then
                DDLLinkSection.SelectedItem.Text = "Intraday Charts"
            ElseIf rd(1) = "3" Then
                DDLLinkSection.SelectedItem.Text = "Stock Market"
            ElseIf rd(1) = "4" Then
                DDLLinkSection.SelectedItem.Text = "Other"
            End If

            TxtURL.Text = rd(2)
            TxtsiteName.Text = rd(3)

        End If

        lDataAccess.DisconnectFromDatabase()
        lDataAccess = Nothing
        objProviderFact = Nothing
    End Sub

    
    Protected Sub LBttnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LBttnDelete.Click
        ' delete link
    End Sub

    Protected Sub LBttnDeActive_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LBttnDeActive.Click
        ' set Access = 0
    End Sub

End Class
