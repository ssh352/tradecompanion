Imports System.io
Public Class MappingTable
    Inherits System.Windows.Forms.Form
    '---------------------------------------
    Dim ah As AlertsHome
    Dim ds As DataSet
    Shared savePass As Integer = 0
    Shared validateNull As Boolean = True
    '---for single window display-----------
    Private Shared _Instance As MappingTable = Nothing

#Region " Windows Form Designer generated code "

    Private Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        '------------------------------------
        ah = New AlertsHome
        ds = ah.getSymbolMap()
        '--------------------------------
        'Add any initialization after the InitializeComponent() call
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdSave As System.Windows.Forms.Button
    Friend WithEvents Done As System.Windows.Forms.Button

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents grdMap As Janus.Windows.GridEX.GridEX
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.grdMap = New Janus.Windows.GridEX.GridEX
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Done = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdSave = New System.Windows.Forms.Button
        CType(Me.grdMap, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grdMap
        '
        Me.grdMap.AllowDelete = Janus.Windows.GridEX.InheritableBoolean.[True]
        Me.grdMap.AlternatingColors = True
        Me.grdMap.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdMap.ColumnAutoResize = True
        Me.grdMap.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grdMap.Location = New System.Drawing.Point(0, 0)
        Me.grdMap.Name = "grdMap"
        Me.grdMap.NewRowFormatStyle.Appearance = Janus.Windows.GridEX.Appearance.Sunken
        Me.grdMap.NewRowFormatStyle.BackColor = System.Drawing.Color.LightYellow
        Me.grdMap.SaveSettings = False
        Me.grdMap.Size = New System.Drawing.Size(871, 367)
        Me.grdMap.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.Done)
        Me.Panel1.Controls.Add(Me.cmdCancel)
        Me.Panel1.Controls.Add(Me.cmdSave)
        Me.Panel1.Location = New System.Drawing.Point(0, 373)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(871, 30)
        Me.Panel1.TabIndex = 1
        '
        'Done
        '
        Me.Done.BackColor = System.Drawing.Color.LightSteelBlue
        Me.Done.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.Done.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.Done.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.Done.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.Done.Location = New System.Drawing.Point(360, 2)
        Me.Done.Name = "Done"
        Me.Done.Size = New System.Drawing.Size(115, 23)
        Me.Done.TabIndex = 5
        Me.Done.Text = "Save and Close"
        Me.Done.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.cmdCancel.BackColor = System.Drawing.Color.LightSteelBlue
        Me.cmdCancel.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.cmdCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.cmdCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.cmdCancel.Location = New System.Drawing.Point(515, 2)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(115, 23)
        Me.cmdCancel.TabIndex = 4
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdSave
        '
        Me.cmdSave.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.cmdSave.BackColor = System.Drawing.Color.LightSteelBlue
        Me.cmdSave.FlatAppearance.BorderColor = System.Drawing.Color.MediumBlue
        Me.cmdSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightBlue
        Me.cmdSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue
        Me.cmdSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.cmdSave.Location = New System.Drawing.Point(205, 2)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(115, 23)
        Me.cmdSave.TabIndex = 3
        Me.cmdSave.Text = "Save"
        Me.cmdSave.UseVisualStyleBackColor = False
        '
        'MappingTable
        '
        Me.AcceptButton = Me.cmdSave
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(871, 404)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.grdMap)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(100, 200)
        Me.MaximizeBox = False
        Me.Name = "MappingTable"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "MappingTable"
        CType(Me.grdMap, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub MappingTable_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        grdMap.DataSource = ds
        grdMap.SetDataBinding(ds, "SymbolMap")
        grdMap.RetrieveStructure()

        'grdMap.DropDowns.Add("SecurityType")
        'grdMap.DropDowns("SecurityType").SetDataBinding(ds, "SecurityType")
        'grdMap.DropDowns("SecurityType").RetrieveStructure()
        'grdMap.DropDowns("SecurityType").ValueMember = "SecurityType"
        'grdMap.DropDowns("SecurityType").DisplayMember = "Name"

        'grdMap.Tables(0).Columns("SecurityType").DropDown = grdMap.DropDowns("SecurityType")
        'grdMap.Tables(0).Columns("SecurityType").DropDown.Columns("SecurityType").Visible = False
        'grdMap.Tables(0).Columns("SecurityType").DropDown.Columns("SecurityType").LimitToList = True
        'grdMap.Tables(0).Columns("SecurityType").EditTarget = Janus.Windows.GridEX.EditTarget.Value
        'grdMap.Tables(0).Columns("SecurityType").CompareTarget = Janus.Windows.GridEX.EditTarget.Value
        'grdMap.Tables(0).Columns("SecurityType").DropDown.ValueMember = "SecurityType"
        'grdMap.Tables(0).Columns("SecurityType").DropDown.DisplayMember = "Name"
        'grdMap.Tables(0).Columns("SecurityType").EditType = Janus.Windows.GridEX.EditType.MultiColumnCombo
        'grdMap.Tables(0).Columns("SecurityType").Visible = False

        grdMap.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True
        grdMap.Tables(0).Columns("SymbolID").Visible = False
        grdMap.AllowDelete = Janus.Windows.GridEX.InheritableBoolean.True
        grdMap.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.True
        grdMap.Tables(0).Columns("TSSymbol").NullText = " "
        grdMap.Tables(0).Columns("TSSymbol").Caption = "Automatic Symbol"
        grdMap.Tables(0).Columns("TSExchange").NullText = " "

        'Make certain column invisible for curren ex as they dont make sense!
        grdMap.Tables(0).Columns("MonthYear").Visible = False
        grdMap.Tables(0).Columns("TSExchange").Visible = False
        grdMap.Tables(0).Columns("TradeExchange").Visible = False
        grdMap.Tables(0).Columns("SecurityType").Visible = False


        grdMap.DropDowns.Add("TradeType")
        grdMap.DropDowns("TradeType").SetDataBinding(ds, "TradeType")
        grdMap.DropDowns("TradeType").RetrieveStructure()
        grdMap.DropDowns("TradeType").ValueMember = "TradeType"
        grdMap.DropDowns("TradeType").DisplayMember = "Name"

        'grdMap.FormatStyles("TradeType").
        grdMap.Tables(0).Columns("TradeType").HeaderToolTip = "Applicable over trade size $ 10 million "
        grdMap.Tables(0).Columns("TradeType").DropDown = grdMap.DropDowns("TradeType")
        grdMap.Tables(0).Columns("TradeType").DropDown.Columns("TradeType").Visible = False
        grdMap.Tables(0).Columns("TradeType").DropDown.Columns("TradeType").LimitToList = True
        grdMap.Tables(0).Columns("TradeType").EditTarget = Janus.Windows.GridEX.EditTarget.Value
        grdMap.Tables(0).Columns("TradeType").CompareTarget = Janus.Windows.GridEX.EditTarget.Value
        grdMap.Tables(0).Columns("TradeType").DropDown.ValueMember = "TradeType"
        grdMap.Tables(0).Columns("TradeType").DropDown.DisplayMember = "Name"
        grdMap.Tables(0).Columns("TradeType").EditType = Janus.Windows.GridEX.EditType.MultiColumnCombo
        grdMap.Tables(0).Columns("TradeType").Visible = False
  
        grdMap.MoveLast()
        grdMap.Refresh()

        grdMap.MoveFirst()
        'grdMap.Refetch()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim ds As DataSet = grdMap.DataSource
        Dim ah As AlertsHome = New AlertsHome
        Dim i As Integer = 0
        validateNull = True
        'Dim validateNull As Boolean = True
        Dim str() As String = {"TSSymbol", "TradeSymbol", "TradeCurrency", "TradeSize"}
        Try
            Dim drr() As DataRow = ds.Tables(0).Select()
            For Each dr As DataRow In drr
                While (i <= 3)
                    If dr.IsNull(str(i)) Then
                        MsgBox(str(i) + " should not left blank.", MsgBoxStyle.Information, "Mapping Table")
                        validateNull = False
                        Exit For
                    End If
                    i = i + 1
                End While
                i = 0
            Next
            If validateNull Then
                ah.saveSymbolMap(ds)
                Util.WriteDebugLog("--- Changes are saved from Mapping window ")
            End If

        Catch ex As Exception
            MsgBox("Probelm in updating Mapping table: Please try again.")
            Util.WriteDebugLog("--- Mapping Table Update ERROR " + ex.Message)
            Util.WriteDebugLog("--- Stack Trace " + ex.StackTrace)
        End Try
    End Sub
    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Util.WriteDebugLog("--- No Changes accepted.Mapping Window Closed")
        Me.Close()
    End Sub
    Private Sub grdMap_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles grdMap.KeyDown
        If e.KeyCode = Keys.Delete Then
            Dim r As Janus.Windows.GridEX.GridEXRow = grdMap.GetRow
            grdMap.AllowDelete = Janus.Windows.GridEX.InheritableBoolean.True
            If MessageBox.Show("Do you want to delete mapping for symbol " + r.Cells("TSSymbol").Text + "?", "Mapping", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                'grdMap.Delete()
                'grdMap.MoveFirst()
                'grdMap.Refresh()
            Else
                grdMap.AllowDelete = Janus.Windows.GridEX.InheritableBoolean.False
            End If
            grdMap.Refresh()
        End If
    End Sub
    Private Sub grdMap_UpdatingCell(ByVal sender As Object, ByVal e As Janus.Windows.GridEX.UpdatingCellEventArgs) Handles grdMap.UpdatingCell
        Try
            If e.Column.Key.ToUpper = "TRADESIZE" Then
                If (Not IsDBNull(e.Value)) Then
                    If 6 = MsgBox("Do you want to change tradesize to all symbols", MsgBoxStyle.YesNo, "Mapping Table") Then
                        For Each r As DataRow In ds.Tables(0).Rows
                            If (r.RowState <> DataRowState.Deleted) Then
                                r.Item(9) = e.Value
                            End If
                        Next
                    End If
                End If
            End If

            If e.Column.Key = "TSSymbol" OrElse e.Column.Key = "TSExch" Then
                Dim s As String = e.Value
                Dim st As String = Nothing
                Dim dvCheck As DataView = ds.Tables(0).DefaultView
                Dim i As Integer = grdMap.CurrentRow.RowIndex
                If (i <> -1) Then
                    st = ds.Tables(0).Rows(i).Item("TSSymbol")
                End If
                If (st <> s.Trim) Then
                    dvCheck.RowFilter() = "TSSymbol = '" + s.Trim + "'"
                    If dvCheck.Count > 0 Then
                        MsgBox("Mapping for the symbol " + s.Trim + " already exists", MsgBoxStyle.OkOnly, "Mapping Table")
                        e.Cancel = True
                    End If
                End If
            End If
        Catch ex As Exception
            Util.WriteDebugLog("Update Cell " + ex.Message + ex.StackTrace)
        End Try
    End Sub

    Private Sub grdMap_AddingRecord(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles grdMap.AddingRecord
        'Dim r As Janus.Windows.GridEX.GridEXRow = grdMap.GetRow

        'Dim s As String = r.Cells("TradeCurrency").Text
        'If s.Trim = "" Then
        '    e.Cancel = True
        '    MessageBox.Show("Trade currency cannot be empty!")
        '    r.EndEdit()
        'End If

        'e.Cancel = True
        'Dim s As String
        'Dim bcheck As Boolean = False
        'For Each cell As Janus.Windows.GridEX.GridEXCell In r.Cells
        '    s = cell.Text
        '    If s.Trim <> "" Then
        '        bcheck = True
        '        Exit For
        '    End If
        'Next

        'If bcheck Then
        '    s = r.Cells("TradeSize").Text
        '    If s.Trim = "" Then r.Cells("TradeSize").Value = 1

        '    s = r.Cells("MonthYear").Text
        '    If s.Trim = "" Then e.Cancel = True
        '    '  s = r.Cells("TSSymbol").Text
        '    '  If s.Trim = "" Then e.Cancel = True

        '    ' s = r.Cells("TSExchange").Text
        '    ' If s.Trim = "" Then e.Cancel = True
        'End If
    End Sub

    Private Sub Ok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Done.Click
        cmdSave_Click(sender, e)
        If validateNull Then
            Me.Close()
        End If
    End Sub

    Public Shared Function Instance() As MappingTable
        If _Instance Is Nothing OrElse _Instance.IsDisposed = True Then
            _Instance = New MappingTable
        End If
        _Instance.BringToFront()
        Return _Instance
    End Function

End Class
