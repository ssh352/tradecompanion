Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Globalization

Public Class SettingsHome

    Public Const EXECUTION_NONE As Integer = 0
    Public Const EXECUTION_MANUAL As Integer = 1
    Public Const EXECUTION_AUTO As Integer = 2
    Public ExecutionMode As Integer = EXECUTION_NONE
    Public UseDefaultTradeSize As Boolean = False
    Public UseSymbolMapping As Boolean = True
    Public FilterAlerts As Boolean = True

    Public CXLoginIP As String = "64.210.170.127"
    Public CXLoginPort As String = "443"
    Public CXLoginSender As String = "scalpercustu1trade"
    Public CXLoginPassword As String = ""
    Public CXLoginTarget As String = "CURRENEX-FXTRADES-FIX"

    'TODO Vm_ Fix
    Public DUKASLoginIP As String = ""
    Public DUKASLoginPort As String = ""
    Public DUKASLoginSender As String = ""
    Public DUKASLoginPassword As String = ""
    Public DUKASLoginTarget As String = ""
    Public DUKASLoginUserName As String = ""

    Public FxIntLoginIP As String = ""
    Public FxIntLoginPort As String = ""
    Public FxIntLoginSendercompId As String = ""
    Public FxIntLoginPassword As String = ""
    Public FxIntLoginTargetcompId As String = ""
    Public FxIntLoginUserName As String = ""
    Public FxIntLoginLegalEntity As String = ""


    Public DBFXUserName As String = "trader"
    Public DBFXPassword As String = "trader"
    Public DBFXURL As String = "http://www.fxcorporate.com/Hosts.jsp"
    Public DBFXAccountType As String = "Demo"

    Public GainUserName As String = "scalper@gain.com"
    Public GainPassword As String = "123456"
    Public GainBrand As String = "Demo"
    Public GainMDHost As String = "DemoSecondary.efxnow.com"
    Public GainMDPort As Integer = 3020
    Public GainPlatform As Integer

    Public EspeedLoginIP As String = "training.espeed.co.uk"
    Public EspeedLoginPort As String = "443"
    Public EspeedLoginUsername As String = ""
    Public EspeedLoginPassword As String = ""


    Public ArielLoginUserID As String = "ARIEL"
    Public ArielLoginUserName As String = "ARIEL"
    Public ArielLoginPassword As String = "ARIEL"
    Public ArielClient As String = "BGC"

    Public IcapIP As String = "10.81.1.155"
    Public IcapPort As String = ""
    Public IcapUserName As String = ""
    Public IcapPassword As String = ""

    Public TradeStationMonitorPath As String = "C:\Program Files\BGC\EXPORT\"
    Public MetaTraderMonitorPath As String = ""
    Public NeuroshellMonitorPath As String = "C:\Program Files\BGC\EXPORT\"


    'default values of the franco
    Public LoginidTC As String
    Public LoginSenderMarketData As String = "scalpercustu1stream"
    Public LoginTargetMarketData As String = "CNX"
    Public LoginIPMarketData As String = "64.210.170.127"
    Public LoginPortMarketData As String = "443"

    'Vm_ Fix TODO add values 
    Public DUKASLoginSenderMarketData As String = ""
    Public DUKASLoginUserNameMarketData As String = ""
    Public DUKASLoginTargetMarketData As String = ""
    Public DUKASLoginIPMarketData As String = ""
    Public DUKASLoginPortMarketData As String = ""

    Public FxIntegralSenderMarketData As String = ""
    Public FxIntegralUserNameMarketData As String = ""
    Public FxIntegralTargetMarketData As String = ""
    Public FxIntegralIPMarketData As String = ""
    Public FxIntegralPortMarketData As String = ""
    Public FxIntegralMarkLegalEntity As String = ""

    Public BaseCurrency As String = "USD"
    Public ExchangeServer As String = "1" 'currenex
    Public Platform As String = "TradeStation"

    Public reconnectInterval As Integer = 1
    Public Culture As CultureInfo = New CultureInfo("en-US")
    Private Shared settings As SettingsHome = Nothing
    Public DiscardAlertInterval As Integer = 0
    Public NSTintialpath As String = ""

    'Public IsOverrideMonitorPath As Boolean = False
    'Public OverrideMonitorPath As String = ""

    Public emailID As String = ""
    'This Flag is used to identify where TC is running under Test mode or not
    Public TESTMODE As Boolean = False

    Private Sub getSettings()
        Dim key As Microsoft.Win32.RegistryKey
        key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\BGC\\Settings")
        Try
            Me.ExecutionMode = key.GetValue("ExecutionMode", EXECUTION_NONE)
            Me.UseDefaultTradeSize = key.GetValue("UseDefaultTradeSize", False)
            Me.UseSymbolMapping = key.GetValue("UseSymbolMapping", True)

            Me.CXLoginIP = key.GetValue("CXLoginIP", "64.210.170.127")
            Me.CXLoginPort = key.GetValue("CXLoginPort", "443")
            Me.CXLoginSender = key.GetValue("CXLoginSender", "scalper")
            Me.CXLoginTarget = key.GetValue("CXLoginTarget", "fbc")
            'Vm_ Fix //TODO
            Me.DUKASLoginIP = key.GetValue("DUKASLoginIP", " ")
            Me.DUKASLoginPort = key.GetValue("DUKASLoginPort", " ")
            Me.DUKASLoginSender = key.GetValue("DUKASLoginSender", " ")
            Me.DUKASLoginTarget = key.GetValue("DUKASLoginTarget", " ")
            'Giri
            Me.FxIntLoginIP = key.GetValue("FxIntLoginIP", "")
            Me.FxIntLoginPort = key.GetValue("FxIntLoginPort", "")
            Me.FxIntLoginSendercompId = key.GetValue("FxIntLoginSendercompId", "")
            Me.FxIntLoginTargetcompId = key.GetValue("FxIntLoginTargetcompId", "")
            Me.FxIntLoginLegalEntity = key.GetValue("FxIntLoginLegalEntity", "")

            Me.EspeedLoginIP = key.GetValue("EspeedLoginIP", "training.espeed.co.uk")
            Me.EspeedLoginPort = key.GetValue("EspeedLoginPort", "443")
            Me.EspeedLoginUsername = key.GetValue("EspeedLoginUsername", "scalper")
            Me.EspeedLoginPassword = key.GetValue("EspeedLoginPassword", "fbc")

            Me.ArielLoginUserID = key.GetValue("ArielLoginUserID", "ARIEL")
            Me.ArielLoginUserName = key.GetValue("ArielLoginUserName", "ARIEL")
            Me.ArielLoginPassword = key.GetValue("ArielLoginPassword", "ARIEL")

            Me.DBFXUserName = key.GetValue("DBFXUserName", "trader")
            Me.DBFXPassword = key.GetValue("DBFXPassword", "trader")
            Me.DBFXURL = key.GetValue("DBFXURL", "http://www.fxcorporate.com/Hosts.jsp")
            Me.DBFXAccountType = key.GetValue("DBFXAccountType", "Demo")

            Me.GainUserName = key.GetValue("GainUserName", "scalper@gain.com")
            Me.GainPassword = key.GetValue("GainPassword", "12345")
            Me.GainBrand = key.GetValue("GainBrand", "Demo")
            Me.GainMDHost = key.GetValue("GainMDHost", "DemoSecondary.efxnow.com")
            Me.GainMDPort = key.GetValue("GainMDPort", 3020)
            Me.GainPlatform = key.GetValue("GainPlatform", 0)

            Me.IcapIP = key.GetValue("IcapIP", " ")
            Me.IcapPort = key.GetValue("IcapPort", " ")
            Me.IcapUserName = key.GetValue("IcapUserName", " ")
            Me.IcapPassword = key.GetValue("IcapPassword", " ")

            Me.LoginSenderMarketData = key.GetValue("LoginSenderMarketData", "scalpercustu1stream")
            Me.LoginTargetMarketData = key.GetValue("LoginTargetMarketData", "CNX")
            Me.LoginIPMarketData = key.GetValue("LoginIPMarketData", "64.210.170.127")
            Me.LoginPortMarketData = key.GetValue("LoginPortMarketData", "443")

            'Vm _Fix //TODO
            Me.DUKASLoginSenderMarketData = key.GetValue("DUKASLoginSenderMarketData", "")
            Me.DUKASLoginTargetMarketData = key.GetValue("DUKASLoginTargetMarketData", "")
            Me.DUKASLoginIPMarketData = key.GetValue("DUKASLoginIPMarketData", "")
            Me.DUKASLoginPortMarketData = key.GetValue("DUKASLoginPortMarketData", "")

            Me.FxIntegralSenderMarketData = key.GetValue("FxIntegralSenderMarketData", "")
            Me.FxIntegralTargetMarketData = key.GetValue("FxIntegralTargetMarketData", "")
            Me.FxIntegralIPMarketData = key.GetValue("FxIntegralIPMarketData", "")
            Me.FxIntegralPortMarketData = key.GetValue("FxIntegralPortMarketData", "")
            Me.FxIntegralMarkLegalEntity = key.GetValue("FxIntegralMarkLegalEntity", "")

            'Me.smtpServer = key.GetValue("SmtpServer", "213.171.216.67")
            'Me.smtpUserID = key.GetValue("SmtpUserID", "mail@scalper.co.uk")
            'Me.smtpPasswd = key.GetValue("SmtpPasswd", "kwakk56")

            Me.BaseCurrency = key.GetValue("BaseCurrency", "USD")
            Me.ExchangeServer = key.GetValue("ExchangeServer", "1")
            Me.FilterAlerts = key.GetValue("FilterAlerts", True)
            Me.Platform = key.GetValue("Platform", "TradeStation")
            Me.DiscardAlertInterval = key.GetValue("DiscardAlertInterval", "0")
            Me.NeuroshellMonitorPath = key.GetValue("NSTMonitorpath", "C:\Program Files\BGC\EXPORT\")
            Me.TradeStationMonitorPath = key.GetValue("MonitorPath", "C:\Program Files\BGC\EXPORT\")
            Me.reconnectInterval = key.GetValue("ReconnectInterval", 1)
        Catch ex As Exception
            Util.WriteDebugLog("SettingsHome->getSettings-> " + ex.Message + ex.StackTrace)
        End Try

        Try
            Me.MetaTraderMonitorPath = key.GetValue("MetaTraderMonitorPath", "")
            If (Me.MetaTraderMonitorPath = "") Then
                Dim metaTraderKey As Microsoft.Win32.RegistryKey
                metaTraderKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\MetaQuotes Software\\MetaTrader 4")
                Me.MetaTraderMonitorPath = metaTraderKey.GetValue("InstallPath", "")
                If (Me.MetaTraderMonitorPath <> "") Then
                    Me.MetaTraderMonitorPath = Me.MetaTraderMonitorPath '+ "\experts\files\"
                End If
            End If
        Catch ex As Exception
            Util.WriteDebugLog("getSettings()--- " & ex.Message)
        End Try

        'Try This line added in above Try catch block
        '    Me.NeuroshellMonitorPath = key.GetValue("NSTMonitorpath", "C:\Program Files\BGC\EXPORT\")
        'Catch ex As Exception
        'End Try

        If Directory.Exists(Me.TradeStationMonitorPath) = False Then
            ' If Dir$(Me.MonitorPath, FileAttribute.Directory).Trim = "" Then
            Try
                MkDir(Me.TradeStationMonitorPath)
            Catch ex As Exception
                Util.WriteDebugLog("getSettings()----" & ex.Message)
            End Try
            'End If
        End If
        Try
            If Directory.Exists(Me.NeuroshellMonitorPath) = False Then
                MkDir(Me.NeuroshellMonitorPath)
            End If
        Catch ex As Exception
            Util.WriteDebugLog("getSettings()----" & ex.Message)
        End Try

        Try
            Me.NSTintialpath = key.GetValue("NSTinitialpath", "")
            If (Me.NSTintialpath = "") Then
                Dim nsttradekey As Microsoft.Win32.RegistryKey
                nsttradekey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("NSTrader.Document\\shell\\open\\command", False)
                If Not (nsttradekey Is Nothing) Then
                    Me.NSTintialpath = nsttradekey.GetValue("")
                    Dim x As Integer = Me.NSTintialpath.LastIndexOf("\")
                    Me.NSTintialpath = Me.NSTintialpath.Remove((x + 1), (Me.NSTintialpath.Length - x - 1))
                End If
            End If
        Catch ex As Exception
            Util.WriteDebugLog("getSettings()----" & ex.Message)
        End Try
        'Dim stgMetaserver As SettingsMetaServer = New SettingsMetaServer
        'stgMetaserver.setSettings()
    End Sub

    Public Sub setDBFXVersion()
        Dim key As Microsoft.Win32.RegistryKey
        key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\\CandleWorks\\FXOrder2Go")
        Try
            key.SetValue("Version", "01.04.041509")
        Catch ex As Exception
            Util.WriteDebugLog("setDBFXVersion()----" & ex.Message)
        End Try
        key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\\CandleWorks\\ProxyServer")
        Try
            key.SetValue("HostsPath", "Hosts.jsp")
            key.SetValue("TCPTimeOut", 180000, Microsoft.Win32.RegistryValueKind.DWord)

        Catch ex As Exception
            Util.WriteDebugLog("setDBFXVersion()----" & ex.Message)
        End Try
    End Sub

    Public Sub setSettings()
        Dim key As Microsoft.Win32.RegistryKey
        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\BGC\\Settings")
        Try
            key.SetValue("ExecutionMode", Me.ExecutionMode)
            key.SetValue("UseDefaultTradeSize", Me.UseDefaultTradeSize)
            key.SetValue("UseSymbolMapping", Me.UseSymbolMapping)

            key.SetValue("MonitorPath", Me.TradeStationMonitorPath)
            key.SetValue("ReconnectInterval", Me.reconnectInterval)

            'key.SetValue("SmtpServer", Me.smtpServer)
            'key.SetValue("SmtpUserID", Me.smtpUserID)
            'key.SetValue("SmtpPasswd", Me.smtpPasswd)


            key.SetValue("BaseCurrency", Me.BaseCurrency)
            key.SetValue("ExchangeServer", Me.ExchangeServer)
            key.SetValue("FilterAlerts", Me.FilterAlerts)
            key.SetValue("Platform", Me.Platform)
            key.SetValue("DiscardAlertInterval", Me.DiscardAlertInterval)
            key.SetValue("MetaTraderMonitorPath", Me.MetaTraderMonitorPath)
            key.SetValue("NSTMonitorpath", Me.NeuroshellMonitorPath)
            EServerDependents.SetRegistery(key)
        Catch ex As Exception
            Util.WriteDebugLog("setSettings()----" & ex.Message)
        End Try
    End Sub

    Public Shared Function getInstance() As SettingsHome
        If (settings Is Nothing) Then
            settings = New SettingsHome()
            settings.setDBFXVersion()
            settings.getSettings()
        End If
        Return settings
    End Function

End Class

Public Class SettingsTrade
    Public TradeTypeManual As Integer = 3
    Public TradeTypeAuto As Integer = 3
    Public TradeTypeAutoOver10Mil As Integer = 1

    Public Sub getSettings()
        Dim key As Microsoft.Win32.RegistryKey
        key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\BGC\\Settings")
        Try
            Me.TradeTypeManual = key.GetValue("TradeTypeManual", 3)
            Me.TradeTypeAuto = key.GetValue("TradeTypeAuto", 3)
            Me.TradeTypeAutoOver10Mil = key.GetValue("TradeTypeAutoOver10Mil", 1)
        Catch ex As Exception
            Util.WriteDebugLog(" getSettings()---" & ex.Message)
        End Try
    End Sub

    Public Sub setSettings()
        Dim key As Microsoft.Win32.RegistryKey
        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\BGC\\Settings")
        Try
            key.SetValue("TradeTypeManual", Me.TradeTypeManual)
            key.SetValue("TradeTypeAuto", Me.TradeTypeAuto)
            key.SetValue("TradeTypeAutoOver10Mil", Me.TradeTypeAutoOver10Mil)
        Catch ex As Exception
            Util.WriteDebugLog(" setSettings()---" & ex.Message)
        End Try
    End Sub

End Class

Public Class SettingsSound
    Public UseDefaultSound As Boolean = True
    Public NoSound As Boolean = False
    Public OwnSound As Boolean = False
    Public OwnSoundName As String = ""

    Public Sub getSettings()
        Dim key As Microsoft.Win32.RegistryKey
        key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\BGC\\Settings")
        Try
            Me.UseDefaultSound = key.GetValue("UseDefaultSound", True)
            Me.NoSound = key.GetValue("NoSound", False)
            Me.OwnSound = key.GetValue("OwnSound", False)
            Me.OwnSoundName = key.GetValue("OwnSoundName", "")
        Catch ex As Exception
            Util.WriteDebugLog("getSettings()--" & ex.Message)
        End Try
    End Sub
    Public Sub setSettings()
        Dim key As Microsoft.Win32.RegistryKey
        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\BGC\\Settings")
        Try
            key.SetValue("UseDefaultSound", Me.UseDefaultSound)
            key.SetValue("NoSound", Me.NoSound)
            key.SetValue("OwnSound", Me.OwnSound)
            key.SetValue("OwnSoundName", Me.OwnSoundName)
        Catch ex As Exception
            Util.WriteDebugLog("setSettings()--" & ex.Message)
        End Try
    End Sub
End Class


Public Class Util
    Protected Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(GetType(Util))
    'Protected Shared ReadOnly logMetaServer As log4net.ILog = log4net.LogManager.GetLogger("MetaServerLog")
    Shared Sub New()
        log4net.Config.XmlConfigurator.Configure(New System.IO.FileInfo(Application.StartupPath + "\\logging.xml"))
    End Sub

    Public Shared Sub WriteDebugLog(ByVal s As String)
        SyncLock GetType(Util)
            log.Info(s)
        End SyncLock
    End Sub
    'Public Shared Sub WriteDebugLogSendFeedMetaServer(ByVal s As String)
    '    SyncLock GetType(Util)
    '        logMetaServer.Info(s)
    '    End SyncLock
    'End Sub

    Public Shared Function Encrypt(ByVal toEncrypt As String, ByVal useHashing As Boolean) As String
        Dim keyArray As Byte()
        Dim toEncryptArray As Byte() = UTF8Encoding.UTF8.GetBytes(toEncrypt)
        'Dim settingsReader As System.Configuration.AppSettingsReader = New AppSettingsReader
        Dim key As String '= CType(settingsReader.GetValue("SecurityKey", GetType(String)), String)
        key = "Franco Dumiccio"
        If useHashing Then
            Dim hashmd5 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key))
            hashmd5.Clear()
        Else
            keyArray = UTF8Encoding.UTF8.GetBytes(key)
        End If
        Dim tdes As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider
        tdes.Key = keyArray
        tdes.Mode = CipherMode.ECB
        tdes.Padding = PaddingMode.PKCS7
        Dim cTransform As ICryptoTransform = tdes.CreateEncryptor
        Dim resultArray As Byte() = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length)
        tdes.Clear()
        Return Convert.ToBase64String(resultArray, 0, resultArray.Length)
    End Function

    Public Shared Function Decrypt(ByVal cipherString As String, ByVal useHashing As Boolean) As String
        Dim keyArray As Byte()
        Dim toEncryptArray As Byte() = Convert.FromBase64String(cipherString)
        'Dim settingsReader As System.Configuration.AppSettingsReader = New AppSettingsReader
        Dim key As String '= CType(settingsReader.GetValue("SecurityKey", GetType(String)), String)
        key = "Franco Dumiccio"
        If useHashing Then
            Dim hashmd5 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key))
            hashmd5.Clear()
        Else
            keyArray = UTF8Encoding.UTF8.GetBytes(key)
        End If
        Dim tdes As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider
        tdes.Key = keyArray
        tdes.Mode = CipherMode.ECB
        tdes.Padding = PaddingMode.PKCS7
        Dim cTransform As ICryptoTransform = tdes.CreateDecryptor
        Dim resultArray As Byte() = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length)
        tdes.Clear()
        Return UTF8Encoding.UTF8.GetString(resultArray)
    End Function

End Class

Public Class EServerDependents

    Public Shared Function GetLoginForm() As Form

        Dim logonScreen As Form = Nothing
        If (SettingsHome.getInstance().ExchangeServer = ExchangeServer.CurrenEx) Then
            logonScreen = New LoginCurrenEX
        ElseIf (SettingsHome.getInstance().ExchangeServer = ExchangeServer.Dukascopy) Then
            logonScreen = New LoginDukascopy
        ElseIf (SettingsHome.getInstance().ExchangeServer = ExchangeServer.Ariel) Then
            logonScreen = New LoginAriel
        ElseIf (SettingsHome.getInstance().ExchangeServer = ExchangeServer.Espeed) Then
            logonScreen = New LoginEspeed
        ElseIf (SettingsHome.getInstance().ExchangeServer = ExchangeServer.DBFX) Then
            logonScreen = New LoginDBFX
        ElseIf (SettingsHome.getInstance().ExchangeServer = ExchangeServer.Gain) Then
            logonScreen = New LoginGain
        ElseIf (SettingsHome.getInstance().ExchangeServer = ExchangeServer.Icap) Then
            logonScreen = New LoginIcap
        ElseIf (SettingsHome.getInstance().ExchangeServer = ExchangeServer.FxIntegral) Then
            logonScreen = New LoginFxIntegral
        End If

        Return logonScreen
    End Function

    Public Shared Function GetCombinedCurrency(ByVal ccy1 As String, ByVal ccy2 As String) As String
        Dim symbol As String = ""
        Select Case SettingsHome.getInstance().ExchangeServer
            'Case ExchangeServer.CurrenEx
            '    symbol = ccy1 + "/" + ccy2
            'Case ExchangeServer.Dukascopy 'Vm_ Fix
            '    symbol = ccy1 + "/" + ccy2
            Case ExchangeServer.Ariel
                symbol = ccy1 + ccy2
            Case ExchangeServer.Espeed
                symbol = ccy1 + "/" + ccy2 + "_SP"
                'Case ExchangeServer.DBFX
                '    symbol = ccy1 + "/" + ccy2
                'Case ExchangeServer.Gain
                '    symbol = ccy1 + "/" + ccy2
                'Case ExchangeServer.Icap
                '    symbol = ccy1 + "/" + ccy2
            Case Else
                symbol = ccy1 + "/" + ccy2
        End Select
        Return symbol
    End Function

    Public Shared Function GetFirstCurrency(ByVal symbol As String) As String
        Dim firstcurrency As String = ""
        Select Case SettingsHome.getInstance().ExchangeServer
            Case ExchangeServer.Ariel
                firstcurrency = symbol.Substring(0, 3)
            Case Else 'ExchangeServer.CurrenEx
                firstcurrency = symbol.Split("/")(0)
                'Case ExchangeServer.Dukascopy 'Vm_ Fix
                '    firstcurrency = symbol.Split("/")(0)

                'Case ExchangeServer.Espeed
                '    firstcurrency = symbol.Split("/")(0)
                'Case ExchangeServer.DBFX
                '    firstcurrency = symbol.Split("/")(0)
                'Case ExchangeServer.Gain
                '    firstcurrency = symbol.Split("/")(0)
                'Case ExchangeServer.Icap
                '    firstcurrency = symbol.Split("/")(0)
        End Select
        Return firstcurrency
    End Function

    Public Shared Function GetSecondCurrency(ByVal symbol As String) As String
        Dim secondcurrency As String = ""
        Select Case SettingsHome.getInstance().ExchangeServer
            Case ExchangeServer.Ariel
                secondcurrency = symbol.Substring(3, 3)
            Case ExchangeServer.Espeed
                secondcurrency = symbol.Substring(4, 3)
            Case Else
                secondcurrency = symbol.Split("/")(1)
                'Case ExchangeServer.CurrenEx
                '    secondcurrency = symbol.Split("/")(1)
                'Case ExchangeServer.Dukascopy 'Vm_ Fix
                '    secondcurrency = symbol.Split("/")(1)
                'Case ExchangeServer.Ariel
                '    secondcurrency = symbol.Substring(3, 3)
                'Case ExchangeServer.Espeed
                '    secondcurrency = symbol.Substring(4, 3) 'Split("/")(1)
                'Case ExchangeServer.DBFX
                '    secondcurrency = symbol.Split("/")(1)
                'Case ExchangeServer.Gain
                '    secondcurrency = symbol.Split("/")(1)
                'Case ExchangeServer.Icap
                '    secondcurrency = symbol.Split("/")(1)
        End Select
        Return secondcurrency
    End Function

    Public Shared Function GetDateTime(ByVal dateti As String) As DateTime
        Dim datet As DateTime
        Select Case SettingsHome.getInstance().ExchangeServer
            Case ExchangeServer.CurrenEx
                datet = DateTime.Parse(dateti.Insert(4, " ").Insert(7, " ").Replace("-", " "))
            Case ExchangeServer.Dukascopy
                datet = DateTime.Parse(dateti.Insert(4, " ").Insert(7, " ").Replace("-", " "))
            Case ExchangeServer.Ariel
                dateti = dateti.Insert(6, "20") + ":00" '.Replace("/", " ")
                Dim culture As CultureInfo = New CultureInfo("fr-FR", True)
                Dim myDateTimeFrenchValue As String = dateti
                datet = _
                                     DateTime.Parse(myDateTimeFrenchValue, _
                                                    culture, _
                                                    DateTimeStyles.NoCurrentDateDefault)
            Case ExchangeServer.Espeed
                datet = Date.Now 'DateTime.Parse(datet)
            Case ExchangeServer.DBFX
                datet = Date.Now
            Case ExchangeServer.Gain
                datet = Date.Now
            Case ExchangeServer.Icap
                datet = DateTime.Parse(dateti.Insert(4, " ").Insert(7, " ").Replace("-", " "))
            Case ExchangeServer.FxIntegral
                datet = DateTime.Parse(dateti.Insert(4, " ").Insert(7, " ").Replace("-", " "))
        End Select
        Return datet
    End Function

    Public Shared Function GetEServerSender() As String
        Dim sender As String = ""
        Select Case SettingsHome.getInstance().ExchangeServer
            Case ExchangeServer.CurrenEx
                sender = SettingsHome.getInstance().CXLoginSender
            Case ExchangeServer.Dukascopy
                sender = SettingsHome.getInstance().DUKASLoginSender
            Case ExchangeServer.Ariel
                sender = SettingsHome.getInstance().ArielLoginUserID
            Case ExchangeServer.Espeed
                sender = SettingsHome.getInstance().EspeedLoginUsername
            Case ExchangeServer.DBFX
                sender = SettingsHome.getInstance().DBFXUserName
            Case ExchangeServer.Gain
                sender = SettingsHome.getInstance().GainUserName
            Case ExchangeServer.Icap
                sender = SettingsHome.getInstance().IcapUserName
            Case ExchangeServer.FxIntegral
                sender = SettingsHome.getInstance().FxIntLoginSendercompId
        End Select
        Return sender
    End Function

    Public Shared Function GetEServerSenderMarketData() As String 'Todo
        Dim sender As String = ""
        Select Case SettingsHome.getInstance().ExchangeServer
            Case ExchangeServer.CurrenEx
                sender = SettingsHome.getInstance().LoginSenderMarketData
            Case ExchangeServer.Dukascopy
                sender = SettingsHome.getInstance().DUKASLoginSenderMarketData
            Case ExchangeServer.FxIntegral
                sender = SettingsHome.getInstance().FxIntegralSenderMarketData
            Case ExchangeServer.Ariel
            Case ExchangeServer.Espeed
            Case ExchangeServer.DBFX
            Case ExchangeServer.Gain
            Case ExchangeServer.Icap
        End Select
        Return sender
    End Function

    Public Shared Sub LogLoginDetails()
        Select Case SettingsHome.getInstance().ExchangeServer
            Case ExchangeServer.CurrenEx
                Util.WriteDebugLog("            IP            -" + SettingsHome.getInstance().CXLoginIP)
                Util.WriteDebugLog("            Port          -" + SettingsHome.getInstance().CXLoginPort)
                Util.WriteDebugLog("            Sender        -" + SettingsHome.getInstance().CXLoginSender)
                Util.WriteDebugLog("            Target        -" + SettingsHome.getInstance().CXLoginTarget)
            Case ExchangeServer.Dukascopy
                Util.WriteDebugLog("            IP            -" + SettingsHome.getInstance().DUKASLoginIP)
                Util.WriteDebugLog("            Port          -" + SettingsHome.getInstance().DUKASLoginPort)
                Util.WriteDebugLog("            Sender        -" + SettingsHome.getInstance().DUKASLoginSender)
                Util.WriteDebugLog("            Target        -" + SettingsHome.getInstance().DUKASLoginTarget)
            Case ExchangeServer.FxIntegral
                Util.WriteDebugLog("            IP            -" + SettingsHome.getInstance().FxIntLoginIP)
                Util.WriteDebugLog("            Port          -" + SettingsHome.getInstance().FxIntLoginPort)
                Util.WriteDebugLog("            Sender        -" + SettingsHome.getInstance().FxIntLoginSendercompId)
                Util.WriteDebugLog("            Target        -" + SettingsHome.getInstance().FxIntLoginTargetcompId)
            Case ExchangeServer.Ariel
                Util.WriteDebugLog("            User ID       -" + SettingsHome.getInstance().ArielLoginUserID)
                Util.WriteDebugLog("            Username      -" + SettingsHome.getInstance().ArielLoginUserName)
            Case ExchangeServer.Espeed
                Util.WriteDebugLog("            IP            -" + SettingsHome.getInstance().EspeedLoginIP)
                Util.WriteDebugLog("            Port          -" + SettingsHome.getInstance().EspeedLoginPort)
                Util.WriteDebugLog("            Username      -" + SettingsHome.getInstance().EspeedLoginUsername)
            Case ExchangeServer.DBFX
                Util.WriteDebugLog("            User Name           -" + SettingsHome.getInstance().DBFXUserName)
                Util.WriteDebugLog("            URL                 -" + SettingsHome.getInstance().DBFXAccountType)
                Util.WriteDebugLog("            Account Type        -" + SettingsHome.getInstance().DBFXAccountType)
            Case ExchangeServer.DBFX
                Util.WriteDebugLog("            User Name               -" + SettingsHome.getInstance().DBFXUserName)
                Util.WriteDebugLog("            Brand                   -" + SettingsHome.getInstance().DBFXAccountType)
                Util.WriteDebugLog("            Market Data Host        -" + SettingsHome.getInstance().DBFXAccountType)
                Util.WriteDebugLog("            Market Data Port        -" + SettingsHome.getInstance().DBFXAccountType)
            Case ExchangeServer.Icap
                Util.WriteDebugLog("            IP            -" + SettingsHome.getInstance().IcapIP)
                Util.WriteDebugLog("            Port          -" + SettingsHome.getInstance().IcapPort)
                Util.WriteDebugLog("            Sender        -" + SettingsHome.getInstance().IcapUserName)
        End Select
    End Sub

    Public Shared Function SetTrader(ByVal trad As Trader) As Trader
        Select Case SettingsHome.getInstance().ExchangeServer
            Case ExchangeServer.CurrenEx
                trad.Param1 = SettingsHome.getInstance().CXLoginIP
                trad.Param2 = SettingsHome.getInstance().CXLoginPort
                trad.Param3 = SettingsHome.getInstance().CXLoginSender
                trad.Param4 = SettingsHome.getInstance().CXLoginTarget
                trad.Param5 = SettingsHome.getInstance().CXLoginPassword '_pwr
            Case ExchangeServer.Dukascopy 'Vm_ Fix
                trad.Param1 = SettingsHome.getInstance().DUKASLoginIP
                trad.Param2 = SettingsHome.getInstance().DUKASLoginPort
                trad.Param3 = SettingsHome.getInstance().DUKASLoginSender
                trad.Param4 = SettingsHome.getInstance().DUKASLoginTarget
                trad.Param5 = SettingsHome.getInstance().DUKASLoginPassword '_pwr
                trad.Param6 = SettingsHome.getInstance().DUKASLoginUserName
            Case ExchangeServer.FxIntegral
                trad.Param1 = SettingsHome.getInstance().FxIntLoginIP
                trad.Param2 = SettingsHome.getInstance().FxIntLoginPort
                trad.Param3 = SettingsHome.getInstance().FxIntLoginSendercompId
                trad.Param4 = SettingsHome.getInstance().FxIntLoginTargetcompId
                trad.Param5 = SettingsHome.getInstance().FxIntLoginPassword
                trad.Param6 = SettingsHome.getInstance().FxIntLoginUserName
                trad.param7 = SettingsHome.getInstance().FxIntLoginLegalEntity
            Case ExchangeServer.Ariel
                trad.Param1 = SettingsHome.getInstance().ArielLoginUserID
                trad.Param2 = SettingsHome.getInstance().ArielLoginUserName
                trad.Param3 = SettingsHome.getInstance().ArielLoginPassword
                trad.Param4 = ""
                trad.Param5 = ""
            Case ExchangeServer.Espeed
                trad.Param1 = SettingsHome.getInstance().EspeedLoginIP
                trad.Param2 = SettingsHome.getInstance().EspeedLoginPort
                trad.Param3 = SettingsHome.getInstance().EspeedLoginUsername
                trad.Param4 = SettingsHome.getInstance().EspeedLoginPassword
                trad.Param5 = ""
            Case ExchangeServer.DBFX
                trad.Param1 = SettingsHome.getInstance().DBFXUserName
                trad.Param2 = SettingsHome.getInstance().DBFXPassword
                trad.Param3 = SettingsHome.getInstance().DBFXURL
                trad.Param4 = SettingsHome.getInstance().DBFXAccountType
                trad.Param5 = ""
            Case ExchangeServer.Gain
                trad.Param1 = SettingsHome.getInstance().GainUserName
                trad.Param2 = SettingsHome.getInstance().GainPassword
                trad.Param3 = SettingsHome.getInstance().GainBrand
                trad.Param4 = SettingsHome.getInstance().GainMDHost
                trad.Param5 = SettingsHome.getInstance().GainMDPort
                trad.Param6 = SettingsHome.getInstance().GainPlatform
            Case ExchangeServer.Icap
                trad.Param1 = SettingsHome.getInstance().IcapIP
                trad.Param2 = SettingsHome.getInstance().IcapPort
                trad.Param3 = SettingsHome.getInstance().IcapUserName
                trad.Param4 = SettingsHome.getInstance().IcapPassword
        End Select
        Return trad
    End Function

    Public Shared Function SetTraderMarketData(ByVal trad As Trader) As Trader

        Select Case SettingsHome.getInstance().ExchangeServer
            Case ExchangeServer.CurrenEx
                trad.Param1 = SettingsHome.getInstance().LoginIPMarketData
                trad.Param2 = SettingsHome.getInstance().LoginPortMarketData
                trad.Param3 = SettingsHome.getInstance().LoginSenderMarketData
                trad.Param4 = SettingsHome.getInstance().LoginTargetMarketData
                trad.Param5 = SettingsHome.getInstance().CXLoginPassword
            Case ExchangeServer.Dukascopy
                trad.Param1 = SettingsHome.getInstance().DUKASLoginIPMarketData
                trad.Param2 = SettingsHome.getInstance().DUKASLoginPortMarketData
                trad.Param3 = SettingsHome.getInstance().DUKASLoginSenderMarketData
                trad.Param4 = SettingsHome.getInstance().DUKASLoginTargetMarketData
                trad.Param5 = SettingsHome.getInstance().DUKASLoginPassword
                trad.Param6 = SettingsHome.getInstance().DUKASLoginUserNameMarketData
            Case ExchangeServer.FxIntegral 'Todo
                trad.Param1 = SettingsHome.getInstance().FxIntegralIPMarketData
                trad.Param2 = SettingsHome.getInstance().FxIntegralPortMarketData
                trad.Param3 = SettingsHome.getInstance().FxIntegralSenderMarketData
                trad.Param4 = SettingsHome.getInstance().FxIntegralTargetMarketData
                trad.Param5 = SettingsHome.getInstance().FxIntLoginPassword
                trad.Param6 = SettingsHome.getInstance().FxIntegralUserNameMarketData
                trad.param7 = SettingsHome.getInstance().FxIntegralMarkLegalEntity
            Case ExchangeServer.Ariel
            Case ExchangeServer.Espeed
            Case ExchangeServer.DBFX
            Case ExchangeServer.Icap
        End Select
        Return trad
    End Function

    Public Sub MarketDataSubscriptionButtons()
        Select Case SettingsHome.getInstance().ExchangeServer
            Case ExchangeServer.CurrenEx
                Form1.GetSingletonOrderform().btnSubscribeMarketData.Visible = True
                Form1.GetSingletonOrderform().btnDisconnectMarketData.Visible = True
            Case ExchangeServer.Ariel
                Form1.GetSingletonOrderform().btnSubscribeMarketData.Visible = False
                Form1.GetSingletonOrderform().btnDisconnectMarketData.Visible = False
            Case ExchangeServer.DBFX
                Form1.GetSingletonOrderform().btnSubscribeMarketData.Visible = False
                Form1.GetSingletonOrderform().btnDisconnectMarketData.Visible = False
            Case ExchangeServer.Gain
                Form1.GetSingletonOrderform().btnSubscribeMarketData.Visible = False
                Form1.GetSingletonOrderform().btnDisconnectMarketData.Visible = False
            Case ExchangeServer.Dukascopy
                Form1.GetSingletonOrderform().btnSubscribeMarketData.Visible = True
                Form1.GetSingletonOrderform().btnDisconnectMarketData.Visible = True
            Case ExchangeServer.Icap
                Form1.GetSingletonOrderform().btnSubscribeMarketData.Visible = True
                Form1.GetSingletonOrderform().btnDisconnectMarketData.Visible = False
            Case ExchangeServer.FxIntegral
                Form1.GetSingletonOrderform().btnSubscribeMarketData.Visible = True
                Form1.GetSingletonOrderform().btnDisconnectMarketData.Visible = True
            Case ExchangeServer.Espeed
                Form1.GetSingletonOrderform().btnSubscribeMarketData.Visible = False
                Form1.GetSingletonOrderform().btnDisconnectMarketData.Visible = False
        End Select
        'If ((SettingsHome.getInstance().ExchangeServer = ExchangeServer.CurrenEx) Or (SettingsHome.getInstance().ExchangeServer = ExchangeServer.Dukascopy)) Then
        '    Form1.GetSingletonOrderform().btnSubscribeMarketData.Visible = True
        '    Form1.GetSingletonOrderform().btnDisconnectMarketData.Visible = True
        'ElseIf (SettingsHome.getInstance().ExchangeServer = ExchangeServer.Ariel) Then
        '    Form1.GetSingletonOrderform().btnSubscribeMarketData.Visible = False
        '    Form1.GetSingletonOrderform().btnDisconnectMarketData.Visible = False
        'ElseIf (SettingsHome.getInstance().ExchangeServer = ExchangeServer.Espeed) Then
        '    Form1.GetSingletonOrderform().btnSubscribeMarketData.Visible = False
        '    Form1.GetSingletonOrderform().btnDisconnectMarketData.Visible = False
        'ElseIf (SettingsHome.getInstance().ExchangeServer = ExchangeServer.DBFX) Then
        '    Form1.GetSingletonOrderform().btnSubscribeMarketData.Visible = False
        '    Form1.GetSingletonOrderform().btnDisconnectMarketData.Visible = False
        'ElseIf (SettingsHome.getInstance().ExchangeServer = ExchangeServer.Gain) Then
        '    Form1.GetSingletonOrderform().btnSubscribeMarketData.Visible = False
        '    Form1.GetSingletonOrderform().btnDisconnectMarketData.Visible = False
        'ElseIf (SettingsHome.getInstance().ExchangeServer = ExchangeServer.Icap) Then
        '    Form1.GetSingletonOrderform().btnSubscribeMarketData.Visible = True
        '    Form1.GetSingletonOrderform().btnDisconnectMarketData.Visible = False
        'End If
    End Sub

    Public Shared Sub SetRegistery(ByVal key As Microsoft.Win32.RegistryKey)
        Select Case SettingsHome.getInstance().ExchangeServer
            Case ExchangeServer.CurrenEx
                key.SetValue("CXLoginIP", SettingsHome.getInstance().CXLoginIP)
                key.SetValue("CXLoginPort", SettingsHome.getInstance().CXLoginPort)
                key.SetValue("CXLoginSender", SettingsHome.getInstance().CXLoginSender)
                key.SetValue("CXLoginTarget", SettingsHome.getInstance().CXLoginTarget)

                key.SetValue("LoginSenderMarketData", SettingsHome.getInstance().LoginSenderMarketData)
                key.SetValue("LoginTargetMarketData", SettingsHome.getInstance().LoginTargetMarketData)
                key.SetValue("LoginIPMarketData", SettingsHome.getInstance().LoginIPMarketData)
                key.SetValue("LoginPortMarketData", SettingsHome.getInstance().LoginPortMarketData)
            Case ExchangeServer.Dukascopy  'Vm_ Fix
                key.SetValue("DUKASLoginIP", SettingsHome.getInstance().DUKASLoginIP)
                key.SetValue("DUKASLoginPort", SettingsHome.getInstance().DUKASLoginPort)
                key.SetValue("DUKASLoginSender", SettingsHome.getInstance().DUKASLoginSender)
                key.SetValue("DUKASLoginTarget", SettingsHome.getInstance().DUKASLoginTarget)

                key.SetValue("DUKASLoginSenderMarketData", SettingsHome.getInstance().DUKASLoginSenderMarketData)
                key.SetValue("DUKASLoginTargetMarketData", SettingsHome.getInstance().DUKASLoginTargetMarketData)
                key.SetValue("DUKASLoginIPMarketData", SettingsHome.getInstance().DUKASLoginIPMarketData)
                key.SetValue("DUKASLoginPortMarketData", SettingsHome.getInstance().DUKASLoginPortMarketData)
            Case ExchangeServer.FxIntegral  'Todo giri
                key.SetValue("FxIntLoginIP", SettingsHome.getInstance().FxIntLoginIP)
                key.SetValue("FxIntLoginPort", SettingsHome.getInstance().FxIntLoginPort)
                key.SetValue("FxIntLoginSendercompId", SettingsHome.getInstance().FxIntLoginSendercompId)
                key.SetValue("FxIntLoginTargetcompId", SettingsHome.getInstance().FxIntLoginTargetcompId)
                key.SetValue("FxIntLoginLegalEntity", SettingsHome.getInstance().FxIntLoginLegalEntity)

                key.SetValue("FxIntegralSenderMarketData", SettingsHome.getInstance().FxIntegralSenderMarketData)
                key.SetValue("FxIntegralTargetMarketData", SettingsHome.getInstance().FxIntegralTargetMarketData)
                key.SetValue("FxIntegralIPMarketData", SettingsHome.getInstance().FxIntegralIPMarketData)
                key.SetValue("FxIntegralPortMarketData", SettingsHome.getInstance().FxIntegralPortMarketData)
                key.SetValue("FxIntegralMarkLegalEntity", SettingsHome.getInstance().FxIntegralMarkLegalEntity)
            Case ExchangeServer.Ariel
                key.SetValue("ArielLoginUserID", SettingsHome.getInstance().ArielLoginUserID)
                key.SetValue("ArielLoginUserName", SettingsHome.getInstance().ArielLoginUserName)
                key.SetValue("ArielLoginPassword", SettingsHome.getInstance().ArielLoginPassword)
            Case ExchangeServer.Espeed
                key.SetValue("EspeedLoginIP", SettingsHome.getInstance().EspeedLoginIP)
                key.SetValue("EspeedLoginPort", SettingsHome.getInstance().EspeedLoginPort)
                key.SetValue("EspeedLoginUsername", SettingsHome.getInstance().EspeedLoginUsername)
                key.SetValue("EspeedLoginPassword", SettingsHome.getInstance().EspeedLoginPassword)
            Case ExchangeServer.DBFX
                key.SetValue("DBFXUserName", SettingsHome.getInstance().DBFXUserName)
                key.SetValue("DBFXPassword", SettingsHome.getInstance().DBFXPassword)
                key.SetValue("DBFXURL", SettingsHome.getInstance().DBFXURL)
                key.SetValue("DBFXAccountType", SettingsHome.getInstance().DBFXAccountType)
            Case ExchangeServer.Gain
                key.SetValue("GainUserName", SettingsHome.getInstance().GainUserName)
                key.SetValue("GainPassword", SettingsHome.getInstance().GainPassword)
                key.SetValue("GainBrand", SettingsHome.getInstance().GainBrand)
                key.SetValue("GainMDHost", SettingsHome.getInstance().GainMDHost)
                key.SetValue("GainMDPort", SettingsHome.getInstance().GainMDPort)
            Case ExchangeServer.Icap
                key.SetValue("IcapIP", SettingsHome.getInstance().IcapIP)
                key.SetValue("IcapPort", SettingsHome.getInstance().IcapPort)
                key.SetValue("IcapUserName", SettingsHome.getInstance().IcapUserName)
                key.SetValue("IcapPassword", SettingsHome.getInstance().IcapPassword)
        End Select
    End Sub

    Public Shared Sub InitializeMarketDataButtons()

        Select Case SettingsHome.getInstance().ExchangeServer
            Case ExchangeServer.CurrenEx
                Form1.GetSingletonOrderform().btnSubscribeMarketData.Enabled = True
            Case ExchangeServer.Dukascopy  'Vm_ Fix
                Form1.GetSingletonOrderform().btnSubscribeMarketData.Enabled = True
            Case ExchangeServer.FxIntegral
                Form1.GetSingletonOrderform().btnSubscribeMarketData.Enabled = True
            Case ExchangeServer.Ariel
                Form1.GetSingletonOrderform().btnSubscribeMarketData.Enabled = False
            Case ExchangeServer.Espeed
                Form1.GetSingletonOrderform().btnSubscribeMarketData.Enabled = False
            Case ExchangeServer.DBFX
                Form1.GetSingletonOrderform().btnSubscribeMarketData.Enabled = False
            Case ExchangeServer.Gain
                Form1.GetSingletonOrderform().btnSubscribeMarketData.Enabled = False
            Case ExchangeServer.Icap
                Form1.GetSingletonOrderform().btnSubscribeMarketData.Enabled = True
        End Select
    End Sub

    Public Shared Sub UpdateMarketDataButtons(ByVal stat As Trader.ConnectionStatus)
        Select Case SettingsHome.getInstance().ExchangeServer
            Case ExchangeServer.CurrenEx
            Case ExchangeServer.Dukascopy 'Vm_Fix
            Case ExchangeServer.FxIntegral
            Case ExchangeServer.Icap
            Case ExchangeServer.Ariel
                Select Case stat
                    Case Trader.ConnectionStatus.CONNECTED
                        'Form1.GetSingletonOrderform().btnSubscribeMarketData.Enabled = True
                        Form1.GetSingletonOrderform().EnableMarketDataButtonByDelegate()
                    Case Trader.ConnectionStatus.DISCONNECTED
                        'Form1.GetSingletonOrderform().btnSubscribeMarketData.Enabled = False
                        Form1.GetSingletonOrderform().DisableMarketDataButtonByDelegate()
                End Select
            Case ExchangeServer.Espeed
                Select Case stat
                    Case Trader.ConnectionStatus.CONNECTED
                        Form1.GetSingletonOrderform().EnableMarketDataButtonByDelegate()
                    Case Trader.ConnectionStatus.DISCONNECTED
                        Form1.GetSingletonOrderform().DisableMarketDataButtonByDelegate()
                End Select
            Case ExchangeServer.DBFX
                Select Case stat
                    Case Trader.ConnectionStatus.CONNECTED
                        'Form1.GetSingletonOrderform().btnSubscribeMarketData.Enabled = True
                        Form1.GetSingletonOrderform().EnableMarketDataButtonByDelegate()
                    Case Trader.ConnectionStatus.DISCONNECTED
                        'Form1.GetSingletonOrderform().btnSubscribeMarketData.Enabled = False
                        Form1.GetSingletonOrderform().DisableMarketDataButtonByDelegate()
                End Select
            Case ExchangeServer.Gain
                Select Case stat
                    Case Trader.ConnectionStatus.CONNECTED
                        'Form1.GetSingletonOrderform().btnSubscribeMarketData.Enabled = True
                        Form1.GetSingletonOrderform().EnableMarketDataButtonByDelegate()
                    Case Trader.ConnectionStatus.DISCONNECTED
                        'Form1.GetSingletonOrderform().btnSubscribeMarketData.Enabled = False
                        Form1.GetSingletonOrderform().DisableMarketDataButtonByDelegate()
                End Select
        End Select
    End Sub

End Class
