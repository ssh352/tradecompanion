<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SoundSettings
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.RadioButtonMyOwnSound = New System.Windows.Forms.RadioButton
        Me.RadioButtonDefault = New System.Windows.Forms.RadioButton
        Me.RadioButtonNoSound = New System.Windows.Forms.RadioButton
        Me.btnBrowse = New System.Windows.Forms.Button
        Me.txtMySound = New System.Windows.Forms.TextBox
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.OK_Button = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.RadioButtonMyOwnSound)
        Me.GroupBox1.Controls.Add(Me.RadioButtonDefault)
        Me.GroupBox1.Controls.Add(Me.RadioButtonNoSound)
        Me.GroupBox1.Controls.Add(Me.btnBrowse)
        Me.GroupBox1.Controls.Add(Me.txtMySound)
        Me.GroupBox1.Location = New System.Drawing.Point(10, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(344, 135)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Play sound when trade go through"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(0, 13)
        Me.Label1.TabIndex = 7
        '
        'RadioButtonMyOwnSound
        '
        Me.RadioButtonMyOwnSound.AutoSize = True
        Me.RadioButtonMyOwnSound.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.RadioButtonMyOwnSound.Location = New System.Drawing.Point(16, 93)
        Me.RadioButtonMyOwnSound.Name = "RadioButtonMyOwnSound"
        Me.RadioButtonMyOwnSound.Size = New System.Drawing.Size(129, 17)
        Me.RadioButtonMyOwnSound.TabIndex = 6
        Me.RadioButtonMyOwnSound.TabStop = True
        Me.RadioButtonMyOwnSound.Text = "My Own Sound (.wav)"
        Me.RadioButtonMyOwnSound.UseVisualStyleBackColor = True
        '
        'RadioButtonDefault
        '
        Me.RadioButtonDefault.AutoSize = True
        Me.RadioButtonDefault.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.RadioButtonDefault.Location = New System.Drawing.Point(16, 56)
        Me.RadioButtonDefault.Name = "RadioButtonDefault"
        Me.RadioButtonDefault.Size = New System.Drawing.Size(114, 17)
        Me.RadioButtonDefault.TabIndex = 5
        Me.RadioButtonDefault.TabStop = True
        Me.RadioButtonDefault.Text = "Use Default (Beep)"
        Me.RadioButtonDefault.UseVisualStyleBackColor = True
        '
        'RadioButtonNoSound
        '
        Me.RadioButtonNoSound.AutoSize = True
        Me.RadioButtonNoSound.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.RadioButtonNoSound.Location = New System.Drawing.Point(16, 19)
        Me.RadioButtonNoSound.Name = "RadioButtonNoSound"
        Me.RadioButtonNoSound.Size = New System.Drawing.Size(72, 17)
        Me.RadioButtonNoSound.TabIndex = 4
        Me.RadioButtonNoSound.TabStop = True
        Me.RadioButtonNoSound.Text = "No Sound"
        Me.RadioButtonNoSound.UseVisualStyleBackColor = True
        '
        'btnBrowse
        '
        Me.btnBrowse.BackColor = System.Drawing.Color.LightSteelBlue
        Me.btnBrowse.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.btnBrowse.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.btnBrowse.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBrowse.Location = New System.Drawing.Point(264, 90)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(74, 23)
        Me.btnBrowse.TabIndex = 2
        Me.btnBrowse.Text = "Bro&wse"
        Me.btnBrowse.UseVisualStyleBackColor = False
        '
        'txtMySound
        '
        Me.txtMySound.AcceptsReturn = True
        Me.txtMySound.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMySound.Location = New System.Drawing.Point(152, 92)
        Me.txtMySound.Name = "txtMySound"
        Me.txtMySound.Size = New System.Drawing.Size(106, 20)
        Me.txtMySound.TabIndex = 3
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.BackColor = System.Drawing.Color.LightSteelBlue
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.Cancel_Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.Cancel_Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.Cancel_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Cancel_Button.Location = New System.Drawing.Point(182, 157)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 6
        Me.Cancel_Button.Text = "Cancel"
        Me.Cancel_Button.UseVisualStyleBackColor = False
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.BackColor = System.Drawing.Color.LightSteelBlue
        Me.OK_Button.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.OK_Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.OK_Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.OK_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.OK_Button.Location = New System.Drawing.Point(109, 157)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 5
        Me.OK_Button.Text = "OK"
        Me.OK_Button.UseVisualStyleBackColor = False
        '
        'SoundSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(365, 188)
        Me.Controls.Add(Me.Cancel_Button)
        Me.Controls.Add(Me.OK_Button)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.Name = "SoundSettings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SoundSettings"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents txtMySound As System.Windows.Forms.TextBox
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents RadioButtonMyOwnSound As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonDefault As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonNoSound As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
