Imports EspeedDriver

Public Class EspeedExecution
    Implements IExecution

    Private WithEvents client As EspeedClient = Nothing
    'Public _IP As String = "localhost"
    'Public _Port As String = "3333"
    Private listOfOrders As List(Of FFillOrder) = New List(Of FFillOrder)
    Public Event OrderFilled(ByVal f As FFillOrder, ByVal fdata As Fill) Implements IExecution.OrderFilled
    Public Event OrderPlaced(ByVal f As FFillOrder) Implements IExecution.OrderPlaced
    Public Event MarketDataUpdate(ByVal f As FillMarketData) Implements IExecution.MarketDataUpdate
    Public Event Disconnected(ByVal reason As String) Implements IExecution.Disconnected
    Public Event Connected() Implements IExecution.Connected
    'pwreset
    Public Event Password_reset_response_received(ByVal UserName As String, ByVal UserStatus As Integer, ByVal UserStatusText As String) Implements IExecution.Password_reset_response_received
    'Public Event HeartBeatREceived(ByVal SeqNo As Integer) Implements IExecution.HeartBeatReceived 'HeartBeatCallback
    Public Event WriteToLogEvent(ByVal msg As String) Implements IExecution.WriteToLogEvent
    'Vm_ Not used
    Public Event FeedHisDataToNST(ByVal symb As String, ByVal dateTime As Double, ByVal priceOpen As Double, ByVal priceClose As Double, ByVal priceHigh As Double, ByVal priceLow As Double, ByVal type As Double) Implements IExecution.FeedHistoricalDataToNST
    Public Event OpenpositionValue(ByVal openValue As String, ByVal _Instrument As String, ByVal _UserId As String) Implements IExecution.OpenPositionValue
    Public Event SymbolStatus(ByVal symbol As String) Implements IExecution.SymbolStatus
    Public Event RepeatOrder(ByVal symbol As String, ByVal quantity As Integer, ByVal side As Integer, ByVal tradeType As String, ByVal timeStamp As String) Implements IExecution.RepeatOrder
    Public Function Logon(ByVal ip As String, ByVal port As String, ByVal username As String, ByVal password As String, Optional ByVal param5 As String = "", Optional ByVal param6 As String = "", Optional ByVal param7 As String = "") As Boolean Implements IExecution.Logon
        Dim wasSuccess As Boolean = False
        'If Not client Is Nothing Then
        '    If client.Connected Then wasSuccess = client.Session.LoggedIn
        'Else
        If (client Is Nothing) Then client = New EspeedClient(ip, System.Int32.Parse(port))
        'If Not client.setPip(Application.StartupPath + "\\scalper_pip.txt") Then
        '    MessageBox.Show("You dont have the the valid 'scalper_pip.txt' file received from Scalper systems in the directory from where you are running this application.\nContact Scalper systems for further assistance.")
        '    Return False
        'End If
        'End If
        If wasSuccess = False Then
            wasSuccess = client.Logon(username, password)
        End If
        Return wasSuccess
    End Function

    Public Sub Logout() Implements IExecution.Logout
        If client Is Nothing Then Exit Sub
        Try
            client.logout()
        Catch ex As Exception
            Throw ex
        End Try

        'Try
        '    If client.Connected Then client.logout()
        '    client.closeLogger()
        'Catch ex As NullReferenceException
        '    MessageBox.Show("Connection closed", "BGC TC")
        'End Try
    End Sub
    Public Sub LogonStatusHandler(ByVal sender As Object, ByVal e As Integer) Handles client.eSpeedLoginResponseEvent
        Select Case e
            Case EspeedClient.CFETI_CONNECTION_ACCEPTED
                RaiseEvent Connected()
            Case EspeedClient.CFETI_LOGIN_REJECTED
                RaiseEvent Disconnected("Login Failed")
            Case EspeedClient.CFETI_LOGIN_TERMINATED
                RaiseEvent Disconnected(Nothing)
            Case EspeedClient.CFETI_LOGOUT_ACCEPTED
                RaiseEvent Disconnected(Nothing)
            Case EspeedClient.CFETI_DISCONNECT_ACCEPTED
                RaiseEvent Disconnected(Nothing)
            Case EspeedClient.CFETI_SESSION_LOST
                RaiseEvent Disconnected("Session Lost")
            Case EspeedClient.CFETI_SESSION_RESTORED
                RaiseEvent Connected()
        End Select
        'Select Case e._fixNotice.Code
        '    Case com.scalper.fix.driver.FIXNotice.FIX_SESSION_ESTABLISHED
        '        RaiseEvent Connected()
        '    Case com.scalper.fix.driver.FIXNotice.FIX_LOGON_FAILED
        '        RaiseEvent Disconnected("Logon Failed")
        '    Case com.scalper.fix.driver.FIXNotice.FIX_CONNECTION_FAILED
        '        RaiseEvent Disconnected("Connection Failed.")
        '    Case com.scalper.fix.driver.FIXNotice.FIX_DISCONNECTED
        '        RaiseEvent Disconnected(Nothing)
        '        client = Nothing
        'End Select
    End Sub

    Public Sub PlaceOrder(ByVal symbol As String, ByVal qty As Integer, ByVal side As Integer, ByVal currency As String, ByVal timestamp As String, ByVal tradeType As Integer, ByVal chartIdentifier As Integer, ByVal price As String, ByVal accountID As String, ByVal systemName As String, ByVal tsOpenPosition As Double) Implements IExecution.PlaceOrder
        'Do Nothing
    End Sub
    'pwreset
    Public Function ResetPassword(ByVal ExistingPassword As String, ByVal NewPassword As String) As Boolean Implements IExecution.ResetPassword
        'do nothing
        Return False
    End Function
    Public Sub PlaceOrder(ByVal symbol As String, ByVal qty As Integer, ByVal side As Integer, ByVal currency As String, ByVal timestamp As String, ByVal tradeType As Integer, ByVal chartIdentifier As Integer, ByVal price As String, ByVal systemName As String, ByVal tsOpenPosition As Double) Implements IExecution.PlaceOrder
        Dim ordID As String
        ordID = client.placeOrder(symbol, qty, side, price, currency, tradeType)
        Dim neworder As FFillOrder = New FFillOrder(ordID)
        neworder.currency = currency
        neworder.quantity = qty
        neworder.symbol = symbol
        neworder.side = side
        neworder.timestamp = timestamp
        neworder.tradeType = tradeType
        neworder.chartIdentifier = chartIdentifier
        neworder.senderID = client.getSender()
        neworder.systemName = systemName
        neworder.tsOpenPosition = tsOpenPosition
        listOfOrders.Add(neworder)
        RaiseEvent OrderPlaced(neworder)

        'Dim ordID As String
        'If client.Session.LoggedIn = False Then
        '    Logon(client.SenderCompID, client.TargetCompID)
        'End If
        'If client.Session.LoggedIn Then
        '    ordID = client.placeOrder(symbol, qty, side, price, currency, tradeType)
        '    Dim neworder As FFillOrder = New FFillOrder(ordID)
        '    neworder.currency = currency
        '    neworder.quantity = qty
        '    neworder.symbol = symbol
        '    neworder.side = side
        '    neworder.timestamp = timestamp
        '    neworder.tradeType = tradeType
        '    neworder.chartIdentifier = chartIdentifier
        '    neworder.senderID = client.SenderCompID()
        '    listOfOrders.Add(neworder)
        '    RaiseEvent OrderPlaced(neworder)
        'Else
        '    RaiseEvent Disconnected("Disconnected")
        'End If
    End Sub

    Public Sub TradeCaptureReport(ByVal clOrderID As String) Implements IExecution.TradeCaptureReport
        'client.TradeCaptureReportRequest(clOrderID)
    End Sub

    

    Private Sub OrderStatusHandler(ByVal sender As Object, ByVal e As EspeedOrderStatusEventArgs) Handles client.eSpeedOrderResponseEvent
        Dim f As FFillOrder = New FFillOrder(e.OrderId)
        Dim wasFilled As Boolean = False
        Dim strStatus As String = ""
        Dim createAlert As Boolean = False
        Select Case e.Command
            Case EspeedClient.CFETI_ORDER_REJECTED
                strStatus = strStatus + "Rejected (" + e.Status + ")"
                createAlert = True
            Case EspeedClient.CFETI_ORDER_QUEUED
                strStatus = strStatus + "New" 'Queued
                createAlert = True
            Case EspeedClient.CFETI_ORDER_EXECUTED
                strStatus = strStatus + "Filled"
                createAlert = True
                wasFilled = True
            Case EspeedClient.CFETI_ORDER_EXECUTING
                strStatus = strStatus + "Partially Filled" 'Executing
                createAlert = True
            Case EspeedClient.CFETI_ORDER_CANCELLED
                strStatus = strStatus + "Cancelled"
                createAlert = True
                'Case EspeedClient.CFETI_TRADE_CONFIRM
                '   strStatus = strStatus + "Filled" 'Trade Confirm
                '  createAlert = True
        End Select

        Dim i As Integer = listOfOrders.FindIndex(New Predicate(Of FFillOrder)(AddressOf f.Predicate))
        If i > -1 Then
            If createAlert Then
                listOfOrders.Item(i).Status = wasFilled.ToString
                listOfOrders.Item(i).Message = strStatus
                Dim fdata As Fill = New Fill
                fdata.Exchange = e.ExchID
                fdata.Symbol = e.Symbol
                fdata.side = e.Side
                fdata.currency = e.Currency
                fdata.orderId = e.OrderId
                fdata.accountId = e.SenderId
                fdata.systemName = listOfOrders.Item(i).systemName
                fdata.monthyear = listOfOrders.Item(i).chartIdentifier
                'If wasFilled Then
                fdata.Qty = CInt(e.Qty)
                fdata.price = e.Price
                'End If
                fdata.timestamp = e.Timestamp
                RaiseEvent OrderFilled(listOfOrders.Item(i), fdata)
            End If
        End If


        'Dim f As FFillOrder = New FFillOrder(e.ClOrdID)
        'Dim wasFilled As Boolean = False
        'Dim instr As String = e.Instrument
        'Dim exch As String = e.ExchangeID
        'Dim side As String = IIf(e.Side = 1, "Buy", "Sell")
        'Dim strStatus As String = "" 'instr + "(" + exch + ") 0" + side + " "
        'Dim createAlert As Boolean = False
        'Select Case e.OrderStatus(0)
        '    Case "A"
        '        strStatus = strStatus + "Pending New"
        '        createAlert = True
        '    Case "0"
        '        strStatus = strStatus + "New"
        '        createAlert = True
        '    Case "1"
        '        strStatus = strStatus + "Partially Filled"
        '        wasFilled = True
        '        createAlert = True
        '    Case "2"
        '        strStatus = strStatus + "Filled"
        '        wasFilled = True
        '        createAlert = True
        '    Case "3"
        '        strStatus = strStatus + "Done for day"
        '    Case "4"
        '        strStatus = strStatus + "Canceled( " + e.OrderStatusLong + " )"
        '        createAlert = True
        '        f.OrderID = e._fixMsg.getStringFieldValue(41)
        '    Case "5"
        '        strStatus = strStatus + "Replaced( " + e.OrderStatusLong + " )"
        '        createAlert = True
        '    Case "A"
        '        strStatus = strStatus + "Pending Cancel"
        '        createAlert = True
        '        f.OrderID = e._fixMsg.getStringFieldValue(41)
        '    Case "8"
        '        strStatus = strStatus + "Rejected( " + e.OrderStatusLong + " )"
        '        createAlert = True
        '    Case Else
        '        strStatus = strStatus + "Unknown( " + e.OrderStatusLong + " )"
        '        createAlert = True
        'End Select
        'Dim i As Integer = listOfOrders.FindIndex(New Predicate(Of FFillOrder)(AddressOf f.Predicate))
        'If i > -1 Then
        '    If createAlert Then
        '        listOfOrders.Item(i).Status = wasFilled.ToString
        '        listOfOrders.Item(i).Message = strStatus
        '        Dim fdata As Fill = New Fill
        '        fdata.Exchange = e.ExchangeID
        '        fdata.Symbol = e.Instrument
        '        fdata.side = e.Side
        '        fdata.currency = e.currency
        '        fdata.orderId = e.OrderID
        '        fdata.accountId = e.Sender
        '        If wasFilled Then
        '            fdata.Qty = e.FilledQty
        '            fdata.price = e.Price
        '        End If
        '        fdata.timestamp = e.FillTime.ToString
        '        RaiseEvent OrderFilled(listOfOrders.Item(i), fdata)
        '    End If
        'End If
    End Sub


    Public Sub SubscribeMarketData(ByVal symbols() As String) Implements IExecution.SubscribeMarketData

        'TODO Check for Espeed connection

        'Subscribe Espeed market data
        client.SubscribeMarketData(symbols)
        RaiseEvent Connected()

    End Sub
    Public Sub UnSubscribeMarketData() Implements IExecution.UnSubscribeMarketData
        'Unsubscribe Espeed market data
        client.UnSubscribeMarketData()
    End Sub
    Private Sub MarketDataStatusHandler(ByVal sender As Object, ByVal e As EspeedMarketDataStatusEventArgs) Handles client.eSpeedMarketDataResponseEvent
        Dim marketData As FillMarketData = New FillMarketData()
        marketData.BidPrice = e.BidPrice
        marketData.OfferPrice = e.OfferPrice
        marketData.TimeStamp = e.TimeStamp
        marketData.Symbol = e.Symbol
        If (marketData.BidPrice <> "" Or marketData.OfferPrice <> "") Then
            RaiseEvent MarketDataUpdate(marketData)
        End If
    End Sub
    Public Sub AppLogon(ByVal userName As String, ByVal passWord As String) Implements IExecution.AppLogon
        'do Nothing
    End Sub
    Public Sub HistoricalData(ByVal symbol As String, ByVal fromDate As String, ByVal toDate As String, ByVal type As String) Implements IExecution.HistoricalData
        'Vm
        ''''There is no support for historical data from Espeed server
    End Sub

    Private Sub EspeedExecution_SymbolStatus(ByVal symbol As String) Handles Me.SymbolStatus

    End Sub
End Class
