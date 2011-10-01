

Partial Class Forms_FrmCallBack
    Inherits System.Web.UI.Page

    Dim ObjClsMail As New ClsMail
    Dim MailSub, MailTo, MailFrom, MailBody, ServerName As String
    Dim sb As New StringBuilder
    Dim SendMail As Boolean


    Protected Sub BttnSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BttnSend.Click


        '******  VALIDATIONS *****************************************
        If TxtName.Text = "" Then
            Response.Write("<script language='javascript'>alert ('Please Enter Your Name');</script>")
            Exit Sub
        End If

        If TxtTelephone.Text = "" Then
            Response.Write("<script language='javascript'>alert ('Please Enter Telephone No');</script>")
            Exit Sub
        End If



        ' **************  Sending Mail *********************************

        MailSub = "Call Back Request - From " & TxtName.Text

        'MailTo = "sales@scalper.co.uk"
        MailTo = System.Configuration.ConfigurationManager.AppSettings("CallBack")

        MailFrom = "CallBack@scalper.co.uk"

        ServerName = System.Configuration.ConfigurationManager.AppSettings("ServerStr")


        ' **************************************************************
        ' ********    Making Table For Mail body ***********************
        ' **************************************************************

        sb.Append("<Table>")

        sb.Append("<tr>")
        sb.Append("<td>From : " & TxtName.Text)
        sb.Append("</td>")
        sb.Append("</tr>")

        sb.Append("<tr>")
        sb.Append("<td>Telephone : " & TxtTelephone.Text)
        sb.Append("</td>")
        sb.Append("</tr>")

        sb.Append("</Table>")

        MailBody = sb.ToString

        '************************************************************************

        ObjClsMail = New ClsMail

        SendMail = ObjClsMail.SendMail(MailSub, MailTo, MailFrom, "", MailBody, ServerName)

        If SendMail = True Then
            Response.Write("<script language='javascript'>alert ('Mail has been Sent');</script>")

            TblCallReq.Visible = False
            TblConfirm.Visible = True
            Label1.Text = Label1.Text & " " & TxtName.Text

            ClearForm()
        Else
            Response.Write("<script language='javascript'>alert ('Mail could not be send this time ! Please Try later');</script>")
        End If

        '******************************************************************************

    End Sub

    Public Sub ClearForm()

        TxtName.Text = ""
        TxtTelephone.Text = ""

    End Sub

End Class
