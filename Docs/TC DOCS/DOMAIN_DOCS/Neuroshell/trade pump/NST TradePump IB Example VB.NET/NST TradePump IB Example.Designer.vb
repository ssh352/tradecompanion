<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class Form1
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents NSTOrders1 As AxNSTOrdersAPI.AxNSTOrders
	Public WithEvents Tws1 As AxTWSLib.AxTws
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.NSTOrders1 = New AxNSTOrdersAPI.AxNSTOrders
        Me.Tws1 = New AxTWSLib.AxTws
        CType(Me.NSTOrders1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Tws1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'NSTOrders1
        '
        Me.NSTOrders1.Enabled = True
        Me.NSTOrders1.Location = New System.Drawing.Point(24, 16)
        Me.NSTOrders1.Name = "NSTOrders1"
        Me.NSTOrders1.OcxState = CType(resources.GetObject("NSTOrders1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.NSTOrders1.Size = New System.Drawing.Size(45, 41)
        Me.NSTOrders1.TabIndex = 0
        '
        'Tws1
        '
        Me.Tws1.Enabled = True
        Me.Tws1.Location = New System.Drawing.Point(88, 16)
        Me.Tws1.Name = "Tws1"
        Me.Tws1.OcxState = CType(resources.GetObject("Tws1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.Tws1.Size = New System.Drawing.Size(153, 25)
        Me.Tws1.TabIndex = 0
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(389, 67)
        Me.Controls.Add(Me.NSTOrders1)
        Me.Controls.Add(Me.Tws1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 30)
        Me.Name = "Form1"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "NST TradePump IB Example"
        Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
        CType(Me.NSTOrders1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Tws1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class