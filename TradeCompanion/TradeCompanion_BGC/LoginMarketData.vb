Public Class LoginMarketData
    Private keys As SettingsHome
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        If keys.ExchangeServer = ExchangeServer.CurrenEx Then
            If Me.txtSender.Text.Trim = "" Or Me.txtIP.Text.Trim = "" Or Me.txtPort.Text.Trim = "" Or Me.txtTarget.Text.Trim = "" Or Me.txtPassword.Text.Trim = "" Then
                MsgBox("All field are mandatory", MsgBoxStyle.OkOnly, "Logon Alert")
            Else
                Util.WriteDebugLog("------------Market Data Connection-----------------")
                Util.WriteDebugLog("...Settings:")
                Util.WriteDebugLog("            IP            -" + Me.txtIP.Text.ToString)
                Util.WriteDebugLog("            Port          -" + Me.txtPort.Text.ToString)
                Util.WriteDebugLog("            Sender        -" + Me.txtSender.Text.ToString)
                Util.WriteDebugLog("            Target        -" + Me.txtTarget.Text.ToString)
                Util.WriteDebugLog("---------------------------------------------")
                keys.LoginIPMarketData = txtIP.Text
                keys.LoginPortMarketData = txtPort.Text
                keys.LoginSenderMarketData = txtSender.Text
                keys.CXLoginPassword = txtPassword.Text
                keys.LoginTargetMarketData = txtTarget.Text
                Me.DialogResult = System.Windows.Forms.DialogResult.OK
                Me.Close()
            End If

        ElseIf keys.ExchangeServer = ExchangeServer.Dukascopy Then

            If Me.dkSendertxt.Text.Trim = "" Or Me.dkIPtxt.Text.Trim = "" Or Me.dkPorttxt.Text.Trim = "" Or Me.dkTargettxt.Text.Trim = "" Or Me.dkPasswdtxt.Text.Trim = "" Or Me.dkUserNametxt.Text.Trim = "" Then
                MsgBox("All field are mandatory", MsgBoxStyle.OkOnly, "Logon Alert")
            Else
                Util.WriteDebugLog("------------Market Data Connection-----------------")
                Util.WriteDebugLog("...Settings:")
                Util.WriteDebugLog("            IP            -" + Me.dkIPtxt.Text.ToString)
                Util.WriteDebugLog("            Port          -" + Me.dkPorttxt.Text.ToString)
                Util.WriteDebugLog("            Sender        -" + Me.dkSendertxt.Text.ToString)
                Util.WriteDebugLog("            Target        -" + Me.dkTargettxt.Text.ToString)
                Util.WriteDebugLog("---------------------------------------------")
                keys.DUKASLoginIPMarketData = dkIPtxt.Text
                keys.DUKASLoginPortMarketData = dkPorttxt.Text
                keys.DUKASLoginSenderMarketData = dkSendertxt.Text
                keys.DUKASLoginUserNameMarketData = dkUserNametxt.Text
                keys.DUKASLoginPassword = dkPasswdtxt.Text
                keys.DUKASLoginTargetMarketData = dkTargettxt.Text
                Me.DialogResult = System.Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        Else

            If Me.FXtxtIP.Text.Trim = "" Or Me.FXtxtLegalEntity.Text.Trim = "" Or Me.FXtxtPass.Text.Trim = "" Or Me.FXtxtPort.Text.Trim = "" Or Me.FXtxtSender.Text.Trim = "" Or Me.FXtxtTarget.Text.Trim = "" Or Me.FXtxtUserName.Text.Trim = "" Then
                MsgBox("All field are mandatory", MsgBoxStyle.OkOnly, "Logon Alert")
            Else
                Util.WriteDebugLog("------------Market Data Connection-----------------")
                Util.WriteDebugLog("...Settings:")
                Util.WriteDebugLog("            IP            -" + Me.FXtxtIP.Text.ToString)
                Util.WriteDebugLog("            Port          -" + Me.FXtxtPort.Text.ToString)
                Util.WriteDebugLog("            Sender        -" + Me.FXtxtSender.Text.ToString)
                Util.WriteDebugLog("            Target        -" + Me.FXtxtTarget.Text.ToString)
                Util.WriteDebugLog("---------------------------------------------")
                keys.FxIntegralIPMarketData = FXtxtIP.Text
                keys.FxIntegralPortMarketData = FXtxtPort.Text
                keys.FxIntegralSenderMarketData = FXtxtSender.Text
                keys.FxIntegralUserNameMarketData = FXtxtUserName.Text
                keys.FxIntLoginPassword = FXtxtPass.Text
                keys.FxIntegralTargetMarketData = FXtxtTarget.Text
                keys.FxIntegralMarkLegalEntity = FXtxtLegalEntity.Text
                Me.DialogResult = System.Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        End If

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub LoginMarketData_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        keys = SettingsHome.getInstance()
        If keys.ExchangeServer = ExchangeServer.CurrenEx Then
            currenexPan.Visible = True
            txtIP.Text = keys.LoginIPMarketData
            txtPort.Text = keys.LoginPortMarketData
            txtSender.Text = keys.LoginSenderMarketData
            txtTarget.Text = keys.LoginTargetMarketData
        ElseIf keys.ExchangeServer = ExchangeServer.Dukascopy Then
            dukascopyPan.Visible = True
            dkIPtxt.Text = keys.DUKASLoginIPMarketData
            dkPorttxt.Text = keys.DUKASLoginPortMarketData
            dkSendertxt.Text = keys.DUKASLoginSenderMarketData
            dkTargettxt.Text = keys.DUKASLoginTargetMarketData
            dkUserNametxt.Text = keys.DUKASLoginUserNameMarketData
        Else
            PnlFXintegral.Visible = True
            FXtxtIP.Text = keys.FxIntegralIPMarketData
            FXtxtPort.Text = keys.FxIntegralPortMarketData
            FXtxtSender.Text = keys.FxIntegralSenderMarketData
            FXtxtTarget.Text = keys.FxIntegralTargetMarketData
            FXtxtLegalEntity.Text = keys.FxIntegralMarkLegalEntity
        End If
        
        'If Form1.GetSingletonOrderform().crashtest Then
        '    Me.OK_Button.PerformClick()  'crash auto performing task
        'End If
    End Sub

End Class
