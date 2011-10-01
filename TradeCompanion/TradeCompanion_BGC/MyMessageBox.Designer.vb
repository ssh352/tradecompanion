<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MyMessageBox
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
        Me.components = New System.ComponentModel.Container
        Me.btnOk = New System.Windows.Forms.Button
        Me.lblMessage = New System.Windows.Forms.Label
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnOk
        '
        Me.btnOk.BackColor = System.Drawing.Color.LightSteelBlue
        Me.btnOk.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.btnOk.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.btnOk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOk.Location = New System.Drawing.Point(177, 65)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(94, 25)
        Me.btnOk.TabIndex = 0
        Me.btnOk.Text = "OK"
        Me.btnOk.UseVisualStyleBackColor = False
        '
        'lblMessage
        '
        Me.lblMessage.Location = New System.Drawing.Point(9, 7)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(349, 43)
        Me.lblMessage.TabIndex = 1
        '
        'Timer1
        '
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(364, 7)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(68, 55)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 2
        Me.PictureBox1.TabStop = False
        '
        'MyMessageBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(459, 92)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.btnOk)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MyMessageBox"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MessageBox"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents lblMessage As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
End Class
