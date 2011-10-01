Namespace GainClient
    Public Class GainMarketData

        Private _Key As String
        Private _CurrencyPair As String
        Private _Ask As Decimal
        Private _Bid As Decimal
        Private _High As Decimal
        Private _Low As Decimal
        Private _DecimalPlaces As Integer
        Private _Dealable As String
        Private _Domain As String
        
        Public Property Key() As String
            Get
                Return _Key
            End Get
            Set(ByVal Value As String)
                _Key = Value
            End Set
        End Property

        Public Property CurrencyPair() As String
            Get
                Return _CurrencyPair
            End Get
            Set(ByVal Value As String)
                _CurrencyPair = Value
            End Set
        End Property

        Public Property Ask() As Decimal
            Get
                Return _Ask
            End Get
            Set(ByVal Value As Decimal)
                _Ask = Value
            End Set
        End Property

        Public Property Bid() As Decimal
            Get
                Return _Bid
            End Get
            Set(ByVal Value As Decimal)
                _Bid = Value
            End Set
        End Property

        Public Property High() As Decimal
            Get
                Return _High
            End Get
            Set(ByVal Value As Decimal)
                _High = Value
            End Set
        End Property

        Public Property Low() As Decimal
            Get
                Return _Low
            End Get
            Set(ByVal Value As Decimal)
                _Low = Value
            End Set
        End Property

        Public Property DecimalPlaces() As Integer
            Get
                Return _DecimalPlaces
            End Get
            Set(ByVal Value As Integer)
                _DecimalPlaces = Value
            End Set
        End Property

        Public Property Dealable() As String
            Get
                Return _Dealable
            End Get
            Set(ByVal Value As String)
                _Dealable = Value
            End Set
        End Property

        Public Property Domain() As String
            Get
                Return _Domain
            End Get
            Set(ByVal Value As String)
                _Domain = Value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Dim rateStr As New System.Text.StringBuilder(130)
            rateStr.Append("Key: ")
            rateStr.Append(Key)
            rateStr.Append(" Currency Pair: ")
            rateStr.Append(CurrencyPair)
            rateStr.Append(" Bid: ")
            rateStr.Append(Bid)
            rateStr.Append(" Ask: ")
            rateStr.Append(Ask)
            rateStr.Append(" High: ")
            rateStr.Append(High)
            rateStr.Append(" Low: ")
            rateStr.Append(Low)
            rateStr.Append(" Dealable: ")
            rateStr.Append(Dealable)
            rateStr.Append(" Domain: ")
            rateStr.Append(Domain)
            rateStr.Append(" Decimal Places: ")
            rateStr.Append(DecimalPlaces)
            ToString = rateStr.ToString()
        End Function

    End Class

End Namespace
