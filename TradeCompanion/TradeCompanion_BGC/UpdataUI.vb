
Public Class UpdateUI

    Public Sub UpDateGird(ByVal drow As DataRow, ByRef ds As DataSet)
        UpdateDSSpotPosition(drow, ds)
    End Sub

    Private Sub UpdateDSSpotPosition(ByVal r As DataRow, ByRef dsSpotPosition As DataSet)
        Try
            dsSpotPosition.Tables(0).Columns("RowID").AutoIncrement = True
            Dim filter As String = ""
            filter = "Symbol= '" + r.Item("Symbol").ToString() + "' And AccountId = '" + r.Item("AccountID").ToString() + "'"
            Dim dr() As DataRow = dsSpotPosition.Tables(0).Select(filter)
            If (dr.Length > 0) Then 'update the row
                UpdateRow(r, dr(0))
            Else ' insert the row
                Dim newDatarow As DataRow = dsSpotPosition.Tables(0).NewRow()
                newDatarow("Symbol") = r("Symbol")
                newDatarow("AccountID") = r("AccountID")
                UpdateRow(r, newDatarow)
                dsSpotPosition.Tables(0).Rows.Add(newDatarow)
            End If
        Catch ex As Exception
            Util.WriteDebugLog("UpDateUI Class -UpdateDSSpotPosition  : " + ex.Message + ex.StackTrace)
        End Try
    End Sub

    Private Sub UpdateRow(ByVal r As DataRow, ByVal dr As DataRow)
        Try

            If (Not (r.IsNull("NetCC1"))) Then
                dr("NetCC1") = r("NetCC1")
            End If

            dr("RealizedBaseCurrency") = r("RealizedBaseCurrency")
            dr("UnRealizedBaseCurrency") = r("UnRealizedBaseCurrency")
            dr("TotalP&L") = dr("UnRealizedBaseCurrency") + dr("RealizedBaseCurrency")

        Catch ex As Exception
            Util.WriteDebugLog("UpDateUI Class -UpdateRow  : " + ex.Message + ex.StackTrace)
        End Try
        
    End Sub

End Class
