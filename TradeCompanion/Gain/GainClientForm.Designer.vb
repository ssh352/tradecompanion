<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GainClientForm
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
        Me.components = New System.ComponentModel.Container
        Me.StatusChk = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'StatusChk
        '
        Me.StatusChk.Interval = 10000
        '
        'GainClientForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(115, 38)
        Me.Name = "GainClientForm"
        Me.Text = "GainClientForm"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents StatusChk As System.Windows.Forms.Timer
End Class
