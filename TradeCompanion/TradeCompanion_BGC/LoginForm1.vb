
Public Enum ExchangeServer
    CurrenEx = 1
    Ariel = 2
    Espeed = 3
    DBFX = 4
    Gain = 5
    Icap = 6
    Dukascopy = 7
    FxIntegral = 8
End Enum
Public Class LoginForm1

    ' TODO: Insert code to perform custom authentication using the provided username and password 
    ' (See http://go.microsoft.com/fwlink/?LinkId=35339).  
    ' The custom principal can then be attached to the current thread's principal as follows: 
    '     My.User.CurrentPrincipal = CustomPrincipal
    ' where CustomPrincipal is the IPrincipal implementation used to perform authentication. 
    ' Subsequently, My.User will return identity information encapsulated in the CustomPrincipal object
    ' such as the username, display name, etc.
    Dim ds As DataSet
    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click

        Try
            Dim verInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(Application.ExecutablePath)
            'Dim wsScalper As New WSScalper.WebServicesScalper
            Dim result As Integer

            'Check if we need to save the password
            If (chkRememberPassword.Checked = True) Then
                'save usename and password
                Dim key As Microsoft.Win32.RegistryKey
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\BGC\\Settings", True)
                Try
                    key.SetValue("UserName", UsernameTextBox.Text.Trim(), Microsoft.Win32.RegistryValueKind.String)
                    key.SetValue("Password", PasswordTextBox.Text.Trim())
                Catch ex As Exception
                    MsgBox(ex.Message + vbCrLf + "Unable to save Username and Password")
                    Util.WriteDebugLog("Unable to save Username and Password---" & ex.Message )
                End Try
            Else
                'delete saved usename and password
                Dim key As Microsoft.Win32.RegistryKey
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\BGC\\Settings", True)
                Try
                    key.SetValue("UserName", "")
                    key.SetValue("Password", "")
                Catch ex As Exception
                    MsgBox(ex.Message + vbCrLf + "Your Username and Password are still saved in this computer")
                    Util.WriteDebugLog("Your Username and Password are still saved in this computer --- " & ex.Message)
                End Try

            End If

            'continue with login

            'result = 1 'wsScalper.ValidatePasswordVersion(UsernameTextBox.Text.Trim(), PasswordTextBox.Text.Trim(), verInfo.FileMajorPart.ToString() + "." + verInfo.FileMinorPart.ToString() + "." + verInfo.FileBuildPart.ToString())
            'If (result = 1) Then
            If (UsernameTextBox.Text.Trim() = "Franco" And PasswordTextBox.Text.Trim() = "Dummicio") Then
                Dim Keys As SettingsHome = SettingsHome.getInstance()
                Keys.emailID = "" 'WSScalper.GetEmailID(UsernameTextBox.Text.Trim())
                Keys.LoginidTC = UsernameTextBox.Text.Trim()
                Keys.ExchangeServer = CmbServer.SelectedIndex + 1
                'Dim automate As XmlRead = New XmlRead()
                'automate.LoginSettings(PasswordTextBox.Text.Trim())

                'check for the trades which are not logged in, login to sever if found
                Dim ah As AlertsHome = New AlertsHome
                'ds = ah.getUnloggedTrades()
                'If (ds.Tables(0).Rows.Count > 0) Then
                'Try
                'AsyncCallWebServices(Keys)
                'Catch ex As Exception
                'Problem in logging tradec
                '   Util.WriteDebugLog(ex.Message + ex.StackTrace)
                'End Try
                '   End If
                Me.DialogResult = Windows.Forms.DialogResult.OK
            Else
                'ElseIf (result = -1) Then
                MsgBox("Invalid username or password", MsgBoxStyle.OkOnly, "TradeCompanion")
                'Else
                'MsgBox("Connection Error", MsgBoxStyle.OkOnly, "TradeCompanion")
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "TradeCompanion")
            Util.WriteDebugLog("Error -- " & ex.Message)
        End Try

    End Sub

    Public Sub AsyncCallWebServices(ByVal Keys As SettingsHome)
        Dim wsServ As WSScalper.WebServicesScalper = New WSScalper.WebServicesScalper
        AddHandler wsServ.AddOrdersCompleted, AddressOf wsServ_AddOrdersCompleted
        wsServ.AddOrdersAsync(ds, Keys.LoginidTC, EServerDependents.GetEServerSender())
    End Sub

    Public Sub wsServ_AddOrdersCompleted(ByVal sender As Object, ByVal args As WSScalper.AddOrdersCompletedEventArgs)
        If Not args.Error Is Nothing Then
            Exit Sub
        End If

        Dim ah As AlertsHome = New AlertsHome
        Dim res As Boolean
        res = args.Result
        If (res) Then
            For Each dr As DataRow In ds.Tables(0).Rows
                'set the logged bit to true
                ah.SetLogBit(True, CInt(dr("RowID")))
            Next dr
        End If
    End Sub

    Private Sub LinkSignup_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        System.Diagnostics.Process.Start("http://www.scalper.co.uk/tradecompanionform.asp")
    End Sub

    Private Sub LoginForm1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        CmbServer.SelectedIndex = (SettingsHome.getInstance().ExchangeServer - 1)

        'Retrieve saved password
        Dim key As Microsoft.Win32.RegistryKey
        key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\BGC\\Settings")
        Try
            UsernameTextBox.Text = key.GetValue("UserName", "")
            PasswordTextBox.Text = key.GetValue("Password", "")
            If (UsernameTextBox.Text <> "") Then
                chkRememberPassword.Checked = True
            End If
        Catch ex As Exception
            'MsgBox(ex.Message + vbCrLf + "Unable to retrieve saved Username and Password")
            Util.WriteDebugLog("LoginForm1_Load() --- " & ex.Message)
        End Try
        UsernameTextBox.Focus()
    End Sub

    ''' <summary>
    ''' This event is for creating new account when an user click on create account button on TC login form 
    ''' This event come up with new singup window that allows user to create account in TC....
    ''' </summary>
    ''' <param name="sender">System.Object</param>
    ''' <param name="e">System.EventArgs</param>
    ''' <remarks></remarks>
    Private Sub createAccount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles createAccount.Click
        Dim signup As New SignUP
        signup.ShowDialog()
    End Sub
    ''' <summary>
    ''' On click event that allows user to reset the password in case of user forgot the password.
    ''' </summary>
    ''' <param name="sender">System.Object</param>
    ''' <param name="e"> System.Windows.Forms.LinkLabelLinkClickedEventArgs</param>
    ''' <remarks></remarks>
    Private Sub forgotPassword_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles forgotPassword.LinkClicked
        Dim resetPassword As New ResetPassword()
        resetPassword.ShowDialog()
    End Sub
End Class
