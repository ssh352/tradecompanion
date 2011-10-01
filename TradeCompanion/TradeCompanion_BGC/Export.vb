Public Class frmExportFilter
    Private Shared _Instance As frmExportFilter = Nothing
    Private Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub
    Public Shared Function Instance() As frmExportFilter
        If _Instance Is Nothing OrElse _Instance.IsDisposed = True Then
            _Instance = New frmExportFilter
        End If
        _Instance.BringToFront()
        Return _Instance
    End Function

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dispose()
    End Sub

    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Form1.GetSingletonOrderform().fnExport(Me.Text, pkrStartDate.Value, pkrEndDate.Value)
        Me.Close()
    End Sub
End Class