Imports System.Threading
Public Class Trader
    Public Event Executed()
    Public Event OrderPlaced()
    Public Event Rejected(ByVal reason As String)
    Public Event Connected()
    Public Event ChangeStat()
    Public Event MarketDataUpdated(ByVal mdata As TradingInterface.FillMarketData)
    Public Event OpenPosition(ByVal openValue As String, ByVal _Instrument As String, ByVal _UserId As String)
    Public WithEvents Ae As New AlertExecution
    Private Delegate Sub NoArgumentDelegate()
    Public mDoNotConnect As Boolean
    Public reconnectThread As System.Threading.Thread
    Dim Keys As SettingsHome

    Dim _stat As ConnectionStatus
    Private _isMarketDataConnection As Boolean = False

    Private _AccId As String = "" 'mdbx

    Public Param1 As String 'ip
    Public Param2 As String 'port
    Public Param3 As String 'sender
    Public Param4 As String 'target
    Public Param5 As String
    Public Param6 As String 'used to identify the demo or live in Gain
    Public param7 As String 'uesd in FX-integral to take legal entity.

    Dim alertQ As New Queue()

    Dim logDisconnect As Boolean = True
    Dim wasNetDown As Boolean = False

    Public Sub New()
        Keys = SettingsHome.getInstance()
        mDoNotConnect = False
    End Sub

    Enum ConnectionStatus
        DISCONNECTED = 1
        CONNECTED = 2
        RECONNECTING = 3
        'HEARTBEAT = 4 'Test
    End Enum

    Public Property IsMarketDataConnection() As Boolean
        Get
            Return _isMarketDataConnection
        End Get
        Set(ByVal value As Boolean)
            _isMarketDataConnection = value
        End Set
    End Property

    Public Property AccountId_tr() As String 'mdbx
        Get
            Return _AccId
        End Get
        Set(ByVal value As String)
            _AccId = value
        End Set
    End Property

    Public ReadOnly Property QLength() As Integer
        Get
            Return alertQ.Count
        End Get
    End Property

    Public Property ConnectionId() As String 'Todo for FxIntegral

        Get
            Select Case Keys.ExchangeServer
                Case ExchangeServer.CurrenEx
                    Return Param3
                Case ExchangeServer.Dukascopy
                    Return Param3
                Case ExchangeServer.FxIntegral
                    Return Param3
                Case ExchangeServer.Ariel
                    Return Param1
                Case ExchangeServer.Espeed
                    Return Param3
                Case ExchangeServer.DBFX
                    Return Param1
                Case ExchangeServer.Gain
                    Return Param1
                Case ExchangeServer.Icap
                    Return Param3
            End Select
        End Get
        Set(ByVal value As String)
            Select Case Keys.ExchangeServer
                Case ExchangeServer.CurrenEx
                    Param3 = value
                Case ExchangeServer.Dukascopy 'Vm_ Fix
                    Param3 = value
                Case ExchangeServer.FxIntegral
                    Param3 = value
                Case ExchangeServer.Ariel
                    Param1 = value
                Case ExchangeServer.Espeed
                    Param3 = value
                Case ExchangeServer.DBFX
                    Param1 = value
                Case ExchangeServer.Gain
                    Param1 = value
                Case ExchangeServer.Icap
                    Param3 = value
            End Select
        End Set
    End Property

    Public Property Stat() As String
        Get
            Return _stat
        End Get
        Set(ByVal value As String)
            _stat = value
            RaiseEvent ChangeStat()
        End Set
    End Property

    Public Function Logon() As Boolean
        Dim connected As Boolean = False
        Try
            connected = Ae.Logon(Me)
            If (connected And IsMarketDataConnection) Then SubscribeMarketData()
        Catch ex As Exception
            Util.WriteDebugLog("Logon ------" & ex.Message)
            connected = False
        End Try
        Return connected
    End Function

    'pwresete
    Public Function ResetPassword_t(ByVal ExistingPassword As String, ByVal NewPassword As String) As Boolean
        'mDoNotConnect = True
        Return Ae.ResetPassword(ExistingPassword, NewPassword)
    End Function

    Public Sub logout()
        If (Not reconnectThread Is Nothing) Then reconnectThread.Abort()
        reconnectThread = Nothing
        Try
            mDoNotConnect = True
            AddInLogWindow(ConnectionId + " Disconnected by user", Color.Red)
            Ae.Logout()
        Catch ex As Exception
            Util.WriteDebugLog(" ERROR  Logout" + ex.Message)
            Util.WriteDebugLog(" ERROR  Stack Trace " + ex.StackTrace)
            'MsgBox(ex.Message, "Trader")
        End Try
        ConnectHTRemove(ConnectionId)
    End Sub

    Public Sub SubscribeMarketData()
        Ae.SubscribeMarketData()
    End Sub

    Public Sub UnSubscribeMarketData()
        Ae.UnSubscribeMarketData()
        Select Case Keys.ExchangeServer
            Case ExchangeServer.CurrenEx
                If (Not reconnectThread Is Nothing) Then reconnectThread.Abort()
                ConnectHTRemove(ConnectionId)
            Case ExchangeServer.Dukascopy 'Vm_ Fix
                If (Not reconnectThread Is Nothing) Then reconnectThread.Abort()
                ConnectHTRemove(ConnectionId)
            Case ExchangeServer.FxIntegral 'Giri
                If (Not reconnectThread Is Nothing) Then reconnectThread.Abort()
                ConnectHTRemove(ConnectionId)
            Case ExchangeServer.Ariel
            Case ExchangeServer.Espeed
            Case ExchangeServer.DBFX
            Case ExchangeServer.Gain
            Case ExchangeServer.Icap
        End Select
    End Sub

    Public Sub send(ByVal execute As AlertsManager.NewAlert)
        If (Stat = ConnectionStatus.CONNECTED) Then
            Ae.Send(execute)
        Else
            If (Keys.DiscardAlertInterval > 0) Then
                AddInLogWindow("Alert queued: " + ConnectionId + " not connected", Color.Blue)
                AddInLogWindow("....Details: " + execute.symbol + " " + execute.contracts.ToString(), Color.Blue)

                Util.WriteDebugLog("--- Connection Error: alert added into queue for id " + ConnectionId)

                alertQ.Enqueue(execute)
                AddInLogWindow("...." + alertQ.Count.ToString() + " alerts queued for " + ConnectionId, Color.Blue)
            Else
                AddInLogWindow("Alert rejected: " + ConnectionId + " not connected", Color.Red)
                AddInLogWindow("....Details can not be queued: discard interval is zero", Color.Red)
                AddInLogWindow("....Details: " + execute.symbol + " " + execute.contracts.ToString(), Color.Red)
            End If
        End If
    End Sub

    Public Sub TradeCaptureReport(ByVal clOrderID As String)
        Ae.TradeCaptureReport(clOrderID)
    End Sub

    Public Sub ProcessQueue()
        Util.WriteDebugLog("--- Process Queue Trades " + alertQ.Count.ToString() + " for id " + ConnectionId)
        AddInLogWindow("Process queued trades for " + ConnectionId, Color.Blue)
        Dim ah As New AlertsHome
        While (alertQ.Count > 0)
            If (Stat = ConnectionStatus.CONNECTED) Then
                Dim alert As AlertsManager.NewAlert = CType(alertQ.Dequeue(), AlertsManager.NewAlert)
                Dim filterAlerts As Boolean = False
                If (Keys.FilterAlerts = True And Keys.Platform <> "MetaTrader") Then
                    filterAlerts = ah.IsDuplateAlert(alert)
                End If
                If (alert.exch = "NA" Or Not filterAlerts) Then
                    If (Math.Abs((CType(alert.localtimestamp.Subtract(DateTime.Now), TimeSpan).Minutes)) < Keys.DiscardAlertInterval) Then
                        Ae.Send(alert)
                        AddInLogWindow("Alert sent for " + ConnectionId, Color.Blue)
                        AddInLogWindow("....Details: " + alert.symbol + " " + alert.contracts.ToString(), Color.Blue)
                    Else
                        AddInLogWindow("Alert rejected for " + ConnectionId + " : maximum time exceeded", Color.Red)
                        AddInLogWindow("....Details: " + alert.symbol + " " + alert.contracts.ToString(), Color.Red)
                    End If
                Else
                    AddInLogWindow("Queued Alert rejected: duplicate alert", Color.Red)
                    AddInLogWindow("....Details: " + alert.symbol + " " + alert.contracts.ToString(), Color.Red)
                End If
            Else
                Exit While
            End If
        End While
    End Sub

    Public Sub Disconnect(ByVal reason As String)
        If Not Ae Is Nothing Then
            If mDoNotConnect = False Then
                Util.WriteDebugLog("Trying to reconnect on Broker - " + Keys.ExchangeServer + " And ConnectionId - " + ConnectionId)
                'If Net link is down then TC must go to reconnect no matter which broker is connected   
                If Not Form1.GetSingletonOrderform.netStatBool Then
                    initiateReconnect()
                    wasNetDown = True
                Else
                    'Socket may get close for the following reasons and we need to try to reconnect server.
                    'Case 1 If user gives wrong uesrname or password in case Currenex The socket will get close   
                    'Case 2 "scheduled logout" Currenex server Down for some time and we need to keep trying to reconnect the server
                    'Case 3 If net link is down then control will not come here it will go for reconnect
                    'Case 4 Else in case of any other Broker TC must go for reconnecting
                    Select Case Keys.ExchangeServer
                        Case ExchangeServer.CurrenEx
                            If reason.Equals("scheduled logout") Or wasNetDown Then
                                'CurrenEx server goes down for 5min then need not to connect, Wait for 5min 
                                Util.WriteDebugLog("CurrenEX going to sleep for 6 min with id " + ConnectionId + " at time" + DateTime.Now.ToString())
                                System.Threading.Thread.Sleep(360000)
                                initiateReconnect()
                                wasNetDown = False
                                'Need to set this to false when net went was down and later it came UP 
                            ElseIf reason.Equals("authentication failure.") Then
                                MessageBox.Show("Failed to connect to server" + vbCrLf + "This may be because of a wrong password or some other reasons" + vbCrLf + "Would you like to reconnect?")
                                ReconnectRequest(_isMarketDataConnection)
                            End If
                        Case ExchangeServer.Dukascopy
                            If reason.Equals("Authorization failed") Then
                                MessageBox.Show("Failed to connect to server" + vbCrLf + "This may be because of a wrong password or some other reasons" + vbCrLf + "Would you like to reconnect?")
                                ReconnectRequest(_isMarketDataConnection)
                            Else
                                initiateReconnect()
                            End If
                        Case ExchangeServer.FxIntegral
                            If reason.Equals("UserAuthenticationFailure") Then
                                MessageBox.Show("Password Wrong", "AutoShark")
                                ReconnectRequest(_isMarketDataConnection)
                            ElseIf (reason.Equals("UserNameSetUp")) Then
                                MessageBox.Show("UserName is wrong", "AutoShark")
                                ReconnectRequest(_isMarketDataConnection)
                            Else
                                initiateReconnect()
                            End If
                        Case ExchangeServer.Icap
                            If reason.Equals("Bad Credentials") Then
                                MessageBox.Show("password is wrong" + vbCrLf + _
                                "Note:After 3 continous wrong passwrod try will block the account")
                            End If
                        Case Else
                            initiateReconnect()
                    End Select
                End If
                Exit Sub
            End If
            'logout()
            Util.WriteDebugLog("--- Logout sent to disconnect")
        End If
        Util.WriteDebugLog("---Disconnected  id " + ConnectionId)
    End Sub

    Private Sub Reconnect()
        If Ae Is Nothing Then
            Exit Sub
        End If
        AddInLogWindow("...." + ConnectionId + " Reconnecting", Color.Orange)
        'This intimates user, if connection goes into reconnection mode 
        AddTraderObject(ConnectionId)
        Stat = ConnectionStatus.RECONNECTING
        While ((mDoNotConnect = False) And Not Logon())
            logDisconnect = False
            Thread.Sleep(Keys.reconnectInterval * 1000)
        End While
    End Sub

    Private Sub marketdata_updated(ByVal mdata As TradingInterface.FillMarketData) Handles Ae.MarketDataUpdated
        RaiseEvent MarketDataUpdated(mdata)
    End Sub

    Private Sub trader_order_placed() Handles Ae.OrderPlaced
        Try
            Util.WriteDebugLog(" .... _OrderPlaced - Show Alerts")
            If Form1.GetSingletonOrderform().InvokeRequired Then
                Form1.GetSingletonOrderform().Invoke(New MethodInvoker(AddressOf Form1.GetSingletonOrderform().ShowAlerts))
            Else
                Form1.GetSingletonOrderform().ShowAlerts()
            End If
        Catch ex As Exception
            Util.WriteDebugLog(" .... _OrderPlaced - ERROR " + ex.Message)
        End Try
    End Sub

    Private Sub trader_Executed() Handles Ae.Executed
        Try
            'Util.WriteDebugLog(" .... _Executed - Show Orders")

        Catch ex As Exception
            Util.WriteDebugLog(" .... _Executed - ERROR " + ex.Message)
        End Try
    End Sub

    Private Sub trader_Rejected(ByVal reason As String) Handles Ae.Rejected
        If (logDisconnect = True) Then
            AddInLogWindow(ConnectionId + " Disconnected", Color.Red)
        End If
        If (IsMarketDataConnection = True) Then
            Util.WriteDebugLog(" ....MarketData Connection trouble id " + ConnectionId)
        Else
            Util.WriteDebugLog(" ....Trade Connection trouble id " + ConnectionId)
        End If
        If Not reason Is Nothing Then
            Util.WriteDebugLog(" .... reason: " + reason)
        End If
        Try
            Disconnect(reason)
            'If Keys.ExchangeServer = ExchangeServer.Icap Then 
            '    Stat = ConnectionStatus.DISCONNECTED     
            '    ConnectHTRemove(ConnectionId)     
            'End If        
        Catch ex As Exception
            Util.WriteDebugLog(" .... ERROR " + ex.Message)
        End Try
    End Sub

    Private Sub WriteToLog(ByVal msg As String) Handles Ae.WriteToLogEvent
        AddInLogWindow(msg, Color.SlateBlue)
        Util.WriteDebugLog(msg)
    End Sub

    'Private Sub trader_HeartBeatReceived(ByVal SeqNo As Integer) Handles Ae.HeartBeatReceived 'HeartBeatCallback
    '    Form1.GetSingletonOrderform().SetHeartBeatFlag(SeqNo)
    'End Sub

    'pwreset
    Private Sub trader_Password_reset_response_received(ByVal UserName As String, ByVal UserStatus As Integer, ByVal UserStatusText As String) Handles Ae.Password_reset_response_received
        Form1.GetSingletonOrderform().Password_Reset_Response(UserName, UserStatus, UserStatusText)
    End Sub

    Private Sub trader_Connected() Handles Ae.Connected
        logDisconnect = True
        AddInLogWindow(ConnectionId + " Connected", Color.Green)
        AddTraderObject(ConnectionId)
        If (IsMarketDataConnection = True) Then
            Util.WriteDebugLog(" ....MarketData Connection established id " + ConnectionId)
        Else
            Util.WriteDebugLog(" ....Trade Connection established id " + ConnectionId)
        End If

        Try
            Stat = ConnectionStatus.CONNECTED
            ''If TC running under Test mode then need not update the GUI....
            If Not Keys.TESTMODE Then EServerDependents.UpdateMarketDataButtons(ConnectionStatus.CONNECTED)
            If (Keys.ExchangeServer <> ExchangeServer.CurrenEx And Keys.ExchangeServer <> ExchangeServer.Icap And Keys.ExchangeServer <> ExchangeServer.Dukascopy And Keys.ExchangeServer <> ExchangeServer.FxIntegral) Then
                If (Not reconnectThread Is Nothing) Then
                    If reconnectThread.IsAlive = True Then reconnectThread.Abort()
                End If
                'it creates the problem at the time of multiple connection to dbfx
                If (Keys.ExchangeServer <> ExchangeServer.DBFX And Keys.ExchangeServer <> ExchangeServer.Gain) Then
                    If (Not IsMarketDataConnection) Then
                        UnSubscribeMarketData()
                    End If
                End If
            End If
            If (alertQ.Count > 0) Then
                ProcessQueue()
            End If
        Catch ex As Exception
            Util.WriteDebugLog(" .... ERROR " + ex.Message)
        End Try
    End Sub

    Private Sub initiateReconnect()
        Try
            If Not reconnectThread Is Nothing Then
                If Not reconnectThread.IsAlive Then
                    'Garbage cleaning. We need dispose any threads created earlier.
                    reconnectThread.Abort()
                    reconnectThread = Nothing
                End If
            End If

            If (reconnectThread Is Nothing) Then
                reconnectThread = New Thread(AddressOf Reconnect)
                reconnectThread.Start()
                Stat = ConnectionStatus.RECONNECTING
            End If
            'Already the reconnect attempt is on. Nothing to do, except wait...
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Util.WriteDebugLog("--- Problem with Reconnect id " + ConnectionId + ex.Message)
        End Try
    End Sub
    Private Sub Ae_OpenPosition(ByVal openValue As String, ByVal _Instrument As String, ByVal _UserId As String) Handles Ae.OpenPosition
        RaiseEvent OpenPosition(openValue, _Instrument, _UserId)
    End Sub

    ''' <summary>
    '''This method is used to set TC into Test mode. 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetTestMode()
        Keys.TESTMODE = True
    End Sub
    ''' <summary>
    '''This private method is used to diaplay the status of user activity on GUI logwindow
    '''Here we check for the condition, Where TC is running under test mode or normal mode
    '''If TC under test mode then we ignore the message or else print on the logwindow in GUI...
    ''' </summary>
    ''' <param name="message">Message to be display on the Logwindow</param>
    ''' <param name="color">Text color for message to be display window</param>
    ''' <remarks></remarks>
    Private Sub AddInLogWindow(ByVal message As String, ByVal color As Color)
        If Not Keys.TESTMODE Then Form1.GetSingletonOrderform().AddInLogWindow(message, color)
    End Sub


    ''' <summary>
    '''This method is to add the trader Object and display on the TC GUI
    '''In case of Test mode; Need to add trader object and show it on the GUI... 
    ''' </summary>
    ''' <param name="ConnectionId">Trader connection id that to add in connection list</param>
    ''' <remarks></remarks>
    Private Sub ConnectHTRemove(ByVal ConnectionId As String)
        If Not Keys.TESTMODE Then Form1.GetSingletonOrderform.ConnectHT.Remove(ConnectionId)
    End Sub

    ''' <summary>
    '''This method is used when user enter the wrong password then prompt him again the Login form 
    ''' If TC is under test mode then you don't have any Form object so you just  need to ignore it...
    ''' </summary>
    ''' <param name="marketDataConnectionStatus">Boolean value that gives you status of marketdata connected or not</param>
    ''' <remarks></remarks>
    Private Sub ReconnectRequest(ByVal marketDataConnectionStatus As Boolean)
        If Not Keys.TESTMODE Then Form1.GetSingletonOrderform.ReconnectRequest(marketDataConnectionStatus)
    End Sub


    ''' <summary>
    ''' This method is add trader object to collection, in case of multiple connection we use this collection object....
    ''' </summary>
    ''' <param name="ConnectionId">Trader object: current user object</param>
    ''' <remarks></remarks>
    Private Sub AddTraderObject(ByVal ConnectionId As String)
        If Not Keys.TESTMODE Then Form1.GetSingletonOrderform.AddTraderObject(ConnectionId, Me)
    End Sub

End Class
