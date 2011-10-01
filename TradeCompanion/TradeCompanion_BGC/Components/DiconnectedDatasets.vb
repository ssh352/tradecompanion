'Imports System.Data
'Imports System.Text.RegularExpressions
'Public Delegate Sub UpdateMDHistory_Delegate(ByVal mdata As TradingInterface.FillMarketData)
'Public Class DiconnectedDatasets
'    Dim _dsMarkeData As DataSet = Nothing
'    Dim d As New UpdateMDHistory_Delegate(AddressOf UpdateMDHistory)
'    Private mdHistory As New Hashtable()
'    Private timeFrame As String = ""
'    Private Shared _disconnectedDataSets As DiconnectedDatasets = Nothing
'    Dim ah As New AlertsHome()
'    Private Sub New()
'    End Sub

'    Public Property DSMarkeData() As DataSet
'        Get
'            If (_dsMarkeData Is Nothing) Then
'                SetMDHash()
'                _dsMarkeData = ah.LoadMarketData()
'            End If
'            DSMarkeData = _dsMarkeData
'        End Get
'        Set(ByVal value As DataSet)
'            _dsMarkeData = value
'        End Set
'    End Property
'    Public Property HTmdHistory() As Hashtable
'        Get
'            HTmdHistory = mdHistory
'        End Get
'        Set(ByVal value As Hashtable)
'            mdHistory = value
'        End Set
'    End Property
'    Public Shared Function GetDiconnectedDatasetsSingleton() As DiconnectedDatasets
'        If (_disconnectedDataSets Is Nothing) Then
'            _disconnectedDataSets = New DiconnectedDatasets()
'        End If
'        Return _disconnectedDataSets
'    End Function
'    Public Sub UpdateMarketData(ByVal mdata As TradingInterface.FillMarketData)
'        Try
'            Dim r As DataRow() = _dsMarkeData.Tables(0).Select("Symbol = '" + mdata.Symbol + "'")


'            If r.Length > 0 Then
'                r(0)("BidPrice") = mdata.BidPrice
'                r(0)("OfferPrice") = mdata.OfferPrice
'                r(0)("TimeStamps") = mdata.TimeStamp

'                ' Align in-memory data with the data source ones
'                _dsMarkeData.AcceptChanges()

'            Else
'                Dim drMarkeData As DataRow = _dsMarkeData.Tables(0).NewRow()
'                drMarkeData("Symbol") = mdata.Symbol
'                drMarkeData("BidPrice") = mdata.BidPrice
'                drMarkeData("OfferPrice") = mdata.OfferPrice
'                drMarkeData("TimeStamps") = mdata.TimeStamp
'                _dsMarkeData.Tables(0).Rows.Add(drMarkeData)
'                _dsMarkeData.AcceptChanges()
'            End If
'            d.BeginInvoke(mdata, Nothing, Nothing)

'        Catch ex As Exception
'            Throw ex
'        End Try
'    End Sub

'    Private Sub UpdateMDHistory(ByVal mdata As TradingInterface.FillMarketData)
'        Dim lockThis As System.Object = New System.Object
'        SyncLock lockThis
'            Try
'                Dim hh As Integer = DateTime.Now.Hour
'                Dim currentTimeFrame As String = hh.ToString() '+ " - " + (hh + 1).ToString() 'IIf(hh = 12, "1", (hh + 1).ToString())
'                Dim diff As String
'                Dim diffInt As Integer
'                diff = CDec(mdata.OfferPrice) - CDec(mdata.BidPrice)

'                If (diff.IndexOf("."c) > 0) Then
'                    diff = diff.Split(".")(1)
'                Else
'                    'Util.WriteDebugLog("Differnce  is Zero")
'                    Return
'                End If
'                Try
'                    'diffInt = CInt(diff)
'                    diffInt = CInt(Regex.Replace(diff, "^0*", ""))
'                Catch ex As Exception
'                    diffInt = 0
'                End Try

'                If ((Not (mdHistory.Contains(mdata.Symbol))) Or timeFrame = currentTimeFrame) Then
'                    Dim prevDiff As Integer = 0
'                    If (mdHistory.Contains(mdata.Symbol)) Then prevDiff = CType(mdHistory.Item(mdata.Symbol), MDHistory).Pips
'                    If ((Not (mdHistory.Contains(mdata.Symbol))) Or diffInt > prevDiff) Then
'                        timeFrame = currentTimeFrame
'                        Dim mdh As New MDHistory
'                        mdh.BidPrice = CDec(mdata.BidPrice)
'                        mdh.OfferPrice = CDec(mdata.OfferPrice)
'                        mdh.Pips = diffInt
'                        mdh.Symbol = mdata.Symbol
'                        mdh.TimeStamp = DateTime.Now
'                        mdHistory.Remove(mdata.Symbol)
'                        mdHistory.Add(mdata.Symbol, mdh)
'                    End If
'                End If
'                If (timeFrame <> currentTimeFrame) Then
'                    timeFrame = currentTimeFrame
'                    Dim ah As New AlertsHome
'                    ah.DumpMDHistory(mdHistory)
'                    mdHistory.Clear()
'                End If

'            Catch ex As Exception
'                Util.WriteDebugLog(ex.Message + ex.Source)
'                Util.WriteDebugLog(ex.StackTrace)
'                Throw ex
'            End Try
'        End SyncLock
'    End Sub

'    Private Sub SetMDHash()
'        Try
'            Dim currentTimeFrame As String = DateTime.Now.Hour.ToString() '+ " - " + (DateTime.Now.Hour + 1).ToString() 'IIf(hh = 12, "1", (hh + 1).ToString())
'            Dim todaydate As String = DateTime.Now.Today.ToOADate.ToString()
'            Dim ah As New AlertsHome()
'            Dim sql As String = "DateMDData = " + todaydate + " and TimeFrame = " + currentTimeFrame
'            Dim ds As DataSet = ah.LoadMarketDataHistory(sql)
'            mdHistory.Clear()
'            For Each r As DataRow In ds.Tables(0).Rows
'                Dim mdh As New MDHistory
'                mdh.BidPrice = r.Item("BidPrice")
'                mdh.OfferPrice = r.Item("OfferPrice")
'                mdh.Pips = r.Item("MaxDifference")
'                mdh.Symbol = r.Item("Symbol")
'                mdh.TimeStamp = Date.FromOADate(r.Item("TimeStamps"))
'                If (mdHistory.ContainsKey(mdh.Symbol)) Then mdHistory.Remove(mdh.Symbol)
'                mdHistory.Add(mdh.Symbol, mdh)
'            Next r
'        Catch ex As Exception
'        End Try
'    End Sub
'End Class
