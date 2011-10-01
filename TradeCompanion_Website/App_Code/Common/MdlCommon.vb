Imports System.Data, System.Data.SqlClient
Imports System.Configuration

Public Module MdlCommon
    Public tmpString As String
    Public tmpDouble As Double
    Public tmpInteger As Integer

    Public objConnection As SqlConnection
    Public objCommand As SqlCommand
    Public objReader As SqlDataReader
    Public objDataTable As DataTable
    Public objDataset As DataSet
    Public objDataAdapter As SqlDataAdapter
    Public objDataRow As DataRow

    'Public objAllFields As DataTable ', objLimited As DataTable
    Public objResultDataSet As New DataSet

    'Public Session("objAdvancedSearchFields") As DataTable

    Private DirSystem As System.IO.Directory
    Private FileSystem As System.IO.File

    Private FileWriter As System.IO.StreamWriter

    ' Added
    Public SignInFlag As Boolean = False


    Public Function Connect() As Boolean
        objConnection = New SqlConnection

        With objConnection
            .ConnectionString = GetConnectionString()
            .Open()
        End With
        Connect = True
    End Function

    Public Function GetConnectionString() As String
        GetConnectionString = ConfigurationSettings.AppSettings("DatabaseConnectionString")
    End Function

    Public Function Disconnect() As Boolean
        With objConnection
            .Close()
            .Dispose()
            Disconnect = True
        End With
    End Function

    Public Function ExecSQLReader(ByVal SQLQuery As String) As SqlDataReader
        Connect()
        objCommand = New SqlCommand(SQLQuery, objConnection)
        ExecSQLReader = objCommand.ExecuteReader()
    End Function

    Public Function DistroyCommandObject() As Boolean
        objReader.Close()
        objReader = Nothing
        objCommand.Dispose()
        Disconnect()
    End Function

    Public Function ExecAdapter(ByVal SQLQuery As String) As DataSet
        objDataAdapter = New SqlDataAdapter(SQLQuery, GetConnectionString)
        objDataset = New DataSet
        objDataAdapter.Fill(objDataset)
        ExecAdapter = objDataset
    End Function

    Public Sub DistroyAdapter()
        objDataset.Dispose()
        objDataAdapter.Dispose()
    End Sub

    Public Function GetCorrectedString(ByVal argString As String) As String
        If argString.Length = 0 Then
            GetCorrectedString = ""
            Exit Function
        End If

        tmpString = ""
        For tmpDouble = 1 To argString.Length
            If Asc(Mid(argString, tmpDouble, 1)) = 39 Then
                tmpString = tmpString & Mid(argString, tmpDouble, 1) & "'"
            Else
                tmpString = tmpString & Mid(argString, tmpDouble, 1)
            End If
        Next

        GetCorrectedString = tmpString
    End Function

    Public Function CheckFileExist(ByVal argFileName As String) As Boolean
        CheckFileExist = FileSystem.Exists(argFileName)
    End Function


    Public Function DeleteRecords(ByVal tblName As String, ByVal pkColumnName As String, ByVal pkColumnValue As String) As Boolean
        Dim tmpQuery As String

        tmpQuery = "Delete from " & tblName & " Where [" & pkColumnName & "] = '" & pkColumnValue & "'"
        Connect()
        objCommand = New SqlCommand(tmpQuery, objConnection)
        objCommand.ExecuteNonQuery()
        objCommand.Dispose() : objCommand = Nothing
        Disconnect()
    End Function
End Module
