VERSION 5.00
Object = "{0A77CCF5-052C-11D6-B0EC-00B0D074179C}#1.0#0"; "Tws.ocx"
Object = "{D7286017-9FA8-426E-9C8A-70B64FEB5F47}#2.0#0"; "NSTOrdersAPI.ocx"
Begin VB.Form Form1 
   Caption         =   "NST TradePump IB Example"
   ClientHeight    =   1005
   ClientLeft      =   60
   ClientTop       =   450
   ClientWidth     =   5835
   Icon            =   "NST TradePump IB Example.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   1005
   ScaleWidth      =   5835
   StartUpPosition =   3  'Windows Default
   WindowState     =   1  'Minimized
   Begin NSTOrdersAPI.NSTOrders NSTOrders1 
      Left            =   360
      Top             =   240
      _ExtentX        =   1191
      _ExtentY        =   1085
      BrokerageFeedBack=   -1  'True
   End
   Begin TWSLib.Tws Tws1 
      Height          =   375
      Left            =   1320
      TabIndex        =   0
      Top             =   240
      Width           =   2295
      _Version        =   65536
      _ExtentX        =   4048
      _ExtentY        =   661
      _StockProps     =   0
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

' *********************************************************
' NeuroShell Trader Order Constants
' *********************************************************
Private Const MARKET_ORDER = 1
Private Const STOP_ORDER = 2
Private Const LIMIT_ORDER = 3
Private Const STOPLIMIT_ORDER = 4
Private Const MARKETCLOSE_ORDER = 5

Private Const LONGENTRY_ACTION = 1
Private Const SHORTENTRY_ACTION = 2
Private Const LONGEXIT_ACTION = 3
Private Const SHORTEXIT_ACTION = 4

Private Connected As Boolean


' *********************************************************
' Interactive Brokers Specific Variables
' *********************************************************
Private InteractiveBrokers_NextValidOrderId As Long
Private InteractiveBrokers_ConnectError As Long
Private InteractiveBrokers_ConnectingFlag As Boolean
Private InteractiveBrokers_ContractDetailsError As Long

Private Sub NSTOrders1_Logon(ByVal Account As String, ByVal Password As String, ByRef LoggedOn As Long)
    ' *********************************************************
    ' NeuroShell Trader Event requesting brokerage logon
    ' *********************************************************
    
    ' exit if already connected to brokerage
    If Connected Then Exit Sub
       
    Me.Show
    
    ' *****************************************
    ' Put code to connect to the brokerage here
    ' *****************************************
    
    ' Display message in NeuroShell Trader describing what user needs to do before Interactive Brokers connection can occur
    Dim txt As String
    txt = "About to establish a connection with Interactive Brokers."
    txt = txt + Chr$(13) + Chr$(13)
    txt = txt + "1) Make sure the IB Trader Workstation is loaded and running before pressing OK."
    txt = txt + Chr$(13) + Chr$(13)
    txt = txt + "2) Verify client support is configured in the IB Trader Workstation.  To configure client support in the IB Trader Workstation, go to the 'Configure' menu, select 'API' and check 'Enable ActiveX and Socket Clients'."
    txt = txt + Chr$(13) + Chr$(13)
    txt = txt + "3) MOST IMPORTANT - After pressing OK, choose Yes on the 'Accept incoming connection attempt' prompt in the IB Trader Workstation." + Chr$(13) + "      *** NOTE: THE IB TRADER WORKSTATION MAY BE BEHIND NEUROSHELL TRADER WINDOW IN SOME CASES. ***"
    NSTOrders1.Message txt    ' Display connection message in NeuroShell Trader
    
    ' Connect to Interactive Brokers
    InteractiveBrokers_NextValidOrderId = 1   ' Init InteractiveBrokers_NextValidOrderId to 1 since Interactive Brokers wants sequential order id's
    InteractiveBrokers_ConnectError = 0
    InteractiveBrokers_ConnectingFlag = True
    Tws1.Connect "", 7496, 2  ' Send Interactive Brokers connect command
    InteractiveBrokers_ConnectingFlag = False
    If InteractiveBrokers_ConnectError = 0 Then
        Connected = True
    Else
        LoggedOn = False  ' signal to NeuroShell Trader that Brokerage logon failed
    End If
End Sub

Private Sub NSTOrders1_NewOrder(OrderId As String, ByVal Action As Long, ByVal Shares As Long, ByVal Ticker As String, ByVal OrderType As Long, ByVal StopPrice As Double, ByVal LimitPrice As Double, ByVal Duration As String, ByVal exchange As String, ByVal secType As String, ByVal ChartName As String, ByVal StrategyName As String, ByVal ChartTimeStamp As String)
    ' *****************************************************************
    ' NeuroShell Trader Event signaling a new order needs to be placed
    ' *****************************************************************
    
    Dim ActionString As String
    Dim OrderTypeString As String
    Dim price As Double
    Dim AuxPrice As Double
    Dim ShortTradingFlag As Boolean
    
    ' exit if not connected to brokerage
    If Not Connected Then Exit Sub
    
    ' Convert NeuroShell Trader Action into appropriate Interactive Brokers ActionString
    Select Case Action
        Case LONGENTRY_ACTION:  ActionString = "BUY"
        Case SHORTENTRY_ACTION: ActionString = "SELL" 'If SecType = "STK" Then ActionString = "SSHORT" Else ActionString = "SELL"
        Case LONGEXIT_ACTION:   ActionString = "SELL"
        Case SHORTEXIT_ACTION:  ActionString = "BUY"
    End Select

    ' Convert NeuroShell Trader OrderType into appropriate Interactive Brokers OrderTypeString
    Select Case OrderType
        Case MARKET_ORDER:      OrderTypeString = "MKT"
        Case STOP_ORDER:        OrderTypeString = "STP"
                                AuxPrice = CDbl(CStr(StopPrice))
        Case LIMIT_ORDER:       OrderTypeString = "LMT"
                                price = CDbl(CStr(LimitPrice))
        Case STOPLIMIT_ORDER:   OrderTypeString = "STPLMT"
                                price = LimitPrice
                                AuxPrice = StopPrice
        Case MARKETCLOSE_ORDER: OrderTypeString = "MKTCLS"
    End Select
    
    ' send order to Interactive Brokers
    Tws1.placeOrder2 InteractiveBrokers_NextValidOrderId, ActionString, Shares, Ticker, secType, exchange, "", "", OrderTypeString, price, AuxPrice, "", "", "", "", "", ""
    
    ' return Interactive Broker's orderid to NeuroShell Trader so that subsequent cancel/modify requests are sent with Interactive Broker's orderid instead of NeuroShell's orderid
    OrderId = CStr(InteractiveBrokers_NextValidOrderId)
    
    ' increment interactive broker's next valid order id
    InteractiveBrokers_NextValidOrderId = InteractiveBrokers_NextValidOrderId + 1
End Sub

Private Sub NSTOrders1_CancelOrder(OrderId As String)
    ' ***************************************************************
    ' NeuroShell Trader Event signaling an order nees to be canceled
    ' ***************************************************************
    
    ' exit if not connected to brokerage
    If Not Connected Then Exit Sub
    
    ' send cancel to Brokerage
    Tws1.cancelOrder CLng(OrderId)
End Sub

Private Sub NSTOrders1_ModifyOrder(OrderId As String, StopPrice As Double, LimitPrice As Double)
    ' ****************************************************************
    ' NeuroShell Trader Event signaling an order needs to be modified
    ' ****************************************************************
End Sub

Private Sub NSTOrders1_Logoff()
    ' *********************************************************
    ' NeuroShell Trader Event requesting brokerage logoff
    ' *********************************************************
    
    ' Exit if not still connected to brokerage
    If Not Connected Then Exit Sub
    
    ' disconnect from Broker
    Tws1.disconnect
    
    ' clear connected flag
    Connected = False
End Sub

Private Sub NSTOrders1_Unload()
    ' ****************************************************************************************
    ' NeuroShell Trader Event triggered when NeuroShell Trader wants the application to unload
    ' ****************************************************************************************
    
    Unload Me
End Sub

Private Sub NSTOrders1_VerifyTicker(ByVal Ticker As String, ByVal exchange As String, ByVal secType As String, ByRef Valid As Long)
    ' ****************************************************************************************
    ' NeuroShell Trader Event triggered when NeuroShell Trader wants a ticker to be verified
    ' ****************************************************************************************
    
    ' exit if not connected to brokerage
    If Not Connected Then Exit Sub
    
    ' set contract details error to -1 to signify no return value set yet
    InteractiveBrokers_ContractDetailsError = -1
    
    ' request ticker details from Interactive Brokers
    Tws1.reqContractDetails2 Ticker, secType, exchange, ""
    
    ' wait for return message from Interactive Brokers
    While InteractiveBrokers_ContractDetailsError = -1
        DoEvents
    Wend
    
    If InteractiveBrokers_ContractDetailsError <> 0 Then
        ' Signal NeuroShell Trader that ticker not valid by setting Valid=0
        Valid = 0
    End If
End Sub

Private Sub Tws1_connectionClosed()
    ' ****************************************************************************************
    ' Interactive Brokers event that is triggered when TWS closes the sockets connection with
    '    the ActiveX control, or when TWS is shut down.
    ' ****************************************************************************************
    
    ' set flag that not connect to brokerage
    Connected = False
    
    ' Display message in NeuroShell Trader that the Interactive Brokers connection was closed
    NSTOrders1.Message "The IB Trader Workstation was either shutdown or the IB Trader Workstation closed the sockets connection.  Trades will not be sent to Interactive Brokers until a link with the IB Trader Workstation is reestablished."
    
    ' shut down application
    Unload Me
End Sub

Private Sub Tws1_contractDetails(ByVal symbol As String, ByVal secType As String, ByVal expiry As String, ByVal strike As Double, ByVal right As String, ByVal exchange As String, ByVal curency As String, ByVal localSymbol As String, ByVal marketName As String, ByVal tradingClass As String, ByVal conid As Long, ByVal minTick As Double, ByVal priceMagnifier As Long, ByVal multiplier As String, ByVal orderTypes As String, ByVal validExchanges As String)
    ' ****************************************************************************************
    ' Interactive Broker's event that is fired when the reqContractDetails()
    '    or reqContractDetails2() methods are invoked
    ' ****************************************************************************************

    ' Set flag that contracts details request was sucessful
    InteractiveBrokers_ContractDetailsError = 0
End Sub

Private Sub Tws1_errMsg(ByVal id As Long, ByVal errorCode As Long, ByVal errorMsg As String)
    ' ****************************************************************************************
    ' Interactive Broker's error notification event
    ' ****************************************************************************************
    
    If errorCode = 200 Then
        ' Set flag that contracts details request was not sucessful
        InteractiveBrokers_ContractDetailsError = 1
    ElseIf InteractiveBrokers_ConnectingFlag And (errorCode = 502 Or errorCode = 508) Then
        ' Set flag that Interactive Brokers connection failed
        InteractiveBrokers_ConnectError = 1
        
        ' Send message to be displayed in NeuroShell Trader
        NSTOrders1.Message "NeuroShell Trader failed to established a connection with Interactive Brokers.  Either the TWS isn't already running or it isn't setup to accept client requests"
    ElseIf errorCode >= 2103 And errorCode <= 2108 Then
        ' ignore the following Interactive Brokers informational error codes
        '   2103 A market data farm is disconnected.
        '   2104 A market data farm is connected.
        '   2105 A historical data farm is disconnected.
        '   2106 A historical data farm is connected.
        '   2107 A historical data farm connection has become inactive but should be available upon demand.
        '   2108 A market data farm connection has become inactive but should be available upon demand.
    ElseIf errorCode = 503 And InteractiveBrokers_ConnectError Then
        '   Ignore Interactive Brokers error code 503 (TWS is out of date and must be upgraded) because this occurs after a 508 which occurs if the user presses NO to accept incoming connection
    ElseIf errorCode = 506 Then
        ' Ignore Interactive Brokers error code 506 (Unexplained Zero Bytes Read) which happens when IBTWS closed before Trader
    ElseIf errorCode = 202 Then
        ' ignore Interactive Brokers errro code 202 (cancled msg) because it would show up everytime a limit/stop order is cancelled and replaced with a different order
    Else
        ' Send all other messages to be displayed in NeuroShell Trader
        NSTOrders1.Message errorMsg
    End If
End Sub

Private Sub Tws1_nextValidId(ByVal id As Long)
    ' **************************************************************************************************
    ' Interactive Broker's event that occurs after a successful connection to the Interactive Broker TWS
    '
    ' id = next available order ID received from Interactive Brokers TWS upon connection.
    '      Increment all successive orders by one based on this ID.
    ' **************************************************************************************************
    
    ' store Interactive Broker's next valid order id
    InteractiveBrokers_NextValidOrderId = id
End Sub

Private Sub Tws1_orderStatus(ByVal id As Long, ByVal status As String, ByVal filled As Long, ByVal remaining As Long, ByVal avgFillPrice As Double, ByVal permId As Long, ByVal parentId As Long, ByVal lastFillPrice As Double, ByVal clientId As Long)
    ' **************************************************************************************************
    ' Interactive Broker's event that occurs whenever the status of an order changes.
    ' **************************************************************************************************

    If status = "Filled" And filled > 0 And lastFillPrice <> 0 Then
        ' Tell NeuroShell Trader that order was filled
        NSTOrders1.OrderFilled CStr(id), filled, avgFillPrice
    ElseIf status = "Cancelled" Then
        ' Tell NeuroShell Trader that order was canceled
        NSTOrders1.OrderCanceled CStr(id)
    ElseIf status = "Inactive" Then
        ' Tell NeuroShell Trader that order was made inactive (usually due to some error in the order parameters or inability of brokerage execute the order)
        NSTOrders1.OrderInactive CStr(id)
    End If
End Sub

