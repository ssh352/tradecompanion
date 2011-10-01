<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DBFXFormClient
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DBFXFormClient))
        Me.AxTradeDeskEventsSink1 = New AxFXCore.AxTradeDeskEventsSink
        Me.tmrMrk = New System.Windows.Forms.Timer(Me.components)
        CType(Me.AxTradeDeskEventsSink1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AxTradeDeskEventsSink1
        '
        Me.AxTradeDeskEventsSink1.Enabled = True
        Me.AxTradeDeskEventsSink1.Location = New System.Drawing.Point(31, 16)
        Me.AxTradeDeskEventsSink1.Name = "AxTradeDeskEventsSink1"
        Me.AxTradeDeskEventsSink1.OcxState = CType(resources.GetObject("AxTradeDeskEventsSink1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxTradeDeskEventsSink1.Size = New System.Drawing.Size(25, 25)
        Me.AxTradeDeskEventsSink1.TabIndex = 0
        '
        'tmrMrk
        '
        '
        'DBFXFormClient
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(115, 50)
        Me.Controls.Add(Me.AxTradeDeskEventsSink1)
        Me.Name = "DBFXFormClient"
        Me.Text = "DBFXFormClient"
        CType(Me.AxTradeDeskEventsSink1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents AxTradeDeskEventsSink1 As AxFXCore.AxTradeDeskEventsSink
    Friend WithEvents tmrMrk As System.Windows.Forms.Timer
End Class
