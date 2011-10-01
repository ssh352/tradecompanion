Public Class AboutTradeCompanion

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
        'Application.ProductName
        'Application.ProductVersion
    End Sub

    Private Sub AboutTradeCompanion_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim verInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(Application.ExecutablePath)
        lblBuild.Text = "Build Version: " + verInfo.FileBuildPart.ToString()
        lblVersion.Text = "Version: " + verInfo.FileMajorPart.ToString() + "." + verInfo.FileMinorPart.ToString()
    End Sub

    Private Sub RichTextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RichTextBox1.TextChanged

    End Sub
End Class