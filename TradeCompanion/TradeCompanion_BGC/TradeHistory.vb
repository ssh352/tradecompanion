Public Class TradeHistory

    Public Sub ShowOrders(ByVal symbol As String, ByVal AccountID As String, Optional ByVal systemName As String = "", Optional ByVal systemID As String = "")
        Try
            Me.Show()
            Dim ah As AlertsHome = New AlertsHome

            Dim querry As String
            querry = "Symbol = '" + symbol + "' and SenderID= '" + AccountID + "'"
            If (systemName <> "") Then
                querry = querry + " and ExecOrderId= '" + systemName + "'"
            End If
            If (systemID <> "") Then
                querry = querry + " and MonthYear= '" + systemID + "'"
            End If

            Dim ds As DataSet = ah.LoadOrders(querry)

            Dim actionColumn As DataColumn = New DataColumn
            With actionColumn
                .DataType = System.Type.GetType("System.String")
                .ColumnName = "Action"
                .Expression = "IIF(side=1,'BUY', 'SELL')"
            End With
            ds.Tables("Orders").Columns.Add(actionColumn)

            tradeHistoryGrid.DataSource = ds
            tradeHistoryGrid.SetDataBinding(ds, "Orders")
            tradeHistoryGrid.RetrieveStructure()
            tradeHistoryGrid.Tables(0).Columns("RowID").Visible = False
            'tradeHistoryGrid.Tables(0).Columns("DateID").Caption = "Date/Time"
            'tradeHistoryGrid.Tables(0).Columns("DateID").FormatString = Now.ToString("dd/MM/yyyy hh:mm:ss tt")
            tradeHistoryGrid.Tables(0).Caption = "Trade History"
            tradeHistoryGrid.TableHeaders = Janus.Windows.GridEX.InheritableBoolean.True
            tradeHistoryGrid.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False
            tradeHistoryGrid.AutoSizeColumns()

            'CurrenEx specific change
            tradeHistoryGrid.Tables(0).Columns("Quantity").FormatString = "#,#"
            tradeHistoryGrid.Tables(0).Columns("MonthYear").Visible = False
            tradeHistoryGrid.Tables(0).Columns("DateID").Visible = False
            tradeHistoryGrid.Tables(0).Columns("Exchange").Visible = False
            tradeHistoryGrid.Tables(0).Columns("ExecOrderId").Visible = False
            tradeHistoryGrid.Tables(0).Columns("ServerLogged").Visible = False
            tradeHistoryGrid.Tables(0).Columns("Side").Visible = False
            tradeHistoryGrid.Tables(0).Columns("USDMarketPrice").Visible = False

            tradeHistoryGrid.Tables(0).Columns("Action").Position = 6
            tradeHistoryGrid.Refresh()
            Application.DoEvents()
            tradeHistoryGrid.MoveLast()
        Catch ex As Exception
            Util.WriteDebugLog(" .... Show Orders ERROR " + ex.Message)
        End Try
    End Sub

    Private Sub TradeHistory_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' ShowOrders()
    End Sub
End Class