Imports System.Threading
'Imports TradingInterface.DBFXExecution
Public Class ManualAlert
    Inherits System.Windows.Forms.Form
    Public alertdata As AlertsManager.NewAlert
    Private Const CANCEL As Integer = 0
    Private tradeType As Integer
    Dim ds As DataSet
    Dim dsActiveSymbol As DataSet
    Dim ah As AlertsHome = New AlertsHome

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Dim obj As Object
        Dim s As Trader
        Dim i As Integer
        Dim iEnum As IDictionaryEnumerator
        iEnum = Form1.GetSingletonOrderform.ConnectHT.GetEnumerator()

        While iEnum.MoveNext()
            obj = iEnum.Value
            s = CType(obj, Trader)
            If ((Not s.IsMarketDataConnection) Or (SettingsHome.getInstance().ExchangeServer <> ExchangeServer.CurrenEx _
                    And SettingsHome.getInstance().ExchangeServer <> ExchangeServer.Dukascopy And _
                        SettingsHome.getInstance().ExchangeServer <> ExchangeServer.FxIntegral)) Then
                If s.Stat = Trader.ConnectionStatus.CONNECTED Then cmbSenderID.Items.Add(s.ConnectionId) 'Exsit before
            End If
        End While


        ' Add any initialization after the InitializeComponent() call.
        ds = ah.getSymbolMap()
        Dim tbl As DataTable = ah.getActions
        ds.Tables.Add(tbl)

        dsActiveSymbol = ah.getActiveSymbolDataSet()
        cmbSymbol.DataSource = dsActiveSymbol.Tables("SymbolMap")
        cmbSymbol.DisplayMember = "TradeSymbol"
        cmbSymbol.ValueMember = "SymbolID"

        cmbSymbol.Refresh()
        'dynamically adding the Quantity to the combobox, 
        'with different server has the different minimumsize(for ICAP it is 100000 minimumsize)
        Dim drc() As DataRow = ds.Tables("SymbolMap").Select("TradeSymbol = '" + cmbSymbol.Text + "'")
        Dim minimumQantity As Integer
        If (drc.Length > 0) Then
            minimumQantity = (drc(0)("TradeSize"))
        End If
        i = 1
        While (i <= 10)
            cmbQuantity.Items.Add(i * minimumQantity)
            i += 1
        End While


        rbSell.Checked = True
        'only in the ICAP case showing the price.
        If (SettingsHome.getInstance().ExchangeServer = ExchangeServer.Icap) Then
            txtPrice.Visible = True
            lblPrice.Visible = True
            txtPrice.Text = ah.GetBidPriceBySymbol(cmbSymbol.Text.Trim())
        End If


        'Add a blank row.
        cmbTradeType.DataSource = ds.Tables("TradeType")
        cmbTradeType.DisplayMember = "Name"
        cmbTradeType.ValueMember = "TradeType"
        cmbTradeType.Refresh()
        Dim sTradeType As New SettingsTrade
        sTradeType.getSettings()
        cmbTradeType.DropDownStyle = ComboBoxStyle.DropDownList
        cmbSymbol.DropDownStyle = ComboBoxStyle.DropDownList
        cmbSenderID.DropDownStyle = ComboBoxStyle.DropDownList
        Select Case sTradeType.TradeTypeManual
            Case 1 'GTC
                cmbTradeType.SelectedValue = 1
            Case 3 'IOC
                cmbTradeType.SelectedValue = 3
        End Select
    End Sub
    Public Sub New(ByVal symbol As String)
        Me.New()
        cmbSymbol.Text = symbol.Trim()
    End Sub

    Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        Try

            If ValidateField() Then
                If (cmbSymbol.Text.Trim = "[ALL]") Then
                    Me.Hide()
                    alertdata = New AlertsManager.NewAlert
                    alertdata.senderID = cmbSenderID.Text.Trim()
                    alertdata.contracts = CType(cmbQuantity.Text.Trim(), Double)
                    alertdata.tradeType = CInt(cmbTradeType.SelectedValue)
                    If (rbBuy.Checked) Then
                        alertdata.actiontype = AlertsManager.ACTION_BUY
                    Else
                        alertdata.actiontype = AlertsManager.ACTION_SELL
                    End If
                    alertdata.timestamp = "simulated"

                    Dim dvActiveSymbol As DataView = dsActiveSymbol.Tables(0).DefaultView
                    dvActiveSymbol.RowFilter = "TradeSymbol<>'[ALL]'"
                    Dim symbArray As New ArrayList
                    Dim i As Integer = 0
                    While ((dvActiveSymbol.Count > 0) And (i < dvActiveSymbol.Count))
                        If (symbArray.Contains(dvActiveSymbol(i)("TradeSymbol"))) Then
                        Else
                            alertdata.symbol = dvActiveSymbol(i)("TradeSymbol")
                            alertdata.currency = dvActiveSymbol(i)("TradeCurrency")
                            Form1.GetSingletonOrderform().watcher_NewAlert(alertdata)
                            symbArray.Add(dvActiveSymbol(i)("TradeSymbol"))
                        End If
                        Thread.Sleep(1000)
                        i = i + 1
                    End While
                    alertdata = Nothing
                Else
                    alertdata = New AlertsManager.NewAlert
                    alertdata.chartIdentifier = -1
                    alertdata.symbol = cmbSymbol.Text.Trim
                    alertdata.contracts = CType(cmbQuantity.Text.Trim(), Double)
                    alertdata.senderID = cmbSenderID.Text.Trim() 'this will send the connection id for trading
                    If (rbBuy.Checked) Then
                        alertdata.actiontype = AlertsManager.ACTION_BUY
                    Else
                        alertdata.actiontype = AlertsManager.ACTION_SELL
                    End If
                    alertdata.currency = TxtCurrency.Text
                    alertdata.tradeType = CInt(cmbTradeType.SelectedValue)
                End If
            Else
                MsgBox("All Fields are Mandatory.", 0, "Alert")
            End If

        Catch ex As Exception
            Util.WriteDebugLog(ex.Message + ex.StackTrace)
        End Try
    End Sub

    Private Sub cmbSymbol_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSymbol.SelectedValueChanged
        Try
            Dim drc() As DataRow = dsActiveSymbol.Tables("SymbolMap").Select("TradeSymbol = '" + cmbSymbol.Text + "'")
            If (drc.Length > 0) Then
                cmbQuantity.Text = drc(0)("TradeSize").ToString()
                TxtCurrency.Text = drc(0)("TradeCurrency")
                If (rbSell.Checked) Then
                    txtPrice.Text = ah.GetBidPriceBySymbol(cmbSymbol.Text.Trim())
                Else
                    txtPrice.Text = ah.GetAskPriceBySymbol(cmbSymbol.Text.Trim())
                End If
            End If
        Catch ex As Exception
            Util.WriteDebugLog(ex.Message + ex.StackTrace)
        End Try
    End Sub

    Private Function ValidateField() As Boolean
        If cmbSenderID.Text.Trim() = "" Or cmbSymbol.Text.Trim() = "" Or Not IsNumeric(cmbQuantity.Text) Or cmbTradeType.Text.Trim() = "" Then
            Return False
        Else
            Return True
        End If
    End Function

    Private Sub btnAccept_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccept.GotFocus
        If ValidateField() Then
            Me.btnAccept.DialogResult = System.Windows.Forms.DialogResult.OK
        End If
    End Sub

    Private Sub rbBuy_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbBuy.CheckedChanged
        If (rbBuy.Checked) Then
            txtPrice.Text = ah.GetAskPriceBySymbol(cmbSymbol.Text.Trim())
        End If
    End Sub

    Private Sub rbSell_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbSell.CheckedChanged
        If (rbSell.Checked) Then
            txtPrice.Text = ah.GetBidPriceBySymbol(cmbSymbol.Text.Trim())
        End If
    End Sub

    Private Sub cmbQuantity_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbQuantity.KeyPress
        'validating the input is numeric or not.
        If (Not IsNumeric(e.KeyChar) And (e.KeyChar <> Chr(8))) Then
            e.KeyChar = ""
        End If
    End Sub
End Class
