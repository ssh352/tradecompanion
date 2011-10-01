Imports System
Imports Gain.GainClient

Public Delegate Sub GainMarketDataEventHandler(ByVal sender As Object, ByVal args As GainMarketData)
Public Delegate Sub GainOrderEventHandler(ByVal sender As Object, ByVal args As GainTrades)
Public Delegate Sub GainLoginEventHandler(ByVal sender As Object, ByVal LOGIN_STATUS As Boolean)
Public Delegate Sub WriteToLogWindowEventHandler(ByVal Messages As String)
Public Delegate Sub OpenPosition(ByVal openValue As String, ByVal _Instrument As String, ByVal _UserId As String)

Public Class GainClientForm

    Dim MyWebService As New com.efxnow.api.Service
    Dim WithEvents MyRateService As New GainMarketService()
    Dim gTrade As GainTrades

    Public Event GainMarketDataEvent As GainMarketDataEventHandler
    Public Event GainOrderEvent As GainOrderEventHandler
    Public Event GainLogonEvent As GainLoginEventHandler
    Public Event WriteToLogWindowEvent As WriteToLogWindowEventHandler
    Public Event openValue As OpenPosition

    Dim _currentUser As String
    Dim _currentPassword As String
    Dim _isTrdLoggedIn As Boolean
    Dim _isEverMarket As Boolean
    Dim _isMDataLogOut As Boolean
    Dim _key As String = "."
    Dim _brand As String
    Dim _host As String
    Dim _Port As Integer = -1
    Dim _Instrument As String = ""

    Public Function Logon(ByVal UserName As String, ByVal Password As String, ByVal Brand As String, ByVal Host As String, ByVal Port As String, ByVal platform As Integer) As Boolean
        Try
            Try
                If (platform) Then
                    MyWebService.Url = "http://api.efxnow.com/WebServices2.8/Service.asmx"
                Else
                    MyWebService.Url = "http://api.efxnow.com/DEMOWebServices2.8/Service.asmx"
                End If
                _key = MyWebService.GetRatesServerAuth(UserName, Password, Brand)
            Catch e As Exception
                GainUtil.WriteDebugLog("Error :" + e.Message)
                RaiseEvent GainLogonEvent(Me, _isTrdLoggedIn)
                Return False
            End Try

            If Not _key.Contains(".") Then
                _currentUser = UserName
                _currentPassword = Password
                _brand = Brand
                _host = Host
                _Port = Port
                _isTrdLoggedIn = True
                'If GainConnection.isEverMarket And Not GainConnection.isMDataLogOut Then
                '    SubscribeMarketData()
                'End If
                If _isEverMarket And Not _isMDataLogOut Then
                    SubscribeMarketData()
                End If

                StatusChk.Start()
                GainUtil.WriteDebugLog(".....Trade Connected.Trading ID:" + UserName)
            Else
                _isTrdLoggedIn = False
                RaiseEvent WriteToLogWindowEvent(".....Trade Connection Failed.")
                GainUtil.WriteDebugLog(".....Trade Connection Failed.")
                MsgBox("Failed to connect to server :" + vbCrLf + "Wrong Username or password")
                Return _isTrdLoggedIn
            End If
        Catch e As Exception
            Logout()
            RaiseEvent WriteToLogWindowEvent(".....Trade Connection Failed.")
            GainUtil.WriteDebugLog(".....Trade Connection Failed.")
            GainUtil.WriteDebugLog(".....Error:" + e.Message)
        End Try

        RaiseEvent GainLogonEvent(Me, _isTrdLoggedIn)
        Return _isTrdLoggedIn
    End Function

    Public Sub Logout()
        If _isEverMarket Then
            UnSubscribeMarketData()
        End If
        If _isTrdLoggedIn Then
            StatusChk.Stop()
            _isTrdLoggedIn = False
            GainUtil.WriteDebugLog(".....Trade Disconnected.")
            RaiseEvent GainLogonEvent(Me, False)
        End If
    End Sub

    Public Sub UnSubscribe()
        If MyRateService._isMktLoggedIn Then
            _isMDataLogOut = True
            UnSubscribeMarketData()
        End If
    End Sub

    Public Sub UnSubscribeMarketData()
        If MyRateService._isMktLoggedIn Then
            MyRateService.Disconnect()
            _isEverMarket = False
            GainUtil.WriteDebugLog(".....MarketData Disconnected.")
        End If

    End Sub

    Public Function SubscribeMarketData() As Boolean
        Try
            If Not _key.Contains(".") Then
                GainUtil.WriteDebugLog(".....Marketdata Key:" + _key)
                MyRateService._isMktLoggedIn = MyRateService.Connect(_host, _Port, _key)
            Else
                GainUtil.WriteDebugLog(".....Authentication Failed.")
                Exit Function
            End If
        Catch
            GainUtil.WriteDebugLog(".....Authentication Failed. Invalid UserName or Password.")
            Exit Function
        End Try

        If MyRateService._isMktLoggedIn Then
            _isEverMarket = True
            _isMDataLogOut = False
            GainUtil.WriteDebugLog(".....Marketdata Connected. ID:" + _currentUser)
        Else
            GainUtil.WriteDebugLog(".....Marketdata Connection Failed.")
        End If

        Return MyRateService._isMktLoggedIn
    End Function

    Public Function PlaceOrder(ByVal Instrument As String, ByVal Lot As Integer, ByVal BuySell As String, ByVal currency As String) As String

        Dim myResponse As New com.efxnow.api.objDealResponse()
        Dim message As String
        Dim side As Integer = 3 'IOC or GTC
        If BuySell = "1" Then
            BuySell = "B"
        Else
            BuySell = "S"
        End If
        If _isTrdLoggedIn Then

            myResponse = MyWebService.DealRequestAtBest(_currentUser, _currentPassword, Instrument, BuySell, Lot.ToString)
            If myResponse.Success Then

                GainUtil.WriteDebugLog(".....Order Placed. Details:" + _currentUser + " " + Instrument + " " + Lot.ToString)
                RaiseEvent WriteToLogWindowEvent(".....Order Placed. Details:" + _currentUser + " " + Instrument + " " + Lot.ToString)
                message = myResponse.Confirmation
                Dim str() As String = message.Split(">")
                Dim timeStemp As String = str(0) 'timestemp
                message = str(1)
                str = message.Split(":")
                Dim Status As String = str(0).Trim 'Status
                gTrade = New GainTrades(myResponse.ConfirmationNumber, Instrument, Lot, currency, BuySell, timeStemp, side, _currentUser, myResponse.Rate, Status)
                _Instrument = Instrument
            Else

                GainUtil.WriteDebugLog(".....Order has not placed Error:" + myResponse.ErrorNumber.ToString)
                GainUtil.WriteDebugLog(".....Details:" + myResponse.ErrorDescription)
                RaiseEvent WriteToLogWindowEvent(".....Order has not placed. Error:" + myResponse.ErrorDescription)

            End If

        Else

            GainUtil.WriteDebugLog(".....Order has not placed.")
            GainUtil.WriteDebugLog(".....Details: Trade is not Connected")

        End If

        Return myResponse.ConfirmationNumber

    End Function

    Public Sub ExecutionReport()
        RaiseEvent GainOrderEvent(Me, gTrade)
        OpenPosition()
    End Sub

    Private Sub StatusChk_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusChk.Tick
        Try
            If MyWebService.GetRatesServerAuth(_currentUser, _currentPassword, _brand).Contains(".") Then
            End If
        Catch ex As Exception
            Logout()
            GainUtil.WriteDebugLog("Trade Connection Failed.") ' Error:" + ex.Message)
        End Try
    End Sub

    '-------------------
    'Events
    '-------------------
    Public Sub MyRateService_OnRateServiceConnected() Handles MyRateService.OnRateServiceConnected

        MyRateService._isMktLoggedIn = True 'raiseevent login
        GainUtil.WriteDebugLog(".....Market Data Connected ID:" + _currentUser)
        RaiseEvent WriteToLogWindowEvent(".....Market Data Connected.")

    End Sub

    Public Sub MyRateService_OnRateServiceDisconnected() Handles MyRateService.OnRateServiceDisconnected

        MyRateService._isMktLoggedIn = False
        GainUtil.WriteDebugLog(".....Market Data Disconnected event called.")
        RaiseEvent WriteToLogWindowEvent(".....Market Data Disconnected event called.")

    End Sub

    Public Sub MyRateService_OnRateServiceConnectionFailed(ByVal ex As System.Exception) Handles MyRateService.OnRateServiceConnectionFailed

        MyRateService._isMktLoggedIn = False
        GainUtil.WriteDebugLog(".....Market Data Connection Failed. Error:" + ex.ToString())
        RaiseEvent WriteToLogWindowEvent(".....Market Data Connection Failed.") ' Error:" + ex.ToString())
        RaiseEvent GainLogonEvent(Me, False)
    End Sub

    Public Sub MyRateService_OnRateServiceConnectionLost(ByVal ex As System.Exception) Handles MyRateService.OnRateServiceConnectionLost

        MyRateService._isMktLoggedIn = False
        _isTrdLoggedIn = False
        RaiseEvent GainLogonEvent(Me, False)
        GainUtil.WriteDebugLog("....Market Data Connection Lost. Exception: " & ex.ToString())
        RaiseEvent WriteToLogWindowEvent("....Market Data Connection Lost. Exception: " & ex.ToString())

    End Sub

    Public Sub MyRateService_OnRateServiceRate(ByVal rate As GainMarketData) Handles MyRateService.OnRateServiceRate
        RaiseEvent GainMarketDataEvent(Me, rate)
        ' GainUtil.WriteDebugLog("...." + rate.CurrencyPair + " " + rate.Ask.ToString + " " + rate.Bid.ToString)
    End Sub

    Public Sub OpenPosition()
        Dim dSet As DataSet
        Dim filter As String
        filter = "Symbol= '" + _Instrument + "'"
        Try
            dSet = MyWebService.GetPositionBlotterDataSet(_currentUser, _currentPassword)
            Dim dr() As DataRow = dSet.Tables(0).Select(filter)
            If (dr.Length = 1) Then
                RaiseEvent openValue(dr(0).Item("ContractPosition").ToString(), _Instrument, _currentUser)
            End If
        Catch ex As Exception
            GainUtil.WriteDebugLog(ex.Message + ex.StackTrace)
        End Try
    End Sub

End Class



