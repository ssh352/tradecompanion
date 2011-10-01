Imports System
Public Class MDHistory
    Dim pbidPrice As Decimal
    Dim pofferPrice As Decimal
    Dim ptimeStamp As DateTime
    Dim psymbol As String
    Dim pPips As Integer

    Public Property BidPrice() As Decimal
        Get
            BidPrice = pbidPrice
        End Get
        Set(ByVal value As Decimal)
            pbidPrice = value
        End Set
    End Property
    Public Property OfferPrice() As Decimal
        Get
            OfferPrice = pofferPrice
        End Get
        Set(ByVal value As Decimal)
            pofferPrice = value
        End Set
    End Property
    Public Property TimeStamp() As DateTime
        Get
            TimeStamp = ptimeStamp
        End Get
        Set(ByVal value As DateTime)
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
    Public Property Pips() As Integer
        Get
            Pips = pPips
        End Get
        Set(ByVal value As Integer)
            pPips = value
        End Set
    End Property
End Class
