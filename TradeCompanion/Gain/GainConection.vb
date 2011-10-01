Imports System

Namespace GainClient

    Public Class GainConnection

        Private Shared _isTrdLoggedIn As Boolean = False
        Private Shared _isMktLoggedIn As Boolean = False
        Private Shared _RateServerKey As String = ""
        Private Shared _UserName As String = ""
        Private Shared _Password As String = ""
        Private Shared _Brand As String = ""
        Private Shared _Host As String = ""
        Private Shared _Port As Integer = -1
        Private Shared _isEverMarket As Boolean = False
        Private Shared _isEverLogOut As Boolean = False

        Public Shared Property isEverMarket() As Boolean

            Get
                Return _isEverMarket
            End Get

            Set(ByVal value As Boolean)
                _isEverMarket = value
            End Set

        End Property

        Public Shared Property isMDataLogOut() As Boolean

            Get
                Return _isEverLogOut
            End Get

            Set(ByVal value As Boolean)
                _isEverLogOut = value
            End Set

        End Property

        Public Shared Property isTrdLoggedIn() As Boolean

            Get
                Return _isTrdLoggedIn
            End Get

            Set(ByVal value As Boolean)
                _isTrdLoggedIn = value
            End Set

        End Property

        Public Shared Property isMktLoggedIn() As Boolean

            Get
                Return _isMktLoggedIn
            End Get

            Set(ByVal value As Boolean)
                _isMktLoggedIn = value
            End Set

        End Property

        Public Shared Property RateServerKey() As String

            Get
                Return _RateServerKey
            End Get

            Set(ByVal value As String)
                _RateServerKey = value
            End Set

        End Property

        Public Shared Property UserName() As String

            Get
                Return _UserName
            End Get

            Set(ByVal value As String)
                _UserName = value
            End Set

        End Property
        Public Shared Property Password() As String

            Get
                Return _Password
            End Get

            Set(ByVal value As String)
                _Password = value
            End Set

        End Property

        Public Shared Property Brand() As String

            Get
                Return _Brand
            End Get

            Set(ByVal value As String)
                _Brand = value
            End Set

        End Property
        Public Shared Property Host() As String

            Get
                Return _Host
            End Get

            Set(ByVal value As String)
                _Host = value
            End Set

        End Property
        Public Shared Property Port() As Integer

            Get
                Return _Port
            End Get

            Set(ByVal value As Integer)
                _Port = value
            End Set

        End Property
    End Class

End Namespace
