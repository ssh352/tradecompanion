Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports System.IO

Module SubMain
    Sub Main()
        Dim currentProcesses() As Process
        currentProcesses = Process.GetProcessesByName(Process.GetCurrentProcess.ProcessName)
        If currentProcesses.GetUpperBound(0) <= 0 Then
            TradeCompanion()
        Else
            MessageBox.Show("An application instance is already running")
            Exit Sub
        End If
    End Sub
    Private Sub TradeCompanion()
        Dim key As Microsoft.Win32.RegistryKey
        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\BGC\\Settings")
        'Dim crashTest As XmlRead = New XmlRead()
        'If crashTest.CrashTest() Then
        'crashTest.Perform()
        ' Else

        Dim dlg As New LoginForm1
        If dlg.ShowDialog() = DialogResult.OK Then
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\BGC\\Settings")
            key.SetValue("Registered", Util.Encrypt("true", True))
            Try
                'The user correctly logged in.
                'Display the main form.
                Dim frm As New Form1
                If (SettingsHome.getInstance().ExchangeServer = "2") Then
                    ArielClient.ShowDialog()
                End If
                frm.ShowDialog()
            Catch ex As Exception
                Util.WriteDebugLog("-----------CRITICAL ERROR--------------")
                Util.WriteDebugLog(ex.Message)
                Util.WriteDebugLog(ex.StackTrace)
            End Try

        End If
        ' End If
        'Dim frm As New Form1
        'frm.ShowDialog()
    End Sub

End Module
