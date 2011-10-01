Imports System.io
Imports System.Runtime.InteropServices
Imports System.Threading
Public Delegate Sub UpdateMarketData_Delegate()
Public Delegate Sub MarketData_Delegate(ByVal mdata As TradingInterface.FillMarketData)
Public Delegate Sub PLTrade_Delegate(ByVal mdata As TradingInterface.Fill)

Public Class AlertExecution
    Public Event Executed()
    Public Event OrderPlaced()
    Public Event Rejected(ByVal reason As String)
    Public Event Connected()
    Public Event SymbolStatus(ByVal symbol As String)
    Public Event OpenPosition(ByVal openValue As String, ByVal _Instrument As String, ByVal _UserId As String)
    Public Event Password_reset_response_received(ByVal UserName As String, ByVal UserStatus As Integer, ByVal UserStatusText As String)
    'Public Event HeartBeatReceived(ByVal SeqNo As Integer) 'HeartBeatCallback
    Public Event WriteToLogEvent(ByVal msg As String)
    Public Event MarketDataUpdated(ByVal mdata As TradingInterface.FillMarketData)
    Private lastRefreshed As Integer
    Public WithEvents ex As TradingInterface.IExecution
    ''Public WithEvents dbEx As TradingInterface.DBFXExecution
    Dim keys As SettingsHome
    Declare Auto Function PlaySound Lib "winmm.dll" (ByVal name As String, ByVal hmod As Integer, ByVal flags As Integer) As Integer
    Declare Function SOrder Lib "wrsthnk.dll" Alias "SendOrder" (ByVal ticker As String, _
                        ByVal ask As Integer, ByVal bid As Integer, _
   ByVal ase As Integer, ByVal askSize As Integer, _
   ByVal bidSize As Integer, ByVal year As Integer, _
   ByVal month As Integer, ByVal day As Integer, _
   ByVal hour As Integer, ByVal minute As Integer, _
   ByVal second As Integer, ByRef errorCode As Integer) _
   As Integer

    'Private timerRefreshPosition As System.Timers.Timer

    'Vm_ Run BackGround thread for Historical data feeding
    Public Shared historicalDataFeedThread As Threading.Thread
    Public Shared keepRunningHisThread As Boolean

    Dim ah As AlertsHome = New AlertsHome()
    Dim cal As New PLCal()
    Dim indSys As New IndSysPL
    Dim NSTBool As Boolean = False

    Dim symOneSec As New Hashtable

    Dim del_calculatepl As New MarketData_Delegate(AddressOf CalculatePL)
    Dim del_updatemd As New MarketData_Delegate(AddressOf UpdateMarketData)
    Dim objLock As New Object
    Private listOfOrders As New ArrayList
    Public Event del_calculatepltrade As PLTrade_Delegate
    Public Sub Send(ByVal request As AlertsManager.NewAlert)
        request.price = 0
        ' Only at Best is not supported on the FX market for Espeed so need to set the price accordingly
        If (SettingsHome.getInstance().ExchangeServer = ExchangeServer.Espeed) Or (SettingsHome.getInstance().ExchangeServer = ExchangeServer.Icap) Then
            Dim ah As AlertsHome = New AlertsHome
            If (request.actiontype = AlertsManager.ACTION_BUY) Then
                request.price = ah.GetAskPriceBySymbol(request.symbol)
            ElseIf (request.actiontype = AlertsManager.ACTION_SELL) Then
                request.price = ah.GetBidPriceBySymbol(request.symbol)
            End If
        End If

        Util.WriteDebugLog(" .... Sending order ")
        Util.WriteDebugLog("             symbol-" + request.symbol)
        Util.WriteDebugLog("           exchange-" + request.exch)
        Util.WriteDebugLog("          monthyear-" + request.month_year)
        Util.WriteDebugLog("             action-" + request.actiontype.ToString)
        Util.WriteDebugLog("           quantity-" + request.contracts.ToString)
        Util.WriteDebugLog("           price-" + request.price.ToString)

        Try
            If SettingsHome.getInstance().ExchangeServer = ExchangeServer.DBFX Then
                ex.PlaceOrder(request.symbol, CInt(request.contracts), request.actiontype, request.currency, request.timestamp, request.tradeType, request.chartIdentifier, request.price, request.senderID, request.uID, request.tsOpenPosition)
            Else
                ex.PlaceOrder(request.symbol, CInt(request.contracts), request.actiontype, request.currency, request.timestamp, request.tradeType, request.chartIdentifier, request.price, request.uID, request.tsOpenPosition) 'Exist before
            End If
        Catch ex As Exception
            Util.WriteDebugLog("Send()-->Ordere place--" & ex.Message)
            Throw ex
        End Try
    End Sub

    Public Function Logon(ByVal trader As Trader) As Boolean
        Dim connected As Boolean = False
        Try
            connected = ex.Logon(trader.Param1, trader.Param2, trader.Param3, trader.Param4, trader.Param5, trader.Param6, trader.param7)
            If connected And NSTBool And keys.ExchangeServer = ExchangeServer.DBFX Then
                NSTfeed.SetTCStatus(True)
                If (IsNothing(historicalDataFeedThread)) Then
                    historicalDataFeedThread = New Threading.Thread(AddressOf HistoricalData)
                    historicalDataFeedThread.Name = "HistoricalDataFeed"
                    keepRunningHisThread = True
                    historicalDataFeedThread.Start()
                End If
            End If
        Catch ex As Exception
            Util.WriteDebugLog(" .... Alert Execution Logon ERROR " + ex.StackTrace + "Source:" + ex.Source + ex.Message)
            connected = False
        End Try
        Return connected
    End Function
    'pwreset
    Public Function ResetPassword(ByVal ExistingPassword As String, ByVal NewPassword As String) As Boolean
        Return ex.ResetPassword(ExistingPassword, NewPassword)
    End Function

    Public Sub Logout()
        Try
            If Form1.GetSingletonOrderform().ConnectHT.Count <= 1 And NSTBool Then
                AbortThread() 'abort the backround thread running for Historical request from NST 
            End If
        Catch ex As Exception
            Util.WriteDebugLog("Logout()---> " & ex.Message)
        End Try
        Try
            ex.Logout()
        Catch ex As Exception
            Util.WriteDebugLog("Logout Exception --" & ex.Message)
        End Try

    End Sub

    Public Sub New()

        keys = SettingsHome.getInstance()

        Select Case keys.ExchangeServer
            Case ExchangeServer.CurrenEx
                ex = New TradingInterface.CurrenexExecution
            Case ExchangeServer.Dukascopy 'Vm_ Fix
                ex = New TradingInterface.DukascopyExecution
            Case ExchangeServer.Ariel
                ex = New TradingInterface.ArielExecution
            Case ExchangeServer.Espeed
                ex = New TradingInterface.EspeedExecution
            Case ExchangeServer.DBFX
                ex = New TradingInterface.DBFXExecution
            Case ExchangeServer.Gain
                ex = New TradingInterface.GainExecution
            Case ExchangeServer.Icap
                ex = New TradingInterface.IcapExecution
            Case ExchangeServer.FxIntegral
                ex = New TradingInterface.FxIntegralExecution
        End Select
        AddHandler del_calculatepltrade, AddressOf CalculatePLTrade
        If (IO.File.Exists("C:\NeuroShell Trader 5\Servers\TradeCompanion DataPump.dll")) Then
            NSTBool = True
        End If
    End Sub

    Public Sub SubscribeMarketData()
        ex.SubscribeMarketData(GetallSymbols())
    End Sub

    Private Function GetallSymbols() As String()
        'retrieve all available symbols
        Dim ah As AlertsHome = New AlertsHome()
        Dim ds As DataSet = ah.GetServerSymbols()

        Dim count As Integer
        count = ds.Tables(0).Rows.Count
        Dim symbols(count) As String
        Dim i As Integer
        i = 0
        For Each r As DataRow In ds.Tables(0).Rows
            If Not r.RowState = DataRowState.Deleted Then
                symbols(i) = r.Item("TradeSymbol")
                i = i + 1
            End If
        Next r
        Return symbols
    End Function

    Public Sub UnSubscribeMarketData()

        Try
            ex.UnSubscribeMarketData()
        Catch ex As Exception
            Util.WriteDebugLog("We have problem in UnSubscribeMarketData()" + ex.Message + ex.StackTrace)
        End Try

    End Sub

    Public Sub TradeCaptureReport(ByVal clOrderID As String)
        ex.TradeCaptureReport(clOrderID)
    End Sub

    Private Sub ex_RepeatOrder(ByVal symbol As String, ByVal quantity As Integer, ByVal side As Integer, ByVal tradeType As String, ByVal timeStamp As String) Handles ex.RepeatOrder
        Dim price As String = "0.0"
        If (side = AlertsManager.ACTION_BUY) Then
            price = ah.GetAskPriceBySymbol(symbol)
        ElseIf (side = AlertsManager.ACTION_SELL) Then
            price = ah.GetBidPriceBySymbol(symbol)
        End If
        'price = PipedPrice(price, side)
        ex.PlaceOrder(symbol, quantity, side, "", DateTime.Now, 3, 0, price, timeStamp, 0)
    End Sub

    Private Sub ex_SymbolStatus(ByVal symbol As String) Handles ex.SymbolStatus
        ah.UpdateActiveSymbols(symbol)
    End Sub

    Private Sub ex_WriteToLog(ByVal msg As String) Handles ex.WriteToLogEvent
        RaiseEvent WriteToLogEvent(msg)
    End Sub

    Private Sub ex_Connected() Handles ex.Connected
        'Util.WriteDebugLog(" ...Connected event raised ")
        RaiseEvent Connected()
    End Sub

    'Private Sub ex_HeartbeatReceived(ByVal SeqNo As Integer) Handles ex.HeartBeatReceived 'HeartBeatCallback
    '    RaiseEvent HeartBeatReceived(SeqNo)
    'End Sub

    'pwreset

    Private Sub ex_Password_reset_response_received(ByVal UserName As String, ByVal UserStatus As Integer, ByVal UserStatusText As String) Handles ex.Password_reset_response_received
        RaiseEvent Password_reset_response_received(UserName, UserStatus, UserStatusText)
    End Sub

    Private Sub ex_Disconnected(ByVal reason As String) Handles ex.Disconnected
        Util.WriteDebugLog(" ...Disconnected event raised ")
        RaiseEvent Rejected(reason)
        If (NSTBool) Then AbortThread()
    End Sub

    Private Sub ex_OrderPlaced(ByVal f As TradingInterface.FFillOrder) Handles ex.OrderPlaced

        Dim wasSuccess As Boolean = False

        Try
            Dim newOrder As AlertsManager.NewAlert = New AlertsManager.NewAlert
            newOrder.currency = f.currency
            newOrder.actiontype = f.side
            newOrder.contracts = CDbl(f.quantity)
            newOrder.symbol = f.symbol
            newOrder.timestamp = f.timestamp
            newOrder.orderID = f.OrderID
            newOrder.status = AlertsManager.STATUS_ACCEPTED
            newOrder.tradeType = f.tradeType
            newOrder.chartIdentifier = f.chartIdentifier
            newOrder.senderID = f.senderID

            Dim ah As AlertsHome = New AlertsHome
            ah.AddAlert(newOrder)
            SyncLock (objLock)
                listOfOrders.Add(f.OrderID)
            End SyncLock

            wasSuccess = True
        Catch ex As Exception
            'If you are here then your in trouble.....
            Util.WriteDebugLog(" ... Alert Execution  ERROR (" + f.OrderID + ") " + f.symbol + " " + f.quantity + " " + ex.Message + " " + ex.StackTrace)
        End Try

        If wasSuccess Then RaiseEvent OrderPlaced()

        'wait 10 seconds for server response.
        Dim waitThread As New Thread(AddressOf OrderResponseWait)
        waitThread.Name = f.OrderID
        waitThread.Start(f.OrderID)
    End Sub

    Private Sub ex_OrderFilled(ByVal f As TradingInterface.FFillOrder, ByVal fdata As TradingInterface.Fill) Handles ex.OrderFilled

        Dim wasSuccess As Boolean = False

        Try
            Thread.CurrentThread.CurrentCulture = SettingsHome.getInstance().Culture

            Dim order As AlertsManager.NewAlert = New AlertsManager.NewAlert
            order.actiontype = fdata.side
            order.contracts = CDbl(fdata.Qty)
            order.exch = fdata.Exchange
            order.month_year = fdata.monthyear
            order.orderID = f.OrderID
            order.price = fdata.price
            order.securityType = 2
            order.statusMessage = f.Message
            order.symbol = fdata.Symbol
            order.timestamp = fdata.timestamp
            order.currency = fdata.currency
            order.execOrderId = fdata.systemName
            order.senderID = fdata.accountId
            fdata.tsOpenPosition = f.tsOpenPosition

            If order.currency = Nothing Then
                Dim OrderPlacedAlert As AlertsManager.NewAlert = New AlertsHome().getAlert(order.orderID)
                order.currency = OrderPlacedAlert.currency
            End If

            Util.WriteDebugLog(" ... Alert Execution (" + f.OrderID + ") " + fdata.Symbol + " " + fdata.Qty.ToString + " " + f.Message)

            ah.AddFill(order)

            If (order.statusMessage = "Partially Filled" Or order.statusMessage = "Filled") Then
                Util.WriteDebugLog(" ... PLTrade Entry (" + f.OrderID + ") " + fdata.Symbol + " " + fdata.Qty.ToString + " " + f.Message + " " + order.price.ToString())
                ah.AddPLTrade(order, PLCal.dsPLTrade)
                RaiseEvent del_calculatepltrade(fdata)
            End If

            SyncLock (objLock)
                listOfOrders.Remove(f.OrderID)
            End SyncLock

            wasSuccess = True

            'Sound on executing the order
            If (order.statusMessage <> "New") Then
                Dim settingsSound As New SettingsSound
                settingsSound.getSettings()
                If (settingsSound.UseDefaultSound = True) Then
                    Beep()
                ElseIf (settingsSound.OwnSound = True) Then
                    Try
                        Const SND_FILENAME As Integer = &H20000
                        PlaySound(Application.StartupPath + "\Sounds\" + settingsSound.OwnSoundName.Trim(), 0, SND_FILENAME)
                    Catch
                        'Problem in playing sound so just play beep sound
                        Beep()
                    End Try
                End If
            End If

        Catch ex As Exception
            Util.WriteDebugLog(" ... Alert Execution  ERROR (" + f.OrderID + ") " + fdata.Symbol + " " + fdata.Qty.ToString + " " + ex.Message + " " + ex.StackTrace)
        End Try

        If wasSuccess Then RaiseEvent Executed()

    End Sub

    Private Sub SendFeed(ByVal f As TradingInterface.FillMarketData)
        Dim result As Integer
        Dim str As String
        Dim i, j, iLength, jLength, base As Integer
        Dim bid, offer As Integer

        str = f.BidPrice

        i = str.IndexOf(".")
        If i <= 0 Then
            i = 0
            iLength = 0
            bid = CInt(str)
        Else
            iLength = str.Length - i - 1
            bid = CInt(str.Substring(0, i) & str.Substring(i + 1))
        End If

        str = f.OfferPrice

        j = str.IndexOf(".")
        If j <= 0 Then
            j = 0
            jLength = 0
            offer = CInt(str)
        Else
            jLength = str.Length - i - 1
            offer = CInt(str.Substring(0, i) & str.Substring(i + 1))
        End If

        base = 1
        If iLength > jLength Then
            For counter As Integer = 1 To iLength Step 1
                base = base * 10
            Next counter
            For counter As Integer = 1 To iLength - jLength Step 1
                offer = offer * 10
            Next counter
        ElseIf iLength <= jLength Then
            For counter As Integer = 1 To jLength Step 1
                base = base * 10
            Next counter
            For counter As Integer = 1 To jLength - iLength Step 1
                bid = bid * 10
            Next counter
        End If

        Dim sec As Integer = DateTime.Now.Second
        Dim min As Integer = DateTime.Now.Minute
        Dim hr As Integer = DateTime.Now.Hour
        Dim month As Integer = DateTime.Now.Month
        Dim year As Integer = DateTime.Now.Year
        Dim day As Integer = DateTime.Now.Day
        Dim Result1 As Integer

        Result1 = SOrder(f.Symbol, offer, bid, base, 0, 0, year, month, day, hr, min, sec, result)
        If result = 64 Then
            Util.WriteDebugLog("Error sending bid/ask data to metaserver error Code:" & result & " Server not Running")
        ElseIf (result = 512) Then
            Util.WriteDebugLog("Error sending bid/ask data to metaserver error Code:" & result & " All values are zero")
        ElseIf (result = 8) Then
            Util.WriteDebugLog("Error sending bid/ask data to metaserver error Code:" & result & " Buffer has not free space")
        ElseIf (Result1 <> 1) Then
            Util.WriteDebugLog("Error sending bid/ask data to metaserver error Code:" & result & " Unknown Reason")
        End If
    End Sub

    Private Sub ex_MarketDataUpdate(ByVal f As TradingInterface.FillMarketData) Handles ex.MarketDataUpdate
        Try
            SendFeed(f)
            If (NSTBool) Then NSTfeed.setdata(f.Symbol, Now.ToOADate(), Convert.ToDouble(f.BidPrice))
            lastRefreshed = -1
            If (symOneSec.ContainsKey(f.Symbol)) Then lastRefreshed = symOneSec(f.Symbol)
            Dim currentSecond As Integer = DateTime.Now.Second
            If SettingsHome.getInstance().ExchangeServer <> ExchangeServer.Icap Or _
              SettingsHome.getInstance().ExchangeServer <> ExchangeServer.FxIntegral Then
                If (Math.Abs(currentSecond - lastRefreshed) >= 2) Then
                    del_updatemd.BeginInvoke(f, Nothing, Nothing) 'update in database
                    del_calculatepl.BeginInvoke(f, Nothing, Nothing) 'update in spot position
                    RaiseEvent MarketDataUpdated(f) 'update in marketdatawindow
                End If
            Else
                If (Math.Abs(currentSecond - lastRefreshed) >= 2) Then
                    del_updatemd.BeginInvoke(f, Nothing, Nothing) 'update in database
                    del_calculatepl.BeginInvoke(f, Nothing, Nothing) 'update in spot position
                End If
                RaiseEvent MarketDataUpdated(f)
            End If
            If (symOneSec.ContainsKey(f.Symbol)) Then
                symOneSec(f.Symbol) = currentSecond
            Else
                symOneSec.Add(f.Symbol, currentSecond)
            End If
        Catch ex As Exception
            Util.WriteDebugLog(" ... Market Data Execution  ERROR (" + f.BidPrice + ") " + f.Symbol + " " + f.TimeStamp + " " + ex.Message)
        End Try

    End Sub

    Private Sub UpdateMarketData(ByVal f As TradingInterface.FillMarketData)
        ah.UpdateMDHistory(f)
    End Sub

    Private Sub CalculatePL(ByVal f As TradingInterface.FillMarketData)
        'SyncLock Me
        Dim timestamp As String = f.TimeStamp
        Dim tradingDate As DateTime
        tradingDate = Date.Now 'EServerDependents.GetDateTime(timestamp) 'Date.Parse(timestamp.Insert(4, " ").Insert(7, " ").Replace("-", " "))
        Dim dsPLCalc As DataSet = PLCal.dsPLCalc
        Dim reverseSymbol As String = ""
        reverseSymbol = EServerDependents.GetCombinedCurrency(EServerDependents.GetSecondCurrency(f.Symbol), EServerDependents.GetFirstCurrency(f.Symbol))
        If ((dsPLCalc.Tables(0).Select("Symbol = '" + f.Symbol + "'").Length > 0) Or _
        (dsPLCalc.Tables(0).Select("BaseSymbol = '" + f.Symbol + "'").Length > 0)) Or _
        (dsPLCalc.Tables(0).Select("BaseSymbol = '" + reverseSymbol + "'").Length > 0) Then
            cal.CalculatePIPSMarketData(f)
            indSys.UpdateIndSysForMarket(f.Symbol)
        End If
        'End SyncLock
    End Sub

    Private Sub CalculatePLTrade(ByVal f As TradingInterface.Fill)
        cal.CalculatePIPS(f)
        indSys.CalIndSysOpenPos(f)
    End Sub

    ''' <summary>
    ''' This method to check wether theres any symbol polted by neuro shell 
    ''' if yes Then it calls for historical request to that symbol.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub HistoricalData()
        Dim symbol As String
        Dim barInt As Single

        While (keepRunningHisThread)
            Try
                symbol = NSTfeed.getHistoricalSymbol()
                If symbol <> "No" Then
                    barInt = NSTfeed.getHistoricalBarInterval()
                    ex.HistoricalData(symbol, (Date.FromOADate(NSTfeed.getHistoricalDataTime())), DateAndTime.Now, barInt)
                    NSTfeed.SetFlag()
                End If
            Catch ex As Exception
                Util.WriteDebugLog("AlertExceution -HistoricalData Thread " + ex.Message)
                AbortThread()
            End Try
            Threading.Thread.Sleep(100)
        End While
    End Sub

    ''' <summary>
    ''' This Event Feed the historical Data to NST
    ''' </summary>
    ''' <param name="symb">symb: Symbol that ploted in neuroshell by the user</param>
    ''' <param name="dateTime">dateTime: The period that user requesting for the Historical data</param>
    ''' <param name="priceOpen">priceOpen: Open price of that symbol in history on that date and time</param>
    ''' <param name="priceClose">priceClose: Close price of that symbol in history on that date and time</param>
    ''' <param name="priceHigh">priceHigh: Highest price of that symbol in that history period on that date and time</param>
    ''' <param name="priceLow">priceLow: Lowest price of that symbol in that history period on that date and time</param>
    ''' <param name="type">Type: Barinterval that user requested for historical data</param>
    ''' <remarks></remarks>
    ''' 
    Public Sub FeedtonstHisData(ByVal symb As String, ByVal dateTime As Double, ByVal priceOpen As Double, ByVal priceClose As Double, ByVal priceHigh As Double, ByVal priceLow As Double, ByVal type As Double) Handles ex.FeedHistoricalDataToNST
        NSTfeed.sethisdata(symb, dateTime, priceOpen, priceClose, priceHigh, priceLow, Convert.ToSingle(type))
    End Sub

    Private Sub OpenPositionValue(ByVal openValue As String, ByVal _Instrument As String, ByVal _UserId As String) Handles ex.OpenPositionValue
        RaiseEvent OpenPosition(openValue, _Instrument, _UserId)
    End Sub

    ''' <summary>
    ''' TO kill historical Thread
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AbortThread()
        keepRunningHisThread = False
        NSTfeed.SetTCStatus(False)
        NSTfeed.SetFlag()
        historicalDataFeedThread = Nothing
    End Sub

    'Private Function PipedPrice(ByVal price As String, ByVal side As Integer) As String
    '    Dim count As Integer = 0
    '    Dim temp As Decimal
    '    Dim pipPrice As String = Nothing
    '    Try
    '        count = (price.Length - 1) - price.IndexOf(".")
    '        If (count = 2) Then
    '            If (side = AlertsManager.ACTION_BUY) Then
    '                temp = Convert.ToDouble(price) + 0.01
    '            Else
    '                temp = Convert.ToDouble(price) - 0.01
    '            End If
    '            pipPrice = temp.ToString("0.00")
    '        Else
    '            If (side = AlertsManager.ACTION_BUY) Then
    '                temp = Convert.ToDouble(price) + 0.0001
    '            Else
    '                temp = Convert.ToDouble(price) - 0.0001
    '            End If
    '            pipPrice = Decimal.Round(temp, 4).ToString("0.0000")
    '        End If
    '        Return pipPrice
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function

    ''' <summary>
    '''Wait for order response from server,
    '''IF user traded but there is no response from server
    '''then we must popup saying that order not filled  
    ''' </summary>
    ''' <param name="orderID">OrderID we will get it when order placed in server, 
    ''' This used to compare listOfOrders, if this orderID exists in the list then order not filled.
    ''' </param>
    ''' <remarks></remarks>
    Private Sub OrderResponseWait(ByVal orderID As Object)

        'Wait for order response say 5 second, i think it's more 
        System.Threading.Thread.Sleep(5000)

        Dim index As Integer = listOfOrders.IndexOf(orderID)
        If index > -1 Then
            Dim popupmsg As New PopupMsgBox
            popupmsg.ShowMessage("Order ID " + orderID + " not Filled ", 3000, "Order Not Filled")
            Util.WriteDebugLog("Order not filled for Order ID --- " + orderID)
        End If

    End Sub

End Class
