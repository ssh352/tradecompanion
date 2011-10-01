Public Class LoginFxIntegral
    Private keys As SettingsHome
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Me.txtSender.Text.Trim = "" Or Me.txtIP.Text.Trim = "" Or Me.txtPort.Text.Trim = "" Or Me.txtTarget.Text.Trim = "" Or Me.txtPassword.Text.Trim = "" Then
            MsgBox("All field are mandatory", MsgBoxStyle.OkOnly, "Logon Alert")
        Else
            keys.FxIntLoginIP = txtIP.Text
            keys.FxIntLoginPort = txtPort.Text
            keys.FxIntLoginSendercompId = txtSender.Text.Trim()
            keys.FxIntLoginPassword = txtPassword.Text.Trim()
            keys.FxIntLoginTargetcompId = txtTarget.Text
            keys.FxIntLoginUserName = txtUserName.Text
            keys.FxIntLoginLegalEntity = txtLegalEntity.Text.Trim()
            keys.reconnectInterval = reconnectInterval.Value

            Util.WriteDebugLog("------------Trade Connection FxIntegral-----------------")
            Util.WriteDebugLog("...Settings:")
            Util.WriteDebugLog("            IP            -" + Me.txtIP.Text.ToString)
            Util.WriteDebugLog("            Port          -" + Me.txtPort.Text.ToString)
            Util.WriteDebugLog("            Sender        -" + Me.txtSender.Text.ToString)
            Util.WriteDebugLog("            UserName        -" + Me.txtUserName.Text.ToString)
            Util.WriteDebugLog("            Target        -" + Me.txtTarget.Text.ToString)

            Me.DialogResult = System.Windows.Forms.DialogResult.OK

            Util.WriteDebugLog("---------------------------------------------")

            Me.Close()
        End If
    End Sub

    Private Sub LoginFxIntegral_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        keys = SettingsHome.getInstance()
        txtIP.Text = keys.FxIntLoginIP
        txtPort.Text = keys.FxIntLoginPort
        txtSender.Text = keys.FxIntLoginSendercompId
        txtTarget.Text = keys.FxIntLoginTargetcompId
        txtUserName.Text = keys.FxIntLoginUserName
        txtPassword.Text = keys.FxIntLoginPassword
        txtLegalEntity.Text = keys.FxIntLoginLegalEntity
        reconnectInterval.Value = keys.reconnectInterval
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class