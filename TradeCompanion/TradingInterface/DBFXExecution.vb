Imports DBFX
Imports DBFX.DBFXClient
Imports com.scalper.fix.driver.client

Public Class DBFXExecution
    Implements IExecution
    Private WithEvents client As DBFX.DBFXFormClient = Nothing
    Private listOfOrders As List(Of FFillOrder) = New List(Of FFillOrder)
    Public Event OrderFilled(ByVal f As FFillOrder, ByVal fdata As Fill) Implements IExecution.OrderFilled
    Public Event OrderPlaced(ByVal f As FFillOrder) Implements IExecution.OrderPlaced
    Public Event MarketDataUpdate(ByVal f As FillMarketData) Implements IExecution.MarketDataUpdate
    Public Event Disconnected(ByVal reason As String) Implements IExecution.Disconnected
    Public Event Connected() Implements IExecution.Connected
    'pwreset
    Public Event Password_reset_response_received(ByVal UserName As String, ByVal UserStatus As Integer, ByVal UserStatusText As String) Implements IExecution.Password_reset_response_received
    Private Symbols As ArrayList
    Public Shared arrAccountID_dbfx As New Hashtable()


    Public Event WriteToLog(ByVal msg As String) Implements IExecution.WriteToLogEvent
    Public Event OpenpositionValue(ByVal openValue As String, ByVal _Instrument As String, ByVal _UserId As String) Implements IExecution.OpenPositionValue
    'Get historical data and feed back to  NST
    Public Event FeedHisDataToNST(ByVal symb As String, ByVal dateTime As Double, ByVal priceOpen As Double, ByVal priceClose As Double, ByVal priceHigh As Double, ByVal priceLow As Double, ByVal type As Double) Implements IExecution.FeedHistoricalDataToNST
    Public Event SymbolStatus(ByVal symbol As String) Implements IExecution.SymbolStatus
    Public Event RepeatOrder(ByVal symbol As String, ByVal quantity As Integer, ByVal side As Integer, ByVal tradeType As String, ByVal timeStamp As String) Implements IExecution.RepeatOrder

    Public Function Logon(ByVal userid As String, ByVal username As String, ByVal password As String, ByVal param4 As String, Optional ByVal param5 As String = "", Optional ByVal param6 As String = "", Optional ByVal param7 As String = "") As Boolean Implements IExecution.Logon
        If (client Is Nothing) Then client = New DBFX.DBFXFormClient
        client.Login(userid, username, password, param4)
        If DBFXConnection.isLoggedIn Then
            If arrAccountID_dbfx Is Nothing Then arrAccountID_dbfx = New Hashtable()
            arrAccountID_dbfx.Add(userid, DBFXAccountInfo.arrAccountID) 'Add user to hashtable 
        End If
        Return DBFXConnection.isLoggedIn
    End Function

    Public Sub Logout() Implements IExecution.Logout
        'Logout the user and remove userid from hashtable
        Try
            arrAccountID_dbfx.Remove(client.logout())
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub PlaceOrder(ByVal symbol As String, ByVal qty As Integer, ByVal side As Integer, ByVal currency As String, ByVal timestamp As String, ByVal tradeType As Integer, ByVal chartIdentifier As Integer, ByVal price As String, ByVal systemName As String, ByVal tsOpenPosition As Double) Implements IExecution.PlaceOrder
        'Do Nothing
    End Sub
    'pwreset
    Public Function ResetPassword(ByVal ExistingPassword As String, ByVal NewPassword As String) As Boolean Implements IExecution.ResetPassword
        'do nothing
        Return False
    End Function

    Public Sub AddnewOrder(ByVal symbol As String, ByVal qty As Integer, ByVal side As Integer, ByVal currency As String, ByVal timestamp As String, ByVal tradeType As Integer, ByVal chartIdentifier As Integer, ByVal accountID As String, ByVal systemName As String, ByVal ordId As String, ByVal tsOpenPosition As Double) Handles client.ADDnewOrder
        Dim neworder As FFillOrder = New FFillOrder(ordId)
        neworder.currency = currency
        neworder.quantity = qty
        neworder.symbol = symbol
        neworder.side = side
        neworder.timestamp = timestamp
        neworder.tradeType = tradeType
        neworder.chartIdentifier = chartIdentifier
        neworder.senderID = client.userId
        neworder.systemName = systemName
        neworder.tsOpenPosition = tsOpenPosition
        listOfOrders.Add(neworder)
        RaiseEvent OrderPlaced(neworder)
    End Sub

    Public Sub PlaceOrder(ByVal symbol As String, ByVal qty As Integer, ByVal side As Integer, ByVal currency As String, ByVal timestamp As String, ByVal tradeType As Integer, ByVal chartIdentifier As Integer, ByVal price As String, ByVal accountID As String, ByVal systemName As String, ByVal tsOpenPosition As Double) Implements IExecution.PlaceOrder
        Dim ordID As String
        ordID = client.PlaceOrder(symbol, qty, side, currency, arrAccountID_dbfx.Item(accountID))
        If ordID <> "" Then
            AddnewOrder(symbol, qty, side, currency, timestamp, tradeType, chartIdentifier, arrAccountID_dbfx.Item(accountID), systemName, ordID, tsOpenPosition)
            'client.ExecutionReport(currency, accountID, ordID)
        End If
    End Sub

    ' TradeCaptureReport() 'Now not in use
    Public Sub TradeCaptureReport(ByVal clOrderID As String) Implements IExecution.TradeCaptureReport
        'client.TradeCaptureReportRequest(clOrderID)
    End Sub

    Public Sub LogonStatusHandler(ByVal sender As Object, ByVal LOGIN_STATUS As Boolean) Handles client.DBFXLogonEvent
        Select Case LOGIN_STATUS
            Case True
                RaiseEvent Connected()
            Case False
                RaiseEvent Disconnected("Logon Failed")
        End Select
    End Sub

    Private Sub MarketDataStatusHandler(ByVal sender As Object, ByVal e As DBFXMarketData) Handles client.DBFXMarketDataEvent
        Dim marketData As FillMarketData = New FillMarketData()
        marketData.BidPrice = e.BidPrice
        marketData.OfferPrice = e.OfferPrice
        marketData.TimeStamp = e.TimeStamp
        marketData.Symbol = e.Symbol
        If (Symbols.Contains(marketData.Symbol) And marketData.BidPrice <> "" And marketData.OfferPrice <> "") Then
            'If marketData.BidPrice <> "" And marketData.OfferPrice <> "" Then
            RaiseEvent MarketDataUpdate(marketData)
        End If
    End Sub

    Private Sub OrderStatusHandler(ByVal sender As Object, ByVal e As DBFXTrades) Handles client.DBFXOrderEvent
        Dim f As FFillOrder = New FFillOrder(e.OrderID)
        Dim wasFilled As Boolean = False
        Dim instr As String = e.Symbol
        Dim exch As String = "FOREX" 'e.ExchangeID
        Dim side As String = IIf(e.Side = 1, "Buy", "Sell")
        Dim strStatus As String = ""
        Dim createAlert As Boolean = False
        Select Case e.Status
            Case "Executed"
                strStatus = strStatus + "Filled"
                wasFilled = True
                createAlert = True
              Case Else
                strStatus = strStatus + "Rejected( " + e.Status + " )"
                createAlert = True
        End Select
        Dim i As Integer = listOfOrders.FindIndex(New Predicate(Of FFillOrder)(AddressOf f.Predicate))
        If i > -1 Then
            If createAlert Then
                listOfOrders.Item(i).Status = wasFilled.ToString
                listOfOrders.Item(i).Message = strStatus
                Dim fdata As Fill = New Fill
                fdata.Exchange = "FOREX"  'e.ExchangeID
                fdata.Symbol = e.Symbol
                fdata.side = e.Side
                fdata.currency = e.Currency
                fdata.orderId = e.OrderID
                fdata.accountId = client.userId
                fdata.systemName = listOfOrders.Item(i).systemName
                fdata.monthyear = listOfOrders.Item(i).chartIdentifier
                If wasFilled Then
                    fdata.Qty = e.Quantity
                    fdata.price = e.Rate
                End If
                fdata.timestamp = e.TimeStamp.ToString
                RaiseEvent OrderFilled(listOfOrders.Item(i), fdata)
            End If
        End If
        client.OpenPosition()
    End Sub

    Public Sub SubscribeMarketData(ByVal symbolsarr() As String) Implements IExecution.SubscribeMarketData
        For Each s As String In symbolsarr
            If (Not Symbols.Contains(s)) Then
                Symbols.Add(s)
            End If
        Next
        client.SubscribeMarketData()
        If (DBFXMarketData.isMDataConnected) Then
            RaiseEvent Connected()
        End If
    End Sub

    Public Sub UnSubscribeMarketData() Implements IExecution.UnSubscribeMarketData
        If DBFXMarketData.isMDataConnected Then
            client.UnSubscribeMarketData()
        End If
    End Sub

    Public Sub WritetoLogWindow(ByVal msg As String) Handles client.WriteToLogWindowEvent
        RaiseEvent WritetoLog(msg)
    End Sub

    Public Sub New()
        Symbols = New ArrayList
    End Sub

    Public Sub AppLogon(ByVal userName As String, ByVal passWord As String) Implements IExecution.AppLogon

    End Sub

    Public Sub HistoricalData(ByVal symbol As String, ByVal fromDate As String, ByVal toDate As String, ByVal type As String) Implements IExecution.HistoricalData
        client.getHistoricalData(symbol, fromDate, toDate, type)
    End Sub

    Public Sub FeedtonstHisData(ByVal symb As String, ByVal dateTime As Double, ByVal priceOpen As Double, ByVal priceClose As Double, ByVal priceHigh As Double, ByVal priceLow As Double, ByVal type As Double) Handles client.WriteToNSTEvent
        RaiseEvent FeedHisDataToNST(symb, dateTime, priceOpen, priceClose, priceHigh, priceLow, type)
    End Sub

    Private Sub DBFXExecution_SymbolStatus(ByVal symbol As String) Handles Me.SymbolStatus

    End Sub

    Private Sub PositionValue(ByVal openValue As String, ByVal _Instrument As String, ByVal _UserId As String) Handles client.openPositionValue
        RaiseEvent OpenpositionValue(openValue, _Instrument, _UserId)
    End Sub

End Class
