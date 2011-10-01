Public Class WarningDuplicateAlerts

    Private Sub lblMessage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMessage.Click

    End Sub

    Private Sub btnAgree_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgree.Click
        SettingsHome.getInstance().FilterAlerts = False
        Util.WriteDebugLog(" .... Filter Alerts = FALSE")
        Me.Close()
    End Sub

    Private Sub btnNotAgree_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNotAgree.Click
        SettingsHome.getInstance().FilterAlerts = True
        Util.WriteDebugLog(" .... Filter Alerts = TRUE")
        Form1.GetSingletonOrderform().FilterAlertsToolStripMenuItem.Checked = True
        Me.Close()
    End Sub
End Class