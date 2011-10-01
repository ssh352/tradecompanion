<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PopupMsgBox
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.msgTxt = New System.Windows.Forms.TextBox
        Me.Label = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'msgTxt
        '
        Me.msgTxt.BackColor = System.Drawing.SystemColors.Info
        Me.msgTxt.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.msgTxt.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.msgTxt.Enabled = False
        Me.msgTxt.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.msgTxt.Location = New System.Drawing.Point(0, 20)
        Me.msgTxt.Multiline = True
        Me.msgTxt.Name = "msgTxt"
        Me.msgTxt.Size = New System.Drawing.Size(321, 86)
        Me.msgTxt.TabIndex = 1
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.Label.Location = New System.Drawing.Point(0, 0)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(38, 17)
        Me.Label.TabIndex = 2
        Me.Label.Text = "Vijay"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'PopupMsgBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.ClientSize = New System.Drawing.Size(321, 106)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label)
        Me.Controls.Add(Me.msgTxt)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "PopupMsgBox"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.TransparencyKey = System.Drawing.Color.Transparent
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents msgTxt As System.Windows.Forms.TextBox
    Friend WithEvents Label As System.Windows.Forms.Label

End Class
