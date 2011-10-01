<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoginMarketData
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
        Me.txtTarget = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtSender = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtPort = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtIP = New System.Windows.Forms.TextBox
        Me.lblIP = New System.Windows.Forms.Label
        Me.cmdButtonsPanel = New System.Windows.Forms.TableLayoutPanel
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.currenexPan = New System.Windows.Forms.Panel
        Me.dukascopyPan = New System.Windows.Forms.Panel
        Me.dkSendertxt = New System.Windows.Forms.TextBox
        Me.dkUserNametxt = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.dkTargettxt = New System.Windows.Forms.TextBox
        Me.dkPasswdtxt = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.dkPorttxt = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.dkIPtxt = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.PnlFXintegral = New System.Windows.Forms.Panel
        Me.FXtxtSender = New System.Windows.Forms.TextBox
        Me.FXtxtUserName = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.FXtxtTarget = New System.Windows.Forms.TextBox
        Me.FXtxtPass = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.FXtxtPort = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.FXtxtIP = New System.Windows.Forms.TextBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.FXtxtLegalEntity = New System.Windows.Forms.TextBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.cmdButtonsPanel.SuspendLayout()
        Me.currenexPan.SuspendLayout()
        Me.dukascopyPan.SuspendLayout()
        Me.PnlFXintegral.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtTarget
        '
        Me.txtTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTarget.Location = New System.Drawing.Point(84, 122)
        Me.txtTarget.Name = "txtTarget"
        Me.txtTarget.Size = New System.Drawing.Size(190, 20)
        Me.txtTarget.TabIndex = 16
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(21, 125)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(38, 13)
        Me.Label3.TabIndex = 15
        Me.Label3.Text = "Target"
        '
        'txtSender
        '
        Me.txtSender.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSender.Location = New System.Drawing.Point(84, 68)
        Me.txtSender.Name = "txtSender"
        Me.txtSender.Size = New System.Drawing.Size(190, 20)
        Me.txtSender.TabIndex = 14
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(21, 71)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(41, 13)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "Sender"
        '
        'txtPort
        '
        Me.txtPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPort.Location = New System.Drawing.Point(84, 42)
        Me.txtPort.Name = "txtPort"
        Me.txtPort.Size = New System.Drawing.Size(190, 20)
        Me.txtPort.TabIndex = 12
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(21, 45)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(26, 13)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Port"
        '
        'txtIP
        '
        Me.txtIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtIP.Location = New System.Drawing.Point(83, 16)
        Me.txtIP.Name = "txtIP"
        Me.txtIP.Size = New System.Drawing.Size(190, 20)
        Me.txtIP.TabIndex = 10
        '
        'lblIP
        '
        Me.lblIP.AutoSize = True
        Me.lblIP.Location = New System.Drawing.Point(20, 19)
        Me.lblIP.Name = "lblIP"
        Me.lblIP.Size = New System.Drawing.Size(57, 13)
        Me.lblIP.TabIndex = 9
        Me.lblIP.Text = "IP address"
        '
        'cmdButtonsPanel
        '
        Me.cmdButtonsPanel.ColumnCount = 2
        Me.cmdButtonsPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.cmdButtonsPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.cmdButtonsPanel.Controls.Add(Me.OK_Button, 0, 0)
        Me.cmdButtonsPanel.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.cmdButtonsPanel.Location = New System.Drawing.Point(97, 216)
        Me.cmdButtonsPanel.Name = "cmdButtonsPanel"
        Me.cmdButtonsPanel.RowCount = 1
        Me.cmdButtonsPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.cmdButtonsPanel.Size = New System.Drawing.Size(146, 29)
        Me.cmdButtonsPanel.TabIndex = 17
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
        'txtPassword
        '
        Me.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPassword.Location = New System.Drawing.Point(84, 94)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(190, 20)
        Me.txtPassword.TabIndex = 19
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(21, 97)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 13)
        Me.Label4.TabIndex = 18
        Me.Label4.Text = "Password"
        '
        'currenexPan
        '
        Me.currenexPan.Controls.Add(Me.txtSender)
        Me.currenexPan.Controls.Add(Me.Label1)
        Me.currenexPan.Controls.Add(Me.txtTarget)
        Me.currenexPan.Controls.Add(Me.txtPassword)
        Me.currenexPan.Controls.Add(Me.txtPort)
        Me.currenexPan.Controls.Add(Me.lblIP)
        Me.currenexPan.Controls.Add(Me.txtIP)
        Me.currenexPan.Controls.Add(Me.Label2)
        Me.currenexPan.Controls.Add(Me.Label3)
        Me.currenexPan.Controls.Add(Me.Label4)
        Me.currenexPan.Location = New System.Drawing.Point(12, 12)
        Me.currenexPan.Name = "currenexPan"
        Me.currenexPan.Size = New System.Drawing.Size(292, 157)
        Me.currenexPan.TabIndex = 20
        Me.currenexPan.Visible = False
        '
        'dukascopyPan
        '
        Me.dukascopyPan.Controls.Add(Me.dkSendertxt)
        Me.dukascopyPan.Controls.Add(Me.dkUserNametxt)
        Me.dukascopyPan.Controls.Add(Me.Label6)
        Me.dukascopyPan.Controls.Add(Me.dkTargettxt)
        Me.dukascopyPan.Controls.Add(Me.dkPasswdtxt)
        Me.dukascopyPan.Controls.Add(Me.Label7)
        Me.dukascopyPan.Controls.Add(Me.dkPorttxt)
        Me.dukascopyPan.Controls.Add(Me.Label8)
        Me.dukascopyPan.Controls.Add(Me.dkIPtxt)
        Me.dukascopyPan.Controls.Add(Me.Label9)
        Me.dukascopyPan.Controls.Add(Me.Label10)
        Me.dukascopyPan.Controls.Add(Me.Label11)
        Me.dukascopyPan.Location = New System.Drawing.Point(12, 12)
        Me.dukascopyPan.Name = "dukascopyPan"
        Me.dukascopyPan.Size = New System.Drawing.Size(289, 171)
        Me.dukascopyPan.TabIndex = 20
        Me.dukascopyPan.Visible = False
        '
        'dkSendertxt
        '
        Me.dkSendertxt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.dkSendertxt.Location = New System.Drawing.Point(82, 63)
        Me.dkSendertxt.Name = "dkSendertxt"
        Me.dkSendertxt.Size = New System.Drawing.Size(190, 20)
        Me.dkSendertxt.TabIndex = 14
        '
        'dkUserNametxt
        '
        Me.dkUserNametxt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.dkUserNametxt.Location = New System.Drawing.Point(81, 89)
        Me.dkUserNametxt.Name = "dkUserNametxt"
        Me.dkUserNametxt.Size = New System.Drawing.Size(190, 20)
        Me.dkUserNametxt.TabIndex = 16
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(19, 40)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(26, 13)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "Port"
        '
        'dkTargettxt
        '
        Me.dkTargettxt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.dkTargettxt.Location = New System.Drawing.Point(81, 140)
        Me.dkTargettxt.Name = "dkTargettxt"
        Me.dkTargettxt.Size = New System.Drawing.Size(190, 20)
        Me.dkTargettxt.TabIndex = 20
        '
        'dkPasswdtxt
        '
        Me.dkPasswdtxt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.dkPasswdtxt.Location = New System.Drawing.Point(81, 114)
        Me.dkPasswdtxt.Name = "dkPasswdtxt"
        Me.dkPasswdtxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.dkPasswdtxt.Size = New System.Drawing.Size(190, 20)
        Me.dkPasswdtxt.TabIndex = 18
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(17, 93)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(60, 13)
        Me.Label7.TabIndex = 13
        Me.Label7.Text = "User Name"
        '
        'dkPorttxt
        '
        Me.dkPorttxt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.dkPorttxt.Location = New System.Drawing.Point(82, 37)
        Me.dkPorttxt.Name = "dkPorttxt"
        Me.dkPorttxt.Size = New System.Drawing.Size(190, 20)
        Me.dkPorttxt.TabIndex = 12
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(18, 14)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(57, 13)
        Me.Label8.TabIndex = 9
        Me.Label8.Text = "IP address"
        '
        'dkIPtxt
        '
        Me.dkIPtxt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.dkIPtxt.Location = New System.Drawing.Point(82, 11)
        Me.dkIPtxt.Name = "dkIPtxt"
        Me.dkIPtxt.Size = New System.Drawing.Size(189, 20)
        Me.dkIPtxt.TabIndex = 10
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(19, 66)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(41, 13)
        Me.Label9.TabIndex = 13
        Me.Label9.Text = "Sender"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(18, 143)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(38, 13)
        Me.Label10.TabIndex = 15
        Me.Label10.Text = "Target"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(18, 117)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(53, 13)
        Me.Label11.TabIndex = 18
        Me.Label11.Text = "Password"
        '
        'PnlFXintegral
        '
        Me.PnlFXintegral.Controls.Add(Me.FXtxtLegalEntity)
        Me.PnlFXintegral.Controls.Add(Me.Label17)
        Me.PnlFXintegral.Controls.Add(Me.FXtxtSender)
        Me.PnlFXintegral.Controls.Add(Me.FXtxtUserName)
        Me.PnlFXintegral.Controls.Add(Me.Label5)
        Me.PnlFXintegral.Controls.Add(Me.FXtxtTarget)
        Me.PnlFXintegral.Controls.Add(Me.FXtxtPass)
        Me.PnlFXintegral.Controls.Add(Me.Label12)
        Me.PnlFXintegral.Controls.Add(Me.FXtxtPort)
        Me.PnlFXintegral.Controls.Add(Me.Label13)
        Me.PnlFXintegral.Controls.Add(Me.FXtxtIP)
        Me.PnlFXintegral.Controls.Add(Me.Label14)
        Me.PnlFXintegral.Controls.Add(Me.Label15)
        Me.PnlFXintegral.Controls.Add(Me.Label16)
        Me.PnlFXintegral.Location = New System.Drawing.Point(15, 9)
        Me.PnlFXintegral.Name = "PnlFXintegral"
        Me.PnlFXintegral.Size = New System.Drawing.Size(289, 186)
        Me.PnlFXintegral.TabIndex = 21
        Me.PnlFXintegral.Visible = False
        '
        'FXtxtSender
        '
        Me.FXtxtSender.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FXtxtSender.Location = New System.Drawing.Point(82, 63)
        Me.FXtxtSender.Name = "FXtxtSender"
        Me.FXtxtSender.Size = New System.Drawing.Size(190, 20)
        Me.FXtxtSender.TabIndex = 14
        '
        'FXtxtUserName
        '
        Me.FXtxtUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FXtxtUserName.Location = New System.Drawing.Point(81, 89)
        Me.FXtxtUserName.Name = "FXtxtUserName"
        Me.FXtxtUserName.Size = New System.Drawing.Size(190, 20)
        Me.FXtxtUserName.TabIndex = 16
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(19, 40)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(26, 13)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Port"
        '
        'FXtxtTarget
        '
        Me.FXtxtTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FXtxtTarget.Location = New System.Drawing.Point(81, 140)
        Me.FXtxtTarget.Name = "FXtxtTarget"
        Me.FXtxtTarget.Size = New System.Drawing.Size(190, 20)
        Me.FXtxtTarget.TabIndex = 20
        '
        'FXtxtPass
        '
        Me.FXtxtPass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FXtxtPass.Location = New System.Drawing.Point(81, 114)
        Me.FXtxtPass.Name = "FXtxtPass"
        Me.FXtxtPass.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.FXtxtPass.Size = New System.Drawing.Size(190, 20)
        Me.FXtxtPass.TabIndex = 18
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(17, 93)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(60, 13)
        Me.Label12.TabIndex = 13
        Me.Label12.Text = "User Name"
        '
        'FXtxtPort
        '
        Me.FXtxtPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FXtxtPort.Location = New System.Drawing.Point(82, 37)
        Me.FXtxtPort.Name = "FXtxtPort"
        Me.FXtxtPort.Size = New System.Drawing.Size(190, 20)
        Me.FXtxtPort.TabIndex = 12
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(18, 14)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(57, 13)
        Me.Label13.TabIndex = 9
        Me.Label13.Text = "IP address"
        '
        'FXtxtIP
        '
        Me.FXtxtIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FXtxtIP.Location = New System.Drawing.Point(82, 11)
        Me.FXtxtIP.Name = "FXtxtIP"
        Me.FXtxtIP.Size = New System.Drawing.Size(189, 20)
        Me.FXtxtIP.TabIndex = 10
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(19, 66)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(41, 13)
        Me.Label14.TabIndex = 13
        Me.Label14.Text = "Sender"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(18, 143)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(38, 13)
        Me.Label15.TabIndex = 15
        Me.Label15.Text = "Target"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(18, 117)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(53, 13)
        Me.Label16.TabIndex = 18
        Me.Label16.Text = "Password"
        '
        'FXtxtLegalEntity
        '
        Me.FXtxtLegalEntity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FXtxtLegalEntity.Location = New System.Drawing.Point(81, 163)
        Me.FXtxtLegalEntity.Name = "FXtxtLegalEntity"
        Me.FXtxtLegalEntity.Size = New System.Drawing.Size(190, 20)
        Me.FXtxtLegalEntity.TabIndex = 22
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(18, 166)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(62, 13)
        Me.Label17.TabIndex = 21
        Me.Label17.Text = "Legal Entity"
        '
        'LoginMarketData
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ClientSize = New System.Drawing.Size(317, 251)
        Me.Controls.Add(Me.PnlFXintegral)
        Me.Controls.Add(Me.dukascopyPan)
        Me.Controls.Add(Me.currenexPan)
        Me.Controls.Add(Me.cmdButtonsPanel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LoginMarketData"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Login MarketData   "
        Me.cmdButtonsPanel.ResumeLayout(False)
        Me.currenexPan.ResumeLayout(False)
        Me.currenexPan.PerformLayout()
        Me.dukascopyPan.ResumeLayout(False)
        Me.dukascopyPan.PerformLayout()
        Me.PnlFXintegral.ResumeLayout(False)
        Me.PnlFXintegral.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtTarget As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtSender As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtPort As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtIP As System.Windows.Forms.TextBox
    Friend WithEvents lblIP As System.Windows.Forms.Label
    Friend WithEvents cmdButtonsPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents currenexPan As System.Windows.Forms.Panel
    Friend WithEvents dukascopyPan As System.Windows.Forms.Panel
    Friend WithEvents dkSendertxt As System.Windows.Forms.TextBox
    Friend WithEvents dkUserNametxt As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents dkTargettxt As System.Windows.Forms.TextBox
    Friend WithEvents dkPasswdtxt As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents dkPorttxt As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents dkIPtxt As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents PnlFXintegral As System.Windows.Forms.Panel
    Friend WithEvents FXtxtLegalEntity As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents FXtxtSender As System.Windows.Forms.TextBox
    Friend WithEvents FXtxtUserName As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents FXtxtTarget As System.Windows.Forms.TextBox
    Friend WithEvents FXtxtPass As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents FXtxtPort As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents FXtxtIP As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
End Class
