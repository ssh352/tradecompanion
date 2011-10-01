Public Class SoundSettings
    Dim settingsSound As New SettingsSound
    Dim openFileDialog1 As New OpenFileDialog()
    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        'OpenFileDialog1.ShowDialog()
        openFileDialog1.InitialDirectory = "d:\"
        'openFileDialog1.Filter = "sound files (*.wav)|*.wav|All files (*.*)|*.*"
        openFileDialog1.Filter = "sound files (*.wav)|*.wav"
        openFileDialog1.FilterIndex = 2
        OpenFileDialog1.RestoreDirectory = True
        openFileDialog1.Multiselect = False
        If openFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim st() As String
            st = openFileDialog1.FileName.Split("\")
            txtMySound.Text = st(st.Length - 1).ToString()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SoundSettings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtMySound.Enabled = False
        btnBrowse.Enabled = False

        settingsSound.getSettings()
        Me.RadioButtonDefault.Checked = settingsSound.UseDefaultSound
        Me.RadioButtonNoSound.Checked = settingsSound.NoSound
        Me.RadioButtonMyOwnSound.Checked = settingsSound.OwnSound
        txtMySound.Text = settingsSound.OwnSoundName
        'If (settingsSound.OwnSound = True) Then
        'txtMySound.Text = settingsSound.OwnSoundName
        'End If
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        'validation for My Own Sound, if selected    
        If (RadioButtonMyOwnSound.Checked = True) Then
            If (txtMySound.Text <> "") Then
                'Copy the file to sound directory
                If (openFileDialog1.FileName <> "") Then 'file already in sounds directory
                    If (System.IO.File.Exists(Application.StartupPath + "\Sounds\" + txtMySound.Text.Trim()) <> True) Then
                        Try
                            FileCopy(openFileDialog1.FileName, Application.StartupPath + "\Sounds\" + txtMySound.Text.Trim())
                        Catch ex As Exception
                            Util.WriteDebugLog("SoundSettings ---" & ex.Message)
                            MessageBox.Show(ex.Message, "TradeCompanion")
                        End Try
                    End If
                End If
            Else
                MsgBox("Select the sound file", MsgBoxStyle.OkOnly, "TradeCompanion")
                Exit Sub
            End If
        End If
        settingsSound.UseDefaultSound = RadioButtonDefault.Checked
        settingsSound.NoSound = RadioButtonNoSound.Checked
        settingsSound.OwnSound = RadioButtonMyOwnSound.Checked
        settingsSound.OwnSoundName = txtMySound.Text
        settingsSound.setSettings()
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub RadioButtonMyOwnSound_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonMyOwnSound.CheckedChanged
        If RadioButtonMyOwnSound.Checked = True Then
            txtMySound.Enabled = True
            txtMySound.ReadOnly = True
            btnBrowse.Enabled = True
        Else
            txtMySound.Enabled = False
            btnBrowse.Enabled = False
        End If
    End Sub
End Class