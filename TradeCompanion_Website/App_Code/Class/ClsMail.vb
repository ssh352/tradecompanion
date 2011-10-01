'*********************************************************************************
'*********             Class For Sending Mail           **************************
'*********************************************************************************

Imports System
Imports System.IO
'Imports System.Web.Mail
Imports System.Net
Imports System.Net.Mail
Imports Microsoft.VisualBasic

Public Class ClsMail
    Dim mail As New MailMessage

    Public Function SendMail(ByVal MailSub As String, ByVal MailTo As String, ByVal MailFrom As String, ByVal MailCC As String, ByVal MailBody As String, ByVal ServerName As String) As Boolean

        Try

            'mail.To = MailTo
            'mail.Cc = MailCC
            'mail.From = MailFrom
            'mail.Subject = MailSub
            'mail.Body = MailBody
            'mail.BodyFormat = MailFormat.Html
            'SmtpMail.SmtpServer = "smtp.tradecompanion.co.uk" 'ServerName
            'SmtpMail.Send(mail)
            'SendMail = True

            Dim client As SmtpClient
            Dim fromAddr, ToAddr As MailAddress
            Dim mesg As MailMessage
            Dim mailToAddr As String = MailTo
            Dim smtpServer As String = "88.208.220.198"
            Dim smtpUserID As String = "logs@tradercompanion.co.uk"
            Dim smtpPasswd As String = "shusiloo"

            client = New SmtpClient(smtpServer)
            fromAddr = New MailAddress("logs@tradercompanion.co.uk", "Franco Dimuccio", System.Text.Encoding.UTF8)
            ToAddr = New MailAddress(mailToAddr, MailTo, System.Text.Encoding.UTF8)
            client.Credentials = New System.Net.NetworkCredential(smtpUserID, smtpPasswd)
            mesg = New MailMessage(fromAddr, ToAddr)
            mesg.Subject = MailSub
            mesg.SubjectEncoding = System.Text.Encoding.UTF8

            mesg.Body = MailBody
            mesg.BodyEncoding = System.Text.Encoding.UTF8
            client.Send(mesg)
            mesg.Dispose()
            SendMail = True
        Catch ex As Exception
            SendMail = False
        End Try

    End Function
End Class
