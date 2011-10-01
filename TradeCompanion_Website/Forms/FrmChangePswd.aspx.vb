Imports FirebirdSql.Data.FirebirdClient
Imports System.Security.Cryptography

Partial Class FrmChangePswd

    Inherits System.Web.UI.Page
    Dim ObjClsUser As clsUsers
    Dim Flag As Boolean

    Protected Sub BttnChange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BttnChange.Click


        '********* Validations ******************************************************************************
        If TxtOldPswd.Text = "" Then
            Response.Write("<script language='javascript'>alert ('Please Enter Your Old Password');</script>")
            Exit Sub
        End If

        If TxtNewPswd.Text = "" Then
            Response.Write("<script language='javascript'>alert ('Please Enter Your New Password');</script>")
            Exit Sub
        End If

        If TxtConfPswd.Text = "" Then
            Response.Write("<script language='javascript'>alert ('Please Confirm Your Password');</script>")
            Exit Sub
        End If

        If TxtNewPswd.Text <> TxtConfPswd.Text Then
            Response.Write("<script language='javascript'>alert ('Provided Password does Not Match');</script>")
            Exit Sub
        End If

        'Dim wsServ As WSScalper.WebServicesScalper = New WSScalper.WebServicesScalper
        'Flag = wsServ.ModifyPassword(Session("UserName").ToString().Trim(), TxtOldPswd.Text().Trim(), TxtNewPswd.Text().Trim())
        Flag = ModifyPassword(Session("UserName").ToString().Trim(), TxtOldPswd.Text().Trim(), TxtNewPswd.Text().Trim())
        If (Flag = True) Then
            'Response.Write("<script language='javascript'>alert ('Password Changed Successfully !!!');</script>")
            Response.Redirect("FrmThanks.aspx")
        Else
            Response.Write("<script language='javascript'>alert ('Password Can Not be Changed.');</script>")
            Exit Sub
        End If
        TxtOldPswd.Text = ""
        TxtNewPswd.Text = ""
        TxtConfPswd.Text = ""

        ObjClsUser = Nothing

    End Sub

    Public Function ModifyPassword(ByVal loginid As String, ByVal oldpassword As String, ByVal newpassword As String) As Boolean
        Dim strSQL As String = "UPDATE USERS SET Passwords = @NewPassword where LoginID = @LoginID and Passwords = @OldPassword and ACTIVES = 'T';"

        Dim objCmd As New FbCommand(strSQL)

        'Create parameters
        Dim paramLoginID As New FbParameter("@LoginID", FbDbType.VarChar, 50)
        paramLoginID.Value = loginid
        objCmd.Parameters.Add(paramLoginID)

        'Encrypt the password
        Dim md5Hasher As New MD5CryptoServiceProvider()
        Dim hashedBytesOldPwd As Byte()
        Dim encoder As New UTF8Encoding()
        hashedBytesOldPwd = md5Hasher.ComputeHash(encoder.GetBytes(oldpassword))

        Dim paramOldPwd As New FbParameter("@OldPassword", FbDbType.Char, 16)
        paramOldPwd.Value = hashedBytesOldPwd
        objCmd.Parameters.Add(paramOldPwd)

        'Encrypt the password
        md5Hasher = New MD5CryptoServiceProvider()
        Dim hashedBytesNewPwd As Byte()
        encoder = New UTF8Encoding()
        hashedBytesNewPwd = md5Hasher.ComputeHash(encoder.GetBytes(newpassword))

        Dim paramNewPwd As New FbParameter("@NewPassword", FbDbType.Char, 16)
        paramNewPwd.Value = hashedBytesNewPwd
        objCmd.Parameters.Add(paramNewPwd)

        Dim connectionstring As String = System.Configuration.ConfigurationManager.AppSettings("FireBirdDBConnectionString")
        Dim fbcon As New FbConnection(connectionstring)
        fbcon.Open()
        objCmd.Connection = fbcon
        Dim rowseffected As Integer
        rowseffected = objCmd.ExecuteNonQuery()
        fbcon.Close()

        If (rowseffected >= 1) Then
            Return True
        Else
            Return False
        End If
    End Function



    'If TxtEMailID.Text = "" Then
    '    Response.Write("<script language='javascript'>alert ('Please Enter Your Existing Password');</script>")
    '    Exit Sub
    'End If

    'If TxtOldPswd.Text = "" Then
    '    Response.Write("<script language='javascript'>alert ('Please Enter Your New Password');</script>")
    '    Exit Sub
    'End If

    'If TxtNewPswd.Text = "" Then
    '    Response.Write("<script language='javascript'>alert ('Please ReType Your New Password');</script>")
    '    Exit Sub
    'End If

    'If TxtOldPswd.Text <> TxtNewPswd.Text Then
    '    Response.Write("<script language='javascript'>alert ('Provided Password does Not Match');</script>")
    '    Exit Sub
    'End If

    '***************************************************************************************************

    'Dim UserName As String
    '    UserName = Session("UserName")

    'Dim PswdFlag As Boolean

    '    ObjClsUser = New clsUsers
    '    PswdFlag = ObjClsUser.ChangeUserPassword("TestAdmin", UserName, TxtEMailID.Text, TxtOldPswd.Text)

    '    If PswdFlag = True Then
    '        Response.Write("<script language='javascript'>alert ('Password Changed Successfully !!!');</script>")


    '    Else
    '        Response.Write("<script language='javascript'>alert ('Password Can Not be Changed.');</script>")
    '        Exit Sub
    '    End If

    '    TxtOldPswd.Text = ""
    '    TxtEMailID.Text = ""
    '    TxtNewPswd.Text = ""

    '    ObjClsUser = Nothing

    'End Sub

    'Protected Sub LBttnForgotPswd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LBttnForgotPswd.Click
    '    Response.Write("<script language='javascript'>window.open ('FrmForgetPswd.aspx', target='_parent')</script>")
    'End Sub

End Class
