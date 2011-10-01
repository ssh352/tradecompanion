<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoginCurrenEX
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
        Me.components = New System.ComponentModel.Container
        Me.cmdButtonsPanel = New System.Windows.Forms.TableLayoutPanel
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.OK_Button = New System.Windows.Forms.Button
        Me.lblIP = New System.Windows.Forms.Label
        Me.txtIP = New System.Windows.Forms.TextBox
        Me.txtPort = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtSender = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtTarget = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.reconnectInterval = New System.Windows.Forms.NumericUpDown
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.mailSettingToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.lblPassword = New System.Windows.Forms.Label
        Me.cmdButtonsPanel.SuspendLayout()
        CType(Me.reconnectInterval, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdButtonsPanel
        '
        Me.cmdButtonsPanel.ColumnCount = 2
        Me.cmdButtonsPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.cmdButtonsPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.cmdButtonsPanel.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.cmdButtonsPanel.Controls.Add(Me.OK_Button, 0, 0)
        Me.cmdButtonsPanel.Location = New System.Drawing.Point(71, 166)
        Me.cmdButtonsPanel.Name = "cmdButtonsPanel"
        Me.cmdButtonsPanel.RowCount = 1
        Me.cmdButtonsPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.cmdButtonsPanel.Size = New System.Drawing.Size(177, 30)
        Me.cmdButtonsPanel.TabIndex = 0
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
        Me.Cancel_Button.TabIndex = 1
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
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        Me.OK_Button.UseVisualStyleBackColor = False
        '
        'lblIP
        '
        Me.lblIP.AutoSize = True
        Me.lblIP.Location = New System.Drawing.Point(13, 15)
        Me.lblIP.Name = "lblIP"
        Me.lblIP.Size = New System.Drawing.Size(57, 13)
        Me.lblIP.TabIndex = 0
        Me.lblIP.Text = "IP address"
        '
        'txtIP
        '
        Me.txtIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtIP.Location = New System.Drawing.Point(71, 11)
        Me.txtIP.Name = "txtIP"
        Me.txtIP.Size = New System.Drawing.Size(190, 20)
        Me.txtIP.TabIndex = 1
        '
        'txtPort
        '
        Me.txtPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPort.Location = New System.Drawing.Point(71, 36)
        Me.txtPort.Name = "txtPort"
        Me.txtPort.Size = New System.Drawing.Size(190, 20)
        Me.txtPort.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 40)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(26, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Port"
        '
        'txtSender
        '
        Me.txtSender.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSender.Location = New System.Drawing.Point(71, 61)
        Me.txtSender.Name = "txtSender"
        Me.txtSender.Size = New System.Drawing.Size(190, 20)
        Me.txtSender.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 65)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(41, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Sender"
        '
        'txtTarget
        '
        Me.txtTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTarget.Location = New System.Drawing.Point(71, 111)
        Me.txtTarget.Name = "txtTarget"
        Me.txtTarget.Size = New System.Drawing.Size(190, 20)
        Me.txtTarget.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(13, 115)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(38, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Target"
        '
        'reconnectInterval
        '
        Me.reconnectInterval.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.reconnectInterval.Location = New System.Drawing.Point(158, 140)
        Me.reconnectInterval.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.reconnectInterval.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.reconnectInterval.Name = "reconnectInterval"
        Me.reconnectInterval.Size = New System.Drawing.Size(37, 20)
        Me.reconnectInterval.TabIndex = 10
        Me.reconnectInterval.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(13, 142)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(141, 13)
        Me.Label7.TabIndex = 11
        Me.Label7.Text = "Attempt to reconnect, every "
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(205, 142)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(56, 13)
        Me.Label8.TabIndex = 12
        Me.Label8.Text = "second(s)."
        '
        'mailSettingToolTip
        '
        Me.mailSettingToolTip.AutoPopDelay = 5000
        Me.mailSettingToolTip.InitialDelay = 0
        Me.mailSettingToolTip.IsBalloon = True
        Me.mailSettingToolTip.ReshowDelay = 10
        Me.mailSettingToolTip.ShowAlways = True
        '
        'txtPassword
        '
        Me.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPassword.Location = New System.Drawing.Point(71, 86)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(190, 20)
        Me.txtPassword.TabIndex = 4
        '
        'lblPassword
        '
        Me.lblPassword.AutoSize = True
        Me.lblPassword.Location = New System.Drawing.Point(13, 90)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(53, 13)
        Me.lblPassword.TabIndex = 0
        Me.lblPassword.Text = "Password"
        '
        'LoginCurrenEX
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(274, 203)
        Me.Controls.Add(Me.lblPassword)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.reconnectInterval)
        Me.Controls.Add(Me.txtTarget)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtSender)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtPort)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtIP)
        Me.Controls.Add(Me.lblIP)
        Me.Controls.Add(Me.cmdButtonsPanel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LoginCurrenEX"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Login"
        Me.cmdButtonsPanel.ResumeLayout(False)
        CType(Me.reconnectInterval, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdButtonsPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents lblIP As System.Windows.Forms.Label
    Friend WithEvents txtIP As System.Windows.Forms.TextBox
    Friend WithEvents txtPort As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtSender As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtTarget As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents reconnectInterval As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents mailSettingToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents lblPassword As System.Windows.Forms.Label

End Class
