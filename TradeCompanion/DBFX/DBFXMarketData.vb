Imports System
Imports System.Collections.Generic

Namespace DBFXClient

    Public Class DBFXMarketData
        Inherits EventArgs

        Private Instrument As String
        Private Ask As Double
        Private Bid As Double
        Private Time As String
        Private Shared isMDataConnection As Boolean = False

        Public Sub New(ByVal aInstrument As String, ByVal aAsk As Double, ByVal aBid As Double, ByVal aTime As String)
            Instrument = aInstrument
            Ask = aAsk
            Bid = aBid
            Time = aTime
        End Sub

        Public Shared Property isMDataConnected() As Boolean
            Get
                Return isMDataConnection
            End Get

            Set(ByVal value As Boolean)
                isMDataConnection = value
            End Set
        End Property

        Public ReadOnly Property TimeStamp() As String
            Get
                Return Time.ToString
            End Get
        End Property

        Public ReadOnly Property Symbol() As String
            Get
                Return Instrument
            End Get
        End Property

        Public ReadOnly Property BidPrice() As Double
            Get
                Return Bid
            End Get
        End Property

        Public ReadOnly Property OfferPrice() As Double
            Get
                Return Ask
            End Get

        End Property

    End Class

End Namespace

