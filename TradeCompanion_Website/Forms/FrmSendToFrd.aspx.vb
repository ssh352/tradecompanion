
Partial Class FrmSendToFrd
    Inherits System.Web.UI.Page

    Dim ObjClsUser As ClsUser
    Dim ObjClsMail As New ClsMail

    Dim MailSub As String
    Dim MailTo As String
    Dim MailFrom As String
    Dim MailCC As String
    Dim MailBody As String
    Dim ServerName As String
    Dim URL As String

    Dim Flag As Boolean

    Dim sb As New StringBuilder

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Request.UrlReferrer.AbsoluteUri

        URL = Session("URL")

        If URL = "" Then
            URL = "tradecompanion.co.uk"
        End If

        Label1.Text = URL

    End Sub

    Protected Sub BttnSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BttnSend.Click

        If TxtName.Text = "" Or TxtFrom.Text = "" Or TxtSendTo.Text = "" Then
            Inform.Visible = False
            Thanks.Visible = False
            Sorry.Visible = True
            Exit Sub
        End If

        MailSub = "Look At This"

        MailTo = TxtSendTo.Text
        MailFrom = TxtFrom.Text
        MailBody = TxtMsg.Text

        ServerName = System.Configuration.ConfigurationManager.AppSettings("ServerStr")



        ' **************  Sending Mail *********************************
        ' ********    Making Table For Mail body ***********************
        ' **************************************************************

        sb.Append("<Table>")

        sb.Append("<tr>")
        sb.Append("<td>")
        sb.Append("This message was sent to you by " & TxtName.Text & " via tradecompanion.co.uk (TradeCompanion")
        sb.Append("Technologies Reseller)")
        sb.Append("</td>")
        sb.Append("</tr>")


        sb.Append("<tr>")
        sb.Append("<td>")
        sb.Append("</td>")
        sb.Append("</tr>")


        If Len(TxtMsg.Text) > 0 Then

            sb.Append("<tr>")
            sb.Append("<td> Message from " & TxtName.Text & " ::")
            sb.Append("</td>")
            sb.Append("</tr>")

            sb.Append("<tr>")
            sb.Append("<td>" & TxtMsg.Text)

            sb.Append("</td>")
            sb.Append("</tr>")

        End If


        sb.Append("<tr>")
        sb.Append("<td> Following is The Link To Visit The Page :: ")
        sb.Append("</td>")
        sb.Append("</tr>")

        sb.Append("<tr>")
        sb.Append("<td>")
        sb.Append("<a href=' " & URL & "'>" & URL & "</a>")
        sb.Append("</td>")
        sb.Append("</tr>")

        sb.Append("</Table>")

        MailBody = sb.ToString

        '*********************************************************************************

        ObjClsMail = New ClsMail

        Flag = ObjClsMail.SendMail(MailSub, MailTo, MailFrom, "", MailBody, ServerName)

        If Flag = True Then

            'Response.Write("<script language='javascript'>alert ('Thanks.');</script>")

            Thanks.Visible = True
            Inform.Visible = False
            Sorry.Visible = False
            ClearForm()
        Else

            Sorry.Visible = True
            Inform.Visible = False
            Thanks.Visible = False

        End If


    End Sub

    Public Sub ClearForm()

        TxtName.Text = ""
        TxtFrom.Text = ""
        TxtSendTo.Text = ""
        TxtMsg.Text = ""

    End Sub

End Class
