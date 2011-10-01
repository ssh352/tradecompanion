<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoginIcap
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
        Me.tIPAddress = New System.Windows.Forms.TextBox
        Me.tPort = New System.Windows.Forms.TextBox
        Me.tUserId = New System.Windows.Forms.TextBox
        Me.tPassword = New System.Windows.Forms.TextBox
        Me.lIPAddress = New System.Windows.Forms.Label
        Me.lPort = New System.Windows.Forms.Label
        Me.lUserID = New System.Windows.Forms.Label
        Me.lPassword = New System.Windows.Forms.Label
        Me.bOk = New System.Windows.Forms.Button
        Me.bCancel = New System.Windows.Forms.Button
        Me.lText1 = New System.Windows.Forms.Label
        Me.Interval_combo = New System.Windows.Forms.NumericUpDown
        Me.lText2 = New System.Windows.Forms.Label
        CType(Me.Interval_combo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tIPAddress
        '
        Me.tIPAddress.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.tIPAddress.Location = New System.Drawing.Point(85, 10)
        Me.tIPAddress.Name = "tIPAddress"
        Me.tIPAddress.Size = New System.Drawing.Size(159, 20)
        Me.tIPAddress.TabIndex = 1
        '
        'tPort
        '
        Me.tPort.Location = New System.Drawing.Point(85, 39)
        Me.tPort.Name = "tPort"
        Me.tPort.Size = New System.Drawing.Size(159, 20)
        Me.tPort.TabIndex = 2
        '
        'tUserId
        '
        Me.tUserId.Location = New System.Drawing.Point(85, 69)
        Me.tUserId.Name = "tUserId"
        Me.tUserId.Size = New System.Drawing.Size(159, 20)
        Me.tUserId.TabIndex = 3
        '
        'tPassword
        '
        Me.tPassword.Location = New System.Drawing.Point(85, 100)
        Me.tPassword.Name = "tPassword"
        Me.tPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.tPassword.Size = New System.Drawing.Size(159, 20)
        Me.tPassword.TabIndex = 4
        '
        'lIPAddress
        '
        Me.lIPAddress.AutoSize = True
        Me.lIPAddress.Location = New System.Drawing.Point(21, 17)
        Me.lIPAddress.Name = "lIPAddress"
        Me.lIPAddress.Size = New System.Drawing.Size(58, 13)
        Me.lIPAddress.TabIndex = 6
        Me.lIPAddress.Text = "IP Address"
        '
        'lPort
        '
        Me.lPort.AutoSize = True
        Me.lPort.Location = New System.Drawing.Point(21, 46)
        Me.lPort.Name = "lPort"
        Me.lPort.Size = New System.Drawing.Size(26, 13)
        Me.lPort.TabIndex = 7
        Me.lPort.Text = "Port"
        '
        'lUserID
        '
        Me.lUserID.AutoSize = True
        Me.lUserID.Location = New System.Drawing.Point(21, 76)
        Me.lUserID.Name = "lUserID"
        Me.lUserID.Size = New System.Drawing.Size(40, 13)
        Me.lUserID.TabIndex = 8
        Me.lUserID.Text = "UserID"
        '
        'lPassword
        '
        Me.lPassword.AutoSize = True
        Me.lPassword.Location = New System.Drawing.Point(21, 107)
        Me.lPassword.Name = "lPassword"
        Me.lPassword.Size = New System.Drawing.Size(53, 13)
        Me.lPassword.TabIndex = 9
        Me.lPassword.Text = "Password"
        '
        'bOk
        '
        Me.bOk.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bOk.BackColor = System.Drawing.Color.LightSteelBlue
        Me.bOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.bOk.Location = New System.Drawing.Point(85, 170)
        Me.bOk.Name = "bOk"
        Me.bOk.Size = New System.Drawing.Size(69, 26)
        Me.bOk.TabIndex = 10
        Me.bOk.Text = "Ok"
        Me.bOk.UseVisualStyleBackColor = False
        '
        'bCancel
        '
        Me.bCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bCancel.BackColor = System.Drawing.Color.LightSteelBlue
        Me.bCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.bCancel.Location = New System.Drawing.Point(169, 170)
        Me.bCancel.Name = "bCancel"
        Me.bCancel.Size = New System.Drawing.Size(75, 26)
        Me.bCancel.TabIndex = 11
        Me.bCancel.Text = "Cancel"
        Me.bCancel.UseVisualStyleBackColor = False
        '
        'lText1
        '
        Me.lText1.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        Me.lText1.AutoSize = True
        Me.lText1.Location = New System.Drawing.Point(12, 138)
        Me.lText1.Name = "lText1"
        Me.lText1.Size = New System.Drawing.Size(141, 13)
        Me.lText1.TabIndex = 12
        Me.lText1.Text = "Attempt to reconnect, every "
        '
        'Interval_combo
        '
        Me.Interval_combo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Interval_combo.Location = New System.Drawing.Point(159, 136)
        Me.Interval_combo.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.Interval_combo.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.Interval_combo.Name = "Interval_combo"
        Me.Interval_combo.Size = New System.Drawing.Size(37, 20)
        Me.Interval_combo.TabIndex = 13
        Me.Interval_combo.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'lText2
        '
        Me.lText2.AutoSize = True
        Me.lText2.Location = New System.Drawing.Point(202, 138)
        Me.lText2.Name = "lText2"
        Me.lText2.Size = New System.Drawing.Size(56, 13)
        Me.lText2.TabIndex = 14
        Me.lText2.Text = "second(s)."
        '
        'LoginIcap
        '
        Me.AcceptButton = Me.bOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(270, 208)
        Me.Controls.Add(Me.lText2)
        Me.Controls.Add(Me.Interval_combo)
        Me.Controls.Add(Me.lText1)
        Me.Controls.Add(Me.bCancel)
        Me.Controls.Add(Me.bOk)
        Me.Controls.Add(Me.lPassword)
        Me.Controls.Add(Me.lUserID)
        Me.Controls.Add(Me.lPort)
        Me.Controls.Add(Me.lIPAddress)
        Me.Controls.Add(Me.tPassword)
        Me.Controls.Add(Me.tUserId)
        Me.Controls.Add(Me.tPort)
        Me.Controls.Add(Me.tIPAddress)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LoginIcap"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "LoginIcap"
        CType(Me.Interval_combo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tIPAddress As System.Windows.Forms.TextBox
    Friend WithEvents tPort As System.Windows.Forms.TextBox
    Friend WithEvents tUserId As System.Windows.Forms.TextBox
    Friend WithEvents tPassword As System.Windows.Forms.TextBox
    Friend WithEvents lIPAddress As System.Windows.Forms.Label
    Friend WithEvents lPort As System.Windows.Forms.Label
    Friend WithEvents lUserID As System.Windows.Forms.Label
    Friend WithEvents lPassword As System.Windows.Forms.Label
    Friend WithEvents bOk As System.Windows.Forms.Button
    Friend WithEvents bCancel As System.Windows.Forms.Button
    Friend WithEvents lText1 As System.Windows.Forms.Label
    Friend WithEvents Interval_combo As System.Windows.Forms.NumericUpDown
    Friend WithEvents lText2 As System.Windows.Forms.Label
End Class
