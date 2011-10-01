<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmExportFilter
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
        Me.lblEndDate = New System.Windows.Forms.Label
        Me.lblStartDate = New System.Windows.Forms.Label
        Me.pkrEndDate = New System.Windows.Forms.DateTimePicker
        Me.pkrStartDate = New System.Windows.Forms.DateTimePicker
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblEndDate)
        Me.GroupBox1.Controls.Add(Me.lblStartDate)
        Me.GroupBox1.Controls.Add(Me.pkrEndDate)
        Me.GroupBox1.Controls.Add(Me.pkrStartDate)
        Me.GroupBox1.Controls.Add(Me.btnCancel)
        Me.GroupBox1.Controls.Add(Me.btnExport)
        Me.GroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GroupBox1.Location = New System.Drawing.Point(1, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(376, 66)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        '
        'lblEndDate
        '
        Me.lblEndDate.AutoSize = True
        Me.lblEndDate.Location = New System.Drawing.Point(146, 15)
        Me.lblEndDate.Name = "lblEndDate"
        Me.lblEndDate.Size = New System.Drawing.Size(52, 13)
        Me.lblEndDate.TabIndex = 6
        Me.lblEndDate.Text = "End Date"
        '
        'lblStartDate
        '
        Me.lblStartDate.AutoSize = True
        Me.lblStartDate.Location = New System.Drawing.Point(8, 15)
        Me.lblStartDate.Name = "lblStartDate"
        Me.lblStartDate.Size = New System.Drawing.Size(55, 13)
        Me.lblStartDate.TabIndex = 5
        Me.lblStartDate.Text = "Start Date"
        '
        'pkrEndDate
        '
        Me.pkrEndDate.CustomFormat = "MM/dd/yyyy"
        Me.pkrEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.pkrEndDate.Location = New System.Drawing.Point(149, 31)
        Me.pkrEndDate.Name = "pkrEndDate"
        Me.pkrEndDate.ShowCheckBox = True
        Me.pkrEndDate.Size = New System.Drawing.Size(121, 20)
        Me.pkrEndDate.TabIndex = 4
        '
        'pkrStartDate
        '
        Me.pkrStartDate.CustomFormat = "MM/dd/yyyy"
        Me.pkrStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.pkrStartDate.Location = New System.Drawing.Point(11, 31)
        Me.pkrStartDate.Name = "pkrStartDate"
        Me.pkrStartDate.ShowCheckBox = True
        Me.pkrStartDate.Size = New System.Drawing.Size(121, 20)
        Me.pkrStartDate.TabIndex = 3
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.LightSteelBlue
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnCancel.Location = New System.Drawing.Point(292, 38)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(78, 21)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnExport
        '
        Me.btnExport.BackColor = System.Drawing.Color.LightSteelBlue
        Me.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnExport.Location = New System.Drawing.Point(292, 11)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(78, 21)
        Me.btnExport.TabIndex = 1
        Me.btnExport.Text = "Export"
        Me.btnExport.UseVisualStyleBackColor = False
        '
        'frmExportFilter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(380, 69)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(200, 200)
        Me.MaximizeBox = False
        Me.Name = "frmExportFilter"
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Export"
        Me.TopMost = True
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents lblEndDate As System.Windows.Forms.Label
    Friend WithEvents lblStartDate As System.Windows.Forms.Label
    Friend WithEvents pkrEndDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents pkrStartDate As System.Windows.Forms.DateTimePicker
End Class
