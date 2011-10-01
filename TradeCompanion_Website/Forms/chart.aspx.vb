Imports WSScalper
Imports System.IO
Imports System.Data
Imports System.Data.OleDb
Imports System.Web.UI.Page
Partial Class chart
    Inherits Page
    Protected Sub Page_load(ByVal sender As Object, ByVal e As EventArgs)

        Dim sql As String = "select * from pageref where trader='" + Request.QueryString("name") + " '"

        Dim conn As OleDb.OleDbConnection

        ' Change By RG Infotech
        'Dim conString As String = "provider=Microsoft.Jet.OLEDB.4.0; Data source=c:\inetpub\wwwroot_test\App_Data\scalper.mdb;"

        Dim conString As String = ConfigurationSettings.AppSettings("DatabaseConnectionString")

        Dim str As String
        Dim ds As New DataSet
        conn = New OleDb.OleDbConnection(conString)
        conn.Open()
        Dim da As New Data.OleDb.OleDbDataAdapter(sql, conn)
        da.Fill(ds)
        Label1.Text = ": " + ds.Tables(0).Rows(0).Item(1).ToString()
        Label2.Text = ": " + ds.Tables(0).Rows(0).Item(6).ToString()
        Label3.Text = ": " + ds.Tables(0).Rows(0).Item(8).ToString()
        Label4.Text = ": " + ds.Tables(0).Rows(0).Item(9).ToString()
        str = ds.Tables(0).Rows(0).Item(7).ToString()
        str = str.Replace("@", "<br>&nbsp;")
        str = str.Replace(",", " ")
        Label5.Text = ": " + str
        str = ds.Tables(0).Rows(0).Item(10).ToString()
        str = str.Replace("@", "<br>&nbsp;")
        str = str.Replace(",", " ")
        Label6.Text = ": " + str
        LogoImage.ImageUrl = "Logo/" + ds.Tables(0).Rows(0).Item(11).ToString()
        ChartImage.ImageUrl = "UserPage/" + ds.Tables(0).Rows(0).Item(1).ToString() + ".aspx"
        conn.Close()

    End Sub

End Class
