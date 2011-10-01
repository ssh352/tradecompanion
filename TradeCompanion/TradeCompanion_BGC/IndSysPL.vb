Public Class IndSysPL
    Dim ah As New AlertsHome
    'Dim dsPLTrade As DataSet
    Public dsIndsys As DataSet
    Public Shared Event OpenPosIndSys(ByVal dr As DataRow)

    Public Sub New()
        'dsPLTrade = PLCal.dsPLTrade

        dsIndsys = ah.LoadIndividualSystem()
        dsIndsys.Tables(0).Columns.Add("TsOpenPosition")
        dsIndsys.Tables(0).Columns("ID").AutoIncrement = True
    End Sub

    Public Sub CalIndSysOpenPos(ByVal trData As TradingInterface.Fill)

        Dim dr As DataRow = dsIndsys.Tables(0).NewRow()
        dr.Item("Symbol") = trData.Symbol
        dr.Item("SystemName") = trData.systemName
        dr.Item("SystemNumber") = trData.monthyear
        dr.Item("OpenPosition") = GetOpenPosition(trData.Symbol, trData.systemName, trData.monthyear, trData.accountId)
        dr.Item("OrderID") = trData.orderId
        dr.Item("EntryDateTime") = trData.timestamp
        dr.Item("EntryPrice") = trData.price
        dr.Item("OpenPositionPL") = GetOpenPositionPLBase(trData.Symbol, trData.systemName, trData.monthyear, trData.accountId)
        dr.Item("RealizedPL") = GetRealizedPipsBase(trData.Symbol, trData.systemName, trData.monthyear, trData.accountId)
        dr.Item("TotalPL") = dr.Item("OpenPositionPL") + dr.Item("RealizedPL")
        dr.Item("TsOpenPosition") = trData.tsOpenPosition
        dr.Item("SenderId") = trData.accountId
        UpdatePLCalcDS(dr)
        ah.UpdateIndSys(dr)
        'Console.WriteLine("Ha Ha Ha got the Value " + trData.tsOpenPosition.ToString())
        RaiseEvent OpenPosIndSys(dr)

    End Sub

    Public Sub UpdateIndSysForMarket(ByVal sym As String)
        SyncLock Me
            For Each drow As DataRow In dsIndsys.Tables(0).Select("Symbol = '" + sym + "'")
                Dim dr As DataRow = dsIndsys.Tables(0).NewRow()
                dr.Item("Symbol") = sym
                dr.Item("SystemName") = drow.Item("SystemName")
                dr.Item("SystemNumber") = drow.Item("SystemNumber")
                dr.Item("OpenPosition") = drow.Item("OpenPosition")
                dr.Item("SenderId") = drow.Item("SenderId")
                dr.Item("OpenPositionPL") = GetOpenPositionPLBase(sym, drow.Item("SystemName"), drow.Item("SystemNumber"), drow.Item("SenderId"))
                dr.Item("RealizedPL") = GetRealizedPipsBase(sym, drow.Item("SystemName"), drow.Item("SystemNumber"), drow.Item("SenderId"))
                dr.Item("TotalPL") = dr.Item("OpenPositionPL") + dr.Item("RealizedPL")
                RaiseEvent OpenPosIndSys(dr)
            Next
        End SyncLock
    End Sub

    Private Sub UpdatePLCalcDS(ByVal r As DataRow)
        Try
            Dim dr() As DataRow = dsIndsys.Tables(0).Select("Symbol= '" + r.Item("Symbol").ToString() + "' And SystemName = '" + r.Item("SystemName").ToString() + "' And SystemNumber = '" + r.Item("SystemNumber").ToString() + "'")
            If (dr.Length > 0) Then
                dr(0)("OpenPosition") = r.Item("OpenPosition")
                dr(0)("EntryDateTime") = r.Item("EntryDateTime")
                dr(0)("OrderID") = r.Item("OrderID")
                dr(0)("EntryPrice") = r.Item("EntryPrice")
                dr(0)("OpenPositionPL") = r.Item("OpenPositionPL")
                dr(0)("RealizedPL") = r.Item("RealizedPL")
                dr(0)("TotalPL") = r.Item("TotalPL")
            Else
                Dim drIndSysPl As DataRow = dsIndsys.Tables(0).NewRow()

                drIndSysPl("Symbol") = r.Item("Symbol")
                drIndSysPl("SystemName") = r.Item("SystemName")
                drIndSysPl("SystemNumber") = r.Item("SystemNumber")
                drIndSysPl("OpenPosition") = r.Item("OpenPosition")
                drIndSysPl("EntryDateTime") = r.Item("EntryDateTime")
                drIndSysPl("OrderID") = r.Item("OrderID")
                drIndSysPl("EntryPrice") = r.Item("EntryPrice")
                drIndSysPl("OpenPositionPL") = r.Item("OpenPositionPL")
                drIndSysPl("RealizedPL") = r.Item("RealizedPL")
                drIndSysPl("TotalPL") = r.Item("TotalPL")
                drIndSysPl("SenderId") = r.Item("SenderId")

                dsIndsys.Tables(0).Rows.Add(drIndSysPl)
            End If
        Catch ex As Exception
            Console.WriteLine(ex.Message + ex.StackTrace)
        End Try
    End Sub

    Private Function GetOpenPosition(ByVal symbol As String, ByVal sysName As String, ByVal sysID As String, ByVal senderId As String) As Decimal
        Try
            Dim filter As String = ""
            filter = "Symbol = '" + symbol + "' AND ExecOrderId = '" + sysName + "' AND SystemID = '" + sysID + "' AND Actions = 1 AND SenderId = '" + senderId + "'"
            Dim totalBuyAmount As Decimal = 0
            Dim drs() As DataRow
            drs = PLCal.dsPLTrade.Tables(0).Select(filter)
            If (drs.Length <> 0) Then
                totalBuyAmount = PLCal.dsPLTrade.Tables(0).Compute("Sum(Amount)", filter)
            End If
            Dim totalSellAmount As Decimal = 0
            filter = "Symbol = '" + symbol + "' AND ExecOrderId = '" + sysName + "' AND SystemID = '" + sysID + "' AND Actions = 2 AND SenderId = '" + senderId + "'"
            drs = PLCal.dsPLTrade.Tables(0).Select(filter)
            If (drs.Length <> 0) Then
                totalSellAmount = PLCal.dsPLTrade.Tables(0).Compute("Sum(Amount)", filter)
            End If
            'Console.WriteLine("In OPen position" + (totalBuyAmount - totalSellAmount).ToString())
            Return (totalBuyAmount - totalSellAmount)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function GetRealizedPips(ByVal symbol As String, ByVal sysName As String, ByVal sysID As String, ByVal senderId As String) As Decimal
        Dim filter As String = ""
        If (symbol <> "") Then
            filter = "Symbol = '" + symbol + "' AND ExecOrderId = '" + sysName + "' AND SystemID = '" + sysID + "' AND SenderId = '" + senderId + "'"
        End If
        Dim tRPips As Decimal = 0
        Try
            tRPips = CDec(PLCal.dsPLTrade.Tables(0).Compute("Sum(PipSys)", filter))
            tRPips = Decimal.Round(CDec(tRPips), 2)
        Catch ex As Exception
        End Try
        Return tRPips
    End Function

    Private Function GetRealizedPipsBase(ByVal symbol As String, ByVal sysName As String, ByVal sysID As String, ByVal senderId As String) As Decimal
        Dim firstcurrency, secondcurrency, basecurrency As String
        Dim tRPips As Decimal
        basecurrency = SettingsHome.getInstance().BaseCurrency
        firstcurrency = EServerDependents.GetFirstCurrency(symbol)
        secondcurrency = EServerDependents.GetSecondCurrency(symbol)
        Try
            tRPips = GetRealizedPips(symbol, sysName, sysID, senderId)
            If (firstcurrency = basecurrency) Then 'case 1 first currency is base currency
                tRPips = tRPips / CDec(ah.GetBidPriceBySymbol(symbol))
            ElseIf (secondcurrency = basecurrency) Then 'case 2 second currency is base currency
                tRPips = tRPips
            Else 'case 3 none of the currency is base currency
                Dim price As Double = 0
                price = CDec(ah.GetBidPriceBySymbol(EServerDependents.GetCombinedCurrency(basecurrency, secondcurrency)))
                If (price <> -1) Then
                    tRPips = tRPips / price
                Else
                    price = CDec(ah.GetBidPriceBySymbol(EServerDependents.GetCombinedCurrency(secondcurrency, basecurrency)))
                    If (price <> -1) Then
                        tRPips = tRPips * price
                    Else
                        tRPips = 0
                    End If
                End If
            End If
        Catch ex As Exception
            tRPips = 0
        End Try

        Return Decimal.Round(CDec(tRPips), 2)
    End Function

    Private Function GetOpenPositionPL(ByVal symbol As String, ByVal sysName As String, ByVal sysID As String, ByVal senderId As String) As Decimal
        Dim filter1 As String = ""
        Dim filter2 As String = ""
        If (symbol <> "") Then
            filter1 = "Actions = 1  AND  RemainingSys > 0 AND Symbol = '" + symbol + "' AND ExecOrderId = '" + sysName + "' AND SystemID = '" + sysID + "' AND SenderId = '" + senderId + "'"
            filter2 = "Actions = 2  AND  RemainingSys > 0 AND Symbol = '" + symbol + "' AND ExecOrderId = '" + sysName + "' AND SystemID = '" + sysID + "' AND SenderId = '" + senderId + "'"
        End If
        Dim drs() As DataRow = PLCal.dsPLTrade.Tables(0).Select(filter1, "DateID")
        Dim totalPips As Decimal = 0

        If (drs.Length > 0) Then
            For Each dr As DataRow In drs
                totalPips = totalPips + (CDec(dr("RemainingSys")) * (ah.GetBidPriceBySymbol(symbol) - CDec(dr("Price"))))
            Next
        Else
            drs = PLCal.dsPLTrade.Tables(0).Select(filter2, "DateID")
            For Each dr As DataRow In drs

                totalPips = totalPips + (CDec(dr("RemainingSys")) * (CDec(dr("Price")) - ah.GetAskPriceBySymbol(symbol)))
            Next
        End If

        Return Decimal.Round(totalPips, 2)
    End Function

    Private Function GetOpenPositionPLBase(ByVal symbol As String, ByVal sysName As String, ByVal sysID As String, ByVal senderId As String) As Decimal
        Dim firstcurrency, secondcurrency, basecurrency As String
        Dim pipsBaseCurrency As Decimal
        basecurrency = SettingsHome.getInstance().BaseCurrency
        firstcurrency = EServerDependents.GetFirstCurrency(symbol)
        secondcurrency = EServerDependents.GetSecondCurrency(symbol)

        pipsBaseCurrency = GetOpenPositionPL(symbol, sysName, sysID, senderId)
        If (firstcurrency = basecurrency) Then
            pipsBaseCurrency = pipsBaseCurrency / CDec(ah.GetBidPriceBySymbol(symbol))
        ElseIf (secondcurrency = basecurrency) Then
            pipsBaseCurrency = pipsBaseCurrency
        Else
            Dim price As Double = 0
            Try
                price = CDec(ah.GetBidPriceBySymbol(EServerDependents.GetCombinedCurrency(basecurrency, secondcurrency)))
                If (price <> -1) Then
                    pipsBaseCurrency = pipsBaseCurrency / price
                Else
                    price = CDec(ah.GetBidPriceBySymbol(EServerDependents.GetCombinedCurrency(secondcurrency, basecurrency)))
                    If (price <> -1) Then
                        pipsBaseCurrency = pipsBaseCurrency * price
                    Else
                        pipsBaseCurrency = 0
                    End If
                End If
            Catch ex As Exception
                pipsBaseCurrency = 0
            End Try
        End If
        Return Decimal.Round(pipsBaseCurrency, 2)
    End Function

End Class
