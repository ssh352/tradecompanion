<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HelpScalper
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
        Me.RTBUserManual = New System.Windows.Forms.RichTextBox
        Me.SuspendLayout()
        '
        'RTBUserManual
        '
        Me.RTBUserManual.BackColor = System.Drawing.Color.White
        Me.RTBUserManual.Location = New System.Drawing.Point(1, 2)
        Me.RTBUserManual.Name = "RTBUserManual"
        Me.RTBUserManual.ReadOnly = True
        Me.RTBUserManual.Size = New System.Drawing.Size(698, 568)
        Me.RTBUserManual.TabIndex = 0
        Me.RTBUserManual.Text = ""
        '
        'HelpScalper
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(699, 568)
        Me.Controls.Add(Me.RTBUserManual)
        Me.MaximizeBox = False
        Me.Name = "HelpScalper"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "User's Manual"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents RTBUserManual As System.Windows.Forms.RichTextBox
End Class
