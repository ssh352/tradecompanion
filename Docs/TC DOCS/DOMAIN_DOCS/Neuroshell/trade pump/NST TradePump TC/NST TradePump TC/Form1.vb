Imports System.IO
Public Class Form1
    Private Const MARKET_ORDER As Short = 1
    Private Const STOP_ORDER As Short = 2
    Private Const LIMIT_ORDER As Short = 3
    Private Const STOPLIMIT_ORDER As Short = 4
    Private Const MARKETCLOSE_ORDER As Short = 5
    Dim connected As Boolean = True

    Private Const LONGENTRY_ACTION As Short = 1 'buy
    Private Const SHORTENTRY_ACTION As Short = 2 'sell
    Private Const LONGEXIT_ACTION As Short = 3
    Private Const SHORTEXIT_ACTION As Short = 4

    Private Sub NSTOrders1_CancelOrder(ByVal sender As Object, ByVal e As AxNSTOrdersAPI.__NSTOrders_CancelOrderEvent) Handles NSTOrders1.CancelOrder
        'MessageBox.Show(e.orderId)
    End Sub

    Private Sub NSTOrders1_Logon(ByVal sender As Object, ByVal e As AxNSTOrdersAPI.__NSTOrders_LogonEvent) Handles NSTOrders1.Logon
        If connected Then Exit Sub
        Dim txt As String = "Connecting to the Trade companion"
        txt = txt + Chr(32) + e.account + Chr(32) + e.password
        NSTOrders1.Message(txt)
    End Sub

    Private Sub NSTOrders1_ModifyOrder(ByVal sender As Object, ByVal e As AxNSTOrdersAPI.__NSTOrders_ModifyOrderEvent) Handles NSTOrders1.ModifyOrder
        'MessageBox.Show(e.orderId + "@" + e.stopPrice.ToString())
    End Sub

    Private Sub NSTOrders1_NewOrder(ByVal sender As Object, ByVal e As AxNSTOrdersAPI.__NSTOrders_NewOrderEvent) Handles NSTOrders1.NewOrder
        Try

            If (e.orderType = 1) Then
                Dim ActionString As String
                Dim duration As Integer = 1

                Select Case e.action
                    Case LONGENTRY_ACTION : ActionString = "Buy"
                    Case SHORTENTRY_ACTION : ActionString = "Sell"
                    Case LONGEXIT_ACTION : ActionString = "Sell"
                    Case SHORTEXIT_ACTION : ActionString = "Buy"
                End Select

                If e.duration = "GTC" Then
                    duration = 1
                End If

                If IO.Directory.Exists("C:\Program Files\BGC\EXPORT") Then
                    Dim str As String = e.ticker.Replace("/", "")
                    Dim workspace As String
                    Dim dateno As String = Date.Today.Year.ToString() + Date.Today.Month.ToString() + Date.Today.Day.ToString()
                    Dim timeno As String = Now.Hour.ToString() + Now.Minute.ToString() + Now.Second.ToString()
                    Dim filename As String = "C:\Program Files\BGC\EXPORT\" + str + timeno + Now.Millisecond.ToString() + ".req"
                    Dim fs As New FileStream(filename, FileMode.Create, FileAccess.Write)
                    Dim s As New StreamWriter(fs)
                    s.BaseStream.Seek(0, SeekOrigin.End)
                    workspace = e.chartName.Remove(e.chartName.IndexOf("."), 4)
                    ' Date Time SectorType Symbol Action Volume ChartIdentifier[Ignored] Chart Name
                    s.WriteLine(dateno + Chr(32) + timeno + Chr(32) + e.secType + Chr(32) + e.ticker + Chr(32) + ActionString + Chr(32) + e.shares.ToString() + Chr(32) + duration.ToString() + Chr(32) + workspace)
                    s.Close()
                End If
            Else
                'NSTOrders1.Message("Non-market order, Check for strategy")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub NSTOrders1_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles NSTOrders1.Unload
        Me.Close()
    End Sub
End Class

