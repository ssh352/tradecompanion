Imports System.io
Public Class IDMappingTable
    Dim ds As DataSet
    Dim ah As AlertsHome
    Private Shared _Instance As IDMappingTable = Nothing

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ah = New AlertsHome
        ds = ah.GetIdMap()
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub IDMappingTable_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim tbl As DataTable = New DataTable
        tbl.Columns.Add("PlatformID", Integer.MaxValue.GetType)
        tbl.Columns.Add("PlatformName", " ".GetType)
        Dim r As DataRow = tbl.NewRow
        r.Item("PlatformID") = 1
        r.Item("PlatformName") = "TradeStation"
        tbl.Rows.Add(r)
        Dim cmbCount As Integer = 1

        LabelInstalledPath.Visible = False
        TextBoxMT4Path.Visible = False
        ButtonMT4InstalledPath.Visible = False

        If (SettingsHome.getInstance().MetaTraderMonitorPath <> "") Then
            cmbCount = cmbCount + 1
            r = tbl.NewRow
            r.Item("PlatformID") = cmbCount
            r.Item("PlatformName") = "MetaTrader"
            tbl.Rows.Add(r)
        End If

        If (SettingsHome.getInstance().NSTintialpath <> "") Then
            cmbCount = cmbCount + 1
            r = tbl.NewRow
            r.Item("PlatformID") = cmbCount
            r.Item("PlatformName") = "NeuroShell"
            tbl.Rows.Add(r)
        End If


        tbl.TableName = "Platform"

        cmbPlatForm.DisplayMember = "PlatformName"
        cmbPlatForm.ValueMember = "PlatformID"
        cmbPlatForm.DataSource = tbl

        cmbPlatForm.Text = SettingsHome.getInstance().Platform
        If (cmbPlatForm.Text = Nothing) Then
            cmbPlatForm.Text = "TradeStation"
        End If

        If (cmbPlatForm.Text = "MetaTrader") Then
            TextBoxMT4Path.Text = SettingsHome.getInstance().MetaTraderMonitorPath

            LabelInstalledPath.Visible = True
            TextBoxMT4Path.Visible = True
            ButtonMT4InstalledPath.Visible = True
        End If
    
        grdIDMap.DataSource = ds
        grdIDMap.SetDataBinding(ds, "IDMap")
        grdIDMap.RetrieveStructure()

        grdIDMap.Tables(0).Columns("ID").Visible = False
        grdIDMap.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True
        grdIDMap.AllowDelete = Janus.Windows.GridEX.InheritableBoolean.True
        grdIDMap.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.True
        grdIDMap.MoveLast()
        grdIDMap.Refresh()

        grdIDMap.MoveFirst()
        'If Form1.GetSingletonOrderform().crashtest Then     
        '    Me.SClose.PerformClick()    
        'End If    
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub

    Private Sub SClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SClose.Click
        Save_Click(sender, e)
        Me.Close()
    End Sub

    Private Sub Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Save.Click
        SettingsHome.getInstance().Platform = cmbPlatForm.Text

        'Form1.GetSingletonOrderform.automate.UpdateSettings("platform")

        If (cmbPlatForm.Text = "MetaTrader") Then
            SettingsHome.getInstance().MetaTraderMonitorPath = TextBoxMT4Path.Text.Trim()
        End If
        AlertWatcher.getInstance().InitializeMonitorPath(SettingsHome.getInstance().Platform)
        Dim ds As DataSet = grdIDMap.DataSource
        Dim ah As AlertsHome = New AlertsHome
        Try
            ah.saveIdMap(ds)
        Catch ex As Exception
            'MsgBox(ex.Message)
            Util.WriteDebugLog(" ERROR SaveID Mapping" + ex.Message)
            Util.WriteDebugLog(" Stack Trace " + ex.StackTrace)
        End Try
        'Remove old alert when user select plateform
        Form1.GetSingletonOrderform.RemoveOldAlert()
    End Sub

    Public Shared Function Instance() As IDMappingTable
        If _Instance Is Nothing OrElse _Instance.IsDisposed = True Then
            _Instance = New IDMappingTable
        End If
        _Instance.BringToFront()
        Return _Instance
    End Function

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelInstalledPath.Click

    End Sub

    Private Sub cmbPlatForm_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPlatForm.SelectedIndexChanged

        If (cmbPlatForm.Text = "TradeStation" Or cmbPlatForm.Text = "NeuroShell") Then
            LabelInstalledPath.Visible = False
            TextBoxMT4Path.Visible = False
            ButtonMT4InstalledPath.Visible = False
        ElseIf (cmbPlatForm.Text = "MetaTrader") Then
            LabelInstalledPath.Visible = True
            TextBoxMT4Path.Visible = True
            ButtonMT4InstalledPath.Visible = True
            TextBoxMT4Path.Text = SettingsHome.getInstance().MetaTraderMonitorPath
        End If
        'If (cmbPlatForm.SelectedValue = "1" Or cmbPlatForm.SelectedValue = "3") Then
        '    LabelInstalledPath.Visible = False
        '    TextBoxMT4Path.Visible = False
        '    ButtonMT4InstalledPath.Visible = False
        'ElseIf (cmbPlatForm.SelectedValue = "2") Then
        '    LabelInstalledPath.Visible = True
        '    TextBoxMT4Path.Visible = True
        '    ButtonMT4InstalledPath.Visible = True
        '    TextBoxMT4Path.Text = SettingsHome.getInstance().MetaTraderMonitorPath
        'End If
    End Sub

    Private Sub ButtonMT4InstalledPath_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonMT4InstalledPath.Click
        If FolderBrowserDialogMT4Path.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If (Directory.Exists(FolderBrowserDialogMT4Path.SelectedPath + "\experts\files\")) Then
                TextBoxMT4Path.Text = FolderBrowserDialogMT4Path.SelectedPath
            Else
                MessageBox.Show("Not a valid MT4 installation path", "AutoShark")
            End If
        End If
    End Sub

    Private Sub grdIDMap_UpdatingCell(ByVal sender As Object, ByVal e As Janus.Windows.GridEX.UpdatingCellEventArgs) Handles grdIDMap.UpdatingCell
        If e.Column.Key = "TradeStationID" Then
            Dim i As Integer = grdIDMap.CurrentRow.RowIndex
            Dim st As String = Nothing
            If (i <> -1) Then
                st = ds.Tables(0).Rows(i).Item("TradeStationID")
            End If
            If (st <> e.Value().ToString().Trim()) Then
                Dim dvIdcheck As DataView = ds.Tables(0).DefaultView
                dvIdcheck.RowFilter = "TradeStationID = '" + e.Value().ToString().Trim() + "'"
                If dvIdcheck.Count > 0 Then
                    MsgBox("Mapping for this id " + e.Value() + " allready exists", MsgBoxStyle.Information, "IDMapping Table")
                    e.Cancel = True
                End If
            End If
        End If
    End Sub

End Class
