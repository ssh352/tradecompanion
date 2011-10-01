Imports System.Data.OleDb
Imports System.Web.UI.HtmlControls
Imports System.Data.SqlClient
Imports System.Data
Imports FirebirdSql.Data.FirebirdClient
Imports System.Diagnostics
Imports System.Data.Odbc
Imports System.Security.Cryptography

Partial Class FrmLogIn
    Inherits System.Web.UI.Page
    
    Public sql As String
    Public str As String
    Public str1 As String
    Dim fbcon As New FbConnection
    Dim fbcmd As New FbCommand
    Dim connectionstring As String

    Dim ObjClsUser As clsUsers
    Dim valid As Integer
    Private lDataAccess As ClsDatabaseAccess
    Private objProviderFact As ClsProviderFactory


    Protected Sub BttnLogIN_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BttnLogIN.Click
        valid = 0

        connectionstring = System.Configuration.ConfigurationManager.AppSettings("FireBirdDBConnectionString")
        fbcon = New FbConnection(connectionstring)
        fbcon.Open()

        Dim md5Hasher As New MD5CryptoServiceProvider()
        Dim hashedBytes As Byte()
        Dim encoder As New UTF8Encoding()
        hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(TxtPswd.Text))

        str = "select * from USERS where LOGINID='" + TxtEMailID.Text + "' AND Passwords = @Password AND ACTIVES = 'T'"
        fbcmd = New FbCommand(str, fbcon)

        Dim paramPwd As FbParameter
        paramPwd = New FbParameter("@Password", FbDbType.Char, 16)
        paramPwd.Value = hashedBytes
        fbcmd.Parameters.Add(paramPwd)

        Dim fda As New FbDataAdapter(fbcmd)
        Dim ds As New DataSet
        fda.Fill(ds)
        If ds.Tables(0).Rows.Count = 1 Then
            valid = 1
        Else
            valid = 0
        End If
        fbcon.Close()

        LblMsg.Text = ""
        'Dim wsServ As WSScalper.WebServicesScalper = New WSScalper.WebServicesScalper
        'valid = wsServ.ValidatePassword(TxtEMailID.Text().Trim(), Trim(TxtPswd.Text))
        If (valid = 1) Then

            Dim Img As System.Web.UI.HtmlControls.HtmlImage
            Img = Page.Master.FindControl("IMG3")
            Img.Src = "../Images/Internal/logout.jpg"

            SignInFlag = True

            Session("UserName") = TxtEMailID.Text

            '************** Getting Access Value of LogIn User  *************************************************************
            Dim Access As Integer = 0
            Session("Access") = Access

            '*******************************************************************************************
            'change 12may
            'ViewTrader.Visible = True
            If (TxtEMailID.Text = "rahul") Then
                Response.Write("<script language='javascript'>window.open ('FrmAdmin.aspx', target='_parent')</script>")
            Else
                Response.Write("<script language='javascript'>window.open ('FrmThanks.aspx', target='_parent')</script>")
            End If
        Else
            LblMsg.Text = "You entered wrong user name or password."
        End If

        ObjClsUser = Nothing

    End Sub

    Protected Sub LBttnForgotPswd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LBttnForgotPswd.Click

        Response.Write("<script language='javascript'>window.open ('FrmForgetPswd.aspx', target='_parent')</script>")

    End Sub

    Protected Sub TxtEMailID_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtEMailID.TextChanged

    End Sub
End Class