Imports System.Configuration
Imports System.io
Imports System.Data.OleDb
Imports System.Text.RegularExpressions
Imports System.Globalization
Imports system.Threading

Public Class AlertsHome
    Private Shared m_datasourcename As String
    Private DateIDServerLog As String
    Shared mdHistory As New Hashtable()
    Shared timeFrame As String = ""

    Shared _lock As New System.Timers.Timer

    Public Sub New()
        Dim DBDir As String = ""
        Select Case SettingsHome.getInstance().ExchangeServer
            Case ExchangeServer.CurrenEx
                DBDir = Application.StartupPath + "\TradeCompanionCurrenEx.mdb"
            Case ExchangeServer.Dukascopy 'Vm_ Fix
                DBDir = Application.StartupPath + "\TradeCompanionDukascopy.mdb"
            Case ExchangeServer.FxIntegral
                DBDir = Application.StartupPath + "\TradeCompanionFxIntegral.mdb"
            Case ExchangeServer.Ariel
                If (SettingsHome.getInstance.ArielClient = "BGC") Then
                    DBDir = Application.StartupPath + "\TradeCompanionArielBGC.mdb"
                Else
                    DBDir = Application.StartupPath + "\TradeCompanionArielODL.mdb"
                End If
            Case ExchangeServer.Espeed
                DBDir = Application.StartupPath + "\TradeCompanionEspeed.mdb"
            Case ExchangeServer.DBFX
                DBDir = Application.StartupPath + "\TradeCompanionDBFX.mdb"
            Case ExchangeServer.Gain
                DBDir = Application.StartupPath + "\TradeCompanionGain.mdb"
            Case ExchangeServer.Icap
                DBDir = Application.StartupPath + "\TradeCompanionIcap.mdb"
        End Select
        Dim connstr As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DBDir + ";"
        m_datasourcename = connstr
    End Sub

    Public Sub New(ByVal connstr As String)
        m_datasourcename = connstr
    End Sub

    Public Sub AddAlert(ByVal alertstring As String)
        Dim a() As String
        Dim signal As AlertsManager.NewAlert = New AlertsManager.NewAlert
        a = Split(alertstring, " ")
        signal.exch = a(2)
        signal.symbol = a(3)
        Dim action As String = a(4)
        Select Case action.ToUpper
            Case "BUY"
                signal.actiontype = 1
            Case "SELL"
                signal.actiontype = 2
        End Select
        signal.contracts = CDbl(a(5))

        AddAlert(signal)
    End Sub

    Public Function ServerLog(ByVal signal As AlertsManager.NewAlert, ByVal dateID As String, ByVal USDMarketPrice As Double) As Boolean
        Try
            Util.WriteDebugLog("--------------serverlog-------------------------")
            Dim orderRow As New WSScalper.OrderRow
            orderRow.OrderID = signal.orderID
            orderRow.Exchange = signal.exch
            orderRow.Status = signal.statusMessage
            orderRow.Symbol = signal.symbol
            orderRow.MonthYear = signal.month_year
            orderRow.Side = signal.actiontype
            orderRow.Quantity = Int(signal.contracts)
            orderRow.Price = CStr(signal.price)

            '20061017-12:52:28 to 2006 10 17 12:52:28
            orderRow.TimeStamp = EServerDependents.GetDateTime(signal.timestamp)

            orderRow.TradeCurrency = signal.currency
            orderRow.ExecOrderId = signal.execOrderId

            Dim Keys As SettingsHome = SettingsHome.getInstance()
            orderRow.TradeCompanionID = Keys.LoginidTC
            orderRow.CurrenExID = EServerDependents.GetEServerSender()
            orderRow.DateIDCustomer = DateTime.FromOADate(Convert.ToDouble(dateID))
            orderRow.SenderId = signal.senderID
            orderRow.MarketPrice = USDMarketPrice

            SyncLock Me
                DateIDServerLog = dateID
                'AsyncCallWebServices(orderRow)
            End SyncLock
        Catch ex As Exception
            'unsuccessfull logged
            Util.WriteDebugLog(ex.Message + ex.StackTrace)
            Return False
        End Try

    End Function

    Public Sub AsyncCallWebServices(ByVal orderRow As WSScalper.OrderRow)
        Dim wsServ As WSScalper.WebServicesScalper = New WSScalper.WebServicesScalper
        AddHandler wsServ.AddOrderReturnDateCompleted, AddressOf wsServ_AddOrderCompleted
        wsServ.AddOrderReturnDateAsync(orderRow)
    End Sub

    Public Sub wsServ_AddOrderCompleted(ByVal sender As Object, ByVal args As WSScalper.AddOrderReturnDateCompletedEventArgs)
        If Not args.Error Is Nothing Then
            Exit Sub
        End If

        'Dim result As Integer
        Dim result As DateTime
        result = args.Result
        SetLogBitByDateID(True, result)
        'If (result <> "") Then
        'successfull logged
        'SetLogBitByDateID(True, result)
        'Return True
        'Else
        'unsuccessfull
        'End If
    End Sub

    Public Sub SetLogBit(ByVal status As Boolean, ByVal rowID As Integer)
        Thread.CurrentThread.CurrentCulture = SettingsHome.getInstance().Culture
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Dim serverLogged As Boolean = False
        Try
            'log the trade to server
            serverLogged = status
            Dim sql As String = "Update Orders Set ServerLogged = " + CStr(status) + " where RowID = " + rowID.ToString()
            conn.Open()
            Dim cmd As OleDbCommand = New OleDbCommand(sql, conn)
            Dim n As Integer = cmd.ExecuteNonQuery

        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()
        End Try
    End Sub

    Public Sub SetLogBitByDateID(ByVal status As Boolean, ByVal DateID As DateTime)
        Thread.CurrentThread.CurrentCulture = SettingsHome.getInstance().Culture
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Dim serverLogged As Boolean = False
        Try
            'log the trade to server
            serverLogged = status
            Dim DateInDouble As Double
            DateInDouble = DateID.ToOADate()
            Util.WriteDebugLog("Server Log set to status = " + CStr(status) + " DateID = " + DateInDouble.ToString())
            Dim sql As String = "Update Orders Set ServerLogged = " + CStr(status) + " where DateID = " + DateInDouble.ToString()
            conn.Open()
            Dim cmd As OleDbCommand = New OleDbCommand(sql, conn)
            Dim n As Integer = cmd.ExecuteNonQuery

        Catch ex As Exception
            '  MessageBox.Show(ex.StackTrace)
            'Throw ex
        Finally
            conn.Close()
        End Try
    End Sub

    Public Sub AddFill(ByVal signal As AlertsManager.NewAlert)
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Dim serverLogged As Boolean = False
        Dim trans As OleDbTransaction
        Try
            Dim inv As CultureInfo = SettingsHome.getInstance().Culture 'taken only english(US) culture for the date to string convertion & to doble to string
            Dim DateID As String
            DateID = Now.ToOADate.ToString(inv)
            Dim USDMarketPrice As Double = 0
            If signal.statusMessage <> "New" Then
                USDMarketPrice = GetUSDMarketPrice(signal.symbol)
            End If

            Dim sql As String = _
                 "INSERT INTO Orders (DateID,OrderID,Symbol,Exchange,Quantity,Side,Status,MonthYear,[TimeStamp],price,TradeCurrency, ExecOrderId, ServerLogged,SenderID, USDMarketPrice) " + _
                 "VALUES (" + DateID + ",'" + signal.orderID + "','" + signal.symbol + "','" + signal.exch + "'," + signal.contracts.ToString + "," + signal.actiontype.ToString + ",'" + signal.statusMessage + _
                 "','" + signal.month_year + "','" + signal.timestamp + "'," + signal.price.ToString(inv) + ",'" + signal.currency + "','" + signal.execOrderId + "'," + CStr(serverLogged) + ",'" + signal.senderID + "'," + USDMarketPrice.ToString(inv) + " )"

            conn.Open()
            trans = conn.BeginTransaction

            Dim cmd As OleDbCommand = New OleDbCommand(sql, conn, trans)
            Dim n As Integer = cmd.ExecuteNonQuery
            trans.Commit()
            'ServerLog(signal, DateID, USDMarketPrice)

        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()
        End Try
    End Sub

    Public Sub AddAlert(ByVal signal As AlertsManager.NewAlert)
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Dim inv As CultureInfo = SettingsHome.getInstance().Culture 'for making the date to string conversion common for all language this is part of globalization 
        Try
            Dim sql As String = _
                 "INSERT INTO Alerts (AlertDate,Symbol,Exchange,Contracts,ActionType, TradeType, Status,MonthYear,SecurityType,[TimeStamp],TradeCurrency, AlertOrderId,ChartIdentifier,SenderID) " + _
                 "VALUES (" + Now.ToOADate.ToString(inv) + ",'" + signal.symbol + "','" + signal.exch + "'," + signal.contracts.ToString + "," + signal.actiontype.ToString + "," + signal.tradeType.ToString + "," + signal.status.ToString + _
                 ",'" + signal.month_year + "'," + signal.securityType.ToString + ",'" + signal.timestamp + "','" + signal.currency + "','" + signal.orderID + "'," + signal.chartIdentifier.ToString() + ",'" + signal.senderID + "')"
            conn.Open()

            Dim cmd As OleDbCommand = New OleDbCommand(sql, conn)
            Dim n As Integer = cmd.ExecuteNonQuery
        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()
        End Try
    End Sub

    Public Sub UpdateMarketData(ByVal mdata As TradingInterface.FillMarketData)
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Try
            Dim sql As String
            If ((Not mdata.BidPrice = "0") And (Not mdata.OfferPrice = "0")) Then
                sql = "Update MarketData Set BidPrice = " + mdata.BidPrice + ", OfferPrice = " + mdata.OfferPrice + ", TimeStamps = '" + DateTime.Now.ToString(SettingsHome.getInstance().Culture) + "' where Symbol = '" + mdata.Symbol + "'"
                conn.Open()
                Dim cmd As OleDbCommand = New OleDbCommand(sql, conn)
                Dim n As Integer = cmd.ExecuteNonQuery
                If (n < 1) Then
                    sql = "INSERT INTO MarketData (Symbol,BidPrice,OfferPrice,TimeStamps) VALUES ('" + mdata.Symbol + "'," + mdata.BidPrice + "," + mdata.OfferPrice + ",'" + DateTime.Now.ToString(SettingsHome.getInstance().Culture) + "')"
                    cmd = New OleDbCommand(sql, conn)
                    n = cmd.ExecuteNonQuery
                End If
                UpdateMDHistory(mdata)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()
        End Try
    End Sub

    Public Function getActions() As DataTable
        Dim tbl As DataTable = New DataTable
        tbl.Columns.Add("ActionID", Integer.MaxValue.GetType)
        tbl.Columns.Add("Action", " ".GetType)
        Dim r As DataRow = tbl.NewRow
        r.Item("ActionID") = 1
        r.Item("Action") = "BUY"
        tbl.Rows.Add(r)

        r = tbl.NewRow
        r.Item("ActionID") = 2
        r.Item("Action") = "SELL"
        tbl.Rows.Add(r)

        tbl.TableName = "Actions"
        Return tbl
    End Function

    Public Function getTradeType() As DataTable
        Dim tbl As DataTable = New DataTable
        tbl.Columns.Add("TradeType", Integer.MaxValue.GetType)
        tbl.Columns.Add("Name", " ".GetType)
        Dim r As DataRow = tbl.NewRow
        r.Item("TradeType") = 1
        r.Item("Name") = "GTC"
        tbl.Rows.Add(r)

        r = tbl.NewRow
        r.Item("TradeType") = 3
        r.Item("Name") = "IOC"
        tbl.Rows.Add(r)

        tbl.TableName = "TradeType"
        Return tbl
    End Function

    Public Function getUnloggedTrades() As DataSet
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Try
            conn.Open()
            Dim ds As DataSet = New DataSet
            Dim sql As String = "SELECT * FROM Orders Where ServerLogged = 0"
            Dim n As Integer = New OleDbDataAdapter(sql, conn).Fill(ds, "Orders")
            Return ds
        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()
        End Try
    End Function

    Public Function getSymbolMap(Optional ByVal symbol As String = Nothing, Optional ByVal exch As String = Nothing, Optional ByVal currency As String = Nothing, Optional ByVal contracts As Double = 0) As DataSet
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Try
            conn.Open()

            Dim sql As String = "SELECT * FROM SymbolMap "
            If symbol <> Nothing Or exch <> Nothing Then
                sql = sql + " WHERE "
            End If
            If symbol <> Nothing Then
                sql = sql + " TSSymbol='" + symbol + "'"
                ' sql = sql + " UCASE(TSSymbol)='" + symbol.ToUpper + "'"
                'If exch <> Nothing And exch <> "NA" Then
                'sql = sql + " AND "
                'End If
            End If
            'If exch <> Nothing And exch <> "NA" Then
            'sql = sql + " TSExchange='" + exch + "'"
            'End If
            If currency <> Nothing Then
                sql = sql + " AND TradeCurrency='" + currency + "'"
            End If
            'Commenting out the query based on the size for now as its generated by _scalper strategy and we dont know abt it.
            'If contracts > 0 Then
            'sql = sql + " AND TradeSize=" + contracts.ToString.Trim
            'End If

            Dim ds As DataSet = New DataSet
            Dim n As Integer = New OleDbDataAdapter(sql, conn).Fill(ds, "SymbolMap")

            Dim tbl As DataTable = New DataTable
            tbl.Columns.Add("SecurityType", Integer.MaxValue.GetType)
            tbl.Columns.Add("Name", " ".GetType)
            tbl.TableName = "SecurityType"

            Dim r As DataRow = tbl.NewRow
            r.Item("SecurityType") = AlertsManager.TYPE_EQUITY
            r.Item("Name") = "EQUITY"
            tbl.Rows.Add(r)

            r = tbl.NewRow
            r.Item("SecurityType") = AlertsManager.TYPE_FUTURE
            r.Item("Name") = "FUTURE"
            tbl.Rows.Add(r)

            ds.Tables.Add(tbl)

            Dim tb2 As DataTable = New DataTable
            tb2.Columns.Add("TradeType", Integer.MaxValue.GetType)
            tb2.Columns.Add("Name", " ".GetType)
            tb2.TableName = "TradeType"

            r = tb2.NewRow
            r.Item("TradeType") = 1
            r.Item("Name") = "GTC"
            tb2.Rows.Add(r)

            r = tb2.NewRow
            r.Item("TradeType") = 3
            r.Item("Name") = "IOC"
            tb2.Rows.Add(r)

            ds.Tables.Add(tb2)

            Return ds
        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()
        End Try
    End Function

    Public Function getActiveSymbolDataSet() As DataSet
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Try
            conn.Open()

            Dim sql As String = "SELECT * FROM SymbolMap where Active=True"
            Dim ds As DataSet = New DataSet
            Dim n As Integer = New OleDbDataAdapter(sql, conn).Fill(ds, "SymbolMap")
            Dim r As DataRow = ds.Tables(0).NewRow()
            r.Item("SymbolID") = 0
            r.Item("TradeSymbol") = "[ALL]"
            r.Item("TradeCurrency") = ""
            r.Item("TradeSize") = 100000
            'ds.Tables(0).Rows.InsertAt(r, 0)
            Return ds
        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()
        End Try
    End Function

    Public Sub saveSymbolMap(ByVal ds As DataSet)
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Dim trans As OleDbTransaction
        Try
            conn.Open()
            trans = conn.BeginTransaction
            Dim sql As String = "DELETE FROM SymbolMap"
            Dim cmd As OleDbCommand = New OleDbCommand(sql, conn, trans)
            cmd.ExecuteNonQuery()
            For Each r As DataRow In ds.Tables(0).Rows
                If Not r.RowState = DataRowState.Deleted Then
                    Dim TSsymbol As String = " " 'Nothing
                    Dim TSExch As String = " " 'Nothing
                    If Not r.IsNull("TSSymbol") Then TSsymbol = r.Item("TSSymbol").toupper
                    If Not r.IsNull("TSExchange") Then TSExch = r.Item("TSExchange").toupper

                    'CurrenEx specific thing.
                    If TSExch.Trim = "" Then
                        TSExch = "FOREX"
                    End If

                    If Not TSsymbol Is Nothing AndAlso Not TSExch Is Nothing Then
                        Dim TradeSymbol As String = " " 'TSsymbol
                        If Not r.IsNull("TradeSymbol") Then
                            TradeSymbol = r.Item("TradeSymbol").toupper()
                            '  If TradeSymbol = "" Then TradeSymbol = TSsymbol
                        End If
                        Dim TradeExchange As String = " " 'TSExch
                        If Not r.IsNull("TradeExchange") Then
                            TradeExchange = r.Item("TradeExchange") '.toupper()
                            '  If TradeExchange = "" Then TradeExchange = TSExch
                        End If
                        Dim contracts As Double = 0
                        If Not r.IsNull("TradeSize") Then contracts = r.Item("TradeSize")
                        Dim securitytype As Integer = AlertsManager.TYPE_EQUITY
                        If Not r.IsNull("SecurityType") Then securitytype = r.Item("SecurityType")
                        Dim monthyear As String = " "
                        If Not r.IsNull("MonthYear") Then monthyear = r.Item("MonthYear")
                        Dim currency As String = ""
                        If Not r.IsNull("TradeCurrency") Then currency = r.Item("TradeCurrency")
                        Dim tradeType As Integer
                        If Not r.IsNull("TradeType") Then tradeType = r.Item("TradeType")
                        Dim active As Boolean
                        If Not r.IsNull("Active") Then active = r.Item("Active")

                        '  Dim symbolID As Integer = 0
                        '  If Not r.IsNull("SymbolID") Then symbolID = r.Item("SymbolID")

                        ' If symbolID = 0 Then
                        sql = "INSERT INTO SymbolMap (TSSymbol,TSExchange,TradeSymbol,TradeCurrency,TradeExchange,TradeSize,MonthYear,SecurityType,TradeType,Active) " + _
                            "VALUES('" + TSsymbol + "','" + TSExch + "','" + TradeSymbol + "','" + currency + "','" + TradeExchange + "'," + contracts.ToString + _
                            ",'" + monthyear + "'," + securitytype.ToString + "," + tradeType.ToString + "," + active.ToString() + ")"
                        'Else
                        '    sql = "UPDATE SymbolMap SET " + _
                        '        " TSSymbol='" + TSsymbol + "'," + _
                        '        " TSExchange='" + TSExch + "'," + _
                        '        " TradeSymbol='" + TradeSymbol + "'," + _
                        '        " TradeExchange='" + TradeExchange + "'," + _
                        '        " TradeSize=" + contracts.ToString + _
                        '        " WHERE SymbolID=" + symbolID.ToString
                        'End If

                        cmd = New OleDbCommand(sql, conn, trans)
                        Try
                            cmd.ExecuteNonQuery()
                        Catch ex As Exception
                            trans.Rollback()
                            Throw ex
                        End Try
                    End If
                End If
            Next r
            trans.Commit()
        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()
        End Try
    End Sub

    Public Function LoadAlerts(Optional ByVal querry As String = "") As DataSet
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Try
            conn.Open()

            Dim sql As String = "SELECT * FROM Alerts "
            If querry <> "" Then
                sql = sql + " WHERE " + querry
            End If
            sql = sql + " Order by AlertDate "
            Dim ds As DataSet = New DataSet
            Dim n As Integer = New OleDbDataAdapter(sql, conn).Fill(ds, "Alerts")

            Return ds
        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()
        End Try
    End Function

    Public Function LoadIndividualSystem(Optional ByVal querry As String = "") As DataSet
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Try
            conn.Open()

            Dim sql As String = "SELECT * FROM IndividualSystem "
            If querry <> "" Then
                sql = sql + " WHERE " + querry
            End If
            sql = sql + " Order by EntryDateTime "
            Dim ds As DataSet = New DataSet
            Dim n As Integer = New OleDbDataAdapter(sql, conn).Fill(ds, "IndividualSystem")

            Return ds
        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()
        End Try
    End Function

    Public Sub UpdateIndSys(ByVal r As DataRow)
        Thread.CurrentThread.CurrentCulture = SettingsHome.getInstance().Culture
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Try
            Util.WriteDebugLog("Updating IndividualSystem in DB")
            Dim sql As String
            conn.Open()
            sql = "Update IndividualSystem Set OpenPositionPL = " + r.Item("OpenPositionPL").ToString() + ""

            If (Not (r.IsNull("OpenPosition"))) Then
                sql = sql + ", OpenPosition = " + r.Item("OpenPosition").ToString() + ""
            End If

            If (Not (r.IsNull("RealizedPL"))) Then
                sql = sql + ", RealizedPL = " + r.Item("RealizedPL").ToString() + ""
            End If
            If (Not (r.IsNull("TotalPL"))) Then
                sql = sql + ", TotalPL = " + r.Item("TotalPL").ToString() + ""
            End If
            If (Not (r.IsNull("EntryPrice"))) Then
                If (r.Item("OpenPosition") = 0) Then
                    sql = sql + ", EntryPrice = " + "0" + ""
                Else
                    sql = sql + ", EntryPrice = " + r.Item("EntryPrice").ToString() + ""
                End If
            End If
            If (Not (r.IsNull("OrderID"))) Then
                If (r.Item("OpenPosition") = 0) Then
                    sql = sql + ", OrderID = '" + " " + "'"
                Else
                    sql = sql + ", OrderID = '" + r.Item("OrderID").ToString().Trim() + "'"
                End If
            End If
            If (Not (r.IsNull("EntryDateTime"))) Then
                If (r.Item("OpenPosition") = 0) Then
                    sql = sql + ", EntryDateTime = '" + " " + "'"
                Else
                    sql = sql + ", EntryDateTime = '" + r.Item("EntryDateTime").ToString() + "' "
                End If
            End If

            sql = sql + " where Symbol= '" + r.Item("Symbol").ToString() + "' And SystemName = '" + r.Item("SystemName").ToString() + "' And SystemNumber = '" + r.Item("SystemNumber").ToString() + "' And SenderID = '" + r.Item("SenderId") + "'"


            Dim cmd As OleDbCommand = New OleDbCommand(sql, conn)
            Dim n As Integer = cmd.ExecuteNonQuery
            If (n < 1) Then

                sql = "INSERT INTO IndividualSystem (Symbol,SystemName,SystemNumber,OpenPosition,EntryDateTime,OrderID,EntryPrice,OpenPositionPL,RealizedPL,TotalPL, SenderID) VALUES(@Symbol,@SystemName,@SystemNumber,@OpenPosition,@EntryDateTime,@OrderID,@EntryPrice,@OpenPositionPL,@RealizedPL,@TotalPL,@SenderID)"
                Dim objcmd As New OleDbCommand(sql)

                Dim paramSymbol As New OleDbParameter("@Symbol", OleDbType.VarChar, 50)
                paramSymbol.Value = r.Item("Symbol")
                objcmd.Parameters.Add(paramSymbol)

                Dim paramSystemName As New OleDbParameter("@SystemName", OleDbType.VarChar, 50)
                paramSystemName.Value = r.Item("SystemName")
                objcmd.Parameters.Add(paramSystemName)

                Dim paramSystemNumber As New OleDbParameter("@SystemNumber", OleDbType.Integer)
                paramSystemNumber.Value = r.Item("SystemNumber")
                objcmd.Parameters.Add(paramSystemNumber)

                Dim paramOpenPosition As New OleDbParameter("@OpenPosition", OleDbType.Integer)
                paramOpenPosition.Value = r.Item("OpenPosition")
                objcmd.Parameters.Add(paramOpenPosition)

                Dim paramEntryDateTime As New OleDbParameter("@EntryDateTime", OleDbType.VarChar, 50)
                paramEntryDateTime.Value = r.Item("EntryDateTime")
                objcmd.Parameters.Add(paramEntryDateTime)

                Dim paramOrderID As New OleDbParameter("@OrderID", OleDbType.VarChar, 50)
                paramOrderID.Value = r.Item("OrderID").ToString().Trim()
                objcmd.Parameters.Add(paramOrderID)

                Dim paramEntryPrice As New OleDbParameter("@EntryPrice", OleDbType.Double)
                paramEntryPrice.Value = r.Item("EntryPrice")
                objcmd.Parameters.Add(paramEntryPrice)

                Dim paramOpenPositionPL As New OleDbParameter("@OpenPositionPL", OleDbType.Double)
                paramOpenPositionPL.Value = r.Item("OpenPositionPL")
                objcmd.Parameters.Add(paramOpenPositionPL)

                Dim paramRealizedPL As New OleDbParameter("@RealizedPL", OleDbType.Double)
                paramRealizedPL.Value = r.Item("RealizedPL")
                objcmd.Parameters.Add(paramRealizedPL)

                Dim paramTotalPL As New OleDbParameter("@TotalPL", OleDbType.Double)
                paramTotalPL.Value = r.Item("TotalPL")
                objcmd.Parameters.Add(paramTotalPL)

                Dim paramSenderID As New OleDbParameter("@SenderID", OleDbType.Char, 50)
                paramSenderID.Value = r.Item("SenderID")
                objcmd.Parameters.Add(paramSenderID)

                objcmd.Connection = conn
                objcmd.ExecuteNonQuery()
            End If
            'Util.WriteDebugLog("Update complete PLCAl in DB")
        Catch ex As Exception
            Util.WriteDebugLog("UPdateIndSys:" + ex.Message + ex.StackTrace)
        Finally
            conn.Close()
        End Try
    End Sub

    Public Function LoadOrders(Optional ByVal querry As String = "") As DataSet
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Try
            conn.Open()

            Dim sql As String = "SELECT * FROM Orders "

            If querry <> "" Then
                sql = sql + " WHERE " + querry
            End If
            sql = sql + " Order by DateID "

            'If dateid > 0 Then
            'sql = sql + " WHERE Int(DateID)=" + dateid.ToString
            'End If
            'sql = sql + " Order By DateID"

            Dim ds As DataSet = New DataSet
            Dim n As Integer = New OleDbDataAdapter(sql, conn).Fill(ds, "Orders")

            Return ds
        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()
        End Try
    End Function

    Public Sub AddOrder(ByVal signal As AlertsManager.NewAlert)
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Dim trans As OleDbTransaction
        Try
            Dim sql As String = _
                 "INSERT INTO Orders (DateID,Symbol,Exchange,Quantity,Side,Status,MonthYear,Price,SenderID) " + _
                 "VALUES (" + Now.ToOADate.ToString + ",'" + signal.symbol + "','" + signal.exch + "'," + signal.contracts.ToString + "," + signal.actiontype.ToString + "," + signal.status.ToString + _
                 ",'" + signal.month_year + "'," + signal.price.ToString + ",'" + signal.senderID + "')"

            conn.Open()
            trans = conn.BeginTransaction

            Dim cmd As OleDbCommand = New OleDbCommand(sql, conn, trans)
            Dim n As Integer = cmd.ExecuteNonQuery
            trans.Commit()


        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()
        End Try
    End Sub

    Public Function getAlert(ByVal orderId As String) As AlertsManager.NewAlert
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Try
            Dim sql As String = _
                 "SELECT * FROM ALERTS WHERE AlertOrderId='" + orderId + "'"
            conn.Open()

            Dim cmd As OleDbCommand = New OleDbCommand(sql, conn)
            Dim dr As OleDbDataReader = cmd.ExecuteReader()

            If dr.Read() Then
                Dim newOrder As AlertsManager.NewAlert = New AlertsManager.NewAlert
                newOrder.contracts = dr.GetDouble(dr.GetOrdinal("Contracts"))
                newOrder.currency = dr.GetString(dr.GetOrdinal("TradeCurrency"))
                Return newOrder
            End If
            Return New AlertsManager.NewAlert

        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()
        End Try
    End Function

    Public Function LoadMarketData() As DataSet
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Try
            conn.Open()

            Dim sql As String = "SELECT * FROM MarketData "

            sql = sql + " Order by Symbol "
            Dim ds As DataSet = New DataSet
            Dim n As Integer = New OleDbDataAdapter(sql, conn).Fill(ds, "MarketData")

            Return ds
        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()
        End Try
    End Function

    Public Function LoadMarketDataHistory(Optional ByVal querry As String = "") As DataSet
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Try
            conn.Open()

            Dim sql As String = "SELECT * FROM MDHistory "
            If querry <> "" Then
                sql = sql + " WHERE " + querry
            End If
            sql = sql + " Order by Symbol "
            Dim ds As DataSet = New DataSet
            Dim n As Integer = New OleDbDataAdapter(sql, conn).Fill(ds, "MDHistory")

            Return ds
        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()
        End Try
    End Function

    Public Function GetServerSymbols() As DataSet
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Try

            conn.Open()

            Dim sql As String = "SELECT DISTINCT TRADESYMBOL FROM SymbolMap "
            'sql = sql + " WHERE "

            Dim ds As DataSet = New DataSet
            Dim n As Integer = New OleDbDataAdapter(sql, conn).Fill(ds, "SymbolMap")


            Return ds
        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()
        End Try
    End Function

    Public Function GetServerIDs() As DataSet
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Try
            conn.Open()

            Dim sql As String = "SELECT DISTINCT SENDERID FROM PLTrade "
            'sql = sql + " WHERE "

            Dim ds As DataSet = New DataSet
            Dim n As Integer = New OleDbDataAdapter(sql, conn).Fill(ds, "PLTrade")

            Return ds
        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()
        End Try
    End Function

    Public Shared Sub SetMDHash()
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Try
            conn.Open()
            Dim currentTimeFrame As String = DateTime.Now.Hour.ToString() '+ " - " + (DateTime.Now.Hour + 1).ToString() 'IIf(hh = 12, "1", (hh + 1).ToString())
            Dim todaydate As String = Date.Today.ToOADate.ToString()
            Dim sql As String = "SELECT * FROM MDHistory where DateMDData = " + todaydate + " and TimeFrame = " + currentTimeFrame
            Dim ds As DataSet = New DataSet
            Dim n As Integer = New OleDbDataAdapter(sql, conn).Fill(ds, "MDHistory")
            mdHistory.Clear()
            For Each r As DataRow In ds.Tables(0).Rows
                Dim mdh As New MDHistory
                mdh.BidPrice = r.Item("BidPrice")
                mdh.OfferPrice = r.Item("OfferPrice")
                mdh.Pips = r.Item("MaxDifference")
                mdh.Symbol = r.Item("Symbol")
                mdh.TimeStamp = Date.FromOADate(r.Item("TimeStamps"))
                If (mdHistory.ContainsKey(mdh.Symbol)) Then mdHistory.Remove(mdh.Symbol)
                mdHistory.Add(mdh.Symbol, mdh)
            Next r
        Catch ex As Exception
            'MessageBox.Show(ex.Message)
            Util.WriteDebugLog("Error MDHIstory:" + ex.Message)
            Util.WriteDebugLog("Stack Trace:" + ex.StackTrace)
        Finally
            conn.Close()
        End Try
    End Sub

    Public Shared Sub DumpMDHistory()
        Dim de As DictionaryEntry
        Dim mdh As MDHistory
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Try
            conn.Open()
            If (mdHistory.Count > 0) Then
                Dim enu As IDictionaryEnumerator = mdHistory.GetEnumerator()
                enu.MoveNext()
                mdh = enu.Value
                Dim currentTimeFrame As String = mdh.TimeStamp.Hour
                Dim todaydate As String = mdh.TimeStamp.Date.ToOADate.ToString()
                'todaydate()
                Dim dSql As String = "Delete FROM MDHistory where DateMDData = " + todaydate + " and TimeFrame = " + currentTimeFrame
                Dim trans As OleDbTransaction = conn.BeginTransaction()
                Dim cmdDel As OleDbCommand = New OleDbCommand(dSql, conn)
                cmdDel.Transaction = trans.Begin()
                Dim effectedRows As Integer = cmdDel.ExecuteNonQuery()
                cmdDel.Transaction.Commit()
                trans.Commit()
            End If

            For Each de In mdHistory
                Try
                    mdh = de.Value
                    Dim hh As Integer = mdh.TimeStamp.Hour
                    Dim timeframe As String = hh.ToString() '+ " - " + (hh + 1).ToString() 'IIf(hh = 12, "1", (hh + 1).ToString())
                    Dim sql As String
                    Dim cmd As OleDbCommand
                    sql = "INSERT INTO MDHistory (Symbol,DateMDData,TimeFrame,MaxDifference,TimeStamps,BidPrice,OfferPrice) VALUES ('" + mdh.Symbol + "'," + mdh.TimeStamp.Date.ToOADate.ToString() + "," + timeframe + "," + mdh.Pips.ToString() + "," + mdh.TimeStamp.ToOADate().ToString() + "," + mdh.BidPrice.ToString() + "," + mdh.OfferPrice.ToString() + ")"
                    cmd = New OleDbCommand(sql, conn)
                    cmd.ExecuteNonQuery()
                Catch ex As Exception
                    Throw ex
                End Try
            Next de
        Catch ex As Exception
            Util.WriteDebugLog(ex.Message + ex.StackTrace)
        Finally
            conn.Close()
        End Try

    End Sub

    Public Sub SaveGraph(ByVal ds As DataSet)
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Dim trans As OleDbTransaction
        Try
            conn.Open()
            trans = conn.BeginTransaction
            Dim sql As String = "DELETE FROM Graph"
            Dim cmd As OleDbCommand = New OleDbCommand(sql, conn, trans)
            cmd.ExecuteNonQuery()
            For Each r As DataRow In ds.Tables(0).Rows
                If Not r.RowState = DataRowState.Deleted Then
                    Dim Trades As String = " " 'Nothing
                    Dim Amount As String = " " 'Nothing
                    If Not r.IsNull("Trades") Then Trades = r.Item("Trades")
                    If Not r.IsNull("Amount") Then Amount = r.Item("Amount")

                    sql = "INSERT INTO Graph (Trades,Amount) " + _
                        "VALUES(" + Trades + "," + Amount + ")"

                    cmd = New OleDbCommand(sql, conn, trans)
                    Try
                        cmd.ExecuteNonQuery()
                    Catch ex As Exception
                        trans.Rollback()
                        Throw ex
                    End Try
                End If
            Next r
            trans.Commit()
        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()
        End Try
    End Sub

    Public Function GetGraph() As DataSet
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Try
            conn.Open()

            'Retrieve only structure
            Dim sql As String = "SELECT * from Graph where ID = 0;"

            Dim ds As DataSet = New DataSet
            Dim n As Integer = New OleDbDataAdapter(sql, conn).Fill(ds, "Graph")


            Return ds
        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()
        End Try
    End Function

    Public Function GetPLTrade(ByVal symbol As String, ByVal id As String) As DataSet
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Try
            conn.Open()
            Dim sql As String
            sql = "SELECT * from PLTrade"
            If (symbol <> "" Or id <> "") Then sql = sql + " WHERE "
            If (symbol <> "") Then
                sql = sql + " Symbol='" + symbol + "'"
            End If
            If (symbol <> "" And id <> "") Then sql = sql + " AND "
            If (id <> "") Then
                sql = sql + " SenderID='" + id + "'"
            End If
            sql = sql + " order by DateID "
            'MsgBox(sql)
            'Retrieve only structure
            'Dim sql As String = "SELECT * from PLTrade;"

            Dim ds As DataSet = New DataSet
            Dim n As Integer = New OleDbDataAdapter(sql, conn).Fill(ds, "PLTrade")

            Return ds
        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()
        End Try
    End Function

    Public Function AddPLTrade(ByVal signal As AlertsManager.NewAlert, ByRef dsPLTrade As DataSet) As Boolean
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Dim amountPrice As Decimal
        SyncLock _lock
            Try
                Dim Sql As String
                Dim cmd As OleDbCommand
                amountPrice = signal.contracts * signal.price
                Thread.CurrentThread.CurrentCulture = SettingsHome.getInstance().Culture

                Dim remaining As Double
                Dim remainingAmount As Double
                Dim pips As Decimal = 0
                Dim pipsBaseCurrency As Decimal = 0
                Dim pipSys As Decimal = 0
                'BUY  1  'SELL 2
                'Dim dsPLTrade As DataSet = GetPLTrade("", "")
                'Dim drPLTrade As DataRow = dsPLTrade.Tables(0).NewRow()
                conn.Open()
                Dim serverTimeStamp As Date = EServerDependents.GetDateTime(signal.timestamp)

                If (signal.actiontype = 1) Then 'BUY
                    pips = PipForBuy(dsPLTrade, signal, conn, remainingAmount, False)
                    pipsBaseCurrency = pips
                    pipSys = PipForBuy(dsPLTrade, signal, conn, remaining, True)

                    'pips will always 0 in case of BUY
                    pips = 0
                    pipsBaseCurrency = 0
                    pipSys = 0
                    Dim DateID As String = Now.ToOADate().ToString()
                    Sql = _
                      "INSERT INTO PLTrade (OrderID,Amount,Remaining,Price,Actions,Symbol,Pips,ExecOrderId,Status,DateID,ServerDateTime,SenderID,PipsBaseCurrency,NetAmount,SystemID,RemainingSys,PipSys) " + _
                      "VALUES ('" + signal.orderID + "'," + signal.contracts.ToString() + "," + remainingAmount.ToString() + "," + signal.price.ToString() + "," + signal.actiontype.ToString() + ",'" + signal.symbol + _
                      "'," + pips.ToString() + ",'" + signal.execOrderId + "','" + signal.statusMessage + "'," + DateID + "," + serverTimeStamp.ToOADate().ToString() + ",'" + signal.senderID + "'," + pipsBaseCurrency.ToString() + _
                       "," + amountPrice.ToString() + ", '" + signal.month_year + "' ," + remaining.ToString() + "," + pipSys.ToString() + ");"
                    Dim newDrPL As DataRow = dsPLTrade.Tables(0).NewRow()
                    newDrPL("OrderID") = signal.orderID
                    newDrPL("Amount") = signal.contracts.ToString()
                    newDrPL("Remaining") = remainingAmount.ToString()
                    newDrPL("Price") = signal.price.ToString()
                    newDrPL("Actions") = signal.actiontype.ToString()
                    newDrPL("Symbol") = signal.symbol
                    newDrPL("Pips") = pips.ToString()
                    newDrPL("ExecOrderId") = signal.execOrderId
                    newDrPL("Status") = signal.statusMessage
                    newDrPL("DateID") = DateID
                    newDrPL("ServerDateTime") = serverTimeStamp.ToOADate().ToString()
                    newDrPL("SenderID") = signal.senderID
                    newDrPL("PipsBaseCurrency") = pipsBaseCurrency.ToString()
                    newDrPL("NetAmount") = amountPrice.ToString()
                    newDrPL("SystemID") = signal.month_year
                    newDrPL("RemainingSys") = remaining
                    newDrPL("PipSys") = pipSys
                    dsPLTrade.Tables(0).Rows.Add(newDrPL)

                    cmd = New OleDbCommand(Sql, conn)
                    cmd.ExecuteNonQuery()
                ElseIf (signal.actiontype = 2) Then 'SELL
                    pips = PipForSell(dsPLTrade, signal, conn, remainingAmount, False)
                    pipsBaseCurrency = pips
                    pipSys = PipForSell(dsPLTrade, signal, conn, remaining, True)

                    Dim DateID As String = Now.ToOADate().ToString()
                    Sql = _
                      "INSERT INTO PLTrade (OrderID,Amount,Remaining,Price,Actions,Symbol,Pips,ExecOrderId,Status,DateID,ServerDateTime,SenderID,PipsBaseCurrency,NetAmount,SystemID,RemainingSys,PipSys) " + _
                      "VALUES ('" + signal.orderID + "'," + signal.contracts.ToString() + "," + remainingAmount.ToString() + "," + signal.price.ToString() + "," + signal.actiontype.ToString + ",'" + signal.symbol + _
                      "'," + pips.ToString() + ",'" + signal.execOrderId + "','" + signal.statusMessage + "'," + DateID + "," + serverTimeStamp.ToOADate().ToString() + ",'" + signal.senderID + "'," + pipsBaseCurrency.ToString() + _
                     "," + amountPrice.ToString() + "," + signal.month_year + "," + remaining.ToString() + "," + pipSys.ToString() + ");"

                    Dim newDrPL As DataRow = dsPLTrade.Tables(0).NewRow()
                    newDrPL("OrderID") = signal.orderID
                    newDrPL("Amount") = signal.contracts.ToString()
                    newDrPL("Remaining") = remainingAmount.ToString()
                    newDrPL("Price") = signal.price.ToString()
                    newDrPL("Actions") = signal.actiontype.ToString()
                    newDrPL("Symbol") = signal.symbol
                    newDrPL("Pips") = pips.ToString()
                    newDrPL("ExecOrderId") = signal.execOrderId
                    newDrPL("Status") = signal.statusMessage
                    newDrPL("DateID") = DateID
                    newDrPL("ServerDateTime") = serverTimeStamp.ToOADate().ToString()
                    newDrPL("SenderID") = signal.senderID
                    newDrPL("PipsBaseCurrency") = pipsBaseCurrency.ToString()
                    newDrPL("NetAmount") = amountPrice.ToString()
                    newDrPL("SystemID") = signal.month_year
                    newDrPL("RemainingSys") = remaining
                    newDrPL("PipSys") = pipSys
                    dsPLTrade.Tables(0).Rows.Add(newDrPL)

                    cmd = New OleDbCommand(Sql, conn)
                    cmd.ExecuteNonQuery()
                End If
                'Return dsPLTrade

            Catch ex As Exception
                Util.WriteDebugLog("ADD PLTrade Error: " + ex.Message)
                Util.WriteDebugLog("StackTrace: " + ex.StackTrace)
            Finally
                'final operation to reequilibrate remaining sizes to positions
                RegularizeRemainingAmount(signal, dsPLTrade, conn, False)
                RegularizeRemainingAmount(signal, dsPLTrade, conn, True)

                conn.Close()
            End Try

        End SyncLock
        'Util.WriteDebugLog("Adding/Calc PL Trade Complete")
    End Function

    Private Function PipForBuy(ByRef dsPLTrade As DataSet, ByVal signal As AlertsManager.NewAlert, ByVal conn As OleDbConnection, ByRef remainingAmount As Double, ByVal sel As Boolean) As Double
        Dim remaining As Double
        Dim pips As Decimal = 0
        Dim pipsBaseCurrency As Decimal = 0
        Dim Sql As String
        Dim drPL() As DataRow
        Dim cmd As OleDbCommand
        Dim n As Integer
        Dim filter As String
        If (sel) Then
            filter = "Actions = 2 AND RemainingSys > 0 AND Symbol = '" + signal.symbol + "' AND SenderID = '" + signal.senderID + "' AND ExecOrderId = '" + signal.execOrderId + "' AND SystemId = '" + signal.month_year + "'"
        Else
            filter = "Actions = 2 AND Remaining > 0 AND Symbol = '" + signal.symbol + "' AND SenderID = '" + signal.senderID + "'"
        End If
        ' AND ExecOrderId = '" + signal.execOrderId + "' AND SystemID= '" + signal.month_year + "'"
        Dim drs() As DataRow = dsPLTrade.Tables(0).Select(filter, "DateID")
        remainingAmount = signal.contracts

        If (drs.Length > 0) Then
            For Each dr As DataRow In drs
                Dim temp As Double
                If (sel) Then
                    temp = dr("RemainingSys")
                    pips = dr("PipSys")
                Else
                    temp = dr("Remaining")
                    pips = dr("Pips")
                End If

                pipsBaseCurrency = dr("PipsBaseCurrency")
                If (remainingAmount > temp) Then
                    remainingAmount = remainingAmount - temp

                    Dim diffPrice As Decimal
                    diffPrice = CDec(dr("Price")) - CDec(signal.price)
                    diffPrice = diffPrice * temp
                    pips = pips + diffPrice
                    pipsBaseCurrency = pips
                    remaining = 0
                Else
                    remaining = temp - remainingAmount
                    Dim diffPrice As Decimal
                    diffPrice = CDec(dr("Price")) - CDec(signal.price)
                    diffPrice = diffPrice * remainingAmount
                    pips = pips + diffPrice

                    remainingAmount = 0

                    pipsBaseCurrency = pips

                    'Sql = "Update PLTrade Set Remaining = " + CStr(remaining) + ", Pips = " + CStr(pips) + ", PipsBaseCurrency= " + CStr(pipsBaseCurrency) + " where RowID = " + CStr(dr("RowID"))
                    If (sel) Then
                        Sql = "Update PLTrade Set RemainingSys = " + CStr(remaining) + ", PipSys = " + CStr(pips) + " where DateID = " + CStr(dr("DateID"))
                    Else
                        Sql = "Update PLTrade Set Remaining = " + CStr(remaining) + ", Pips = " + CStr(pips) + ", PipsBaseCurrency= " + CStr(pipsBaseCurrency) + " where DateID = " + CStr(dr("DateID"))
                    End If

                    cmd = New OleDbCommand(Sql, conn)
                    n = cmd.ExecuteNonQuery

                    drPL = dsPLTrade.Tables(0).Select("DateID = " + CStr(dr("DateID")))
                    If (drPL.Length > 0) Then
                        If (sel) Then
                            drPL(0)("RemainingSys") = CStr(remaining)
                            drPL(0)("PipSys") = CStr(pips)
                        Else
                            drPL(0)("Remaining") = CStr(remaining)
                            drPL(0)("Pips") = CStr(pips)
                            drPL(0)("PipsBaseCurrency") = CStr(pipsBaseCurrency)
                        End If
                    Else
                        Util.WriteDebugLog("ADD PLTrade : No row to update")
                    End If
                    Exit For
                End If

                'update the dr here
                'Sql = "Update PLTrade Set Remaining = " + CStr(remaining) + ", Pips = " + CStr(pips) + ", PipsBaseCurrency= " + CStr(pipsBaseCurrency) + " where DateID = " + CStr(dr("DateID"))
                If (sel) Then
                    Sql = "Update PLTrade Set RemainingSys = " + CStr(remaining) + ", PipSys = " + CStr(pips) + " where DateID = " + CStr(dr("DateID"))
                Else
                    Sql = "Update PLTrade Set Remaining = " + CStr(remaining) + ", Pips = " + CStr(pips) + ", PipsBaseCurrency= " + CStr(pipsBaseCurrency) + " where DateID = " + CStr(dr("DateID"))
                End If
                cmd = New OleDbCommand(Sql, conn)
                n = cmd.ExecuteNonQuery

                drPL = dsPLTrade.Tables(0).Select("DateID = " + CStr(dr("DateID")))
                If (drPL.Length > 0) Then
                    If (sel) Then
                        drPL(0)("RemainingSys") = CStr(remaining)
                        drPL(0)("PipSys") = CStr(pips)
                    Else
                        drPL(0)("Remaining") = CStr(remaining)
                        drPL(0)("Pips") = CStr(pips)
                        drPL(0)("PipsBaseCurrency") = CStr(pipsBaseCurrency)
                    End If
                Else
                    Util.WriteDebugLog("ADD PLTrade : No row to update")
                End If
            Next
        End If
        Return pips
    End Function

    Private Function PipForSell(ByRef dsPLTrade As DataSet, ByVal signal As AlertsManager.NewAlert, ByVal conn As OleDbConnection, ByRef remainingAmount As Double, ByVal sel As Boolean) As Double
        Dim remaining As Double
        Dim pips As Decimal = 0
        Dim pipsBaseCurrency As Decimal = 0
        Dim Sql As String
        Dim drPL() As DataRow
        Dim cmd As OleDbCommand
        Dim n As Integer

        Dim filter As String
        If (sel) Then
            filter = "Actions = 1 AND RemainingSys > 0 AND Symbol = '" + signal.symbol + "'  AND SenderID ='" + signal.senderID + "' AND ExecOrderId = '" + signal.execOrderId + "' AND SystemId = '" + signal.month_year + "'"
        Else
            filter = "Actions = 1 AND Remaining > 0 AND Symbol = '" + signal.symbol + "'  AND SenderID ='" + signal.senderID + "'"
        End If
        Dim drs() As DataRow = dsPLTrade.Tables(0).Select(filter, "DateID")
        remainingAmount = signal.contracts

        For Each dr As DataRow In drs
            Dim temp As Double
            If (sel) Then
                temp = dr("RemainingSys")
            Else
                temp = dr("Remaining")
            End If

            If (remainingAmount > temp) Then
                remainingAmount = remainingAmount - temp

                Dim diffPrice As Decimal
                diffPrice = CDec(signal.price) - CDec(dr("Price"))
                diffPrice = diffPrice * temp
                pips = pips + diffPrice

                'added to run it, need to delete this column
                pipsBaseCurrency = pips
                remaining = 0
            Else
                remaining = temp - remainingAmount

                Dim diffPrice As Decimal
                diffPrice = CDec(signal.price) - CDec(dr("Price"))
                diffPrice = diffPrice * remainingAmount
                pips = pips + diffPrice

                'added to run it, need to delete this column
                pipsBaseCurrency = pips
                remainingAmount = 0
                drPL = dsPLTrade.Tables(0).Select("DateID = " + CStr(dr("DateID")))
                If (sel) Then
                    Sql = "Update PLTrade Set RemainingSys = " + CStr(remaining) + " where DateID = " + CStr(dr("DateID"))
                    drPL(0)("RemainingSys") = CStr(remaining)
                Else
                    Sql = "Update PLTrade Set Remaining = " + CStr(remaining) + " where DateID = " + CStr(dr("DateID"))
                    drPL(0)("Remaining") = CStr(remaining)
                End If
                cmd = New OleDbCommand(Sql, conn)
                n = cmd.ExecuteNonQuery
                Exit For
            End If

            'update the dr here
            drPL = dsPLTrade.Tables(0).Select("DateID = " + CStr(dr("DateID")))
            If (sel) Then
                Sql = "Update PLTrade Set RemainingSys = " + CStr(remaining) + " where DateID = " + CStr(dr("DateID"))
                drPL(0)("RemainingSys") = CStr(remaining)
            Else
                Sql = "Update PLTrade Set Remaining = " + CStr(remaining) + " where DateID = " + CStr(dr("DateID"))
                drPL(0)("Remaining") = CStr(remaining)
            End If
            cmd = New OleDbCommand(Sql, conn)
            n = cmd.ExecuteNonQuery
        Next
        Return pips
    End Function
    Private Sub RegularizeRemainingAmount(ByVal signal As AlertsManager.NewAlert, ByRef dsPLTrade As DataSet, ByVal conn As OleDbConnection, ByVal sel As Boolean)
        Try

            Dim netccy1 As Double = GetNetCC1(dsPLTrade, signal, sel)
            If netccy1 >= 0 Then

                Dim remain As Integer = GetRemainingBySide(signal, dsPLTrade, 1, sel)
                If remain <> netccy1 Then  'some remaining has not been cleared - decrease number and correct P/L - UP/L
                    Dim diff As Integer = remain - netccy1
                    ModifyRemaining(diff, 1, signal, dsPLTrade, conn, sel)
                End If

                remain = GetRemainingBySide(signal, dsPLTrade, 2, sel)
                If remain <> 0 Then
                    ModifyRemaining(remain, 2, signal, dsPLTrade, conn, sel)
                End If

            ElseIf netccy1 <= 0 Then
                Dim remain As Integer = GetRemainingBySide(signal, dsPLTrade, 2, sel)
                If remain <> Math.Abs(netccy1) Then  'some remaining has not been cleared - decrease number and correct P/L - UP/L
                    Dim diff As Integer = remain - Math.Abs(netccy1)
                    ModifyRemaining(diff, 2, signal, dsPLTrade, conn, sel)
                End If

                remain = GetRemainingBySide(signal, dsPLTrade, 1, sel)
                If remain <> 0 Then
                    ModifyRemaining(remain, 1, signal, dsPLTrade, conn, sel)
                End If

            End If

        Catch ex As Exception
            Util.WriteDebugLog(ex.Message + ex.StackTrace)
        End Try

    End Sub

    Private Function SelectDistinct(ByVal sourceRows() As DataRow, ByVal sourceColumn As String) As Hashtable
        Try
            Dim ht As Hashtable = New Hashtable
            For Each dr As DataRow In sourceRows
                If Not ht.ContainsKey(dr(sourceColumn)) Then
                    ht.Add(dr(sourceColumn), Nothing)
                End If
            Next
            Return ht
        Catch ex As System.Exception
            Util.WriteDebugLog("Error SelectDistinct" + ex.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetBidPriceBySymbol(ByVal symbol As String) As String 'Test1234
        Try
            Dim dv As DataView = Form1.GetSingletonOrderform().dsMarkeData.Tables(0).Copy().DefaultView 'LoadMarketData().Tables(0).DefaultView
            'Dim dv As DataView = DiconnectedDatasets.GetDiconnectedDatasetsSingleton().DSMarkeData.Tables(0).DefaultView 'LoadMarketData().Tables(0).DefaultView
            dv.RowFilter = "Symbol = '" + symbol + "'"
            If (dv.Count > 0) Then
                Return dv(0)("BidPrice")
            Else
                Return -1
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetAskPriceBySymbol(ByVal symbol As String) As String 'Test1234
        Dim dv As DataView = Form1.GetSingletonOrderform().dsMarkeData.Tables(0).Copy().DefaultView 'LoadMarketData().Tables(0).DefaultView
        'Dim dv As DataView = DiconnectedDatasets.GetDiconnectedDatasetsSingleton().DSMarkeData.Tables(0).DefaultView 'LoadMarketData().Tables(0).DefaultView
        dv.RowFilter = "Symbol = '" + symbol + "'"
        If (dv.Count > 0) Then
            Return dv(0)("OfferPrice")
        Else
            Return -1
        End If
    End Function

    Public Function GetIdMap() As DataSet
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Try
            conn.Open()

            Dim sql As String = "SELECT * FROM IDMap "

            Dim ds As DataSet = New DataSet
            Dim n As Integer = New OleDbDataAdapter(sql, conn).Fill(ds, "IDMap")
            Return ds
        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()
        End Try
    End Function

    Public Sub saveIdMap(ByVal ds As DataSet)
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Dim trans As OleDbTransaction
        Try
            conn.Open()
            trans = conn.BeginTransaction()
            Dim sql As String = "DELETE FROM IDMap"
            Dim cmd As OleDbCommand = New OleDbCommand(sql, conn, trans)
            cmd.ExecuteNonQuery()
            For Each r As DataRow In ds.Tables(0).Rows
                If Not r.RowState = DataRowState.Deleted Then
                    Dim TSID As String = " "
                    If Not r.IsNull("TradeStationID") Then TSID = r.Item("TradeStationID")
                    Dim serverID As String = " "
                    If Not r.IsNull("ServerID") Then serverID = r.Item("ServerID")
                    Dim active As Boolean
                    If Not r.IsNull("Active") Then active = r.Item("Active")

                    sql = "INSERT INTO IDMap(TradeStationID,ServerID,Active)" + _
                        "VALUES('" + TSID + "','" + serverID + "'," + active.ToString() + ")"

                    cmd = New OleDbCommand(sql, conn, trans)
                    Try
                        cmd.ExecuteNonQuery()
                    Catch ex As Exception
                        trans.Rollback()
                        Throw ex
                    End Try
                End If
            Next r
            trans.Commit()
        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()
        End Try
    End Sub

    Public Function MapTSID(ByVal TSID As String) As String
        Dim serverID As String = ""
        Dim ds As DataSet = GetIdMap()
        Dim dv As DataView = ds.Tables(0).DefaultView
        dv.RowFilter = "TradeStationID = '" + TSID + "'"
        If (dv.Count > 0) Then
            serverID = dv(0)("ServerID")
        End If
        Return serverID
    End Function

    Public Sub UpdateMDHistory(ByVal mdata As TradingInterface.FillMarketData)
        'Dim lockThis As System.Object = New System.Object
        'SyncLock lockThis
        'Dim inv As CultureInfo = New CultureInfo("en-US")
        Thread.CurrentThread.CurrentCulture = SettingsHome.getInstance().Culture
        Try
            'Util.WriteDebugLog("Market Data History manipulation")
            Dim hh As Integer = DateTime.Now.Hour
            Dim currentTimeFrame As String = hh.ToString() '+ " - " + (hh + 1).ToString() 'IIf(hh = 12, "1", (hh + 1).ToString())
            Dim diff As String
            Dim diffInt As Integer
            diff = CDec(mdata.OfferPrice) - CDec(mdata.BidPrice)

            If (diff.IndexOf("."c) > 0) Then
                diff = diff.Split(".")(1)
            Else
                'Util.WriteDebugLog("Differnce  is Zero")
                Return
            End If
            Try
                'diffInt = CInt(diff)
                diffInt = CInt(Regex.Replace(diff, "^0*", ""))
            Catch ex As Exception
                diffInt = 0
            End Try

            If ((Not (mdHistory.Contains(mdata.Symbol))) Or timeFrame = currentTimeFrame) Then
                Dim prevDiff As Integer = 0
                If (mdHistory.Contains(mdata.Symbol)) Then prevDiff = CType(mdHistory.Item(mdata.Symbol), MDHistory).Pips
                If ((Not (mdHistory.Contains(mdata.Symbol))) Or diffInt > prevDiff) Then
                    timeFrame = currentTimeFrame
                    Dim mdh As New MDHistory
                    mdh.BidPrice = CDec(mdata.BidPrice)
                    mdh.OfferPrice = CDec(mdata.OfferPrice)
                    mdh.Pips = diffInt
                    mdh.Symbol = mdata.Symbol
                    mdh.TimeStamp = DateTime.Now
                    mdHistory.Remove(mdata.Symbol)
                    mdHistory.Add(mdata.Symbol, mdh)
                End If
            End If
            If (timeFrame <> currentTimeFrame) Then
                timeFrame = currentTimeFrame
                'Dim ah As New AlertsHome
                DumpMDHistory()
                mdHistory.Clear()
            End If
            'Util.WriteDebugLog("Market Data History manipulation Complete")
        Catch ex As Exception
            Util.WriteDebugLog(ex.Message + ex.Source)
            Util.WriteDebugLog(ex.StackTrace)
            Throw ex
        End Try
        'End SyncLock
    End Sub

    Public Sub UpdatePLCal(ByVal r As DataRow)
        'Dim inv As CultureInfo = New CultureInfo("")
        Thread.CurrentThread.CurrentCulture = SettingsHome.getInstance().Culture
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Try
            Util.WriteDebugLog("Updating PLCAl in DB")
            Dim sql As String
            conn.Open()
            sql = "Update PLCal set Realized = " + r.Item("Realized").ToString() + ""

            If (Not (r.IsNull("UnRealized"))) Then
                sql = sql + ", Unrealized = " + r.Item("UnRealized").ToString() + ""
            End If
            'sql = "Update PLCal set Realized = " + r.Item("Realized").ToString() + ", Unrealized = " + r.Item("UnRealized").ToString()

            If (Not (r.IsNull("UnrealizedBaseCurrency"))) Then
                sql = sql + ", UnrealizedBaseCurrency = " + r.Item("UnrealizedBaseCurrency").ToString() + ""
            End If
            If (Not (r.IsNull("RealizedBaseCurrency"))) Then
                sql = sql + ", RealizedBaseCurrency = " + r.Item("RealizedBaseCurrency").ToString() + ""
            End If
            If (Not (r.IsNull("NetCC1"))) Then
                sql = sql + ", NetCC1 = " + r.Item("NetCC1").ToString() + ""
            End If
            If (Not (r.IsNull("NetCC2"))) Then
                sql = sql + ", NetCC2 = " + r.Item("NetCC2").ToString() + ""
            End If
            If (Not (r.IsNull("AverageBuyRate"))) Then
                sql = sql + ", AverageBuyRate = " + r.Item("AverageBuyRate").ToString() + ""
            End If
            If (Not (r.IsNull("AverageSellRate"))) Then
                sql = sql + ", AverageSellRate = " + r.Item("AverageSellRate").ToString() + ""
            End If
            If (Not (r.IsNull("AllInRate"))) Then
                sql = sql + ", AllInRate = " + r.Item("AllInRate").ToString() + ""
            End If
            If (Not (r.IsNull("OpenRate"))) Then
                sql = sql + ", OpenRate = " + r.Item("OpenRate").ToString() + ""
            End If
            If (Not (r.IsNull("MktRate"))) Then sql = sql + ", MktRate = " + r.Item("MktRate").ToString() + ""
            sql = sql + " where Symbol= '" + r.Item("Symbol").ToString() + "' And AccountId = '" + r.Item("AccountID").ToString() + "'"


            Dim cmd As OleDbCommand = New OleDbCommand(sql, conn)
            Dim n As Integer = cmd.ExecuteNonQuery
            If (n < 1) Then
                Dim secondcurrency, basecurrency, baseSymbol As String
                basecurrency = SettingsHome.getInstance().BaseCurrency
                If (r.Item("Symbol").ToString().Contains(basecurrency)) Then
                    baseSymbol = r.Item("Symbol").ToString()
                Else
                    secondcurrency = EServerDependents.GetSecondCurrency(r.Item("Symbol").ToString())
                    baseSymbol = EServerDependents.GetCombinedCurrency(basecurrency, secondcurrency)
                End If

                sql = "INSERT INTO PLCal (Symbol,Realized,Unrealized,AccountID,RealizedBaseCurrency,UnrealizedBaseCurrency,BaseSymbol,NetCC1, NetCC2, AverageBuyRate, AverageSellRate,AllInRate,OpenRate,MktRate) VALUES(@Symbol,@Realized,@UnRealized,@AccountID,@RealizedBaseCurrency,@UnrealizedBaseCurrency,@BaseSymbol,@NetCC1,@NetCC2,@AverageBuyRate,@AverageSellRate,@AllInRate,@OpenRate,@MktRate)"
                Dim objcmd As New OleDbCommand(sql)

                Dim paramSymbol As New OleDbParameter("@Symbol", OleDbType.VarChar, 50)
                paramSymbol.Value = r.Item("Symbol")
                objcmd.Parameters.Add(paramSymbol)

                Dim paramRealized As New OleDbParameter("@Realized", OleDbType.Double)
                paramRealized.Value = r.Item("Realized")
                objcmd.Parameters.Add(paramRealized)

                Dim paramUnrealized As New OleDbParameter("@Unrealized", OleDbType.Double)
                paramUnrealized.Value = r.Item("Unrealized")
                objcmd.Parameters.Add(paramUnrealized)

                Dim paramAccountID As New OleDbParameter("@AccountID", OleDbType.VarChar, 50)
                paramAccountID.Value = r.Item("AccountID")
                objcmd.Parameters.Add(paramAccountID)

                Dim paramRealizedBaseCurrency As New OleDbParameter("@RealizedBaseCurrency", OleDbType.Double)
                paramRealizedBaseCurrency.Value = r.Item("RealizedBaseCurrency")
                objcmd.Parameters.Add(paramRealizedBaseCurrency)

                Dim paramUnrealizedBaseCurrency As New OleDbParameter("@UnrealizedBaseCurrency", OleDbType.Double)
                paramUnrealizedBaseCurrency.Value = r.Item("UnrealizedBaseCurrency")
                objcmd.Parameters.Add(paramUnrealizedBaseCurrency)

                Dim paramBaseSymbol As New OleDbParameter("@BaseSymbol", OleDbType.VarChar, 50)
                paramBaseSymbol.Value = baseSymbol
                objcmd.Parameters.Add(paramBaseSymbol)

                Dim paramNetCC1 As New OleDbParameter("@NetCC1", OleDbType.Double)
                paramNetCC1.Value = r.Item("NetCC1")
                objcmd.Parameters.Add(paramNetCC1)

                Dim paramNetCC2 As New OleDbParameter("@NetCC2", OleDbType.Double)
                paramNetCC2.Value = r.Item("NetCC2")
                objcmd.Parameters.Add(paramNetCC2)

                Dim paramAverageBuyRate As New OleDbParameter("@AverageBuyRate", OleDbType.Double)
                paramAverageBuyRate.Value = r.Item("AverageBuyRate")
                objcmd.Parameters.Add(paramAverageBuyRate)

                Dim paramAverageSellRate As New OleDbParameter("@AverageSellRate", OleDbType.Double)
                paramAverageSellRate.Value = r.Item("AverageSellRate")
                objcmd.Parameters.Add(paramAverageSellRate)

                Dim paramAllInRate As New OleDbParameter("@AllInRate", OleDbType.Double)
                paramAllInRate.Value = r.Item("AllInRate")
                objcmd.Parameters.Add(paramAllInRate)

                Dim paramOpenRate As New OleDbParameter("@OpenRate", OleDbType.Double)
                paramOpenRate.Value = r.Item("OpenRate")
                objcmd.Parameters.Add(paramOpenRate)

                Dim paramMktRate As New OleDbParameter("@MktRate", OleDbType.Double)
                paramMktRate.Value = r.Item("MktRate")
                objcmd.Parameters.Add(paramMktRate)

                objcmd.Connection = conn
                objcmd.ExecuteNonQuery()
            End If
            'Util.WriteDebugLog("Update complete PLCAl in DB")
        Catch ex As Exception
            Util.WriteDebugLog("UpdatePLCal:" + ex.Message + ex.StackTrace)
        Finally
            conn.Close()
        End Try
    End Sub

    Public Function ReturnNull(ByVal value As String) As Object
        Dim objReturn As Object
        If value = String.Empty Then
            objReturn = DBNull.Value
        Else
            objReturn = value
        End If
        Return objReturn
    End Function

    Public Function loadPLCal() As DataSet
        SyncLock Me
            Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
            Try
                conn.Open()
                Dim sql As String = "Select * from PLCal"
                sql = sql + " Order by Symbol "
                Dim ds As DataSet = New DataSet
                Dim n As Integer = New OleDbDataAdapter(sql, conn).Fill(ds, "PLCal")

                Return ds
            Catch ex As Exception
                Throw ex
            Finally
                conn.Close()
            End Try
        End SyncLock
    End Function

    Public Function GetRealizedPIPS(ByVal symbol As String, ByVal dsPLTrade As DataSet, ByVal senderID As String) As Decimal
        Dim filter As String = ""
        If (symbol <> "") Then
            filter = "Symbol = '" + symbol + "' AND SenderID = '" + senderID + "'" 'AND ServerDateTime >= " + tradingdate.Date.Date.ToOADate().ToString() + " AND ServerDateTime < " + tradingdate.Date.Date.AddDays(1).ToOADate().ToString()
        End If
        Dim tRPips As Decimal = 0
        tRPips = CDec(dsPLTrade.Tables(0).Compute("Sum(Pips)", filter))
        Return Decimal.Round(CDec(tRPips), 2)
    End Function

    Public Function GetUnRealizedPIPS(ByVal symbol As String, ByVal currentBidPrice As Double, ByVal dsPLTrade As DataSet, ByVal senderID As String) As Decimal
        Dim filter As String = ""
        If (symbol <> "") Then
            filter = "Actions = 1 AND Remaining > 0 AND Symbol = '" + symbol + "' AND SenderID = '" + senderID + "'" 'AND ServerDateTime >= " + tradingdate.Date.Date.ToOADate().ToString() + " AND ServerDateTime < " + tradingdate.Date.Date.AddDays(1).ToOADate().ToString()
        End If
        dsPLTrade.Tables(0).Select(filter)
        Dim drs() As DataRow = dsPLTrade.Tables(0).Select(filter, "DateID")
        Dim totalPips As Decimal = 0
        For Each dr As DataRow In drs
            totalPips = totalPips + (CDec(dr("Remaining")) * (currentBidPrice - CDec(dr("Price"))))
        Next
        Return Decimal.Round(totalPips, 2)

    End Function

    Public Function GetRealizedPIPSBaseCurrency(ByVal symbol As String, ByVal dsPLTrade As DataSet, ByVal senderID As String) As Decimal
        Dim filter As String = ""
        If (symbol <> "") Then
            filter = "Symbol = '" + symbol + "' AND SenderID = '" + senderID + "'" 'AND ServerDateTime >= " + tradingdate.Date.Date.ToOADate().ToString() + " AND ServerDateTime < " + tradingdate.Date.Date.AddDays(1).ToOADate().ToString()
        End If
        Dim tRPips As Decimal = 0
        tRPips = CDec(dsPLTrade.Tables(0).Compute("Sum(PipsBaseCurrency)", filter))
        Return Decimal.Round(CDec(tRPips), 2)
    End Function

    Public Function GetUnRealizedPIPSBaseCurrency(ByVal symbol As String, ByVal currentBidPrice As Double, ByVal dsPLTrade As DataSet, ByVal senderID As String, ByVal dsMarketData As DataSet) As Decimal
        Dim filter As String = ""
        Dim firstcurrency, secondcurrency, basecurrency As String
        basecurrency = SettingsHome.getInstance().BaseCurrency
        firstcurrency = EServerDependents.GetFirstCurrency(symbol)
        secondcurrency = EServerDependents.GetSecondCurrency(symbol)

        If (symbol <> "") Then
            filter = "Actions = 1 AND Remaining > 0 AND Symbol = '" + symbol + "' AND SenderID = '" + senderID + "'" 'AND ServerDateTime >= " + tradingdate.Date.Date.ToOADate().ToString() + " AND ServerDateTime < " + tradingdate.Date.Date.AddDays(1).ToOADate().ToString()
        End If
        dsPLTrade.Tables(0).Select(filter)
        Dim drs() As DataRow = dsPLTrade.Tables(0).Select(filter, "DateID")
        Dim pipsBaseCurrency As Decimal = 0
        For Each dr As DataRow In drs
            'base currency Pips calcullation
            If (firstcurrency = basecurrency) Then 'case 1 first currency is base currency
                pipsBaseCurrency = pipsBaseCurrency + (CDec(dr("Remaining")) * (currentBidPrice - CDec(dr("Price")))) / CDec(dr("Price"))
            ElseIf (secondcurrency = basecurrency) Then 'case 2 second currency is base currency
                pipsBaseCurrency = pipsBaseCurrency + (CDec(dr("Remaining")) * (currentBidPrice - CDec(dr("Price"))))
            Else 'case 3 none of the currency is base currency
                pipsBaseCurrency = pipsBaseCurrency + (CDec(dr("Remaining")) * (currentBidPrice - CDec(dr("Price")))) / GetBidPriceBySymbol(dsMarketData, EServerDependents.GetCombinedCurrency(basecurrency, secondcurrency))
            End If

        Next
        Return Decimal.Round(pipsBaseCurrency, 2)

    End Function

    Public Function GetBidPriceBySymbol(ByVal dsMarketdata As DataSet, ByVal symbol As String) As Double
        'Thread.CurrentThread.CurrentCulture = SettingsHome.getInstance().Culture
        Dim dv As DataView = dsMarketdata.Tables(0).DefaultView
        dv.RowFilter = "Symbol = '" + symbol + "'"
        If (dv.Count > 0) Then
            Return CDbl(dv(0)("BidPrice"))
        Else
            Return -1
        End If
    End Function
    Public Function GetOfferPriceBySymbol(ByVal dsMarketdata As DataSet, ByVal symbol As String) As Double
        Dim dv As DataView = dsMarketdata.Tables(0).DefaultView
        dv.RowFilter = "Symbol = '" + symbol + "'"
        Return CDbl(dv(0)("OfferPrice"))
    End Function

    Public Function GetSymbolsTradedByAccount(ByVal accountID As String, ByVal dsPLTrade As DataSet) As Hashtable
        Dim filter As String = ""
        filter = "SenderID = '" + accountID + "'" '"ServerDateTime >= " + tradingdate.Date.Date.ToOADate().ToString() + " AND ServerDateTime < " + tradingdate.Date.Date.AddDays(1).ToOADate().ToString()
        Dim drs() As DataRow = dsPLTrade.Tables(0).Select(filter)
        Return SelectDistinct(drs, "Symbol")
    End Function

    Public Function GetDistinctSymbolPlTrade() As DataSet
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Try
            conn.Open()
            Dim sql As String = "Select Distinct Symbol from PLTrade"
            Dim ds As DataSet = New DataSet
            Dim n As Integer = New OleDbDataAdapter(sql, conn).Fill(ds, "PLTrade")
            Return ds
        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()
        End Try
    End Function

    Public Function IsDuplateAlert(ByVal st As AlertsManager.NewAlert) As Boolean
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Try
            conn.Open()

            Dim sql As String = "SELECT * FROM Alerts WHERE SYMBOL = '" + st.symbol.ToString() + "' AND Contracts = " + st.contracts.ToString() + " AND ActionType = " + st.actiontype.ToString() + " AND TimeStamp =  '" + st.timestamp + "' AND ChartIdentifier = " + st.chartIdentifier.ToString()
            Dim ds As DataSet = New DataSet
            Dim n As Integer = New System.Data.OleDb.OleDbDataAdapter(sql, conn).Fill(ds, "Alerts")
            If (n > 0) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()
        End Try
    End Function

    Public Function GetTradeCurrencyBySymbol(ByVal symbol As String) As String
        Dim dv As DataView = getSymbolMap().Tables(0).DefaultView
        dv.RowFilter = "TradeSymbol = '" + symbol + "'"
        Return CStr(dv(0)("TradeCurrency"))
    End Function

    Public Function GetUSDMarketPrice(ByVal symbol As String) As Double 'Market price to calculate the RelaizedUSD in Admin

        Thread.CurrentThread.CurrentCulture = SettingsHome.getInstance().Culture
        Dim dsMarketData As DataSet = Form1.GetSingletonOrderform().dsMarkeData.Copy()
        Dim firstcurrency, secondcurrency, basecurrency As String
        basecurrency = SettingsHome.getInstance().BaseCurrency
        firstcurrency = EServerDependents.GetFirstCurrency(symbol)
        secondcurrency = EServerDependents.GetSecondCurrency(symbol)
        Try
            If (firstcurrency = basecurrency) Then 'case 1 first currency is base currency
                Return GetBidPriceBySymbol(dsMarketData, symbol)
            ElseIf (secondcurrency = basecurrency) Then 'case 2 second currency is base currency
                Return 1.0
            Else 'case 3 none of the currency is base currency
                Dim price As Double = 0
                price = GetBidPriceBySymbol(dsMarketData, EServerDependents.GetCombinedCurrency(basecurrency, secondcurrency))
                If (price <> -1) Then
                    Return price
                Else
                    price = GetBidPriceBySymbol(dsMarketData, EServerDependents.GetCombinedCurrency(secondcurrency, basecurrency))
                    If (price <> -1) Then
                        Return (1 / price)
                    Else
                        Return 1.0
                    End If
                End If
            End If
        Catch ex As Exception
            Util.WriteDebugLog(ex.Message + ex.StackTrace)
            Return 1.0
        End Try

    End Function

    Public Shared Sub DumpMDtoDB()
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        conn.Open()
        Dim dsDumpMarketData As DataSet = Form1.GetSingletonOrderform().dsMarkeData.Copy()
        Try
            For Each dr As DataRow In dsDumpMarketData.Tables(0).Rows
                Dim BidPrice As String
                Dim OfferPrice As String
                Dim Symbol As String
                Symbol = dr("Symbol").ToString()
                BidPrice = dr("BidPrice").ToString()
                OfferPrice = dr("OfferPrice").ToString()
                Dim sql As String
                sql = "Update MarketData Set BidPrice = " + BidPrice + ", OfferPrice = " + OfferPrice + ", TimeStamps = '" + DateTime.Now.ToString(SettingsHome.getInstance().Culture) + "' where Symbol = '" + Symbol + "'"
                Dim cmd As OleDbCommand = New OleDbCommand(sql, conn)
                Dim n As Integer = cmd.ExecuteNonQuery
                If (n < 1) Then
                    sql = "INSERT INTO MarketData (Symbol,BidPrice,OfferPrice,TimeStamps) VALUES ('" + Symbol + "'," + BidPrice + "," + OfferPrice + ",'" + DateTime.Now.ToString(SettingsHome.getInstance().Culture) + "')"
                    cmd = New OleDbCommand(sql, conn)
                    n = cmd.ExecuteNonQuery
                End If
            Next
        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()
        End Try
    End Sub

    Public Sub UpdateActiveSymbols(ByVal symbol As String)
        Dim conn As OleDbConnection = New OleDbConnection(m_datasourcename)
        Try
            Dim sql As String
            'UPDATE SymbolMap SET Active = Yes WHERE TradeSymbol='EUR/AUD';
            sql = "UPDATE SymbolMap SET Active = Yes WHERE TradeSymbol = '" + symbol + "'"
            conn.Open()
            Dim cmd As OleDbCommand = New OleDbCommand(sql, conn)
            Dim n As Integer = cmd.ExecuteNonQuery
            conn.Close()
        Catch ex As Exception
            Util.WriteDebugLog(ex.Message + ex.StackTrace)
        End Try
    End Sub

#Region "CG Added Code"

    'Public Function GetNetCC1(ByVal symbol As String, ByVal dsPLTrade As DataSet, ByVal senderID As String, ByVal sysName As String, ByVal sysId As String) As Decimal
    Public Function GetNetCC1(ByVal dsPLTrade As DataSet, ByVal signal As AlertsManager.NewAlert, ByVal sel As Boolean) As Decimal
        Try
            Dim filter As String = ""
            If (sel) Then
                filter = "Symbol = '" + signal.symbol + "' AND SenderID = '" + signal.senderID + "' AND Actions = 1  AND ExecOrderId = '" + signal.execOrderId + "' AND SystemID= '" + signal.month_year + "'"
            Else
                filter = "Symbol = '" + signal.symbol + "' AND SenderID = '" + signal.senderID + "' AND Actions = 1 "
            End If
            Dim totalBuyAmount As Decimal = 0
            Dim drs() As DataRow
            drs = dsPLTrade.Tables(0).Select(filter)
            If drs.Length <> 0 Then totalBuyAmount = CDec(dsPLTrade.Tables(0).Compute("Sum(Amount)", filter))
            Dim totalSellAmount As Decimal = 0
            If (sel) Then
                filter = "Symbol = '" + signal.symbol + "' AND SenderID = '" + signal.senderID + "' AND Actions = 2  AND ExecOrderId = '" + signal.execOrderId + "' AND SystemID= '" + signal.month_year + "'"
            Else
                filter = "Symbol = '" + signal.symbol + "' AND SenderID = '" + signal.senderID + "' AND Actions = 2 "
            End If
            drs = dsPLTrade.Tables(0).Select(filter)
            If drs.Length <> 0 Then totalSellAmount = CDec(dsPLTrade.Tables(0).Compute("Sum(Amount)", filter))
            Return (totalBuyAmount - totalSellAmount)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ' Public Function GetRemainingBySide(ByVal side As Integer, ByVal symbol As String, ByVal dsPLTrade As DataSet, ByVal senderID As String, ByVal sysName As String, ByVal sysId As String) As Decimal
    Public Function GetRemainingBySide(ByVal signal As AlertsManager.NewAlert, ByVal dsPLTrade As DataSet, ByVal side As Integer, ByVal sel As Boolean) As Decimal
        Dim filter As String = ""
        'If (signal.symbol <> "") Then
        If (sel) Then
            filter = "Actions = " & side & " AND RemainingSys > 0 AND Symbol = '" + signal.symbol + "' AND SenderID = '" + signal.senderID + "' AND ExecOrderId = '" + signal.execOrderId + "' AND SystemID= '" + signal.month_year + "'"
        Else
            filter = "Actions = " & side & " AND Remaining > 0 AND Symbol = '" + signal.symbol + "' AND SenderID = '" + signal.senderID + "'"
        End If

        'End If
        dsPLTrade.Tables(0).Select(filter)
        Dim drs() As DataRow = dsPLTrade.Tables(0).Select(filter, "DateID")
        Dim remaining As Integer = 0
        For Each dr As DataRow In drs
            If (sel) Then
                remaining += CInt(dr("RemainingSys"))
            Else
                remaining += CInt(dr("Remaining"))
            End If

        Next
        Return remaining
    End Function

    Private Sub ModifyRemaining(ByVal diff As Integer, ByVal side As Integer, ByVal signal As AlertsManager.NewAlert, ByRef dsPLTrade As DataSet, ByVal conn As OleDbConnection, ByVal sel As Boolean)

        Try


            Dim filter As String
            If (sel) Then
                filter = "Actions = " & side.ToString & " AND RemainingSys > 0 AND Symbol = '" + signal.symbol + "' AND SenderID = '" + signal.senderID + "' AND ExecOrderId = '" + signal.execOrderId + "' AND SystemID= '" + signal.month_year + "'"
            Else
                filter = "Actions = " & side.ToString & " AND Remaining > 0 AND Symbol = '" + signal.symbol + "' AND SenderID = '" + signal.senderID + "'"
            End If

            Dim drs() As DataRow = dsPLTrade.Tables(0).Select(filter, "DateID")
            Dim sql As String
            Dim cmd As OleDbCommand
            Dim drPL() As DataRow

            Dim n As Integer
            If (drs.Length > 0) Then
                For Each dr As DataRow In drs
                    Dim adjust As Integer
                    Dim remaining As Integer
                    Dim remainingAmount As Integer
                    Dim pips As Double
                    If (sel) Then
                        adjust = Math.Max(0, CInt(dr("RemainingSys")) - diff)
                        remaining = CInt(dr("RemainingSys"))
                        pips = dr("PipSys")
                    Else
                        adjust = Math.Max(0, CInt(dr("Remaining")) - diff)
                        remaining = CInt(dr("Remaining"))
                        pips = dr("Pips")
                    End If

                    Dim pipsBaseCurrency As Double
                    If (diff > remaining) Then
                        diff = diff - remaining

                        Dim diffPrice As Decimal
                        diffPrice = CDec(dr("Price")) - CDec(signal.price)
                        diffPrice = diffPrice * remaining
                        pips = pips + diffPrice
                        pipsBaseCurrency = pips
                        remaining = 0
                    Else
                        remaining = remaining - diff
                        Dim diffPrice As Decimal
                        diffPrice = CDec(dr("Price")) - CDec(signal.price)
                        diffPrice = diffPrice * diff
                        pips = pips + diffPrice

                        remainingAmount = 0

                        pipsBaseCurrency = pips

                        'Sql = "Update PLTrade Set Remaining = " + CStr(remaining) + ", Pips = " + CStr(pips) + ", PipsBaseCurrency= " + CStr(pipsBaseCurrency) + " where RowID = " + CStr(dr("RowID"))
                        If (sel) Then
                            sql = "Update PLTrade Set RemainingSys = " + CStr(remaining) + ", PipSys = " + CStr(pips) + " where DateID = " + CStr(dr("DateID"))
                        Else
                            sql = "Update PLTrade Set Remaining = " + CStr(remaining) + ", Pips = " + CStr(pips) + ", PipsBaseCurrency= " + CStr(pipsBaseCurrency) + " where DateID = " + CStr(dr("DateID"))
                        End If

                        cmd = New OleDbCommand(sql, conn)
                        n = cmd.ExecuteNonQuery

                        drPL = dsPLTrade.Tables(0).Select("DateID = " + CStr(dr("DateID")))
                        If (drPL.Length > 0) Then
                            If (sel) Then
                                drPL(0)("RemainingSys") = CStr(remaining)
                                drPL(0)("PipSys") = CStr(pips)
                            Else
                                drPL(0)("Remaining") = CStr(remaining)
                                drPL(0)("Pips") = CStr(pips)
                                drPL(0)("PipsBaseCurrency") = CStr(pipsBaseCurrency)
                            End If

                        Else
                            Util.WriteDebugLog("ADD PLTrade : No row to update")
                        End If
                        Exit For
                    End If
                    'update the dr here
                    If (sel) Then
                        sql = "Update PLTrade Set RemainingSys = " + CStr(remaining) + ", PipSys = " + CStr(pips) + " where DateID = " + CStr(dr("DateID"))
                    Else
                        sql = "Update PLTrade Set Remaining = " + CStr(remaining) + ", Pips = " + CStr(pips) + ", PipsBaseCurrency= " + CStr(pipsBaseCurrency) + " where DateID = " + CStr(dr("DateID"))
                    End If

                    cmd = New OleDbCommand(sql, conn)
                    n = cmd.ExecuteNonQuery

                    drPL = dsPLTrade.Tables(0).Select("DateID = " + CStr(dr("DateID")))
                    If (drPL.Length > 0) Then
                        If (sel) Then
                            drPL(0)("RemainingSys") = CStr(remaining)
                            drPL(0)("PipSys") = CStr(pips)
                        Else
                            drPL(0)("Remaining") = CStr(remaining)
                            drPL(0)("Pips") = CStr(pips)
                            drPL(0)("PipsBaseCurrency") = CStr(pipsBaseCurrency)
                        End If

                    Else
                        Util.WriteDebugLog("ADD PLTrade : No row to update")
                    End If
                Next
            End If

        Catch ex As Exception
            Util.WriteDebugLog(ex.Message + ex.StackTrace)
        End Try

    End Sub

#End Region

End Class
