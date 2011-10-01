Imports System.Data.OleDb

Partial Class FrmSignUp

    Inherits System.Web.UI.Page
    Private lDataAccess As ClsDatabaseAccess
    Private objProviderFact As ClsProviderFactory
    Dim ObjClsInputValidity As ClsInputValidity
    Dim ObjPswdGen As ClsGeneratePswd

    Dim ValidityFlag As Boolean

    Dim MailSub As String
    Dim MailTo As String
    Dim MailFrom As String
    Dim MailCC As String
    Dim MailBody As String
    Dim ServerName As String
    Dim ObjClsMail As ClsMail
    Dim sb As New StringBuilder
    Dim Flag As Boolean
    Dim StrOrder As String

    Dim strPswd As String


    Protected Sub BttnSignUp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BttnSignUp.Click

        'TxtMailID.Value 

        ObjPswdGen = New ClsGeneratePswd

        strPswd = ObjPswdGen.getRandomAlphaNumeric()


        '**********   Validate Mendatory ields ************************

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

        '  ******  Check Duplicate UserName **************************

        Dim rd As OleDbDataReader
        Dim FlagUserExist As Boolean

        lDataAccess = New ClsDatabaseAccess
        lDataAccess.ConnectToDatabase()

        objProviderFact = New ClsProviderFactory

        lDataAccess.objCommand = objProviderFact.GetCommandType

        lDataAccess.objCommand.Connection = lDataAccess.objConnection

        lDataAccess.objCommand.CommandText = "Select * from TestAdmin Where userName = '" & TxtEMailID.Text & "' "

        rd = lDataAccess.objCommand.ExecuteReader()

        If rd.Read Then
            FlagUserExist = True
            Response.Write("<script language='javascript'>alert ('Sorry, This User Name is already taken.');</script>")
            TxtEMailID.Text = ""
            Exit Sub
        Else
            FlagUserExist = False
        End If

        lDataAccess.DisconnectFromDatabase()
        lDataAccess = Nothing
        objProviderFact = Nothing

        '****************************************************************************

        '  ***********************************************************

        lDataAccess = New ClsDatabaseAccess
        lDataAccess.ConnectToDatabase()

        objProviderFact = New ClsProviderFactory

        lDataAccess.objCommand = objProviderFact.GetCommandType

        lDataAccess.objCommand.Connection = lDataAccess.objConnection

        'lDataAccess.objCommand.CommandText = "Insert into TestAdmin (UserName,YourName,Address1,Address2,County, PostCode,Telephone,Access) Values (' " & TxtEMailAdd.Text & " ',' " & TxtFirstName.Text & " " & TxtLastName.Text & " ' ,' " & TxtAddress1.Text & " ',' " & TxtAddress2.Text & " ',' " & DDLCountry.SelectedItem.Text & " ',' " & TxtPostalCode.Text & " ',' " & TxtContactPhoneNo.Text & " ',0 )"

        lDataAccess.objCommand.CommandText = "Insert into TestAdmin (UserName,Pswd,Access) Values ('" & TxtEMailID.Text & "', '" & strPswd & "' , 0)"

        lDataAccess.objCommand.ExecuteNonQuery()

        lDataAccess.DisconnectFromDatabase()


        ' *********  Sending Mail to User  ********************************************

        '        MailSub = "Completed Order Form : " & Request.QueryString("Flag")
        MailSub = "Scalper.co.uk Registration - Your Password"

        'MailTo = System.Configuration.ConfigurationManager.AppSettings("AdminID")
        MailTo = TxtEMailID.Text

        MailFrom = "signup@scalper.co.uk"

        ServerName = System.Configuration.ConfigurationManager.AppSettings("ServerStr")


        ' **************  Sending Mail *********************************
        ' ********    Making Table For Mail body ***********************
        ' **************************************************************

        sb.Append("<Table>")

        sb.Append("<tr>")
        sb.Append("<td>")
        sb.Append("Thankyou for registering with Scalper.co.uk")
        sb.Append("</td>")
        sb.Append("</tr>")

        sb.Append("<tr>")
        sb.Append("<td>")
        sb.Append("Your login details are as follows")
        sb.Append("</td>")
        sb.Append("</tr>")

        sb.Append("<tr>")
        sb.Append("<td> User Name : " & TxtEMailID.Text)
        sb.Append("</td>")
        sb.Append("</tr>")

        sb.Append("<tr>")
        sb.Append("<td> Password : " & strPswd)
        sb.Append("</td>")
        sb.Append("</tr>")


        '*************************************************************
        MailBody = sb.ToString

        '************************************************************************

        ObjClsMail = New ClsMail

        Flag = ObjClsMail.SendMail(MailSub, MailTo, MailFrom, "", MailBody, ServerName)

        If Flag = True Then
            '  Response.Write("<script language='javascript'>window.open ('FrmThanks.aspx', target='_blank', 'height=400, width=350')</script>")
            TblSignUp.Visible = False
            TblTrue.Visible = True
        End If
        '******************************************************************************

    End Sub
End Class
