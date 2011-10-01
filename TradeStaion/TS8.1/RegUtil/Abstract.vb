Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions
Public Class Exports
    Private Shared Function MonitorPath() As String
        Dim key As Microsoft.Win32.RegistryKey
        key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\BGC\\Settings")
        Dim path As String = "C:\Program Files\BGC\EXPORT\"
        Try
            path = key.GetValue("MonitorPath", "C:\Program Files\BGC\EXPORT\")
        Catch ex As Exception

        End Try
        Return path
    End Function

    <Exported("GETMONITORPATHLENGTH", 2)> Public Shared Function GetMonitorPathLength() As Integer
        Dim mpath As String = MonitorPath()
        If mpath Is Nothing Then Return 0 Else Return mpath.Length + 1
    End Function
    <Exported("GETMONITORPATH", 1)> Public Shared Function GetMonitorPath(ByVal lpwstr As Integer, ByVal stringlen As Integer) As Integer
        Dim strlenwritten As Integer = 0
        Dim stringtomodify As String

        Try

            ' This is the string we will return, since we are returning a 'C' string, we
            ' must null terminate it.
            stringtomodify = MonitorPath() + vbNullChar

            ' Make sure buffer is large enough
            If stringtomodify.Length > stringlen Then
                ' If string is too long, truncate it.
                stringtomodify = stringtomodify.Substring(0, stringlen - 1) + vbNullChar
            End If

            ' This is the string length to be written
            strlenwritten = stringtomodify.Length

            ' Copy the string to a character array
            ' chararray = stringtomodify.ToCharArray

            ' Copy the string to a byte array
            Dim encoding As New Text.ASCIIEncoding
            Dim byteArray As Byte() = encoding.GetBytes(stringtomodify)

            ' Then marshal that array into the pointer
            Marshal.Copy(byteArray, 0, IntPtr.op_Explicit(lpwstr), strlenwritten)

        Catch ex As Exception
            'it will return -1 in case of any Exception
        End Try

        ' Return string length minus one - do not include the terminating null (standard Windows API practice)
        Return (strlenwritten - 1)
    End Function
    '<Exported("RemoveSpecialChars", 1)> Public Shared Function RemoveSpecialChars(ByVal input As String) As String
    '    Dim fn As String = "C:\Documents and Settings\Rahul\Desktop\RC.txt"
    '    Dim Fs As System.IO.FileStream = New System.IO.FileStream(fn, System.IO.FileMode.Append, System.IO.FileAccess.Write)
    '    Dim tw As System.IO.StreamWriter = New System.IO.StreamWriter(Fs)
    '    tw.WriteLine("Input: GS:EUR/USD:B118525.req Output:" + Regex.Replace("GS:EUR/USD:B118525.req", "[^\w\.@-]", ""))
    '    tw.WriteLine("Input: " + input + " Output:" + Regex.Replace(input, "[^\w\.@-]", ""))
    '    tw.Close()

    '    Return Regex.Replace(input, "[^\w\.@-]", "")
    'End Function

End Class
