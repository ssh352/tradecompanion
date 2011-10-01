<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PasswordReset
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtUserID = New System.Windows.Forms.TextBox
        Me.txtExistingPassword = New System.Windows.Forms.TextBox
        Me.txtNewPassword = New System.Windows.Forms.TextBox
        Me.txtNewPassword2 = New System.Windows.Forms.TextBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lblStatus = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(52, 45)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(46, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "User ID:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(52, 75)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(95, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Existing Password:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(52, 106)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(81, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "New Password:"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(52, 129)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(95, 26)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Re-Enter New Password"
        '
        'txtUserID
        '
        Me.txtUserID.Location = New System.Drawing.Point(151, 42)
        Me.txtUserID.Name = "txtUserID"
        Me.txtUserID.Size = New System.Drawing.Size(183, 20)
        Me.txtUserID.TabIndex = 4
        '
        'txtExistingPassword
        '
        Me.txtExistingPassword.Location = New System.Drawing.Point(151, 72)
        Me.txtExistingPassword.Name = "txtExistingPassword"
        Me.txtExistingPassword.Size = New System.Drawing.Size(183, 20)
        Me.txtExistingPassword.TabIndex = 5
        '
        'txtNewPassword
        '
        Me.txtNewPassword.Location = New System.Drawing.Point(151, 102)
        Me.txtNewPassword.Name = "txtNewPassword"
        Me.txtNewPassword.Size = New System.Drawing.Size(183, 20)
        Me.txtNewPassword.TabIndex = 6
        '
        'txtNewPassword2
        '
        Me.txtNewPassword2.Location = New System.Drawing.Point(151, 132)
        Me.txtNewPassword2.Name = "txtNewPassword2"
        Me.txtNewPassword2.Size = New System.Drawing.Size(183, 20)
        Me.txtNewPassword2.TabIndex = 7
        '
        'btnOK
        '
        Me.btnOK.BackColor = System.Drawing.Color.LightSteelBlue
        Me.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOK.Location = New System.Drawing.Point(251, 171)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(82, 24)
        Me.btnOK.TabIndex = 8
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.LightSteelBlue
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Location = New System.Drawing.Point(151, 171)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(82, 24)
        Me.btnCancel.TabIndex = 9
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(158, 202)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(168, 13)
        Me.lblStatus.TabIndex = 10
        Me.lblStatus.Text = "Changing password, please wait..."
        '
        'PasswordReset
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(388, 234)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.txtNewPassword2)
        Me.Controls.Add(Me.txtNewPassword)
        Me.Controls.Add(Me.txtExistingPassword)
        Me.Controls.Add(Me.txtUserID)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "PasswordReset"
        Me.Text = "Change Password"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtUserID As System.Windows.Forms.TextBox
    Friend WithEvents txtExistingPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtNewPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtNewPassword2 As System.Windows.Forms.TextBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label
End Class
