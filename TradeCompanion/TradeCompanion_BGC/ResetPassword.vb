Public Class ResetPassword

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            If (txtLoginID.Text = "" Or txtEmailID.Text = "") Then
                MsgBox("All fields are mandatory", MsgBoxStyle.OkOnly, "TradeCompanion")
                Return
            End If
            Dim wsScalper As New WSScalper.WebServicesScalper
            Dim result As Integer
            result = wsScalper.ForgotPassword(txtLoginID.Text.Trim(), txtEmailID.Text.Trim())
            If (result = 1) Then
                MsgBox("Email has been sent to you with new passord", MsgBoxStyle.OkOnly, "TradeCompanion")
            Else
                MsgBox("Invalid loginid or email", MsgBoxStyle.OkOnly, "TradeCompanion")
                Return
            End If

            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "TradeCompanion")
            Util.WriteDebugLog("ResetPassword --- " + ex.Message)
        End Try
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class