Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms

Public Class MyMessageBox

    <Description("Select Flasher Interval")> _
      Public Enum FlashIntervalSpeed
        Slow = 0
        Mid = 1
        Fast = 2
        BlipSlow = 3
        BlipMid = 4
        BlipFast = 5
    End Enum
    Protected Const m_iFlashIntervalMid As Integer = 500
    Protected Const m_iFlashIntervalFast As Integer = 200
    Protected Const m_iFlashIntervalSlow As Integer = 1000
    Protected Const m_iFlashIntervalBlipOn As Integer = 70
    Protected colorOff As Color = SystemColors.Control
    Protected colorOn As Color = Color.LightGreen
    Protected beep As Boolean = False

    Protected m_bIsFlashEnabled As Boolean = False
    Protected iFlashPeriodON As Integer
    Protected iFlashPeriodOFF As Integer
    Protected isOnImage As Boolean
    Public Property FlasherButtonColorOff() As Color
        Get
            Return colorOff
        End Get
        Set(ByVal Value As Color)
            colorOff = Value
        End Set
    End Property
    Public Property FlasherButtonColorOn() As Color
        Get
            Return colorOn
        End Get
        Set(ByVal Value As Color)
            colorOn = Value
        End Set
    End Property
    Public ReadOnly Property FlasherButtonStatus() As Boolean
        Get
            Return m_bIsFlashEnabled
        End Get
    End Property

    Protected Overrides Sub OnPaint(ByVal pe As PaintEventArgs)
        ' TODO: Add custom paint code here

        ' Calling the base class OnPaint
        MyBase.OnPaint(pe)
    End Sub
    Public Sub FlasherButtonStart(ByVal SelectFlashMode As FlashIntervalSpeed)
        Select Case SelectFlashMode
            Case FlashIntervalSpeed.Slow
                iFlashPeriodON = m_iFlashIntervalSlow / 2
                iFlashPeriodOFF = iFlashPeriodON
                Exit Select
            Case FlashIntervalSpeed.Mid
                iFlashPeriodON = m_iFlashIntervalMid / 2
                iFlashPeriodOFF = iFlashPeriodON
                Exit Select
            Case FlashIntervalSpeed.Fast
                iFlashPeriodON = m_iFlashIntervalFast / 2
                iFlashPeriodOFF = iFlashPeriodON
                Exit Select
            Case FlashIntervalSpeed.BlipSlow
                iFlashPeriodON = m_iFlashIntervalBlipOn
                iFlashPeriodOFF = m_iFlashIntervalSlow - m_iFlashIntervalBlipOn
                Exit Select
            Case FlashIntervalSpeed.BlipMid
                iFlashPeriodON = m_iFlashIntervalBlipOn
                iFlashPeriodOFF = m_iFlashIntervalMid - m_iFlashIntervalBlipOn
                Exit Select
            Case FlashIntervalSpeed.BlipFast
                iFlashPeriodON = m_iFlashIntervalBlipOn
                iFlashPeriodOFF = m_iFlashIntervalFast - m_iFlashIntervalBlipOn
                Exit Select
            Case Else
                Return
        End Select
        'If m_bIsFlashEnabled = False Then
        m_bIsFlashEnabled = True
        Timer1.Interval = iFlashPeriodON
        'MyBase.BackColor = colorOn
        PictureBox1.Image = My.Resources.vbCriticalOn
        isOnImage = True
        Timer1.Start()
        'End If
    End Sub
    <Description("Disable button flashing")> _
    <Category("Layout")> _
    <Browsable(True)> _
    Public Sub FlasherButtonStop()
        If Not Timer1 Is Nothing Then
            MyBase.BackColor = colorOff
            Timer1.Stop()
            Timer1.Dispose()
        End If
        m_bIsFlashEnabled = False
    End Sub

    Public Sub FlasherButtonColor(ByVal colorOn As Color, ByVal colorOff As Color, ByVal beep As Boolean, ByVal text As String, ByVal headerText As String)
        Me.colorOn = colorOn
        Me.colorOff = colorOff
        Me.Text = headerText
        lblMessage.Text = text
        Me.beep = beep

    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Timer1.Stop()
        Me.Dispose()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If isOnImage = False Then
            PictureBox1.Image = My.Resources.vbCriticalOn
            Timer1.Interval = iFlashPeriodON
            'If Me.beep Then
            'System.Media.SystemSounds.Beep.Play()
            'End If
            isOnImage = True
        Else
            PictureBox1.Image = Nothing
            Timer1.Interval = iFlashPeriodOFF
            isOnImage = False
        End If
        PictureBox1.Invalidate()
      
        'If MyBase.BackColor = colorOff Then
        '    MyBase.BackColor = colorOn
        '    Timer1.Interval = iFlashPeriodON
        '    If Me.beep Then
        '        System.Media.SystemSounds.Beep.Play()
        '    End If
        'Else
        '    MyBase.BackColor = colorOff
        '    Timer1.Interval = iFlashPeriodOFF
        'End If
        'Me.Invalidate()
    End Sub
End Class