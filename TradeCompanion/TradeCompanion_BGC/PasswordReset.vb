Public Class PasswordReset

    'Public WithEvents ex As TradingInterface.IExecution
    'Public WithEvents Ae As New AlertExecution
    'Private WithEvents client As com.scalper.fix.driver.client.CurrenExClient = Nothing

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Me.txtUserID.Text.Trim = "" Or Me.txtExistingPassword.Text.Trim = "" Or Me.txtNewPassword.Text.Trim = "" Or Me.txtNewPassword2.Text.Trim = "" Then
            MsgBox("All field are mandatory", MsgBoxStyle.OkOnly, "Mandatory fields cannot be empty")
        ElseIf Me.txtNewPassword.Text <> Me.txtNewPassword2.Text Then
            MsgBox("The new passwords do not match")
        Else
            Dim obj As Object
            Dim s1 As Trader
            If (Form1.GetSingletonOrderform().ConnectHT.Contains(Me.txtUserID.Text.Trim())) Then
                obj = Form1.GetSingletonOrderform().ConnectHT.Item(Me.txtUserID.Text.Trim)
                s1 = CType(obj, Trader)
                lblStatus.Visible = True
                Me.Enabled = False
                s1.ResetPassword_t(Me.txtExistingPassword.Text.Trim(), Me.txtNewPassword.Text.Trim())
                Me.Enabled = True
                lblStatus.Visible = False
                Me.Close()
            Else
                MessageBox.Show("User ID not recognised : " + Me.txtUserID.Text.Trim())
            End If
        End If
    End Sub

    Private Sub PasswordReset_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Enabled = True
        lblStatus.Visible = False
    End Sub
End Class