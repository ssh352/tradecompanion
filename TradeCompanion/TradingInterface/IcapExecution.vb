Imports Icap.com.scalper.fix.driver.client
Imports System.Threading

Public Class IcapExecution
    Implements IExecution

    Private WithEvents client As Icap.com.scalper.fix.driver.client.IcapClient = Nothing

    Public Event Connected() Implements IExecution.Connected

    Public Event Disconnected(ByVal reason As String) Implements IExecution.Disconnected

    Public Event MarketDataUpdate(ByVal f As FillMarketData) Implements IExecution.MarketDataUpdate

    Public Event OrderFilled(ByVal f As FFillOrder, ByVal fdata As Fill) Implements IExecution.OrderFilled

    Public Event OrderPlaced(ByVal f As FFillOrder) Implements IExecution.OrderPlaced

    Public Event WriteToLogEvent(ByVal msg As String) Implements IExecution.WriteToLogEvent

    Private listOfOrders As List(Of FFillOrder) = New List(Of FFillOrder)
    Private _IP, _Port, _Sender, _password As String
    'Vm_ Not used
    Public Event FeedHisDataToNST(ByVal symb As String, ByVal dateTime As Double, ByVal priceOpen As Double, ByVal priceClose As Double, ByVal priceHigh As Double, ByVal priceLow As Double, ByVal type As Double) Implements IExecution.FeedHistoricalDataToNST
    Public Event OpenpositionValue(ByVal openValue As String, ByVal _Instrument As String, ByVal _UserId As String) Implements IExecution.OpenPositionValue
    'pwreset
    Public Event Password_reset_response_received(ByVal UserName As String, ByVal UserStatus As Integer, ByVal UserStatusText As String) Implements IExecution.Password_reset_response_received
    Public Event SymbolStatus(ByVal symbol As String) Implements IExecution.SymbolStatus
    Public Event RepeatOrder(ByVal symbol As String, ByVal quantity As Integer, ByVal side As Integer, ByVal tradeType As String, ByVal timeStamp As String) Implements IExecution.RepeatOrder
    Private count As Integer


    Public Function Logon(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String, Optional ByVal param5 As String = "", Optional ByVal param6 As String = "", Optional ByVal param7 As String = "") As Boolean Implements IExecution.Logon
        _IP = param1
        _Port = param2
        _Sender = param3
        _password = param4

        Dim wasSuccess As Boolean = False
        If client Is Nothing Then
            Try
                client = New IcapClient(_IP, System.Int32.Parse(_Port))
            Catch ex As Exception
                RaiseEvent Disconnected("Connection Failed.")
                Return False
            End Try
        Else
            If client.Connected Then wasSuccess = client.Session.LoggedIn
        End If

        If Not wasSuccess Then
            wasSuccess = client.logon(_Sender, "ICAP_Ai_Server")
        End If

        Return wasSuccess
    End Function

    Public Sub Logout() Implements IExecution.Logout
        If client Is Nothing Then Exit Sub
        Try
            UnSubscribeMarketData()
            If client.stApplogon Then
                client.AppLogout()
            End If
            If client.Connected Then client.logout()
            client.closeLogger()
        Catch ex As NullReferenceException
            Throw ex 'MessageBox.Show("Connection closed", "BGC TC")
        End Try
    End Sub

    Public Sub AppLogon(ByVal userName As String, ByVal passWord As String) Implements IExecution.AppLogon
        If client.Session.LoggedIn Then
            client.AppLogon(userName, passWord)
        End If
    End Sub

    Public Sub PlaceOrder(ByVal symbol As String, ByVal qty As Integer, ByVal side As Integer, ByVal currency As String, ByVal timestamp As String, ByVal tradeType As Integer, ByVal chartIdentifier As Integer, ByVal price As String, ByVal systemName As String, ByVal tsOpenPosition As Double) Implements IExecution.PlaceOrder
        Dim ordID As String
        If client.Session.LoggedIn = False Then
            Logon(_IP, _Port, _Sender, _password)
        End If
        If client.Session.LoggedIn Then
            ordID = client.placeOrder(symbol, qty, side, price, currency, tradeType)
            Dim neworder As FFillOrder = New FFillOrder(ordID)
            neworder.currency = currency
            neworder.quantity = qty
            neworder.symbol = symbol
            neworder.side = side
            neworder.timestamp = timestamp
            neworder.tradeType = tradeType
            neworder.chartIdentifier = chartIdentifier
            neworder.senderID = client.SenderCompID()
            neworder.systemName = systemName
            neworder.tsOpenPosition = tsOpenPosition
            listOfOrders.Add(neworder)
            RaiseEvent OrderPlaced(neworder)
        Else
            RaiseEvent Disconnected("Disconnected")
        End If
    End Sub

    Public Sub PlaceOrder(ByVal symbol As String, ByVal qty As Integer, ByVal side As Integer, ByVal currency As String, ByVal timestamp As String, ByVal tradeType As Integer, ByVal chartIdentifier As Integer, ByVal price As String, ByVal accountID As String, ByVal systemName As String, ByVal tsOpenPosition As Double) Implements IExecution.PlaceOrder
        'this method is used to repeate the order when orders got expired with same amount & the current price
        Dim ordID As String
        If Not client.Session.LoggedIn Then
            Logon(_IP, _Port, _Sender, _password)
        End If
        If client.Session.LoggedIn Then
            ordID = client.placeOrder(symbol, qty, side, price, currency, tradeType, accountID)
        Else
            RaiseEvent Disconnected("Disconnected")
        End If
    End Sub
    'pwreset
    Public Function ResetPassword(ByVal ExistingPassword As String, ByVal NewPassword As String) As Boolean Implements IExecution.ResetPassword
        client.PasswordReset(ExistingPassword, NewPassword)
    End Function

    Public Sub SubscribeMarketData(ByVal symbols() As String) Implements IExecution.SubscribeMarketData
        'If client.Session.LoggedIn = False Then
        '    Logon(_IP, _Port, _Sender, _password)
        'End If
        If client.stApplogon Then
            client.subscribeMarketData(symbols)
        Else
            ' RaiseEvent Disconnected("Connection not their")
            'MessageBox.Show("Trader connection is not their")
        End If

    End Sub

    Public Sub TradeCaptureReport(ByVal clOrderID As String) Implements IExecution.TradeCaptureReport
        client.TradeCaptureReportRequest(clOrderID)
    End Sub

    Public Sub UnSubscribeMarketData() Implements IExecution.UnSubscribeMarketData
        If (client.marketDataSubStatus) Then
            client.UnSubscribeMarketData()
            client.marketDataSubStatus = False
        End If
    End Sub

    Public Sub LogonStatusHandler(ByVal sender As Object, ByVal e As Icap.com.scalper.fix.driver.FIXTResponseEventArgs) Handles client.FIXResponseEvent
        Select Case e._fixNotice.Code
            Case Icap.com.scalper.fix.driver.FIXTNotice.FIX_SESSION_ESTABLISHED
                client.AppLogon(_Sender, _password)
                'RaiseEvent Connected()
            Case Icap.com.scalper.fix.driver.FIXTNotice.FIX_LOGON_FAILED
                RaiseEvent Disconnected("Logon Failed")
            Case Icap.com.scalper.fix.driver.FIXTNotice.FIX_CONNECTION_FAILED
                RaiseEvent Disconnected("Connection Failed.")
                'client = Nothing
            Case Icap.com.scalper.fix.driver.FIXTNotice.FIX_DISCONNECTED
                RaiseEvent Disconnected(Nothing)
                client = Nothing
            Case Icap.com.scalper.fix.driver.FIXTNotice.FIX_APP_LOGON_SUCCESS
                client.stApplogon = True
                RaiseEvent Connected()
            Case Icap.com.scalper.fix.driver.FIXTNotice.FIX_APP_LOGON_FAILS
                'If client.Connected Then client.logout()
                RaiseEvent Disconnected(e.Text)
            Case Icap.com.scalper.fix.driver.FIXTNotice.FIX_PASS_CHANGED
                RaiseEvent Password_reset_response_received(e._fixNotice.Message.getValue("553"), "5", e.Text)
            Case Icap.com.scalper.fix.driver.FIXTNotice.FIX_PASS_EXPIRED
                client.stApplogon = True
                RaiseEvent Connected()
                RaiseEvent Password_reset_response_received(e._fixNotice.Message.getValue("553"), "7", e.Text)
            Case Icap.com.scalper.fix.driver.FIXTNotice.FIX_CANCEL_DUP_SESSION
                client.CancelDuplicateSession(e._fixNotice.Message.getValue("923"))
            Case Icap.com.scalper.fix.driver.FIXTNotice.FIX_PASS_CHANGE_FAILED
                RaiseEvent Password_reset_response_received(e._fixNotice.Message.getValue("553"), "3", e.Text)
        End Select
    End Sub

    Private Sub MarketDataStatusHandler(ByVal sender As Object, ByVal e As Icap.com.scalper.fix.driver.IcapMarketDataStatusEventArgs) Handles client.IcapMarketDataResponseEvent
        Dim marketData As FillMarketData = New FillMarketData()
        marketData.BidPrice = e.BidPrice
        marketData.OfferPrice = e.OfferPrice
        marketData.TimeStamp = e.TimeStamp
        marketData.Symbol = e.Symbol
        If (marketData.BidPrice <> "" And marketData.OfferPrice <> "") Then
            RaiseEvent MarketDataUpdate(marketData)
        End If
    End Sub

    Private Sub OrderStatusHandler(ByVal sender As Object, ByVal e As Icap.com.scalper.fix.driver.IcapOrderStatusEventArgs) Handles client.IcapOrderResponseEvent
        Dim f As FFillOrder = New FFillOrder(e.ClOrdID)
        Dim wasFilled As Boolean = False
        Dim instr As String = e.Instrument
        Dim exch As String = e.ExchangeID
        Dim side As String = IIf(e.Side = 1, "Buy", "Sell")
        Dim strStatus As String = "" 'instr + "(" + exch + ") " + side + " "
        Dim createAlert As Boolean = False
        Dim i As Integer = listOfOrders.FindIndex(New Predicate(Of FFillOrder)(AddressOf f.Predicate))
        Select Case e.OrderStatus(0)
            Case "A"
                strStatus = strStatus + "Pending New"
                createAlert = True
            Case "0"
                strStatus = strStatus + "New"
                If (listOfOrders.Item(i).Tag > 0) Then 'do not allow repeate orders to main window
                    createAlert = False
                Else
                    createAlert = True
                End If
            Case "1"
                strStatus = strStatus + "Partially Filled"
                wasFilled = True
                createAlert = True
            Case "2"
                strStatus = strStatus + "Filled"
                wasFilled = True
                createAlert = True
            Case "3"
                strStatus = strStatus + "Done for day"
            Case "4"
                strStatus = strStatus + "Canceled( " + e.OrderStatusLong + " )"
                createAlert = True
                f.OrderID = e._fixMsg.getStringFieldValue(41)
            Case "5"
                strStatus = strStatus + "Replaced( " + e.OrderStatusLong + " )"
                createAlert = True
            Case "A"
                strStatus = strStatus + "Pending Cancel"
                createAlert = True
                f.OrderID = e._fixMsg.getStringFieldValue(41)
            Case "8"
                strStatus = strStatus + "Rejected( " + e.OrderStatusLong + " )"
                createAlert = True
            Case "C"
                strStatus = strStatus + "Expired( " + e.OrderStatusLong + " )"
                If (listOfOrders.Item(i).Tag < (count + 1)) Then 'check & repeat the orders for five times
                    createAlert = False
                    listOfOrders.Item(i).Tag += 1
                    RaiseEvent RepeatOrder(e.Instrument, e.OrderedQty, e.Side, "3", e.ClOrdID)
                Else
                    createAlert = True
                End If
            Case Else
                strStatus = strStatus + "Unknown( " + e.OrderStatusLong + " )"
                createAlert = True
        End Select
        'Dim i As Integer = listOfOrders.FindIndex(New Predicate(Of FFillOrder)(AddressOf f.Predicate))
        If i > -1 Then
            If createAlert Then
                listOfOrders.Item(i).Status = wasFilled.ToString
                listOfOrders.Item(i).Message = strStatus
                Dim fdata As Fill = New Fill
                fdata.Exchange = e.ExchangeID
                fdata.Symbol = e.Instrument
                fdata.side = e.Side
                fdata.currency = e.currency
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

    Private Sub MarketDataLogonResponse() Handles client.IcapMarketDataSubResponseEvent
        RaiseEvent Connected()
    End Sub

    Public Sub HistoricalData(ByVal symbol As String, ByVal fromDate As String, ByVal toDate As String, ByVal type As String) Implements IExecution.HistoricalData
        'Vm
        ''''There is no support for historical data from ICAP server
    End Sub

    Private Sub client_IcapMarketSymbolStatusEvent(ByVal symbol As String) Handles client.IcapMarketSymbolStatusEvent
        RaiseEvent SymbolStatus(symbol)
    End Sub
    Public Sub New()
        'this code block is used to get the count for that much of time the expired orders need to     
        'be repreated      
        Dim fs As System.IO.StreamReader
        fs = New System.IO.StreamReader(Application.StartupPath + "\RepeatCount.txt")
        If (Not IsNothing(fs)) Then
            count = Convert.ToInt16(fs.ReadLine().Trim())
            If (count > 15) Then
                count = 15
            End If
        End If
        fs.Close()
    End Sub
End Class
