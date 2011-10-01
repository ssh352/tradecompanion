Imports ChartDirector
Imports WSScalper
Imports System.IO
Imports System.Data
Imports System.Data.OleDb
Imports System.Web.UI.Page
Partial Class rahul
        Inherits System.Web.UI.Page
    Dim Trader As String
    Dim SenderID As String
    Dim Symbol As String
    Dim stDate As String
    Dim edDate As String

    Protected Sub Page_load(ByVal sender As Object, ByVal e As EventArgs)

        ' Change By RG Infotech
        'Dim conString As String = "provider=Microsoft.Jet.OLEDB.4.0; Data source=d:/wwwroot_test/app_data/scalper.mdb;"

        Dim conString As String = ConfigurationSettings.AppSettings("DatabaseConnectionString")

        Dim Sql As String
        Dim conn As OleDb.OleDbConnection
        Dim da As OleDbDataAdapter
        Dim ds As New System.Data.DataSet
        Dim WSQuery As String
        Dim WSS As WebServicesScalper
        Sql = "select * from pageref where trader='rahul'"
        conn = New OleDb.OleDbConnection(conString)
        conn.Open()
        da = New Data.OleDb.OleDbDataAdapter(Sql, conn)
        da.Fill(ds)
        conn.Close()
        Trader = ds.Tables(0).Rows(0).Item(1).ToString()
        SenderID = ds.Tables(0).Rows(0).Item(2).ToString()
        Symbol = ds.Tables(0).Rows(0).Item(3).ToString()
        stDate = ds.Tables(0).Rows(0).Item(4).ToString()
        edDate = ds.Tables(0).Rows(0).Item(5).ToString()
        stDate = Replace(stDate, "/", "-")
        edDate = Replace(edDate, "/", "-")
        'prepare query for web service
        WSQuery = "Select PIPS, DATEID, USDMARKETPRICE from PLTRADE where TCID='" + Trader + "' and "
        Dim SID() As String
        Dim i As Integer
        If InStr(SenderID, ",") <> 0 Then
            SID = Split(SenderID, ",", -1, 1)
            SenderID = ""
            For i = 0 To arrLength(SID) - 1
                SenderID = SenderID + "'" + SID(i) + "'"
                If i < arrLength(SID) - 1 Then
                    SenderID = SenderID + ","
                End If
            Next
        Else
            SenderID = "'" + SenderID + "'"
        End If
        WSQuery = WSQuery + " SENDERID in (" + SenderID + ") and"
        If InStr(Symbol, ",") <> 0 Then
            SID = Split(Symbol, ",", -1, 1)
            Symbol = ""
            For i = 0 To arrLength(SID) - 1
                Symbol = Symbol + " '" + SID(i) + "'"
                If i < arrLength(SID) - 1 Then
                    Symbol = Symbol + ","
                End If
            Next
        Else
            Symbol = " '" + Symbol + "'"
        End If
        WSQuery = WSQuery + " symbol in (" + Symbol + ") and "
        WSQuery = WSQuery + " SERVERDATETIME>="
        WSS = New WebServicesScalper()
        ds = WSS.GetGraphData(WSQuery, stDate, edDate)
        Dim rsCount As Integer = ds.Tables(0).Rows.Count
        Dim data(1) As Double
        Dim labels(1) As Date
        If (rsCount > 0) Then
            For i = 0 To rsCount - 1
                ReDim Preserve data(i)
                ReDim Preserve labels(i)
                data(i) = Decimal.Round(ds.Tables(0).Rows(i).Item(0), 2)
                labels(i) = DateTime.FromOADate(ds.Tables(0).Rows(i).Item(1))
            Next
            Dim c As XYChart = New XYChart(900, 700)
            c.addTitle("CHART ANALYSIS FOR PROFIT & LOSS", "arialbd.ttf")
            c.setPlotArea(70, 20, 720, 520)
            Dim layer As Layer = c.addBarLayer2()
            layer.addDataSet(data, layer.yZoneColor(0, &HFF6600, &H6666FF))
            layer.setAggregateLabelStyle("arialbd.ttf", 8, layer.yZoneColor(0, &HCC3300, &H3333FF))
            c.xAxis().setLabels(labels).setFontStyle("arialbd.ttf")
            c.xAxis().setLabels(labels).setFontAngle(30)
            c.setYAxisOnRight(False)
            c.yAxis().setLabelStyle("arialbd.ttf")
            c.yAxis().setTitle("REALISED PIPS IN USD")
            c.xAxis().setTitle("Date")
            c.yAxis().addZone(0, 99999999, &HCCCCFF)
            c.yAxis().addZone(-99999999, 0, &HFFCCCC)
            Response.ContentType = "image/png"
            Response.BinaryWrite(c.makeChart2(0)) 'PNG-0,GIF-1,JPG-2,WMP-3,BMP-4
            Response.End()
        Else
            Response.Write("<h1>No Data Available to Create Chart.</h1>")
        End If
    End Sub
   Protected Function arrLength(ByVal arrlen() As String) As Integer
       Dim ItemCount As Integer = 0
       Dim ItemIndex As Integer
       For ItemIndex = 0 To UBound(arrlen)
           If Not (arrlen(ItemIndex)) = Nothing Then
               ItemCount = ItemCount + 1
           End If
       Next
       arrLength = ItemCount
   End Function
End Class

