Public Class Alert
    Inherits System.Windows.Forms.Form
    Public alertdata As AlertsManager.NewAlert
    Friend WithEvents Currency As System.Windows.Forms.TextBox
    Friend WithEvents Quantity As System.Windows.Forms.Label
    Friend WithEvents Action As System.Windows.Forms.Label
    Friend WithEvents CurrencyLbl As System.Windows.Forms.Label
    Friend WithEvents Symbol As System.Windows.Forms.Label
    Friend WithEvents lblTradeType As System.Windows.Forms.Label
    Friend WithEvents cmbTradeType As System.Windows.Forms.ComboBox
    Private Const CANCEL As Integer = 0
#Region " Windows Form Designer generated code "

    Public Sub New(ByVal a As AlertsManager.NewAlert)
        MyBase.New()
        
        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        alertdata = a
        txtSymbol.Text = a.symbol
        'txtExch.Text = a.exch
        txtContracts.Text = a.contracts
        Currency.Text = a.currency
        'txtMonthYear.Text = a.month_year
        Select Case a.actiontype
            Case AlertsManager.ACTION_BUY
                txtAction.Text = "BUY"
            Case AlertsManager.ACTION_SELL
                txtAction.Text = "SELL"
        End Select

        Dim sTradeType As New SettingsTrade
        sTradeType.getSettings()
        Select Case a.tradeType
            Case 1 'GTC
                cmbTradeType.SelectedIndex = 1
            Case 3 'IOC
                cmbTradeType.SelectedIndex = 0
        End Select

        Beep()
        'Me.TopMost = True
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents txtSymbol As System.Windows.Forms.TextBox
    Friend WithEvents txtAction As System.Windows.Forms.TextBox
    Friend WithEvents txtContracts As System.Windows.Forms.TextBox
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lblTradeType = New System.Windows.Forms.Label
        Me.Currency = New System.Windows.Forms.TextBox
        Me.cmbTradeType = New System.Windows.Forms.ComboBox
        Me.Quantity = New System.Windows.Forms.Label
        Me.Action = New System.Windows.Forms.Label
        Me.CurrencyLbl = New System.Windows.Forms.Label
        Me.Symbol = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnAccept = New System.Windows.Forms.Button
        Me.txtContracts = New System.Windows.Forms.TextBox
        Me.txtAction = New System.Windows.Forms.TextBox
        Me.txtSymbol = New System.Windows.Forms.TextBox
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.Panel1.Controls.Add(Me.lblTradeType)
        Me.Panel1.Controls.Add(Me.Currency)
        Me.Panel1.Controls.Add(Me.cmbTradeType)
        Me.Panel1.Controls.Add(Me.Quantity)
        Me.Panel1.Controls.Add(Me.Action)
        Me.Panel1.Controls.Add(Me.CurrencyLbl)
        Me.Panel1.Controls.Add(Me.Symbol)
        Me.Panel1.Controls.Add(Me.btnCancel)
        Me.Panel1.Controls.Add(Me.btnAccept)
        Me.Panel1.Controls.Add(Me.txtContracts)
        Me.Panel1.Controls.Add(Me.txtAction)
        Me.Panel1.Controls.Add(Me.txtSymbol)
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(278, 170)
        Me.Panel1.TabIndex = 3
        '
        'lblTradeType
        '
        Me.lblTradeType.AutoSize = True
        Me.lblTradeType.Location = New System.Drawing.Point(12, 142)
        Me.lblTradeType.Name = "lblTradeType"
        Me.lblTradeType.Size = New System.Drawing.Size(58, 13)
        Me.lblTradeType.TabIndex = 18
        Me.lblTradeType.Text = "Trade type"
        '
        'Currency
        '
        Me.Currency.BackColor = System.Drawing.SystemColors.Window
        Me.Currency.Enabled = False
        Me.Currency.Location = New System.Drawing.Point(79, 43)
        Me.Currency.Name = "Currency"
        Me.Currency.Size = New System.Drawing.Size(90, 20)
        Me.Currency.TabIndex = 17
        '
        'cmbTradeType
        '
        Me.cmbTradeType.BackColor = System.Drawing.SystemColors.Window
        Me.cmbTradeType.FormattingEnabled = True
        Me.cmbTradeType.Items.AddRange(New Object() {"IOC", "GTC"})
        Me.cmbTradeType.Location = New System.Drawing.Point(79, 134)
        Me.cmbTradeType.Name = "cmbTradeType"
        Me.cmbTradeType.Size = New System.Drawing.Size(90, 21)
        Me.cmbTradeType.TabIndex = 17
        '
        'Quantity
        '
        Me.Quantity.AutoSize = True
        Me.Quantity.Location = New System.Drawing.Point(12, 106)
        Me.Quantity.Name = "Quantity"
        Me.Quantity.Size = New System.Drawing.Size(46, 13)
        Me.Quantity.TabIndex = 16
        Me.Quantity.Text = "Quantity"
        '
        'Action
        '
        Me.Action.AutoSize = True
        Me.Action.Location = New System.Drawing.Point(12, 76)
        Me.Action.Name = "Action"
        Me.Action.Size = New System.Drawing.Size(37, 13)
        Me.Action.TabIndex = 15
        Me.Action.Text = "Action"
        '
        'CurrencyLbl
        '
        Me.CurrencyLbl.AutoSize = True
        Me.CurrencyLbl.Location = New System.Drawing.Point(12, 46)
        Me.CurrencyLbl.Name = "CurrencyLbl"
        Me.CurrencyLbl.Size = New System.Drawing.Size(49, 13)
        Me.CurrencyLbl.TabIndex = 14
        Me.CurrencyLbl.Text = "Currency"
        '
        'Symbol
        '
        Me.Symbol.AutoSize = True
        Me.Symbol.Location = New System.Drawing.Point(12, 16)
        Me.Symbol.Name = "Symbol"
        Me.Symbol.Size = New System.Drawing.Size(41, 13)
        Me.Symbol.TabIndex = 13
        Me.Symbol.Text = "Symbol"
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.LightSteelBlue
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Location = New System.Drawing.Point(190, 131)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnAccept
        '
        Me.btnAccept.BackColor = System.Drawing.Color.LightSteelBlue
        Me.btnAccept.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnAccept.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.btnAccept.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.btnAccept.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.btnAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAccept.Location = New System.Drawing.Point(190, 102)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.TabIndex = 4
        Me.btnAccept.Text = "Send"
        Me.btnAccept.UseVisualStyleBackColor = False
        '
        'txtContracts
        '
        Me.txtContracts.BackColor = System.Drawing.SystemColors.Window
        Me.txtContracts.Enabled = False
        Me.txtContracts.Location = New System.Drawing.Point(79, 105)
        Me.txtContracts.Name = "txtContracts"
        Me.txtContracts.Size = New System.Drawing.Size(90, 20)
        Me.txtContracts.TabIndex = 3
        '
        'txtAction
        '
        Me.txtAction.BackColor = System.Drawing.SystemColors.Window
        Me.txtAction.Enabled = False
        Me.txtAction.Location = New System.Drawing.Point(79, 73)
        Me.txtAction.Name = "txtAction"
        Me.txtAction.Size = New System.Drawing.Size(90, 20)
        Me.txtAction.TabIndex = 2
        '
        'txtSymbol
        '
        Me.txtSymbol.BackColor = System.Drawing.SystemColors.Window
        Me.txtSymbol.Enabled = False
        Me.txtSymbol.Location = New System.Drawing.Point(79, 13)
        Me.txtSymbol.Name = "txtSymbol"
        Me.txtSymbol.Size = New System.Drawing.Size(90, 20)
        Me.txtSymbol.TabIndex = 0
        '
        'Alert
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(280, 172)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Alert"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Alert_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = Now
    End Sub

    Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        alertdata.symbol = txtSymbol.Text.Trim() '.ToUpper
        'alertdata.exch = txtExch.Text.Trim '.ToUpper
        alertdata.contracts = txtContracts.Text.Trim()
        Select Case txtAction.Text.ToUpper
            Case "BUY"
                alertdata.actiontype = AlertsManager.ACTION_BUY
            Case "SELL"
                alertdata.actiontype = AlertsManager.ACTION_SELL
        End Select
        alertdata.currency = Currency.Text.Trim()

        If (cmbTradeType.SelectedIndex = 0) Then 'IOC
            alertdata.tradeType = 3
        ElseIf (cmbTradeType.SelectedIndex = 1) Then 'GTC
            alertdata.tradeType = 1
        End If

        'Not reqd for currenex.alertdata.month_year = txtMonthYear.Text.Trim
        'If alertdata.month_year = "" Then
        'alertdata.month_year = " "
        'End If
        'Not required for currenEx.If alertdata.month_year.Trim = "" Then alertdata.status = CANCEL

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

    End Sub

    Private Sub Alert_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        Me.TopMost = True
    End Sub
End Class
