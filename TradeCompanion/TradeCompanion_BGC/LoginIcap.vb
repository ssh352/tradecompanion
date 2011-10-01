Public Class LoginIcap
    Private keys As SettingsHome
    Private Sub bOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bOk.Click
        If Me.tUserId.Text.Trim = "" Or Me.tIPAddress.Text.Trim = "" Or Me.tPort.Text.Trim = "" Or Me.tPassword.Text.Trim = "" Then
            MsgBox("All field are mandatory", MsgBoxStyle.OkOnly, "Logon Alert")
        Else
            keys.IcapIP = tIPAddress.Text
            keys.IcapPort = tPort.Text
            keys.IcapUserName = tUserId.Text.Trim()
            keys.IcapPassword = tPassword.Text
            keys.reconnectInterval = Interval_combo.Value

            Util.WriteDebugLog("------------Trade Connection ICAP-----------------")
            Util.WriteDebugLog("...Settings:")
            Util.WriteDebugLog("            IP            -" + Me.tIPAddress.Text.ToString)
            Util.WriteDebugLog("            Port          -" + Me.tPort.Text.ToString)
            Util.WriteDebugLog("            Sender        -" + Me.tUserId.Text.ToString)


            Me.DialogResult = System.Windows.Forms.DialogResult.OK

            Util.WriteDebugLog("---------------------------------------------")

            Me.Close()
        End If
    End Sub

    Private Sub bCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bCancel.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub LoginIcap_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Keys = SettingsHome.getInstance()
        tIPAddress.Text = keys.IcapIP
        tPort.Text = keys.IcapPort
        tUserId.Text = keys.IcapUserName
        'tPassword.Text = keys.IcapPassword
        Interval_combo.Value = keys.reconnectInterval
    End Sub
End Class