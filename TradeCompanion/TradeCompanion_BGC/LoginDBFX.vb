Imports System.Windows.Forms
Public Class LoginDBFX

    Private keys As SettingsHome
    'Private trader As AlertExecution
    'Public ReadOnly Property LoginObject() As AlertExecution
    '    Get
    '        LoginObject = trader
    '    End Get
    'End Property
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        If Me.txtUserName.Text.Trim = "" Or Me.txtPassword.Text.Trim = "" Or Me.txtURL.Text.Trim = "" Or Me.txtAccountType.Text.Trim = "" Then
            MsgBox("All field are mandatory", MsgBoxStyle.OkOnly, "Logon Alert")
            'Util.WriteDebugLog(" ...Incomplete request ignored for Trade.")
        Else
            keys.DBFXUserName = txtUserName.Text
            keys.DBFXPassword = txtPassword.Text
            keys.DBFXURL = txtURL.Text.Trim()
            keys.DBFXAccountType = txtAccountType.Text
            keys.reconnectInterval = reconnectInterval.Value
            Util.WriteDebugLog("------------Trade Connection DBFX-----------------")
            Util.WriteDebugLog("...Settings:")
            Util.WriteDebugLog("            User Name           -" + Me.txtUserName.Text.ToString)
            Util.WriteDebugLog("            Password            -" + Me.txtPassword.Text.ToString)
            Util.WriteDebugLog("            URL                 -" + Me.txtURL.Text.ToString)
            Util.WriteDebugLog("            Account Type        -" + Me.txtAccountType.Text.ToString)


            'If (Me.mailSettingsPanel.Visible) Then
            '    keys.smtpServer = smtpServerFld.Text
            '    keys.smtpUserID = smtpUserFld.Text
            '    keys.smtpPasswd = smtpPasswdFld.Text

            '    Util.WriteDebugLog("          SMTP Server     -" + Me.smtpServerFld.Text.ToString)
            '    Util.WriteDebugLog("          SMTP User ID    -" + Me.smtpUserFld.Text.ToString)
            '    Util.WriteDebugLog("          SMTP Password   -" + Me.smtpPasswdFld.Text.ToString)

            'End If
            'keys.setSettings()
            Me.DialogResult = System.Windows.Forms.DialogResult.OK

            Util.WriteDebugLog("---------------------------------------------")

            'If (trader Is Nothing) Then
            '    trader = New AlertExecution
            '    If Me.trader.Logon Then
            '        Me.DialogResult = System.Windows.Forms.DialogResult.OK
            '    Else
            '        MessageBox.Show("Failed to logon to the CurrenEx application.")
            '        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
            '    End If
            'End If

            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub Login_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        keys = SettingsHome.getInstance()
        txtUserName.Text = keys.DBFXUserName
        txtPassword.Text = keys.DBFXPassword
        txtURL.Text = keys.DBFXURL
        txtAccountType.Text = keys.DBFXAccountType
        reconnectInterval.Value = keys.reconnectInterval
        'If (Me.mailSettingsPanel.Visible) Then
        '    smtpServerFld.Text = keys.smtpServer
        '    smtpUserFld.Text = keys.smtpUserID
        '    smtpPasswdFld.Text = keys.smtpPasswd
        '    Dim s As String = "Application logs can be sent to BGC to diagonize problems with this software. These logs will be sent via email. Please provide these details in order to send the email correctly."
        '    Dim i As Integer
        '    For i = 0 To GroupBox1.Controls.Count - 1
        '        mailSettingToolTip.SetToolTip(GroupBox1.Controls(i), s)
        '    Next
        'End If
    End Sub
End Class