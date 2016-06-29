Public Class DisconnectForm
    Public Shared disCount As Integer
    Dim b As New Control()

    Public Sub DisconnectShow(ByVal s As Hashtable)
        Me.Panel1.Controls.Clear()
        Dim j As Integer = 0
        disCount = 0
        Dim obj As Object
        Dim s1 As Trader
        Dim iEnum As IDictionaryEnumerator
        iEnum = s.GetEnumerator()
        While iEnum.MoveNext()
            obj = iEnum.Value
            s1 = CType(obj, Trader)
            If (s1.IsMarketDataConnection = False Or (SettingsHome.getInstance().ExchangeServer <> ExchangeServer.CurrenEx _
                    And SettingsHome.getInstance().ExchangeServer <> ExchangeServer.Dukascopy And SettingsHome.getInstance().ExchangeServer <> ExchangeServer.FxIntegral)) Then
                'If (s1.Stat = Trader.ConnectionStatus.CONNECTED) Or (s1.Stat = Trader.ConnectionStatus.RECONNECTING) Then
                Dim checkbox As New CheckBox
                checkbox.Text = s1.ConnectionId
                checkbox.AutoSize = True
                checkbox.Location = New System.Drawing.Point(10, 10 + (j * 10))
                Me.Panel1.Controls.Add(checkbox)
                j = j + 2
                'End If
            End If
        End While
        Me.Text = "Disconnect"
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim obj As Object
        Dim s1 As Trader
        'Dim count As Integer
        'Dim i As Integer = 0
        For Each b In Me.Panel1.Controls
            If b.GetType().Name = "CheckBox" Then
                Dim c As New CheckBox()
                c = b
                If c.Checked Then
                    'Form1.GetSingletonOrderform().ConnectHT.Contains(c.Text.Trim())
                    obj = Form1.GetSingletonOrderform().ConnectHT.Item(c.Text.Trim())
                    s1 = CType(obj, Trader)

                    disCount = disCount + 1
                    If (s1.QLength > 0) Then
                        If MessageBox.Show(s1.QLength.ToString() + " trades to be executed for id " + s1.ConnectionId + Environment.NewLine + "Are you sure, you want to disconnect?", "AutoShark", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.No Then
                            Continue For
                        End If
                        Form1.GetSingletonOrderform().AddInLogWindow("Alert rejected:" + s1.ConnectionId + " disconnected by user", Color.Red)
                        Form1.GetSingletonOrderform().AddInLogWindow("....Details: " + s1.QLength.ToString() + " queued alerts rejected", Color.Red)
                    End If
                    's1.mDoNotConnect = True
                    'If (s1.IsMarketDataConnection = True) Then Form1.GetSingletonOrderform().DisconnectMarketData()
                    If (s1.IsMarketDataConnection = True) Then
                        Form1.GetSingletonOrderform().CloseConnectionStatusMarketDataByDelegate()
                        'Dim del As New IntArgumentDelegate(AddressOf ShowConnectionStatusMarketData)
                        'BeginInvoke(del, ConnectionStatus.DISCONNECTED)
                    End If

                    s1.logout()
                    Util.WriteDebugLog("Logout ends")
                    'Form1.GetSingletonOrderform().automate.Removetradesettings(c.Text().Trim())

 				    'Form1.GetSingletonOrderform().automate.Removetradesettings(c.Text().Trim())
                    s1.Stat = Trader.ConnectionStatus.DISCONNECTED
                    s1.Ae.ex = Nothing
                    s1.Ae = Nothing
                    s1 = Nothing
                End If
            End If
        Next
        If Not disCount = 0 Then
            Me.Close()
        Else
            MessageBox.Show("Select atleast one connection", "AutoShark")
        End If

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

End Class