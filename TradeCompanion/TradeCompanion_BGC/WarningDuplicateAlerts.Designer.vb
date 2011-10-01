<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WarningDuplicateAlerts
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
        Me.lblMessage = New System.Windows.Forms.Label
        Me.btnNotAgree = New System.Windows.Forms.Button
        Me.btnAgree = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lblMessage
        '
        Me.lblMessage.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMessage.Location = New System.Drawing.Point(3, 0)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(367, 53)
        Me.lblMessage.TabIndex = 0
        Me.lblMessage.Text = "This may cause duplicate alerts routed from tradestation and you accept all the r" & _
            "isk associated with it."
        Me.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnNotAgree
        '
        Me.btnNotAgree.Location = New System.Drawing.Point(90, 56)
        Me.btnNotAgree.Name = "btnNotAgree"
        Me.btnNotAgree.Size = New System.Drawing.Size(89, 24)
        Me.btnNotAgree.TabIndex = 1
        Me.btnNotAgree.Text = "I Do Not Agree"
        Me.btnNotAgree.UseVisualStyleBackColor = True
        '
        'btnAgree
        '
        Me.btnAgree.Location = New System.Drawing.Point(185, 56)
        Me.btnAgree.Name = "btnAgree"
        Me.btnAgree.Size = New System.Drawing.Size(89, 24)
        Me.btnAgree.TabIndex = 2
        Me.btnAgree.Text = "I Agree"
        Me.btnAgree.UseVisualStyleBackColor = True
        '
        'WarningDuplicateAlerts
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(365, 85)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnAgree)
        Me.Controls.Add(Me.btnNotAgree)
        Me.Controls.Add(Me.lblMessage)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "WarningDuplicateAlerts"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Warning"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblMessage As System.Windows.Forms.Label
    Friend WithEvents btnNotAgree As System.Windows.Forms.Button
    Friend WithEvents btnAgree As System.Windows.Forms.Button
End Class
