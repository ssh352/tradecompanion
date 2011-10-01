'Imports System.io
'Public Class MarketDataExecution
'    Public Event Rejected(ByVal reason As String)
'    Public Event Connected()
'    Public Event MarketDataUpdated()
'    Private lastRefreshed As Integer
'    Public Shared Event PLCalculation(ByVal serverDateTime As DateTime)
'    Dim ah As AlertsHome

'    'Public Shared currentPrice As New Hashtable()
'    'Dim orderStatusTable As Hashtable = New Hashtable()
'    Private WithEvents ex As TradingInterface.IExecution '= New TradingInterface.CurrenexExecution
'    Dim keys As SettingsHome
'    'Declare Auto Function PlaySound Lib "winmm.dll" (ByVal name As String, ByVal hmod As Integer, ByVal flags As Integer) As Integer

'    Declare Function Send Lib "wrsthnk.dll" Alias "SendTrade" (ByVal ticker As String, ByVal trade As Integer, ByVal open As Integer, _
'                            ByVal high As Integer, ByVal low As Integer, ByVal change As Integer, ByVal prevClose As Integer, _
'        ByVal base As Integer, ByVal tvolume As Integer, ByVal cvolume As Integer, _
'        ByVal year As Integer, ByVal month As Integer, ByVal day As Integer, _
'        ByVal hour As Integer, ByVal minute As Integer, ByVal second As Integer, ByRef errorCode As Integer) _
'        As Integer

'    Declare Function SOrder Lib "wrsthnk.dll" Alias "SendOrder" (ByVal ticker As String, _
'                            ByVal ask As Integer, ByVal bid As Integer, _
'       ByVal ase As Integer, ByVal askSize As Integer, _
'       ByVal bidSize As Integer, ByVal year As Integer, _
'       ByVal month As Integer, ByVal day As Integer, _
'       ByVal hour As Integer, ByVal minute As Integer, _
'       ByVal second As Integer, ByRef errorCode As Integer) _
'       As Integer


'    Public Function Logon() As Boolean
'        Dim connected As Boolean = False
'        Try
'            'ex.IP = keys.LoginIPMarketData
'            'ex.Port = keys.LoginPortMarketData
'            connected = ex.Logon(keys.LoginIPMarketData, keys.LoginPortMarketData, keys.LoginSenderMarketData, keys.LoginTargetMarketData)
'            'connected = ex.Logon("scalpercustu1stream", "CNX")
'            'retrieve all available symbols
'            'Dim ah As AlertsHome = New AlertsHome()
'            Dim ds As DataSet = ah.getSymbolMap()

'            Dim count As Integer
'            count = ds.Tables(0).Rows.Count
'            Dim symbols(count) As String

'            Dim i As Integer
'            i = 0
'            For Each r As DataRow In ds.Tables(0).Rows
'                If Not r.RowState = DataRowState.Deleted Then
'                    symbols(i) = r.Item("TradeSymbol")
'                    i = i + 1
'                End If
'            Next r

'            If (connected) Then ex.SubscribeMarketData(symbols)
'        Catch ex As Exception
'            Util.WriteDebugLog(" .... MarketDataExecution Logon ERROR " + ex.Message)
'            connected = False
'        End Try
'        Return connected
'    End Function
'    Public Sub Logout()
'        'ex.Logout()
'        ex.UnSubscribeMarketData()
'        'Select Case keys.ExchangeServer
'        '    Case ExchangeServer.CurrenEx
'        '        ex.Logout()
'        '    Case ExchangeServer.Ariel
'        '        ex.UnSubscribeMarketData()
'        '    Case ExchangeServer.Espeed
'        '        ex.UnSubscribeMarketData()
'        'End Select
'    End Sub

'    Public Sub New(ByVal con As TradingInterface.IExecution)
'        keys = SettingsHome.getInstance()
'        ex = con
'        ah = New AlertsHome()

'        Dim ds As DataSet = ah.getSymbolMap()

'        Dim count As Integer
'        count = ds.Tables(0).Rows.Count
'        Dim symbols(count) As String

'        Dim i As Integer
'        i = 0
'        For Each r As DataRow In ds.Tables(0).Rows
'            If Not r.RowState = DataRowState.Deleted Then
'                symbols(i) = r.Item("TradeSymbol")
'                i = i + 1
'            End If
'        Next r
'        ex.SubscribeMarketData(symbols)
'    End Sub
'    Public Sub New()
'        keys = SettingsHome.getInstance()

'        Select Case keys.ExchangeServer
'            Case ExchangeServer.CurrenEx
'                ex = New TradingInterface.CurrenexExecution
'                'Case ExchangeServer.Ariel
'                'ex = New TradingInterface.ArielExecution
'                'Case ExchangeServer.Espeed
'                'ex = New TradingInterface.EspeedExecution
'        End Select

'        'ex.IP = keys.LoginIPMarketData
'        'ex.Port = keys.LoginPortMarketData

'        ah = New AlertsHome()
'    End Sub

'    Private Sub ex_Connected() Handles ex.Connected
'        Util.WriteDebugLog(" ... MarketDataExecution Connected event raised ")
'        EServerDependents.UpdateMarketDataButtonsForcibly("2")
'        RaiseEvent Connected()
'    End Sub

'    Private Sub ex_Disconnected(ByVal reason As String) Handles ex.Disconnected
'        Util.WriteDebugLog(" ... MarketDataExecution Disconnected event raised ")
'        RaiseEvent Rejected(reason)
'    End Sub

'    Private Sub SendFeed(ByVal f As TradingInterface.FillMarketData)
'        Dim result As Integer

'        Dim str As String
'        Dim i, j, iLength, jLength, base As Integer
'        Dim bid, offer As Integer
'        str = f.BidPrice
'        i = str.IndexOf(".")
'        If i <= 0 Then
'            i = 0
'            iLength = 0
'            bid = CInt(str)
'        Else
'            iLength = str.Length - i - 1
'            bid = CInt(str.Substring(0, i) & str.Substring(i + 1))
'        End If


'        str = f.OfferPrice
'        j = str.IndexOf(".")
'        If j <= 0 Then
'            j = 0
'            jLength = 0
'            offer = CInt(str)
'        Else
'            jLength = str.Length - i - 1
'            offer = CInt(str.Substring(0, i) & str.Substring(i + 1))
'        End If

'        base = 1

'        If iLength > jLength Then
'            For counter As Integer = 1 To iLength Step 1
'                base = base * 10
'            Next counter
'            For counter As Integer = 1 To iLength - jLength Step 1
'                offer = offer * 10
'            Next counter
'        ElseIf iLength <= jLength Then
'            For counter As Integer = 1 To jLength Step 1
'                base = base * 10
'            Next counter
'            For counter As Integer = 1 To jLength - iLength Step 1
'                bid = bid * 10
'            Next counter
'        End If

'        Dim sec As Integer = DateTime.Now.Second
'        Dim min As Integer = DateTime.Now.Minute
'        Dim hr As Integer = DateTime.Now.Hour
'        Dim month As Integer = DateTime.Now.Month
'        Dim year As Integer = DateTime.Now.Year
'        Dim day As Integer = DateTime.Now.Day

'        Dim Result1 As Integer
'        Result1 = SOrder(f.Symbol, offer, bid, base, 0, 0, year, month, day, hr, min, sec, result)
'        If Result1 <> 1 Then
'            Util.WriteDebugLog("Error sending bid/ask data to metaserver error Code:" & result)
'        End If
'    End Sub
'    Private Sub ex_MarketDataUpdate(ByVal f As TradingInterface.FillMarketData) Handles ex.MarketDataUpdate

'        Dim successful As Boolean

'        Dim cal As New PLCal()
'        SendFeed(f)

'        successful = False
'        Try
'            ah.UpdateMarketData(f)
'            successful = True
'        Catch ex As Exception
'            'MsgBox(ex.Message)
'            Util.WriteDebugLog(" ... Market Data Execution  ERROR (" + f.BidPrice + ") " + f.Symbol + " " + f.TimeStamp + " " + ex.Message)
'        End Try
'        Try
'            If successful = True Then
'                If (lastRefreshed <> DateTime.Now.Second) Then RaiseEvent MarketDataUpdated()
'                Dim timestamp As String = f.TimeStamp
'                Dim tradingDate As DateTime
'                tradingDate = EServerDependents.GetDateTime(timestamp) 'Date.Parse(timestamp.Insert(4, " ").Insert(7, " ").Replace("-", " "))
'                Dim dsPLCalc As DataSet = ah.loadPLCal()
'                If ((dsPLCalc.Tables(0).Select("Symbol = '" + f.Symbol + "'").Length > 0) Or _
'(dsPLCalc.Tables(0).Select("BaseSymbol = '" + f.Symbol + "'").Length > 0)) Then
'                    cal.CalculatePIPSMarketData(f)
'                    RaiseEvent PLCalculation(tradingDate)
'                End If
'                lastRefreshed = DateTime.Now.Second
'            End If
'        Catch ex As Exception
'            'Util.WriteDebugLog(" ... Market Data Execution  ERROR (" + f.BidPrice + ") " + f.Symbol + " " + f.TimeStamp + " " + ex.Message)
'        End Try

'    End Sub
'End Class
