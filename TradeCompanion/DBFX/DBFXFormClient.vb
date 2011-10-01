Imports FXCore
Imports DBFX.DBFXClient
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Collections
Imports System.Windows.Forms
Imports System.Threading


Public Delegate Sub DBFXMarketDataEventHandler(ByVal sender As Object, ByVal args As DBFXMarketData)
Public Delegate Sub DBFXOrderEventHandler(ByVal sender As Object, ByVal args As DBFXTrades)
Public Delegate Sub DBFXLoginEventHandler(ByVal sender As Object, ByVal LOGIN_STATUS As Boolean)
Public Delegate Sub WriteToLogWindowEventHandler(ByVal Messages As String)
Public Delegate Sub FeedHistoricalDataHandler(ByVal symb As String, ByVal dateTime As Double, ByVal priceOpen As Double, ByVal priceClose As Double, ByVal priceHigh As Double, ByVal priceLow As Double, ByVal type As Double)
Public Delegate Sub OpenPosition(ByVal openValue As String, ByVal _Instrument As String, ByVal _UserId As String)
Public Delegate Sub Addneworder(ByVal symbol As String, ByVal qty As Integer, ByVal side As Integer, ByVal currency As String, ByVal timestamp As String, ByVal tradeType As Integer, ByVal chartIdentifier As Integer, ByVal accountID As String, ByVal systemName As String, ByVal ordId As String, ByVal tsOpenPosition As Double)


Public Class DBFXFormClient
    Dim BuySell As Boolean
    Dim MarketRate As Double = -1
    Dim oDBFXTrade As DBFXTrades
    Private oCore As CoreAut
    Private oTradeDesk As TradeDeskAut
    Private SubscribeID As Integer = -1
    Public Event DBFXMarketDataEvent As DBFXMarketDataEventHandler
    Public Event DBFXOrderEvent As DBFXOrderEventHandler
    Public Event DBFXLogonEvent As DBFXLoginEventHandler
    Public Event ADDnewOrder As Addneworder
    Public Event WriteToLogWindowEvent As WriteToLogWindowEventHandler
    Public Event openPositionValue As OpenPosition
    Public barInterval As String = "t1"
    Public Event WriteToNSTEvent As FeedHistoricalDataHandler
    Dim oTable As TableAut
    Dim r As RowAut
    Dim accountID As String
    Dim prevOrderId As String
    Dim ev As DBFXMarketData
    Dim marketTable As TableAut
    Dim oAccTable As TableAut 'mdbx
    Public userId As String
    Dim _Instrument As String = ""


    Public Sub Login(ByVal uName As String, ByVal pWard As String, ByVal url As String, ByVal mode As String)
        userId = uName
        Try
            oCore = New CoreAut
            DBFXUtil.WriteDebugLog(".....Core Object Created.")
        Catch e As Exception
            DBFXUtil.WriteDebugLog(".....Fails to create Core Object: " + e.Message)
            RaiseEvent WriteToLogWindowEvent("Fails to Login. Unable to create Core Object.")
            MessageBox.Show("Fails to Login:" + e.Message)
            Exit Sub
        End Try
        Try
            oTradeDesk = oCore.CreateTradeDesk("trader")
            DBFXUtil.WriteDebugLog(".....Trade Desk Created.")
        Catch e As Exception
            DBFXUtil.WriteDebugLog(".....Fails to create Trade Desk: " + e.Message)
            RaiseEvent WriteToLogWindowEvent("Fails to Login. Unable to create Trade Desk.")
            MessageBox.Show("Fails to Login:" + e.Message)
            Exit Sub
        End Try
        Try
            oTradeDesk.Login(uName, pWard, url, mode)
            DBFXConnection.isLoggedIn = oTradeDesk.IsLoggedIn()
            DBFXUtil.WriteDebugLog("..... Trade Connected.")
        Catch e As Exception
            DBFXConnection.isLoggedIn = oTradeDesk.IsLoggedIn()
            DBFXUtil.WriteDebugLog(".....Failed to Login:" + e.Message)
            RaiseEvent WriteToLogWindowEvent("....." + e.Message)
            MessageBox.Show(e.Message)
            Exit Sub
        End Try
        oAccTable = oTradeDesk.FindMainTable("accounts")
        Dim oRow As RowAut
        For Each oRow In oAccTable.Rows 'try oRow = oAccTable.Rows 'fixme amit
            DBFXAccountInfo.arrAccountID = oRow.AccountName
        Next
        marketTable = oTradeDesk.FindMainTable("offers")
        SubscribeID = oTradeDesk.Subscribe(AxTradeDeskEventsSink1)      'Subscribe user for market Data at login time only
        'raiseEvent
        RaiseEvent DBFXLogonEvent(Me, DBFXConnection.isLoggedIn)
    End Sub

    Public Function logout() As String
        If DBFXConnection.isLoggedIn Then
            oTradeDesk.Unsubscribe(SubscribeID) 'Disconnect the user from marketData at the time of logout
            If DBFXMarketData.isMDataConnected Then
                UnSubscribeMarketData()
            End If
            oTradeDesk.Logout()
            DBFXUtil.WriteDebugLog(".... Trade Disconnected. Logout called by user.")
            DBFXConnection.isLoggedIn = oTradeDesk.IsLoggedIn()
            oTradeDesk = Nothing
        End If
        Return userId
    End Function

    Public Sub SubscribeMarketData()
        If Not DBFXMarketData.isMDataConnected Then
            If SubscribeID > 0 Then
                tmrMrk.Interval = 2000
                tmrMrk.Start()
                DBFXMarketData.isMDataConnected = True
                DBFXUtil.WriteDebugLog(".... Market Data Subscribed. ID :" + SubscribeID.ToString)
            Else
                DBFXMarketData.isMDataConnected = False
                DBFXUtil.WriteDebugLog(".... Market Data Subscription Faild.")
                RaiseEvent WriteToLogWindowEvent(".... Market Data Subscription Faild.")
            End If
        End If

    End Sub

    Public Sub UnSubscribeMarketData()
        If DBFXMarketData.isMDataConnected Then
            tmrMrk.Stop()
            DBFXMarketData.isMDataConnected = False
            DBFXUtil.WriteDebugLog(".... Market Data Unsubscribed.")
        End If
    End Sub

    ''' <summary>
    ''' This event raised when the row is added to the any of the tables(orders, trades, closetrades, Acounts, offers)
    '''  we are using this for the getting the response for the order that we placed.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub AxTradeDeskEventsSink1_OnRowAddedEvent(ByVal sender As Object, ByVal e As AxFXCore.ITradeDeskEvents_OnRowAddedEvent) Handles AxTradeDeskEventsSink1.OnRowAddedEvent
        Try
            Dim tableAutmation As ITableAut = TryCast(e.pTableDisp, FXCore.ITableAut)
            Dim side As Integer
            
            If (tableAutmation.Type = "trades") Then
                oTable = oTradeDesk.TablesManager.FindTable("trades")
                r = oTable.FindRow("TradeID", e.sRowID)
                If (r.CellValue("BS") = "B") Then
                    side = 1
                Else
                    side = 2
                End If
                If (prevOrderId = "") Then
                    prevOrderId = r.CellValue("OpenOrderID")
                    _Instrument = r.CellValue("Instrument")
                    RaiseEvent ADDnewOrder(_Instrument, r.CellValue("Lot"), side, _Instrument.Substring(0, 3), r.CellValue("Time"), 3, 0, userId, "simulated", prevOrderId, 0)
                End If
                If (r.CellValue("OpenOrderID") = prevOrderId) Then
                    oDBFXTrade = New DBFXTrades(r.CellValue("OpenOrderID"), r.CellValue("OfferID"), r.CellValue("Lot"), r.CellValue("Instrument"), r.CellValue("BS"), r.CellValue("Time"), side, userId, r.CellValue("Open"), "Executed")
                    DBFXUtil.WriteDebugLog(r.CellValue("Open").ToString() + " " + r.CellValue("Instrument") + " " + r.CellValue("OpenOrderID") + " " + r.CellValue("Lot").ToString())
                    RaiseEvent DBFXOrderEvent(Me, oDBFXTrade)
                    DBFXUtil.WriteDebugLog(">>>>Order executed orderID: " + r.CellValue("OpenOrderID") + " Symbol: " + r.CellValue("Instrument") + " UserID: " + userId)
                    prevOrderId = ""
                End If
            ElseIf (tableAutmation.Type = "closed trades") Then
                oTable = oTradeDesk.TablesManager.FindTable("closed trades")
                r = oTable.FindRow("TradeID", e.sRowID)
                If (r.CellValue("BS") = "B") Then
                    side = 2
                Else
                    side = 1
                End If
                If (prevOrderId = "") Then
                    prevOrderId = r.CellValue("CloseOrderID")
                    _Instrument = r.CellValue("Instrument")
                    RaiseEvent ADDnewOrder(_Instrument, r.CellValue("Lot"), side, _Instrument.Substring(0, 3), r.CellValue("CloseTime"), 3, 0, userId, "simulated", prevOrderId, 0)
                End If
                If (r.CellValue("CloseOrderID") = prevOrderId) Then
                    oDBFXTrade = New DBFXTrades(r.CellValue("CloseOrderID"), r.CellValue("OfferID"), r.CellValue("Lot"), r.CellValue("Instrument"), r.CellValue("BS"), r.CellValue("CloseTime"), side, userId, r.CellValue("Close"), "Executed")
                    DBFXUtil.WriteDebugLog(r.CellValue("Close").ToString() + " " + r.CellValue("Instrument") + " " + r.CellValue("CloseOrderID") + " " + r.CellValue("Lot").ToString())
                    RaiseEvent DBFXOrderEvent(Me, oDBFXTrade)
                    DBFXUtil.WriteDebugLog(">>>Order executed orderID: " + r.CellValue("CloseOrderID") + " Symbol: " + r.CellValue("Instrument") + " UserID: " + userId)
                    prevOrderId = ""
                End If
            End If
        Catch ex As Exception
            DBFXUtil.WriteDebugLog(ex.Message + ex.StackTrace)
        End Try
    End Sub

    ''This method no more need, it used to update the marketdata
    ''This was consumeing  more CPU, so we implemented the other way to solve the problem

    'Public Sub AxTradeDeskEventsSink1_OnRowChangedEvent(ByVal sender As Object, ByVal e As AxFXCore.ITradeDeskEvents_OnRowChangedEvent) Handles AxTradeDeskEventsSink1.OnRowChangedEvent
    '    Try
    '        oTable = oTradeDesk.TablesManager.FindTable("offers")
    '        If (Not e.sRowID = Nothing) Then
    '            r = oTable.FindRow("OfferID", e.sRowID)
    '            ev = New DBFXMarketData(r.CellValue("Instrument"), r.CellValue("Ask"), r.CellValue("Bid"), r.CellValue("Time"))
    '            RaiseEvent DBFXMarketDataEvent(Me, ev)
    '        End If
    '    Catch ex As Exception
    '        'Console.WriteLine(ex.Message)
    '    End Try
    'End Sub

    Public Function PlaceOrder(ByVal Instrument As String, ByVal Lot As Integer, ByVal side As Integer, ByVal Curr As String, ByVal accID As String) As String
        Dim QuoteID As String = ""
        Dim AccountName As String = accID
        Dim orderID As Object = Nothing
        Dim DI As Object = Nothing
        Dim Remark As String = ""
        Dim strAccId As String = ""
        Dim TradeID As String = ""
        MarketRate = 0
        _Instrument = Instrument
        If side = 1 Then
            BuySell = True
        Else
            BuySell = False
        End If

        strAccId = getAccountId(AccountName)
        If strAccId = -1 Then
            DBFXUtil.WriteDebugLog(".....Account Id is not available. Place Order Failed.")
            RaiseEvent WriteToLogWindowEvent("Place order failed. ERROR:  No Account Id available.")
            RaiseEvent WriteToLogWindowEvent(".....Details:" + AccountName + " " + Instrument + " " + Lot.ToString)
            Return ""
            Exit Function
        End If
        Try
            oTradeDesk.CreateFixOrder(oTradeDesk.FIX_OPENMARKET, TradeID, MarketRate, MarketRate, QuoteID, strAccId, Instrument, BuySell, Lot, Remark, orderID, DI)
            DBFXUtil.WriteDebugLog(">>>Order sent to DBFX Server: " + Instrument + ",  " + orderID + ", " + userId)
            DBFXUtil.WriteDebugLog(">>>Order sent to DBFX Server: " + Instrument + ",  " + orderID)
        Catch ex As Exception
            DBFXUtil.WriteDebugLog(".....Order has not Placed. Error:" + ex.Message)
            RaiseEvent WriteToLogWindowEvent("Place order Failed. ERROR: " + ex.Message.ToString)
            RaiseEvent WriteToLogWindowEvent(".....Details:" + AccountName + " " + Instrument + " " + Lot.ToString)
            Return ""
            Exit Function
        End Try
        prevOrderId = orderID 'giri
        Return orderID.ToString()
    End Function

    Public Sub ExecutionReport(ByVal Curr As String, ByVal accID As String, ByVal orderID As Object)
        'giri
        DBFXUtil.WriteDebugLog("order status " + oDBFXTrade.Status) 'giri
        RaiseEvent DBFXOrderEvent(Me, oDBFXTrade)
    End Sub

    Private Function getAccountId(ByVal sAccName As String) As String
        Dim oAccTable As TableAut = oTradeDesk.FindMainTable("accounts")
        Dim oRow As RowAut
        For Each oRow In oAccTable.Rows
            If oRow.AccountName = sAccName Then
                accountID = oRow.AccountID
                Return accountID
                Exit Function
            End If
        Next
        Return -1
    End Function

    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>
    ''' This method is to get historical Data from DBFX server and Feed to NST 
    ''' </summary>
    ''' <param name="symbol"></param>
    ''' <param name="startDate"></param>
    ''' <param name="endDate"></param>
    ''' <param name="requestType"></param>
    ''' <remarks></remarks>
    Public Sub getHistoricalData(ByVal symbol As String, ByVal startDate As String, ByVal endDate As String, ByVal requestType As String)
        Dim rate As FXCore.MarketRateAut
        Dim count As Integer
        Dim blockEndDate As String
        Try
            startDate = Convert.ToDateTime(startDate)
            While (startDate < Convert.ToDateTime(endDate))

                blockEndDate = GetNextDateTime(startDate, Convert.ToInt32(requestType)) ' Set End Date to  Block row 
                If (Date.Compare(Convert.ToDateTime(blockEndDate), Convert.ToDateTime(endDate)) = 1 Or requestType = 0) Then
                    blockEndDate = endDate
                End If

                Dim rates As FXCore.MarketRateEnumAut = DirectCast(oTradeDesk.GetPriceHistory(symbol, barInterval, Convert.ToDateTime(startDate), Convert.ToDateTime(blockEndDate), -1, True, _
                        True), FXCore.IMarketRateEnumAut)

                For count = 1 To rates.Count
                    rate = DirectCast(rates.Item(count), FXCore.MarketRateAut)
                    RaiseEvent WriteToNSTEvent(rate.Instrument, rate.StartDate.ToOADate, rate.BidOpen, rate.BidClose, rate.BidHigh, rate.BidLow, requestType)
                Next

                startDate = blockEndDate ' Move fromDate to next block of row
            End While
        Catch ex As Exception
            DBFXUtil.WriteDebugLog("Historical Data Request " & ex.ToString())
            'MsgBox(ex.Message)
        End Try
    End Sub

    Private Function GetNextDateTime(ByVal datetime As String, ByVal type As Integer) As String

        If type = 0 Then
            datetime = Date.Now.ToString()
            barInterval = "t1"
        ElseIf type < 5 Then
            datetime = AddminutesToDate(datetime, 100)
            barInterval = "m1"
        ElseIf type < 15 Then
            datetime = AddminutesToDate(datetime, 500)
            barInterval = "m5"
        ElseIf type < 30 Then
            datetime = AddminutesToDate(datetime, 450)
            barInterval = "m15"
        ElseIf type < 60 Then
            datetime = AddminutesToDate(datetime, 900)
            barInterval = "m30"
        ElseIf type < 1440 Then
            datetime = Convert.ToDateTime(datetime).AddHours(100)
            barInterval = "H1"
        ElseIf type < 302400 Then
            datetime = Convert.ToDateTime(datetime).AddDays(100)
            barInterval = "D1"
        Else
            datetime = Convert.ToDateTime(datetime).AddMonths(1)
            barInterval = "M1"
        End If

        Return datetime
    End Function

    Private Function AddminutesToDate(ByVal DateTime As String, ByVal min As Integer) As String
        Return Convert.ToDateTime(DateTime).AddMinutes(min)
    End Function

    ''' <summary>
    ''' This method rise an event that updata the Market value after every 2 second
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tmrMrk_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrMrk.Tick
        Dim oRow As RowAut 
        For Each oRow In marketTable.Rows
            ev = New DBFXMarketData(oRow.CellValue("Instrument"), oRow.CellValue("Ask"), oRow.CellValue("Bid"), oRow.CellValue("Time"))
            RaiseEvent DBFXMarketDataEvent(Me, ev)
        Next
    End Sub

    Public Sub OpenPosition()
        Dim oTab As FXCore.TableAut
        Dim openValue As Integer = 0
        Try
            Thread.Sleep(100)
            oTab = oTradeDesk.TablesManager.FindTable("trades")
            For Each row As FXCore.RowAut In oTab.Rows
                If (row.CellValue("Instrument").ToString() = _Instrument) Then
                    If (row.CellValue("IsBuy")) Then
                        openValue = openValue + Convert.ToInt32(row.CellValue("Lot").ToString())
                    Else
                        openValue = openValue - Convert.ToInt32(row.CellValue("Lot").ToString())
                    End If
                End If
            Next
        Catch ex As Exception
            DBFXUtil.WriteDebugLog(ex.Message + ex.StackTrace)
        End Try
        RaiseEvent openPositionValue(Convert.ToString(openValue), _Instrument, userId)
    End Sub

End Class

