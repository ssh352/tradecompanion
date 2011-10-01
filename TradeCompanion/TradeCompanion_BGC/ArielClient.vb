Public Class ArielClient

    Private Sub okBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles okBtn.Click

        If rbnBgcClient.Checked Then
            Filecopy(Application.StartupPath + "\Bgcclient\")
            SettingsHome.getInstance.ArielClient = "BGC"
        Else
            Filecopy(Application.StartupPath + "\Odlclient\")
            SettingsHome.getInstance.ArielClient = "ODL"
        End If
        Me.Close()
    End Sub

    Private Sub Filecopy(ByVal sourceFile As String)
        Try
            System.IO.File.Copy(sourceFile + "ClientAPI.ocx", Application.StartupPath + "\ClientAPI.ocx", True)
            System.IO.File.Copy(sourceFile + "ClientAPI.ppc", Application.StartupPath + "\ClientAPI.ppc", True)
        Catch ex As Exception
            Util.WriteDebugLog("ArielClient->FileCopy--- " + ex.Message + ex.StackTrace)
        End Try
    End Sub

End Class