''<Purpose>Pop Up message Box</Purpose>
''
''<Usage>Pop up message window,
'' By usign this class your application can send message that will pop up window</Usage>
''
''<Requirements>This code is developed with farmework .NET 2.0  </Requirements>
''
''<Author> 
''Vijay N. Majagaonkar
''S7 Software Solutions http://www.s7software.com
''</Author>

Public Class PopupMsgBox


    ''' <summary>
    ''' Method to popup window with out any message
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ShowMessage()
        msgTxt.Text = ""
        Me.Show()
        FadingForm()
    End Sub

    ''' <summary>
    ''' This Method show message on window
    ''' </summary>
    ''' <param name="message">Message to display on popup window</param>
    ''' <remarks></remarks>
    Public Sub ShowMessage(ByVal message As String)
        msgTxt.Text = message
        Me.Show()
        FadingForm()
    End Sub

    ''' <summary>
    ''' This Method show message on window with time interval set by user
    ''' </summary>
    ''' <param name="message">Message to display on popup window</param>
    ''' <param name="timeInterval">How much time popup window to display</param>
    ''' <remarks></remarks>
    Public Sub ShowMessage(ByVal message As String, ByVal timeInterval As Integer)
        msgTxt.Text = message
        Me.Show()
        FadingForm()
    End Sub

    ''' <summary>
    ''' This Method show message on window
    ''' </summary>
    ''' <param name="message">Message to display on popup window</param>
    ''' <param name="timeInterval">How much time popup window to display</param>
    ''' <param name="lable">Label to show on Top of message Box</param>
    ''' <remarks></remarks>
    Public Sub ShowMessage(ByVal message As String, ByVal timeInterval As Integer, ByVal lable As String)
        msgTxt.Text = message
        Label.Text = lable
        Me.Show()
        Me.Refresh()
        FadingForm()
    End Sub

    ''' <summary>
    ''' Here we make window to display at the left bottom of the screen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PopupMsgBox_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Location = New System.Drawing.Point((Screen.PrimaryScreen.Bounds.Width - (Size.Width + 5)), (Screen.PrimaryScreen.Bounds.Height - (Size.Height + 35)))
    End Sub

    ''' <summary>
    ''' This Method is used to fade out while closeing popup window
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FadingForm()

        Dim iCount As Integer

        For iCount = 1000 To 10 Step -10
            Me.Opacity = iCount / 100
            'Me.Refresh()
            Threading.Thread.Sleep(50)
        Next

        Me.Dispose(True)

    End Sub

End Class
