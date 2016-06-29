Imports System.Globalization
Public Class StrategyPerformanceReport
    Dim ds As DataSet
    Dim dsTradeSummary As New DataSet

    Public Shared _Instance As StrategyPerformanceReport = Nothing
    Private WithEvents plcal As New PLCal
    Dim dsSpotPosition As New DataSet
    Private Delegate Sub DateTimeArgumentDelegate()
    Public Delegate Sub spr_Delegate(ByVal alertData As AlertsManager.NewAlert)
    Dim del_watcher As New spr_Delegate(AddressOf asyncWatcherCall)

    Private Sub grdPLWindow_FormattingRow(ByVal sender As System.Object, ByVal e As Janus.Windows.GridEX.RowLoadEventArgs) Handles grdPLWindow.FormattingRow
        Try
            Dim d As Double = e.Row.Cells("DateID").Value
            e.Row.Cells("DateID").Text = DateTime.FromOADate(d).ToString

        Catch ex As Exception
            Util.WriteDebugLog(" Orders Formating Row ERROR " + ex.Message)
        End Try
    End Sub

    Public Sub ShowPLWindow()
        Try
            Dim ah As AlertsHome = New AlertsHome
            Dim symbol As String = ""
            Dim accountid As String = ""
           
            If (cmbSymbol.SelectedValue.ToString() <> "[ALL]") Then
                symbol = cmbSymbol.SelectedValue.ToString()
            End If
            If (cmbID.SelectedValue.ToString() <> "[ALL]") Then
                accountid = cmbID.SelectedValue.ToString()
            End If
            ds = ah.GetPLTrade(symbol, accountid)

            Dim actionColumn As DataColumn = New DataColumn
            With actionColumn
                .DataType = System.Type.GetType("System.String")
                .ColumnName = "Action"
                .Expression = "IIF(Actions=1,'BUY', 'SELL')"
            End With

            ds.Tables("PLTrade").Columns.Add(actionColumn)

            grdPLWindow.DataSource = ds
            grdPLWindow.SetDataBinding(ds, "PLTrade")
            grdPLWindow.RetrieveStructure()
            grdPLWindow.Tables(0).Columns("RowID").Visible = False
            grdPLWindow.Tables(0).Caption = "P/L Window"
            grdPLWindow.TableHeaders = Janus.Windows.GridEX.InheritableBoolean.True
            grdPLWindow.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False
            grdPLWindow.FocusStyle = Janus.Windows.GridEX.FocusStyle.None

            grdPLWindow.Tables(0).Columns("Amount").FormatString = "#,#"
            grdPLWindow.Tables(0).Columns("DateID").Position = 0
            grdPLWindow.Tables(0).Columns("Actions").Visible = False
            grdPLWindow.Tables(0).Columns("Pips").Visible = False
            grdPLWindow.Tables(0).Columns("Remaining").Visible = False
            grdPLWindow.Tables(0).Columns("ExecOrderId").Visible = False
            grdPLWindow.Tables(0).Columns("ServerDateTime").Visible = False
            grdPLWindow.Tables(0).Columns("PipsBaseCurrency").Visible = False
            grdPLWindow.Tables(0).Columns("NetAmount").Visible = False


            grdPLWindow.ColumnAutoResize = True
            grdPLWindow.AutoSizeColumns()
            grdPLWindow.Refresh()

        Catch ex As Exception
            Util.WriteDebugLog(" .... Show Alerts ERROR " + ex.Message)
        Finally
        End Try

    End Sub

    Private Sub btnGetPL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetPL.Click
        grdPeriodicalReturns.DataSource = Nothing
        grdPLWindow.DataSource = Nothing

        ShowPLWindow()

        Dim netProfit As Decimal

        lblShowNetProfit.Text = ""
        lblShowAvgTradeNetProfit.Text = ""
        lblShowTotalTrades.Text = ""
        cmbReportType.SelectedIndex = -1
        Dim count As Integer = ds.Tables(0).Rows.Count
        If (count > 0) Then
            Try
                Try
                    netProfit = GetNetProfit() 'CType(ds.Tables(0).Compute("Sum(Pips)", ""), Decimal) 'BUY
                Catch ex As Exception
                    netProfit = 0
                End Try


                lblShowNetProfit.Text = netProfit
                lblShowAvgTradeNetProfit.Text = Decimal.Round(netProfit / count, 4)
                lblShowTotalTrades.Text = ds.Tables(0).Rows.Count

                If (netProfit < 0) Then
                    lblShowNetProfit.ForeColor = Color.Red
                Else
                    lblShowNetProfit.ForeColor = Color.Green
                End If

                If (netProfit / count < 0) Then
                    lblShowAvgTradeNetProfit.ForeColor = Color.Red
                Else
                    lblShowAvgTradeNetProfit.ForeColor = Color.Green
                End If
                cmbReportType.SelectedIndex = 0
            Catch ex As Exception
                Util.WriteDebugLog(" .... Show PL Summary ERROR " + ex.Message)
            End Try
        End If

    End Sub

    Private Sub StrategyPerformanceReport_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        _Instance = Nothing
    End Sub

    Private Sub StrategyPerformanceReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim ah As AlertsHome = New AlertsHome

        Dim dsLoad As DataSet = ah.GetServerSymbols()
        dsLoad.Tables(0).Rows.Add("[ALL]")
        Dim dv As DataView = dsLoad.Tables(0).DefaultView
        dv.Sort = "TradeSymbol"

        cmbSymbol.DataSource = dv
        cmbSymbol.DisplayMember = "TradeSymbol"
        cmbSymbol.ValueMember = "TradeSymbol"

        dsLoad = ah.GetServerIDs()
        dsLoad.Tables(0).Rows.Add("[ALL]")
        dv = dsLoad.Tables(0).DefaultView
        dv.Sort = "SenderID"

        cmbID.DataSource = dv
        cmbID.ValueMember = "SenderID"
        cmbID.DisplayMember = "SenderID"


        dsTradeSummary.Tables.Add("TradeSummary")
        dsTradeSummary.Tables(0).Columns.Add("Period")
        dsTradeSummary.Tables(0).Columns.Add("NetProfit")
        dsTradeSummary.Tables(0).Columns.Add("Trades")

        cmbSymbol.DropDownStyle = ComboBoxStyle.DropDownList
        cmbID.DropDownStyle = ComboBoxStyle.DropDownList
        cmbReportType.DropDownStyle = ComboBoxStyle.DropDownList

    End Sub

    Private Sub cmbReportType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbReportType.SelectedIndexChanged

        Try
            Dim startDate As DateTime
            Dim endDate As DateTime
            Dim nextDate As DateTime
            If (ds.Tables(0).Rows.Count > 0) Then
                startDate = DateTime.FromOADate(ds.Tables(0).Rows(0)("ServerDateTime").ToString())
                endDate = DateTime.FromOADate(ds.Tables(0).Rows(ds.Tables(0).Rows.Count - 1)("ServerDateTime").ToString())
                Dim cal As Calendar
                cal = CultureInfo.InvariantCulture.Calendar
                dsTradeSummary.Tables(0).Rows.Clear()
                Select Case cmbReportType.SelectedIndex
                    Case 0 'Annualy
                        While (startDate.Year <= endDate.Year)
                            nextDate = "31 DECEMBER " + startDate.Year.ToString() + " 23:59:59"

                            dsTradeSummary.Tables(0).Rows.Add(GetRow(startDate, nextDate))
                            startDate = nextDate.AddSeconds(1)
                        End While

                    Case 2 'Monthly
                        While (startDate <= endDate)
                            Dim st As String
                            st = cal.GetDaysInMonth(startDate.Year, startDate.Month).ToString() + " " + MonthName(startDate.Month) + " " + startDate.Year.ToString() + " 23:59:59"
                            nextDate = st

                            dsTradeSummary.Tables(0).Rows.Add(GetRow(startDate, nextDate))
                            startDate = nextDate.AddSeconds(1)
                        End While

                    Case 3 'Weekly
                        While (startDate <= endDate)
                            Dim addDays As Integer
                            addDays = 7 - cal.GetDayOfWeek(startDate.Date)
                            nextDate = startDate.Date.Date.AddDays(addDays) + " 23:59:59"

                            dsTradeSummary.Tables(0).Rows.Add(GetRow(startDate, nextDate))
                            startDate = nextDate.AddSeconds(1)
                        End While

                    Case 1 'Daily
                        While (startDate <= endDate)
                            Dim st As String
                            st = startDate.Date.Date + " 23:59:59"
                            nextDate = st

                            dsTradeSummary.Tables(0).Rows.Add(GetRow(startDate, nextDate))
                            startDate = nextDate.AddSeconds(1)
                        End While

                End Select
                ShowPeriodicalReturns()
            End If
        Catch ex As Exception
            Util.WriteDebugLog(" .... Show Periodical Returns ERROR " + ex.Message)
        End Try
    End Sub

    Public Sub ShowPeriodicalReturns()
        Try
            grdPeriodicalReturns.DataSource = dsTradeSummary
            grdPeriodicalReturns.SetDataBinding(dsTradeSummary, "TradeSummary")
            grdPeriodicalReturns.RetrieveStructure()
            grdPeriodicalReturns.Tables(0).Caption = "Periodical Returns"
            grdPeriodicalReturns.TableHeaders = Janus.Windows.GridEX.InheritableBoolean.True
            grdPeriodicalReturns.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False

            grdPeriodicalReturns.ColumnAutoResize = True

            grdPeriodicalReturns.Refresh()

        Catch ex As Exception
            Util.WriteDebugLog(" .... Show Alerts ERROR " + ex.Message)
        Finally
        End Try

    End Sub

    Private Function GetRow(ByVal startDate As DateTime, ByVal nextDate As DateTime) As DataRow
        Dim sumBUY As Decimal
        Dim filter As String
        Dim count As Integer

        filter = "ServerDateTime >= " + startDate.ToOADate().ToString() + " and ServerDateTime <= " + nextDate.ToOADate().ToString()
        Try
            sumBUY = GetNetProfit(filter) 'CType(ds.Tables(0).Compute("Sum(PipsBaseCurrency)", filter), Decimal) 'BUY
        Catch ex As Exception
            sumBUY = 0
        End Try
       
        Dim dv As DataView = ds.Tables(0).DefaultView
        dv = ds.Tables(0).DefaultView
        dv.RowFilter = filter
        count = count + dv.Count

        Dim dr As DataRow = dsTradeSummary.Tables(0).NewRow()
        dr("Period") = startDate.ToString() + " - " + nextDate.ToString()
        dr("NetProfit") = sumBUY.ToString()
        dr("Trades") = count.ToString()

        Return dr
    End Function


    Private Sub TabPLWindow_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabPLWindow.SelectionChanged

        If (TabPLWindow.SelectedIndex = 0) Then
            cmbSymbol.Visible = False
            cmbID.Visible = False
            btnGetPL.Visible = False
            lblDate.Visible = True
            lblShowDate.Visible = True
            lblAccount.Visible = False
            lblSymbol.Visible = False
            clearSpotposBtn.Visible = True
        Else
            clearSpotposBtn.Visible = False
            cmbSymbol.Visible = True
            cmbID.Visible = True
            btnGetPL.Visible = True
            lblDate.Visible = False
            lblShowDate.Visible = False
            lblAccount.Visible = True
            lblSymbol.Visible = True
        End If
    End Sub

    Delegate Sub DataRowParameterDelegate(ByVal dr As DataRow)

    Private Sub trader_PLCalculation(ByVal dr As DataRow) Handles plcal.PLCalculation
        Try
            Try
                'Dim upda As New UpdateUI
                If (Me.InvokeRequired) Then
                    Me.Invoke(CType(AddressOf UpdateDSSpotPosition, DataRowParameterDelegate), dr)
                Else
                    UpdateDSSpotPosition(dr)
                    'upda.UpDateGird(dr, dsSpotPosition)
                End If
            Catch ex As Exception
                Util.WriteDebugLog(" .... trader_PLCalculation - ERROR " + ex.Message)
            End Try

        Catch ex As Exception
            Util.WriteDebugLog(" .... trader_PLCalculation - ERROR " + ex.Message)
        End Try
    End Sub

    Private Sub UpdateDSSpotPosition(ByVal r As DataRow)
        dsSpotPosition.Tables(0).Columns("RowID").AutoIncrement = True
        Dim filter As String = ""
        filter = "Symbol= '" + r.Item("Symbol").ToString() + "' And AccountId = '" + r.Item("AccountID").ToString() + "'"
        Dim dr() As DataRow = dsSpotPosition.Tables(0).Select(filter)
        If (dr.Length > 0) Then 'update the row
            UpdateRow(r, dr(0))
        Else ' insert the row
            filter = "Symbol= '[NET]' And AccountId = '" + r.Item("AccountID").ToString() + "'"
            dr = dsSpotPosition.Tables(0).Select(filter)
            If (dr.Length > 0) Then
                Dim index As Integer = dsSpotPosition.Tables(0).Rows.IndexOf(dr(0))
                AddRow(r, index)
            Else
                AddRow(r, -1)
            End If
        End If

        filter = "Symbol= '[NET]' And AccountId = '" + r.Item("AccountID").ToString() + "'"
        dr = dsSpotPosition.Tables(0).Select(filter)
        If (dr.Length > 0) Then
            UpdateSumRow(r.Item("AccountID"), dr(0))
        Else
            AddsumRow(r.Item("AccountID"))
        End If
    End Sub

    Private Sub UpdateRow(ByVal r As DataRow, ByVal dr As DataRow)
        If (Not (r.IsNull("Realized"))) Then
            dr("Realized") = r("Realized")
        End If

        If (Not (r.IsNull("Unrealized"))) Then
            dr("Unrealized") = r("Unrealized")
        End If

        If (Not (r.IsNull("RealizedBaseCurrency"))) Then
            dr("RealizedBaseCurrency") = r("RealizedBaseCurrency")
        End If

        If (Not (r.IsNull("UnRealizedBaseCurrency"))) Then
            dr("UnRealizedBaseCurrency") = r("UnRealizedBaseCurrency")
        End If

        If (Not (r.IsNull("AccountID"))) Then
            dr("AccountID") = r("AccountID")
        End If

        If (Not (r.IsNull("NetCC1"))) Then
            dr("NetCC1") = r("NetCC1")
        End If

        If (Not (r.IsNull("NetCC2"))) Then
            dr("NetCC2") = r("NetCC2")
        End If

        If (Not (r.IsNull("AverageBuyRate"))) Then
            dr("AverageBuyRate") = r("AverageBuyRate")
        End If

        If (Not (r.IsNull("AverageSellRate"))) Then
            dr("AverageSellRate") = r("AverageSellRate")
        End If

        If (Not (r.IsNull("AllInRate"))) Then
            dr("AllInRate") = r("AllInRate")
        End If

        If (Not (r.IsNull("OpenRate"))) Then
            dr("OpenRate") = r("OpenRate")
        End If

        If (Not (r.IsNull("MktRate"))) Then
            dr("MktRate") = r("MktRate")
        End If

    End Sub

    Private Sub AddRow(ByVal r As DataRow, ByVal index As Integer)
        Dim dr As DataRow = dsSpotPosition.Tables(0).NewRow()
        Dim secondcurrency, basecurrency, baseSymbol As String
        basecurrency = SettingsHome.getInstance().BaseCurrency
        If (r.IsNull("NetCC1")) Then
            Exit Sub
        End If

        If (r.Item("Symbol").ToString().Contains(basecurrency)) Then
            baseSymbol = r.Item("Symbol").ToString()
        Else
            secondcurrency = EServerDependents.GetSecondCurrency(r.Item("Symbol").ToString())
            baseSymbol = EServerDependents.GetCombinedCurrency(basecurrency, secondcurrency)
        End If
        If (Not (r.IsNull("Symbol"))) Then
            dr("Symbol") = r("Symbol")
        End If
        If (Not (r.IsNull("Realized"))) Then
            dr("Realized") = r("Realized")
        End If
        If (Not (r.IsNull("Unrealized"))) Then
            dr("Unrealized") = r("Unrealized")
        End If
        If (Not (r.IsNull("AccountID"))) Then
            dr("AccountID") = r("AccountID")
        End If

        If (Not (r.IsNull("RealizedBaseCurrency"))) Then
            dr("RealizedBaseCurrency") = r("RealizedBaseCurrency")
        End If

        If (Not (r.IsNull("UnRealizedBaseCurrency"))) Then
            dr("UnRealizedBaseCurrency") = r("UnRealizedBaseCurrency")
        End If

        'If (Not (r.IsNull("BaseSymbol"))) Then
        dr("BaseSymbol") = baseSymbol
        'End If

        If (Not (r.IsNull("NetCC1"))) Then
            dr("NetCC1") = r("NetCC1")
        End If

        If (Not (r.IsNull("NetCC2"))) Then
            dr("NetCC2") = r("NetCC2")
        End If

        If (Not (r.IsNull("AverageBuyRate"))) Then
            dr("AverageBuyRate") = r("AverageBuyRate")
        End If

        If (Not (r.IsNull("AverageSellRate"))) Then
            dr("AverageSellRate") = r("AverageSellRate")
        End If

        If (Not (r.IsNull("AllInRate"))) Then
            dr("AllInRate") = r("AllInRate")
        End If

        If (Not (r.IsNull("OpenRate"))) Then
            dr("OpenRate") = r("OpenRate")
        End If

        If (Not (r.IsNull("MktRate"))) Then
            dr("MktRate") = r("MktRate")
        End If
        If (index = -1) Then
            dsSpotPosition.Tables(0).Rows.Add(dr)
        Else
            dsSpotPosition.Tables(0).Rows.InsertAt(dr, index)
        End If
    End Sub

    Private Sub ShowSpotPosition()
        SyncLock Me
            Try
                Dim ah As AlertsHome = New AlertsHome
                lblShowDate.Text = Now.Date.Date.ToString().Split(" ")(0)
                dsSpotPosition = ah.loadPLCal()
                grdSpotPosition.DataSource = dsSpotPosition
                grdSpotPosition.Refresh()
                grdSpotPosition.SetDataBinding(dsSpotPosition, "PLCal")
                grdSpotPosition.RetrieveStructure()
                grdSpotPosition.Tables(0).Columns("RowID").Visible = False
                grdSpotPosition.Tables(0).Caption = "Spot Position"
                grdSpotPosition.TableHeaders = Janus.Windows.GridEX.InheritableBoolean.True
                grdSpotPosition.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False
                grdSpotPosition.Tables(0).Groups.Add("AccountID")
                grdSpotPosition.Tables(0).Columns("RealizedBaseCurrency").Caption = "Realized " + SettingsHome.getInstance().BaseCurrency
                grdSpotPosition.Tables(0).Columns("UnRealizedBaseCurrency").Caption = "UnRealized " + SettingsHome.getInstance().BaseCurrency
                grdSpotPosition.Tables(0).Columns("AccountID").Visible = False
                grdSpotPosition.Tables(0).Columns("BaseSymbol").Visible = False
                grdSpotPosition.Tables(0).Columns("NetCC1").Position = 2
                grdSpotPosition.Tables(0).Columns("NetCC1").FormatString = "#,#"
                grdSpotPosition.Tables(0).Columns("NetCC2").Position = 3
                grdSpotPosition.Tables(0).Columns("NetCC2").FormatString = "#,#"
                grdSpotPosition.Tables(0).Columns("AverageBuyRate").Position = 4
                grdSpotPosition.Tables(0).Columns("AverageSellRate").Position = 5
                grdSpotPosition.Tables(0).Columns("AllInRate").Position = 6
                grdSpotPosition.Tables(0).Columns("OpenRate").Position = 7
                grdSpotPosition.Tables(0).Columns("NetCC1").Caption = "NetCCY1 Amt"
                grdSpotPosition.Tables(0).Columns("NetCC2").Caption = "NetCCY2 Amt"
                grdSpotPosition.Tables(0).Columns("AverageBuyRate").Caption = "Avg Buy Rate"
                grdSpotPosition.Tables(0).Columns("AverageSellRate").Caption = "Avg Sell Rate"
                grdSpotPosition.Tables(0).Columns.Add(New Janus.Windows.GridEX.GridEXColumn("Reset"))
                grdSpotPosition.Tables(0).Columns("Reset").Caption = "Set To Zero"
                'grdSpotPosition.Tables(0).Columns("Reset").ButtonStyle = Janus.Windows.GridEX.ButtonStyle.ButtonCell
                grdSpotPosition.Tables(0).Columns("Reset").ButtonText = "Set to Zero"
                grdSpotPosition.Tables(0).Columns("Reset").TextAlignment = Janus.Windows.GridEX.TextAlignment.Center
                grdSpotPosition.Tables(0).Columns("Reset").ButtonDisplayMode = Janus.Windows.GridEX.CellButtonDisplayMode.Always
                grdSpotPosition.Tables(0).Columns("Reset").BoundMode = Janus.Windows.GridEX.ColumnBoundMode.Bound
                grdSpotPosition.Tables(0).Columns("Reset").ButtonStyle = Janus.Windows.GridEX.ButtonStyle.TextButton
                
               

                grdSpotPosition.Tables(0).RowHeight = 20
                grdSpotPosition.Tables(0).Columns("Symbol").Width = 70
                grdSpotPosition.Tables(0).Columns("NetCC1").Width = 80
                grdSpotPosition.Tables(0).Columns("NetCC2").Width = 80
                grdSpotPosition.Tables(0).Columns("Reset").Width = 70
                grdSpotPosition.Tables(0).Columns("AverageBuyRate").Width = 80
                grdSpotPosition.Tables(0).Columns("AverageSellRate").Width = 80
                grdSpotPosition.Tables(0).Columns("Realized").Width = 70
                grdSpotPosition.Tables(0).Columns("Unrealized").Width = 70
                grdSpotPosition.Tables(0).Columns("RealizedBaseCurrency").Width = 80
                grdSpotPosition.Tables(0).Columns("UnrealizedBaseCurrency").Width = 90
                grdSpotPosition.Tables(0).Columns("OpenRate").Width = 80
                grdSpotPosition.Tables(0).Columns("AllInRate").Width = 70
                grdSpotPosition.Tables(0).Columns("MktRate").Width = 70
                'grdSpotPosition.Tables(0).Columns("Symbol").SortIndicator = Janus.Windows.GridEX.SortIndicator.Descending

                Dim dsAccountID As DataSet = ah.GetServerIDs()
                For Each dr As DataRow In dsAccountID.Tables(0).Rows
                    AddsumRow(dr("SenderID"))
                Next
            Catch ex As Exception
                Util.WriteDebugLog("ShowSpotPosition ..." + ex.Message + ex.StackTrace)
                MessageBox.Show("ShowSpotPosition ..." + ex.Message + ex.Source + ex.StackTrace)
            End Try
        End SyncLock
    End Sub

    Private Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ShowSpotPosition()
        TabPLWindow.SelectedIndex = 0
    End Sub

    Public Shared Function Instance() As StrategyPerformanceReport
        If _Instance Is Nothing OrElse _Instance.IsDisposed = True Then
            _Instance = New StrategyPerformanceReport
        End If
        _Instance.BringToFront()
        Return _Instance
    End Function

    Private Sub grdSpotPosition_FormattingRow(ByVal sender As System.Object, ByVal e As Janus.Windows.GridEX.RowLoadEventArgs) Handles grdSpotPosition.FormattingRow
        Try

            Dim pips As Double
            If (Not IsDBNull(e.Row.Cells("Realized").Value)) Then
                pips = CDbl(e.Row.Cells("Realized").Value)
                Dim f1 As New Janus.Windows.GridEX.GridEXFormatStyle()
                If (pips > 0) Then
                    f1.ForeColor = Color.Green
                    e.Row.Cells("Realized").FormatStyle = f1
                ElseIf (pips < 0) Then
                    f1.ForeColor = Color.Red
                    e.Row.Cells("Realized").FormatStyle = f1
                End If
            End If

            If (Not IsDBNull(e.Row.Cells("UnRealized").Value)) Then
                pips = CDbl(e.Row.Cells("UnRealized").Value)
                Dim f2 As New Janus.Windows.GridEX.GridEXFormatStyle()
                If (pips > 0) Then
                    f2.ForeColor = Color.Green
                    e.Row.Cells("UnRealized").FormatStyle = f2
                ElseIf (pips < 0) Then
                    f2.ForeColor = Color.Red
                    e.Row.Cells("UnRealized").FormatStyle = f2
                End If
            End If

            If (Not IsDBNull(e.Row.Cells("RealizedBaseCurrency").Value)) Then
                pips = CDbl(e.Row.Cells("RealizedBaseCurrency").Value)
                Dim f3 As New Janus.Windows.GridEX.GridEXFormatStyle()
                'for the net profit row
                If ("[NET]" = e.Row.Cells("Symbol").Value) Then f3.FontBold = Janus.Windows.GridEX.TriState.True
                If (pips > 0) Then
                    f3.ForeColor = Color.Green
                    e.Row.Cells("RealizedBaseCurrency").FormatStyle = f3
                ElseIf (pips < 0) Then
                    f3.ForeColor = Color.Red
                    e.Row.Cells("RealizedBaseCurrency").FormatStyle = f3
                End If
            End If

            If (Not IsDBNull(e.Row.Cells("UnRealizedBaseCurrency").Value)) Then
                pips = CDbl(e.Row.Cells("UnRealizedBaseCurrency").Value)
                Dim f4 As New Janus.Windows.GridEX.GridEXFormatStyle()
                If (pips > 0) Then
                    f4.ForeColor = Color.Green
                    e.Row.Cells("UnRealizedBaseCurrency").FormatStyle = f4
                ElseIf (pips < 0) Then
                    f4.ForeColor = Color.Red
                    e.Row.Cells("UnRealizedBaseCurrency").FormatStyle = f4
                End If
            End If

            If (Not IsDBNull(e.Row.Cells("NetCC1").Value)) Then
                pips = CDbl(e.Row.Cells("NetCC1").Value)
                Dim f5 As New Janus.Windows.GridEX.GridEXFormatStyle()
                If (pips < 0) Then
                    f5.ForeColor = Color.Red
                    e.Row.Cells("NetCC1").FormatStyle = f5
                End If
            End If
            If (Not IsDBNull(e.Row.Cells("NetCC2").Value)) Then
                pips = CDbl(e.Row.Cells("NetCC2").Value)
                Dim f6 As New Janus.Windows.GridEX.GridEXFormatStyle()
                If (pips < 0) Then
                    f6.ForeColor = Color.Red
                    e.Row.Cells("NetCC2").FormatStyle = f6
                End If
            End If
            If ("[NET]" = e.Row.Cells("Symbol").Value) Then
                Dim f7 As New Janus.Windows.GridEX.GridEXFormatStyle
                f7.FontBold = Janus.Windows.GridEX.TriState.True
                e.Row.Cells("Symbol").FormatStyle = f7
            End If
        Catch ex As Exception
            Util.WriteDebugLog(" SpotPosition Formating Row ERROR " + ex.Message)
        End Try
    End Sub

    Private Function GetNetProfit(Optional ByVal filter As String = Nothing) As Decimal
        Dim ah As New AlertsHome()
        Dim netprofit As Decimal = 0
        Dim net As Decimal = 0
        Dim dsMarketData As DataSet = Form1.GetSingletonOrderform().dsMarkeData.Copy()  'ah.LoadMarketData()
        Dim symbol As String = cmbSymbol.SelectedValue.ToString()
        If symbol = "[ALL]" Then
            Dim dsSymbol As DataSet = ah.GetDistinctSymbolPlTrade()
            For Each dr As DataRow In dsSymbol.Tables(0).Rows
                netprofit = netprofit + NetProfitperSymbol(dsMarketData, dr("Symbol"), filter)
            Next
            Return netprofit
        Else
            netprofit = NetProfitperSymbol(dsMarketData, symbol, filter)
            Return netprofit
        End If
    End Function

    Private Function GetBidPriceBySymbol(ByVal dsMarketdata As DataSet, ByVal symbol As String) As Double
        Dim dv As DataView = dsMarketdata.Tables(0).DefaultView
        dv.RowFilter = "Symbol = '" + symbol + "'"
        If (dv.Count > 0) Then
            Return CDbl(dv(0)("BidPrice"))
        Else
            Return -1
        End If
    End Function

    Private Function GetOfferPriceBySymbol(ByVal dsMarketdata As DataSet, ByVal symbol As String) As Double
        Dim dv As DataView = dsMarketdata.Tables(0).DefaultView
        dv.RowFilter = "Symbol = '" + symbol + "'"
        If (dv.Count > 0) Then
            Return CDbl(dv(0)("OfferPrice"))
        Else
            Return -1
        End If
    End Function

    Private Function NetProfitperSymbol(ByVal dsMarketdata As DataSet, ByVal symbol As String, ByVal filter As String) As Decimal
        Dim net As Decimal
        If filter <> Nothing Then
            filter = filter + " And Symbol = '" + symbol + "'"
        Else
            filter = " Symbol = '" + symbol + "'"
        End If
        Try
            net = ds.Tables(0).Compute("Sum(Pips)", filter)
        Catch ex As Exception
            net = 0
        End Try

        If net <> 0 Then
            Dim firstcurrency, secondcurrency, basecurrency As String
            basecurrency = SettingsHome.getInstance().BaseCurrency
            firstcurrency = EServerDependents.GetFirstCurrency(symbol)
            secondcurrency = EServerDependents.GetSecondCurrency(symbol)
            If (firstcurrency = basecurrency) Then 'case 1 first currency is base currency
                net = net / GetBidPriceBySymbol(dsMarketdata, symbol)
            ElseIf (secondcurrency = basecurrency) Then 'case 2 second currency is base currency
                net = net
            Else                'case 3 none of the currency is base currency
                Dim price As Double = 0
                Try
                    price = GetBidPriceBySymbol(dsMarketdata, EServerDependents.GetCombinedCurrency(basecurrency, secondcurrency))
                    If (price <> -1) Then
                        net = net / price
                    Else
                        price = GetBidPriceBySymbol(dsMarketdata, EServerDependents.GetCombinedCurrency(secondcurrency, basecurrency))
                        If (price <> -1) Then
                            net = net * price
                        Else
                            net = 0
                        End If
                    End If
                Catch ex As Exception
                    net = 0
                End Try
            End If
        End If
        Return Decimal.Round(net, 4)
    End Function

    Private Sub grdSpotPosition_ColumnButtonClick(ByVal sender As Object, ByVal e As Janus.Windows.GridEX.ColumnActionEventArgs) Handles grdSpotPosition.ColumnButtonClick
        If grdSpotPosition.CurrentRow.Cells("Symbol").Value <> "[NET]" Then

            grdSpotPosition.CurrentRow.Cells(1).Value.ToString()
            Dim NetCc1 As Double = grdSpotPosition.CurrentRow.Cells("NetCc1").Value
            Dim alertdata As AlertsManager.NewAlert = New AlertsManager.NewAlert
            alertdata.symbol = grdSpotPosition.CurrentRow.Cells("Symbol").Value
            alertdata.senderID = grdSpotPosition.CurrentRow.Cells("AccountID").Value.ToString.Trim

            Dim action As String = ""
            If NetCc1 < 0 Then
                action = "BUY"
                alertdata.contracts = CDbl(NetCc1 * -1)
                alertdata.actiontype = AlertsManager.ACTION_BUY
            End If
            If NetCc1 > 0 Then
                action = "SELL"
                alertdata.contracts = CDbl(NetCc1)
                alertdata.actiontype = AlertsManager.ACTION_SELL
            End If
            If NetCc1 = 0 Then
                MsgBox("No open position available.", MsgBoxStyle.OkOnly, "AutoShark")
                grdSpotPosition.Focus()
                Exit Sub
            End If
            Dim tradesett As New SettingsTrade()
            tradesett.getSettings()
            Dim ah As New AlertsHome
            alertdata.chartIdentifier = -1
            alertdata.currency = ah.GetTradeCurrencyBySymbol(alertdata.symbol)
            alertdata.tradeType = tradesett.TradeTypeManual '3
            alertdata.timestamp = "simulated"
            Dim sID As String
            'If SettingsHome.getInstance.ExchangeServer = ExchangeServer.DBFX Then
            '    sID = SettingsHome.getInstance.DBFXUserName
            'Else
            sID = alertdata.senderID.Trim
            'End If
            If (Form1.GetSingletonOrderform().ConnectHT.ContainsKey(sID)) Then
                Dim obj As Object = Form1.GetSingletonOrderform().ConnectHT.Item(sID)
                Dim trader As Trader = CType(obj, Trader)
                If Not trader.Stat = Form1.ConnectionStatus.CONNECTED Then
                    MsgBox("ID " + alertdata.senderID + " is not connected.", MsgBoxStyle.OkOnly, "AutoShark")
                    grdSpotPosition.Focus()
                    Return
                End If
            Else
                MsgBox("ID " + alertdata.senderID + " is not connected.", MsgBoxStyle.OkOnly, "AutoShark")
                grdSpotPosition.Focus()
                Return
            End If

            'Form1.GetSingletonOrderform().watcher_NewAlert(alertdata)
            del_watcher.BeginInvoke(alertdata, Nothing, Nothing)
            MessageBox.Show("Order placed: " + action + " " + alertdata.symbol + " " + alertdata.contracts.ToString, "AutoShark")
            grdSpotPosition.Focus()
        End If
    End Sub

    Private Sub AddsumRow(ByVal accountID As String) ' Net Realized row 
        Dim dr As DataRow = dsSpotPosition.Tables(0).NewRow()
        dr("AccountId") = accountID
        dr("Symbol") = "[NET]"
        dr("RealizedBaseCurrency") = Decimal.Round(dsSpotPosition.Tables(0).Compute("Sum(RealizedBaseCurrency)", "AccountId='" + accountID + "'"), 4)
        dsSpotPosition.Tables(0).Rows.Add(dr)
    End Sub

    Private Sub UpdateSumRow(ByVal accountID As String, ByVal dr As DataRow) 'Updating Net Realized row
        dr("Symbol") = "[NET]"
        dr("AccountId") = accountID
        dr("RealizedBaseCurrency") = Decimal.Round(dsSpotPosition.Tables(0).Compute("Sum(RealizedBaseCurrency)", "Symbol<>'[NET]' And AccountId='" + accountID + "'"), 4)
    End Sub

    Private Sub asyncWatcherCall(ByVal alertdata As AlertsManager.NewAlert)
        Form1.GetSingletonOrderform().watcher_NewAlert(alertdata)
    End Sub
    Private Sub clearSpotposBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clearSpotposBtn.Click


        ClearSpotPosition()
    End Sub
    ''' <summary>  
    ''' ''' This will clear all flat positon from SPR window.  
    ''' ''' When user click on Clear Spot Position button,we clear all row from Grid whos openpositon is Zero...   
    ''' ''' </summary>    
    ''' ''' <remarks></remarks>  
    Private Sub ClearSpotPosition()
        Try
            Dim rowCount As Integer
            Dim filter As String = ""
            filter = "NetCC1= '0'"
            Dim dr() As DataRow = dsSpotPosition.Tables(0).Select(filter)
            For rowCount = 0 To dr.Length - 1
                dsSpotPosition.Tables(0).Rows.Remove(dr(rowCount))
            Next
        Catch ex As Exception
            Util.WriteDebugLog("Clear spot position Error -- " & ex.Message)
        End Try
    End Sub
End Class