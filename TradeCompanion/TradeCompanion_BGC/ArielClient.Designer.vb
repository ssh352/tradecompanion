<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ArielClient
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
        Me.rbnBgcClient = New System.Windows.Forms.RadioButton
        Me.rbnODLClient = New System.Windows.Forms.RadioButton
        Me.okBtn = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'rbnBgcClient
        '
        Me.rbnBgcClient.AutoSize = True
        Me.rbnBgcClient.Location = New System.Drawing.Point(35, 22)
        Me.rbnBgcClient.Name = "rbnBgcClient"
        Me.rbnBgcClient.Size = New System.Drawing.Size(98, 17)
        Me.rbnBgcClient.TabIndex = 0
        Me.rbnBgcClient.TabStop = True
        Me.rbnBgcClient.Text = "BGC Ariel client"
        Me.rbnBgcClient.UseVisualStyleBackColor = True
        '
        'rbnODLClient
        '
        Me.rbnODLClient.AutoSize = True
        Me.rbnODLClient.Location = New System.Drawing.Point(35, 60)
        Me.rbnODLClient.Name = "rbnODLClient"
        Me.rbnODLClient.Size = New System.Drawing.Size(98, 17)
        Me.rbnODLClient.TabIndex = 0
        Me.rbnODLClient.TabStop = True
        Me.rbnODLClient.Text = "ODL Ariel client"
        Me.rbnODLClient.UseVisualStyleBackColor = True
        '
        'okBtn
        '
        Me.okBtn.Location = New System.Drawing.Point(180, 35)
        Me.okBtn.Name = "okBtn"
        Me.okBtn.Size = New System.Drawing.Size(74, 27)
        Me.okBtn.TabIndex = 1
        Me.okBtn.Text = "Ok"
        Me.okBtn.UseVisualStyleBackColor = True
        '
        'ArielClient
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(266, 98)
        Me.ControlBox = False
        Me.Controls.Add(Me.okBtn)
        Me.Controls.Add(Me.rbnODLClient)
        Me.Controls.Add(Me.rbnBgcClient)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "ArielClient"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ArielClient Selection"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents rbnBgcClient As System.Windows.Forms.RadioButton
    Friend WithEvents rbnODLClient As System.Windows.Forms.RadioButton
    Friend WithEvents okBtn As System.Windows.Forms.Button
End Class
