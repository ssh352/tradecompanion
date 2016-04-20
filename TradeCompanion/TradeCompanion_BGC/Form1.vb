Imports System.IO
Imports System.Configuration
Imports System.Net.Mail
Imports System.Threading
Imports Janus.Windows.GridEX
Imports Janus.Windows.GridEX.Export
Imports System.Security.Permissions
Imports System.Globalization
Imports System.Data.OleDb
Imports System.Net.NetworkInformation
<Assembly: SecurityPermission(SecurityAction.RequestMinimum, ControlThread:=True)> 
Public Delegate Sub SendEmail_Delegate()

Public Class Form1
    Inherits System.Windows.Forms.Form
    Private WithEvents watcher As AlertWatcher
    Friend WithEvents btnConnect As System.Windows.Forms.Button
    Friend WithEvents btnDisconnect As System.Windows.Forms.Button
    Friend WithEvents mnuMain As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuConnect As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents btnPlaceOrder As System.Windows.Forms.Button
    Friend WithEvents CancelOrders As System.Windows.Forms.Button
    Friend WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuUserManual As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutTradeCompanionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TabControl1 As Crownwood.Magic.Controls.TabControl
    Friend WithEvents TabOrders As Crownwood.Magic.Controls.TabPage
    'Private WithEvents trader As New AlertExecution
    Friend WithEvents lblFromDate As System.Windows.Forms.Label
    Friend WithEvents btnGetOrders As System.Windows.Forms.Button
    Friend WithEvents lblToDate As System.Windows.Forms.Label
    Friend WithEvents dtpStartDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents grpFilter As System.Windows.Forms.GroupBox
    Friend WithEvents grdIndSys As Janus.Windows.GridEX.GridEX
    Friend WithEvents grdMarketData As Janus.Windows.GridEX.GridEX
    Friend WithEvents grdAlerts As Janus.Windows.GridEX.GridEX
    'Private WithEvents marketdata As MarketDataExecution = Nothing
    Friend WithEvents TabMDHistory As Crownwood.Magic.Controls.TabPage
    Friend WithEvents grdMDHistory As Janus.Windows.GridEX.GridEX
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StrategyPerformanceReportToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MarketDataConnToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TradeConnToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Public Event NewAlert(ByVal execute As AlertsManager.NewAlert)
    Dim Mapping As MappingTable
    Dim IdMapping As New IDMappingTable()
    Dim spr As StrategyPerformanceReport
    Dim d As New SendEmail_Delegate(AddressOf SendEmail)
    Private Shared singleton_Form1 As Form1 = Nothing
    Friend WithEvents StatusPanel As System.Windows.Forms.Panel
    Friend WithEvents MappingTableToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents IDMappingTableToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents btnSubscribeMarketData As System.Windows.Forms.Button
    Friend WithEvents btnDisconnectMarketData As System.Windows.Forms.Button
    Friend WithEvents dtpEndDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents SettingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UseSymbolMappingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UseDefaultTradesizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CToolStripMenuItem As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ConfigurationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CurrenExTradeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CurrenExMarketDataToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LoginTCToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TradeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SoundsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FilterAlertsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TabLog As Crownwood.Magic.Controls.TabPage
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents btnClearLog As System.Windows.Forms.Button
    Friend WithEvents btnExportLog As System.Windows.Forms.Button
    Friend WithEvents RichTextBoxLog As System.Windows.Forms.RichTextBox
    Friend WithEvents mnuChangePassword As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pbxMarketDataStatus As System.Windows.Forms.PictureBox
    Friend WithEvents OpenPositionGridEX As Janus.Windows.GridEX.GridEX
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox

    Private WithEvents plcal As New PLCal
    Dim dsAllSys As DataSet
    Dim dsIndSys As DataSet
    Dim upda As New UpdateUI
    Private WithEvents indSys As New IndSysPL
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents RestoreToolStrip As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MinimizeToolStrip As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStrip As System.Windows.Forms.ToolStripMenuItem
    Dim ah As New AlertsHome



#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        AddHandler NetworkChange.NetworkAvailabilityChanged, AddressOf netStatus
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        If (singleton_Form1 Is Nothing) Then singleton_Form1 = Me
        'Add any initialization after the InitializeComponent() call
       'crashtest = False 'automate.CrashTest()        'delete all alerts when TC Starts
        RemoveOldAlert()
    End Sub

    Public Shared Function GetSingletonOrderform() As Form1
        Return singleton_Form1
    End Function

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            Me.NotifyIcon1.Dispose()
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
    Friend WithEvents grpExecutionMode As System.Windows.Forms.GroupBox
    Friend WithEvents rbNoExecution As System.Windows.Forms.RadioButton
    Friend WithEvents rbManual As System.Windows.Forms.RadioButton
    Friend WithEvents rbAuto As System.Windows.Forms.RadioButton
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim TabAlerts As Crownwood.Magic.Controls.TabPage
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.grdAlerts = New Janus.Windows.GridEX.GridEX()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnConnect = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.StatusPanel = New System.Windows.Forms.Panel()
        Me.grpFilter = New System.Windows.Forms.GroupBox()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.dtpEndDate = New System.Windows.Forms.DateTimePicker()
        Me.lblFromDate = New System.Windows.Forms.Label()
        Me.btnGetOrders = New System.Windows.Forms.Button()
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker()
        Me.lblToDate = New System.Windows.Forms.Label()
        Me.btnPlaceOrder = New System.Windows.Forms.Button()
        Me.btnDisconnect = New System.Windows.Forms.Button()
        Me.grpExecutionMode = New System.Windows.Forms.GroupBox()
        Me.rbAuto = New System.Windows.Forms.RadioButton()
        Me.rbManual = New System.Windows.Forms.RadioButton()
        Me.rbNoExecution = New System.Windows.Forms.RadioButton()
        Me.mnuMain = New System.Windows.Forms.MenuStrip()
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuConnect = New System.Windows.Forms.ToolStripMenuItem()
        Me.MarketDataConnToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TradeConnToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuChangePassword = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StrategyPerformanceReportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MappingTableToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.IDMappingTableToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SettingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UseSymbolMappingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FilterAlertsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UseDefaultTradesizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CToolStripMenuItem = New System.Windows.Forms.ToolStripSeparator()
        Me.ConfigurationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CurrenExTradeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CurrenExMarketDataToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LoginTCToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TradeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SoundsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuUserManual = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutTradeCompanionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TabControl1 = New Crownwood.Magic.Controls.TabControl()
        Me.TabOrders = New Crownwood.Magic.Controls.TabPage()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.btnSubscribeMarketData = New System.Windows.Forms.Button()
        Me.btnDisconnectMarketData = New System.Windows.Forms.Button()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.grdMarketData = New Janus.Windows.GridEX.GridEX()
        Me.OpenPositionGridEX = New Janus.Windows.GridEX.GridEX()
        Me.grdIndSys = New Janus.Windows.GridEX.GridEX()
        Me.TabMDHistory = New Crownwood.Magic.Controls.TabPage()
        Me.grdMDHistory = New Janus.Windows.GridEX.GridEX()
        Me.TabLog = New Crownwood.Magic.Controls.TabPage()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.btnClearLog = New System.Windows.Forms.Button()
        Me.btnExportLog = New System.Windows.Forms.Button()
        Me.RichTextBoxLog = New System.Windows.Forms.RichTextBox()
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RestoreToolStrip = New System.Windows.Forms.ToolStripMenuItem()
        Me.MinimizeToolStrip = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStrip = New System.Windows.Forms.ToolStripMenuItem()
        Me.pbxMarketDataStatus = New System.Windows.Forms.PictureBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        TabAlerts = New Crownwood.Magic.Controls.TabPage()
        TabAlerts.SuspendLayout()
        CType(Me.grdAlerts, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.grpFilter.SuspendLayout()
        Me.grpExecutionMode.SuspendLayout()
        Me.mnuMain.SuspendLayout()
        Me.TabOrders.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        CType(Me.grdMarketData, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OpenPositionGridEX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grdIndSys, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabMDHistory.SuspendLayout()
        CType(Me.grdMDHistory, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabLog.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        CType(Me.pbxMarketDataStatus, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TabAlerts
        '
        TabAlerts.Controls.Add(Me.grdAlerts)
        TabAlerts.Location = New System.Drawing.Point(0, 0)
        TabAlerts.Name = "TabAlerts"
        TabAlerts.Selected = False
        TabAlerts.Size = New System.Drawing.Size(1190, 534)
        TabAlerts.TabIndex = 1
        TabAlerts.Title = "Alerts"
        '
        'grdAlerts
        '
        Me.grdAlerts.BackColor = System.Drawing.Color.WhiteSmoke
        Me.grdAlerts.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grdAlerts.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center
        Me.grdAlerts.Location = New System.Drawing.Point(0, 0)
        Me.grdAlerts.Name = "grdAlerts"
        Me.grdAlerts.Size = New System.Drawing.Size(1190, 534)
        Me.grdAlerts.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Gainsboro
        Me.Panel1.Controls.Add(Me.btnConnect)
        Me.Panel1.Controls.Add(Me.GroupBox2)
        Me.Panel1.Controls.Add(Me.grpFilter)
        Me.Panel1.Controls.Add(Me.btnPlaceOrder)
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Controls.Add(Me.btnDisconnect)
        Me.Panel1.Controls.Add(Me.grpExecutionMode)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 28)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1190, 105)
        Me.Panel1.TabIndex = 2
        Me.Panel1.Tag = ""
        '
        'btnConnect
        '
        Me.btnConnect.AutoSize = True
        Me.btnConnect.BackColor = System.Drawing.Color.LightSteelBlue
        Me.btnConnect.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.btnConnect.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.btnConnect.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnConnect.Location = New System.Drawing.Point(596, 7)
        Me.btnConnect.Name = "btnConnect"
        Me.btnConnect.Size = New System.Drawing.Size(142, 33)
        Me.btnConnect.TabIndex = 8
        Me.btnConnect.Text = " Connect Trade"
        Me.btnConnect.UseVisualStyleBackColor = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.StatusPanel)
        Me.GroupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GroupBox2.Location = New System.Drawing.Point(378, 2)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(212, 101)
        Me.GroupBox2.TabIndex = 21
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Connection Status"
        '
        'StatusPanel
        '
        Me.StatusPanel.Location = New System.Drawing.Point(7, 17)
        Me.StatusPanel.Name = "StatusPanel"
        Me.StatusPanel.Size = New System.Drawing.Size(198, 79)
        Me.StatusPanel.TabIndex = 21
        '
        'grpFilter
        '
        Me.grpFilter.Controls.Add(Me.btnExport)
        Me.grpFilter.Controls.Add(Me.dtpEndDate)
        Me.grpFilter.Controls.Add(Me.lblFromDate)
        Me.grpFilter.Controls.Add(Me.btnGetOrders)
        Me.grpFilter.Controls.Add(Me.dtpStartDate)
        Me.grpFilter.Controls.Add(Me.lblToDate)
        Me.grpFilter.Location = New System.Drawing.Point(728, 7)
        Me.grpFilter.Name = "grpFilter"
        Me.grpFilter.Size = New System.Drawing.Size(282, 92)
        Me.grpFilter.TabIndex = 20
        Me.grpFilter.TabStop = False
        Me.grpFilter.Text = "Filter Orders"
        '
        'btnExport
        '
        Me.btnExport.BackColor = System.Drawing.Color.LightSteelBlue
        Me.btnExport.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.btnExport.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.btnExport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExport.Location = New System.Drawing.Point(186, 53)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(88, 25)
        Me.btnExport.TabIndex = 23
        Me.btnExport.Text = "Export"
        Me.btnExport.UseVisualStyleBackColor = False
        '
        'dtpEndDate
        '
        Me.dtpEndDate.CustomFormat = "MM/dd/yyyy"
        Me.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpEndDate.Location = New System.Drawing.Point(46, 53)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.ShowCheckBox = True
        Me.dtpEndDate.Size = New System.Drawing.Size(133, 22)
        Me.dtpEndDate.TabIndex = 22
        '
        'lblFromDate
        '
        Me.lblFromDate.AutoSize = True
        Me.lblFromDate.Location = New System.Drawing.Point(6, 22)
        Me.lblFromDate.Name = "lblFromDate"
        Me.lblFromDate.Size = New System.Drawing.Size(40, 17)
        Me.lblFromDate.TabIndex = 15
        Me.lblFromDate.Text = "From"
        '
        'btnGetOrders
        '
        Me.btnGetOrders.BackColor = System.Drawing.Color.LightSteelBlue
        Me.btnGetOrders.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.btnGetOrders.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.btnGetOrders.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.btnGetOrders.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGetOrders.Location = New System.Drawing.Point(186, 16)
        Me.btnGetOrders.Name = "btnGetOrders"
        Me.btnGetOrders.Size = New System.Drawing.Size(88, 26)
        Me.btnGetOrders.TabIndex = 19
        Me.btnGetOrders.Text = "Get Orders"
        Me.btnGetOrders.UseVisualStyleBackColor = False
        '
        'dtpStartDate
        '
        Me.dtpStartDate.CustomFormat = "MM/dd/yyyy"
        Me.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpStartDate.Location = New System.Drawing.Point(46, 17)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.ShowCheckBox = True
        Me.dtpStartDate.Size = New System.Drawing.Size(133, 22)
        Me.dtpStartDate.TabIndex = 16
        '
        'lblToDate
        '
        Me.lblToDate.AutoSize = True
        Me.lblToDate.Location = New System.Drawing.Point(7, 54)
        Me.lblToDate.Name = "lblToDate"
        Me.lblToDate.Size = New System.Drawing.Size(25, 17)
        Me.lblToDate.TabIndex = 17
        Me.lblToDate.Text = "To"
        '
        'btnPlaceOrder
        '
        Me.btnPlaceOrder.AutoSize = True
        Me.btnPlaceOrder.BackColor = System.Drawing.Color.LightSteelBlue
        Me.btnPlaceOrder.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.btnPlaceOrder.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.btnPlaceOrder.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.btnPlaceOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPlaceOrder.Location = New System.Drawing.Point(596, 72)
        Me.btnPlaceOrder.Name = "btnPlaceOrder"
        Me.btnPlaceOrder.Size = New System.Drawing.Size(129, 33)
        Me.btnPlaceOrder.TabIndex = 11
        Me.btnPlaceOrder.Text = "Manual Order"
        Me.btnPlaceOrder.UseVisualStyleBackColor = False
        Me.btnPlaceOrder.Visible = False
        '
        'btnDisconnect
        '
        Me.btnDisconnect.AutoSize = True
        Me.btnDisconnect.BackColor = System.Drawing.Color.LightSteelBlue
        Me.btnDisconnect.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.btnDisconnect.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.btnDisconnect.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.btnDisconnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDisconnect.Location = New System.Drawing.Point(596, 39)
        Me.btnDisconnect.Name = "btnDisconnect"
        Me.btnDisconnect.Size = New System.Drawing.Size(159, 34)
        Me.btnDisconnect.TabIndex = 9
        Me.btnDisconnect.Text = "Disconnect Trade"
        Me.btnDisconnect.UseVisualStyleBackColor = False
        '
        'grpExecutionMode
        '
        Me.grpExecutionMode.BackColor = System.Drawing.Color.Gainsboro
        Me.grpExecutionMode.Controls.Add(Me.rbAuto)
        Me.grpExecutionMode.Controls.Add(Me.rbManual)
        Me.grpExecutionMode.Controls.Add(Me.rbNoExecution)
        Me.grpExecutionMode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpExecutionMode.Location = New System.Drawing.Point(7, 2)
        Me.grpExecutionMode.Name = "grpExecutionMode"
        Me.grpExecutionMode.Size = New System.Drawing.Size(363, 103)
        Me.grpExecutionMode.TabIndex = 4
        Me.grpExecutionMode.TabStop = False
        Me.grpExecutionMode.Text = "Automatic Execution Mode"
        '
        'rbAuto
        '
        Me.rbAuto.AutoSize = True
        Me.rbAuto.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.rbAuto.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbAuto.ForeColor = System.Drawing.Color.Maroon
        Me.rbAuto.Location = New System.Drawing.Point(42, 69)
        Me.rbAuto.Name = "rbAuto"
        Me.rbAuto.Size = New System.Drawing.Size(134, 24)
        Me.rbAuto.TabIndex = 2
        Me.rbAuto.Text = "Automated"
        '
        'rbManual
        '
        Me.rbManual.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.rbManual.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbManual.ForeColor = System.Drawing.Color.Maroon
        Me.rbManual.Location = New System.Drawing.Point(42, 45)
        Me.rbManual.Name = "rbManual"
        Me.rbManual.Size = New System.Drawing.Size(244, 20)
        Me.rbManual.TabIndex = 1
        Me.rbManual.Text = "Manual Confirmation"
        '
        'rbNoExecution
        '
        Me.rbNoExecution.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.rbNoExecution.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbNoExecution.ForeColor = System.Drawing.Color.Maroon
        Me.rbNoExecution.Location = New System.Drawing.Point(42, 17)
        Me.rbNoExecution.Name = "rbNoExecution"
        Me.rbNoExecution.Size = New System.Drawing.Size(176, 25)
        Me.rbNoExecution.TabIndex = 0
        Me.rbNoExecution.Text = "No Execution"
        '
        'mnuMain
        '
        Me.mnuMain.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.mnuMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.ViewToolStripMenuItem, Me.SettingToolStripMenuItem, Me.mnuHelp})
        Me.mnuMain.Location = New System.Drawing.Point(0, 0)
        Me.mnuMain.Name = "mnuMain"
        Me.mnuMain.Size = New System.Drawing.Size(1190, 28)
        Me.mnuMain.TabIndex = 8
        Me.mnuMain.Text = "MenuStrip1"
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuConnect, Me.mnuChangePassword, Me.ExitToolStripMenuItem, Me.mnuExit})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(44, 24)
        Me.mnuFile.Text = "File"
        '
        'mnuConnect
        '
        Me.mnuConnect.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MarketDataConnToolStripMenuItem, Me.TradeConnToolStripMenuItem})
        Me.mnuConnect.Name = "mnuConnect"
        Me.mnuConnect.Size = New System.Drawing.Size(201, 26)
        Me.mnuConnect.Text = "Connect"
        '
        'MarketDataConnToolStripMenuItem
        '
        Me.MarketDataConnToolStripMenuItem.Name = "MarketDataConnToolStripMenuItem"
        Me.MarketDataConnToolStripMenuItem.Size = New System.Drawing.Size(166, 26)
        Me.MarketDataConnToolStripMenuItem.Text = "Market Data"
        '
        'TradeConnToolStripMenuItem
        '
        Me.TradeConnToolStripMenuItem.Name = "TradeConnToolStripMenuItem"
        Me.TradeConnToolStripMenuItem.Size = New System.Drawing.Size(166, 26)
        Me.TradeConnToolStripMenuItem.Text = "Trade"
        '
        'mnuChangePassword
        '
        Me.mnuChangePassword.Enabled = False
        Me.mnuChangePassword.Name = "mnuChangePassword"
        Me.mnuChangePassword.Size = New System.Drawing.Size(201, 26)
        Me.mnuChangePassword.Text = "Change password"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(198, 6)
        '
        'mnuExit
        '
        Me.mnuExit.Name = "mnuExit"
        Me.mnuExit.Size = New System.Drawing.Size(201, 26)
        Me.mnuExit.Text = "Exit"
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StrategyPerformanceReportToolStripMenuItem, Me.MappingTableToolStripMenuItem, Me.IDMappingTableToolStripMenuItem})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(53, 24)
        Me.ViewToolStripMenuItem.Text = "View"
        '
        'StrategyPerformanceReportToolStripMenuItem
        '
        Me.StrategyPerformanceReportToolStripMenuItem.Name = "StrategyPerformanceReportToolStripMenuItem"
        Me.StrategyPerformanceReportToolStripMenuItem.Size = New System.Drawing.Size(275, 26)
        Me.StrategyPerformanceReportToolStripMenuItem.Text = "Strategy Performance Report"
        '
        'MappingTableToolStripMenuItem
        '
        Me.MappingTableToolStripMenuItem.Name = "MappingTableToolStripMenuItem"
        Me.MappingTableToolStripMenuItem.Size = New System.Drawing.Size(275, 26)
        Me.MappingTableToolStripMenuItem.Text = "Currency Mapping"
        '
        'IDMappingTableToolStripMenuItem
        '
        Me.IDMappingTableToolStripMenuItem.Name = "IDMappingTableToolStripMenuItem"
        Me.IDMappingTableToolStripMenuItem.Size = New System.Drawing.Size(275, 26)
        Me.IDMappingTableToolStripMenuItem.Text = "Connection Mapping"
        '
        'SettingToolStripMenuItem
        '
        Me.SettingToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UseSymbolMappingToolStripMenuItem, Me.FilterAlertsToolStripMenuItem, Me.UseDefaultTradesizeToolStripMenuItem, Me.CToolStripMenuItem, Me.ConfigurationToolStripMenuItem})
        Me.SettingToolStripMenuItem.Name = "SettingToolStripMenuItem"
        Me.SettingToolStripMenuItem.Size = New System.Drawing.Size(74, 24)
        Me.SettingToolStripMenuItem.Text = "Settings"
        '
        'UseSymbolMappingToolStripMenuItem
        '
        Me.UseSymbolMappingToolStripMenuItem.Checked = True
        Me.UseSymbolMappingToolStripMenuItem.CheckOnClick = True
        Me.UseSymbolMappingToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.UseSymbolMappingToolStripMenuItem.Name = "UseSymbolMappingToolStripMenuItem"
        Me.UseSymbolMappingToolStripMenuItem.Size = New System.Drawing.Size(227, 26)
        Me.UseSymbolMappingToolStripMenuItem.Text = "Use Symbol Mapping"
        '
        'FilterAlertsToolStripMenuItem
        '
        Me.FilterAlertsToolStripMenuItem.CheckOnClick = True
        Me.FilterAlertsToolStripMenuItem.Name = "FilterAlertsToolStripMenuItem"
        Me.FilterAlertsToolStripMenuItem.Size = New System.Drawing.Size(227, 26)
        Me.FilterAlertsToolStripMenuItem.Text = "Filter Alerts"
        '
        'UseDefaultTradesizeToolStripMenuItem
        '
        Me.UseDefaultTradesizeToolStripMenuItem.CheckOnClick = True
        Me.UseDefaultTradesizeToolStripMenuItem.Name = "UseDefaultTradesizeToolStripMenuItem"
        Me.UseDefaultTradesizeToolStripMenuItem.Size = New System.Drawing.Size(227, 26)
        Me.UseDefaultTradesizeToolStripMenuItem.Text = "Use Default Tradesize"
        '
        'CToolStripMenuItem
        '
        Me.CToolStripMenuItem.Name = "CToolStripMenuItem"
        Me.CToolStripMenuItem.Size = New System.Drawing.Size(224, 6)
        '
        'ConfigurationToolStripMenuItem
        '
        Me.ConfigurationToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CurrenExTradeToolStripMenuItem, Me.CurrenExMarketDataToolStripMenuItem, Me.LoginTCToolStripMenuItem, Me.TradeToolStripMenuItem, Me.SoundsToolStripMenuItem})
        Me.ConfigurationToolStripMenuItem.Name = "ConfigurationToolStripMenuItem"
        Me.ConfigurationToolStripMenuItem.Size = New System.Drawing.Size(227, 26)
        Me.ConfigurationToolStripMenuItem.Text = "Configuration"
        '
        'CurrenExTradeToolStripMenuItem
        '
        Me.CurrenExTradeToolStripMenuItem.Name = "CurrenExTradeToolStripMenuItem"
        Me.CurrenExTradeToolStripMenuItem.Size = New System.Drawing.Size(224, 26)
        Me.CurrenExTradeToolStripMenuItem.Text = "CurrenEx Trade"
        '
        'CurrenExMarketDataToolStripMenuItem
        '
        Me.CurrenExMarketDataToolStripMenuItem.Name = "CurrenExMarketDataToolStripMenuItem"
        Me.CurrenExMarketDataToolStripMenuItem.Size = New System.Drawing.Size(224, 26)
        Me.CurrenExMarketDataToolStripMenuItem.Text = "CurrenEx MarketData"
        '
        'LoginTCToolStripMenuItem
        '
        Me.LoginTCToolStripMenuItem.Name = "LoginTCToolStripMenuItem"
        Me.LoginTCToolStripMenuItem.Size = New System.Drawing.Size(224, 26)
        Me.LoginTCToolStripMenuItem.Text = "Login TC"
        '
        'TradeToolStripMenuItem
        '
        Me.TradeToolStripMenuItem.Name = "TradeToolStripMenuItem"
        Me.TradeToolStripMenuItem.Size = New System.Drawing.Size(224, 26)
        Me.TradeToolStripMenuItem.Text = "Trade"
        '
        'SoundsToolStripMenuItem
        '
        Me.SoundsToolStripMenuItem.Name = "SoundsToolStripMenuItem"
        Me.SoundsToolStripMenuItem.Size = New System.Drawing.Size(224, 26)
        Me.SoundsToolStripMenuItem.Text = "Sounds"
        '
        'mnuHelp
        '
        Me.mnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuUserManual, Me.AboutTradeCompanionToolStripMenuItem})
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(53, 24)
        Me.mnuHelp.Text = "Help"
        '
        'mnuUserManual
        '
        Me.mnuUserManual.Name = "mnuUserManual"
        Me.mnuUserManual.Size = New System.Drawing.Size(247, 26)
        Me.mnuUserManual.Text = "User Manual"
        '
        'AboutTradeCompanionToolStripMenuItem
        '
        Me.AboutTradeCompanionToolStripMenuItem.Name = "AboutTradeCompanionToolStripMenuItem"
        Me.AboutTradeCompanionToolStripMenuItem.Size = New System.Drawing.Size(247, 26)
        Me.AboutTradeCompanionToolStripMenuItem.Text = "About Trade Companion"
        '
        'TabControl1
        '
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.HideTabsMode = Crownwood.Magic.Controls.TabControl.HideTabsModes.ShowAlways
        Me.TabControl1.Location = New System.Drawing.Point(0, 133)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.SelectedTab = Me.TabOrders
        Me.TabControl1.Size = New System.Drawing.Size(1190, 563)
        Me.TabControl1.TabIndex = 10
        Me.TabControl1.TabPages.AddRange(New Crownwood.Magic.Controls.TabPage() {Me.TabOrders, TabAlerts, Me.TabMDHistory, Me.TabLog})
        '
        'TabOrders
        '
        Me.TabOrders.Controls.Add(Me.SplitContainer1)
        Me.TabOrders.Location = New System.Drawing.Point(0, 0)
        Me.TabOrders.Name = "TabOrders"
        Me.TabOrders.Size = New System.Drawing.Size(1190, 534)
        Me.TabOrders.TabIndex = 0
        Me.TabOrders.Title = "Orders"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.pbxMarketDataStatus)
        Me.SplitContainer1.Panel1.Controls.Add(Me.btnSubscribeMarketData)
        Me.SplitContainer1.Panel1.Controls.Add(Me.btnDisconnectMarketData)
        Me.SplitContainer1.Panel1.Controls.Add(Me.SplitContainer2)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.grdIndSys)
        Me.SplitContainer1.Size = New System.Drawing.Size(1190, 534)
        Me.SplitContainer1.SplitterDistance = 260
        Me.SplitContainer1.TabIndex = 20
        '
        'btnSubscribeMarketData
        '
        Me.btnSubscribeMarketData.AutoSize = True
        Me.btnSubscribeMarketData.BackColor = System.Drawing.Color.LightSteelBlue
        Me.btnSubscribeMarketData.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.btnSubscribeMarketData.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.btnSubscribeMarketData.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.btnSubscribeMarketData.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSubscribeMarketData.Location = New System.Drawing.Point(812, 3)
        Me.btnSubscribeMarketData.Name = "btnSubscribeMarketData"
        Me.btnSubscribeMarketData.Size = New System.Drawing.Size(198, 37)
        Me.btnSubscribeMarketData.TabIndex = 21
        Me.btnSubscribeMarketData.Text = "Subscribe Marketdata"
        Me.btnSubscribeMarketData.UseVisualStyleBackColor = False
        '
        'btnDisconnectMarketData
        '
        Me.btnDisconnectMarketData.AutoSize = True
        Me.btnDisconnectMarketData.BackColor = System.Drawing.Color.LightSteelBlue
        Me.btnDisconnectMarketData.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.btnDisconnectMarketData.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.btnDisconnectMarketData.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.btnDisconnectMarketData.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDisconnectMarketData.Location = New System.Drawing.Point(989, 3)
        Me.btnDisconnectMarketData.Name = "btnDisconnectMarketData"
        Me.btnDisconnectMarketData.Size = New System.Drawing.Size(217, 37)
        Me.btnDisconnectMarketData.TabIndex = 20
        Me.btnDisconnectMarketData.Text = "Unsubscribe Marketdata"
        Me.btnDisconnectMarketData.UseVisualStyleBackColor = False
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.grdMarketData)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.OpenPositionGridEX)
        Me.SplitContainer2.Size = New System.Drawing.Size(1190, 260)
        Me.SplitContainer2.SplitterDistance = 460
        Me.SplitContainer2.TabIndex = 24
        '
        'grdMarketData
        '
        Me.grdMarketData.BackColor = System.Drawing.Color.WhiteSmoke
        Me.grdMarketData.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grdMarketData.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center
        Me.grdMarketData.Location = New System.Drawing.Point(0, 0)
        Me.grdMarketData.Name = "grdMarketData"
        Me.grdMarketData.Size = New System.Drawing.Size(460, 260)
        Me.grdMarketData.TabIndex = 19
        '
        'OpenPositionGridEX
        '
        Me.OpenPositionGridEX.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.[False]
        Me.OpenPositionGridEX.Dock = System.Windows.Forms.DockStyle.Fill
        Me.OpenPositionGridEX.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
        Me.OpenPositionGridEX.Location = New System.Drawing.Point(0, 0)
        Me.OpenPositionGridEX.Name = "OpenPositionGridEX"
        Me.OpenPositionGridEX.Size = New System.Drawing.Size(726, 260)
        Me.OpenPositionGridEX.TabIndex = 23
        Me.OpenPositionGridEX.TableHeaders = Janus.Windows.GridEX.InheritableBoolean.[True]
        '
        'grdIndSys
        '
        Me.grdIndSys.AcceptsEscape = False
        Me.grdIndSys.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.[False]
        Me.grdIndSys.BackColor = System.Drawing.Color.WhiteSmoke
        Me.grdIndSys.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grdIndSys.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center
        Me.grdIndSys.Location = New System.Drawing.Point(0, 0)
        Me.grdIndSys.Name = "grdIndSys"
        Me.grdIndSys.Size = New System.Drawing.Size(1190, 270)
        Me.grdIndSys.TabIndex = 0
        '
        'TabMDHistory
        '
        Me.TabMDHistory.Controls.Add(Me.grdMDHistory)
        Me.TabMDHistory.Location = New System.Drawing.Point(0, 0)
        Me.TabMDHistory.Name = "TabMDHistory"
        Me.TabMDHistory.Selected = False
        Me.TabMDHistory.Size = New System.Drawing.Size(1190, 534)
        Me.TabMDHistory.TabIndex = 2
        Me.TabMDHistory.Title = "MD History"
        '
        'grdMDHistory
        '
        Me.grdMDHistory.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grdMDHistory.Location = New System.Drawing.Point(0, 0)
        Me.grdMDHistory.Name = "grdMDHistory"
        Me.grdMDHistory.Size = New System.Drawing.Size(1190, 534)
        Me.grdMDHistory.TabIndex = 0
        '
        'TabLog
        '
        Me.TabLog.Controls.Add(Me.Panel2)
        Me.TabLog.Location = New System.Drawing.Point(0, 0)
        Me.TabLog.Name = "TabLog"
        Me.TabLog.Selected = False
        Me.TabLog.Size = New System.Drawing.Size(1190, 534)
        Me.TabLog.TabIndex = 3
        Me.TabLog.Title = "Log Window"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.btnClearLog)
        Me.Panel2.Controls.Add(Me.btnExportLog)
        Me.Panel2.Controls.Add(Me.RichTextBoxLog)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1190, 534)
        Me.Panel2.TabIndex = 0
        '
        'btnClearLog
        '
        Me.btnClearLog.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnClearLog.Location = New System.Drawing.Point(148, 487)
        Me.btnClearLog.Name = "btnClearLog"
        Me.btnClearLog.Size = New System.Drawing.Size(115, 31)
        Me.btnClearLog.TabIndex = 2
        Me.btnClearLog.Text = "Clear"
        Me.btnClearLog.UseVisualStyleBackColor = True
        '
        'btnExportLog
        '
        Me.btnExportLog.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExportLog.Location = New System.Drawing.Point(11, 487)
        Me.btnExportLog.Name = "btnExportLog"
        Me.btnExportLog.Size = New System.Drawing.Size(115, 31)
        Me.btnExportLog.TabIndex = 1
        Me.btnExportLog.Text = "Export"
        Me.btnExportLog.UseVisualStyleBackColor = True
        '
        'RichTextBoxLog
        '
        Me.RichTextBoxLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RichTextBoxLog.BackColor = System.Drawing.Color.White
        Me.RichTextBoxLog.Location = New System.Drawing.Point(0, 0)
        Me.RichTextBoxLog.Name = "RichTextBoxLog"
        Me.RichTextBoxLog.ReadOnly = True
        Me.RichTextBoxLog.ShowSelectionMargin = True
        Me.RichTextBoxLog.Size = New System.Drawing.Size(1190, 475)
        Me.RichTextBoxLog.TabIndex = 0
        Me.RichTextBoxLog.Text = ""
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Disconnected"
        Me.NotifyIcon1.Visible = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RestoreToolStrip, Me.MinimizeToolStrip, Me.ExitToolStrip})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(146, 82)
        '
        'RestoreToolStrip
        '
        Me.RestoreToolStrip.Name = "RestoreToolStrip"
        Me.RestoreToolStrip.Size = New System.Drawing.Size(145, 26)
        Me.RestoreToolStrip.Text = "Restore"
        '
        'MinimizeToolStrip
        '
        Me.MinimizeToolStrip.Name = "MinimizeToolStrip"
        Me.MinimizeToolStrip.Size = New System.Drawing.Size(145, 26)
        Me.MinimizeToolStrip.Text = "Minimize"
        '
        'ExitToolStrip
        '
        Me.ExitToolStrip.Name = "ExitToolStrip"
        Me.ExitToolStrip.Size = New System.Drawing.Size(145, 26)
        Me.ExitToolStrip.Text = "Exit"
        '
        'pbxMarketDataStatus
        '
        Me.pbxMarketDataStatus.Location = New System.Drawing.Point(1164, 8)
        Me.pbxMarketDataStatus.Name = "pbxMarketDataStatus"
        Me.pbxMarketDataStatus.Size = New System.Drawing.Size(28, 23)
        Me.pbxMarketDataStatus.TabIndex = 22
        Me.pbxMarketDataStatus.TabStop = False
        Me.pbxMarketDataStatus.Visible = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.TradeCompanion.My.Resources.Resources.Png_
        Me.PictureBox1.Location = New System.Drawing.Point(1018, 3)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(172, 94)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 10
        Me.PictureBox1.TabStop = False
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(1190, 696)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.mnuMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.mnuMain
        Me.MinimumSize = New System.Drawing.Size(1208, 135)
        Me.Name = "Form1"
        Me.Text = "AutoShark"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        TabAlerts.ResumeLayout(False)
        CType(Me.grdAlerts, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.grpFilter.ResumeLayout(False)
        Me.grpFilter.PerformLayout()
        Me.grpExecutionMode.ResumeLayout(False)
        Me.grpExecutionMode.PerformLayout()
        Me.mnuMain.ResumeLayout(False)
        Me.mnuMain.PerformLayout()
        Me.TabOrders.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.ResumeLayout(False)
        CType(Me.grdMarketData, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OpenPositionGridEX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grdIndSys, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabMDHistory.ResumeLayout(False)
        CType(Me.grdMDHistory, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabLog.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        CType(Me.pbxMarketDataStatus, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region
    'Private m_busy As Boolean = False
    Private keys As SettingsHome
    Private mDoNotConnect As Boolean
    Private mDoNotConnectMarketData As Boolean

    Private grdAlertLayOut As Janus.Windows.GridEX.GridEXLayout = Nothing
    'Private setLayOutAlerts As Boolean = False

    Private grdOrdersLayOut As Janus.Windows.GridEX.GridEXLayout = Nothing
    'Private setLayOutOrders As Boolean = False

    Private grdMarketDataLayOut As Janus.Windows.GridEX.GridEXLayout = Nothing
    'Private setLayOutOrders As Boolean = False
    Private reconnectThread As Thread
    Private reconnectThreadMarketData As Thread
    Private reconnectInterval As Integer

    Public HeartBeatReceived As Boolean = False
    Public dsMarkeData As DataSet
    'Public automate As XmlRead = New XmlRead()
    'Friend crashtest As Boolean = False 'holds the crash result
    'Function to start Metaserver
    Declare Auto Function Connect Lib "wrsthnk.dll" Alias "StartServer" (ByVal m_hWnd As IntPtr, ByRef errorCode As Integer) As Integer
    Declare Auto Function Disconnect Lib "wrsthnk.dll" Alias "StopServer" (ByRef errorCode As Integer) As Boolean
    Public ConnectHT As New Hashtable()
    'Dim connectionCount As Integer = 0
    Dim DisCount As Integer = 0
    Private Delegate Sub NoArgumentDelegate()
    Private Delegate Sub IntArgumentDelegate(ByVal b As ConnectionStatus)
    Private Delegate Sub BoolArgumentDelegate(ByVal b As Boolean)
    Private Delegate Sub MarketDataArgumentDelegate(ByVal mdata As TradingInterface.FillMarketData)
    Private Delegate Sub ColorArgumentDelegate(ByVal str As String, ByVal col As Color)
    Delegate Sub ConnectionStatusParameterDelegate(ByVal con As ConnectionStatus)
    Public netStatBool As Boolean = True

    Enum TAB_PAGE_OPEN
        TAB_ORDERS = 0
        TAB_ALERTS = 1
        TAB_MDHISTORY = 2
        TAB_PLWINDOW = 3
    End Enum

    Enum ConnectionStatus
        DISCONNECTED = 1
        CONNECTED = 2
        RECONNECTING = 3
    End Enum

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        keys.setSettings()
        Try
            'Dim wsScalper As New WSScalper.WebServicesScalper
            'Dim result As Boolean
            'result = wsScalper.Loggedin(keys.LoginidTC, False)
            'If (result = True) Then
            'Util.WriteDebugLog("Successful Logout From Server")
            'Else
            'Util.WriteDebugLog("Problem Server Logout")
            'End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "TradeCompanion")
        End Try

        mDoNotConnect = True
        mDoNotConnectMarketData = True

        NotifyIcon1.Visible = False

        'If Not marketdata Is Nothing Then marketdata.Logout()
        Try
            Dim trad As Trader
            Dim iEnum As IDictionaryEnumerator
            iEnum = ConnectHT.GetEnumerator()
            While iEnum.MoveNext()
                trad = CType(iEnum.Value, Trader)
                'trad.mDoNotConnect = True
                trad.logout()
                'automate.Removetradesettings(trad.ConnectionId)
                iEnum = ConnectHT.GetEnumerator()
            End While
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
        AlertsHome.DumpMDtoDB()
        'If Not marketdata Is Nothing Then marketdata.Logout()
        AlertsHome.DumpMDHistory()

        'automate.RemoveMarketsettings()
        'automate.Crashupdate("false")

        Dim result1 As Integer
        If Not Disconnect(result1) Then
            If (result1 = 16) Then
                Util.WriteDebugLog("Error closing metaserver. Error code:" & " the interface's thread is not responding")
            ElseIf (result1 = 1) Then
                Util.WriteDebugLog("Error closing metaserver. Error code:" & " error closing of memory-mapped file")
            ElseIf (result1 = 64) Then
                Util.WriteDebugLog("Error closing metaserver. Error code:" & "  interface is not initialized")
            Else
                Util.WriteDebugLog("Error closing metaserver. Error code:" & "  Unknown error")
            End If
        End If

        'Util.WriteDebugLog("Stop HeartBeat Thread")
        'Dim hb As HeartBeatThread = HeartBeatThread.getInstance()
        'hb.StopHeartBeat()   

        Util.WriteDebugLog("------------------------------ Closed -------------------------------------")

    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim handler As ThreadExceptionHandler = New ThreadExceptionHandler()
        AddHandler Application.ThreadException, AddressOf handler.Application_ThreadException
        Me.IsMdiContainer = True
        AddHandler NetworkChange.NetworkAvailabilityChanged, AddressOf netStatus
        Try
            Util.WriteDebugLog("------------------------------ Started -------------------------------------")
            keys = SettingsHome.getInstance()
            Dim verInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(Application.ExecutablePath)

            Util.WriteDebugLog("... Default Settings:")
            Util.WriteDebugLog("            Version       -" + verInfo.FileMajorPart.ToString() + "." + verInfo.FileMinorPart.ToString())
            Util.WriteDebugLog("            Build Version -" + verInfo.FileBuildPart.ToString())
            Util.WriteDebugLog("            ExchangeServer-" + keys.ExchangeServer)
            Util.WriteDebugLog("            Mode          -" + keys.ExecutionMode.ToString)
            EServerDependents.LogLoginDetails()
            Util.WriteDebugLog("            Monitor       -" + keys.TradeStationMonitorPath)
            Util.WriteDebugLog("            Default Size  -" + keys.UseDefaultTradeSize.ToString)
            Util.WriteDebugLog("            Mapping       -" + keys.UseSymbolMapping.ToString)
            Util.WriteDebugLog("            Filter Alerts -" + keys.FilterAlerts.ToString)
            Util.WriteDebugLog("            Platform      -" + keys.Platform)

            'start metaserver
            Dim result As Integer
            If (Connect(Me.Handle, result) = 0) Then
                If (result = 1) Then
                    Util.WriteDebugLog("Error Starting meta server ErrorCode:" & " error making the memory-mapped file")
                ElseIf (result = 256) Then
                    Util.WriteDebugLog("Error Starting meta server ErrorCode:" & " server thread is not responding")
                ElseIf (result = 128) Then
                    Util.WriteDebugLog("Error Starting meta server ErrorCode:" & " another application based on MSDK is already running")
                ElseIf (result = 2) Then
                    Util.WriteDebugLog("Error Starting meta server ErrorCode:" & " error starting winsock server")
                ElseIf (result = 4) Then
                    Util.WriteDebugLog("Error Starting meta server ErrorCode:" & " you have interface already running")
                Else
                    Util.WriteDebugLog("Error Starting meta server ErrorCode:" & " Unknown error")
                End If
            End If

            If CultureInfo.CurrentCulture.Name <> "en-US" Then
                Dim objCI As CultureInfo = New CultureInfo("en-US")
                objCI.DateTimeFormat.AMDesignator = "AM"
                objCI.DateTimeFormat.DateSeparator = "/"
                Thread.CurrentThread.CurrentCulture = objCI
                Thread.CurrentThread.CurrentUICulture = objCI
            End If

            rbAuto.Enabled = False
            rbManual.Enabled = False
            rbNoExecution.Checked = True
            btnConnect.Visible = True
            btnDisconnectMarketData.Enabled = False

            btnDisconnect.Visible = False
            EServerDependents.InitializeMarketDataButtons()
            UseSymbolMappingToolStripMenuItem.Checked = keys.UseSymbolMapping
            UseDefaultTradesizeToolStripMenuItem.Checked = keys.UseDefaultTradeSize
            FilterAlertsToolStripMenuItem.Checked = keys.FilterAlerts

            Dim style As Janus.Windows.GridEX.GridEXFormatStyle = New Janus.Windows.GridEX.GridEXFormatStyle
            style.ForeColor = Color.LightGray
            style.Key = "Rejected"
            grdAlerts.FormatStyles.Add(style)

            style = New Janus.Windows.GridEX.GridEXFormatStyle
            style.ForeColor = Color.Black
            style.Key = "Accepted"
            grdAlerts.FormatStyles.Add(style)


            style = New Janus.Windows.GridEX.GridEXFormatStyle
            style.ForeColor = Color.Black
            style.Key = "MarketData"
            grdMarketData.FormatStyles.Add(style)

            grdIndSys.ColumnAutoResize = True
            grdAlerts.ColumnAutoResize = True
            grdMarketData.ColumnAutoResize = True

            ShowAlerts()
            ShowUpdate()
            ShowMDHistory()
 			ShowOpenPosition()
            dsIndSys = ah.LoadIndividualSystem()
            showIndividualSystem()

            dtpEndDate.Value = DateTime.Now
            dtpEndDate.Checked = False
            If keys.ExchangeServer = ExchangeServer.CurrenEx Then
                CurrenExTradeToolStripMenuItem.Text = "CurrenEx Trade"
                CurrenExMarketDataToolStripMenuItem.Visible = True
                Me.Text = "Auto Shark - CurrenEx"
            ElseIf keys.ExchangeServer = ExchangeServer.Ariel Then
                CurrenExTradeToolStripMenuItem.Text = "Ariel Trade"
                CurrenExMarketDataToolStripMenuItem.Visible = False
                Me.Text = "Auto Shark - Ariel"
            ElseIf keys.ExchangeServer = ExchangeServer.Espeed Then
                Me.Text = "Auto Shark - eSpeed"
                CurrenExTradeToolStripMenuItem.Text = "eSpeed Trade"
                CurrenExMarketDataToolStripMenuItem.Visible = False
            ElseIf keys.ExchangeServer = ExchangeServer.DBFX Then
                Me.Text = "Auto Shark - DBFX"
                CurrenExTradeToolStripMenuItem.Text = "DBFX Trade"
                CurrenExMarketDataToolStripMenuItem.Visible = False
            ElseIf keys.ExchangeServer = ExchangeServer.Gain Then
                Me.Text = "Auto Shark - Gain Trade"
                CurrenExTradeToolStripMenuItem.Text = "Gain Trade"
                CurrenExMarketDataToolStripMenuItem.Visible = False
            ElseIf keys.ExchangeServer = ExchangeServer.Icap Then
                Me.Text = "Auto Shark - Icap"
                CurrenExTradeToolStripMenuItem.Text = "Icap Trade"
                CurrenExMarketDataToolStripMenuItem.Visible = False
            ElseIf keys.ExchangeServer = ExchangeServer.Dukascopy Then 'Vm_ Fix
                Me.Text = "Auto Shark - Dukascopy"
                CurrenExTradeToolStripMenuItem.Text = "Dukascopy Trade"
                CurrenExMarketDataToolStripMenuItem.Visible = False
            ElseIf keys.ExchangeServer = ExchangeServer.FxIntegral Then 'G_
                Me.Text = "Auto Shark - FxIntegral"
                CurrenExTradeToolStripMenuItem.Text = "FxIntegral Trade"
                CurrenExMarketDataToolStripMenuItem.Visible = False
            End If

            watcher = AlertWatcher.getInstance()
            watcher.InitializeMonitorPath(keys.Platform)

            'If crashtest Then
            '    Dim mktconnected As Boolean = automate.Autoconnect() 'testing marketdata need to be connected auto for the espeed & ariel
            '    If keys.ExchangeServer = ExchangeServer.CurrenEx Or Not mktconnected Then
            '        crashtest = False
            '    End If
            'End If
            'automate.Crashupdate("true")

        Catch ex As Exception
            MsgBox(ex.Message)
            Util.WriteDebugLog(" .... Form_Load ERROR " + ex.Message + ex.StackTrace)
            Me.Close()
            End
        End Try



        ' watcher.CurrentSettings = keys
        ' watcher.NotifyFilter = NotifyFilters.FileName
        ' AddHandler watcher.Created, AddressOf OnChanged
        ' AddHandler watcher.Error, AddressOf OnError

        ' watcher.EnableRaisingEvents = True

    End Sub

    Private Sub netStatus(ByVal sender As Object, ByVal e As NetworkAvailabilityEventArgs)
        netStatBool = e.IsAvailable
    End Sub
    Public Sub ShowAlerts()
        Try

            'm_busy = True
            Dim querry As String
            querry = ""
            If dtpStartDate.Checked Then
                querry = "Int(AlertDate) >= " + Int(dtpStartDate.Value.ToOADate()).ToString()
            End If
            If (dtpEndDate.Checked) Then
                If (querry <> "") Then
                    querry = querry + " and Int(AlertDate) <= " + Int(dtpEndDate.Value.ToOADate()).ToString()
                Else
                    querry = "Int(AlertDate) <= " + Int(dtpEndDate.Value.ToOADate()).ToString()
                End If
            End If

            Dim ds As DataSet = ah.LoadAlerts(querry)
            'Dim ds As DataSet = ah.LoadAlerts(Int(Now.ToOADate))

            grdAlerts.DataSource = ds
            grdAlerts.SetDataBinding(ds, "Alerts")
            grdAlerts.RetrieveStructure()
            grdAlerts.Tables(0).Columns("AlertID").Visible = False
            grdAlerts.Tables(0).Columns("status").Visible = False
            grdAlerts.Tables(0).Columns("SecurityType").Visible = False
            grdAlerts.Tables(0).Caption = "Captured Alerts"
            grdAlerts.TableHeaders = Janus.Windows.GridEX.InheritableBoolean.True
            grdAlerts.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False

            'CurrenEx specific change
            grdAlerts.Tables(0).Columns("ChartIdentifier").Visible = False
            grdAlerts.Tables(0).Columns("MonthYear").Visible = False
            grdAlerts.Tables(0).Columns("Exchange").Visible = False
            grdAlerts.Tables(0).Columns("Contracts").Caption = "Amount"
            grdAlerts.Tables(0).Columns("Contracts").FormatString = "#,#"
            grdAlerts.Tables(0).Columns("ActionType").FormatString = "IIF(ActionType = 1,'BUY', 'SELL')"
            grdAlerts.Tables(0).Columns("TradeType").FormatString = "IIF(TradeType = 1,'GTC','IOC')"
            grdAlerts.Tables(0).Columns("AlertDate").FormatString = "dd/MM/yyyy hh:mm:ss tt"
            grdAlerts.Refresh()
            Application.DoEvents()
            grdAlerts.MoveLast()

            If Not (grdAlertLayOut Is Nothing) Then
                grdAlerts.LoadLayout(grdAlertLayOut)
            End If
        Catch ex As Exception
            'MsgBox(ex.Message)
            Util.WriteDebugLog(" .... Show Alerts ERROR " + ex.Message)
        Finally
            'm_busy = False
        End Try

    End Sub

    Public Sub watcher_NewAlert(ByVal execute As AlertsManager.NewAlert) Handles watcher.NewAlert
        Util.WriteDebugLog(" .... New alert captured")
        Dim obj As Object
        Dim trader As Trader
        Try
            If Not execute Is Nothing Then
                Util.WriteDebugLog(" .... Sent for execution")
                Dim i As Integer
                'map the id
                Dim senderID As String = ""
                If (execute.timestamp <> "simulated") Then
                    execute.uID = execute.senderID
                    senderID = ah.MapTSID(execute.senderID)
                Else
                    execute.uID = "simulated"
                    senderID = execute.senderID
                End If
                If (senderID = "") Then
                    Form1.GetSingletonOrderform().AddInLogWindow("Alert rejected : invalid connection mapping >>" + execute.senderID, Color.Red)
                    Form1.GetSingletonOrderform().AddInLogWindow("....Details: " + execute.symbol + " " + execute.contracts.ToString(), Color.Red)
                    Util.WriteDebugLog(" .... Rejected for execution " + execute.senderID + " not found in table IDMAP")
                Else
                    If SettingsHome.getInstance.ExchangeServer = ExchangeServer.DBFX Then
                        obj = ConnectHT.Item(senderID)
                        trader = CType(obj, Trader)
                        'If trader.Stat = ConnectionStatus.CONNECTED Then
                        While trader Is Nothing AndAlso i <= 10000
                            i += 1
                        End While
                        If Not trader Is Nothing Then
                            execute.senderID = senderID
                            trader.send(execute)
                        End If
                    Else
                        'For other drivers
                        If (ConnectHT.ContainsKey(senderID.Trim())) Then
                            obj = ConnectHT.Item(senderID.Trim())
                            trader = CType(obj, Trader)
                            'If trader.Stat = ConnectionStatus.CONNECTED Then
                            While trader Is Nothing AndAlso i <= 10000
                                i += 1
                            End While
                            If Not trader Is Nothing Then
                                trader.send(execute)
                            End If
                        Else
                            Util.WriteDebugLog(" ....Rejected for execution Trader " + senderID + " not found")
                            Form1.GetSingletonOrderform().AddInLogWindow("Alert rejected :" + senderID + " not found", Color.Red)
                            Form1.GetSingletonOrderform().AddInLogWindow("....Details: " + execute.symbol + " " + execute.contracts.ToString(), Color.Red)
                        End If
                    End If
                End If
            Else
                Util.WriteDebugLog(" .... Rejected for execution")
                Form1.GetSingletonOrderform().AddInLogWindow("Alert rejected : unknown reason", Color.Red)
                Form1.GetSingletonOrderform().AddInLogWindow("....Details: " + execute.symbol + " " + execute.contracts.ToString(), Color.Red)
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)

            Util.WriteDebugLog(" .... Sent for execution ERROR " + ex.Message)
        End Try

        If InvokeRequired Then
            Dim del As New NoArgumentDelegate(AddressOf ShowAlerts)
            BeginInvoke(del)
        Else
            ShowAlerts()
        End If

    End Sub

    Private Sub UpdateWatcherSettings()
        If Not watcher Is Nothing Then watcher.CurrentSettings = keys
    End Sub

    Private Sub chkDefaultTradeSize_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'keys.UseDefaultTradeSize = chkDefaultTradeSize.Checked
        UpdateWatcherSettings()
        Util.WriteDebugLog(" .... Use Default Trade Size = TRUE")
    End Sub

    Private Sub chkSymbolMapping_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'keys.UseSymbolMapping = chkSymbolMapping.Checked
        UpdateWatcherSettings()
        Util.WriteDebugLog(" .... Use Symbol Mapping = TRUE")
    End Sub

    Private Sub rbAuto_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbAuto.CheckedChanged
        keys.ExecutionMode = SettingsHome.EXECUTION_AUTO
        'automate.UpdateSettings("executionmode")
        UpdateWatcherSettings()
        If rbAuto.Checked = True Then ShowIDMappingTable()
        Util.WriteDebugLog(" .... Execution mode = EXECUTION_AUTO")
    End Sub

    Private Sub rbManual_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbManual.CheckedChanged
        keys.ExecutionMode = SettingsHome.EXECUTION_MANUAL
        'automate.UpdateSettings("executionmode")
        UpdateWatcherSettings()
        If rbManual.Checked = True Then ShowIDMappingTable()
        Util.WriteDebugLog(" .... Execution mode = EXECUTION_MANUAL")
    End Sub

    Private Sub rbNoExecution_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbNoExecution.CheckedChanged
        keys.ExecutionMode = SettingsHome.EXECUTION_NONE
        'If Not crashtest Then automate.UpdateSettings("executionmode")
        UpdateWatcherSettings()
        Util.WriteDebugLog(" .... Execution mode = EXECUTION_NONE")
    End Sub

    Private Sub Form1_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        keys.setSettings()
    End Sub

    Private Sub lnkMapping_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Util.WriteDebugLog(" --- Show Mapping Table")
        ShowMappingTable()
    End Sub

    Private Sub grdAlerts_FormattingRow(ByVal sender As Object, ByVal e As Janus.Windows.GridEX.RowLoadEventArgs) Handles grdAlerts.FormattingRow
        Try
            Dim d As Double = e.Row.Cells("AlertDate").Value
            e.Row.Cells("AlertDate").Text = DateTime.FromOADate(d).ToString

            Dim actiontype As Integer = e.Row.Cells("ActionType").Value
            Select Case actiontype
                Case AlertsManager.ACTION_BUY
                    e.Row.Cells("ActionType").Text = "BUY"
                Case AlertsManager.ACTION_SELL
                    e.Row.Cells("ActionType").Text = "SELL"
            End Select
            Dim tradetype As Integer = e.Row.Cells("TradeType").Value
            Select Case tradetype
                Case AlertsManager.TYPE_GTC
                    e.Row.Cells("TradeType").Text = "GTC"
                Case AlertsManager.TYPE_IOC
                    e.Row.Cells("TradeType").Text = "IOC"
            End Select

            Dim securitytype As Integer = e.Row.Cells("SecurityType").Value

            Select Case securitytype
                Case AlertsManager.TYPE_EQUITY
                    e.Row.Cells("SecurityType").Text = "EQUITY"
                Case AlertsManager.TYPE_FUTURE
                    e.Row.Cells("SecurityType").Text = "FUTURE"
            End Select
            If e.Row.Cells("status").Value = AlertsManager.STATUS_REJECTED Then
                e.Row.RowStyle = grdAlerts.FormatStyles("Rejected")
            Else
                e.Row.RowStyle = grdAlerts.FormatStyles("Accepted")
            End If
        Catch ex As Exception
            'MsgBox(ex.Message)
            Util.WriteDebugLog(" Alerts Formating Row ERROR " + ex.Message)
        End Try

    End Sub

    Private Sub btnConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConnect.Click
        Util.WriteDebugLog(" --- Connect")
        Connect()
    End Sub

    Private Function GetTradeConnectionCount() As Integer
        Dim obj As Object
        Dim s1 As Trader
        Dim iEnum As IDictionaryEnumerator
        iEnum = ConnectHT.GetEnumerator()
        Dim count As Integer = 0
        While iEnum.MoveNext()
            obj = iEnum.Value
            s1 = CType(obj, Trader)
            If ((keys.ExchangeServer = ExchangeServer.CurrenEx) Or (keys.ExchangeServer = ExchangeServer.Dukascopy) _
            Or (keys.ExchangeServer = ExchangeServer.FxIntegral)) Then
                If (s1.IsMarketDataConnection = False) Then
                    count = count + 1
                End If
            Else
                count = count + 1
            End If
        End While
        Return count
    End Function

    Private Sub btnDisconnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisconnect.Click
        Util.WriteDebugLog("--- Disconnect")
        mDoNotConnect = True

        Dim disconectForm As New DisconnectForm()
        disconectForm.DisconnectShow(ConnectHT)
        Dim result As DialogResult = disconectForm.ShowDialog()
        'DisCount = DisconnectForm.disCount
        'connectionCount = connectionCount - DisCount
        ShowConnectionStatus(GetTradeConnectionCount())
    End Sub

    Private Sub ShowConnectionStatus(ByVal isConnected As Integer)
        If isConnected = 0 Then
            rbAuto.Enabled = False
            rbManual.Enabled = False
            btnConnect.Visible = True
            btnDisconnect.Visible = False
            btnPlaceOrder.Visible = False
            NotifyIcon1.Icon = New Icon(Application.StartupPath + "\TCRed.ico")
            NotifyIcon1.Text = "Disconnected"
        Else
            rbAuto.Enabled = True
            rbManual.Enabled = True
            btnConnect.Visible = True
            btnDisconnect.Visible = True
            btnDisconnect.Text = "Disconnect Trade"
            btnPlaceOrder.Visible = True
            btnPlaceOrder.Enabled = True
            Select Case keys.ExecutionMode
                Case SettingsHome.EXECUTION_AUTO
                    rbAuto.Checked = True
                Case SettingsHome.EXECUTION_MANUAL
                    rbManual.Checked = True
                Case SettingsHome.EXECUTION_NONE
                    rbNoExecution.Checked = True
            End Select
            NotifyIcon1.Icon = New Icon(Application.StartupPath + "\TCGreen.ico")
            NotifyIcon1.Text = "Connected" + vbNewLine + "No. of connections: " + ConnectHT.Count.ToString()
        End If

    End Sub

    Private Sub Connect()
        Try
            Util.WriteDebugLog(" .... Connecting")
            If (SettingsHome.getInstance().ExchangeServer = ExchangeServer.Espeed) Then
                If (ConnectHT.Count = 1) Then
                    MessageBox.Show("Espeed does not support multiple connections", "TradeCompanion")
                    Return
                End If
            End If
            If (SettingsHome.getInstance().ExchangeServer = ExchangeServer.Icap) Then
                If (ConnectHT.Count = 1) Then
                    MessageBox.Show("Icap does not support multiple connections", "TradeCompanion")
                    Return
                End If
            End If
            Dim logonScreen As Form = EServerDependents.GetLoginForm()
            Dim result As DialogResult = logonScreen.ShowDialog()
            Select Case result
                Case Windows.Forms.DialogResult.OK
                Case Windows.Forms.DialogResult.Cancel
                    Exit Sub
            End Select
            If ConnectHT.Contains(EServerDependents.GetEServerSender().Trim()) Then
                MessageBox.Show("Connection already exists for ID: " + EServerDependents.GetEServerSender())
            Else
                Dim trader As New Trader()
                trader = EServerDependents.SetTrader(trader)
                trader.mDoNotConnect = False
                AddHandler trader.ChangeStat, AddressOf StatechangeHandler
                AddHandler trader.OpenPosition, AddressOf OpenPosition
                If (trader.Logon()) Then
                    'ConnectHT.Add(EServerDependents.GetEServerSender().Trim(), trader)
                    'Util.WriteDebugLog(" .... Connected Trading")
                    AddTraderObject(EServerDependents.GetEServerSender().Trim(), trader)
                Else
                    Util.WriteDebugLog(" .... Connection Trading was not established")
                End If
                If SettingsHome.getInstance.ExchangeServer = ExchangeServer.Ariel Then
                    AddTraderObject(EServerDependents.GetEServerSender().Trim(), trader)
                End If
                'If Not crashtest Then automate.AddTradesettings()
                'ConnectionsShow()
                'ShowConnectionStatus(GetTradeConnectionCount())
            End If
        Catch ex As Exception
            Util.WriteDebugLog(" ERROR  Connect" + ex.Message)
            Util.WriteDebugLog(" Stack Trace" + ex.StackTrace)
        End Try
    End Sub
    Private Delegate Sub ShowConnectionBtn(ByVal countID As Integer)

    Public Sub AddTraderObject(ByVal connID As String, ByVal traderObj As Trader)
        If Not ConnectHT.ContainsKey(connID) Then
            ConnectHT.Add(connID, traderObj)
            Util.WriteDebugLog(" .... Connected Trading")
            StatechangeHandler()
            Dim delShowConnection As New ShowConnectionBtn(AddressOf ShowConnectionStatus)
            BeginInvoke(delShowConnection, GetTradeConnectionCount())
        End If
    End Sub

    Private Sub ShowOpenPosition()
        dsAllSys = ah.loadPLCal()
        dsAllSys.Tables(0).Columns("RowID").AutoIncrement = True
        dsAllSys.Tables(0).Columns("RowID").AutoIncrementSeed = 1000
        dsAllSys.Tables(0).Columns("RowID").AutoIncrementStep = 10

        dsAllSys.Tables(0).Columns.Add("TotalP&L")
        dsAllSys.Tables(0).Columns.Add("BrokerPosition")
        OpenPositionGridEX.DataSource = dsAllSys
        OpenPositionGridEX.SetDataBinding(dsAllSys, "PLCal")
        OpenPositionGridEX.RetrieveStructure()
        OpenPositionGridEX.KeepRowSettings = True
        OpenPositionGridEX.Tables(0).Caption = "All Systems"
        OpenPositionGridEX.Tables(0).Groups.Add("AccountID")
        OpenPositionGridEX.Tables(0).Columns("NetCC2").Visible = False
        OpenPositionGridEX.Tables(0).Columns("AverageSellRate").Visible = False
        OpenPositionGridEX.Tables(0).Columns("AllInRate").Visible = False
        OpenPositionGridEX.Tables(0).Columns("AccountID").Visible = False
        OpenPositionGridEX.Tables(0).Columns("MktRate").Visible = False
        OpenPositionGridEX.Tables(0).Columns("Unrealized").Visible = False
        OpenPositionGridEX.Tables(0).Columns("Realized").Visible = False
        OpenPositionGridEX.Tables(0).Columns("AverageBuyRate").Visible = False
        OpenPositionGridEX.Tables(0).Columns("RowID").Visible = False
        OpenPositionGridEX.Tables(0).Columns("BaseSymbol").Visible = False
        OpenPositionGridEX.Tables(0).Columns("OpenRate").Visible = False
        OpenPositionGridEX.Tables(0).Columns("UnRealizedBaseCurrency").Caption = "Opoen Position P&L"
        OpenPositionGridEX.Tables(0).Columns("UnRealizedBaseCurrency").Position = 3
        OpenPositionGridEX.Tables(0).Columns("RealizedBaseCurrency").Caption = "Realized P&L"
        OpenPositionGridEX.Tables(0).Columns("RealizedBaseCurrency").Position = 4
        OpenPositionGridEX.Tables(0).Columns("NetCC1").Caption = "Opoen Position"
        OpenPositionGridEX.Tables(0).Columns("NetCC1").Position = 2
        OpenPositionGridEX.Tables(0).Columns("Symbol").Position = 1
        OpenPositionGridEX.Tables(0).Columns("TotalP&L").Caption = "Total P&L"
        OpenPositionGridEX.Tables(0).Columns("BrokerPosition").Caption = "Broker Position"
        OpenPositionGridEX.Refresh()
    End Sub


    Private Sub Updateopenposition(ByVal row As DataRow) Handles plcal.PLCalculation
        upda.UpDateGird(row, dsAllSys)
    End Sub

    Private Sub OpenPosition(ByVal openvalue As String, ByVal _Instrument As String, ByVal _UserId As String)
        Try
            Dim filter As String = ""
            filter = "Symbol= '" + _Instrument + "' And AccountId = '" + _UserId + "'"
            Dim dr() As DataRow = dsAllSys.Tables(0).Select(filter)
            If (dr.Length = 0) Then
                'some times Data set won't update give some time to update the dataSet
                Thread.Sleep(100)
                dr = dsAllSys.Tables(0).Select(filter)
            End If
            dr(0).Item("BrokerPosition") = openvalue
            dsAllSys.AcceptChanges()
            OpenPositionGridEX.Refresh()
            ChangeBackcolor(dr(0))
        Catch ex As Exception
            Util.WriteDebugLog("Form1 -OpenPosition " + ex.Message + ex.StackTrace)
        End Try
    End Sub

    Public Sub ChangeBackcolor(ByVal dr As DataRow)
        Try
            Dim formatStyle As GridEXFormatStyle
            Dim rowGride As GridEXRow = OpenPositionGridEX.GetRow(dr)
            formatStyle = New GridEXFormatStyle()
            formatStyle.BackColor = Color.LightGreen
            If (dr.Item("BrokerPosition").ToString() <> dr.Item("NetCC1").ToString()) Then
                formatStyle.BackColor = Color.LightPink
            End If
            If Not IsNothing(rowGride) Then
                rowGride.Cells("BrokerPosition").FormatStyle = formatStyle
            End If
        Catch ex As Exception
            Util.WriteDebugLog("Form1 -ChangeBackcolor " + ex.Message + ex.StackTrace)
        End Try
    End Sub

    Private Sub ShowMappingTable()
        Mapping = MappingTable.Instance
        Mapping.Owner = Me
        Mapping.WindowState = FormWindowState.Normal
        Mapping.SetDesktopLocation(100, 100)
        Mapping.Show()
    End Sub

    Private Sub ShowIDMappingTable()
        'If IdMapping.IsDisposed Then IdMapping = New IDMappingTable
        IdMapping = IDMappingTable.Instance
        IdMapping.Owner = Me
        IdMapping.WindowState = FormWindowState.Normal
        IdMapping.Show()
    End Sub

    Private Sub mnuExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExit.Click
        Util.WriteDebugLog("--- Exit")
        Try
            Dim trad As Trader
            Dim iEnum As IDictionaryEnumerator
            iEnum = ConnectHT.GetEnumerator()
            While iEnum.MoveNext()
                trad = CType(iEnum.Value, Trader)
                'trad.mDoNotConnect = True
                trad.logout()
                'automate.Removetradesettings(trad.ConnectionId)
                iEnum = ConnectHT.GetEnumerator()
            End While
        Catch ex As Exception
            'MessageBox.Show(ex.ToString())
        End Try
        If btnDisconnectMarketData.Enabled Then
            DisconnectMarketData()
        End If
        Me.Close()
    End Sub

    Private Sub mnuConnect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuConnect.Click
        Util.WriteDebugLog("--- Connect")
        Connect()
    End Sub

    Private Sub mnuMapping_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Util.WriteDebugLog("--- Mapping")
        ShowMappingTable()
    End Sub

    Private Sub btnPlaceOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPlaceOrder.Click
        Try
            Dim result As Windows.Forms.DialogResult
            Dim mapped As AlertsManager.NewAlert = New AlertsManager.NewAlert
            Dim AlertWindow As ManualAlert = New ManualAlert
            result = AlertWindow.ShowDialog
            mapped = AlertWindow.alertdata


            If result = Windows.Forms.DialogResult.OK Then
                'We should update the alert table only if the order is placed properly.
                'SO moved the following logic to AlertExecution.vb which will get notified from execution.vb on 
                'Successful placement of the order.

                'Dim i As Integer = 0
                'mapped.status = result
                'mapped.timestamp = "simulated"
                'ah.AddAlert(mapped)
                If (Not (mapped Is Nothing)) Then
                    mapped.timestamp = "simulated"
                    watcher_NewAlert(mapped)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Util.WriteDebugLog("Place order ERROR:" + ex.Message + ex.Source)
            Util.WriteDebugLog("Place order ERROR:" + ex.StackTrace)
        End Try
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        d.BeginInvoke(Nothing, Nothing)
    End Sub

    Private Sub mnuLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CurrenExTradeToolStripMenuItem.Click
        Util.WriteDebugLog(" .... Login Settings")
        Dim logonScreen As Form = EServerDependents.GetLoginForm()
        logonScreen.ShowDialog()
    End Sub

    Private Sub mnuTrade_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TradeToolStripMenuItem.Click
        Util.WriteDebugLog(" .... Trade Settings")

        Dim tradeSettings As New TradeSettings
        tradeSettings.Text = "Trade Settings"
        tradeSettings.ShowDialog()
    End Sub

    Private Sub SoundsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SoundsToolStripMenuItem.Click
        Util.WriteDebugLog(" .... Sound Settings")

        Dim soundSettings As New SoundSettings
        soundSettings.Text = "Sound Settings"
        soundSettings.ShowDialog()
    End Sub

    Private Sub grdAlerts_GroupsChanged(ByVal sender As System.Object, ByVal e As Janus.Windows.GridEX.GroupsChangedEventArgs) Handles grdAlerts.GroupsChanged
        'setLayOutAlerts = True
        grdAlertLayOut = New Janus.Windows.GridEX.GridEXLayout
        grdAlertLayOut = grdAlerts.DesignTimeLayout
    End Sub

    Private Sub mnuUserManual_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuUserManual.Click
        Dim helpfile As String = Application.StartupPath + "\help.chm"
        Help.ShowHelp(Me, helpfile)
    End Sub

    Private Sub AboutTradeCompanionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutTradeCompanionToolStripMenuItem.Click
        Dim aTradeCompanion As New AboutTradeCompanion
        aTradeCompanion.ShowDialog()
    End Sub

    Private Sub mnuLoginTC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoginTCToolStripMenuItem.Click
        Util.WriteDebugLog(" .... Login Settings TC")

        Dim logonTC As New LoginSettingsScalper()
        logonTC.ShowDialog()
    End Sub

    Private Sub market_data_updated(ByVal mdata As TradingInterface.FillMarketData)
        Try
            If InvokeRequired Then
                Dim del As New MarketDataArgumentDelegate(AddressOf UpdateMarketData)
                BeginInvoke(del, mdata)
            Else
                UpdateMarketData(mdata)
            End If
        Catch ex As Exception
            Util.WriteDebugLog(" .... _OrderPlaced - ERROR " + ex.Message)
        End Try
    End Sub

    Private Sub UpdateMarketData(ByVal mdata As TradingInterface.FillMarketData)
        'SyncLock Me
        Dim drr() As DataRow = dsMarkeData.Tables(0).Select("Symbol = '" + mdata.Symbol + "'")
        If (drr.Length > 0) Then
            'This to check the market data changed or not in case of change of marketdate
            'Update or leave it with old value
            If (drr(0).ItemArray(2) <> mdata.BidPrice Or drr(0).ItemArray(3) <> mdata.OfferPrice) Then
                drr(0)("BidPrice") = mdata.BidPrice
                drr(0)("OfferPrice") = mdata.OfferPrice
                drr(0)("TimeStamps") = DateTime.Now.ToString()
            End If
        Else
            Dim dr As DataRow = dsMarkeData.Tables(0).NewRow()
            dr("Symbol") = mdata.Symbol
            dr("BidPrice") = mdata.BidPrice
            dr("OfferPrice") = mdata.OfferPrice
            dr("TimeStamps") = DateTime.Now.ToString()
            dsMarkeData.Tables(0).Rows.Add(dr)
        End If
        'End SyncLock
    End Sub

    Public Sub ShowUpdate()
        Try
            dsMarkeData = ah.LoadMarketData()
            dsMarkeData.EnforceConstraints = False

            ' Create the second, calculated, column.
            Dim diffColumn As DataColumn = New DataColumn
            With diffColumn
                .DataType = System.Type.GetType("System.String")
                .ColumnName = "Difference"
                .Expression = "Convert(OfferPrice, 'System.Decimal') - Convert(BidPrice, 'System.Decimal')" '"BidPrice - OfferPrice"
            End With


            ' Add columns to DataTable
            dsMarkeData.Tables("MarketData").Columns.Add(diffColumn)

            dsMarkeData.Tables(0).Columns("ID").AutoIncrement = True

            grdMarketData.DataSource = dsMarkeData
            grdMarketData.SetDataBinding(dsMarkeData, "MarketData")
            grdMarketData.RetrieveStructure()
            grdMarketData.Tables(0).Columns("ID").Visible = False
            grdMarketData.Tables(0).Caption = "Market Data"
            grdMarketData.TableHeaders = Janus.Windows.GridEX.InheritableBoolean.True
            grdMarketData.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False
            grdMarketData.AutoSizeColumns()
            grdMarketData.Tables(0).Columns("Difference").Caption = "Spread"
            grdMarketData.Tables(0).Columns("Difference").Position = 4
            If (Not (grdMarketDataLayOut Is Nothing)) Then
                grdMarketData.LoadLayout(grdMarketDataLayOut)
            End If
        Catch ex As Exception
            Util.WriteDebugLog(" .... Show Alerts ERROR " + ex.Message)
        End Try

    End Sub

    Public Sub ShowMDHistory()
        Try
            Dim querry As String
            querry = ""
            If dtpStartDate.Checked Then
                querry = "Int(DateMDData) >= " + Int(dtpStartDate.Value.ToOADate()).ToString()
            End If
            If (dtpEndDate.Checked) Then
                If (querry <> "") Then
                    querry = querry + " and Int(DateMDData) <= " + Int(dtpEndDate.Value.ToOADate()).ToString()
                Else
                    querry = "Int(DateMDData) <= " + Int(dtpEndDate.Value.ToOADate()).ToString()
                End If
            End If

            Dim ds As DataSet = ah.LoadMarketDataHistory(querry)

            grdMDHistory.DataSource = ds
            grdMDHistory.SetDataBinding(ds, "MDHistory")
            grdMDHistory.RetrieveStructure()
            grdMDHistory.Tables(0).Columns("ID").Visible = False
            grdMDHistory.Tables(0).Caption = "Market Data History"
            grdMDHistory.TableHeaders = Janus.Windows.GridEX.InheritableBoolean.True
            grdMDHistory.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False

            grdMDHistory.Tables(0).Columns("TimeStamps").Width = 148
            grdMDHistory.Tables(0).Columns("Symbol").Width = 195
            grdMDHistory.Tables(0).Columns("DateMDData").Width = 148
            grdMDHistory.Tables(0).Columns("DateMDData").FormatString = "dd/MM/yyyy"
            grdMDHistory.Tables(0).Columns("TimeFrame").Width = 148
            grdMDHistory.Tables(0).Columns("TimeStamps").FormatString = "dd/MM/yyyy hh:mm:ss tt"
            grdMDHistory.Tables(0).Columns("MaxDifference").Width = 148
            grdMDHistory.Tables(0).GroupMode = GroupMode.Collapsed
            grdMDHistory.Tables(0).Groups.Add("DateMDData")
            grdMDHistory.Tables(0).Groups.Add("TimeFrame")
            grdMDHistory.Tables(0).Columns("TimeFrame").FormatString = "-"
            grdMDHistory.Refresh()

        Catch ex As Exception
            Util.WriteDebugLog(" .... Show Alerts ERROR " + ex.Message)
        End Try

    End Sub

    Private Sub mnuMarketData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CurrenExMarketDataToolStripMenuItem.Click
        Util.WriteDebugLog(" .... Login Settings CurrenEX Market Data")

        Dim marketdata As New LoginMarketData
        marketdata.ShowDialog()
    End Sub

    Private Sub btnSubscribeMarketData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubscribeMarketData.Click

        Try
            Util.WriteDebugLog(" --- Connect Market Data ")
            Select Case SettingsHome.getInstance().ExchangeServer
                Case ExchangeServer.CurrenEx
                    ConnectMarketData()
                Case ExchangeServer.Dukascopy 'Vm_ Fix
                    ConnectMarketData()
                Case ExchangeServer.FxIntegral 'Giri
                    ConnectMarketData()
                Case ExchangeServer.Ariel
                    AssignTrderConnection()
                Case ExchangeServer.Espeed
                    AssignTrderConnection()
                Case ExchangeServer.DBFX
                    AssignTrderConnection()
                Case ExchangeServer.Gain
                    AssignTrderConnection()
                Case ExchangeServer.Icap
                    AssignTrderConnection()
            End Select
        Catch ex As Exception
            Util.WriteDebugLog(" ERROR  SubscribeMarketData" + ex.Message)
            Util.WriteDebugLog(" StackTrace" + ex.StackTrace)
        End Try

    End Sub

    Private Sub AssignTrderConnection()
        Try
            Dim _trader As Trader
            Dim enumerator As IDictionaryEnumerator = ConnectHT.GetEnumerator()
            Dim obj As Object = Nothing
            If (enumerator.MoveNext()) Then obj = enumerator.Value
            If (Not obj Is Nothing) Then
                _trader = CType(obj, Trader)
                If (_trader.Stat = ConnectionStatus.CONNECTED) Then
                    AddHandler _trader.MarketDataUpdated, AddressOf market_data_updated
                    _trader.IsMarketDataConnection = True
                    _trader.SubscribeMarketData()
                    'automate.Addmarketsettings()
                Else
                    MessageBox.Show("No active connections", "Tradecompanion")
                End If
            Else
                MessageBox.Show("No connections available", "Tradecompanion")
            End If
        Catch ex As Exception
            Util.WriteDebugLog(" ERROR  AssignTrderConnection" + ex.Message)
            Util.WriteDebugLog(" Stack Trace" + ex.StackTrace)
        End Try
    End Sub

    Private Sub ConnectMarketData()
        Try
            Util.WriteDebugLog(" .... Connecting Market Data")
            Dim logonScreen As New LoginMarketData
            Dim result As DialogResult = logonScreen.ShowDialog()
            Select Case result
                Case Windows.Forms.DialogResult.OK
                    mDoNotConnectMarketData = False
                Case Windows.Forms.DialogResult.Cancel
                    mDoNotConnectMarketData = True
                    'DisconnectMarketData()
                    Exit Sub
            End Select
            'New connection with market data
            Dim _trader As Trader = New Trader()
            _trader = EServerDependents.SetTraderMarketData(_trader)
            AddHandler _trader.ChangeStat, AddressOf StatechangeHandler
            AddHandler _trader.MarketDataUpdated, AddressOf market_data_updated
            _trader.mDoNotConnect = False
            _trader.IsMarketDataConnection = True
            'If Not crashtest Then automate.Addmarketsettings()
            If (_trader.Logon()) Then
                ShowConnectionStatusMarketData(ConnectionStatus.CONNECTED)
                Util.WriteDebugLog(" .... Connected Market Data")
                '_trader.SubscribeMarketData()
            Else
                ShowConnectionStatusMarketData(ConnectionStatus.DISCONNECTED)
                Util.WriteDebugLog(" .... Connection Market Data was not established")
            End If
            If (ConnectHT.Contains(EServerDependents.GetEServerSenderMarketData().Trim())) Then ConnectHT.Remove(EServerDependents.GetEServerSenderMarketData().Trim())
            ConnectHT.Add(EServerDependents.GetEServerSenderMarketData().Trim(), _trader)
        Catch ex As Exception
            Util.WriteDebugLog(" ERROR  ConnectMarketData" + ex.Message)
            Util.WriteDebugLog(" ERROR  Stack Trace" + ex.StackTrace)
        End Try

    End Sub

    Public Sub DisconnectMarketData()

        Dim obj As Object
        Dim IdCount As Integer = 0
        Dim j As Integer = 0
        Dim s1 As Trader
        Dim iEnum As IDictionaryEnumerator
        iEnum = ConnectHT.GetEnumerator()
        While iEnum.MoveNext()
            obj = iEnum.Value
            s1 = CType(obj, Trader)
            If (s1.IsMarketDataConnection) Then
                If ((SettingsHome.getInstance().ExchangeServer = ExchangeServer.CurrenEx) Or (SettingsHome.getInstance().ExchangeServer = ExchangeServer.Dukascopy) Or (SettingsHome.getInstance().ExchangeServer = ExchangeServer.FxIntegral)) Then
                    s1.mDoNotConnect = True
                End If
                s1.IsMarketDataConnection = False
                s1.UnSubscribeMarketData()
                Dim del As New IntArgumentDelegate(AddressOf ShowConnectionStatusMarketData)
                BeginInvoke(del, ConnectionStatus.DISCONNECTED)
                'automate.RemoveMarketsettings()
                Exit Sub
            End If
        End While
        Util.WriteDebugLog("---Disconnected Market Data")
    End Sub

    Private Sub ShowConnectionStatusMarketData(ByVal isConnected As ConnectionStatus)
        Try
            Select Case (isConnected)
                Case ConnectionStatus.CONNECTED
                    btnSubscribeMarketData.Enabled = False
                    btnDisconnectMarketData.Enabled = True
                    btnDisconnectMarketData.Text = "Disconnect Marketdata"
                    MarketDataConnToolStripMenuItem.Enabled = False
                    pbxMarketDataStatus.Visible = True
                    pbxMarketDataStatus.BackColor = Color.Green
                Case ConnectionStatus.DISCONNECTED
                    btnSubscribeMarketData.Enabled = True
                    btnDisconnectMarketData.Enabled = False
                    'pbxMarketDataStatus.BackColor = Color.Red
                    pbxMarketDataStatus.Visible = False
                    MarketDataConnToolStripMenuItem.Enabled = True
                Case ConnectionStatus.RECONNECTING
                    btnSubscribeMarketData.Enabled = False
                    btnDisconnectMarketData.Text = "Reconnecting...Cancel"
                    btnDisconnectMarketData.Enabled = True
                    pbxMarketDataStatus.Visible = True
                    pbxMarketDataStatus.BackColor = Color.Orange
            End Select
            pbxMarketDataStatus.Refresh()
        Catch ex As Exception
            Util.WriteDebugLog(" ERROR  ShowConnectionStatusMarketData" + ex.Message)
            Util.WriteDebugLog(" ERROR  Stack Trace" + ex.StackTrace)
            'MsgBox(ex.Message)
            'MsgBox(ex.StackTrace)
        End Try
    End Sub

    Private Sub btnDisconnectMarketData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisconnectMarketData.Click
        Try
            DisconnectMarketData()
        Catch ex As Exception
            Util.WriteDebugLog(" ERROR  btnDisconnectMarketData_Click" + ex.Message + " " + ex.StackTrace)
        End Try
    End Sub

    Private Sub btnGetOrders_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetOrders.Click
        Select Case TabControl1.SelectedIndex
            Case TAB_PAGE_OPEN.TAB_ORDERS
                ShowOpenPosition()
            Case TAB_PAGE_OPEN.TAB_ALERTS
                ShowAlerts()
            Case TAB_PAGE_OPEN.TAB_MDHISTORY
                ShowMDHistory()
        End Select
    End Sub

    Private Sub grdMarketData_GroupsChanged(ByVal sender As System.Object, ByVal e As Janus.Windows.GridEX.GroupsChangedEventArgs) Handles grdMarketData.GroupsChanged
        grdMarketDataLayOut = New Janus.Windows.GridEX.GridEXLayout
        grdMarketDataLayOut = grdMarketData.DesignTimeLayout
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim grid As GridEX = Nothing
        Select Case TabControl1.SelectedIndex
            Case TAB_PAGE_OPEN.TAB_ORDERS
                grid = grdIndSys
            Case TAB_PAGE_OPEN.TAB_ALERTS
                grid = grdAlerts
            Case TAB_PAGE_OPEN.TAB_MDHISTORY
                grid = grdMDHistory
        End Select

        Dim sfd As SaveFileDialog = New SaveFileDialog
        sfd.Filter = "Excel file (*.xls)|*.xls"
        If sfd.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim stream As FileStream = Nothing
            Try
                Dim gridEXExporter As GridEXExporter = New GridEXExporter
                gridEXExporter.IncludeFormatStyle = True
                gridEXExporter.GridEX = grid

                'If TypeOf grid.DataSource Is DataTable AndAlso CType(grid.DataSource, DataTable).ChildRelations.Count > 0 Then
                '    gridEXExporter.IncludeChildTables = True
                'End If
                gridEXExporter.IncludeCollapsedRows = False
                gridEXExporter.IncludeHeaders = True
                stream = New FileStream(sfd.FileName, FileMode.Create)
                gridEXExporter.Export(stream)
                Select Case TabControl1.SelectedIndex
                    Case TAB_PAGE_OPEN.TAB_ORDERS
                        MessageBox.Show("Orders Exported", "TradeCompanion")
                    Case TAB_PAGE_OPEN.TAB_ALERTS
                        MessageBox.Show("Alerts Exported", "TradeCompanion")
                    Case TAB_PAGE_OPEN.TAB_MDHISTORY
                        MessageBox.Show("MDHistroy Exported", "TradeCompanion")
                End Select
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            Finally
                If Not (stream Is Nothing) Then
                    stream.Close()
                    stream.Dispose()
                    stream = Nothing
                End If
            End Try
        Else
            'MessageBox.Show("Excel Export Cancel!")
        End If
    End Sub

    Private Sub TabControl1_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl1.SelectionChanged
        Select Case TabControl1.SelectedIndex
            Case 0
                grpFilter.Text = "Filter Orders"
                btnGetOrders.Text = "Get Orders"
            Case 1
                grpFilter.Text = "Filter Alerts"
                btnGetOrders.Text = "Get Alerts"
            Case 2
                grpFilter.Text = "Filter MData"
                btnGetOrders.Text = "Get MData"
        End Select
    End Sub

    Private Sub grdMDHistory_FormattingRow(ByVal sender As Object, ByVal e As Janus.Windows.GridEX.RowLoadEventArgs) Handles grdMDHistory.FormattingRow
        Try
            If e.Row.RowType = RowType.GroupHeader Then
                If e.Row.Group.Column.Key = "DateMDData" Then
                    e.Row.GroupCaption = DateTime.FromOADate(e.Row.GroupValue)
                ElseIf e.Row.Group.Column.Key = "TimeFrame" Then
                    e.Row.GroupCaption = e.Row.GroupValue.ToString() + " - " + (e.Row.GroupValue + 1).ToString()
                End If
            End If

            Dim d As Double = e.Row.Cells("DateMDData").Value
            If (d > 0) Then
                e.Row.Cells("DateMDData").Text = DateTime.FromOADate(d).ToString.Split(" ")(0)

            End If
            d = e.Row.Cells("TimeStamps").Value
            If (d > 0) Then
                e.Row.Cells("TimeStamps").Text = DateTime.FromOADate(d).ToString()
            End If

            d = e.Row.Cells("TimeFrame").Value
            If (d >= 0 And e.Row.RowType <> RowType.GroupHeader) Then
                e.Row.Cells("TimeFrame").Text = d.ToString() + " - " + (d + 1).ToString()
            End If
        Catch ex As Exception
            Util.WriteDebugLog("MDHistory Formating Row ERROR " + ex.Message + ex.StackTrace)
        End Try
    End Sub

    Private Sub SendEmail()
        Dim client As SmtpClient
        Dim fromAddr, ToAddr As MailAddress
        Dim mesg As MailMessage
        Dim filename As String

        'hardcoded for the time being need to be replace and provide the registery values-Rahul
        Dim smtpUserID As String
        smtpUserID = ConfigurationManager.AppSettings("FromID") '"tradecompanion@gmail.com"


        Dim mailToAddr As String = ConfigurationManager.AppSettings("ToId")

        Try
            client = New SmtpClient(ConfigurationManager.AppSettings("SmtpServer"))
            client.Port = ConfigurationManager.AppSettings("Port")
            fromAddr = New MailAddress(smtpUserID, EServerDependents.GetEServerSender(), System.Text.Encoding.UTF8)
            ToAddr = New MailAddress(mailToAddr, "Franco Dimuccio", System.Text.Encoding.UTF8)

            client.Credentials = New System.Net.NetworkCredential(smtpUserID, ConfigurationManager.AppSettings("Pass"))
            client.EnableSsl = True

            mesg = New MailMessage(fromAddr, ToAddr)
            mesg.Subject = "Tradecompanion logs from -" + EServerDependents.GetEServerSender() + " on " + System.DateTime.Now
            mesg.SubjectEncoding = System.Text.Encoding.UTF8
            mesg.Body = "Hi, " + vbCrLf + _
                        "  Attached are the logs created by " + SettingsHome.getInstance().ExchangeServer + " Client. " + vbCrLf + _
                        "  Log Details: " + vbCrLf + _
                        "     Creation Date: " + System.DateTime.Now + vbCrLf + _
                        "     User ID: " + EServerDependents.GetEServerSender() + vbCrLf + _
                        "     Login IP: " + EServerDependents.GetEServerSender() + vbCrLf + _
                        vbCrLf + vbCrLf + _
                        "Regards, " + vbCrLf + "-TradeCompanion"

            mesg.BodyEncoding = System.Text.Encoding.UTF8
            Try
                filename = Application.StartupPath + "\\LOG\\BGC.log"
                If (File.Exists(filename)) Then
                    mesg.Attachments.Add(New Attachment(filename))
                End If

                filename = Application.StartupPath + "\\LOG\\Currenex.log"
                If (File.Exists(filename)) Then
                    mesg.Attachments.Add(New Attachment(filename))
                End If

                filename = Application.StartupPath + "\\LOG\\Ariel.log"
                If (File.Exists(filename)) Then
                    mesg.Attachments.Add(New Attachment(filename))
                End If

                filename = Application.StartupPath + "\\LOG\\Espeed.log"
                If (File.Exists(filename)) Then
                    mesg.Attachments.Add(New Attachment(filename))
                End If
                filename = Application.StartupPath + "\\LOG\\DBFX.log"
                If (File.Exists(filename)) Then
                    mesg.Attachments.Add(New Attachment(filename))
                End If
                filename = Application.StartupPath + "\\LOG\\Gain.log"
                If (File.Exists(filename)) Then
                    mesg.Attachments.Add(New Attachment(filename))
                End If
                filename = Application.StartupPath + "\\LOG\\Icap.log"
                If (File.Exists(filename)) Then
                    mesg.Attachments.Add(New Attachment(filename))
                End If
                filename = Application.StartupPath + "\\LOG\\Dukascopy.log" 'Vm_ Fix
                If (File.Exists(filename)) Then
                    mesg.Attachments.Add(New Attachment(filename))
                End If
                filename = Application.StartupPath + "\\LOG\\FxIntegral.log" 'Giri
                If (File.Exists(filename)) Then
                    mesg.Attachments.Add(New Attachment(filename))
                End If
            Catch msgException As Exception
                MessageBox.Show("Please ensure that you have disconnected from the server. Or else the logs cannot be released and sent via email.")
                mesg.Dispose()
                Exit Sub
            End Try
            client.Send(mesg)
            mesg.Dispose()

            MessageBox.Show("Logs has been emailed to " + mailToAddr + " successfully.")

        Catch ex As Exception
            MessageBox.Show("Error Occured: " + ex.Message)
        End Try

    End Sub

    Private Sub NotifyIcon1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotifyIcon1.DoubleClick
        Show()
        WindowState = FormWindowState.Maximized
    End Sub

    Private Sub StrategyPerformanceReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StrategyPerformanceReportToolStripMenuItem.Click
        spr = StrategyPerformanceReport.Instance
        spr.WindowState = FormWindowState.Normal
        spr.SetDesktopLocation(100, 100)
        spr.Show()
    End Sub

    Private Sub TradeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles TradeConnToolStripMenuItem.Click
        Util.WriteDebugLog("--- Connect")
        Connect()
    End Sub

    Private Sub MarketDataToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MarketDataConnToolStripMenuItem.Click
        Select Case SettingsHome.getInstance().ExchangeServer
            Case ExchangeServer.CurrenEx
                ConnectMarketData()
            Case ExchangeServer.Dukascopy 'Vm_ Fix
                ConnectMarketData()
            Case ExchangeServer.FxIntegral 'Giri
                ConnectMarketData()
            Case ExchangeServer.Ariel
                AssignTrderConnection()
            Case ExchangeServer.Espeed
                AssignTrderConnection()
            Case ExchangeServer.DBFX
                AssignTrderConnection()
            Case ExchangeServer.Gain
                AssignTrderConnection()
            Case ExchangeServer.Icap
                AssignTrderConnection()
        End Select
    End Sub

    Public Sub StatechangeHandler()
        Dim del As New NoArgumentDelegate(AddressOf ConnectionsShow)
        BeginInvoke(del)
        'ConnectionsShow()
    End Sub

    Public Sub ConnectionsShow()

        Me.StatusPanel.Controls.Clear()
        Dim obj As Object
        Dim IdCount As Integer = 0
        Dim j As Integer = 0
        Dim s1 As Trader
        Dim iEnum As IDictionaryEnumerator
        iEnum = ConnectHT.GetEnumerator()
        While iEnum.MoveNext()
            obj = iEnum.Value
            s1 = CType(obj, Trader)
            If (s1.IsMarketDataConnection) Then
                Select Case s1.Stat
                    Case Trader.ConnectionStatus.CONNECTED
                        Dim del As New IntArgumentDelegate(AddressOf ShowConnectionStatusMarketData)
                        BeginInvoke(del, ConnectionStatus.CONNECTED)
                    Case Trader.ConnectionStatus.DISCONNECTED
                        Dim del As New IntArgumentDelegate(AddressOf ShowConnectionStatusMarketData)
                        BeginInvoke(del, ConnectionStatus.DISCONNECTED)
                    Case Trader.ConnectionStatus.RECONNECTING
                        Dim del As New IntArgumentDelegate(AddressOf ShowConnectionStatusMarketData)
                        BeginInvoke(del, ConnectionStatus.RECONNECTING)
                End Select
            End If

            If ((Not s1.IsMarketDataConnection) Or (keys.ExchangeServer <> ExchangeServer.CurrenEx And _
                 keys.ExchangeServer <> ExchangeServer.Dukascopy And keys.ExchangeServer <> ExchangeServer.FxIntegral)) Then 'Vm_ fix
                IdCount = IdCount + 1
                Dim label As New Label()
                Dim picbox As New PictureBox()
                label.Text = s1.ConnectionId
                If (keys.ExchangeServer = ExchangeServer.Gain) Then
                    If (s1.Param6 = "1") Then
                        label.Text = label.Text + "(Live)"
                    Else
                        label.Text = label.Text + "(Demo)"
                    End If
                End If
                label.AutoSize = True
                picbox.Size = New System.Drawing.Size(10, 10)
                picbox.Location = New System.Drawing.Point(0, (j * 10) + 2)
                label.Location = New System.Drawing.Point(picbox.Location.X + 20, picbox.Location.Y - 3)
                Select Case s1.Stat
                    Case Trader.ConnectionStatus.CONNECTED
                        picbox.BackColor = Color.Green
                        'picbox.Image = Image.FromFile(Application.StartupPath + "\Heart.gif")
                        'TODO //for FxIntegral
                        If (SettingsHome.getInstance().ExchangeServer = ExchangeServer.CurrenEx Or SettingsHome.getInstance().ExchangeServer = ExchangeServer.Icap) Then
                            mnuChangePassword.Enabled = True
                        End If
                    Case Trader.ConnectionStatus.DISCONNECTED
                        picbox.BackColor = Color.Red
                        'picbox.Image = Nothing
                        mnuChangePassword.Enabled = False
                    Case Trader.ConnectionStatus.RECONNECTING
                        picbox.BackColor = Color.Orange
                        'picbox.Image = Nothing
                        mnuChangePassword.Enabled = False
                End Select
                Me.StatusPanel.Controls.Add(label)
                Me.StatusPanel.Controls.Add(picbox)
                j = j + 2
            End If
        End While
        If IdCount > 3 Then
            Me.StatusPanel.AutoScroll = True
        End If
        Me.StatusPanel.Refresh()
    End Sub

    Private Sub MappingTableToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MappingTableToolStripMenuItem.Click
        ShowMappingTable()
    End Sub

    Private Sub IDMappingTableToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IDMappingTableToolStripMenuItem.Click
        ShowIDMappingTable()
    End Sub

    Private Sub CToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CToolStripMenuItem.Click

    End Sub

    Private Sub UseSymbolMappingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UseSymbolMappingToolStripMenuItem.Click
        keys.UseSymbolMapping = UseSymbolMappingToolStripMenuItem.Checked
        UpdateWatcherSettings()
        'automate.UpdateSettings("symbolmaping")
        If UseSymbolMappingToolStripMenuItem.Checked Then
            Util.WriteDebugLog(" .... Use Symbol Mapping = TRUE")
        Else
            Util.WriteDebugLog(" .... Use Symbol Mapping = FALSE")
        End If
    End Sub

    Private Sub UseDefaultTradesizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UseDefaultTradesizeToolStripMenuItem.Click
        keys.UseDefaultTradeSize = UseDefaultTradesizeToolStripMenuItem.Checked
        UpdateWatcherSettings()
        'automate.UpdateSettings("defaulttradesize")
        If UseDefaultTradesizeToolStripMenuItem.Checked Then
            Util.WriteDebugLog(" .... Use Default Trade Size = TRUE")
        Else
            Util.WriteDebugLog(" .... Use Default Trade Size = FALSE")
        End If
    End Sub


    Private Sub btnSubscribeMarketData_EnabledChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubscribeMarketData.EnabledChanged
        If (btnSubscribeMarketData.Enabled = True) Then
            btnSubscribeMarketData.BackColor = Color.LightSteelBlue
        Else
            btnSubscribeMarketData.BackColor = Color.SlateGray
        End If
    End Sub

    Private Sub btnDisconnectMarketData_EnabledChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisconnectMarketData.EnabledChanged
        If (btnDisconnectMarketData.Enabled = True) Then
            btnDisconnectMarketData.BackColor = Color.LightSteelBlue
        Else
            btnDisconnectMarketData.BackColor = Color.SlateGray
        End If
    End Sub

    Private Sub EnableMarketDataButton()
        btnSubscribeMarketData.Enabled = True
        'If crashtest Then     
        '    crashtest = False 
        '    Form1.GetSingletonOrderform().btnSubscribeMarketData.PerformClick()   
        'End If   
    End Sub

    Private Sub DisableMarketDataButton()
        btnSubscribeMarketData.Enabled = False
    End Sub

    Public Sub EnableMarketDataButtonByDelegate()
        Try


            If (Me.InvokeRequired) Then
                Me.Invoke(New MethodInvoker(AddressOf EnableMarketDataButton))
            Else
                EnableMarketDataButton()
            End If
        Catch ex As Exception
            Util.WriteDebugLog("Error EnableMarketDataButtonByDelegate " + ex.Message)
            'MsgBox(ex.Message)
        End Try

    End Sub

    Public Sub DisableMarketDataButtonByDelegate()
        Try


            If (Me.InvokeRequired) Then
                Me.Invoke(New MethodInvoker(AddressOf DisableMarketDataButton))
            Else
                DisableMarketDataButton()
            End If
        Catch ex As Exception
            Util.WriteDebugLog("Error DisableMarketDataButtonByDelegate " + ex.Message)
            'MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub CloseConnectionStatusMarketDataByDelegate()
        'Dim del As New IntArgumentDelegate(AddressOf ShowConnectionStatusMarketData)
        'BeginInvoke(del, ConnectionStatus.DISCONNECTED)
        Try

            If (Me.InvokeRequired) Then
                Me.Invoke(CType(AddressOf ShowConnectionStatusMarketData, ConnectionStatusParameterDelegate), ConnectionStatus.DISCONNECTED)
            Else
                ShowConnectionStatusMarketData(ConnectionStatus.DISCONNECTED)
            End If
        Catch ex As Exception
            Util.WriteDebugLog("Error CloseConnectionStatusMarketDataByDelegate " + ex.Message)
            'MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub FilterAlertsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FilterAlertsToolStripMenuItem.Click
        If FilterAlertsToolStripMenuItem.Checked Then
            keys.FilterAlerts = True
            Util.WriteDebugLog(" .... Filter Alerts = TRUE")
        Else
            Dim wda As New WarningDuplicateAlerts()
            wda.ShowDialog()
        End If
        'automate.UpdateSettings("filteralerts")
    End Sub

    Public Sub AddInLogWindow(ByVal str As String, ByVal col As Color)
        Try
            Dim param(1) As Object
            param(0) = str
            param(1) = col
            If (Me.InvokeRequired) Then
                Dim del As New ColorArgumentDelegate(AddressOf LogWindow)
                BeginInvoke(del, param)
            Else
                LogWindow(str, col)
            End If
        Catch ex As Exception
            Util.WriteDebugLog("Errow displaying log: " + ex.Message)
            'MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub LogWindow(ByVal str As String, ByVal col As Color)
        str = Format(DateTime.Now, "yyyy/MM/dd-HH:mm:ss.fff") + "  " + str
        Dim start As Integer = RichTextBoxLog.TextLength
        RichTextBoxLog.AppendText(str & Chr(13))
        Dim end1 As Integer = RichTextBoxLog.TextLength

        ' Textbox may transform chars, so (end-start) != text.Length
        RichTextBoxLog.Select(start, end1 - start)

        RichTextBoxLog.SelectionColor = col
        ' could set box.SelectionBackColor, box.SelectionFont too.

        RichTextBoxLog.SelectionLength = 0 ' clear
    End Sub

    Private Sub btnExportLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportLog.Click
        Dim sfd As SaveFileDialog = New SaveFileDialog
        sfd.Filter = "Text File (*.txt)|*.txt"
        If sfd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim stream As FileStream = Nothing
            stream = New FileStream(sfd.FileName, FileMode.Create)
            RichTextBoxLog.SaveFile(stream, RichTextBoxStreamType.PlainText)
        End If
    End Sub

    Private Sub btnClearLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearLog.Click
        RichTextBoxLog.Clear()
    End Sub

    'This is added to clean up auto Alert Directory
    Public Sub RemoveOldAlert()
        Dim fileName As String
        Dim AlertPath As String
        Dim SH As TradeCompanion.SettingsHome = SettingsHome.getInstance()
        If SH.Platform = "TradeStation" Then
            AlertPath = SH.TradeStationMonitorPath
        ElseIf SH.Platform = "NeuroShell" Then
            AlertPath = SH.NeuroshellMonitorPath
        ElseIf SH.Platform = "MetaTrader" Then
            AlertPath = SH.MetaTraderMonitorPath + "\experts\files"
        End If
        For Each fileName In Directory.GetFiles(AlertPath)
            If fileName.Contains(".req") Then
                File.Delete(fileName)
            End If
        Next fileName
        SH = Nothing
    End Sub

    Public Sub Password_Reset_Response(ByVal UserName As String, ByVal UserStatus As Integer, ByVal UserStatusText As String)
        Dim ServerResponse As String = ""
        If (Not UserStatusText Is Nothing) Then
            If (UserStatusText.Trim() <> "") Then
                ServerResponse = "Server response: " + UserStatusText
            End If

        End If
        If UserStatus = 3 Then
            MessageBox.Show("Cannot change password, username or password is not recognised, please try again" + vbCrLf + _
                            "UserName: " + UserName + vbCrLf + vbCrLf + _
                            ServerResponse, "Password change unsuccessfull", MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf UserStatus = 5 Then
            MessageBox.Show("Password changed successfully for username """ + UserName + """" + vbCrLf + _
                            ServerResponse, "Password change unsuccessfull", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf UserStatus = 6 Then
            MessageBox.Show("Unexpected server response received, you password may not be changed" + vbCrLf + _
                            "UserName: " + UserName + vbCrLf + vbCrLf + _
                            ServerResponse, "Password change unsuccessfull", MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf UserStatus = 7 Then 'customised
            PasswordReset.ShowDialog()
            'MessageBox.Show("Change the password as the password expired" + vbCrLf + _
            '"UserName: " + UserName + vbCrLf + UserStatusText, "Password Expired", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            MessageBox.Show("Unexpected response received" + vbCrLf + _
                            "UserName: " + UserName + vbCrLf + vbCrLf + _
                            ServerResponse, "Password change unsuccessfull", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Public Sub ReconnectRequest(ByVal marketID As Boolean)
        If marketID Then
            Dim delConnect As New NoArgumentDelegate(AddressOf ConnectMarketData)
            BeginInvoke(delConnect)
        Else
            Dim delConnect As New NoArgumentDelegate(AddressOf Connect)
            BeginInvoke(delConnect)
        End If
    End Sub

    'Public Sub SetHeartBeatFlag(ByVal SeqNo As Integer)
    '    '//AMIT_ update UI here
    '    'lblHeartBeat.BackColor = Color.LightGray
    '    'Dim th As New Thread(AddressOf HeartBeatIndicator)
    '    'th.Start()
    '    'lblHeartBeat.BackColor = Color.LightGreen
    '    'MessageBox.Show(lblHeartBeat.BackColor.ToString())
    '    'Me.HeartBeatTimer.Enabled = True
    '    'Thread.Sleep(200)
    '    'lblHeartBeat.BackColor = Color.LightGray
    '    'Thread.Sleep(1000)
    '    'HeartBeatReceived = True
    'End Sub
    'Private Sub HeartBeatTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HeartBeatTimer.Tick
    '    'If HeartBeatReceived Then
    '    '    lblHeartBeat.BackColor = Color.LightGreen
    '    '    Me.HeartBeatReceived = False
    '    'Else
    '    '    lblHeartBeat.BackColor = Color.LightGray
    '    'End If
    '    Debug.Print("Heartybeat tomer")
    'End Sub

    Private Sub grdMarketData_RowDoubleClick(ByVal sender As Object, ByVal e As Janus.Windows.GridEX.RowActionEventArgs) Handles grdMarketData.RowDoubleClick
        Dim symbol As String
        symbol = e.Row.Cells(1).Value.ToString()

        Try
            Dim result As Windows.Forms.DialogResult
            Dim mapped As AlertsManager.NewAlert = New AlertsManager.NewAlert
            Dim AlertWindow As ManualAlert = New ManualAlert(symbol)
            result = AlertWindow.ShowDialog
            mapped = AlertWindow.alertdata

            If result = Windows.Forms.DialogResult.OK Then

                'We should update the alert table only if the order is placed properly. 
                'SO moved the following logic to AlertExecution.vb which will get notified from execution.vb on 
                'Successful placement of the order. 
                'Dim i As Integer = 0 
                'mapped.status = result 
                'mapped.timestamp = "simulated" 
                'ah.AddAlert(mapped) 

                If (Not (mapped Is Nothing)) Then
                    mapped.timestamp = "simulated"
                    watcher_NewAlert(mapped)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Util.WriteDebugLog("Place order ERROR:" + ex.Message + ex.Source)
            Util.WriteDebugLog("Place order ERROR:" + ex.StackTrace)
        End Try
    End Sub

    Private Sub mnuChangePassword_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuChangePassword.Click
        PasswordReset.ShowDialog()
    End Sub

    Private Sub OpenPositionGridEX_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles OpenPositionGridEX.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            If (OpenPositionGridEX.GetValue("Symbol") <> "") Then
                TradeHistory.ShowOrders(OpenPositionGridEX.GetValue("Symbol"), OpenPositionGridEX.GetValue("AccountID"))
            End If
        End If
    End Sub

    Public Sub showIndividualSystem()
        Try
            dsIndSys = ah.LoadIndividualSystem()
            dsIndSys.Tables(0).Columns.Add("CurrentPrice")
            dsIndSys.Tables(0).Columns.Add("TSOpenPosition")
            grdIndSys.DataSource = dsIndSys
            grdIndSys.SetDataBinding(dsIndSys, "IndividualSystem")
            grdIndSys.RetrieveStructure()
            grdIndSys.Tables(0).Columns("EntryDateTime").FormatString = "dd/MM/yyyy hh:mm:ss tt"
            grdIndSys.Tables(0).Caption = "Individual System"
            grdIndSys.TableHeaders = InheritableBoolean.True
            grdIndSys.Tables(0).Columns("ID").Visible = False
            grdIndSys.Tables(0).Columns("CurrentPrice").Caption = "CurrentPrice"
            grdIndSys.Tables(0).Columns("CurrentPrice").Position = 4
        Catch ex As Exception
            Console.WriteLine(ex.Message + ex.StackTrace)
        End Try
    End Sub

    Private Sub UpdateDSSpotPosition(ByVal r As DataRow) Handles indSys.OpenPosIndSys
        Try
            'Console.WriteLine(Date.Now.ToString() + " Trade Status Update")
            dsIndSys.Tables(0).Columns("ID").AutoIncrement = True
            Dim filter As String = ""
            filter = "Symbol= '" + r.Item("Symbol").ToString() + "' And SystemName = '" + r.Item("SystemName").ToString() + "' And SystemNumber = '" + r.Item("SystemNumber") + "' AND SenderId = '" + r.Item("SenderID") + "'"
            Dim dr() As DataRow = dsIndSys.Tables(0).Select(filter)
            If (dr.Length > 0) Then 'update the row
                UpdateRow(r, dr(0))
            Else ' insert the row
                AddRow(r)
            End If
        Catch ex As Exception
            Util.WriteDebugLog(ex.Message + ex.StackTrace)
        End Try
    End Sub

    Private Sub UpdateRow(ByVal r As DataRow, ByVal dr As DataRow)
        If (Not (r.IsNull("OpenPosition"))) Then
            dr("OpenPosition") = r("OpenPosition")
        End If

        dr("OpenPositionPL") = r("OpenPositionPL")
        dr("RealizedPL") = r("RealizedPL")
        dr("TotalPL") = r("TotalPL")

        If (Not (r.IsNull("TsOpenPosition"))) Then
            If (r("TsOpenPosition") > 0) Then
                dr("TSOpenPosition") = "Long" + r("TsOpenPosition")
            ElseIf (r("TsOpenPosition") < 0) Then
                dr("TSOpenPosition") = "Short" + r("TsOpenPosition")
            Else
                dr("TSOpenPosition") = "flat"
            End If
        End If

        If (r("OpenPosition") = 0) Then
            dr("OrderID") = ""
            dr("CurrentPrice") = 0
            dr("EntryPrice") = 0
            dr("EntryDateTime") = ""
        Else
            If (Not (r.IsNull("EntryDateTime"))) Then
                dr("EntryDateTime") = r("EntryDateTime")
            End If
            If (Not (r.IsNull("OrderID"))) Then
                dr("OrderID") = r("OrderID")
            End If
            If (Not (r.IsNull("EntryPrice"))) Then
                dr("EntryPrice") = r("EntryPrice")
            End If
            If (r("OpenPosition") < 0) Then
                dr("CurrentPrice") = ah.GetAskPriceBySymbol(r("Symbol"))
            Else
                dr("CurrentPrice") = ah.GetBidPriceBySymbol(r("Symbol"))
            End If
        End If
    End Sub

    Private Sub AddRow(ByVal r As DataRow)
        Dim dr As DataRow = dsIndSys.Tables(0).NewRow()
        dr("Symbol") = r("Symbol")
        dr("SystemName") = r("SystemName")
        dr("SystemNumber") = r("SystemNumber")
        dr("SenderId") = r("SenderId")
        'dr("TSOpenPosition") = r("TsOpenPosition")
        UpdateRow(r, dr)
        dsIndSys.Tables(0).Rows.Add(dr)
    End Sub

    Private Sub grdIndSys_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles grdIndSys.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            If (grdIndSys.GetValue("Symbol") <> "") Then
                TradeHistory.ShowOrders(grdIndSys.GetValue("Symbol"), grdIndSys.GetValue("SenderId"), grdIndSys.GetValue("SystemName"), grdIndSys.GetValue("SystemNumber"))
            End If
        End If
    End Sub

    Private Sub RestoreToolStrip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RestoreToolStrip.Click
        Show()
        WindowState = FormWindowState.Maximized
    End Sub

    Private Sub MinimizeToolStrip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MinimizeToolStrip.Click
        Show()
        WindowState = FormWindowState.Minimized
    End Sub

    Private Sub ExitToolStrip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStrip.Click
        Me.Close()
    End Sub
End Class

Friend Class ThreadExceptionHandler
    '''
    ''' Handles the thread exception.
    '''
    Public Sub Application_ThreadException(ByVal sender As System.Object, ByVal e As ThreadExceptionEventArgs)
        Try
            ' Just send an email
            IntimateUser(e.Exception)

            'Exit the program if the user clicks Abort.
            'Dim result As DialogResult = ShowThreadExceptionDialog(e.Exception)
            'If (result = DialogResult.Abort) Then
            'Application.Exit()
            'End If
        Catch
            ' Fatal error, terminate program
            Try
                MessageBox.Show("Fatal Error", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Finally
                Application.Exit()
            End Try
        End Try
    End Sub

    '
    ' Intimate User with an email when there seems to be an Unhandled exception
    '
    Public Sub IntimateUser(ByVal ex As Exception)
        'Send Email to user
        Util.WriteDebugLog("Unhandled Exception")
        Util.WriteDebugLog(ex.StackTrace)

        'Dim client As SmtpClient
        'Dim fromAddr, ToAddr As MailAddress
        'Dim mesg As MailMessage

        ''hardcoded for the time being need to be replace and provide the registery values
        'Dim smtpServer As String = "88.208.220.198"
        'Dim smtpUserID As String = "logs@tradercompanion.co.uk"
        'Dim smtpPasswd As String = "shusiloo"

        ''To be replaced by the actual mail id
        'Dim mailToAddr As String = SettingsHome.getInstance().emailID
        'If (mailToAddr.Trim() = "") Then
        '    Util.WriteDebugLog("Invalid email id - Unable to send error stacktrace")
        'End If

        'Try
        '    client = New SmtpClient(smtpServer)
        '    fromAddr = New MailAddress("logs@tradercompanion.co.uk", EServerDependents.GetEServerSender(), System.Text.Encoding.UTF8)
        '    ToAddr = New MailAddress(mailToAddr, "", System.Text.Encoding.UTF8)

        '    client.Credentials = New System.Net.NetworkCredential(smtpUserID, smtpPasswd)

        '    mesg = New MailMessage(fromAddr, ToAddr)
        '    mesg.Subject = "[BGC TC] BreakDown -" + EServerDependents.GetEServerSender() + " on " + System.DateTime.Now
        '    mesg.SubjectEncoding = System.Text.Encoding.UTF8
        '    mesg.Body = "Hi, " + vbCrLf + _
        '                "  There seems to be an unhandled exception. Please check if Trade Companion is running fine." + vbCrLf + _
        '                "  Error Details: " + vbCrLf + _
        '                ex.StackTrace() + vbCrLf + _
        '                vbCrLf + vbCrLf + _
        '                "Regards, " + vbCrLf + " - BGC"

        '    mesg.BodyEncoding = System.Text.Encoding.UTF8
        '    client.Send(mesg)
        '    mesg.Dispose()
        'Catch e As Exception
        '    MessageBox.Show("Error Occured: " + e.Message)
        'End Try
    End Sub

    '''
    ''' Creates and displays the error message. Currently not in use
    '''
    Private Function ShowThreadExceptionDialog( _
        ByVal ex As Exception) As DialogResult

        Dim errorMessage As String = _
            "Unhandled Exception:" _
            & vbCrLf & vbCrLf & _
            ex.Message & vbCrLf & vbCrLf & _
            ex.GetType().ToString() & vbCrLf & vbCrLf & _
            "Stack Trace:" & vbCrLf & _
            ex.StackTrace

        Return MessageBox.Show(errorMessage, _
            "Application Error", _
            MessageBoxButtons.AbortRetryIgnore, _
            MessageBoxIcon.Stop)
    End Function

End Class ' ThreadExceptionHandler

