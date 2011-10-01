Namespace DBFXClient
    Public Class DBFXTrades
        Inherits EventArgs

        Private tOrderID As String
        Private tCurrency As String
        Private tTime As String
        Private tAccountID As String
        Private tOfferID As String
        Private tBuySell As Boolean
        Private tLot As Integer
        Private tSide As String
        Private tStatus As String
        Private tRate As Double
        Public Sub New(ByVal OrderID As String, ByVal OfferID As String, ByVal Lot As Integer, ByVal currency As String, ByVal BuySell As String, ByVal Time As String, ByVal Side As Integer, ByVal AccountID As String, ByVal Rate As Double, ByVal Status As String)
            tLot = Lot
            tTime = Time
            tSide = Side
            tRate = Rate
            tStatus = Status
            tOrderID = OrderID
            tOfferID = OfferID
            If BuySell = "B" Then
                tBuySell = True
            Else
                tBuySell = False
            End If
            tCurrency = currency
            tAccountID = AccountID
        End Sub

        Public ReadOnly Property OrderID() As String
            Get
                Return tOrderID
            End Get
        End Property

        Public ReadOnly Property Quantity() As Integer
            Get
                Return tLot
            End Get
        End Property

        Public ReadOnly Property Currency() As String
            Get
                Return tCurrency
            End Get
        End Property

        Public ReadOnly Property TimeStamp() As String
            Get
                Return tTime
            End Get
        End Property

        Public ReadOnly Property SenderID() As String
            Get
                Return tAccountID
            End Get
        End Property

        Public ReadOnly Property Symbol() As String
            Get
                Return tCurrency
            End Get
        End Property

        Public ReadOnly Property Side() As Integer
            Get
                Return tSide
            End Get
        End Property

        Public ReadOnly Property TradeType() As Integer
            Get
                Return tSide
            End Get
        End Property

        Public ReadOnly Property Status() As String
            Get
                Return tStatus
            End Get
        End Property

        Public ReadOnly Property Rate() As Double
            Get
                Return tRate
            End Get
        End Property
    End Class
End Namespace
