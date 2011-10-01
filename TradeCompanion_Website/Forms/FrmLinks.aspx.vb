
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls

Partial Class FrmLinks
    Inherits System.Web.UI.Page

    Private lDataAccess As ClsDatabaseAccess
    Private objProviderFact As ClsProviderFactory

    Dim EditFlag As Boolean


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.IsPostBack = False Then

            BindGridView("LinkType=1", GViewIndicesForex)
            BindGridView("LinkType=2", GViewIntradayCharts)
            BindGridView("LinkType=3", GViewStockMkt)
            BindGridView("LinkType=4", GViewOther)


            BindGridView("LinkType=1", GViewIndicesForex_Admin)
            BindGridView("LinkType=2", GViewIntradayCharts_Admin)
            BindGridView("LinkType=3", GViewStockMkt_Admin)
            BindGridView("LinkType=4", GViewOther_Admin)

        End If

        'If Session("Access") = 5 Then
        'LBttnViewSuggestLink.Visible = True
        'end If

        'If Session("EditFlag") = True Then
        '    TblAddLink.Visible = True
        '    TblGrid.Visible = False
        'End If

        Dim id As Integer
        id = Request.QueryString("LinkNum")

        If Not id = 0 Then
            TblGrid.Visible = False
            TblAddLink.Visible = True

        End If

        Dim i As Integer = Session("Access")


    End Sub

    ' ****** Function for Bind Grid *******

    'Public Function BindGridView(ByVal argField As String, ByVal argGridView As GridView) As OleDbDataReader

    Public Function BindGridView(ByVal argField As String, ByVal argGridView As GridView)


        Dim rd As OleDbDataReader

        lDataAccess = New ClsDatabaseAccess
        lDataAccess.ConnectToDatabase()

        objProviderFact = New ClsProviderFactory

        lDataAccess.objCommand = objProviderFact.GetCommandType

        lDataAccess.objCommand.Connection = lDataAccess.objConnection

        '        lDataAccess.objCommand.CommandText = "SELECT url,sitename FROM links WHERE Active =1 AND " & argField & " ORDER BY hits DESC "

        lDataAccess.objCommand.CommandText = "SELECT url,sitename,LinkNum FROM links WHERE Active =1 AND " & argField & " ORDER BY hits DESC "

        rd = lDataAccess.objCommand.ExecuteReader()

        argGridView.DataSource = rd
        argGridView.DataBind()

        lDataAccess.DisconnectFromDatabase()
        lDataAccess = Nothing
        objProviderFact = Nothing

    End Function

    Protected Sub LBttnIndicesForex_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LBttnIndicesForex.Click

        TblGrid.Visible = True

        GViewIndicesForex.Visible = True
        GViewIntradayCharts.Visible = False
        GViewStockMkt.Visible = False
        GViewOther.Visible = False

        TblAddLink.Visible = False

    End Sub

    Protected Sub LBttnIntradayCharts_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LBttnIntradayCharts.Click

        TblGrid.Visible = True

        GViewIndicesForex.Visible = False
        GViewIntradayCharts.Visible = True
        GViewStockMkt.Visible = False
        GViewOther.Visible = False

        TblAddLink.Visible = False

    End Sub

    Protected Sub LBttnStockMkt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LBttnStockMkt.Click

        TblGrid.Visible = True

        GViewIndicesForex.Visible = False
        GViewIntradayCharts.Visible = False
        GViewStockMkt.Visible = True
        GViewOther.Visible = False

        TblAddLink.Visible = False

    End Sub

    Protected Sub LBttnOther_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LBttnOther.Click

        TblGrid.Visible = True

        GViewIndicesForex.Visible = False
        GViewIntradayCharts.Visible = False
        GViewStockMkt.Visible = False
        GViewOther.Visible = True

        TblAddLink.Visible = False

    End Sub

    Protected Sub LBttnSuggestLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LBttnSuggestLink.Click

        TblAddLink.Visible = True
        TblGrid.Visible = False

        'GViewIndicesForex.Visible = False
        'GViewIntradayCharts.Visible = False
        'GViewStockMkt.Visible = False
        'GViewOther.Visible = False

    End Sub

    Protected Sub BttnSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BttnSend.Click

        If DDLLinkSection.SelectedItem.Value = 0 Then
            Response.Write("<script language='javascript'>alert ('Please Select Section.');</script>")
            Exit Sub
        End If


        lDataAccess = New ClsDatabaseAccess
        lDataAccess.ConnectToDatabase()

        objProviderFact = New ClsProviderFactory

        lDataAccess.objCommand = objProviderFact.GetCommandType

        lDataAccess.objCommand.Connection = lDataAccess.objConnection

        lDataAccess.objCommand.CommandText = "INSERT INTO Links (linkType,url,siteName,active) VALUES ('" & DDLLinkSection.SelectedItem.Value & "', '" & TxtURL.Text & "' ,'" & TxtsiteName.Text & "',0)"

        lDataAccess.objCommand.ExecuteNonQuery()

        lDataAccess.DisconnectFromDatabase()


    End Sub

    'Protected Sub LBttnViewSuggestLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LBttnViewSuggestLink.Click

    '    'GViewIndicesForex.Columns.Add()
    '    Dim bCol As BoundColumn
    '    Dim HyperCol As HyperLinkField

    '    HyperCol = New HyperLinkField

    '    HyperCol.HeaderText = "EDIT"
    '    HyperCol.DataNavigateUrlFormatString = ""

    '    HyperCol.DataTextField = "Edit"


    '    GViewIndicesForex.Columns.Add(HyperCol)

    'End Sub

    Protected Sub TxtURL_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtURL.TextChanged

    End Sub
End Class
'Dim HyperCol As HyperLinkField
'HyperCol = New HyperLinkField
'HyperCol.HeaderText = "EDIT"
'HyperCol.NavigateUrl = "Frmlinks.aspx"
'HyperCol.Text = "Edit"
'GViewIntradayCharts.Columns.Add(HyperCol)
'Session("EditFlag") = True
