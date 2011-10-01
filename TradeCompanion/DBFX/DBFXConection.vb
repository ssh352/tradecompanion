Imports System
Imports FXCore

Namespace DBFXClient

    Public Class DBFXConnection

        Private Shared loggedIn As Boolean = False

        Public Shared Property isLoggedIn() As Boolean
            Get
                Return loggedIn
            End Get

            Set(ByVal value As Boolean)
                loggedIn = value
            End Set

        End Property

    End Class

End Namespace
