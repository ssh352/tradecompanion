Imports System.Data

Public Class clsUsers

#Region "Variables"
    Private lDataAccess As ClsDatabaseAccessNew
    Private lCommand As IDbCommand
    Private lCommonFunctions As ClsCommonFunctions
    Private lDataReader As IDataReader
    Private lInformation As String
    Private lConnection As IDbConnection
    Private lParameter As IDbDataParameter
#End Region

    'Function used to authenticate user
    '    Friend Overridable Function AuthenticateUser(ByVal argUserName As String, ByVal argPassword As String) As Boolean
    Public Function AuthenticateUser(ByVal argTblName As String, ByVal argUserName As String, ByVal argPassword As String) As Boolean

        SetCommandObject()

        '        lCommand.CommandText = "Select * from Users Where User_Name = @UserName And User_Status = 1"

        lCommand.CommandText = "Select * from " & argTblName & " Where userName = '" & argUserName & "' "

        ' AddParameterInCommand("@Username", argUserName)

        lDataReader = lCommand.ExecuteReader

        If lDataReader.Read Then
            If lDataReader("Pswd") = argPassword Then
                Return True
                'lInformation = ""
                'Return lInformation
            Else
                lInformation = "You entered wrong user name or password."
                Return False
            End If
        Else
            lInformation = "You entered wrong user name or password."
            Return False
        End If

        DestroyCommandObject()
    End Function

    'Function used to change user password

    Public Function ChangeUserPassword(ByVal argTblName As String, ByVal argUserName As String, ByVal argOldPassword As String, ByVal argNewPassword As String) As Boolean
        If AuthenticateUser(argTblName, argUserName, argOldPassword) = False Then
            lInformation = "Old password do not match"
            MsgBox(lInformation, MsgBoxStyle.Information, "Change Password")
            Return False
            Exit Function
        End If

        SetCommandObject()

        ' lCommand.CommandText = "Update users Set Password = @NewPassword Where User_Name = @UserName"
        lCommand.CommandText = "Update " & argTblName & " Set Pswd = @NewPassword Where UserName = @UserName"

        AddParameterInCommand("@NewPassword", argNewPassword)
        AddParameterInCommand("@UserName", argUserName)
        lCommand.ExecuteNonQuery()

        DestroyCommandObject()
        lInformation = "Password changed successfully."

        Return True

        'MsgBox(lInformation, MsgBoxStyle.Information, "Change Password")

    End Function


    'Function used to Insert new user
    Public Sub InsertNewUser(ByVal argUserName As String, ByVal argPersonName As String, ByVal argPassword As String _
    , ByVal argUserType As Integer, ByVal argCreateByUserName As String, Optional ByVal argExpiredOn As String = "", Optional ByVal argActivationDate As String = "")

        If CheckUserExist(argUserName) = True Then Exit Sub
        If CheckUserType(argUserType) = False Then Exit Sub

        If CheckUserExist(argCreateByUserName) = False Then
            lInformation = "Ubable to create user (" & argUserName & ") , creator (" & argCreateByUserName & ") does not exist."
            Exit Sub
        End If

        SetCommandObject()

        lCommand.CommandText = "Insert Into Users Values (@UserName, @Person_Name, @Password," _
        & "@User_Type_Id, @CreateDate, @CreatedBy, @ModifyDate, @ModifiedBy, @Status," _
        & "@Expiry_Date, @Activation_Date, @LoggedIn)"

        AddParameterInCommand("@Username", argUserName)
        AddParameterInCommand("@Person_Name", argPersonName)
        AddParameterInCommand("@Password", argUserName)
        AddParameterInCommand("@User_Type_ID", argUserType)
        AddParameterInCommand("@CreatedBy", argCreateByUserName)
        AddParameterInCommand("@Expiry_Date", DBNull.Value)
        AddParameterInCommand("@Activation_Date", DBNull.Value)
        AddParameterInCommand("@CreateDate", Now)
        AddParameterInCommand("@ModifiedBy", DBNull.Value)
        AddParameterInCommand("@ModifyDate", DBNull.Value)
        AddParameterInCommand("@Status", 1)
        AddParameterInCommand("@LoggedIN", 0)
        lCommand.ExecuteNonQuery()

        DestroyCommandObject()
        lInformation = "New user created successfully."
    End Sub

    'Function used to check user exist
    Private Function CheckUserExist(ByVal argUserName As String) As Boolean
        SetCommandObject()

        'lDataReader = lDataAccess.ExecReader("Select * from Users Where User_Name = @UserName")

        'AddParameterInCommand("@UserName", argUserName)

        lDataReader = lDataAccess.ExecReader("Select * from Users Where User_Name = '" & argUserName & "' ")

        If lDataReader.Read Then
            lInformation = "User already exist."
            Return True
        End If

        DestroyCommandObject()
    End Function

    'Property used to return the information generated by the class
    Public ReadOnly Property GetInformationFromClass() As String
        Get
            GetInformationFromClass = lInformation
        End Get
    End Property

    'Sub used to Add parameters in command object
    Private Sub AddParameterInCommand(ByVal argParameterName As String, ByVal argParameterValue As Object)
        lParameter = lCommand.CreateParameter

        lParameter.ParameterName = argParameterName : lParameter.Value = argParameterValue
        lCommand.Parameters.Add(lParameter)
        lParameter = Nothing
    End Sub

    'Sub used to delete user from table 
    Public Sub DeleteUser(ByVal argUserName As String, Optional ByVal DeletePermanently As Boolean = False)

        SetCommandObject()

        If DeletePermanently = True Then
            lCommand.CommandText = "Delete from Users Where User_Name = @UserName"
        Else
            lCommand.CommandText = "Update Users Set Status_ID = 0 Where User_Name = @UserName"
        End If

        AddParameterInCommand("@UserName", argUserName)

        lCommand.ExecuteNonQuery()

        DestroyCommandObject()
        lInformation = "User deleted successfully."
    End Sub

    'Sub used to update user information
    Public Sub UpdateUser(ByVal argUserName As String, ByVal argPersonName As String _
    , ByVal argUserType As Integer, ByVal argModifyBy As String, Optional ByVal argExpiredOn As String = "", Optional ByVal argActivationDate As String = "")

        If CheckUserType(argUserType) = False Then Exit Sub
        If CheckUserType(argUserType) = False Then Exit Sub

        If CheckUserExist(argModifyBy) = False Then
            lInformation = "Ubable to modify user (" & argUserName & "), modifier user (" & argModifyBy & ") does not exist."
            Exit Sub
        End If

        SetCommandObject()

        lCommand.CommandText = "Update Users Set Person_Name = @Person_Name, " _
        & "User_Type_ID = @User_Type_Id, Modified_date = @ModifiedDate, Modified_User_Name = @ModifiedBy, " _
        & "Expiry_Date = @Expiry_Date, Activation_Date = @Activation_Date Where User_Name = @UserName"

        AddParameterInCommand("@Username", argUserName)
        AddParameterInCommand("@Person_Name", argPersonName)
        AddParameterInCommand("@User_Type_ID", argUserType)
        AddParameterInCommand("@Expiry_Date", DBNull.Value)
        AddParameterInCommand("@Activation_Date", DBNull.Value)
        AddParameterInCommand("@ModifiedBy", argModifyBy)
        AddParameterInCommand("@ModifiedDate", Now)
        lCommand.ExecuteNonQuery()

        DestroyCommandObject()
        lInformation = "User updated successfully."
    End Sub

    'Function used to return the user information as supplied by the user
    Public Function GetUserInformation(ByVal argUserName As String) As DataRow
        If CheckUserExist(argUserName) = False Then Exit Function

        lDataAccess = New ClsDatabaseAccessNew
        GetUserInformation = lDataAccess.ExecDataAdapter("Select * from Users Where User_Name = '" & argUserName & "'").Tables(0).Rows(0)
        lDataAccess = Nothing
    End Function

    'Function used to return the All user list
    Public Function GetAllUsers(Optional ByVal argColumns As String = "") As DataTable
        lDataAccess = New ClsDatabaseAccessNew
        If Trim(argColumns) = "" Then
            Return lDataAccess.ExecDataAdapter("Select * from Users").Tables(0)
        Else
            Return lDataAccess.ExecDataAdapter("Select " & argColumns & " from Users").Tables(0)
        End If
        lDataAccess = Nothing
    End Function

    'Function used to check the user type exist or not
    Private Function CheckUserType(ByVal argUserTypeID As Integer) As Boolean
        SetCommandObject()

        lCommand.CommandText = "Select Count(*) from UserTypes Where User_Type_ID = @UserTypeID"

        AddParameterInCommand("@UserTypeID", argUserTypeID)

        Dim lRowsCount As Integer = lCommand.ExecuteScalar()

        DestroyCommandObject()

        If lRowsCount = 0 Then
            lInformation = "User type does not exist."
            Return False
        Else
            Return True
        End If
    End Function

    'Function used to return the user types
    Public Function GetUserTypes() As DataTable
        lDataAccess = New ClsDatabaseAccessNew
        Return lDataAccess.ExecDataAdapter("Select * from UserTypes").Tables(0)
        lDataAccess = Nothing
    End Function

    Private Sub SetCommandObject()
        lDataAccess = New ClsDatabaseAccessNew
        lDataAccess.ConnectToDatabase()
        lConnection = lDataAccess.GetConnectionObject()
        lCommand = lDataAccess.GetCommandObject()
        lCommand.Connection = lConnection
    End Sub

    Private Sub DestroyCommandObject()
        lCommand.Dispose()
        lDataAccess.DisconnectFromDatabase()
        lDataAccess = Nothing
    End Sub
End Class
