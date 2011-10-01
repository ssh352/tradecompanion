Namespace GainClient

    Public Class GainTrades

        Inherits EventArgs

        Private _OrderID As String
        Private _Currency As String
        Private _Time As String
        Private Shared _AccountID As String
        Private _OfferID As String
        Private _BuySell As Boolean
        Private _Lot As Integer
        Private _Side As String
        Private _Status As String
        Private _Rate As Double

        Public Sub New(ByVal OrderID As String, ByVal OfferID As String, ByVal Lot As Integer, ByVal currency As String, ByVal BuySell As String, ByVal Time As String, ByVal Side As Integer, ByVal AccountID As String, ByVal Rate As Double, ByVal Status As String)
            _Lot = Lot
            _Time = Time
            _Side = Side
            _Rate = Rate
            _Status = Status
            _OrderID = OrderID
            _OfferID = OfferID
            If BuySell = "B" Then
                _BuySell = True
            Else
                _BuySell = False
            End If
            _Currency = currency
            _AccountID = AccountID
        End Sub

        Public ReadOnly Property OrderID() As String
            Get
                Return _OrderID
            End Get
        End Property

        Public ReadOnly Property Quantity() As Integer
            Get
                Return _Lot
            End Get
        End Property

        Public ReadOnly Property Currency() As String
            Get
                Return _Currency
            End Get
        End Property

        Public ReadOnly Property TimeStamp() As String
            Get
                Return _Time
            End Get
        End Property

        Public Shared ReadOnly Property SenderID() As String
            Get
                Return _AccountID
            End Get
        End Property

        Public ReadOnly Property Symbol() As String
            Get
                Return _OfferID
            End Get
        End Property

        Public ReadOnly Property Side() As Integer
            Get
                If _BuySell Then
                    Return 1
                Else
                    Return 2
                End If
            End Get
        End Property

        Public ReadOnly Property TradeType() As Integer
            Get
                Return _Side
            End Get
        End Property

        Public ReadOnly Property Status() As String
            Get
                Return _Status
            End Get
        End Property

        Public ReadOnly Property Rate() As Double
            Get
                Return _Rate
            End Get
        End Property

    End Class

End Namespace
