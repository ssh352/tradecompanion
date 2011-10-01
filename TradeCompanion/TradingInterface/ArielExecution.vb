Imports com.scalper.fix.driver.client

Public Class ArielExecution
    Implements IExecution
    Private WithEvents client As ArielDriver.ArielClientForm = Nothing

    Private listOfOrders As List(Of FFillOrder) = New List(Of FFillOrder)
    Public Event OrderFilled(ByVal f As FFillOrder, ByVal fdata As Fill) Implements IExecution.OrderFilled
    Public Event OrderPlaced(ByVal f As FFillOrder) Implements IExecution.OrderPlaced
    Public Event MarketDataUpdate(ByVal f As FillMarketData) Implements IExecution.MarketDataUpdate
    Public Event Disconnected(ByVal reason As String) Implements IExecution.Disconnected
    Public Event Connected() Implements IExecution.Connected
    'pwreset
    Public Event Password_reset_response_received(ByVal UserName As String, ByVal UserStatus As Integer, ByVal UserStatusText As String) Implements IExecution.Password_reset_response_received
    'Public Event HeartBeatREceived(ByVal SeqNo As Integer) Implements IExecution.HeartBeatReceived 'HeartBeatCallback
    Private Symbols As ArrayList = New ArrayList()
    Public Event WriteToLogEvent(ByVal msg As String) Implements IExecution.WriteToLogEvent
    'Vm_ Not used
    Public Event FeedHisDataToNST(ByVal symb As String, ByVal dateTime As Double, ByVal priceOpen As Double, ByVal priceClose As Double, ByVal priceHigh As Double, ByVal priceLow As Double, ByVal type As Double) Implements IExecution.FeedHistoricalDataToNST
    Public Event SymbolStatus(ByVal symbol As String) Implements IExecution.SymbolStatus
    Public Event RepeatOrder(ByVal symbol As String, ByVal quantity As Integer, ByVal side As Integer, ByVal tradeType As String, ByVal timeStamp As String) Implements IExecution.RepeatOrder
    Public Event OpenpositionValue(ByVal symbol As String, ByVal _Instrument As String, ByVal _UserId As String) Implements IExecution.OpenPositionValue

    Public Function Logon(ByVal userid As String, ByVal username As String, ByVal password As String, ByVal param4 As String, Optional ByVal param5 As String = "", Optional ByVal param6 As String = "", Optional ByVal param7 As String = "") As Boolean Implements IExecution.Logon
        If (client Is Nothing) Then client = New ArielDriver.ArielClientForm()
        Return client.Login(userid, username, password)
    End Function

    Public Sub Logout() Implements IExecution.Logout
        client.Logout()
    End Sub

    Public Sub PlaceOrder(ByVal symbol As String, ByVal qty As Integer, ByVal side As Integer, ByVal currency As String, ByVal timestamp As String, ByVal tradeType As Integer, ByVal chartIdentifier As Integer, ByVal price As String, ByVal accountID As String, ByVal systemName As String, ByVal tsOpenPosition As Double) Implements IExecution.PlaceOrder
        'Do Nothing
    End Sub
    ''Vm_
    Public Sub HistoricalData(ByVal symbol As String, ByVal fromDate As String, ByVal toDate As String, ByVal type As String) Implements IExecution.HistoricalData
        ''There is no support for historical data from ArielExceution server
    End Sub
    'pwreset
    Public Function ResetPassword(ByVal ExistingPassword As String, ByVal NewPassword As String) As Boolean Implements IExecution.ResetPassword
        'do nothing
        Return False
    End Function

    Public Sub PlaceOrder(ByVal symbol As String, ByVal qty As Integer, ByVal side As Integer, ByVal currency As String, ByVal timestamp As String, ByVal tradeType As Integer, ByVal chartIdentifier As Integer, ByVal price As String, ByVal systemName As String, ByVal tsOpenPosition As Double) Implements IExecution.PlaceOrder
        Dim ordID As String
        ordID = client.PlaceOrder(symbol, qty, side, currency)
        Dim neworder As FFillOrder = New FFillOrder(ordID)
        neworder.currency = currency
        neworder.quantity = qty
        neworder.symbol = symbol
        neworder.side = side
        neworder.timestamp = timestamp
        neworder.tradeType = tradeType
        neworder.chartIdentifier = chartIdentifier
        neworder.senderID = client.SenderID
        neworder.systemName = systemName
        neworder.tsOpenPosition = tsOpenPosition
        listOfOrders.Add(neworder)
        RaiseEvent OrderPlaced(neworder)
    End Sub

    Public Sub TradeCaptureReport(ByVal clOrderID As String) Implements IExecution.TradeCaptureReport
        'client.TradeCaptureReportRequest(clOrderID)
    End Sub
    Public Sub LogonStatusHandler(ByVal sender As Object, ByVal LOGIN_STATUS As Integer) Handles client.ArielLoginResponseEvent
        Select Case LOGIN_STATUS
            Case ArielDriver.LOGIN_STATUS.LOGIN_ACCEPTED
                RaiseEvent Connected()
            Case ArielDriver.LOGIN_STATUS.LOGIN_REJECTED
                RaiseEvent Disconnected("Logon Failed")
            Case ArielDriver.LOGIN_STATUS.LOGIN_TERMINATED
                RaiseEvent Disconnected(Nothing)
        End Select
    End Sub

    Private Sub MarketDataStatusHandler(ByVal sender As Object, ByVal e As ArielDriver.ArielMarketDataStatusEventArgs) Handles client.ArielMarketDataResponseEvent        'Dim marketData As FillMarketData = New FillMarketData()
        Dim marketData As FillMarketData = New FillMarketData()
        marketData.BidPrice = e.BidPrice
        marketData.OfferPrice = e.OfferPrice
        marketData.TimeStamp = e.TimeStamp
        marketData.Symbol = e.Symbol
        If (Symbols.Contains(marketData.Symbol) And marketData.BidPrice <> "" And marketData.OfferPrice <> "") Then
            RaiseEvent MarketDataUpdate(marketData)
        End If
    End Sub

    Private Sub OrderStatusHandler(ByVal sender As Object, ByVal e As ArielDriver.ArielOrderStatusEventArgs) Handles client.ArielOrderResponseEvent
        Dim f As FFillOrder = New FFillOrder(e.ClOrdID)
        Dim wasFilled As Boolean = False
        Dim instr As String = e.Instrument
        Dim exch As String = "FOREX" 'e.ExchangeID
        Dim side As String = IIf(e.Side = 1, "Buy", "Sell")
        Dim strStatus As String = ""
        Dim createAlert As Boolean = False
        Select Case e.OrderStatus
            Case "Order Accepted"
                strStatus = strStatus + "Filled"
                wasFilled = True
                createAlert = True
            Case "Order Cancelled"
                strStatus = strStatus + "Rejected( " + e.OrderStatusLong + " )"
                createAlert = True
        End Select
        Dim i As Integer = listOfOrders.FindIndex(New Predicate(Of FFillOrder)(AddressOf f.Predicate))
        If i > -1 Then
            If createAlert Then
                listOfOrders.Item(i).Status = wasFilled.ToString
                listOfOrders.Item(i).Message = strStatus
                Dim fdata As Fill = New Fill
                fdata.Exchange = "FOREX"  'e.ExchangeID
                fdata.Symbol = e.Instrument
                fdata.side = e.Side
                fdata.currency = e.Currency
                fdata.orderId = e.OrderID
                fdata.accountId = e.Sender
                fdata.systemName = listOfOrders.Item(i).systemName
                fdata.monthyear = listOfOrders.Item(i).chartIdentifier
                If wasFilled Then
                    fdata.Qty = e.FilledQty
                    fdata.price = e.Price
                End If
                fdata.timestamp = e.FillTime.ToString
                RaiseEvent OrderFilled(listOfOrders.Item(i), fdata)
            End If
        End If
    End Sub

    Public Sub SubscribeMarketData(ByVal symbolsarr() As String) Implements IExecution.SubscribeMarketData
        For Each s As String In symbolsarr
            If (Not Symbols.Contains(s)) Then
                Symbols.Add(s)
            End If
        Next
        If (client.SubscribeMarketData()) Then
            RaiseEvent Connected()
        End If

    End Sub

    Public Sub UnSubscribeMarketData() Implements IExecution.UnSubscribeMarketData
        client.UnSubscribeMarketData()
    End Sub

    Public Sub AppLogon(ByVal userName As String, ByVal passWord As String) Implements IExecution.AppLogon
        'do nothing
    End Sub

    Private Sub ArielExecution_SymbolStatus(ByVal symbol As String) Handles Me.SymbolStatus

    End Sub
End Class
