
Partial Class Forms_FrmContact
    Inherits System.Web.UI.Page

    Dim ObjClsInputValidity As ClsInputValidity
    Dim ObjClsMail As New ClsMail

    Dim MailSub, MailTo, MailFrom, MailBody, ServerName As String

    'Dim Pswd As String
   
    Dim Flag As Boolean
    Dim ValidityFlag As Boolean
    Dim sb As New StringBuilder

    Protected Sub BttnSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BttnSend.Click


        ' ****** Validate E Mail Address ***************************************************
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
        '************************************************************************************

        ' *********      Validations     ****************************************************
        If TxtName.Text = "" Then
            Response.Write("<script language='javascript'>alert ('Please Enter EMail Address');</script>")
            Exit Sub
        End If

        If TxtTelephone.Text = "" Then
            Response.Write("<script language='javascript'>alert ('Please Enter Telephone');</script>")
            Exit Sub
        End If

        If TxtEnquiry.Text = "" Then
            Response.Write("<script language='javascript'>alert ('Please Enter your Enquiry Details');</script>")
            Exit Sub
        End If
        '************************************************************************************


        ' ******** Sending Mail   *****************************

        MailSub = "Contact Form Submission - From " & TxtName.Text

        'MailTo = "sales@scalper.co.uk"
        MailTo = System.Configuration.ConfigurationManager.AppSettings("SalesDptID")

        MailFrom = TxtEMailID.Text

        ServerName = System.Configuration.ConfigurationManager.AppSettings("ServerStr")

        ' **************  Sending Mail *********************************
        ' ********    Making Table For Mail body ***********************
        ' **************************************************************

        sb.Append("<Table>")

        'sb.Append("<tr>")
        'sb.Append("<td>")
        'sb.Append("Your login details are as follows")
        'sb.Append("</td>")
        'sb.Append("</tr>")

        sb.Append("<tr>")
        sb.Append("<td>Enquiry From : " & TxtName.Text)
        sb.Append("</td>")
        sb.Append("</tr>")

        sb.Append("<tr>")
        sb.Append("<td>Telephone : " & TxtTelephone.Text)
        sb.Append("</td>")
        sb.Append("</tr>")

        sb.Append("<tr>")
        sb.Append("<td>EMail ID : " & TxtEMailID.Text)
        sb.Append("</td>")
        sb.Append("</tr>")

        sb.Append("</Table>")

        MailBody = sb.ToString

        '************************************************************************

        ObjClsMail = New ClsMail

        Flag = ObjClsMail.SendMail(MailSub, MailTo, MailFrom, "", MailBody, ServerName)

        If Flag = True Then

            Sorry.Visible = False
            Thanks.Visible = True
            ClearForm()

        Else

            Sorry.Visible = True
            Thanks.Visible = False

        End If

        '******************************************************************************
    End Sub

    Public Sub ClearForm()

        TxtName.Text = ""
        TxtEMailID.Text = ""
        TxtTelephone.Text = ""
        TxtEnquiry.Text = ""

    End Sub

   
End Class
