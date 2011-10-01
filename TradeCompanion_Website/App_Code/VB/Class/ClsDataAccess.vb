'PROJECT ID: - .NET CODE LIBRARY
'CREATED BY: - ASHISH
'PURPOSE: - THIS CLASS WILL ACCESS DATABASE TABLES EXECUTES DATA ADAPTER & DATA READER ETC.
'DEPANDANCY: - TO USE THIS CLASS YOU NEED TO INCLUDE CLSPROVIDERFACTORY.VB CLASS.

Imports System, System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Data.Odbc


Public Class ClsDatabaseAccess
    Inherits ClsProviderFactory

#Region "Variables"
    Public objConnection As IDbConnection
    Public objDataAdapter As IDbDataAdapter
    Public objDataSet As DataSet
    Public objCommand As IDbCommand

    Private lInfoMessage As String
    Private lErrorMessage As String
#End Region

#Region "Properties of the Class"

#End Region

    'Function used to connect to database
    Public Function ConnectToDatabase() As Boolean
        objConnection = GetConnection

        If IsNothing(objConnection) Then
            ConnectToDatabase = False
            Exit Function
        End If

        With objConnection
            .ConnectionString = GetConnectionString
            .Open()
        End With
        ConnectToDatabase = True
    End Function

    'Function used to disconnect from the database
    Public Function DisconnectFromDatabase() As Boolean
        If Not IsNothing(objConnection) Then
            If objConnection.State = ConnectionState.Open Then
                objConnection.Close()
                objConnection = Nothing
            End If
        End If
    End Function

    'Executes the Data Adapter
    Public Function ExecDataAdapter(ByVal argSQLQuery As String) As DataSet
        'objDataAdapter = GetAdapterType

        Try
            objDataAdapter = New SqlClient.SqlDataAdapter(argSQLQuery, GetConnectionString)
        Catch ex As Exception
            'MsgBox(ex.Message)                   ' nb on 21 may 07
        End Try


        If IsNothing(objDataAdapter) Then
            Exit Function
        End If


        'objCommand = GetCommandType
        'objCommand.CommandText = argSQLQuery
        'ConnectToDatabase()
        'objCommand.Connection = objConnection
        'objCommand = GetCommandType

        'If IsNothing(objCommand) Then
        '    Exit Function
        'End If

        objDataSet = New DataSet

        Try
            objDataAdapter.Fill(objDataSet)
        Catch ex As Exception
            'MsgBox(ex.Message)                        ' nb on 19 June 07
        End Try

        ExecDataAdapter = objDataSet
        'DisconnectFromDatabase()
    End Function

    'Executes Reader Object
    Public Function ExecReader(ByVal argSQLQuery As String) As IDataReader
        If ConnectToDatabase() = False Then
            Exit Function
        End If

        objCommand = GetCommandType()
        objCommand.CommandText = argSQLQuery
        objCommand.Connection = objConnection
        Try
            ExecReader = objCommand.ExecuteReader
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Function

    'Distroy Reader Object
    Public Sub DestroyReaderObject()
        If Not IsNothing(objCommand) Then
            objCommand.Dispose() : objCommand = Nothing
        End If
        DisconnectFromDatabase()
    End Sub

    Public Sub DestroyDataAdapter()
        If Not IsNothing(objDataAdapter) Then
            objDataAdapter = Nothing
            objDataSet = Nothing
            objCommand = Nothing
        End If
    End Sub
End Class
