Imports System.Windows.Forms

Public Class LoginDukascopy
    Private keys As SettingsHome
    
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        If Me.txtSender.Text.Trim = "" Or Me.txtIP.Text.Trim = "" Or Me.txtPort.Text.Trim = "" Or Me.txtTarget.Text.Trim = "" Or Me.txtPassword.Text.Trim = "" Then
            MsgBox("All field are mandatory", MsgBoxStyle.OkOnly, "Logon Alert")
        Else
            keys.DUKASLoginIP = txtIP.Text
            keys.DUKASLoginPort = txtPort.Text
            keys.DUKASLoginSender = txtSender.Text.Trim()
            keys.DUKASLoginPassword = txtPassword.Text.Trim()
            keys.DUKASLoginTarget = txtTarget.Text
            keys.DUKASLoginUserName = txtUserName.Text
            keys.reconnectInterval = reconnectInterval.Value

            Util.WriteDebugLog("------------Trade Connection Dukascopy-----------------")
            Util.WriteDebugLog("...Settings:")
            Util.WriteDebugLog("            IP            -" + Me.txtIP.Text.ToString)
            Util.WriteDebugLog("            Port          -" + Me.txtPort.Text.ToString)
            Util.WriteDebugLog("            Sender        -" + Me.txtSender.Text.ToString)
            'Util.WriteDebugLog("            Password      -" + Me.txtPassword.Text.ToString)
            Util.WriteDebugLog("            Target        -" + Me.txtTarget.Text.ToString)

            Me.DialogResult = System.Windows.Forms.DialogResult.OK

            Util.WriteDebugLog("---------------------------------------------")

            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Login_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        keys = SettingsHome.getInstance()
        txtIP.Text = keys.DUKASLoginIP
        txtPort.Text = keys.DUKASLoginPort
        txtSender.Text = keys.DUKASLoginSender
        txtTarget.Text = keys.DUKASLoginTarget
        txtUserName.Text = keys.DUKASLoginUserName
        txtPassword.Text = keys.DUKASLoginPassword
        reconnectInterval.Value = keys.reconnectInterval
    End Sub
End Class
