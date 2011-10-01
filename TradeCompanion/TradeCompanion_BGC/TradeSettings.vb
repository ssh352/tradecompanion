Public Class TradeSettings
    Dim settingsTrade As New SettingsTrade


    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub


    Private Sub TradeSettings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        settingsTrade.getSettings()

        Select Case settingsTrade.TradeTypeAuto
            Case 3
                cmbTradeTypeAutomatic.SelectedIndex = 0
            Case 1
                cmbTradeTypeAutomatic.SelectedIndex = 1
        End Select

        Select Case settingsTrade.TradeTypeManual
            Case 3
                cmbTradeTypeManualOrder.SelectedIndex = 0
            Case 1
                cmbTradeTypeManualOrder.SelectedIndex = 1
        End Select

        Select Case settingsTrade.TradeTypeAutoOver10Mil
            Case 3
                cmbTradeTypeOver10mil.SelectedIndex = 0
            Case 1
                cmbTradeTypeOver10mil.SelectedIndex = 1
        End Select
        DiscardAlertInterval.Value = SettingsHome.getInstance().DiscardAlertInterval
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If (cmbTradeTypeManualOrder.SelectedIndex = 0) Then 'IOC
            settingsTrade.TradeTypeManual = 3
        ElseIf (cmbTradeTypeManualOrder.SelectedIndex = 1) Then 'GTC
            settingsTrade.TradeTypeManual = 1
        End If

        If (cmbTradeTypeAutomatic.SelectedIndex = 0) Then 'IOC
            settingsTrade.TradeTypeAuto = 3
        ElseIf (cmbTradeTypeAutomatic.SelectedIndex = 1) Then 'GTC
            settingsTrade.TradeTypeAuto = 1
        End If

        If (cmbTradeTypeOver10mil.SelectedIndex = 0) Then 'IOC
            settingsTrade.TradeTypeAutoOver10Mil = 3
        ElseIf (cmbTradeTypeOver10mil.SelectedIndex = 1) Then 'GTC
            settingsTrade.TradeTypeAutoOver10Mil = 1
        End If

        SettingsHome.getInstance().DiscardAlertInterval = DiscardAlertInterval.Value

        settingsTrade.setSettings()

        'Form1.GetSingletonOrderform().automate.TradeSettings()

        Me.DialogResult = System.Windows.Forms.DialogResult.OK

        Me.Close()
    End Sub
End Class