Option Strict Off
Option Explicit On
Friend Class Form1
	Inherits System.Windows.Forms.Form
	
	' *********************************************************
	' NeuroShell Trader Order Constants
	' *********************************************************
	Private Const MARKET_ORDER As Short = 1
	Private Const STOP_ORDER As Short = 2
	Private Const LIMIT_ORDER As Short = 3
	Private Const STOPLIMIT_ORDER As Short = 4
	Private Const MARKETCLOSE_ORDER As Short = 5
	
	Private Const LONGENTRY_ACTION As Short = 1
	Private Const SHORTENTRY_ACTION As Short = 2
	Private Const LONGEXIT_ACTION As Short = 3
	Private Const SHORTEXIT_ACTION As Short = 4
	
	Private Connected As Boolean
	
	
	' *********************************************************
	' Interactive Brokers Specific Variables
	' *********************************************************
	Private InteractiveBrokers_NextValidOrderId As Integer
	Private InteractiveBrokers_ConnectError As Integer
	Private InteractiveBrokers_ConnectingFlag As Boolean
	Private InteractiveBrokers_ContractDetailsError As Integer
	
	Private Sub NSTOrders1_Logon(ByVal eventSender As System.Object, ByVal eventArgs As AxNSTOrdersAPI.__NSTOrders_LogonEvent) Handles NSTOrders1.Logon
		' *********************************************************
		' NeuroShell Trader Event requesting brokerage logon
		' *********************************************************
       
		' exit if already connected to brokerage
        If Connected Then Exit Sub
		
		Me.Show()
		
		' *****************************************
		' Put code to connect to the brokerage here
		' *****************************************
		
		' Display message in NeuroShell Trader describing what user needs to do before Interactive Brokers connection can occur
		Dim txt As String
        txt = "About to establish a connection with Trade Companion."
		txt = txt & Chr(13) & Chr(13)
		txt = txt & "1) Make sure the IB Trader Workstation is loaded and running before pressing OK."
		txt = txt & Chr(13) & Chr(13)
		txt = txt & "2) Verify client support is configured in the IB Trader Workstation.  To configure client support in the IB Trader Workstation, go to the 'Configure' menu, select 'API' and check 'Enable ActiveX and Socket Clients'."
		txt = txt & Chr(13) & Chr(13)
		txt = txt & "3) MOST IMPORTANT - After pressing OK, choose Yes on the 'Accept incoming connection attempt' prompt in the IB Trader Workstation." & Chr(13) & "      *** NOTE: THE IB TRADER WORKSTATION MAY BE BEHIND NEUROSHELL TRADER WINDOW IN SOME CASES. ***"
        ' Display connection message in NeuroShell Trader
        NSTOrders1.Message(txt)

		' Connect to Interactive Brokers
		InteractiveBrokers_NextValidOrderId = 1 ' Init InteractiveBrokers_NextValidOrderId to 1 since Interactive Brokers wants sequential order id's
		InteractiveBrokers_ConnectError = 0
		InteractiveBrokers_ConnectingFlag = True
        'Tws1.connect("", 7496, 2) ' Send Interactive Brokers connect command
		InteractiveBrokers_ConnectingFlag = False
		If InteractiveBrokers_ConnectError = 0 Then
			Connected = True
		Else
            eventArgs.loggedOn = False ' signal to NeuroShell Trader that Brokerage logon failed
        End If

	End Sub
	
	Private Sub NSTOrders1_NewOrder(ByVal eventSender As System.Object, ByVal eventArgs As AxNSTOrdersAPI.__NSTOrders_NewOrderEvent) Handles NSTOrders1.NewOrder
		' *****************************************************************
		' NeuroShell Trader Event signaling a new order needs to be placed
        ' *****************************************************************
        Dim str As String = eventArgs.chartName
        str = str & Chr(13) & Chr(13)
        str = str & eventArgs.orderId & Chr(32) & eventArgs.shares & Chr(32)
        str = str & eventArgs.strategyName

        MessageBox.Show(str)
		Dim ActionString As String
		Dim OrderTypeString As String
		Dim price As Double
		Dim AuxPrice As Double
        Dim ShortTradingFlag As Boolean = True


		' exit if not connected to brokerage
		If Not Connected Then Exit Sub
        MessageBox.Show("Connected ")
		' Convert NeuroShell Trader Action into appropriate Interactive Brokers ActionString
		Select Case eventArgs.Action
			Case LONGENTRY_ACTION : ActionString = "BUY"
			Case SHORTENTRY_ACTION : ActionString = "SELL" 'If SecType = "STK" Then ActionString = "SSHORT" Else ActionString = "SELL"
			Case LONGEXIT_ACTION : ActionString = "SELL"
			Case SHORTEXIT_ACTION : ActionString = "BUY"
		End Select
		
		' Convert NeuroShell Trader OrderType into appropriate Interactive Brokers OrderTypeString
		Select Case eventArgs.OrderType
			Case MARKET_ORDER : OrderTypeString = "MKT"
			Case STOP_ORDER : OrderTypeString = "STP"
				AuxPrice = CDbl(CStr(eventArgs.StopPrice))
			Case LIMIT_ORDER : OrderTypeString = "LMT"
				price = CDbl(CStr(eventArgs.LimitPrice))
			Case STOPLIMIT_ORDER : OrderTypeString = "STPLMT"
				price = eventArgs.LimitPrice
				AuxPrice = eventArgs.StopPrice
			Case MARKETCLOSE_ORDER : OrderTypeString = "MKTCLS"
		End Select
		
		' send order to Interactive Brokers
        'Tws1.placeOrder2(InteractiveBrokers_NextValidOrderId, ActionString, eventArgs.shares, eventArgs.ticker, eventArgs.secType, eventArgs.exchange, "", "", OrderTypeString, price, AuxPrice, "", "", "", "", "", "")
		
		' return Interactive Broker's orderid to NeuroShell Trader so that subsequent cancel/modify requests are sent with Interactive Broker's orderid instead of NeuroShell's orderid
		eventArgs.OrderId = CStr(InteractiveBrokers_NextValidOrderId)
		
		' increment interactive broker's next valid order id
		InteractiveBrokers_NextValidOrderId = InteractiveBrokers_NextValidOrderId + 1
	End Sub
	
	Private Sub NSTOrders1_CancelOrder(ByVal eventSender As System.Object, ByVal eventArgs As AxNSTOrdersAPI.__NSTOrders_CancelOrderEvent) Handles NSTOrders1.CancelOrder
		' ***************************************************************
		' NeuroShell Trader Event signaling an order nees to be canceled
		' ***************************************************************
		
		' exit if not connected to brokerage
		If Not Connected Then Exit Sub
		
		' send cancel to Brokerage
		Tws1.cancelOrder(CInt(eventArgs.OrderId))
	End Sub
	
	Private Sub NSTOrders1_ModifyOrder(ByVal eventSender As System.Object, ByVal eventArgs As AxNSTOrdersAPI.__NSTOrders_ModifyOrderEvent) Handles NSTOrders1.ModifyOrder
		' ****************************************************************
		' NeuroShell Trader Event signaling an order needs to be modified
		' ****************************************************************
	End Sub
	
	Private Sub NSTOrders1_Logoff(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles NSTOrders1.Logoff
		' *********************************************************
		' NeuroShell Trader Event requesting brokerage logoff
		' *********************************************************
        'MessageBox.Show("Logoff method")
		' Exit if not still connected to brokerage
		If Not Connected Then Exit Sub
		
		' disconnect from Broker
        Tws1.disconnect()
		
		' clear connected flag
		Connected = False
	End Sub
	
	Private Sub NSTOrders1_Unload(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles NSTOrders1.Unload
		' ****************************************************************************************
		' NeuroShell Trader Event triggered when NeuroShell Trader wants the application to unload
		' ****************************************************************************************
        'MessageBox.Show("UNload method")
		Me.Close()
	End Sub
	
	Private Sub NSTOrders1_VerifyTicker(ByVal eventSender As System.Object, ByVal eventArgs As AxNSTOrdersAPI.__NSTOrders_VerifyTickerEvent) Handles NSTOrders1.VerifyTicker
		' ****************************************************************************************
		' NeuroShell Trader Event triggered when NeuroShell Trader wants a ticker to be verified
		' ****************************************************************************************
        MessageBox.Show("verify Ticker")
		' exit if not connected to brokerage
		If Not Connected Then Exit Sub
		
		' set contract details error to -1 to signify no return value set yet
		InteractiveBrokers_ContractDetailsError = -1
		
		' request ticker details from Interactive Brokers
        'Tws1.reqContractDetails2(eventArgs.Ticker, eventArgs.secType, eventArgs.exchange, "")
		
		' wait for return message from Interactive Brokers
		While InteractiveBrokers_ContractDetailsError = -1
			System.Windows.Forms.Application.DoEvents()
		End While
		
		If InteractiveBrokers_ContractDetailsError <> 0 Then
			' Signal NeuroShell Trader that ticker not valid by setting Valid=0
			eventArgs.Valid = 0
		End If
	End Sub
	
	Private Sub Tws1_connectionClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Tws1.connectionClosed
		' ****************************************************************************************
		' Interactive Brokers event that is triggered when TWS closes the sockets connection with
		'    the ActiveX control, or when TWS is shut down.
		' ****************************************************************************************
		
		' set flag that not connect to brokerage
		Connected = False
		
		' Display message in NeuroShell Trader that the Interactive Brokers connection was closed
		NSTOrders1.Message("The IB Trader Workstation was either shutdown or the IB Trader Workstation closed the sockets connection.  Trades will not be sent to Interactive Brokers until a link with the IB Trader Workstation is reestablished.")
		
		' shut down application
		Me.Close()
	End Sub
	
	Private Sub Tws1_contractDetails(ByVal eventSender As System.Object, ByVal eventArgs As AxTWSLib._DTwsEvents_contractDetailsEvent) Handles Tws1.contractDetails
		' ****************************************************************************************
		' Interactive Broker's event that is fired when the reqContractDetails()
		'    or reqContractDetails2() methods are invoked
		' ****************************************************************************************
		
		' Set flag that contracts details request was sucessful
		InteractiveBrokers_ContractDetailsError = 0
	End Sub
	
	Private Sub Tws1_errMsg(ByVal eventSender As System.Object, ByVal eventArgs As AxTWSLib._DTwsEvents_errMsgEvent) Handles Tws1.errMsg
		' ****************************************************************************************
		' Interactive Broker's error notification event
		' ****************************************************************************************
		
		If eventArgs.errorCode = 200 Then
			' Set flag that contracts details request was not sucessful
			InteractiveBrokers_ContractDetailsError = 1
		ElseIf InteractiveBrokers_ConnectingFlag And (eventArgs.errorCode = 502 Or eventArgs.errorCode = 508) Then 
			' Set flag that Interactive Brokers connection failed
			InteractiveBrokers_ConnectError = 1
			
			' Send message to be displayed in NeuroShell Trader
			NSTOrders1.Message("NeuroShell Trader failed to established a connection with Interactive Brokers.  Either the TWS isn't already running or it isn't setup to accept client requests")
		ElseIf eventArgs.errorCode >= 2103 And eventArgs.errorCode <= 2108 Then 
			' ignore the following Interactive Brokers informational error codes
			'   2103 A market data farm is disconnected.
			'   2104 A market data farm is connected.
			'   2105 A historical data farm is disconnected.
			'   2106 A historical data farm is connected.
			'   2107 A historical data farm connection has become inactive but should be available upon demand.
			'   2108 A market data farm connection has become inactive but should be available upon demand.
		ElseIf eventArgs.errorCode = 503 And InteractiveBrokers_ConnectError Then 
			'   Ignore Interactive Brokers error code 503 (TWS is out of date and must be upgraded) because this occurs after a 508 which occurs if the user presses NO to accept incoming connection
		ElseIf eventArgs.errorCode = 506 Then 
			' Ignore Interactive Brokers error code 506 (Unexplained Zero Bytes Read) which happens when IBTWS closed before Trader
		ElseIf eventArgs.errorCode = 202 Then 
			' ignore Interactive Brokers errro code 202 (cancled msg) because it would show up everytime a limit/stop order is cancelled and replaced with a different order
		Else
			' Send all other messages to be displayed in NeuroShell Trader
			NSTOrders1.Message(eventArgs.errorMsg)
		End If
	End Sub
	
	Private Sub Tws1_nextValidId(ByVal eventSender As System.Object, ByVal eventArgs As AxTWSLib._DTwsEvents_nextValidIdEvent) Handles Tws1.nextValidId
		' **************************************************************************************************
		' Interactive Broker's event that occurs after a successful connection to the Interactive Broker TWS
		'
		' id = next available order ID received from Interactive Brokers TWS upon connection.
		'      Increment all successive orders by one based on this ID.
		' **************************************************************************************************
		
		' store Interactive Broker's next valid order id
		InteractiveBrokers_NextValidOrderId = eventArgs.id
	End Sub
	
	Private Sub Tws1_orderStatus(ByVal eventSender As System.Object, ByVal eventArgs As AxTWSLib._DTwsEvents_orderStatusEvent) Handles Tws1.orderStatus
		' **************************************************************************************************
		' Interactive Broker's event that occurs whenever the status of an order changes.
		' **************************************************************************************************
		
		If eventArgs.status = "Filled" And eventArgs.filled > 0 And eventArgs.lastFillPrice <> 0 Then
			' Tell NeuroShell Trader that order was filled
			NSTOrders1.OrderFilled(CStr(eventArgs.id), eventArgs.filled, eventArgs.avgFillPrice)
		ElseIf eventArgs.status = "Cancelled" Then 
			' Tell NeuroShell Trader that order was canceled
			NSTOrders1.OrderCanceled(CStr(eventArgs.id))
		ElseIf eventArgs.status = "Inactive" Then 
			' Tell NeuroShell Trader that order was made inactive (usually due to some error in the order parameters or inability of brokerage execute the order)
			NSTOrders1.OrderInactive(CStr(eventArgs.id))
		End If
	End Sub


End Class