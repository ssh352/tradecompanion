<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ManualAlert
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
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.cmbQuantity = New System.Windows.Forms.ComboBox
        Me.lblPrice = New System.Windows.Forms.Label
        Me.rbBuy = New System.Windows.Forms.RadioButton
        Me.rbSell = New System.Windows.Forms.RadioButton
        Me.txtPrice = New System.Windows.Forms.TextBox
        Me.lblSenderID = New System.Windows.Forms.Label
        Me.cmbSenderID = New System.Windows.Forms.ComboBox
        Me.lblTradeType = New System.Windows.Forms.Label
        Me.cmbTradeType = New System.Windows.Forms.ComboBox
        Me.TxtCurrency = New System.Windows.Forms.TextBox
        Me.Quantity = New System.Windows.Forms.Label
        Me.Action = New System.Windows.Forms.Label
        Me.CurencyLbl = New System.Windows.Forms.Label
        Me.Symbol = New System.Windows.Forms.Label
        Me.cmbSymbol = New System.Windows.Forms.ComboBox
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnAccept = New System.Windows.Forms.Button
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Panel1.Controls.Add(Me.cmbQuantity)
        Me.Panel1.Controls.Add(Me.lblPrice)
        Me.Panel1.Controls.Add(Me.rbBuy)
        Me.Panel1.Controls.Add(Me.rbSell)
        Me.Panel1.Controls.Add(Me.txtPrice)
        Me.Panel1.Controls.Add(Me.lblSenderID)
        Me.Panel1.Controls.Add(Me.cmbSenderID)
        Me.Panel1.Controls.Add(Me.lblTradeType)
        Me.Panel1.Controls.Add(Me.cmbTradeType)
        Me.Panel1.Controls.Add(Me.TxtCurrency)
        Me.Panel1.Controls.Add(Me.Quantity)
        Me.Panel1.Controls.Add(Me.Action)
        Me.Panel1.Controls.Add(Me.CurencyLbl)
        Me.Panel1.Controls.Add(Me.Symbol)
        Me.Panel1.Controls.Add(Me.cmbSymbol)
        Me.Panel1.Controls.Add(Me.btnCancel)
        Me.Panel1.Controls.Add(Me.btnAccept)
        Me.Panel1.Location = New System.Drawing.Point(1, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(284, 206)
        Me.Panel1.TabIndex = 4
        '
        'cmbQuantity
        '
        Me.cmbQuantity.FormattingEnabled = True
        Me.cmbQuantity.Location = New System.Drawing.Point(70, 142)
        Me.cmbQuantity.Name = "cmbQuantity"
        Me.cmbQuantity.Size = New System.Drawing.Size(121, 21)
        Me.cmbQuantity.TabIndex = 23
        '
        'lblPrice
        '
        Me.lblPrice.AutoSize = True
        Me.lblPrice.Location = New System.Drawing.Point(209, 150)
        Me.lblPrice.Name = "lblPrice"
        Me.lblPrice.Size = New System.Drawing.Size(31, 13)
        Me.lblPrice.TabIndex = 22
        Me.lblPrice.Text = "Price"
        Me.lblPrice.Visible = False
        '
        'rbBuy
        '
        Me.rbBuy.AutoSize = True
        Me.rbBuy.Location = New System.Drawing.Point(148, 111)
        Me.rbBuy.Name = "rbBuy"
        Me.rbBuy.Size = New System.Drawing.Size(43, 17)
        Me.rbBuy.TabIndex = 21
        Me.rbBuy.TabStop = True
        Me.rbBuy.Text = "Buy"
        Me.rbBuy.UseVisualStyleBackColor = True
        '
        'rbSell
        '
        Me.rbSell.AutoSize = True
        Me.rbSell.Location = New System.Drawing.Point(70, 111)
        Me.rbSell.Name = "rbSell"
        Me.rbSell.Size = New System.Drawing.Size(42, 17)
        Me.rbSell.TabIndex = 20
        Me.rbSell.TabStop = True
        Me.rbSell.Text = "Sell"
        Me.rbSell.UseVisualStyleBackColor = True
        '
        'txtPrice
        '
        Me.txtPrice.Enabled = False
        Me.txtPrice.Location = New System.Drawing.Point(212, 173)
        Me.txtPrice.Name = "txtPrice"
        Me.txtPrice.Size = New System.Drawing.Size(66, 20)
        Me.txtPrice.TabIndex = 19
        Me.txtPrice.Visible = False
        '
        'lblSenderID
        '
        Me.lblSenderID.AutoSize = True
        Me.lblSenderID.Location = New System.Drawing.Point(9, 17)
        Me.lblSenderID.Name = "lblSenderID"
        Me.lblSenderID.Size = New System.Drawing.Size(55, 13)
        Me.lblSenderID.TabIndex = 18
        Me.lblSenderID.Text = "Sender ID"
        '
        'cmbSenderID
        '
        Me.cmbSenderID.FormattingEnabled = True
        Me.cmbSenderID.Location = New System.Drawing.Point(70, 12)
        Me.cmbSenderID.Name = "cmbSenderID"
        Me.cmbSenderID.Size = New System.Drawing.Size(121, 21)
        Me.cmbSenderID.TabIndex = 17
        '
        'lblTradeType
        '
        Me.lblTradeType.AutoSize = True
        Me.lblTradeType.Location = New System.Drawing.Point(9, 176)
        Me.lblTradeType.Name = "lblTradeType"
        Me.lblTradeType.Size = New System.Drawing.Size(58, 13)
        Me.lblTradeType.TabIndex = 16
        Me.lblTradeType.Text = "Trade type"
        '
        'cmbTradeType
        '
        Me.cmbTradeType.BackColor = System.Drawing.SystemColors.Window
        Me.cmbTradeType.FormattingEnabled = True
        Me.cmbTradeType.Location = New System.Drawing.Point(70, 173)
        Me.cmbTradeType.Name = "cmbTradeType"
        Me.cmbTradeType.Size = New System.Drawing.Size(121, 21)
        Me.cmbTradeType.TabIndex = 15
        '
        'TxtCurrency
        '
        Me.TxtCurrency.BackColor = System.Drawing.SystemColors.Window
        Me.TxtCurrency.Enabled = False
        Me.TxtCurrency.Location = New System.Drawing.Point(70, 77)
        Me.TxtCurrency.Name = "TxtCurrency"
        Me.TxtCurrency.Size = New System.Drawing.Size(121, 20)
        Me.TxtCurrency.TabIndex = 13
        '
        'Quantity
        '
        Me.Quantity.AutoSize = True
        Me.Quantity.Location = New System.Drawing.Point(9, 145)
        Me.Quantity.Name = "Quantity"
        Me.Quantity.Size = New System.Drawing.Size(46, 13)
        Me.Quantity.TabIndex = 12
        Me.Quantity.Text = "Quantity"
        '
        'Action
        '
        Me.Action.AutoSize = True
        Me.Action.Location = New System.Drawing.Point(9, 111)
        Me.Action.Name = "Action"
        Me.Action.Size = New System.Drawing.Size(37, 13)
        Me.Action.TabIndex = 11
        Me.Action.Text = "Action"
        '
        'CurencyLbl
        '
        Me.CurencyLbl.AutoSize = True
        Me.CurencyLbl.Location = New System.Drawing.Point(9, 80)
        Me.CurencyLbl.Name = "CurencyLbl"
        Me.CurencyLbl.Size = New System.Drawing.Size(49, 13)
        Me.CurencyLbl.TabIndex = 10
        Me.CurencyLbl.Text = "Currency"
        '
        'Symbol
        '
        Me.Symbol.AutoSize = True
        Me.Symbol.Location = New System.Drawing.Point(9, 49)
        Me.Symbol.Name = "Symbol"
        Me.Symbol.Size = New System.Drawing.Size(41, 13)
        Me.Symbol.TabIndex = 9
        Me.Symbol.Text = "Symbol"
        '
        'cmbSymbol
        '
        Me.cmbSymbol.Location = New System.Drawing.Point(70, 46)
        Me.cmbSymbol.Name = "cmbSymbol"
        Me.cmbSymbol.Size = New System.Drawing.Size(121, 21)
        Me.cmbSymbol.TabIndex = 7
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.LightSteelBlue
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Location = New System.Drawing.Point(212, 44)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(66, 23)
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnAccept
        '
        Me.btnAccept.BackColor = System.Drawing.Color.LightSteelBlue
        Me.btnAccept.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.btnAccept.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.btnAccept.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.btnAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAccept.Location = New System.Drawing.Point(212, 12)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(66, 23)
        Me.btnAccept.TabIndex = 4
        Me.btnAccept.Text = "Send"
        Me.btnAccept.UseVisualStyleBackColor = False
        '
        'ManualAlert
        '
        Me.AcceptButton = Me.btnAccept
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FloralWhite
        Me.ClientSize = New System.Drawing.Size(286, 205)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.Name = "ManualAlert"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Manual Order"
        Me.TopMost = True
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents cmbSymbol As System.Windows.Forms.ComboBox
    Friend WithEvents CurencyLbl As System.Windows.Forms.Label
    Friend WithEvents Symbol As System.Windows.Forms.Label
    Friend WithEvents Quantity As System.Windows.Forms.Label
    Friend WithEvents Action As System.Windows.Forms.Label
    Friend WithEvents TxtCurrency As System.Windows.Forms.TextBox 'Janus.Windows.GridEX.EditControls.EditBox
    Friend WithEvents lblTradeType As System.Windows.Forms.Label
    Friend WithEvents cmbTradeType As System.Windows.Forms.ComboBox
    Friend WithEvents lblSenderID As System.Windows.Forms.Label
    Friend WithEvents cmbSenderID As System.Windows.Forms.ComboBox
    Friend WithEvents txtPrice As System.Windows.Forms.TextBox
    Friend WithEvents rbBuy As System.Windows.Forms.RadioButton
    Friend WithEvents rbSell As System.Windows.Forms.RadioButton
    Friend WithEvents lblPrice As System.Windows.Forms.Label
    Friend WithEvents cmbQuantity As System.Windows.Forms.ComboBox
    'Friend WithEvents SchedulePrintDocument1 As Janus.Windows.Schedule.SchedulePrintDocument
End Class
