Imports System.io

Public Class AlertWatcher
    Private watcher As FileSystemWatcher
    Private appSettings As SettingsHome
    Public Event NewAlert(ByVal execute As AlertsManager.NewAlert)
    Private Shared _SingletonAlertWatcher As AlertWatcher = Nothing
    Private Sub New()
        appSettings = SettingsHome.getInstance() 'Keys
        watcher = New FileSystemWatcher()
        watcher.Filter = "*.req"
        If (appSettings.Platform = "TradeStation") Then 'TradeStation
            watcher.Path = appSettings.TradeStationMonitorPath
        ElseIf (appSettings.Platform = "MetaTrader") Then 'MetaTrader
            If (appSettings.MetaTraderMonitorPath = "") Then
                MessageBox.Show("Metatrader 4 is not installed", "AutoShark")
                Exit Sub
            End If
            watcher.Path = appSettings.MetaTraderMonitorPath

        ElseIf appSettings.Platform = "NeuroShell" Then 'Neuroshell
            watcher.Path = appSettings.NeuroshellMonitorPath
        End If

        watcher.IncludeSubdirectories = False
        watcher.InternalBufferSize = 28672
        watcher.NotifyFilter = NotifyFilters.FileName
        AddHandler watcher.Created, AddressOf OnChanged
        AddHandler watcher.Error, AddressOf OnError
        watcher.EnableRaisingEvents = True
    End Sub

    Public Shared Function getInstance() As AlertWatcher
        If (_SingletonAlertWatcher Is Nothing) Then
            _SingletonAlertWatcher = New AlertWatcher()
        End If
        Return _SingletonAlertWatcher
    End Function

    Public Sub InitializeMonitorPath(ByVal PlatForm As String)
        If (PlatForm = "TradeStation") Then 'TradeStation
            watcher.Path = appSettings.TradeStationMonitorPath
        ElseIf (PlatForm = "MetaTrader") Then 'MetaTrader
            If (appSettings.MetaTraderMonitorPath = "") Then
                MessageBox.Show("Metatrader 4 is not installed", "AutoShark")
                Exit Sub
            End If
            watcher.Path = appSettings.MetaTraderMonitorPath + "\experts\files\"
        ElseIf (PlatForm = "NeuroShell") Then
            watcher.Path = appSettings.NeuroshellMonitorPath
        End If
    End Sub

    Public Sub Quit()
        watcher.EnableRaisingEvents = False
    End Sub

    Public Property CurrentSettings() As SettingsHome
        Get
            CurrentSettings = appSettings
        End Get
        Set(ByVal Value As SettingsHome)
            appSettings = Value
            'appSettings.setSettings()
        End Set
    End Property

    Private Sub OnError(ByVal source As Object, ByVal e As ErrorEventArgs)
        Dim myReturnedException As Exception = e.GetException()
        'MessageBox.Show(("The returned exception is: " & myReturnedException.Message))
        If TypeOf myReturnedException Is InternalBufferOverflowException Then
            Util.WriteDebugLog("The file system watcher experienced an internal buffer overflow: " _
                + myReturnedException.Message)
        Else
            Util.WriteDebugLog("The file system experienced an internal ERROR: " _
                + myReturnedException.Message)
        End If
    End Sub

    Private Sub OnChanged(ByVal source As Object, ByVal e As FileSystemEventArgs)    'Shared
        ' Specify what is done when a file is changed, created, or deleted.
        ' Console.WriteLine("File: " & e.FullPath & " " & e.ChangeType)
        Util.WriteDebugLog("The file system _OnChanged Event :" + e.FullPath)
        Dim fn As String = e.FullPath
        Dim m As AlertsManager = New AlertsManager
        Try
            Dim execute As AlertsManager.NewAlert
            execute = m.ProcessAlert(fn, appSettings)
            If execute.status = AlertsManager.STATUS_ACCEPTED Then
                RaiseEvent NewAlert(execute)
            End If
        Catch ex As Exception
            Util.WriteDebugLog("The file system _OnChanged Event ERROR:" + ex.Message)
            'MsgBox(ex.Message)
        End Try
    End Sub
End Class
