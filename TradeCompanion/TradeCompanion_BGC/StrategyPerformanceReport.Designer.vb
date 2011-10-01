<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class StrategyPerformanceReport
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(StrategyPerformanceReport))
        Me.TabPLWindow = New Crownwood.Magic.Controls.TabControl
        Me.TabPagePeriodicalReturns = New Crownwood.Magic.Controls.TabPage
        Me.cmbReportType = New System.Windows.Forms.ComboBox
        Me.grdPeriodicalReturns = New Janus.Windows.GridEX.GridEX
        Me.TabPageSpotPosition = New Crownwood.Magic.Controls.TabPage
        Me.grdSpotPosition = New Janus.Windows.GridEX.GridEX
        Me.TabPagePerformaceSummary = New Crownwood.Magic.Controls.TabPage
        Me.PictureBox6 = New System.Windows.Forms.PictureBox
        Me.PictureBox3 = New System.Windows.Forms.PictureBox
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.lblShowTotalTrades = New System.Windows.Forms.Label
        Me.lblShowAvgTradeNetProfit = New System.Windows.Forms.Label
        Me.lblTotalTrades = New System.Windows.Forms.Label
        Me.lblShowNetProfit = New System.Windows.Forms.Label
        Me.lblNetProfit = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.TabPageTradeList = New Crownwood.Magic.Controls.TabPage
        Me.grdPLWindow = New Janus.Windows.GridEX.GridEX
        Me.cmbSymbol = New System.Windows.Forms.ComboBox
        Me.btnGetPL = New System.Windows.Forms.Button
        Me.lblDate = New System.Windows.Forms.Label
        Me.lblShowDate = New System.Windows.Forms.Label
        Me.cmbID = New System.Windows.Forms.ComboBox
        Me.lblAccount = New System.Windows.Forms.Label
        Me.lblSymbol = New System.Windows.Forms.Label
        Me.clearSpotposBtn = New System.Windows.Forms.Button
        Me.TabPagePeriodicalReturns.SuspendLayout()
        CType(Me.grdPeriodicalReturns, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageSpotPosition.SuspendLayout()
        CType(Me.grdSpotPosition, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPagePerformaceSummary.SuspendLayout()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageTradeList.SuspendLayout()
        CType(Me.grdPLWindow, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TabPLWindow
        '
        Me.TabPLWindow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TabPLWindow.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TabPLWindow.HideTabsMode = Crownwood.Magic.Controls.TabControl.HideTabsModes.ShowAlways
        Me.TabPLWindow.Location = New System.Drawing.Point(0, 43)
        Me.TabPLWindow.Name = "TabPLWindow"
        Me.TabPLWindow.SelectedIndex = 0
        Me.TabPLWindow.SelectedTab = Me.TabPageSpotPosition
        Me.TabPLWindow.Size = New System.Drawing.Size(1014, 555)
        Me.TabPLWindow.TabIndex = 0
        Me.TabPLWindow.TabPages.AddRange(New Crownwood.Magic.Controls.TabPage() {Me.TabPageSpotPosition, Me.TabPagePerformaceSummary, Me.TabPageTradeList, Me.TabPagePeriodicalReturns})
        '
        'TabPagePeriodicalReturns
        '
        Me.TabPagePeriodicalReturns.BackColor = System.Drawing.Color.White
        Me.TabPagePeriodicalReturns.Controls.Add(Me.cmbReportType)
        Me.TabPagePeriodicalReturns.Controls.Add(Me.grdPeriodicalReturns)
        Me.TabPagePeriodicalReturns.Location = New System.Drawing.Point(0, 0)
        Me.TabPagePeriodicalReturns.Name = "TabPagePeriodicalReturns"
        Me.TabPagePeriodicalReturns.Selected = False
        Me.TabPagePeriodicalReturns.Size = New System.Drawing.Size(1014, 530)
        Me.TabPagePeriodicalReturns.TabIndex = 2
        Me.TabPagePeriodicalReturns.Title = "Periodical Returns"
        '
        'cmbReportType
        '
        Me.cmbReportType.FormattingEnabled = True
        Me.cmbReportType.Items.AddRange(New Object() {"Anually", "Daily", "Monthly", "Weekly"})
        Me.cmbReportType.Location = New System.Drawing.Point(853, 3)
        Me.cmbReportType.Name = "cmbReportType"
        Me.cmbReportType.Size = New System.Drawing.Size(164, 23)
        Me.cmbReportType.TabIndex = 2
        '
        'grdPeriodicalReturns
        '
        Me.grdPeriodicalReturns.ColumnAutoResize = True
        Me.grdPeriodicalReturns.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grdPeriodicalReturns.Location = New System.Drawing.Point(0, 0)
        Me.grdPeriodicalReturns.Name = "grdPeriodicalReturns"
        Me.grdPeriodicalReturns.Size = New System.Drawing.Size(1014, 530)
        Me.grdPeriodicalReturns.TabIndex = 1
        '
        'TabPageSpotPosition
        '
        Me.TabPageSpotPosition.Controls.Add(Me.grdSpotPosition)
        Me.TabPageSpotPosition.Location = New System.Drawing.Point(0, 0)
        Me.TabPageSpotPosition.Name = "TabPageSpotPosition"
        Me.TabPageSpotPosition.Size = New System.Drawing.Size(1014, 530)
        Me.TabPageSpotPosition.TabIndex = 3
        Me.TabPageSpotPosition.Title = "Spot Position"
        '
        'grdSpotPosition
        '
        Me.grdSpotPosition.ColumnAutoResize = True
        Me.grdSpotPosition.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grdSpotPosition.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World)
        Me.grdSpotPosition.Location = New System.Drawing.Point(0, 0)
        Me.grdSpotPosition.Name = "grdSpotPosition"
        Me.grdSpotPosition.Size = New System.Drawing.Size(1014, 530)
        Me.grdSpotPosition.TabIndex = 0
        '
        'TabPagePerformaceSummary
        '
        Me.TabPagePerformaceSummary.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.TabPagePerformaceSummary.Controls.Add(Me.PictureBox6)
        Me.TabPagePerformaceSummary.Controls.Add(Me.PictureBox3)
        Me.TabPagePerformaceSummary.Controls.Add(Me.PictureBox1)
        Me.TabPagePerformaceSummary.Controls.Add(Me.lblShowTotalTrades)
        Me.TabPagePerformaceSummary.Controls.Add(Me.lblShowAvgTradeNetProfit)
        Me.TabPagePerformaceSummary.Controls.Add(Me.lblTotalTrades)
        Me.TabPagePerformaceSummary.Controls.Add(Me.lblShowNetProfit)
        Me.TabPagePerformaceSummary.Controls.Add(Me.lblNetProfit)
        Me.TabPagePerformaceSummary.Controls.Add(Me.Label2)
        Me.TabPagePerformaceSummary.Location = New System.Drawing.Point(0, 0)
        Me.TabPagePerformaceSummary.Name = "TabPagePerformaceSummary"
        Me.TabPagePerformaceSummary.Selected = False
        Me.TabPagePerformaceSummary.Size = New System.Drawing.Size(1014, 530)
        Me.TabPagePerformaceSummary.TabIndex = 0
        Me.TabPagePerformaceSummary.Title = "Performace Summary"
        '
        'PictureBox6
        '
        Me.PictureBox6.Image = CType(resources.GetObject("PictureBox6.Image"), System.Drawing.Image)
        Me.PictureBox6.Location = New System.Drawing.Point(35, 72)
        Me.PictureBox6.Name = "PictureBox6"
        Me.PictureBox6.Size = New System.Drawing.Size(53, 51)
        Me.PictureBox6.TabIndex = 17
        Me.PictureBox6.TabStop = False
        '
        'PictureBox3
        '
        Me.PictureBox3.Image = CType(resources.GetObject("PictureBox3.Image"), System.Drawing.Image)
        Me.PictureBox3.Location = New System.Drawing.Point(35, 263)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(53, 51)
        Me.PictureBox3.TabIndex = 14
        Me.PictureBox3.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(35, 170)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(53, 47)
        Me.PictureBox1.TabIndex = 12
        Me.PictureBox1.TabStop = False
        '
        'lblShowTotalTrades
        '
        Me.lblShowTotalTrades.AutoSize = True
        Me.lblShowTotalTrades.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowTotalTrades.Location = New System.Drawing.Point(325, 183)
        Me.lblShowTotalTrades.Name = "lblShowTotalTrades"
        Me.lblShowTotalTrades.Size = New System.Drawing.Size(16, 16)
        Me.lblShowTotalTrades.TabIndex = 11
        Me.lblShowTotalTrades.Text = "0"
        '
        'lblShowAvgTradeNetProfit
        '
        Me.lblShowAvgTradeNetProfit.AutoSize = True
        Me.lblShowAvgTradeNetProfit.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowAvgTradeNetProfit.Location = New System.Drawing.Point(325, 280)
        Me.lblShowAvgTradeNetProfit.Name = "lblShowAvgTradeNetProfit"
        Me.lblShowAvgTradeNetProfit.Size = New System.Drawing.Size(16, 16)
        Me.lblShowAvgTradeNetProfit.TabIndex = 10
        Me.lblShowAvgTradeNetProfit.Text = "0"
        '
        'lblTotalTrades
        '
        Me.lblTotalTrades.AutoSize = True
        Me.lblTotalTrades.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalTrades.ForeColor = System.Drawing.Color.CornflowerBlue
        Me.lblTotalTrades.Location = New System.Drawing.Point(96, 183)
        Me.lblTotalTrades.Name = "lblTotalTrades"
        Me.lblTotalTrades.Size = New System.Drawing.Size(124, 16)
        Me.lblTotalTrades.TabIndex = 6
        Me.lblTotalTrades.Text = "Total no of trades"
        '
        'lblShowNetProfit
        '
        Me.lblShowNetProfit.AutoSize = True
        Me.lblShowNetProfit.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowNetProfit.Location = New System.Drawing.Point(328, 89)
        Me.lblShowNetProfit.Name = "lblShowNetProfit"
        Me.lblShowNetProfit.Size = New System.Drawing.Size(16, 16)
        Me.lblShowNetProfit.TabIndex = 1
        Me.lblShowNetProfit.Text = "0"
        '
        'lblNetProfit
        '
        Me.lblNetProfit.AutoSize = True
        Me.lblNetProfit.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNetProfit.ForeColor = System.Drawing.Color.CornflowerBlue
        Me.lblNetProfit.Location = New System.Drawing.Point(96, 89)
        Me.lblNetProfit.Name = "lblNetProfit"
        Me.lblNetProfit.Size = New System.Drawing.Size(106, 16)
        Me.lblNetProfit.TabIndex = 0
        Me.lblNetProfit.Text = "Total Net Profit"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.CornflowerBlue
        Me.Label2.Location = New System.Drawing.Point(96, 280)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(171, 16)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Average Trade Net Profit"
        '
        'TabPageTradeList
        '
        Me.TabPageTradeList.Controls.Add(Me.grdPLWindow)
        Me.TabPageTradeList.Location = New System.Drawing.Point(0, 0)
        Me.TabPageTradeList.Name = "TabPageTradeList"
        Me.TabPageTradeList.Selected = False
        Me.TabPageTradeList.Size = New System.Drawing.Size(1014, 530)
        Me.TabPageTradeList.TabIndex = 1
        Me.TabPageTradeList.Title = "Trade List"
        '
        'grdPLWindow
        '
        Me.grdPLWindow.AlternatingColors = True
        Me.grdPLWindow.AlternatingRowFormatStyle.Appearance = Janus.Windows.GridEX.Appearance.Flat
        Me.grdPLWindow.AlternatingRowFormatStyle.BackColor = System.Drawing.Color.White
        Me.grdPLWindow.AlternatingRowFormatStyle.FontUnderline = Janus.Windows.GridEX.TriState.[False]
        Me.grdPLWindow.AlternatingRowFormatStyle.ForeColor = System.Drawing.Color.Black
        Me.grdPLWindow.ColumnAutoResize = True
        Me.grdPLWindow.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grdPLWindow.Location = New System.Drawing.Point(0, 0)
        Me.grdPLWindow.Name = "grdPLWindow"
        Me.grdPLWindow.Size = New System.Drawing.Size(1014, 530)
        Me.grdPLWindow.TabIndex = 0
        '
        'cmbSymbol
        '
        Me.cmbSymbol.FormattingEnabled = True
        Me.cmbSymbol.Location = New System.Drawing.Point(50, 7)
        Me.cmbSymbol.Name = "cmbSymbol"
        Me.cmbSymbol.Size = New System.Drawing.Size(140, 21)
        Me.cmbSymbol.TabIndex = 1
        '
        'btnGetPL
        '
        Me.btnGetPL.BackColor = System.Drawing.Color.LightSteelBlue
        Me.btnGetPL.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.btnGetPL.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.btnGetPL.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.btnGetPL.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGetPL.Location = New System.Drawing.Point(451, 6)
        Me.btnGetPL.Name = "btnGetPL"
        Me.btnGetPL.Size = New System.Drawing.Size(78, 21)
        Me.btnGetPL.TabIndex = 2
        Me.btnGetPL.Text = "Refresh"
        Me.btnGetPL.UseVisualStyleBackColor = False
        '
        'lblDate
        '
        Me.lblDate.AutoSize = True
        Me.lblDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDate.Location = New System.Drawing.Point(0, 11)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.Size = New System.Drawing.Size(33, 13)
        Me.lblDate.TabIndex = 4
        Me.lblDate.Text = "Date:"
        '
        'lblShowDate
        '
        Me.lblShowDate.AutoSize = True
        Me.lblShowDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowDate.Location = New System.Drawing.Point(37, 12)
        Me.lblShowDate.Name = "lblShowDate"
        Me.lblShowDate.Size = New System.Drawing.Size(0, 13)
        Me.lblShowDate.TabIndex = 5
        '
        'cmbID
        '
        Me.cmbID.FormattingEnabled = True
        Me.cmbID.Location = New System.Drawing.Point(268, 7)
        Me.cmbID.Name = "cmbID"
        Me.cmbID.Size = New System.Drawing.Size(140, 21)
        Me.cmbID.TabIndex = 6
        '
        'lblAccount
        '
        Me.lblAccount.AutoSize = True
        Me.lblAccount.Location = New System.Drawing.Point(215, 10)
        Me.lblAccount.Name = "lblAccount"
        Me.lblAccount.Size = New System.Drawing.Size(47, 13)
        Me.lblAccount.TabIndex = 7
        Me.lblAccount.Text = "Account"
        '
        'lblSymbol
        '
        Me.lblSymbol.AutoSize = True
        Me.lblSymbol.Location = New System.Drawing.Point(3, 10)
        Me.lblSymbol.Name = "lblSymbol"
        Me.lblSymbol.Size = New System.Drawing.Size(41, 13)
        Me.lblSymbol.TabIndex = 8
        Me.lblSymbol.Text = "Symbol"
        '
        'clearSpotposBtn
        '
        Me.clearSpotposBtn.BackColor = System.Drawing.Color.LightSteelBlue
        Me.clearSpotposBtn.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.clearSpotposBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.clearSpotposBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.clearSpotposBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.clearSpotposBtn.Location = New System.Drawing.Point(893, 12)
        Me.clearSpotposBtn.Name = "clearSpotposBtn"
        Me.clearSpotposBtn.Size = New System.Drawing.Size(109, 21)
        Me.clearSpotposBtn.TabIndex = 9
        Me.clearSpotposBtn.Text = "Clear Spot Position"
        Me.clearSpotposBtn.UseVisualStyleBackColor = False
        '
        'StrategyPerformanceReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1014, 598)
        Me.Controls.Add(Me.clearSpotposBtn)
        Me.Controls.Add(Me.lblSymbol)
        Me.Controls.Add(Me.lblAccount)
        Me.Controls.Add(Me.cmbID)
        Me.Controls.Add(Me.lblShowDate)
        Me.Controls.Add(Me.lblDate)
        Me.Controls.Add(Me.btnGetPL)
        Me.Controls.Add(Me.cmbSymbol)
        Me.Controls.Add(Me.TabPLWindow)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(100, 200)
        Me.MaximizeBox = False
        Me.Name = "StrategyPerformanceReport"
        Me.Text = "Strategy Performance Report"
        Me.TabPagePeriodicalReturns.ResumeLayout(False)
        CType(Me.grdPeriodicalReturns, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageSpotPosition.ResumeLayout(False)
        CType(Me.grdSpotPosition, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPagePerformaceSummary.ResumeLayout(False)
        Me.TabPagePerformaceSummary.PerformLayout()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageTradeList.ResumeLayout(False)
        CType(Me.grdPLWindow, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TabPLWindow As Crownwood.Magic.Controls.TabControl
    Friend WithEvents TabPageTradeList As Crownwood.Magic.Controls.TabPage
    Friend WithEvents TabPagePerformaceSummary As Crownwood.Magic.Controls.TabPage
    Friend WithEvents cmbSymbol As System.Windows.Forms.ComboBox
    Friend WithEvents btnGetPL As System.Windows.Forms.Button
    Friend WithEvents grdPLWindow As Janus.Windows.GridEX.GridEX
    Friend WithEvents lblShowNetProfit As System.Windows.Forms.Label
    Friend WithEvents lblNetProfit As System.Windows.Forms.Label
    Friend WithEvents lblShowTotalTrades As System.Windows.Forms.Label
    Friend WithEvents lblTotalTrades As System.Windows.Forms.Label
    Friend WithEvents TabPagePeriodicalReturns As Crownwood.Magic.Controls.TabPage
    Friend WithEvents grdPeriodicalReturns As Janus.Windows.GridEX.GridEX
    Friend WithEvents cmbReportType As System.Windows.Forms.ComboBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents lblShowAvgTradeNetProfit As System.Windows.Forms.Label
    Friend WithEvents TabPageSpotPosition As Crownwood.Magic.Controls.TabPage
    Friend WithEvents grdSpotPosition As Janus.Windows.GridEX.GridEX
    Friend WithEvents lblDate As System.Windows.Forms.Label
    Friend WithEvents lblShowDate As System.Windows.Forms.Label
    Friend WithEvents cmbID As System.Windows.Forms.ComboBox
    Friend WithEvents lblAccount As System.Windows.Forms.Label
    Friend WithEvents lblSymbol As System.Windows.Forms.Label
    Friend WithEvents PictureBox6 As System.Windows.Forms.PictureBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents clearSpotposBtn As System.Windows.Forms.Button
    '-------------------------------
    '-------------------------------
End Class
