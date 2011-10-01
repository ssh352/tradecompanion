Imports System
Imports System.Windows.Forms
Imports log4net

Namespace DBFXClient

    Public Class DBFXUtil

        Public Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(GetType(DBFXUtil))

        Shared Sub New()
            Config.XmlConfigurator.Configure(New System.IO.FileInfo(Application.StartupPath + "\\\\logging.xml"))
        End Sub

        Public Shared Sub WriteDebugLog(ByVal s As String)
            SyncLock GetType(DBFXUtil)
                log.Info(s)
            End SyncLock
        End Sub

        Public Sub WriteDebugLogDebug(ByVal s As String)
            SyncLock GetType(DBFXUtil)
                log.Debug(s)
            End SyncLock
        End Sub

        Public Sub WriteDebugLogWarn(ByVal s As String)
            SyncLock GetType(DBFXUtil)
                log.Warn(s)
            End SyncLock
        End Sub

        Public Sub WriteDebugLogError(ByVal s As String)
            SyncLock GetType(DBFXUtil)
                log.Error(s)
            End SyncLock
        End Sub

    End Class

End Namespace
