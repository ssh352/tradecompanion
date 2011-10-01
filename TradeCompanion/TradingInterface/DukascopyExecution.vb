''<Purpose>Interface for Dukascopy FIX API</Purpose>
''
''<Usage>Connect FIX driver for Dukascopy,
'' By usign this class client communactes Dukascopy server for Login, Logout, PlcaeOrder, CancelOrder, UpdateMarket Data</Usage>
''
''<Requirements> Implements IExecution, Dukascopy.com.scalper.fix.driver </Requirements>
''
''<Author> S7 Software Solutions </Author>

Imports Dukascopy.com.scalper.fix.driver.client

Public Class DukascopyExecution
    Implements IExecution

    Private WithEvents client As Dukascopy.com.scalper.fix.driver.client.DukascopyClient = Nothing
    Private listOfOrders As List(Of FFillOrder) = New List(Of FFillOrder)

    Public Event Connected() Implements IExecution.Connected
    Public Event Disconnected(ByVal reason As String) Implements IExecution.Disconnected
    Public Event FeedHistoricalDataToNST(ByVal symb As String, ByVal dateTime As Double, ByVal priceOpen As Double, ByVal priceClose As Double, ByVal priceHigh As Double, ByVal priceLow As Double, ByVal type As Double) Implements IExecution.FeedHistoricalDataToNST
    Public Event MarketDataUpdate(ByVal f As FillMarketData) Implements IExecution.MarketDataUpdate
    Public Event OrderFilled(ByVal f As FFillOrder, ByVal fdata As Fill) Implements IExecution.OrderFilled
    Public Event OrderPlaced(ByVal f As FFillOrder) Implements IExecution.OrderPlaced
    Public Event Password_reset_response_received(ByVal UserName As String, ByVal UserStatus As Integer, ByVal UserStatusText As String) Implements IExecution.Password_reset_response_received
    Public Event RepeatOrder(ByVal symbol As String, ByVal quantity As Integer, ByVal side As Integer, ByVal tradeType As String, ByVal timeStamp As String) Implements IExecution.RepeatOrder
    Public Event OpenpositionValue(ByVal openValue As String, ByVal _Instrument As String, ByVal _UserId As String) Implements IExecution.OpenPositionValue
    Public Event SymbolStatus(ByVal symbol As String) Implements IExecution.SymbolStatus
    Public Event WriteToLogEvent(ByVal msg As String) Implements IExecution.WriteToLogEvent
    Private _IP, _Port, _Sender, _Target, _Username, _Password As String

    Public Sub HistoricalData(ByVal symbol As String, ByVal fromDate As String, ByVal toDate As String, ByVal type As String) Implements IExecution.HistoricalData
        'Vm
        'There is no support for historical data from Dukascopy server
    End Sub

    Public Function Logon(ByVal ip As String, ByVal port As String, ByVal sender As String, ByVal target As String, Optional ByVal password As String = "", Optional ByVal userName As String = "", Optional ByVal param7 As String = "") As Boolean Implements IExecution.Logon

        Dim wasSuccess As Boolean = False
        _IP = ip
        _Port = port
        _Sender = sender
        _Target = target
        _Username = userName
        _Password = password

        If Not client Is Nothing Then
            If client.Connected Then wasSuccess = client.Session.LoggedIn
        Else
            Try
                client = New DukascopyClient(ip, System.Int32.Parse(port))
            Catch ex As Exception
                MsgBox("Server socket connection failed, Check for the following condition" + vbCrLf + "1. Stunnel is not running" + vbCrLf + "2. Stunnel config file is missing")
                Return False
            End Try
        End If

        If wasSuccess = False Then
            wasSuccess = client.logon(sender, target, userName, password)
        End If
        Return wasSuccess
    End Function

    Public Sub AppLogon(ByVal userName As String, ByVal passWord As String) Implements IExecution.AppLogon

    End Sub

    Public Sub Logout() Implements IExecution.Logout
        If client Is Nothing Then Exit Sub
        Try
            If client.Connected Then client.logout()
            client.closeLogger()
        Catch ex As NullReferenceException
            Throw ex 'MessageBox.Show("Connection closed", "BGC TC")
        End Try
    End Sub

    Public Sub PlaceOrder(ByVal symbol As String, ByVal qty As Integer, ByVal side As Integer, ByVal currency As String, ByVal timestamp As String, ByVal tradeType As Integer, ByVal chartIdentifier As Integer, ByVal price As String, ByVal systemName As String, ByVal tsOpenPosition As Double) Implements IExecution.PlaceOrder
        Dim ordID As String
        If client.Session.LoggedIn = False Then
            Logon(_IP, _Port, _Sender, _Target)
        End If
        If client.Session.LoggedIn Then
            ordID = client.placeOrder(symbol, qty, side, price, currency, tradeType)
            Dim newOrder As FFillOrder = New FFillOrder(ordID)
            newOrder.currency = currency
            newOrder.quantity = qty
            newOrder.symbol = symbol
            newOrder.side = side
            newOrder.timestamp = timestamp
            newOrder.tradeType = tradeType
            newOrder.chartIdentifier = chartIdentifier
            newOrder.senderID = client.SenderCompID()
            newOrder.systemName = systemName
            listOfOrders.Add(newOrder)
            RaiseEvent OrderPlaced(newOrder)
        Else
            RaiseEvent Disconnected("Disconnected")
        End If
    End Sub

    Public Sub PlaceOrder(ByVal symbol As String, ByVal qty As Integer, ByVal side As Integer, ByVal currency As String, ByVal timestamp As String, ByVal tradeType As Integer, ByVal chartIdentifier As Integer, ByVal price As String, ByVal accountID As String, ByVal systemName As String, ByVal tsOpenPosition As Double) Implements IExecution.PlaceOrder
        'Do Nothing
    End Sub

    Public Function ResetPassword(ByVal ExistingPassword As String, ByVal NewPassword As String) As Boolean Implements IExecution.ResetPassword
        'Not supported in this broker
        'client.PasswordReset(ExistingPassword, NewPassword)
    End Function

    Public Sub SubscribeMarketData(ByVal symbols() As String) Implements IExecution.SubscribeMarketData

        If client.Session.LoggedIn = False Then
            Logon(_IP, _Port, _Sender, _Username, _Target)
        End If
        If client.Session.LoggedIn Then
            client.SubscribeMarketData(symbols)
        Else
            RaiseEvent Disconnected("Disconnected")
        End If

    End Sub

    Public Sub TradeCaptureReport(ByVal clOrderID As String) Implements IExecution.TradeCaptureReport
        'Not used
        'client.TradeCaptureReportRequest(clOrderID)
    End Sub

    Public Sub UnSubscribeMarketData() Implements IExecution.UnSubscribeMarketData
        If client.Connected Then
            client.UnSubscribeMarketData()
        End If
    End Sub

    Public Sub LogonStatusHandler(ByVal sender As Object, ByVal e As Dukascopy.com.scalper.fix.driver.FIXResponseEventArgs) Handles client.FIXResponseEvent
        Select Case e._fixNotice.Code
            Case Dukascopy.com.scalper.fix.driver.FIXNotice.FIX_SESSION_ESTABLISHED
                RaiseEvent Connected()
            Case Dukascopy.com.scalper.fix.driver.FIXNotice.FIX_LOGON_FAILED
                RaiseEvent Disconnected(e.Text)
                'client = Nothing
            Case Dukascopy.com.scalper.fix.driver.FIXNotice.FIX_CONNECTION_FAILED
                RaiseEvent Disconnected("Connection Failed.")
                'client = Nothing
            Case Dukascopy.com.scalper.fix.driver.FIXNotice.FIX_DISCONNECTED
                RaiseEvent Disconnected(e.Text)
                client = Nothing
            Case Dukascopy.com.scalper.fix.driver.FIXNotice.FIX_LOGOUT_RECEIVED
                RaiseEvent Disconnected(e.Text)
                client = Nothing
            Case Dukascopy.com.scalper.fix.driver.FIXNotice.FIX_LOGOUT_RESP_RECEIVED
                RaiseEvent Disconnected(e.Text)
                client = Nothing
        End Select
    End Sub

    Private Sub MarketDataStatusHandler(ByVal sender As Object, ByVal e As Dukascopy.com.scalper.fix.driver.DukascopyMarketDataStatusEventArgs) Handles client.DukasCopyMarketDataResponseEvent
        If (e.BidPrice <> "" And e.OfferPrice <> "") Then
            Dim marketData As FillMarketData = New FillMarketData()
            marketData.BidPrice = e.BidPrice
            marketData.OfferPrice = e.OfferPrice
            marketData.TimeStamp = e.TimeStamp
            marketData.Symbol = e.Symbol
            RaiseEvent MarketDataUpdate(marketData)
        End If
    End Sub

    Private Sub OrderStatusHandler(ByVal sender As Object, ByVal e As Dukascopy.com.scalper.fix.driver.DukascopyOrderStatusEventArgs) Handles client.DukasCopyOrderResponseEvent
        Dim f As FFillOrder = New FFillOrder(e.ClOrdID)
        Dim wasFilled As Boolean = False
        Dim instr As String = e.Instrument
        Dim exch As String = e.ExchangeID
        Dim side As String = IIf(e.Side = 1, "Buy", "Sell")
        Dim strStatus As String = "" 'instr + "(" + exch + ") " + side + " "
        Dim createAlert As Boolean = False
        Select Case e.OrderStatus(0)
            Case "A"
                strStatus = strStatus + "Pending New"
                createAlert = True
            Case "B"
                strStatus = strStatus + "Calculated"
                createAlert = False
            Case "0"
                strStatus = strStatus + "New"
                createAlert = True
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
            Case "6"
                strStatus = strStatus + "Pending Cancel"
                createAlert = True
                f.OrderID = e._fixMsg.getStringFieldValue(41)
            Case "8"
                strStatus = strStatus + "Rejected( " + e.OrderStatusLong + " )"
                createAlert = True
            Case Else
                strStatus = strStatus + "Unknown( " + e.OrderStatusLong + " )"
                createAlert = True
        End Select
        Dim i As Integer = listOfOrders.FindIndex(New Predicate(Of FFillOrder)(AddressOf f.Predicate))
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

    Private Sub PositionValue(ByVal sender As Object, ByVal e As Dukascopy.com.scalper.fix.driver.DukascopyOpenPositionEventArgs) Handles client.dukascopyOpenPositonEvent
        If e.Symbol <> "" Then
            RaiseEvent OpenpositionValue(e.OpenPosition, e.Symbol, e.SenderID)
        End If
    End Sub
End Class
