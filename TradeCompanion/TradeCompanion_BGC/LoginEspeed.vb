Imports System.Windows.Forms

Public Class LoginEspeed
    Private keys As SettingsHome

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        If Me.txtSender.Text.Trim = "" Or Me.txtIP.Text.Trim = "" Or Me.txtPort.Text.Trim = "" Or Me.txtTarget.Text.Trim = "" Then
            MsgBox("All field are mandatory", MsgBoxStyle.OkOnly, "Logon Alert")
        Else
            keys.EspeedLoginIP = txtIP.Text
            keys.EspeedLoginPort = txtPort.Text
            keys.EspeedLoginUsername = txtSender.Text.Trim()
            keys.EspeedLoginPassword = txtTarget.Text
            keys.reconnectInterval = reconnectInterval.Value

            Util.WriteDebugLog("------------Trade Connection ESPEED-----------------")
            Util.WriteDebugLog("...Settings:")
            Util.WriteDebugLog("            IP            -" + Me.txtIP.Text.ToString)
            Util.WriteDebugLog("            Port          -" + Me.txtPort.Text.ToString)
            Util.WriteDebugLog("            Username        -" + Me.txtSender.Text.ToString)

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
        txtIP.Text = keys.EspeedLoginIP
        txtPort.Text = keys.EspeedLoginPort
        txtSender.Text = keys.EspeedLoginUsername
        txtTarget.Text = keys.EspeedLoginPassword
        reconnectInterval.Value = keys.reconnectInterval
        'If Form1.GetSingletonOrderform().crashtest Then   
        '    Me.OK_Button.PerformClick() 
        'End If   
    End Sub
End Class
