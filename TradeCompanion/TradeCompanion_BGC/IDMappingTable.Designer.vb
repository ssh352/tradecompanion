<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class IDMappingTable
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
        Me.grdIDMap = New Janus.Windows.GridEX.GridEX
        Me.Save = New System.Windows.Forms.Button
        Me.SClose = New System.Windows.Forms.Button
        Me.Cancel = New System.Windows.Forms.Button
        Me.cmbPlatForm = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.LabelInstalledPath = New System.Windows.Forms.Label
        Me.TextBoxMT4Path = New System.Windows.Forms.TextBox
        Me.ButtonMT4InstalledPath = New System.Windows.Forms.Button
        Me.FolderBrowserDialogMT4Path = New System.Windows.Forms.FolderBrowserDialog
        CType(Me.grdIDMap, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grdIDMap
        '
        Me.grdIDMap.ColumnAutoResize = True
        Me.grdIDMap.GroupByBoxVisible = False
        Me.grdIDMap.Location = New System.Drawing.Point(-1, 66)
        Me.grdIDMap.Name = "grdIDMap"
        Me.grdIDMap.NewRowFormatStyle.Appearance = Janus.Windows.GridEX.Appearance.Sunken
        Me.grdIDMap.NewRowFormatStyle.BackColor = System.Drawing.SystemColors.Info
        Me.grdIDMap.Size = New System.Drawing.Size(400, 189)
        Me.grdIDMap.TabIndex = 0
        '
        'Save
        '
        Me.Save.BackColor = System.Drawing.Color.LightSteelBlue
        Me.Save.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.Save.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.Save.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Save.Location = New System.Drawing.Point(50, 261)
        Me.Save.Name = "Save"
        Me.Save.Size = New System.Drawing.Size(92, 25)
        Me.Save.TabIndex = 1
        Me.Save.Text = "Save"
        Me.Save.UseVisualStyleBackColor = False
        '
        'SClose
        '
        Me.SClose.AutoSize = True
        Me.SClose.BackColor = System.Drawing.Color.LightSteelBlue
        Me.SClose.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.SClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.SClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.SClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.SClose.Location = New System.Drawing.Point(148, 261)
        Me.SClose.Name = "SClose"
        Me.SClose.Size = New System.Drawing.Size(100, 25)
        Me.SClose.TabIndex = 2
        Me.SClose.Text = "Save and Close"
        Me.SClose.UseVisualStyleBackColor = False
        '
        'Cancel
        '
        Me.Cancel.BackColor = System.Drawing.Color.LightSteelBlue
        Me.Cancel.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.Cancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.Cancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Cancel.Location = New System.Drawing.Point(253, 261)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.Size = New System.Drawing.Size(92, 25)
        Me.Cancel.TabIndex = 3
        Me.Cancel.Text = "Cancel"
        Me.Cancel.UseVisualStyleBackColor = False
        '
        'cmbPlatForm
        '
        Me.cmbPlatForm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPlatForm.FormattingEnabled = True
        Me.cmbPlatForm.Location = New System.Drawing.Point(111, 5)
        Me.cmbPlatForm.Name = "cmbPlatForm"
        Me.cmbPlatForm.Size = New System.Drawing.Size(287, 21)
        Me.cmbPlatForm.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(99, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Automated Platform"
        '
        'LabelInstalledPath
        '
        Me.LabelInstalledPath.AutoSize = True
        Me.LabelInstalledPath.Location = New System.Drawing.Point(5, 40)
        Me.LabelInstalledPath.Name = "LabelInstalledPath"
        Me.LabelInstalledPath.Size = New System.Drawing.Size(96, 13)
        Me.LabelInstalledPath.TabIndex = 6
        Me.LabelInstalledPath.Text = "MT4 Installed Path"
        '
        'TextBoxMT4Path
        '
        Me.TextBoxMT4Path.Location = New System.Drawing.Point(111, 37)
        Me.TextBoxMT4Path.Name = "TextBoxMT4Path"
        Me.TextBoxMT4Path.ReadOnly = True
        Me.TextBoxMT4Path.Size = New System.Drawing.Size(217, 20)
        Me.TextBoxMT4Path.TabIndex = 7
        '
        'ButtonMT4InstalledPath
        '
        Me.ButtonMT4InstalledPath.Location = New System.Drawing.Point(334, 35)
        Me.ButtonMT4InstalledPath.Name = "ButtonMT4InstalledPath"
        Me.ButtonMT4InstalledPath.Size = New System.Drawing.Size(64, 25)
        Me.ButtonMT4InstalledPath.TabIndex = 8
        Me.ButtonMT4InstalledPath.Text = "Change"
        Me.ButtonMT4InstalledPath.UseVisualStyleBackColor = True
        '
        'IDMappingTable
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(401, 299)
        Me.Controls.Add(Me.ButtonMT4InstalledPath)
        Me.Controls.Add(Me.TextBoxMT4Path)
        Me.Controls.Add(Me.LabelInstalledPath)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbPlatForm)
        Me.Controls.Add(Me.Cancel)
        Me.Controls.Add(Me.SClose)
        Me.Controls.Add(Me.Save)
        Me.Controls.Add(Me.grdIDMap)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "IDMappingTable"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "IDMappingTable"
        CType(Me.grdIDMap, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents grdIDMap As Janus.Windows.GridEX.GridEX
    Friend WithEvents Save As System.Windows.Forms.Button
    Friend WithEvents SClose As System.Windows.Forms.Button
    Friend WithEvents Cancel As System.Windows.Forms.Button
    Friend WithEvents cmbPlatForm As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents LabelInstalledPath As System.Windows.Forms.Label
    Friend WithEvents TextBoxMT4Path As System.Windows.Forms.TextBox
    Friend WithEvents ButtonMT4InstalledPath As System.Windows.Forms.Button
    Friend WithEvents FolderBrowserDialogMT4Path As System.Windows.Forms.FolderBrowserDialog
End Class
