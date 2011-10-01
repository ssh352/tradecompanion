<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoginFxIntegral
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
        Me.lblPassword = New System.Windows.Forms.Label
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.reconnectInterval = New System.Windows.Forms.NumericUpDown
        Me.txtTarget = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtUserName = New System.Windows.Forms.TextBox
        Me.txtSender = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtPort = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtIP = New System.Windows.Forms.TextBox
        Me.lblIP = New System.Windows.Forms.Label
        Me.cmdButtonsPanel = New System.Windows.Forms.TableLayoutPanel
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.OK_Button = New System.Windows.Forms.Button
        Me.txtLegalEntity = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        CType(Me.reconnectInterval, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmdButtonsPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblPassword
        '
        Me.lblPassword.AutoSize = True
        Me.lblPassword.Location = New System.Drawing.Point(17, 124)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(53, 13)
        Me.lblPassword.TabIndex = 19
        Me.lblPassword.Text = "Password"
        '
        'txtPassword
        '
        Me.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPassword.Location = New System.Drawing.Point(78, 120)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(190, 20)
        Me.txtPassword.TabIndex = 24
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(212, 204)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(56, 13)
        Me.Label8.TabIndex = 28
        Me.Label8.Text = "second(s)."
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(20, 204)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(141, 13)
        Me.Label7.TabIndex = 27
        Me.Label7.Text = "Attempt to reconnect, every "
        '
        'reconnectInterval
        '
        Me.reconnectInterval.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.reconnectInterval.Location = New System.Drawing.Point(165, 202)
        Me.reconnectInterval.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.reconnectInterval.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.reconnectInterval.Name = "reconnectInterval"
        Me.reconnectInterval.Size = New System.Drawing.Size(37, 20)
        Me.reconnectInterval.TabIndex = 26
        Me.reconnectInterval.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'txtTarget
        '
        Me.txtTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTarget.Location = New System.Drawing.Point(78, 145)
        Me.txtTarget.Name = "txtTarget"
        Me.txtTarget.Size = New System.Drawing.Size(190, 20)
        Me.txtTarget.TabIndex = 25
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(15, 149)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(38, 13)
        Me.Label3.TabIndex = 15
        Me.Label3.Text = "Target"
        '
        'txtUserName
        '
        Me.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtUserName.Location = New System.Drawing.Point(78, 94)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.Size = New System.Drawing.Size(190, 20)
        Me.txtUserName.TabIndex = 23
        '
        'txtSender
        '
        Me.txtSender.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSender.Location = New System.Drawing.Point(78, 68)
        Me.txtSender.Name = "txtSender"
        Me.txtSender.Size = New System.Drawing.Size(190, 20)
        Me.txtSender.TabIndex = 22
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(16, 99)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(60, 13)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "User Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(17, 72)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(41, 13)
        Me.Label2.TabIndex = 18
        Me.Label2.Text = "Sender"
        '
        'txtPort
        '
        Me.txtPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPort.Location = New System.Drawing.Point(78, 43)
        Me.txtPort.Name = "txtPort"
        Me.txtPort.Size = New System.Drawing.Size(190, 20)
        Me.txtPort.TabIndex = 21
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 47)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(26, 13)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Port"
        '
        'txtIP
        '
        Me.txtIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtIP.Location = New System.Drawing.Point(78, 18)
        Me.txtIP.Name = "txtIP"
        Me.txtIP.Size = New System.Drawing.Size(190, 20)
        Me.txtIP.TabIndex = 20
        '
        'lblIP
        '
        Me.lblIP.AutoSize = True
        Me.lblIP.Location = New System.Drawing.Point(20, 22)
        Me.lblIP.Name = "lblIP"
        Me.lblIP.Size = New System.Drawing.Size(57, 13)
        Me.lblIP.TabIndex = 16
        Me.lblIP.Text = "IP address"
        '
        'cmdButtonsPanel
        '
        Me.cmdButtonsPanel.ColumnCount = 2
        Me.cmdButtonsPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.cmdButtonsPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.cmdButtonsPanel.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.cmdButtonsPanel.Controls.Add(Me.OK_Button, 0, 0)
        Me.cmdButtonsPanel.Location = New System.Drawing.Point(74, 233)
        Me.cmdButtonsPanel.Name = "cmdButtonsPanel"
        Me.cmdButtonsPanel.RowCount = 1
        Me.cmdButtonsPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.cmdButtonsPanel.Size = New System.Drawing.Size(177, 30)
        Me.cmdButtonsPanel.TabIndex = 14
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
        Me.Cancel_Button.Location = New System.Drawing.Point(96, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(72, 23)
        Me.Cancel_Button.TabIndex = 8
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
        Me.OK_Button.Location = New System.Drawing.Point(8, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(72, 23)
        Me.OK_Button.TabIndex = 7
        Me.OK_Button.Text = "OK"
        Me.OK_Button.UseVisualStyleBackColor = False
        '
        'txtLegalEntity
        '
        Me.txtLegalEntity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtLegalEntity.Location = New System.Drawing.Point(78, 171)
        Me.txtLegalEntity.Name = "txtLegalEntity"
        Me.txtLegalEntity.Size = New System.Drawing.Size(190, 20)
        Me.txtLegalEntity.TabIndex = 30
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(15, 175)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(62, 13)
        Me.Label5.TabIndex = 29
        Me.Label5.Text = "Legal Entity"
        '
        'LoginFxIntegral
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(285, 270)
        Me.Controls.Add(Me.txtLegalEntity)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.lblPassword)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.reconnectInterval)
        Me.Controls.Add(Me.txtTarget)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtUserName)
        Me.Controls.Add(Me.txtSender)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtPort)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtIP)
        Me.Controls.Add(Me.lblIP)
        Me.Controls.Add(Me.cmdButtonsPanel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "LoginFxIntegral"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "LoginFxIntegral"
        CType(Me.reconnectInterval, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmdButtonsPanel.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblPassword As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents reconnectInterval As System.Windows.Forms.NumericUpDown
    Friend WithEvents txtTarget As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtUserName As System.Windows.Forms.TextBox
    Friend WithEvents txtSender As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtPort As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtIP As System.Windows.Forms.TextBox
    Friend WithEvents lblIP As System.Windows.Forms.Label
    Friend WithEvents cmdButtonsPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents txtLegalEntity As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
End Class
