<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoginSettingsScalper
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtRNewPassword = New System.Windows.Forms.TextBox
        Me.lblRetypeNewPassword = New System.Windows.Forms.Label
        Me.txtNewPassword = New System.Windows.Forms.TextBox
        Me.lblNewPassword = New System.Windows.Forms.Label
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.LblPassword = New System.Windows.Forms.Label
        Me.txtLoginID = New System.Windows.Forms.TextBox
        Me.lblLoginid = New System.Windows.Forms.Label
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.OK_Button = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'txtRNewPassword
        '
        Me.txtRNewPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtRNewPassword.Location = New System.Drawing.Point(164, 116)
        Me.txtRNewPassword.Name = "txtRNewPassword"
        Me.txtRNewPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtRNewPassword.Size = New System.Drawing.Size(190, 20)
        Me.txtRNewPassword.TabIndex = 16
        '
        'lblRetypeNewPassword
        '
        Me.lblRetypeNewPassword.AutoSize = True
        Me.lblRetypeNewPassword.Location = New System.Drawing.Point(26, 119)
        Me.lblRetypeNewPassword.Name = "lblRetypeNewPassword"
        Me.lblRetypeNewPassword.Size = New System.Drawing.Size(112, 13)
        Me.lblRetypeNewPassword.TabIndex = 15
        Me.lblRetypeNewPassword.Text = "Retype new password"
        '
        'txtNewPassword
        '
        Me.txtNewPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNewPassword.Location = New System.Drawing.Point(164, 89)
        Me.txtNewPassword.Name = "txtNewPassword"
        Me.txtNewPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtNewPassword.Size = New System.Drawing.Size(190, 20)
        Me.txtNewPassword.TabIndex = 14
        '
        'lblNewPassword
        '
        Me.lblNewPassword.AutoSize = True
        Me.lblNewPassword.Location = New System.Drawing.Point(26, 94)
        Me.lblNewPassword.Name = "lblNewPassword"
        Me.lblNewPassword.Size = New System.Drawing.Size(77, 13)
        Me.lblNewPassword.TabIndex = 13
        Me.lblNewPassword.Text = "New password"
        '
        'txtPassword
        '
        Me.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPassword.Location = New System.Drawing.Point(164, 63)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(190, 20)
        Me.txtPassword.TabIndex = 12
        '
        'LblPassword
        '
        Me.LblPassword.AutoSize = True
        Me.LblPassword.Location = New System.Drawing.Point(26, 66)
        Me.LblPassword.Name = "LblPassword"
        Me.LblPassword.Size = New System.Drawing.Size(53, 13)
        Me.LblPassword.TabIndex = 11
        Me.LblPassword.Text = "Password"
        '
        'txtLoginID
        '
        Me.txtLoginID.AcceptsReturn = True
        Me.txtLoginID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtLoginID.Location = New System.Drawing.Point(164, 37)
        Me.txtLoginID.Name = "txtLoginID"
        Me.txtLoginID.Size = New System.Drawing.Size(190, 20)
        Me.txtLoginID.TabIndex = 10
        '
        'lblLoginid
        '
        Me.lblLoginid.AutoSize = True
        Me.lblLoginid.Location = New System.Drawing.Point(26, 37)
        Me.lblLoginid.Name = "lblLoginid"
        Me.lblLoginid.Size = New System.Drawing.Size(47, 13)
        Me.lblLoginid.TabIndex = 9
        Me.lblLoginid.Text = "Login ID"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.BackColor = System.Drawing.Color.LightSteelBlue
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.Cancel_Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.Cancel_Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.Cancel_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Cancel_Button.Location = New System.Drawing.Point(176, 167)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 18
        Me.Cancel_Button.Text = "Cancel"
        Me.Cancel_Button.UseVisualStyleBackColor = False
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.BackColor = System.Drawing.Color.LightSteelBlue
        Me.OK_Button.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.OK_Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.OK_Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.OK_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.OK_Button.Location = New System.Drawing.Point(103, 167)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 17
        Me.OK_Button.Text = "OK"
        Me.OK_Button.UseVisualStyleBackColor = False
        '
        'LoginSettingsScalper
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(367, 212)
        Me.Controls.Add(Me.Cancel_Button)
        Me.Controls.Add(Me.OK_Button)
        Me.Controls.Add(Me.txtRNewPassword)
        Me.Controls.Add(Me.lblRetypeNewPassword)
        Me.Controls.Add(Me.txtNewPassword)
        Me.Controls.Add(Me.lblNewPassword)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.LblPassword)
        Me.Controls.Add(Me.txtLoginID)
        Me.Controls.Add(Me.lblLoginid)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LoginSettingsScalper"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Login Scalper"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtRNewPassword As System.Windows.Forms.TextBox
    Friend WithEvents lblRetypeNewPassword As System.Windows.Forms.Label
    Friend WithEvents txtNewPassword As System.Windows.Forms.TextBox
    Friend WithEvents lblNewPassword As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents LblPassword As System.Windows.Forms.Label
    Friend WithEvents txtLoginID As System.Windows.Forms.TextBox
    Friend WithEvents lblLoginid As System.Windows.Forms.Label
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents OK_Button As System.Windows.Forms.Button
End Class
