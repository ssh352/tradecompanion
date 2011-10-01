<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoginAriel
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
        Me.txtAccountNo = New System.Windows.Forms.TextBox
        Me.txtUsername = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.reconnectInterval = New System.Windows.Forms.NumericUpDown
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.mailSettingToolTip = New System.Windows.Forms.ToolTip(Me.components)
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
        Me.cmdButtonsPanel.Location = New System.Drawing.Point(79, 132)
        Me.cmdButtonsPanel.Name = "cmdButtonsPanel"
        Me.cmdButtonsPanel.RowCount = 1
        Me.cmdButtonsPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.cmdButtonsPanel.Size = New System.Drawing.Size(146, 29)
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
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
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
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        Me.OK_Button.UseVisualStyleBackColor = False
        '
        'lblIP
        '
        Me.lblIP.AutoSize = True
        Me.lblIP.Location = New System.Drawing.Point(12, 8)
        Me.lblIP.Name = "lblIP"
        Me.lblIP.Size = New System.Drawing.Size(43, 13)
        Me.lblIP.TabIndex = 1
        Me.lblIP.Text = "User ID"
        '
        'txtAccountNo
        '
        Me.txtAccountNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAccountNo.Location = New System.Drawing.Point(75, 5)
        Me.txtAccountNo.Name = "txtAccountNo"
        Me.txtAccountNo.Size = New System.Drawing.Size(190, 20)
        Me.txtAccountNo.TabIndex = 2
        '
        'txtUsername
        '
        Me.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtUsername.Location = New System.Drawing.Point(76, 31)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(190, 20)
        Me.txtUsername.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "User Name"
        '
        'txtPassword
        '
        Me.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPassword.Location = New System.Drawing.Point(76, 57)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(190, 20)
        Me.txtPassword.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 60)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Password"
        '
        'reconnectInterval
        '
        Me.reconnectInterval.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.reconnectInterval.Location = New System.Drawing.Point(160, 98)
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
        Me.Label7.Location = New System.Drawing.Point(13, 100)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(141, 13)
        Me.Label7.TabIndex = 11
        Me.Label7.Text = "Attempt to reconnect, every "
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(203, 100)
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
        'LoginAriel
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(304, 171)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.reconnectInterval)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtUsername)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtAccountNo)
        Me.Controls.Add(Me.lblIP)
        Me.Controls.Add(Me.cmdButtonsPanel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LoginAriel"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Login Ariel Server"
        Me.cmdButtonsPanel.ResumeLayout(False)
        CType(Me.reconnectInterval, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdButtonsPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents lblIP As System.Windows.Forms.Label
    Friend WithEvents txtAccountNo As System.Windows.Forms.TextBox
    Friend WithEvents txtUsername As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents reconnectInterval As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents mailSettingToolTip As System.Windows.Forms.ToolTip

End Class
