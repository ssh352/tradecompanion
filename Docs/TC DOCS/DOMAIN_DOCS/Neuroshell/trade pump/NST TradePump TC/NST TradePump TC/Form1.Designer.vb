<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.NSTOrders1 = New AxNSTOrdersAPI.AxNSTOrders
        CType(Me.NSTOrders1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'NSTOrders1
        '
        Me.NSTOrders1.Enabled = True
        Me.NSTOrders1.Location = New System.Drawing.Point(37, 12)
        Me.NSTOrders1.Name = "NSTOrders1"
        Me.NSTOrders1.OcxState = CType(resources.GetObject("NSTOrders1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.NSTOrders1.Size = New System.Drawing.Size(50, 38)
        Me.NSTOrders1.TabIndex = 0
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 77)
        Me.Controls.Add(Me.NSTOrders1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.ShowInTaskbar = False
        Me.Text = "TradeCompanion Trade Pump"
        CType(Me.NSTOrders1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents NSTOrders1 As AxNSTOrdersAPI.AxNSTOrders

End Class
