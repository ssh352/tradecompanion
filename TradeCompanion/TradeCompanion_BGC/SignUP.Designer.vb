<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SignUP
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SignUP))
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtEmailid = New System.Windows.Forms.TextBox
        Me.txtFirstname = New System.Windows.Forms.TextBox
        Me.txtLastName = New System.Windows.Forms.TextBox
        Me.txtCity = New System.Windows.Forms.TextBox
        Me.txtCountry = New System.Windows.Forms.TextBox
        Me.txtPhoneNo = New System.Windows.Forms.TextBox
        Me.txtAddress = New System.Windows.Forms.RichTextBox
        Me.cmdSignUP = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.BtnClose = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(32, 80)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(47, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "EmailID*"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(32, 103)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "First name*"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(32, 126)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(58, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Last Name"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(32, 149)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(49, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Address*"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(32, 217)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(28, 13)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "City*"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(32, 243)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(47, 13)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Country*"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(32, 269)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(59, 13)
        Me.Label8.TabIndex = 7
        Me.Label8.Text = "Phone No*"
        '
        'txtEmailid
        '
        Me.txtEmailid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtEmailid.Location = New System.Drawing.Point(148, 73)
        Me.txtEmailid.Name = "txtEmailid"
        Me.txtEmailid.Size = New System.Drawing.Size(171, 20)
        Me.txtEmailid.TabIndex = 100
        '
        'txtFirstname
        '
        Me.txtFirstname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFirstname.Location = New System.Drawing.Point(148, 99)
        Me.txtFirstname.Name = "txtFirstname"
        Me.txtFirstname.Size = New System.Drawing.Size(171, 20)
        Me.txtFirstname.TabIndex = 101
        '
        'txtLastName
        '
        Me.txtLastName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtLastName.Location = New System.Drawing.Point(148, 125)
        Me.txtLastName.Name = "txtLastName"
        Me.txtLastName.Size = New System.Drawing.Size(171, 20)
        Me.txtLastName.TabIndex = 102
        '
        'txtCity
        '
        Me.txtCity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCity.Location = New System.Drawing.Point(148, 210)
        Me.txtCity.Name = "txtCity"
        Me.txtCity.Size = New System.Drawing.Size(171, 20)
        Me.txtCity.TabIndex = 104
        '
        'txtCountry
        '
        Me.txtCountry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCountry.Location = New System.Drawing.Point(148, 236)
        Me.txtCountry.Name = "txtCountry"
        Me.txtCountry.Size = New System.Drawing.Size(171, 20)
        Me.txtCountry.TabIndex = 105
        '
        'txtPhoneNo
        '
        Me.txtPhoneNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPhoneNo.Location = New System.Drawing.Point(148, 262)
        Me.txtPhoneNo.Name = "txtPhoneNo"
        Me.txtPhoneNo.Size = New System.Drawing.Size(171, 20)
        Me.txtPhoneNo.TabIndex = 106
        '
        'txtAddress
        '
        Me.txtAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAddress.Location = New System.Drawing.Point(148, 151)
        Me.txtAddress.Name = "txtAddress"
        Me.txtAddress.Size = New System.Drawing.Size(171, 53)
        Me.txtAddress.TabIndex = 103
        Me.txtAddress.Text = ""
        '
        'cmdSignUP
        '
        Me.cmdSignUP.BackColor = System.Drawing.Color.LightSteelBlue
        Me.cmdSignUP.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.cmdSignUP.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.cmdSignUP.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.cmdSignUP.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmdSignUP.Location = New System.Drawing.Point(148, 315)
        Me.cmdSignUP.Name = "cmdSignUP"
        Me.cmdSignUP.Size = New System.Drawing.Size(83, 26)
        Me.cmdSignUP.TabIndex = 107
        Me.cmdSignUP.Text = "Sign Up"
        Me.cmdSignUP.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label4.Location = New System.Drawing.Point(225, 54)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(94, 13)
        Me.Label4.TabIndex = 108
        Me.Label4.Text = "* Mandatory Fields"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(6, 5)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(365, 16)
        Me.Label9.TabIndex = 109
        Me.Label9.Text = "Fill the details to signup.  Login details will be emailed to you."
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(6, 26)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(353, 16)
        Me.Label10.TabIndex = 110
        Me.Label10.Text = "If you already have login details, please close this window."
        '
        'BtnClose
        '
        Me.BtnClose.BackColor = System.Drawing.Color.LightSteelBlue
        Me.BtnClose.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.BtnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.BtnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.BtnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnClose.Location = New System.Drawing.Point(236, 315)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(83, 26)
        Me.BtnClose.TabIndex = 111
        Me.BtnClose.Text = "Cancel"
        Me.BtnClose.UseVisualStyleBackColor = False
        '
        'SignUP
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(376, 356)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cmdSignUP)
        Me.Controls.Add(Me.txtAddress)
        Me.Controls.Add(Me.txtPhoneNo)
        Me.Controls.Add(Me.txtCountry)
        Me.Controls.Add(Me.txtCity)
        Me.Controls.Add(Me.txtLastName)
        Me.Controls.Add(Me.txtFirstname)
        Me.Controls.Add(Me.txtEmailid)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SignUP"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Signup Form [Tradercompanion]"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtEmailid As System.Windows.Forms.TextBox
    Friend WithEvents txtFirstname As System.Windows.Forms.TextBox
    Friend WithEvents txtLastName As System.Windows.Forms.TextBox
    Friend WithEvents txtCity As System.Windows.Forms.TextBox
    Friend WithEvents txtCountry As System.Windows.Forms.TextBox
    Friend WithEvents txtPhoneNo As System.Windows.Forms.TextBox
    Friend WithEvents txtAddress As System.Windows.Forms.RichTextBox
    Friend WithEvents cmdSignUP As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents BtnClose As System.Windows.Forms.Button
End Class
