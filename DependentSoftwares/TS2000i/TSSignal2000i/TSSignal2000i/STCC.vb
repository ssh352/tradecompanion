Imports ORTCLib
Imports System.io
Public Class STCC
    Private TS As STCCOrders = New STCCOrders
    Private WithEvents Orders As FilledOrders
    Private MonitorPath As String = "C:\Program Files\Scalper\EXPORT\"
    Private r As Random = New Random(Now.Millisecond)
    Private ws As ArrayList = New ArrayList
    Private Sub Orders_Add(ByVal pDisp As Object) Handles Orders.Add
        Try
            Dim neworder As FilledOrder = pDisp
            WriteAlert(neworder)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub WriteAlert(ByVal neworder As FilledOrder)
        Try
            Dim wsindex As Integer = ws.IndexOf(neworder.WorkSpace)
            If wsindex = -1 Then
                wsindex = ws.Add(neworder.WorkSpace)
            End If
            Dim orderdatetime As Date = neworder.TimeFilled
            Dim orderaction As String = neworder.Order
            Dim acct As String = neworder.WorkSpace
            If acct.LastIndexOf("\") > 0 Then
                acct = acct.Substring(acct.LastIndexOf("\") + 1)
            End If
            If acct.LastIndexOf(".") > 0 Then
                acct = acct.Substring(0, acct.LastIndexOf("."))
            End If
            acct = skipChar(acct, " ")
            Dim orderrecord As String = "1" + Format(orderdatetime, "yyMMdd") + " " + _
                                        Format(orderdatetime, "HHmm") + " " + _
                                        "NA" + " " + neworder.Symbol + " " + _
                                        GetAction(orderaction) + " " + GetSize(orderaction) + " " + _
                                        (wsindex + 1).ToString + neworder.WindowID.ToString + " " + acct
            Dim fn As String = MonitorPath + getValidFileName(neworder.Symbol).ToUpper + r.Next.ToString + ".req"
            While File.Exists(fn)
                fn = MonitorPath + getValidFileName(neworder.Symbol).ToUpper + r.Next.ToString + ".req"
            End While
            Dim sw As StreamWriter = New StreamWriter(fn)
            sw.WriteLine(orderrecord)
            sw.Close()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub getSettings()
        Dim key As Microsoft.Win32.RegistryKey
        key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Scalper\\Settings")
        Try
            Me.MonitorPath = key.GetValue("MonitorPath", "C:\Program Files\Scalper\EXPORT\")
        Catch ex As Exception
        End Try

    End Sub
    Private Function GetAction(ByVal s As String) As String
        Dim j As Integer
        Dim sVal As String
        j = InStr(s, " ")
        If j = 0 Then
            sVal = s
        Else
            sVal = Mid(s, 1, j - 1)
        End If
        Return sVal.ToUpper
    End Function
    Private Function GetSize(ByVal s As String) As String
        Dim j As Integer
        Dim sVal As String
        j = InStr(s, " ")
        If j = 0 Then
            sVal = s
        Else
            s = Mid(s, j + 1)

            j = InStr(s, " ")

            If j = 0 Then
                sVal = s

            Else
                sVal = Mid(s, 1, j - 1)
            End If
        End If
        Return sVal

    End Function
    Private Function getValidFileName(ByVal s As String) As String
        Dim newS As String = s
        s = skipChar(s, "\")
        s = skipChar(s, "/")
        s = skipChar(s, ":")
        s = skipChar(s, "*")
        s = skipChar(s, "?")
        s = skipChar(s, "<")
        s = skipChar(s, ">")

        Return s
    End Function
    Private Function skipChar(ByVal s As String, ByVal chr As String) As String
        Return s.Replace(chr, "")
    End Function
    Public Sub New()
        Orders = TS.FilledOrders
        getSettings()
        If Directory.Exists(MonitorPath) = False Then
            Directory.CreateDirectory(MonitorPath)
        End If
    End Sub
End Class
