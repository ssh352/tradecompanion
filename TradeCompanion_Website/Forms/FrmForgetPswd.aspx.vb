
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb

Partial Class FrmForgetPswd

    Inherits System.Web.UI.Page
    Private lDataAccess As ClsDatabaseAccess
    Private objProviderFact As ClsProviderFactory

    Dim ObjClsInputValidity As ClsInputValidity
    Dim ObjClsMail As New ClsMail

    Dim MailSub, MailTo, MailFrom As String

    Dim Pswd As String
    Dim MailBody As String
    Dim ServerName As String
    Dim Flag As Boolean
    Dim ValidityFlag As Boolean
    Dim sb As New StringBuilder

    Protected Sub BttnSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BttnSend.Click

        ' ****** Validate E Mail Address ********************************************
        If TxtEMailID.Text = "" Then
            Response.Write("<script language='javascript'>alert ('Please Enter EMail Address');</script>")
            Exit Sub
        Else
            ObjClsInputValidity = New ClsInputValidity

            ValidityFlag = ObjClsInputValidity.EmailInputValidate(TxtEMailID.Text)
            If ValidityFlag = False Then
                Response.Write("<script language='javascript'>alert ('Please Enter Valid EMail Address');</script>")
                TxtEMailID.Text = ""
                Exit Sub
            End If
        End If
        '****************************************************************************


        ' **** Retreive Pswd From DB *************************************************

        'Dim rd As SqlDataReader
        Dim rd As OleDbDataReader


        lDataAccess = New ClsDatabaseAccess
        lDataAccess.ConnectToDatabase()

        objProviderFact = New ClsProviderFactory

        lDataAccess.objCommand = objProviderFact.GetCommandType

        lDataAccess.objCommand.Connection = lDataAccess.objConnection

        lDataAccess.objCommand.CommandText = "Select Pswd from TestAdmin Where userName = '" & TxtEMailID.Text & "' "

        rd = lDataAccess.objCommand.ExecuteReader()

        If rd.Read Then

            Pswd = rd(0)
        Else

            LblSend.Visible = False
            LblSorry.Visible = True
            LblSorry.Text = "Sorry, there is nobody registered with that email address"
            Exit Sub

        End If

        lDataAccess.DisconnectFromDatabase()

        '****************************************************************************

        MailSub = "Scalper.co.uk - Password Retrieval Service"


        MailTo = TxtEMailID.Text
        'MailTo = "nitinbansal@rmaxit.com"

        MailFrom = "passwords@scalper.co.uk"

        'MailBody = TxtMsg.Text

        ServerName = System.Configuration.ConfigurationManager.AppSettings("ServerStr")

        ' **************  Sending Mail *********************************
        ' ********    Making Table For Mail body ***********************
        ' **************************************************************

        sb.Append("<Table>")

        sb.Append("<tr>")
        sb.Append("<td>")
        sb.Append("Your login details are as follows")
        sb.Append("</td>")
        sb.Append("</tr>")

        sb.Append("<tr>")
        sb.Append("<td>")
        sb.Append("</td>")
        sb.Append("</tr>")

        sb.Append("<tr>")
        sb.Append("<td>UserName : " & TxtEMailID.Text)
        sb.Append("</td>")
        sb.Append("</tr>")

        sb.Append("<tr>")
        sb.Append("<td>Password : " & Pswd)
        sb.Append("</td>")
        sb.Append("</tr>")

        sb.Append("</Table>")

        MailBody = sb.ToString

        '************************************************************************

        ObjClsMail = New ClsMail

        Flag = ObjClsMail.SendMail(MailSub, MailTo, MailFrom, "", MailBody, ServerName)

        If Flag = True Then


            LblSorry.Visible = False
            LblSend.Visible = True
            LblSend.Text = "Your Password has been sent to " & TxtEMailID.Text
            TxtEMailID.Text = ""
        Else

            LblSorry.Visible = False
            LblSend.Visible = True
            LblSend.Text = "Your Password can not be sent to " & TxtEMailID.Text & "this time "

        End If
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LblSorry.Visible = False
        LblSend.Visible = False
    End Sub

End Class
