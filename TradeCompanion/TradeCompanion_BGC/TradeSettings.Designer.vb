<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TradeSettings
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
        Me.cmdButtonsPanel = New System.Windows.Forms.TableLayoutPanel
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.OK_Button = New System.Windows.Forms.Button
        Me.panelManualOrderSettings = New System.Windows.Forms.Panel
        Me.grpManulSettings = New System.Windows.Forms.GroupBox
        Me.lblTradeType = New System.Windows.Forms.Label
        Me.cmbTradeTypeManualOrder = New System.Windows.Forms.ComboBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.DiscardAlertInterval = New System.Windows.Forms.NumericUpDown
        Me.lblDiscardAlertInterval = New System.Windows.Forms.Label
        Me.cmbTradeTypeOver10mil = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmbTradeTypeAutomatic = New System.Windows.Forms.ComboBox
        Me.cmdButtonsPanel.SuspendLayout()
        Me.panelManualOrderSettings.SuspendLayout()
        Me.grpManulSettings.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.DiscardAlertInterval, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdButtonsPanel
        '
        Me.cmdButtonsPanel.ColumnCount = 2
        Me.cmdButtonsPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.36986!))
        Me.cmdButtonsPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.63014!))
        Me.cmdButtonsPanel.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.cmdButtonsPanel.Controls.Add(Me.OK_Button, 0, 0)
        Me.cmdButtonsPanel.Location = New System.Drawing.Point(72, 284)
        Me.cmdButtonsPanel.Name = "cmdButtonsPanel"
        Me.cmdButtonsPanel.RowCount = 1
        Me.cmdButtonsPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.cmdButtonsPanel.Size = New System.Drawing.Size(150, 33)
        Me.cmdButtonsPanel.TabIndex = 1
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
        Me.Cancel_Button.Location = New System.Drawing.Point(80, 5)
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
        Me.OK_Button.Location = New System.Drawing.Point(5, 5)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        Me.OK_Button.UseVisualStyleBackColor = False
        '
        'panelManualOrderSettings
        '
        Me.panelManualOrderSettings.Controls.Add(Me.grpManulSettings)
        Me.panelManualOrderSettings.Location = New System.Drawing.Point(25, 12)
        Me.panelManualOrderSettings.Name = "panelManualOrderSettings"
        Me.panelManualOrderSettings.Size = New System.Drawing.Size(244, 96)
        Me.panelManualOrderSettings.TabIndex = 2
        '
        'grpManulSettings
        '
        Me.grpManulSettings.Controls.Add(Me.lblTradeType)
        Me.grpManulSettings.Controls.Add(Me.cmbTradeTypeManualOrder)
        Me.grpManulSettings.Location = New System.Drawing.Point(2, 5)
        Me.grpManulSettings.Name = "grpManulSettings"
        Me.grpManulSettings.Size = New System.Drawing.Size(239, 82)
        Me.grpManulSettings.TabIndex = 0
        Me.grpManulSettings.TabStop = False
        Me.grpManulSettings.Text = "Manual Orders"
        '
        'lblTradeType
        '
        Me.lblTradeType.AutoSize = True
        Me.lblTradeType.Location = New System.Drawing.Point(2, 37)
        Me.lblTradeType.Name = "lblTradeType"
        Me.lblTradeType.Size = New System.Drawing.Size(91, 13)
        Me.lblTradeType.TabIndex = 18
        Me.lblTradeType.Text = "Default trade type"
        '
        'cmbTradeTypeManualOrder
        '
        Me.cmbTradeTypeManualOrder.BackColor = System.Drawing.Color.White
        Me.cmbTradeTypeManualOrder.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.cmbTradeTypeManualOrder.FormattingEnabled = True
        Me.cmbTradeTypeManualOrder.Items.AddRange(New Object() {"IOC", "GTC"})
        Me.cmbTradeTypeManualOrder.Location = New System.Drawing.Point(140, 34)
        Me.cmbTradeTypeManualOrder.Name = "cmbTradeTypeManualOrder"
        Me.cmbTradeTypeManualOrder.Size = New System.Drawing.Size(93, 21)
        Me.cmbTradeTypeManualOrder.TabIndex = 17
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.GroupBox1)
        Me.Panel2.Location = New System.Drawing.Point(25, 117)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(244, 153)
        Me.Panel2.TabIndex = 3
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.DiscardAlertInterval)
        Me.GroupBox1.Controls.Add(Me.lblDiscardAlertInterval)
        Me.GroupBox1.Controls.Add(Me.cmbTradeTypeOver10mil)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.cmbTradeTypeAutomatic)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(239, 144)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Automatic Execution"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(179, 112)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(52, 13)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "minute(s)."
        '
        'DiscardAlertInterval
        '
        Me.DiscardAlertInterval.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.DiscardAlertInterval.Location = New System.Drawing.Point(136, 109)
        Me.DiscardAlertInterval.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
        Me.DiscardAlertInterval.Name = "DiscardAlertInterval"
        Me.DiscardAlertInterval.Size = New System.Drawing.Size(37, 20)
        Me.DiscardAlertInterval.TabIndex = 13
        '
        'lblDiscardAlertInterval
        '
        Me.lblDiscardAlertInterval.AutoSize = True
        Me.lblDiscardAlertInterval.Location = New System.Drawing.Point(6, 114)
        Me.lblDiscardAlertInterval.Name = "lblDiscardAlertInterval"
        Me.lblDiscardAlertInterval.Size = New System.Drawing.Size(103, 13)
        Me.lblDiscardAlertInterval.TabIndex = 4
        Me.lblDiscardAlertInterval.Text = "Discard alert interval"
        '
        'cmbTradeTypeOver10mil
        '
        Me.cmbTradeTypeOver10mil.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.cmbTradeTypeOver10mil.FormattingEnabled = True
        Me.cmbTradeTypeOver10mil.Items.AddRange(New Object() {"IOC", "GTC"})
        Me.cmbTradeTypeOver10mil.Location = New System.Drawing.Point(136, 69)
        Me.cmbTradeTypeOver10mil.Name = "cmbTradeTypeOver10mil"
        Me.cmbTradeTypeOver10mil.Size = New System.Drawing.Size(93, 21)
        Me.cmbTradeTypeOver10mil.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 73)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(126, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Trade size over 10 million"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(91, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Default trade type"
        '
        'cmbTradeTypeAutomatic
        '
        Me.cmbTradeTypeAutomatic.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.cmbTradeTypeAutomatic.FormattingEnabled = True
        Me.cmbTradeTypeAutomatic.Items.AddRange(New Object() {"IOC", "GTC"})
        Me.cmbTradeTypeAutomatic.Location = New System.Drawing.Point(136, 29)
        Me.cmbTradeTypeAutomatic.Name = "cmbTradeTypeAutomatic"
        Me.cmbTradeTypeAutomatic.Size = New System.Drawing.Size(93, 21)
        Me.cmbTradeTypeAutomatic.TabIndex = 0
        '
        'TradeSettings
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(294, 329)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.panelManualOrderSettings)
        Me.Controls.Add(Me.cmdButtonsPanel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.Name = "TradeSettings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Trade Settings"
        Me.cmdButtonsPanel.ResumeLayout(False)
        Me.panelManualOrderSettings.ResumeLayout(False)
        Me.grpManulSettings.ResumeLayout(False)
        Me.grpManulSettings.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.DiscardAlertInterval, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cmdButtonsPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents panelManualOrderSettings As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents grpManulSettings As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblTradeType As System.Windows.Forms.Label
    Friend WithEvents cmbTradeTypeManualOrder As System.Windows.Forms.ComboBox
    Friend WithEvents cmbTradeTypeAutomatic As System.Windows.Forms.ComboBox
    Friend WithEvents cmbTradeTypeOver10mil As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblDiscardAlertInterval As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents DiscardAlertInterval As System.Windows.Forms.NumericUpDown
End Class
