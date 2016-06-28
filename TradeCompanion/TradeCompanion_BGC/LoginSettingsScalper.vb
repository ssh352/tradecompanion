Public Class LoginSettingsScalper

    Private Sub txtPort_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPassword.TextChanged

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        'TODO
        'do validation if any field is blank
        'do the validation for retype new password
        If (txtLoginID.Text = "" Or txtPassword.Text = "" Or txtNewPassword.Text = "" Or txtRNewPassword.Text = "") Then
            MsgBox("All fields are mandatory", MsgBoxStyle.OkOnly, "AutoShark")
            Return
        End If

        If (txtNewPassword.Text.Trim().Length < 3) Then
            MsgBox("Minimun length 3 characters for password. Please type again", MsgBoxStyle.OkOnly, "AutoShark")
            txtNewPassword.Text = ""
            txtRNewPassword.Text = ""
            Return
        End If

        If (txtNewPassword.Text.Trim().Length > 15) Then
            MsgBox("Maximun length 15 characters for password. Please type again", MsgBoxStyle.OkOnly, "AutoShark")
            txtNewPassword.Text = ""
            txtRNewPassword.Text = ""
            Return
        End If

        If (txtNewPassword.Text.Trim().IndexOf(" ") >= 0) Then
            MsgBox("Password should not contain spaces. Please type again", MsgBoxStyle.OkOnly, "AutoShark")
            txtNewPassword.Text = ""
            txtRNewPassword.Text = ""
            Return
        End If

        If (txtNewPassword.Text.Trim() <> txtRNewPassword.Text.Trim()) Then
            MsgBox("New password and Retype new password are not same. Please type again", MsgBoxStyle.OkOnly, "AutoShark")
            txtNewPassword.Text = ""
            txtRNewPassword.Text = ""
            Return
        End If

        Try
            Dim wsScalper As New WSScalper.WebServicesScalper
            Dim result As Boolean
            result = wsScalper.ModifyPassword(txtLoginID.Text.Trim(), txtPassword.Text.Trim(), txtNewPassword.Text.Trim())
            If (result = True) Then
                MsgBox("Password updated successfully", MsgBoxStyle.OkOnly, "AutoShark")
            Else
                MsgBox("Invalid loginid or password", MsgBoxStyle.OkOnly, "AutoShark")
                Return
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "AutoShark")
            Util.WriteDebugLog("LoginSettingsScalper -- " & ex.Message)
        End Try

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
End Class