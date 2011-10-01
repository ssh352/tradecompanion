<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoginForm1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub
    Friend WithEvents LogoPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents UsernameLabel As System.Windows.Forms.Label
    Friend WithEvents PasswordLabel As System.Windows.Forms.Label
    Friend WithEvents UsernameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PasswordTextBox As System.Windows.Forms.TextBox
    Friend WithEvents OK As System.Windows.Forms.Button

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LoginForm1))
        Me.LogoPictureBox = New System.Windows.Forms.PictureBox
        Me.UsernameLabel = New System.Windows.Forms.Label
        Me.PasswordLabel = New System.Windows.Forms.Label
        Me.UsernameTextBox = New System.Windows.Forms.TextBox
        Me.PasswordTextBox = New System.Windows.Forms.TextBox
        Me.OK = New System.Windows.Forms.Button
        Me.ColorDialog1 = New System.Windows.Forms.ColorDialog
        Me.ServerLabel = New System.Windows.Forms.Label
        Me.CmbServer = New System.Windows.Forms.ComboBox
        Me.chkRememberPassword = New System.Windows.Forms.CheckBox
        Me.createAccount = New System.Windows.Forms.Button
        Me.forgotPassword = New System.Windows.Forms.LinkLabel
        CType(Me.LogoPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LogoPictureBox
        '
        Me.LogoPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.LogoPictureBox.Image = CType(resources.GetObject("LogoPictureBox.Image"), System.Drawing.Image)
        Me.LogoPictureBox.Location = New System.Drawing.Point(-1, -2)
        Me.LogoPictureBox.Name = "LogoPictureBox"
        Me.LogoPictureBox.Size = New System.Drawing.Size(423, 105)
        Me.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.LogoPictureBox.TabIndex = 0
        Me.LogoPictureBox.TabStop = False
        '
        'UsernameLabel
        '
        Me.UsernameLabel.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UsernameLabel.ForeColor = System.Drawing.SystemColors.Highlight
        Me.UsernameLabel.Location = New System.Drawing.Point(42, 110)
        Me.UsernameLabel.Name = "UsernameLabel"
        Me.UsernameLabel.Size = New System.Drawing.Size(84, 23)
        Me.UsernameLabel.TabIndex = 0
        Me.UsernameLabel.Text = "&Loginid  "
        Me.UsernameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PasswordLabel
        '
        Me.PasswordLabel.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PasswordLabel.ForeColor = System.Drawing.SystemColors.Highlight
        Me.PasswordLabel.Location = New System.Drawing.Point(42, 142)
        Me.PasswordLabel.Name = "PasswordLabel"
        Me.PasswordLabel.Size = New System.Drawing.Size(84, 23)
        Me.PasswordLabel.TabIndex = 2
        Me.PasswordLabel.Text = "&Password"
        Me.PasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'UsernameTextBox
        '
        Me.UsernameTextBox.Location = New System.Drawing.Point(161, 113)
        Me.UsernameTextBox.Name = "UsernameTextBox"
        Me.UsernameTextBox.Size = New System.Drawing.Size(220, 20)
        Me.UsernameTextBox.TabIndex = 1
        '
        'PasswordTextBox
        '
        Me.PasswordTextBox.Location = New System.Drawing.Point(161, 145)
        Me.PasswordTextBox.Name = "PasswordTextBox"
        Me.PasswordTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.PasswordTextBox.Size = New System.Drawing.Size(220, 20)
        Me.PasswordTextBox.TabIndex = 2
        '
        'OK
        '
        Me.OK.BackColor = System.Drawing.Color.LightSteelBlue
        Me.OK.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.OK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.OK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.OK.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.OK.Location = New System.Drawing.Point(275, 228)
        Me.OK.Name = "OK"
        Me.OK.Size = New System.Drawing.Size(106, 27)
        Me.OK.TabIndex = 5
        Me.OK.Text = "&Login"
        Me.OK.UseVisualStyleBackColor = False
        '
        'ServerLabel
        '
        Me.ServerLabel.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ServerLabel.ForeColor = System.Drawing.SystemColors.Highlight
        Me.ServerLabel.Location = New System.Drawing.Point(42, 179)
        Me.ServerLabel.Name = "ServerLabel"
        Me.ServerLabel.Size = New System.Drawing.Size(84, 23)
        Me.ServerLabel.TabIndex = 8
        Me.ServerLabel.Text = "&Server"
        '
        'CmbServer
        '
        Me.CmbServer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbServer.FormattingEnabled = True
        Me.CmbServer.Items.AddRange(New Object() {"CurrenEx", "Ariel", "Espeed", "DBFX", "Gain", "Icap", "Dukascopy", "FxIntegral"})
        Me.CmbServer.Location = New System.Drawing.Point(161, 176)
        Me.CmbServer.Name = "CmbServer"
        Me.CmbServer.Size = New System.Drawing.Size(220, 21)
        Me.CmbServer.TabIndex = 3
        '
        'chkRememberPassword
        '
        Me.chkRememberPassword.AutoSize = True
        Me.chkRememberPassword.Location = New System.Drawing.Point(162, 205)
        Me.chkRememberPassword.Name = "chkRememberPassword"
        Me.chkRememberPassword.Size = New System.Drawing.Size(126, 17)
        Me.chkRememberPassword.TabIndex = 4
        Me.chkRememberPassword.Text = "Remember Password"
        Me.chkRememberPassword.UseVisualStyleBackColor = True
        '
        'createAccount
        '
        Me.createAccount.BackColor = System.Drawing.Color.LightSteelBlue
        Me.createAccount.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.createAccount.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.createAccount.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.createAccount.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.createAccount.Location = New System.Drawing.Point(161, 228)
        Me.createAccount.Name = "createAccount"
        Me.createAccount.Size = New System.Drawing.Size(104, 26)
        Me.createAccount.TabIndex = 7
        Me.createAccount.Text = "&Create Account"
        Me.createAccount.UseVisualStyleBackColor = False
        '
        'forgotPassword
        '
        Me.forgotPassword.AutoSize = True
        Me.forgotPassword.LinkColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.forgotPassword.Location = New System.Drawing.Point(288, 268)
        Me.forgotPassword.Name = "forgotPassword"
        Me.forgotPassword.Size = New System.Drawing.Size(95, 13)
        Me.forgotPassword.TabIndex = 9
        Me.forgotPassword.TabStop = True
        Me.forgotPassword.Text = "&Forgot Password ?"
        '
        'LoginForm1
        '
        Me.AcceptButton = Me.OK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(421, 290)
        Me.Controls.Add(Me.forgotPassword)
        Me.Controls.Add(Me.createAccount)
        Me.Controls.Add(Me.chkRememberPassword)
        Me.Controls.Add(Me.CmbServer)
        Me.Controls.Add(Me.ServerLabel)
        Me.Controls.Add(Me.OK)
        Me.Controls.Add(Me.PasswordTextBox)
        Me.Controls.Add(Me.UsernameTextBox)
        Me.Controls.Add(Me.PasswordLabel)
        Me.Controls.Add(Me.UsernameLabel)
        Me.Controls.Add(Me.LogoPictureBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LoginForm1"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "TradeCompanion"
        CType(Me.LogoPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ColorDialog1 As System.Windows.Forms.ColorDialog
    Friend WithEvents ServerLabel As System.Windows.Forms.Label
    Friend WithEvents CmbServer As System.Windows.Forms.ComboBox
    Friend WithEvents chkRememberPassword As System.Windows.Forms.CheckBox
    Friend WithEvents createAccount As System.Windows.Forms.Button
    Friend WithEvents forgotPassword As System.Windows.Forms.LinkLabel

End Class
