<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ResetPassword
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
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.OK_Button = New System.Windows.Forms.Button
        Me.txtEmailID = New System.Windows.Forms.TextBox
        Me.LblEmailID = New System.Windows.Forms.Label
        Me.txtLoginID = New System.Windows.Forms.TextBox
        Me.lblLoginid = New System.Windows.Forms.Label
        Me.SuspendLayout()
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
        Me.Cancel_Button.Location = New System.Drawing.Point(149, 72)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 28
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
        Me.OK_Button.Location = New System.Drawing.Point(76, 72)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 27
        Me.OK_Button.Text = "OK"
        Me.OK_Button.UseVisualStyleBackColor = False
        '
        'txtEmailID
        '
        Me.txtEmailID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtEmailID.Location = New System.Drawing.Point(92, 38)
        Me.txtEmailID.Name = "txtEmailID"
        Me.txtEmailID.Size = New System.Drawing.Size(190, 20)
        Me.txtEmailID.TabIndex = 22
        '
        'LblEmailID
        '
        Me.LblEmailID.AutoSize = True
        Me.LblEmailID.Location = New System.Drawing.Point(12, 41)
        Me.LblEmailID.Name = "LblEmailID"
        Me.LblEmailID.Size = New System.Drawing.Size(46, 13)
        Me.LblEmailID.TabIndex = 21
        Me.LblEmailID.Text = "Email ID"
        '
        'txtLoginID
        '
        Me.txtLoginID.AcceptsReturn = True
        Me.txtLoginID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtLoginID.Location = New System.Drawing.Point(92, 9)
        Me.txtLoginID.Name = "txtLoginID"
        Me.txtLoginID.Size = New System.Drawing.Size(190, 20)
        Me.txtLoginID.TabIndex = 20
        '
        'lblLoginid
        '
        Me.lblLoginid.AutoSize = True
        Me.lblLoginid.Location = New System.Drawing.Point(12, 9)
        Me.lblLoginid.Name = "lblLoginid"
        Me.lblLoginid.Size = New System.Drawing.Size(47, 13)
        Me.lblLoginid.TabIndex = 19
        Me.lblLoginid.Text = "Login ID"
        '
        'ResetPassword
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(293, 107)
        Me.Controls.Add(Me.Cancel_Button)
        Me.Controls.Add(Me.OK_Button)
        Me.Controls.Add(Me.txtEmailID)
        Me.Controls.Add(Me.LblEmailID)
        Me.Controls.Add(Me.txtLoginID)
        Me.Controls.Add(Me.lblLoginid)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ResetPassword"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Reset Password"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents txtEmailID As System.Windows.Forms.TextBox
    Friend WithEvents LblEmailID As System.Windows.Forms.Label
    Friend WithEvents txtLoginID As System.Windows.Forms.TextBox
    Friend WithEvents lblLoginid As System.Windows.Forms.Label
End Class
