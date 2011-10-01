''' <summary>
''' 
''' <Purpose>Interface for All Broker Driver</Purpose>
''' 
''' <Usage>This Interface provides the link between the client and Broker Driver API on the Fly at runtime</Usage>
'''
'''  <Requirements>This Interface must implements this interface by all Broker Drivers</Requirements>
''' 
''' <Author>S7 Software Solutions </Author>
''' 
''' </summary>

Public Interface IExecution
    'Property IP() As String
    'Property Port() As String
    '@Navin
    Event WriteToLogEvent(ByVal msg As String)
    '#Navin
    Event OrderFilled(ByVal f As FFillOrder, ByVal fdata As Fill)
    Event OrderPlaced(ByVal f As FFillOrder)
    Event MarketDataUpdate(ByVal f As FillMarketData)
    Event Disconnected(ByVal reason As String)
    Event Connected()
    Event SymbolStatus(ByVal symbol As String)
    'pwreset
    Event Password_reset_response_received(ByVal UserName As String, ByVal UserStatus As Integer, ByVal UserStatusText As String)
    'Event HeartBeatReceived(ByVal SeqNo As Integer) 'HeartBeatCallback
    Function Logon(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String, Optional ByVal param5 As String = "", Optional ByVal param6 As String = "", Optional ByVal param7 As String = "") As Boolean
    Sub Logout()
    'Test1234
    Sub PlaceOrder(ByVal symbol As String, ByVal qty As Integer, ByVal side As Integer, ByVal currency As String, ByVal timestamp As String, ByVal tradeType As Integer, ByVal chartIdentifier As Integer, ByVal price As String, ByVal systemName As String, ByVal tsOpenPosition As Double)
    '@Navin
    Sub PlaceOrder(ByVal symbol As String, ByVal qty As Integer, ByVal side As Integer, ByVal currency As String, ByVal timestamp As String, ByVal tradeType As Integer, ByVal chartIdentifier As Integer, ByVal price As String, ByVal accountID As String, ByVal systemName As String, ByVal tsOpenPosition As Double)
    '#Navin
    Sub SubscribeMarketData(ByVal symbols() As String)
    Sub UnSubscribeMarketData()
    Sub TradeCaptureReport(ByVal clOrderID As String)
    Sub AppLogon(ByVal userName As String, ByVal passWord As String)
    'Vm
    Sub HistoricalData(ByVal symbol As String, ByVal fromDate As String, ByVal toDate As String, ByVal type As String)
    Event FeedHistoricalDataToNST(ByVal symb As String, ByVal dateTime As Double, ByVal priceOpen As Double, ByVal priceClose As Double, ByVal priceHigh As Double, ByVal priceLow As Double, ByVal type As Double)
    Event RepeatOrder(ByVal symbol As String, ByVal quantity As Integer, ByVal side As Integer, ByVal tradeType As String, ByVal timeStamp As String)
    Event OpenPositionValue(ByVal openValue As String, ByVal _Instrument As String, ByVal _UserId As String)
    'pwreset
    Function ResetPassword(ByVal ExistingPassword As String, ByVal NewPassword As String) As Boolean
End Interface

Public Class Fill
    Public Symbol As String = Nothing
    Public Exchange As String = Nothing
    Public side As Integer = 0
    Public Qty As Integer = 0
    Public price As Double = 0
    Public monthyear As String = ""
    Public timestamp As String = Nothing
    Public currency As String = Nothing
    Public orderId As String = Nothing
    Public accountId As String = Nothing
    Public systemName As String = Nothing
    Public tsOpenPosition As Double = 0
End Class

Public Class FFillOrder
    Public clOrdID As String
    Public symbol As String = ""
    Public quantity As Integer = 0
    Public currency As String = ""
    Public timestamp As String = ""
    Public side As Integer = -1
    Public tradeType As Integer
    Public chartIdentifier As Integer
    Public senderID As String = ""
    Public systemName As String = ""
    Public tsOpenPosition As Double = 0

    Private _status As String = Nothing
    Private _msgText As String = Nothing
    Private _tag As Integer = 0
    Public Sub New(ByVal OrderID)
        clOrdID = OrderID
    End Sub

    Public Property OrderID() As String
        Get
            OrderID = clOrdID
        End Get
        Set(ByVal value As String)
            clOrdID = value
        End Set
    End Property

    Public Property Status() As String
        Get
            Status = _status
        End Get
        Set(ByVal value As String)
            _status = value
        End Set
    End Property

    Public Property Message() As String
        Get
            Message = _msgText
        End Get
        Set(ByVal value As String)
            _msgText = value
        End Set
    End Property

    Public Property Tag() As String
        Get
            Return _tag
        End Get
        Set(ByVal value As String)
            _tag = value
        End Set
    End Property

    Public Function Predicate(ByVal f As FFillOrder) As Boolean
        Return f.OrderID = clOrdID
    End Function

    Public Function PredicateTag(ByVal f As FFillOrder) As Boolean
        Return f.Tag = _tag
    End Function
End Class

Public Class FillMarketData
    Private pbidPrice As String
    Private pofferPrice As String
    Private ptimeStamp As String
    Private psymbol As String

    Public Property BidPrice() As String
        Get
            BidPrice = pbidPrice
        End Get
        Set(ByVal value As String)
            pbidPrice = value
        End Set
    End Property

    Public Property OfferPrice() As String
        Get
            OfferPrice = pofferPrice
        End Get
        Set(ByVal value As String)
            pofferPrice = value
        End Set
    End Property

    Public Property TimeStamp() As String
        Get
            TimeStamp = ptimeStamp
        End Get
        Set(ByVal value As String)
            ptimeStamp = value
        End Set
    End Property

    Public Property Symbol() As String
        Get
            Symbol = psymbol
        End Get
        Set(ByVal value As String)
            psymbol = value
        End Set
    End Property

End Class



