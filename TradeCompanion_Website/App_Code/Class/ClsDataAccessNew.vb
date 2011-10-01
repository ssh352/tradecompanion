'PROJECT ID: - .NET CODE LIBRARY
'CREATED BY: - ASHISH
'PURPOSE: - THIS CLASS WILL ACCESS DATABASE TABLES EXECUTES DATA ADAPTER & DATA READER ETC.
'DEPANDANCY: - TO USE THIS CLASS YOU NEED TO INCLUDE CLSPROVIDERFACTORY.VB CLASS.

Imports System, System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Data.Odbc

Public Class ClsDatabaseAccessNew
    Inherits ClsProviderFactory

#Region "Variables"
    Private objConnection As IDbConnection
    Private objDataAdapter As IDbDataAdapter
    Private objDataSet As DataSet
    Private objCommand As IDbCommand

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
        objDataAdapter = GetAdapterType

        If IsNothing(objDataAdapter) Then
            Exit Function
        End If

        ConnectToDatabase()
        objCommand = GetCommandType
        objCommand.CommandText = argSQLQuery
        objCommand.Connection = objConnection
        objDataAdapter.SelectCommand = objCommand
        objDataSet = New DataSet
        objDataAdapter.Fill(objDataSet)
        DisconnectFromDatabase()

        Return objDataSet
    End Function

    'Executes Reader Object
    Public Function ExecReader(ByVal argSQLQuery As String) As IDataReader
        If ConnectToDatabase() = False Then
            Exit Function
        End If

        objCommand = GetCommandType
        objCommand.CommandText = argSQLQuery
        objCommand.Connection = objConnection
        ExecReader = objCommand.ExecuteReader
    End Function

    'Distroy Reader Object
    Public Sub DestroyReaderObject()
        If Not IsNothing(objCommand) Then
            objCommand.Dispose() : objCommand = Nothing
        End If
        DisconnectFromDatabase()
    End Sub

    Public Sub DestroyDataAdapter()

    End Sub

    Public ReadOnly Property GetConnectionObject() As IDbConnection
        Get
            GetConnectionObject = objConnection
        End Get
    End Property

    Public ReadOnly Property GetCommandObject() As IDbCommand
        Get
            GetCommandObject = GetCommandType()
        End Get
    End Property
End Class
