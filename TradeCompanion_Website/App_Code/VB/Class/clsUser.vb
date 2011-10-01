Imports System.Data.SqlClient

Public Class ClsUser

    Public Function CheckUserPassword(ByVal tblName As String, ByVal fldUser As String, ByVal fldPassword As String, ByVal argUserName As String, ByVal argPassword As String) As Boolean
        'Try
        objReader = ExecSQLReader("Select * from " & tblName & " Where " & fldUser & " = '" & GetCorrectedString(argUserName) & "'")

        If objReader.Read Then
            If objReader(fldPassword) = argPassword Then
                CheckUserPassword = True
            Else
                CheckUserPassword = False
            End If
        Else
            CheckUserPassword = False
        End If
        DistroyCommandObject()
        'Catch ex As Exception

        'End Try
    End Function

    Public Function VerifyUser(ByVal tblName As String, ByVal fldUser_name As String, ByVal fldPassword As String, ByVal argUser_Name As String, ByVal argPassword As String) As String
        'Try
        Dim StrDate As Date
        Dim strPersonName As String

        StrDate = Now

        objReader = ExecSQLReader("Select * from " & tblName & " Where " & fldUser_name & " = '" & GetCorrectedString(argUser_Name) & "'")


        If objReader.Read Then

            If Not objReader(fldPassword) = argPassword Then
                VerifyUser = "You entered wrong user name or password."
            Else
                VerifyUser = "OK"
            End If

        Else
            VerifyUser = "You entered wrong user name or password."
        End If


        DistroyCommandObject()
        'Catch ex As Exception 

        'End Try
    End Function

    ' With status Id"""''''''''''''''''''''''

    'Public Function VerifyUser(ByVal tblName As String, ByVal fldUser_name As String, ByVal fldPassword As String, ByVal fldStatus_id As String, ByVal fldPerson_name As String, ByVal argUser_Name As String, ByVal argPassword As String) As String
    '    'Try
    '    Dim StrDate As Date
    '    Dim strPersonName As String

    '    StrDate = Now

    '    objReader = ExecSQLReader("Select * from " & tblName & " Where " & fldUser_name & " = '" & GetCorrectedString(argUser_Name) & "'")


    '    If objReader.Read Then

    '        If objReader(fldPassword) = argPassword Then

    '            'If objReader(fldExpiry_date) >= StrDate Then

    '            If objReader("LoggedIN") = True Then
    '                VerifyUser = "This user is already logged in."

    '            Else
    '                If objReader(fldStatus_id) = "1" Then
    '                    VerifyUser = ""
    '                ElseIf objReader(fldStatus_id) = "2" Then
    '                    VerifyUser = "User is deactive."
    '                End If
    '            End If
    '        Else
    '            VerifyUser = "You entered wrong user name or password."
    '        End If
    '    Else
    '        VerifyUser = "You entered wrong user name or password."
    '    End If


    '    DistroyCommandObject()
    '    'Catch ex As Exception 

    '    'End Try
    'End Function


    Public Function CheckDuplicateUserName(ByVal tblName As String, ByVal fldUserCode As String, ByVal argUserCode As String) As Boolean
        'Try
        objReader = ExecSQLReader("Select * from " & tblName & " Where " & fldUserCode & " = '" & GetCorrectedString(argUserCode) & "'")

        If objReader.Read Then
            CheckDuplicateUserName = True
        End If

        DistroyCommandObject()
    End Function

    Public Function CreateUser(ByVal tblName As String, ByVal fldUser_name As String, ByVal fldPerson_name As String, ByVal fldPassword As String, ByVal fldUser_type_id As String, ByVal fldCreate_date As String, ByVal fldCreated_by_User_name As String, ByVal fldModified_date As String, ByVal fldModified_user_name As String, ByVal fldStatus_id As String, ByVal fldExpiry_date As String, ByVal fldActivation_Date As String, ByVal argUser_name As String, ByVal argPerson_name As String, ByVal argPassword As String, ByVal argUser_type_id As String, ByVal argCreate_date As String, ByVal argCreated_by_User_name As String, ByVal argModified_date As String, ByVal argModified_user_name As String, ByVal argStatus_id As String, ByVal argExpiry_date As String, ByVal argActivation_Date As String)
        Dim SqlQuery As String

        'Try

        Connect()

        'SqlQuery = "Insert Into " & tblName & " ( " & fldUser_name & " , " & fldPerson_name & " , " & fldPassword & " , " & fldUser_type_id & ", " & fldCreate_date & ", " & fldCreated_by_User_name & ", " & fldModified_date & ", " & fldModified_user_name & ", " & fldStatus_id & ", " & fldExpiry_date & " , " & fldActivation_Date & ") Values ('" & argUser_name & "' , '" & argPerson_name & "' , '" & argPassword & "' , '" & argUser_type_id & "', '" & argCreate_date & "', '" & argCreated_by_User_name & "', '" & argModified_date & "', '" & argModified_user_name & "', '" & argStatus_id & "', '" & argExpiry_date & "', '" & argActivation_Date & "')"
        SqlQuery = "Insert Into " & tblName & " ( " & fldUser_name & " , " & fldPerson_name & " , " & fldPassword & " , " & fldUser_type_id & ", " & fldCreate_date & ", " & fldCreated_by_User_name & ", " & fldModified_date & ", " & fldModified_user_name & ", " & fldStatus_id & ", " & fldExpiry_date & " , " & fldActivation_Date & ", LoggedIn) Values ('" & argUser_name & "' , '" & argPerson_name & "' , '" & argPassword & "' , '" & argUser_type_id & "', '" & argCreate_date & "', '" & argCreated_by_User_name & "', NULL , NULL, '" & argStatus_id & "', '" & argExpiry_date & "', '" & argActivation_Date & "', 0)"


        objCommand = New SqlCommand(SqlQuery, objConnection)

        'objCommand.Parameters.Add("@User_name", argUser_name)
        'objCommand.Parameters.Add("@Person_name", argPerson_name)
        'objCommand.Parameters.Add("@Password", argPassword)
        'objCommand.Parameters.Add("@User_type_id", argUser_type_id)


        'objCommand.Parameters.Add("@Create_date", argCreate_date)
        'objCommand.Parameters.Add("@Created_by_User_name", argCreated_by_User_name)


        ''objCommand.Parameters.Add("@Modified_date", argModified_date)
        ''objCommand.Parameters.Add("@Modified_user_name", argModified_user_name)
        'objCommand.Parameters.Add("@Modified_date", System.DBNull.Value)
        'objCommand.Parameters.Add("@Modified_user_name", System.DBNull.Value)

        'objCommand.Parameters.Add("@Status_id", argStatus_id)
        'objCommand.Parameters.Add("@Expiry_date", argExpiry_date)
        'objCommand.Parameters.Add("@Activation_Date", argActivation_Date)

        objCommand.ExecuteNonQuery()

        objCommand.Dispose()
        Disconnect()


        'Catch ex As Exception

        'End Try

    End Function

    Public Function UpdateUser(ByVal tblName As String, ByVal fldUser_name As String, ByVal fldPerson_name As String, ByVal fldPassword As String, ByVal fldUser_type_id As String, ByVal fldCreated_by_User_name As String, ByVal fldModified_date As String, ByVal fldModified_user_name As String, ByVal fldStatus_id As String, ByVal fldExpiry_date As String, ByVal fldActivation_Date As String, ByVal argUser_name As String, ByVal argPerson_name As String, ByVal argPassword As String, ByVal argUser_type_id As String, ByVal argCreated_by_User_name As String, ByVal argModified_date As String, ByVal argModified_user_name As String, ByVal argStatus_id As String, ByVal argExpiry_date As String, ByVal argActivation_Date As String)

        Dim SqlQuery As String

        'Try

        Connect()

        'SqlQuery = "Update " & tblName & " Set  " & fldPerson_name & " = '" & argPerson_name & "' ," & fldPassword & " = '" & argPassword & "'," & fldUser_type_id & " = '" & argUser_type_id & "'," & fldCreate_date & " = '" & argCreate_date & "'," & fldCreated_by_User_name & " = '" & argCreated_by_User_name & "'," & fldModified_date & " = '" & argModified_date & "'," & fldModified_user_name & " = '" & argModified_user_name & "'," & fldStatus_id & " = '" & argStatus_id & "'," & fldExpiry_date & " = '" & argExpiry_date & "'," & fldActivation_Date & " = '" & argActivation_Date & "' where " & fldUser_name & " = '" & argUser_name & "' "

        SqlQuery = "Update " & tblName & " Set  " & fldPerson_name & " = '" & argPerson_name & "' ," & fldPassword & " = '" & argPassword & "'," & fldUser_type_id & " = '" & argUser_type_id & "', " & fldModified_date & " = '" & argModified_date & "'," & fldModified_user_name & " = '" & argModified_user_name & "'," & fldStatus_id & " = '" & argStatus_id & "'," & fldExpiry_date & " = '" & argExpiry_date & "'," & fldActivation_Date & " = '" & argActivation_Date & "' where " & fldUser_name & " = '" & argUser_name & "' "

        objCommand = New SqlCommand(SqlQuery, objConnection)

        objCommand.Parameters.Add("@User_name", argUser_name)
        objCommand.Parameters.Add("@Person_name", argPerson_name)
        objCommand.Parameters.Add("@Password", argPassword)
        objCommand.Parameters.Add("@User_type_id", argUser_type_id)


        'objCommand.Parameters.Add("@Create_date", argCreate_date)
        'objCommand.Parameters.Add("@Created_by_User_name", argCreated_by_User_name)


        objCommand.Parameters.Add("@Modified_date", argModified_date)
        objCommand.Parameters.Add("@Modified_user_name", argModified_user_name)

        objCommand.Parameters.Add("@Status_id", argStatus_id)
        objCommand.Parameters.Add("@Expiry_date", argExpiry_date)
        objCommand.Parameters.Add("@Activation_Date", argActivation_Date)

        objCommand.ExecuteNonQuery()

        objCommand.Dispose()
        Disconnect()


        'Catch ex As Exception

        'End Try

    End Function

    Public Function DeleteUser(ByVal tblName As String, ByVal fldUser_name As String, ByVal argUser_name As String)

        Dim SqlQuery As String

        'Try

        Connect()

        SqlQuery = "Delete from " & tblName & " where " & fldUser_name & " = '" & argUser_name & "' "

        objCommand = New SqlCommand(SqlQuery, objConnection)

        objCommand.ExecuteNonQuery()


        objCommand.Dispose()
        Disconnect()


        'Catch ex As Exception

        'End Try


    End Function


    'Public Function CreateUser(ByVal tblName As String, ByVal fldUserCode As String, ByVal fldFullName As String, ByVal fldPassword As String, ByVal fldUTID As String, ByVal argUserCode As String, ByVal argFullName As String, ByVal argPassword As String, ByVal argUTID As Integer)
    '    Dim SqlQuery As String

    '    SqlQuery = "Insert Into " & tblName & " ( " & fldUserCode & " , " & fldFullName & " , " & fldPassword & " , " & fldUTID & " ) Values (' " & argUserCode & " ' ,' " & argFullName & " ' ,' " & argPassword & " ',' " & argUTID & " ')"

    '    objCommand = New SqlCommand(SqlQuery, objConnection)
    '    objCommand.Parameters.Add("fldUserCode", argUserCode)
    '    objCommand.Parameters.Add("fldFullName", argFullName)
    '    objCommand.Parameters.Add("fldPassword", argPassword)
    '    objCommand.Parameters.Add("fldUTID", argUTID)

    '    objCommand.ExecuteNonQuery()
    '    objCommand.Dispose()
    'End Function

    Public Function GetUserType(ByVal argUserType As Int16) As String
        If argUserType = 1 Then
            GetUserType = "Systems Administrator"
        ElseIf argUserType = 2 Then
            GetUserType = "Super User"
        ElseIf argUserType = 3 Then
            GetUserType = "Advanced User"
        ElseIf argUserType = 4 Then
            GetUserType = "Basic User"
        End If
    End Function
End Class
