Imports System.IO
Imports System.Data
Partial Class Proccess
    Inherits System.Web.UI.Page
    Dim Trader As String
    Dim oSenderID As String
    Dim oSymbol As String
    Dim SDate As String
    Dim EDate As String
    Dim Desc As String
    Dim Logo As String
    Dim TName As String
    Dim Address As String
    Dim Contact As String
    Dim Email As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim strTemp() As String = Request.RawUrl.Split(";")

        Dim i As Integer
        For i = 1 To strTemp.Length - 1
            Dim strsplt() As String = strTemp(i).Split("=")
            If strsplt(0) = "QSTrader" Then
                Trader = strsplt(1)
                Trader = Trader.Replace("%20", " ")
            ElseIf strsplt(0) = "QSSenderID" Then
                oSenderID = strsplt(1)
                oSenderID = oSenderID.Replace("%20", ",")
            ElseIf strsplt(0) = "QSSymbols" Then
                oSymbol = strsplt(1)
                oSymbol = oSymbol.Replace("%20", ",")
            ElseIf strsplt(0) = "QSstDate" Then
                SDate = strsplt(1)
                SDate = SDate.Replace("%20", " ")
            ElseIf strsplt(0) = "QSedDate" Then
                EDate = strsplt(1)
                EDate = EDate.Replace("%20", " ")
            ElseIf strsplt(0) = "QSDescription" Then
                Desc = strsplt(1)
                Desc = Desc.Replace("%20", " ")
            ElseIf strsplt(0) = "QSLogo" Then
                Logo = strsplt(1)
                Logo = Logo.Replace("%20", " ")
            ElseIf strsplt(0) = "QSTraderName" Then
                TName = strsplt(1)
                TName = TName.Replace("%20", " ")
            ElseIf strsplt(0) = "QSContact" Then
                Contact = strsplt(1)
                Contact = Contact.Replace("%20", " ")
            ElseIf strsplt(0) = "QSAddress" Then
                Address = strsplt(1)
                Address = Address.Replace("%20", " ")
            ElseIf strsplt(0) = "QSEmail" Then
                Email = strsplt(1)
                Email = Email.Replace("%20", " ")
            End If
        Next
        Dim FileContents(9) As String
        'FileContents(0) = "<%@ Page Language=""VB"" CodeFile=""" + Trader + ".aspx.vb"" Inherits=""" + Trader + """ %>"
        FileContents(0) = "<%@ Page Language=""VB"" CodeFile=""" + Trader + ".aspx.vb"" Inherits=""" + "TCUser" + """ %>"
        FileContents(1) = "<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">"
        FileContents(2) = "<html xmlns=""http://www.w3.org/1999/xhtml"" >"
        FileContents(3) = "<head runat=""server"">"
        FileContents(4) = "    <title></title>"
        FileContents(5) = "</head>"
        FileContents(6) = "<body>"
        FileContents(7) = "</body>"
        FileContents(8) = "</html>"

        ' Change By RG Infotech

        'File.WriteAllLines("c:\inetpub\wwwroot_test\forms\UserPage\" + Trader + ".aspx", FileContents)

        File.WriteAllLines(ConfigurationSettings.AppSettings("FormPath") + "\UserPage\" + Trader + ".aspx", FileContents)


        Dim vbContent(120) As String
        vbContent(0) = "Imports ChartDirector"
        vbContent(1) = "Imports WSScalper"
        vbContent(2) = "Imports System.IO"
        vbContent(3) = "Imports System.Data"
        vbContent(4) = "Imports System.Data.OleDb"
        vbContent(5) = "Imports System.Web.UI.Page"
        vbContent(6) = "Partial Class TCUser"
        'vbContent(6) = "Partial Class " + Trader
        vbContent(7) = "        Inherits System.Web.UI.Page"
        vbContent(8) = "    Dim Trader As String"
        vbContent(9) = "    Dim SenderID As String"
        vbContent(10) = "    Dim Symbol As String"
        vbContent(11) = "    Dim stDate As String"
        vbContent(12) = "    Dim edDate As String"
        vbContent(13) = "    Protected Sub Page_load(ByVal sender As Object, ByVal e As EventArgs)"

        ' Change By RG Infotech
        'vbContent(14) = "       Dim conString As String = ""provider=Microsoft.Jet.OLEDB.4.0; Data source=c:\inetpub\wwwroot_test\app_data\scalper.mdb;"""

        'Dim conString As String = ConfigurationSettings.AppSettings("DatabaseConnectionString")
        vbContent(14) = "       Dim conString As String = ConfigurationSettings.AppSettings(""DatabaseConnectionString"")    "

        vbContent(15) = "       Dim Sql As String"
        vbContent(16) = "       Dim conn As OleDb.OleDbConnection"
        vbContent(17) = "       Dim da As OleDbDataAdapter"
        vbContent(18) = "       Dim ds As New System.Data.DataSet"
        vbContent(19) = "       Dim WSQuery As String"
        vbContent(20) = "       Dim WSS As WebServicesScalper"
        vbContent(21) = "       Sql = ""select * from pageref where trader='" + Trader + "'"""
        vbContent(22) = "       conn = New OleDb.OleDbConnection(conString)"
        vbContent(23) = "       conn.Open()"
        vbContent(24) = "       da = New Data.OleDb.OleDbDataAdapter(Sql, conn)"
        vbContent(25) = "       da.Fill(ds)"
        vbContent(26) = "       conn.Close()"
        vbContent(27) = "       Trader = ds.Tables(0).Rows(0).Item(1).ToString()"
        vbContent(28) = "       SenderID = ds.Tables(0).Rows(0).Item(2).ToString()"
        vbContent(29) = "       Symbol = ds.Tables(0).Rows(0).Item(3).ToString()"
        vbContent(30) = "       stDate = ds.Tables(0).Rows(0).Item(4).ToString()"
        vbContent(31) = "       edDate = ds.Tables(0).Rows(0).Item(5).ToString()"
        vbContent(32) = "       stDate = Replace(stDate, ""/"", ""-"")"
        vbContent(33) = "       edDate = Replace(edDate, ""/"", ""-"")"
        vbContent(34) = "       'prepare query for web service"
        vbContent(35) = "       WSQuery = ""Select PIPS, DATEID, USDMARKETPRICE, AMOUNT, ACTIONS, REMAINING from PLTRADE where TCID='"" + Trader + ""' and """
        vbContent(36) = "       Dim SID() As String"
        vbContent(37) = "       Dim i As Integer"
        vbContent(38) = "       If InStr(SenderID, "","") <> 0 Then"
        vbContent(39) = "           SID = Split(SenderID, "","", -1, 1)"
        vbContent(40) = "           SenderID = """""
        vbContent(41) = "           For i = 0 To arrLength(SID) - 1"
        vbContent(42) = "               SenderID = SenderID + ""'"" + SID(i) + ""'"""
        vbContent(43) = "               If i < arrLength(SID) - 1 Then"
        vbContent(44) = "                   SenderID = SenderID + "","""
        vbContent(45) = "               End If"
        vbContent(46) = "           Next"
        vbContent(47) = "       Else"
        vbContent(48) = "           SenderID = ""'"" + SenderID + ""'"""
        vbContent(49) = "       End If"
        vbContent(50) = "       WSQuery = WSQuery + "" SENDERID in ("" + SenderID + "") and"""
        vbContent(51) = "       If InStr(Symbol, "","") <> 0 Then"
        vbContent(52) = "           SID = Split(Symbol, "","", -1, 1)"
        vbContent(53) = "           Symbol = """""
        vbContent(54) = "           For i = 0 To arrLength(SID) - 1"
        vbContent(55) = "               Symbol = Symbol + "" '"" + SID(i) + ""'"""
        vbContent(56) = "               If i < arrLength(SID) - 1 Then"
        vbContent(57) = "                   Symbol = Symbol + "","""
        vbContent(58) = "               End If"
        vbContent(59) = "           Next"
        vbContent(60) = "       Else"
        vbContent(61) = "           Symbol = "" '"" + Symbol + ""'"""
        vbContent(62) = "       End If"
        vbContent(63) = "       WSQuery = WSQuery + "" symbol in ("" + Symbol + "") and """
        vbContent(64) = "       WSQuery = WSQuery + "" SERVERDATETIME>="""
        vbContent(65) = "       WSS = New WebServicesScalper()"
        vbContent(66) = "       ds = WSS.GetGraphData(WSQuery, stDate, edDate)"
        vbContent(67) = "       Dim rsCount As Integer = ds.Tables(0).Rows.Count"
        vbContent(68) = "       Dim data(1) As Double"
        vbContent(69) = "       Dim labels(1) As Date"
        vbContent(70) = "       If (rsCount > 0) Then"
        vbContent(71) = "           For i = 0 To rsCount - 1"
        vbContent(72) = "               ReDim Preserve Data(i)"
        vbContent(73) = "               ReDim Preserve labels(i)"
        vbContent(74) = "               If i = 0 Then"
        vbContent(75) = "               Data(i) = Decimal.Round(ds.Tables(0).Rows(i).Item(0), 2)"
        vbContent(76) = "               ElseIf Decimal.Round(ds.Tables(0).Rows(i).Item(0), 2) < -5.5 Then"
        vbContent(77) = "               Data(i) = Data(i - 1) - 5.5"
        vbContent(78) = "               Else"
        vbContent(79) = "               Data(i) = Decimal.Round(ds.Tables(0).Rows(i).Item(0), 2) + Data(i - 1)"
        vbContent(80) = "               End If"
        vbContent(81) = "               labels(i) = DateTime.FromOADate(ds.Tables(0).Rows(i).Item(1))"
        vbContent(82) = "           Next"
        vbContent(83) = "           Dim c As XYChart = New XYChart(900, 700)"
        vbContent(84) = "           c.addTitle(""CHART ANALYSIS FOR PROFIT & LOSS"", ""arialbd.ttf"")"
        vbContent(85) = "           c.setPlotArea(70, 20, 720, 520)"
        vbContent(86) = "           Dim layer As Layer = c.addLineLayer2() 'c.addBarLayer2()"
        vbContent(87) = "           layer.addDataSet(Data, layer.yZoneColor(0, System.Drawing.ColorTranslator.ToWin32(System.Drawing.Color.DarkBlue), System.Drawing.ColorTranslator.ToWin32(System.Drawing.Color.DarkGreen)))"
        vbContent(88) = "           layer.setAggregateLabelStyle(""arialbd.ttf"", 8, layer.yZoneColor(0, &HCC3300, &H3333FF))"
        vbContent(89) = "           c.xAxis().setLabels(labels).setFontStyle(""arialbd.ttf"")"
        vbContent(90) = "           c.xAxis().setLabels(labels).setFontAngle(30)"
        vbContent(91) = "           c.setYAxisOnRight(False)"
        vbContent(92) = "           c.yAxis().setLabelStyle(""arialbd.ttf"")"
        vbContent(93) = "           c.yAxis().setTitle(""REALISED PIPS IN USD"")"
        vbContent(95) = "           c.xAxis().setTitle(""Date"")"
        vbContent(96) = "           c.yAxis().addZone(0, 99999999, &HCCCCFF)"
        vbContent(97) = "           c.yAxis().addZone(-99999999, 0, &HFFCCCC)"
        vbContent(98) = "           Response.ContentType = ""image/png"""
        vbContent(99) = "           Response.BinaryWrite(c.makeChart2(0)) 'PNG-0,GIF-1,JPG-2,WMP-3,BMP-4"
        vbContent(100) = "           Response.End()"
        vbContent(101) = "       Else"
        vbContent(102) = "           Response.Write(""<h1>No Data Available to Create Chart.</h1>"")"
        vbContent(103) = "       End If"
        vbContent(104) = "   End Sub"
        vbContent(105) = "   Protected Function arrLength(ByVal arrlen() As String) As Integer"
        vbContent(106) = "       Dim ItemCount As Integer = 0"
        vbContent(107) = "       Dim ItemIndex As Integer"
        vbContent(108) = "       For ItemIndex = 0 To UBound(arrlen)"
        vbContent(109) = "           If Not (arrlen(ItemIndex)) = Nothing Then"
        vbContent(110) = "               ItemCount = ItemCount + 1"
        vbContent(111) = "           End If"
        vbContent(112) = "       Next"
        vbContent(113) = "       arrLength = ItemCount"
        vbContent(114) = "   End Function"
        vbContent(115) = "End Class"

        'File.WriteAllLines("C:\inetpub\wwwroot_test\forms\UserPage\" + Trader + ".aspx.vb", vbContent)

        'Change By RG Infotech
        File.WriteAllLines(ConfigurationSettings.AppSettings("FormPath") + "\UserPage\" + Trader + ".aspx.vb", vbContent)

        UpdateDB()
        Response.Redirect("trader.aspx")
    End Sub

    Protected Sub UpdateDB()
        Dim sql As String = "select * from pageref where trader='" + Trader + " '"
        Dim conn As OleDb.OleDbConnection

        ' Change By RG Infotech
        'Dim conString As String = "provider=Microsoft.Jet.OLEDB.4.0; Data source=c:\inetpub\wwwroot_test\app_data\scalper.mdb;"

        Dim conString As String = ConfigurationSettings.AppSettings("DatabaseConnectionString")

        Dim ds As New DataSet
        conn = New OleDb.OleDbConnection(conString)
        conn.Open()
        Dim da As New Data.OleDb.OleDbDataAdapter(sql, conn)
        da.Fill(ds)
        If ds.Tables(0).Rows.Count > 0 Then
            UpdatePage()
        Else
            InsertPage()
        End If
    End Sub

    Sub InsertPage()

        Dim sql As String = "INSERT INTO PageRef (Trader,SenderID,Symbol,StartDate,EndDate,TraderName,Address,Contact,Email,Description,Logo) VALUES ('" + Trader + "','" + oSenderID + "','" + oSymbol + "','" + SDate + "','" + EDate + "','" + TName + "','" + Address + "','" + Contact + "','" + Email + "','" + Desc + "','" + Logo + "')"

        'Dim conString As String = "provider=Microsoft.Jet.OLEDB.4.0; Data source=c:\inetpub\wwwroot_test\app_data\scalper.mdb;"

        Dim conString As String = ConfigurationSettings.AppSettings("DatabaseConnectionString")

        Dim conn As OleDb.OleDbConnection = New OleDb.OleDbConnection(conString)
        Dim DBInsert As New OleDb.OleDbCommand()
        DBInsert.CommandText = sql
        DBInsert.Connection = conn
        DBInsert.Connection.Open()
        DBInsert.ExecuteNonQuery()
        DBInsert.Connection.Close()
    End Sub

    Sub UpdatePage()

        Dim sql As String = "UPDATE PageRef SET Trader='" + Trader + "',SenderID='" + oSenderID + "',Symbol='" + oSymbol + "',StartDate='" + SDate + "', EndDate='" + EDate + "',TraderName='" + TName + "',Address='" + Address + "',Contact='" + Contact + "',Email='" + Email + "',Description='" + Desc + "',Logo='" + Logo + "' WHERE Trader='" + Trader + "'"

        'Dim conString As String = "provider=Microsoft.Jet.OLEDB.4.0; Data source=c:\inetpub\wwwroot_test\app_data\scalper.mdb;"

        Dim conString As String = ConfigurationSettings.AppSettings("DatabaseConnectionString")

        Dim conn As OleDb.OleDbConnection = New OleDb.OleDbConnection(conString)
        Dim DBUpdate As New OleDb.OleDbCommand()
        DBUpdate.CommandText = sql
        DBUpdate.Connection = conn
        DBUpdate.Connection.Open()
        DBUpdate.ExecuteNonQuery()
        DBUpdate.Connection.Close()
    End Sub
End Class
