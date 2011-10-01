
Imports System
Imports System.IO
Imports System.Xml
Imports System.Text
Imports System.Security.Cryptography
'** For Encrypt *****
Imports System.Configuration
Imports System.Web.Security
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Web
'-------------------------------

Partial Class Forms_TEST
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim str As String
        str = TextBox1.Text

        str = str.Replace("@@", "<br>&nbsp;")

        'str = str.Replace("tt", "ggg")

        'AA.InnerHtml = TextBox1.Text

        Label1.Text = str

        '**********************************************

        Dim sb As New StringBuilder
        '*****************************************************************

        sb.Append("<Table>")

        sb.Append("<tr>")
        sb.Append("<td>" & str)
        sb.Append("</td>")
        sb.Append("</tr>")

        ' Third Row (Blank)
        sb.Append("<tr>")
        sb.Append("<td>")
        sb.Append("</td>")
        sb.Append("</tr>")

        sb.Append("</Table>")
        '**********************************************************

        AA.InnerHtml = sb.ToString


    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '<PRE lang=vbnet id=pre1 style="MARGIN-TOP: 0px">
    ' Encrypt the text

    Public Shared Function EncryptText(ByVal strText As String) As String
        Return Encrypt(strText, "&%#@?,:*")
    End Function

    'Decrypt the text 
    Public Shared Function DecryptText(ByVal strText As String) As String
        Return Decrypt(strText, "&%#@?,:*")
    End Function

    'The function used to encrypt the text
    Private Shared Function Encrypt(ByVal strText As String, ByVal strEncrKey As String) As String

        Dim byKey() As Byte = {}
        Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}

        Try

            ' byKey() = System.Text.Encoding.UTF8.GetBytes(Left(strEncrKey, 8))

            '''''''byKey() = System.Text.Encoding.UTF8.GetBytes(Left(strEncrKey, 5))

            Dim des As New DESCryptoServiceProvider()
            Dim inputByteArray() As Byte = Encoding.UTF8.GetBytes(strText)
            Dim ms As New MemoryStream()
            Dim cs As New CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write)
            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()
            Return Convert.ToBase64String(ms.ToArray())

        Catch ex As Exception
            Return ex.Message
        End Try

    End Function

    'The function used to decrypt the text
    Private Shared Function Decrypt(ByVal strText As String, ByVal sDecrKey _
               As String) As String
        Dim byKey() As Byte = {}
        Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
        Dim inputByteArray(strText.Length) As Byte

        Try
            byKey = System.Text.Encoding.UTF8.GetBytes(Left(sDecrKey, 8))
            Dim des As New DESCryptoServiceProvider()
            inputByteArray = Convert.FromBase64String(strText)
            Dim ms As New MemoryStream()
            Dim cs As New CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write)

            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()
            Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8

            Return encoding.GetString(ms.ToArray())

        Catch ex As Exception
            Return ex.Message
        End Try

    End Function

   
    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click

        Txtresult.Text = EncryptText(TxtEncrypt.Text)


    End Sub

    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click
        '-- For Encryption
        EncryptConnStr("DataProtectionConfigurationProvider")
        '--or--
        ' Encrypt("RSAProtectedConfigurationProvider")

        '--*****-----------------------------------------------------------
    End Sub

    '************************** Encrypt data *********************
    Public Sub EncryptConnStr(ByVal protectionProvider As String)

        'http://www.ondotnet.com/pub/a/dotnet/2005/02/15/encryptingconnstring.html

        'System.Configuration.ConfigurationManager.AppSettings("ServerStr")

        '---open the web.config file
        'Dim config As Configuration = ConfigurationManager.OpenWebConfiguration(Request.ApplicationPath)

        Dim config As Configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath)

        'Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(Request.ApplicationPath)

        '---indicate the section to protect
        'Dim section As ConfigurationSection = config.Sections("connectionStrings")

        Dim section As ConfigurationSection = config.Sections("appSettings")

        'Dim section As ConfigurationSection = config.AppSettings("")

        'appSettings

        '---specify the protection provider
        section.SectionInformation.ProtectSection(protectionProvider)

        '---Apple the protection and update
        config.Save()
    End Sub
End Class
