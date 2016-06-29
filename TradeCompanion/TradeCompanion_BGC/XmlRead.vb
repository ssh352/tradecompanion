'Imports Microsoft.Win32
'Imports System.Threading
'Public Class XmlRead
'    'this class is used to check crash and automate the process 
'    Public dsCrash As DataSet = New DataSet()
'    Dim key As Microsoft.Win32.RegistryKey
'    Dim keys As SettingsHome
'    Dim xPath As String = Application.StartupPath() + "\settings.xml"
'    Public Sub New()
'        dsCrash.ReadXmlSchema(Application.StartupPath() + "\settings.xsd")
'        If IO.File.Exists(xPath) Then
'            dsCrash.ReadXml(xPath)
'        End If
'    End Sub
'    Public Function CrashTest() As Boolean
'        'testing for crash
'        Dim temp As Boolean = False
'        If dsCrash.Tables("settings").Rows.Count > 0 Then
'            If dsCrash.Tables("settings").Rows(0).Item("crash").ToString().Trim() = "true" Then temp = True
'        End If
'        Return temp
'    End Function
'    Public Sub Perform()
'        'porforming the auto login to TC & setting the last settings before crash from xml data
'        Dim dlg As New LoginForm1
'        dlg.Show()
'        Dim j As Integer = dsCrash.Tables("settings").Rows(0).Item("server").ToString().Trim()
'        j = j - 1
'        dlg.UsernameTextBox.Text = dsCrash.Tables("settings").Rows(0).Item("loginid")
'        dlg.PasswordTextBox.Text = dsCrash.Tables("settings").Rows(0).Item("password")
'        dlg.CmbServer.SelectedIndex = j
'        dlg.OK.PerformClick()
'        Dim d As DialogResult = dlg.DialogResult
'        If d = DialogResult.OK Then
'            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\BGC\\Settings")
'            key.SetValue("Registered", Util.Encrypt("true", True))
'            dlg.Close()
'            Dim frm As New Form1
'            keys = SettingsHome.getInstance()
'            If Not IsDBNull(dsCrash.Tables("settings").Rows(0).Item("symbolmaping")) Then
'                If dsCrash.Tables("settings").Rows(0).Item("symbolmaping") = True Then
'                    keys.UseSymbolMapping = True
'                Else
'                    keys.UseSymbolMapping = False
'                End If
'            End If
'            If Not IsDBNull(dsCrash.Tables("settings").Rows(0).Item("defaulttradesize")) Then
'                If dsCrash.Tables("settings").Rows(0).Item("defaulttradesize") = True Then
'                    keys.UseDefaultTradeSize = True
'                Else
'                    keys.UseDefaultTradeSize = False
'                End If
'            End If
'            If Not IsDBNull(dsCrash.Tables("settings").Rows(0).Item("filteralerts")) Then
'                If dsCrash.Tables("settings").Rows(0).Item("filteralerts") = True Then
'                    keys.FilterAlerts = True
'                Else
'                    keys.FilterAlerts = False
'                End If
'            End If
'            If Not IsDBNull(dsCrash.Tables("settings").Rows(0).Item("reconnectinterval")) Then
'                keys.reconnectInterval = dsCrash.Tables("settings").Rows(0).Item("reconnectinterval")
'            End If
'            If (dsCrash.Tables("manual").Rows.Count > 0) Then
'                Dim settrade As SettingsTrade = New SettingsTrade()
'                settrade.TradeTypeAuto = dsCrash.Tables("automatic").Rows(0).Item("defaulttradetype")
'                settrade.TradeTypeManual = dsCrash.Tables("manual").Rows(0).Item("defaulttradetype")
'                settrade.TradeTypeAutoOver10Mil = dsCrash.Tables("automatic").Rows(0).Item("tradesizeover10m")
'                settrade.setSettings()
'            End If
'            If Not IsDBNull(dsCrash.Tables("settings").Rows(0).Item("platformforgettingalerts")) Then
'                keys.Platform = dsCrash.Tables("settings").Rows(0).Item("platformforgettingalerts")
'            End If
'            Try
'                frm.ShowDialog()
'            Catch ex As Exception
'                Util.WriteDebugLog(ex.Message() + ex.StackTrace())
'            End Try
'        End If
'    End Sub
'    Public Function Autoconnect() As Boolean
'        'auto connect the trade & market data
'        keys = SettingsHome.getInstance()
'        Dim serv As Integer = dsCrash.Tables("settings").Rows(0).Item("server").ToString().Trim()
'        Select Case serv
'            Case ExchangeServer.CurrenEx
'                If (dsCrash.Tables("connectiontrade").Rows.Count > 0) Then
'                    Dim i As Integer = 0
'                    Dim count As Integer = dsCrash.Tables("connectiontrade").Rows.Count()
'                    While i < count 'for multiple connections in currnex
'                        keys.CXLoginIP = dsCrash.Tables("connectiontrade").Rows(i).Item("param1").ToString().Trim()
'                        keys.CXLoginPort = dsCrash.Tables("connectiontrade").Rows(i).Item("param2").ToString().Trim()
'                        keys.CXLoginSender = dsCrash.Tables("connectiontrade").Rows(i).Item("param3").ToString().Trim()
'                        keys.CXLoginTarget = dsCrash.Tables("connectiontrade").Rows(i).Item("param4").ToString().Trim()
'                        Form1.GetSingletonOrderform().btnConnect.PerformClick()
'                        i = i + 1
'                    End While
'                End If
'                If (dsCrash.Tables("connectionmarket").Rows.Count > 0) Then
'                    keys.LoginIPMarketData = dsCrash.Tables("connectionmarket").Rows(0).Item("param1").ToString().Trim()
'                    keys.LoginPortMarketData = dsCrash.Tables("connectionmarket").Rows(0).Item("param2").ToString().Trim()
'                    keys.LoginSenderMarketData = dsCrash.Tables("connectionmarket").Rows(0).Item("param3").ToString().Trim()
'                    keys.LoginTargetMarketData = dsCrash.Tables("connectionmarket").Rows(0).Item("param4").ToString().Trim()
'                    Form1.GetSingletonOrderform().btnSubscribeMarketData.PerformClick()
'                End If
'            Case ExchangeServer.Ariel
'                If (dsCrash.Tables("connectiontrade").Rows.Count > 0) Then
'                    keys.ArielLoginUserName = dsCrash.Tables("connectiontrade").Rows(0).Item("param1").ToString().Trim()
'                    keys.ArielLoginUserID = dsCrash.Tables("connectiontrade").Rows(0).Item("param2").ToString().Trim()
'                    keys.ArielLoginPassword = dsCrash.Tables("connectiontrade").Rows(0).Item("param3").ToString().Trim()
'                    Form1.GetSingletonOrderform().btnConnect.PerformClick()
'                End If
'            Case ExchangeServer.Espeed
'                If (dsCrash.Tables("connectiontrade").Rows.Count > 0) Then
'                    keys.EspeedLoginIP = dsCrash.Tables("connectiontrade").Rows(0).Item("param1").ToString().Trim()
'                    keys.EspeedLoginPort = dsCrash.Tables("connectiontrade").Rows(0).Item("param2").ToString().Trim()
'                    keys.EspeedLoginUsername = dsCrash.Tables("connectiontrade").Rows(0).Item("param3").ToString().Trim()
'                    keys.EspeedLoginPassword = dsCrash.Tables("connectiontrade").Rows(0).Item("param4").ToString().Trim()
'                    Form1.GetSingletonOrderform().btnConnect.PerformClick()
'                End If
'        End Select
'        'set the execution mode 
'        If Not IsDBNull(dsCrash.Tables("settings").Rows(0).Item("executionmode")) Then
'            Dim j As Integer = dsCrash.Tables("settings").Rows(0).Item("executionmode")
'            Select Case j
'                Case 0
'                    Form1.GetSingletonOrderform().rbNoExecution.Checked = True
'                Case 1
'                    Form1.GetSingletonOrderform().rbManual.Checked = True
'                Case 2
'                    Form1.GetSingletonOrderform().rbAuto.Checked = True
'            End Select
'        End If
'        If serv <> ExchangeServer.CurrenEx Then
'            If (dsCrash.Tables("connectionmarket").Rows.Count > 0) Then
'                Return True
'            End If
'        End If
'        Return False
'    End Function
'    Public Sub UpdateSettings(ByVal str As String)
'        keys = SettingsHome.getInstance()
'        Select Case str 'upadateing settings to xml
'            Case "executionmode"
'                dsCrash.Tables("settings").Rows(0).Item("executionmode") = keys.ExecutionMode
'            Case "filteralerts"
'                dsCrash.Tables("settings").Rows(0).Item("filteralerts") = keys.FilterAlerts
'            Case "defaulttradesize"
'                dsCrash.Tables("settings").Rows(0).Item("defaulttradesize") = keys.UseDefaultTradeSize
'            Case "symbolmaping"
'                dsCrash.Tables("settings").Rows(0).Item("symbolmaping") = keys.UseSymbolMapping
'            Case "platform"
'                dsCrash.Tables("settings").Rows(0).Item("platformforgettingalerts") = keys.Platform
'        End Select
'        dsCrash.WriteXml(xPath)
'    End Sub
'    Public Sub AddTradesettings() 'adding new trade connection to xml
'        keys = SettingsHome.getInstance()
'        If (dsCrash.Tables("connections").Rows.Count = 0) Then
'            Dim dr As DataRow = dsCrash.Tables("connections").NewRow()
'            dr("settings_Id") = 0
'            dr("connections_Id") = 0
'            dsCrash.Tables("connections").Rows.Add(dr)
'        End If
'        If (dsCrash.Tables("trade").Rows.Count = 0) Then
'            Dim dr1 As DataRow = dsCrash.Tables("trade").NewRow()
'            dr1("connections_Id") = 0
'            dr1("trade_Id") = 0
'            dsCrash.Tables("trade").Rows.Add(dr1)
'        End If
'        Select Case keys.ExchangeServer
'            Case ExchangeServer.CurrenEx
'                Dim row As DataRow = dsCrash.Tables("connectiontrade").NewRow()
'                row("param1") = keys.CXLoginIP
'                row("param2") = keys.CXLoginPort
'                row("param3") = keys.CXLoginSender
'                row("param4") = keys.CXLoginTarget
'                row("Trade_Id") = 0 'foreigne key for the relation with the trade table of dataset
'                dsCrash.Tables("connectiontrade").Rows.Add(row)
'            Case ExchangeServer.Ariel
'                Dim row As DataRow = dsCrash.Tables("connectiontrade").NewRow()
'                row("param1") = keys.ArielLoginUserName
'                row("param2") = keys.ArielLoginUserID
'                row("param3") = keys.ArielLoginPassword
'                row("Trade_Id") = 0
'                dsCrash.Tables("connectiontrade").Rows.Add(row)
'            Case ExchangeServer.Espeed
'                Dim row As DataRow = dsCrash.Tables("connectiontrade").NewRow()
'                row("param1") = keys.EspeedLoginIP
'                row("param2") = keys.EspeedLoginPort
'                row("param3") = keys.EspeedLoginUsername
'                row("param4") = keys.EspeedLoginPassword
'                row("Trade_Id") = 0
'                dsCrash.Tables("connectiontrade").Rows.Add(row)
'        End Select
'        dsCrash.Tables("settings").Rows(0).Item("reconnectinterval") = keys.reconnectInterval
'        dsCrash.WriteXml(xPath)
'    End Sub
'    Public Sub Addmarketsettings() 'adding new marketdata connection to xml
'        If (dsCrash.Tables("connections").Rows.Count = 0) Then
'            Dim dr As DataRow = dsCrash.Tables("connections").NewRow()
'            dr("settings_Id") = 0
'            dr("connections_Id") = 0
'            dsCrash.Tables("connections").Rows.Add(dr)
'        End If
'        If (dsCrash.Tables("marketdata").Rows.Count = 0) Then
'            Dim dr1 As DataRow = dsCrash.Tables("marketdata").NewRow()
'            dr1("connections_Id") = 0
'            dr1("marketdata_Id") = 0
'            dsCrash.Tables("marketdata").Rows.Add(dr1)
'        End If
'        If dsCrash.Tables("connectionmarket").Rows.Count() = 0 Then
'            keys = SettingsHome.getInstance()
'            Select Case keys.ExchangeServer
'                Case ExchangeServer.CurrenEx
'                    Dim row As DataRow = dsCrash.Tables("connectionmarket").NewRow()
'                    row("param1") = keys.LoginIPMarketData
'                    row("param2") = keys.LoginPortMarketData
'                    row("param3") = keys.LoginSenderMarketData
'                    row("param4") = keys.LoginTargetMarketData
'                    row("marketdata_Id") = 0 'foreigne key for the relation with the trade table of dataset
'                    dsCrash.Tables("connectionmarket").Rows.Add(row)
'                Case ExchangeServer.Ariel
'                    Dim row As DataRow = dsCrash.Tables("connectionmarket").NewRow()
'                    row("param1") = keys.ArielLoginUserName
'                    row("param2") = keys.ArielLoginUserID
'                    row("param3") = keys.ArielLoginPassword
'                    row("marketdata_Id") = 0
'                    dsCrash.Tables("connectionmarket").Rows.Add(row)
'                Case ExchangeServer.Espeed
'                    Dim row As DataRow = dsCrash.Tables("connectionmarket").NewRow()
'                    row("param1") = keys.EspeedLoginIP
'                    row("param2") = keys.EspeedLoginPort
'                    row("param3") = keys.EspeedLoginUsername
'                    row("param4") = keys.EspeedLoginPassword
'                    row("marketdata_Id") = 0
'                    dsCrash.Tables("connectionmarket").Rows.Add(row)
'            End Select
'            dsCrash.WriteXml(xPath)
'        End If
'    End Sub
'    Public Sub Removetradesettings(ByVal str As String) 'removing new trade connection to xml
'        Dim count As Integer
'        Dim i As Integer = 0
'        Select Case SettingsHome.getInstance().ExchangeServer
'            Case ExchangeServer.CurrenEx
'                count = dsCrash.Tables("connectiontrade").Rows.Count()
'                While i < count
'                    If dsCrash.Tables("connectiontrade").Rows(i).Item("param3") = str Then
'                        dsCrash.Tables("connectiontrade").Rows.RemoveAt(i)
'                        Exit While
'                    End If
'                    i = i + 1
'                End While
'            Case ExchangeServer.Ariel
'                count = dsCrash.Tables("connectiontrade").Rows.Count()
'                While i < count
'                    If dsCrash.Tables("connectiontrade").Rows(i).Item("param1") = str Then
'                        dsCrash.Tables("connectiontrade").Rows.RemoveAt(i)
'                        If dsCrash.Tables("connectionmarket").Rows.Count() Then
'                            dsCrash.Tables("connectionmarket").Rows.RemoveAt(0)
'                        End If
'                    End If
'                    i = i + 1
'                End While
'            Case ExchangeServer.Espeed
'                count = dsCrash.Tables("connectiontrade").Rows.Count()
'                While i < count
'                    If dsCrash.Tables("connectiontrade").Rows(i).Item("param3") = str Then
'                        dsCrash.Tables("connectiontrade").Rows.RemoveAt(i)
'                        If dsCrash.Tables("connectionmarket").Rows.Count() Then
'                            dsCrash.Tables("connectionmarket").Rows.RemoveAt(0)
'                        End If
'                    End If
'                    i = i + 1
'                End While
'        End Select
'        dsCrash.WriteXml(xPath)
'    End Sub
'    Public Sub Crashupdate(ByVal str As String)
'        If dsCrash.Tables("settings").Rows.Count > 0 Then
'            dsCrash.Tables("settings").Rows(0).Item("crash") = str.Trim()
'        Else
'            Dim row As DataRow = dsCrash.Tables("settings").NewRow()
'            row("crash") = str
'            dsCrash.Tables("settings").Rows.Add(row)
'        End If
'        dsCrash.WriteXml(xPath)
'    End Sub
'    Public Sub LoginSettings(ByVal str As String) 'loginTC setting update to xml
'        keys = SettingsHome.getInstance()
'        If dsCrash.Tables("settings").Rows.Count > 0 Then
'            dsCrash.Tables("settings").Rows(0).Item("loginid") = keys.LoginidTC
'            dsCrash.Tables("settings").Rows(0).Item("password") = str
'            dsCrash.Tables("settings").Rows(0).Item("server") = keys.ExchangeServer
'        Else
'            Dim row As DataRow = dsCrash.Tables("settings").NewRow()
'            row("loginid") = keys.LoginidTC
'            row("password") = str
'            row("server") = keys.ExchangeServer
'            dsCrash.Tables("settings").Rows.Add(row)
'        End If
'        dsCrash.WriteXml(xPath)
'    End Sub
'    Public Sub RemoveMarketsettings()
'        If (dsCrash.Tables("connectionmarket").Rows.Count > 0) Then
'            dsCrash.Tables("connectionmarket").Rows.RemoveAt(0)
'            dsCrash.WriteXml(xPath)
'        End If
'    End Sub
'    Public Sub TradeSettings() 'updating the trade settings to xml
'        Dim settingsTrade As SettingsTrade = New SettingsTrade()
'        settingsTrade.getSettings()
'        If dsCrash.Tables("manual").Rows.Count > 0 Then
'            dsCrash.Tables("manual").Rows(0).Item("defaulttradetype") = settingsTrade.TradeTypeManual
'            dsCrash.Tables("automatic").Rows(0).Item("defaulttradetype") = settingsTrade.TradeTypeAuto
'            dsCrash.Tables("automatic").Rows(0).Item("tradesizeover10m") = settingsTrade.TradeTypeAutoOver10Mil
'            dsCrash.Tables("automatic").Rows(0).Item("discardalertInterval") = SettingsHome.getInstance().DiscardAlertInterval
'        Else
'            If (dsCrash.Tables("tradesettings").Rows.Count = 0) Then
'                Dim dr As DataRow = dsCrash.Tables("tradesettings").NewRow()
'                dr("tradesettings_Id") = 0
'                dr("settings_Id") = 0
'                dsCrash.Tables("tradesettings").Rows.Add(dr)
'            End If
'            Dim row As DataRow = dsCrash.Tables("manual").NewRow()
'            row("defaulttradetype") = settingsTrade.TradeTypeManual
'            row("tradesettings_Id") = 0
'            dsCrash.Tables("manual").Rows.Add(row)
'            Dim row1 As DataRow = dsCrash.Tables("automatic").NewRow()
'            row1("defaulttradetype") = settingsTrade.TradeTypeAuto
'            row1("tradesizeover10m") = settingsTrade.TradeTypeAutoOver10Mil
'            row1("discardalertInterval") = SettingsHome.getInstance().DiscardAlertInterval
'            row1("tradesettings_Id") = 0
'            dsCrash.Tables("automatic").Rows.Add(row1)
'        End If
'        dsCrash.WriteXml(xPath)
'    End Sub
'End Class
