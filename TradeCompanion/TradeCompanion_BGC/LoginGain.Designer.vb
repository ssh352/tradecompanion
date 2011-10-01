<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoginGain
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
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.reconnectInterval = New System.Windows.Forms.NumericUpDown
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtMDHost = New System.Windows.Forms.TextBox
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtBrand = New System.Windows.Forms.TextBox
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.txtUserName = New System.Windows.Forms.TextBox
        Me.lblIP = New System.Windows.Forms.Label
        Me.txtMDPort = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.cmbPlatform = New System.Windows.Forms.ComboBox
        Me.lblPlatform = New System.Windows.Forms.Label
        CType(Me.reconnectInterval, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.Cancel_Button.Location = New System.Drawing.Point(175, 202)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 21)
        Me.Cancel_Button.TabIndex = 28
        Me.Cancel_Button.Text = "Cancel"
        Me.Cancel_Button.UseVisualStyleBackColor = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(224, 173)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(56, 13)
        Me.Label8.TabIndex = 38
        Me.Label8.Text = "second(s)."
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(34, 173)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(141, 13)
        Me.Label7.TabIndex = 37
        Me.Label7.Text = "Attempt to reconnect, every "
        '
        'reconnectInterval
        '
        Me.reconnectInterval.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.reconnectInterval.Location = New System.Drawing.Point(181, 171)
        Me.reconnectInterval.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.reconnectInterval.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.reconnectInterval.Name = "reconnectInterval"
        Me.reconnectInterval.Size = New System.Drawing.Size(37, 20)
        Me.reconnectInterval.TabIndex = 36
        Me.reconnectInterval.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(19, 39)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 13)
        Me.Label1.TabIndex = 30
        Me.Label1.Text = "Password"
        Me.Label1.UseWaitCursor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(19, 65)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(35, 13)
        Me.Label2.TabIndex = 32
        Me.Label2.Text = "Brand"
        '
        'txtMDHost
        '
        Me.txtMDHost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMDHost.Location = New System.Drawing.Point(107, 88)
        Me.txtMDHost.Name = "txtMDHost"
        Me.txtMDHost.Size = New System.Drawing.Size(190, 20)
        Me.txtMDHost.TabIndex = 35
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.BackColor = System.Drawing.Color.LightSteelBlue
        Me.OK_Button.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.OK_Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.OK_Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.OK_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.OK_Button.Location = New System.Drawing.Point(102, 202)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 21)
        Me.OK_Button.TabIndex = 26
        Me.OK_Button.Text = "OK"
        Me.OK_Button.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(19, 91)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(86, 13)
        Me.Label3.TabIndex = 34
        Me.Label3.Text = "Marketdata Host"
        '
        'txtBrand
        '
        Me.txtBrand.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtBrand.Location = New System.Drawing.Point(107, 62)
        Me.txtBrand.Name = "txtBrand"
        Me.txtBrand.Size = New System.Drawing.Size(190, 20)
        Me.txtBrand.TabIndex = 33
        '
        'txtPassword
        '
        Me.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPassword.Location = New System.Drawing.Point(107, 36)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(190, 20)
        Me.txtPassword.TabIndex = 31
        '
        'txtUserName
        '
        Me.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtUserName.Location = New System.Drawing.Point(107, 10)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.Size = New System.Drawing.Size(190, 20)
        Me.txtUserName.TabIndex = 29
        '
        'lblIP
        '
        Me.lblIP.AutoSize = True
        Me.lblIP.Location = New System.Drawing.Point(18, 13)
        Me.lblIP.Name = "lblIP"
        Me.lblIP.Size = New System.Drawing.Size(60, 13)
        Me.lblIP.TabIndex = 27
        Me.lblIP.Text = "User Name"
        '
        'txtMDPort
        '
        Me.txtMDPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMDPort.Location = New System.Drawing.Point(107, 114)
        Me.txtMDPort.Name = "txtMDPort"
        Me.txtMDPort.Size = New System.Drawing.Size(190, 20)
        Me.txtMDPort.TabIndex = 40
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(19, 117)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(83, 13)
        Me.Label4.TabIndex = 39
        Me.Label4.Text = "Marketdata Port"
        '
        'cmbPlatform
        '
        Me.cmbPlatform.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPlatform.Items.AddRange(New Object() {"Practice", "Live"})
        Me.cmbPlatform.Location = New System.Drawing.Point(107, 140)
        Me.cmbPlatform.Name = "cmbPlatform"
        Me.cmbPlatform.Size = New System.Drawing.Size(190, 21)
        Me.cmbPlatform.TabIndex = 41
        '
        'lblPlatform
        '
        Me.lblPlatform.AutoSize = True
        Me.lblPlatform.Location = New System.Drawing.Point(19, 143)
        Me.lblPlatform.Name = "lblPlatform"
        Me.lblPlatform.Size = New System.Drawing.Size(45, 13)
        Me.lblPlatform.TabIndex = 42
        Me.lblPlatform.Text = "Platform"
        '
        'LoginGain
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(321, 233)
        Me.Controls.Add(Me.lblPlatform)
        Me.Controls.Add(Me.cmbPlatform)
        Me.Controls.Add(Me.txtMDPort)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Cancel_Button)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.reconnectInterval)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtMDHost)
        Me.Controls.Add(Me.OK_Button)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtBrand)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.txtUserName)
        Me.Controls.Add(Me.lblIP)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LoginGain"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "LoginGain"
        CType(Me.reconnectInterval, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents reconnectInterval As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtMDHost As System.Windows.Forms.TextBox
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtBrand As System.Windows.Forms.TextBox
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtUserName As System.Windows.Forms.TextBox
    Friend WithEvents lblIP As System.Windows.Forms.Label
    Friend WithEvents txtMDPort As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmbPlatform As System.Windows.Forms.ComboBox
    Friend WithEvents lblPlatform As System.Windows.Forms.Label
End Class
