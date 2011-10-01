Public Class HelpScalper

    Private Sub HelpScalper_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            RTBUserManual.LoadFile(Application.StartupPath + "\readme.rtf")
        Catch
            'Problem in opening file
        End Try
    End Sub
End Class