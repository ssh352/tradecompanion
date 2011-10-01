Imports System.IO
Imports System.Configuration
Imports System.Runtime.InteropServices

Public Class AlertsManager
    Private m_Confirmation As Boolean = True
    Public Const ACTION_BUY As Integer = 1
    Public Const ACTION_SELL As Integer = 2
    Public Const ACTION_NONE As Integer = 10
    Public Const STATUS_ACCEPTED As DialogResult = DialogResult.OK
    Public Const STATUS_REJECTED As DialogResult = DialogResult.Cancel
    Public Const STATUS_CANCELED As DialogResult = DialogResult.None
    Public Const TYPE_EQUITY As Integer = 1
    Public Const TYPE_FUTURE As Integer = 2
    Public Const TYPE_IOC As Integer = 3
    Public Const TYPE_GTC As Integer = 1
    Const SW_SHOWNOACTIVATE As Int32 = 4    '<DllImport("User32.dll")> Public Shared Function ShowWindow(ByVal hWnd As IntPtr, ByVal cmdShow As Int32) As Integer
    'End Function
    ' handle to window
    ' placement-order handle
    ' horizontal position
    ' vertical position
    ' width
    ' height
    '<DllImport("User32.dll")> Public Shared Function SetWindowPos(ByVal hWnd As IntPtr, ByVal hWndInsertAfter As Int32, ByVal X As Int32, ByVal Y As Int32, ByVal cx As Int32, ByVal cy As Int32, _
    ' ByVal uFlags As Int32) As Boolean
    '    ' window-positioning options
    'End Function

    'Const SW_SHOWNOACTIVATE As Int32 = 4
    'Const SWP_NOACTIVATE As Int32 = 16
    'Const HWND_TOPMOST As Int32 = -1


    Public Function ProcessAlert(ByVal fn As String, ByVal keys As SettingsHome) As NewAlert
        Util.WriteDebugLog(" ... Captured " + fn)
        Dim Fs As FileStream = Nothing
        While IsNothing(Fs)
            Try
                Fs = New FileStream(fn, FileMode.Open, FileAccess.Read)
            Catch ex As Exception
                Fs = Nothing
            End Try
        End While
        Dim result As Windows.Forms.DialogResult
        Dim mapped As NewAlert = Nothing
        Try
            Dim tw As StreamReader = New StreamReader(Fs)
            Dim s As String = tw.ReadLine
            tw.Close()

            Util.WriteDebugLog(" ... Processing TS alert " + s)

            Dim fnnew As String = Replace(fn, ".req", ".rex")
            Rename(fn, fnnew)

            Dim a As NewAlert = New NewAlert(s, keys)
            'MessageBox.Show("before map")
            mapped = MapedAlert(a, keys.UseDefaultTradeSize, keys.UseSymbolMapping)
            'MessageBox.Show("after map")
            'check here for duplicate alerts
            'same symbol, exchange, number of contracts, action type and the same timestamp 
            Dim ah As AlertsHome = New AlertsHome
            Dim filterAlerts As Boolean = False
            If (keys.FilterAlerts = True And SettingsHome.getInstance().Platform = "TradeStation") Then
                'MessageBox.Show("before filter")
                filterAlerts = ah.IsDuplateAlert(mapped)
                'MessageBox.Show("after filter result: " & filterAlerts.ToString)
            End If
            If (a.exch = "NA" Or Not filterAlerts) Then 'NA comes from TS2000i and probably will not have duplicate alerts
                'MessageBox.Show("execution")
                Select Case keys.ExecutionMode
                    Case SettingsHome.EXECUTION_NONE
                        AddInLogWindow("Alert rejected: no execution mode", Color.Red)
                        AddInLogWindow("....Details: " + mapped.symbol + " " + mapped.contracts.ToString(), Color.Red)
                        result = STATUS_REJECTED
                    Case SettingsHome.EXECUTION_MANUAL
                        Dim allow As Boolean = AllowExecution(a.symbol, a.senderID)
                        If (allow) Then
                            Dim AlertWindow As Alert = New Alert(mapped)
                            result = AlertWindow.ShowDialog()
                            mapped = AlertWindow.alertdata
                            If mapped.status = STATUS_CANCELED Then
                                result = STATUS_REJECTED
                                AddInLogWindow("Alert rejected: cancelled by user", Color.Red)
                                AddInLogWindow("....Details: " + mapped.symbol + " " + mapped.contracts.ToString(), Color.Red)
                            End If
                        Else
                            result = STATUS_REJECTED
                        End If

                    Case SettingsHome.EXECUTION_AUTO
                        Dim allow As Boolean = AllowExecution(a.symbol, a.senderID)
                        'MessageBox.Show("auto")
                        If (allow) Then
                            If mapped.status = STATUS_CANCELED Then
                                result = STATUS_REJECTED
                            Else
                                '  MessageBox.Show("accepted")
                                result = STATUS_ACCEPTED
                            End If
                        Else
                            result = STATUS_REJECTED
                        End If
                End Select
            Else
                result = STATUS_REJECTED
                Util.WriteDebugLog("..Duplicate Alert-")
                AddInLogWindow("Alert rejected: duplicate alert", Color.Red)
                AddInLogWindow("....Details: " + mapped.symbol + " " + mapped.contracts.ToString(), Color.Red)

            End If

            Util.WriteDebugLog(" .... TradeCompanion alert: ")
            Util.WriteDebugLog("             symbol-" + mapped.symbol)
            Util.WriteDebugLog("           exchange-" + mapped.exch)
            Util.WriteDebugLog("          monthyear-" + mapped.month_year)
            Util.WriteDebugLog("             action-" + mapped.actiontype.ToString)
            Util.WriteDebugLog("           quantity-" + mapped.contracts.ToString)
            '  WriteDebugLog("              price-" + mapped.price.ToString)
            Util.WriteDebugLog("             status-" + mapped.status.ToString)


            Dim i As Integer = 0
            mapped.status = result
            'CurrenEx change, we dont need to add alert here, will add it on placing the order.
            'ah.AddAlert(mapped)
            Kill(fnnew)
        Catch ex As Exception
            MsgBox(ex.Message)
            Util.WriteDebugLog(" ... TradeCompanion alert ERROR " + ex.Message)
            Util.WriteDebugLog(" ... Stack Trace " + ex.StackTrace)
            mapped = Nothing
        End Try
        Return mapped
    End Function

    Public Function AllowExecution(ByVal symbol As String, ByVal senderID As String) As Boolean
        ' Allow only active symbols and connections
        Dim ah As New AlertsHome
        Dim dsSymbol As DataSet = ah.getSymbolMap(symbol)
        Dim dvSymbol As DataView = dsSymbol.Tables(0).DefaultView()
        dvSymbol.RowFilter = "TSSymbol='" + symbol + "' AND Active='True'"

        Dim dsCon As DataSet = ah.GetIdMap()
        Dim dvCon As DataView = dsCon.Tables(0).DefaultView()
        dvCon.RowFilter = "TradeStationID='" + senderID + "' AND Active='True'"

        If (dvCon.Count < 1) Then
            Util.WriteDebugLog("--- There are no active connections...check connection mapping")
            AddInLogWindow("...There are no active connections...check connection mapping", Color.Red)
            Return False
        End If

        If (dvSymbol.Count < 1) Then
            Util.WriteDebugLog("--- There are no active symbols...check symbol mapping")
            AddInLogWindow("...There are no active symbols...check symbol mapping", Color.Red)
            Return False
        End If
        Return True
    End Function

    Public Function MapedAlert(ByVal a As NewAlert, Optional ByVal useDefaultSize As Boolean = False, Optional ByVal useMappedSymbol As Boolean = False) As NewAlert
        Dim ah As AlertsHome = New AlertsHome
        Dim m As NewAlert = New NewAlert
        Dim ds As DataSet
        Try
            ds = ah.getSymbolMap(a.symbol.Trim, a.exch.Trim, a.currency, a.contracts)
            If ds.Tables(0).Rows.Count = 0 Then
                a.month_year = " "
                If useMappedSymbol Then
                    a.status = STATUS_CANCELED
                    AddInLogWindow("Alert rejected: invalid symbol mapping", Color.Red)
                    AddInLogWindow("....Details: " + a.symbol + " " + a.contracts.ToString(), Color.Red)
                    Return a
                Else
                    Return a
                End If
            End If
            If ds.Tables(0).Rows.Count > 1 Then
                Throw New Exception("Duplicate Mapping for Symbol " + a.symbol + "(" + a.exch + ")")
            End If
            Dim r As DataRow = ds.Tables(0).Rows(0)
            If useMappedSymbol Then
                m.symbol = r.Item("TradeSymbol").ToString.Trim
                m.exch = r.Item("TradeExchange").ToString.Trim
                m.currency = r.Item("TradeCurrency").ToString.Trim
            Else
                m.symbol = a.symbol.Trim
                m.exch = a.exch.Trim
            End If
            m.actiontype = a.actiontype
            m.status = m.status
            m.timestamp = a.timestamp
            If useDefaultSize Then
                ' MessageBox.Show("DefaultSize")
                m.contracts = r.Item("TradeSize")
                'changed by CG on 28/01/08 - multiplies actual default size in config by number of contracts 
                'sent by TradeStation so that amount is always proportional
                '   MessageBox.Show("Map Contracts = " & m.contracts.ToString)
                '  MessageBox.Show("TS Contracts = " & a.contracts.ToString)
                m.contracts *= a.contracts
                ' MessageBox.Show(m.contracts.ToString)
                'if no config is found or it is set to zero than use the number 
                If m.contracts = 0 Then
                    '    MessageBox.Show("contracts = 0")
                    m.contracts = a.contracts * 100000
                End If

            Else
                'mutliply it with lot size
                m.contracts = a.contracts * 100000
            End If
            m.tradeType = a.tradeType
            m.tsOpenPosition = a.tsOpenPosition
            If Not r.IsNull("MonthYear") Then m.month_year = r.Item("MonthYear")
            If Not r.IsNull("SecurityType") Then m.securityType = r.Item("SecurityType")
            If m.month_year = "" Then
                m.month_year = " "
            End If
            'Not required for currenEx. If m.month_year.Trim = "" Then m.status = STATUS_CANCELED
            m.chartIdentifier = a.chartIdentifier
            m.senderID = a.senderID
            Return m
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    '''Method is used to diaplay the status of user activity on GUI logwindow
    ''' </summary>
    ''' <param name="message">Message to be display on the Logwindow</param>
    ''' <param name="color">Text color for message to be display window</param>
    ''' <remarks></remarks>
    Private Sub AddInLogWindow(ByVal message As String, ByVal color As Color)
        If Not SettingsHome.getInstance.TESTMODE Then Form1.GetSingletonOrderform().AddInLogWindow(message, color)
    End Sub

    Public Class NewAlert
        Public exch As String
        Public symbol As String
        Public contracts As Double
        Public actiontype As Integer
        Public status As Integer
        Public securityType As Integer
        Public month_year As String
        Public price As String ''Test1234
        Public orderID As String
        Public timestamp As String
        Public statusMessage As String
        Public currency As String
        Public execOrderId As String
        Public tradeType As Integer
        Public chartIdentifier As Integer
        Public senderID As String
        Public localtimestamp As DateTime
        Public uID As String 'mdbx
        Public tsOpenPosition As Double = 0

        Public Sub New()
            exch = ""
            symbol = ""
            contracts = 1
            actiontype = ACTION_BUY
            status = STATUS_REJECTED
            securityType = TYPE_EQUITY
            month_year = " "
            orderID = ""
            timestamp = ""
            statusMessage = ""
            currency = ""
            execOrderId = ""
            senderID = ""
            localtimestamp = DateTime.Now
            tsOpenPosition = 0
            'tradeType = 3
        End Sub

        Public Sub New(ByVal alertstring As String, ByVal keys As SettingsHome)
            Dim a() As String
            a = Split(alertstring.Trim(), " ")
            exch = a(2)
            symbol = a(3)
            Dim action As String = a(4)
            localtimestamp = DateTime.Now
            Select Case action.ToUpper
                Case "BUY"
                    actiontype = ACTION_BUY
                Case "SELL"
                    actiontype = ACTION_SELL
                Case "BUYLIMIT"
                    actiontype = ACTION_BUY
                Case "BUYSTOP"
                    actiontype = ACTION_BUY
                Case "SELLLIMIT"
                    actiontype = ACTION_SELL
                Case "SELLSTOP"
                    actiontype = ACTION_SELL
            End Select
            contracts = CDbl(a(5))

            'timestamp = "20" + a(0).Substring(1) + "-" + a(1).Trim

            If (SettingsHome.getInstance().Platform = "TradeStation") Then 'TradeStation
                chartIdentifier = CInt(a(6))
                senderID = a(7)
                timestamp = "20" + a(0).Substring(1) + "-" + a(1).Trim
                If (a.Length > 8) Then  'need to remove when finalized giri
                    tsOpenPosition = CDbl(a(8))
                End If

            ElseIf (SettingsHome.getInstance().Platform = "MetaTrader") Then 'MetaTrader4
                price = CDbl(a(6))
                chartIdentifier = CInt(a(7))
                For counter As Integer = 8 To a.Length - 1 Step 1
                    senderID = senderID + a(counter) + " "
                Next counter
                senderID = senderID.Trim()
                timestamp = "20" + a(0).Substring(1) + "-" + a(1).Trim
            ElseIf (SettingsHome.getInstance().Platform = "NeuroShell") Then 'Neuroshell
                chartIdentifier = CInt(a(6))
                For count As Integer = 7 To a.Length - 1 Step 1
                    senderID = senderID + a(count) + " "
                Next
                timestamp = a(0).Trim + "-" + a(1).Trim
            End If

            'set the tradetype to GTC if the mode of execution is autmatic and  amount sxceeding 10 million
            Dim sTradeType As New SettingsTrade
            sTradeType.getSettings()
            Select Case keys.ExecutionMode
                Case SettingsHome.EXECUTION_NONE
                    tradeType = TYPE_IOC
                Case SettingsHome.EXECUTION_MANUAL
                    Select Case sTradeType.TradeTypeManual
                        Case 1 'GTC
                            tradeType = TYPE_GTC
                        Case 3 'IOC
                            tradeType = TYPE_IOC
                    End Select
                Case SettingsHome.EXECUTION_AUTO
                    'if trade size is greater then 10 million
                    If (contracts > 10000000) Then
                        Select Case sTradeType.TradeTypeAutoOver10Mil
                            Case 1 'GTC
                                tradeType = TYPE_GTC
                            Case 3 'IOC
                                tradeType = TYPE_IOC
                        End Select
                    Else
                        Select Case sTradeType.TradeTypeAuto
                            Case 1 'GTC
                                tradeType = TYPE_GTC
                            Case 3 'IOC
                                tradeType = TYPE_IOC
                        End Select
                    End If
            End Select

        End Sub

    End Class

End Class
