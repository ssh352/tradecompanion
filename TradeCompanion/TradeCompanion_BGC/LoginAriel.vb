Imports System.Windows.Forms

Public Class LoginAriel
    Private keys As SettingsHome
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        If Me.txtAccountNo.Text.Trim = "" Or Me.txtPassword.Text.Trim = "" Or Me.txtUsername.Text.Trim = "" Then
            MsgBox("All field are mandatory", MsgBoxStyle.OkOnly, "Logon Alert")
        Else
            keys.ArielLoginUserID = txtAccountNo.Text
            keys.ArielLoginUserName = txtUsername.Text
            keys.ArielLoginPassword = txtPassword.Text.Trim()
            keys.reconnectInterval = reconnectInterval.Value

            Util.WriteDebugLog("------------Trade Connection ARIEL-----------------")
            Util.WriteDebugLog("...Settings:")
            Util.WriteDebugLog("            User ID       -" + Me.txtAccountNo.Text.ToString)
            Util.WriteDebugLog("            Username      -" + Me.txtUsername.Text.ToString)
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
        txtAccountNo.Text = keys.ArielLoginUserID
        txtUsername.Text = keys.ArielLoginUserName
        txtPassword.Text = keys.ArielLoginPassword
        reconnectInterval.Value = keys.reconnectInterval
        'If Form1.GetSingletonOrderform().crashtest Then        '    Me.OK_Button.PerformClick()        'End If
    End Sub

    Private Sub txtPort_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUsername.TextChanged

    End Sub
End Class
