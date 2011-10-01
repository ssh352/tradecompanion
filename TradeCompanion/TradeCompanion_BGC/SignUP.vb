Imports System.Text.RegularExpressions
Public Class SignUP

    Private Sub cmdSignUP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSignUP.Click
        Dim wsServ As WSScalper.WebServicesScalper = New WSScalper.WebServicesScalper
        Dim emailID As String = txtEmailid.Text
        Dim username As String = txtFirstname.Text + " " + txtLastName.Text
        Dim phoneNo As String = txtPhoneNo.Text
        Dim address As String = txtAddress.Text
        Dim city As String = txtCity.Text
        Dim country As String = txtCountry.Text

        If (Validate()) Then
            Dim result As Integer = wsServ.AddUser(emailID, username, emailID, phoneNo, address, city, country)
            If (result = -10 Or result = -11) Then
                MessageBox.Show("Email id already registered", "Tradercompanion")
            ElseIf (result = -2) Then
                MessageBox.Show("Connection Problem, please try after some time", "Tradercompanion")
            ElseIf (result = -4) Then
                MessageBox.Show("Email error, please try after some time", "Tradercompanion")
            ElseIf (result = -5) Then
                MessageBox.Show("Database error, please contact software provider", "Tradercompanion")
            ElseIf (result > 0) Then
                MessageBox.Show("Thank you  for signing up, Email is sent to you with login details. ", "Tradercompanion")
                'Set the Registered to true
                Dim key As Microsoft.Win32.RegistryKey
                key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\BGC\\Settings")
                key.SetValue("Registered", Util.Encrypt("true", True))
                Me.Close()
            End If
        End If

    End Sub

    Private Shadows Function Validate() As Boolean
        Dim msgSting As String = "Mandatory Fields:"
        Dim dataError As String = "Validation Error:"

        If txtEmailid.Text = "" Then
            msgSting = msgSting + Environment.NewLine + "Email ID"
        Else
            Dim strRegex As String = "^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + "\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + ".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
            Dim re As Regex = New Regex(strRegex)
            If Not re.IsMatch(txtEmailid.Text.Trim) Then
                dataError = dataError + Environment.NewLine + "Invalid Email ID"
            End If
        End If

        If txtFirstname.Text = "" Then
            msgSting = msgSting + Environment.NewLine + "First Name"
        End If
        If txtAddress.Text = "" Then
            msgSting = msgSting + Environment.NewLine + "Address"
        End If
        If txtCity.Text = "" Then
            msgSting = msgSting + Environment.NewLine + "City"
        End If
        If txtCountry.Text = "" Then
            msgSting = msgSting + Environment.NewLine + "Country"
        End If

        If txtPhoneNo.Text = "" Then
            msgSting = msgSting + Environment.NewLine + "Phone No"
        End If
 

        If Not (msgSting = "Mandatory Fields:") Then
            MessageBox.Show(msgSting, "Tradecompanion")
            Return False
        Else
            If Not (dataError = "Validation Error:") Then
                MessageBox.Show(dataError, "Tradecompanion")
                Return False
            Else
                Return True
            End If
        End If
    End Function

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Me.Dispose()
    End Sub

   
    Private Sub SignUP_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class