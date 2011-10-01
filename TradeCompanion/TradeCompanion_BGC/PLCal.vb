Imports System.Threading
Public Delegate Sub UpdatePL_Delegate(ByVal dr As Datarow)
Public Class PLCal
    Dim se As SettingsHome = SettingsHome.getInstance()
    Public Shared Event PLCalculation(ByVal r As DataRow)
    Dim ah As AlertsHome = New AlertsHome
    Public Shared dsPLTrade As DataSet
    Public Shared dsPLCalc As DataSet
    Public Sub New()
        dsPLTrade = ah.GetPLTrade("", "")
        dsPLTrade.Tables(0).Columns("RowID").AutoIncrement = True

        dsPLCalc = ah.loadPLCal()
        dsPLCalc.Tables(0).Columns("RowID").AutoIncrement = True
    End Sub

    Public Sub CalculatePIPS(ByVal tradeData As TradingInterface.Fill)
        Try

            Thread.CurrentThread.CurrentCulture = se.Culture
            Dim dsMarketData As DataSet = Form1.GetSingletonOrderform().dsMarkeData.Copy()
            Dim accountID As String = tradeData.accountId
            Dim Key As String
            Dim BidPrice As Double
            Dim OfferPrice As Double

            Key = tradeData.Symbol
            BidPrice = ah.GetBidPriceBySymbol(dsMarketData, Key) 'note : changed
            OfferPrice = ah.GetOfferPriceBySymbol(dsMarketData, Key)

            Dim drSpotPosition As DataRow = dsPLCalc.Tables(0).NewRow()
            Dim realized As Decimal = GetRealizedPIPS(Key, dsPLTrade, accountID)
            drSpotPosition("Symbol") = Key
            drSpotPosition("Realized") = realized
            drSpotPosition("AccountID") = accountID
            drSpotPosition("UnRealized") = GetUnRealizedPIPS(Key, BidPrice, dsPLTrade, accountID, OfferPrice)
            'Try
            drSpotPosition("RealizedBaseCurrency") = GetRealizedPIPSBaseCurrency(Key, drSpotPosition("Realized"), dsMarketData) 'dsPLTrade, accountID, dsMarketData)
            'Catch ex As Exception
            '    drSpotPosition("RealizedBaseCurrency") = 0
            'End Try

            'Try
            drSpotPosition("UnRealizedBaseCurrency") = GetUnRealizedPIPSBaseCurrency(Key, drSpotPosition("UnRealized"), dsMarketData) 'BidPrice, dsPLTrade, accountID, dsMarketData, OfferPrice)
            'Catch ex As Exception
            '    drSpotPosition("UnRealizedBaseCurrency") = 0
            'End Try
            Dim netccy1 As Decimal = GetNetCC1(Key, dsPLTrade, accountID) 'GetNetCC1(Key, dsPLTrade, accountID)
            Dim netccy2 As Decimal = GetNetCC2(Key, dsPLTrade, accountID)
            drSpotPosition("NetCC1") = netccy1
            drSpotPosition("NetCC2") = netccy2
            drSpotPosition("AverageBuyRate") = GetAvgBuyRate(Key, dsPLTrade, accountID)
            drSpotPosition("AverageSellRate") = GetAvgSellRate(Key, dsPLTrade, accountID)
            If netccy1 <> 0 Then
                drSpotPosition("AllInRate") = Decimal.Round((-netccy2 / netccy1), 9) '" - " becouse netccy2 is calculated with respect to secondary currency it is calculated  negativelly
                drSpotPosition("OpenRate") = Decimal.Round((((-netccy2) + realized) / netccy1), 9)
            Else
                drSpotPosition("AllInRate") = 0
                drSpotPosition("OpenRate") = 0
            End If

            If netccy1 > 0 Then
                drSpotPosition("MktRate") = BidPrice
            ElseIf netccy1 < 0 Then
                drSpotPosition("MktRate") = OfferPrice
            Else
                drSpotPosition("MktRate") = 0
            End If

            UpdatePLCalcDS(drSpotPosition)
            UpdateAsyncDB(drSpotPosition)

            'If (Not (StrategyPerformanceReport._Instance Is Nothing)) Then
            RaiseEvent PLCalculation(drSpotPosition)
            'End If
        Catch ex As Exception 'added to prevent exceptions from blocking proper display
            Util.WriteDebugLog("CalculatePIPS -- " & ex.Message)
        End Try
    End Sub

    Private Sub UpdatePLCalcDS(ByVal r As DataRow)
        Dim dr() As DataRow = dsPLCalc.Tables(0).Select("Symbol= '" + r.Item("Symbol").ToString() + "' And AccountId = '" + r.Item("AccountID").ToString() + "'")
        If (dr.Length > 0) Then
            dr(0)("Realized") = r.Item("Realized")
            dr(0)("UnRealized") = r.Item("UnRealized")
            dr(0)("UnrealizedBaseCurrency") = r.Item("UnrealizedBaseCurrency")
            dr(0)("RealizedBaseCurrency") = r.Item("RealizedBaseCurrency")
            dr(0)("NetCC1") = r.Item("NetCC1")
            dr(0)("NetCC2") = r.Item("NetCC2")
            dr(0)("AverageBuyRate") = r.Item("AverageBuyRate")
            dr(0)("AverageSellRate") = r.Item("AverageSellRate")
            dr(0)("AllInRate") = r.Item("AllInRate")
            dr(0)("OpenRate") = r.Item("OpenRate")
            dr(0)("MktRate") = r.Item("MktRate")
        Else
            Dim drPLCalc As DataRow = dsPLCalc.Tables(0).NewRow()
            Dim secondcurrency, basecurrency, baseSymbol As String
            basecurrency = se.BaseCurrency
            If (r.Item("Symbol").ToString().Contains(basecurrency)) Then
                baseSymbol = r.Item("Symbol").ToString()
            Else
                secondcurrency = EServerDependents.GetSecondCurrency(r.Item("Symbol").ToString())
                baseSymbol = EServerDependents.GetCombinedCurrency(basecurrency, secondcurrency)
            End If
            drPLCalc("Symbol") = r.Item("Symbol")
            drPLCalc("Realized") = r.Item("Realized")
            drPLCalc("UnRealized") = r.Item("UnRealized")
            drPLCalc("AccountID") = r.Item("AccountID")
            drPLCalc("RealizedBaseCurrency") = r.Item("RealizedBaseCurrency")
            drPLCalc("UnrealizedBaseCurrency") = r.Item("UnrealizedBaseCurrency")
            drPLCalc("BaseSymbol") = baseSymbol
            drPLCalc("NetCC1") = r.Item("NetCC1")
            drPLCalc("NetCC2") = r.Item("NetCC2")
            drPLCalc("AverageBuyRate") = r.Item("AverageBuyRate")
            drPLCalc("AverageSellRate") = r.Item("AverageSellRate")
            drPLCalc("AllInRate") = r.Item("AllInRate")
            drPLCalc("OpenRate") = r.Item("OpenRate")
            drPLCalc("MktRate") = r.Item("MktRate")
            dsPLCalc.Tables(0).Rows.Add(drPLCalc)
        End If
    End Sub

    Dim del_updatedb As New UpdatePL_Delegate(AddressOf UpdateAsyncDB)
    Public Sub CalculatePIPSMarketData(ByVal marketData As TradingInterface.FillMarketData)
        SyncLock Me
            Thread.CurrentThread.CurrentCulture = se.Culture
            Dim dsServerID As DataSet = ah.GetServerIDs()
            Dim dsMarketData As DataSet = Form1.GetSingletonOrderform().dsMarkeData.Copy()

            For Each dr As DataRow In dsServerID.Tables(0).Rows
                Dim accountID As String = dr("SenderID")
                Dim Key As String
                Dim BidPrice As Double
                Dim OfferPrice As Double

                If ((dsPLCalc.Tables(0).Select("Symbol = '" + marketData.Symbol + "' AND AccountID = '" + accountID + "'").Length > 0)) Then
                    Key = marketData.Symbol
                    BidPrice = ah.GetBidPriceBySymbol(dsMarketData, Key) 'CDbl(marketData.BidPrice)
                    OfferPrice = ah.GetOfferPriceBySymbol(dsMarketData, Key) 'CDbl(marketData.OfferPrice)

                    Dim drSpotPosition As DataRow = dsPLCalc.Tables(0).NewRow() 'dsSpotPosition.Tables(0).NewRow()
                    drSpotPosition("Symbol") = Key
                    drSpotPosition("Realized") = GetRealizedPIPS(Key, dsPLTrade, accountID)
                    drSpotPosition("AccountID") = accountID
                    drSpotPosition("UnRealized") = GetUnRealizedPIPS(Key, BidPrice, dsPLTrade, accountID, OfferPrice)
                    'Try
                    drSpotPosition("RealizedBaseCurrency") = GetRealizedPIPSBaseCurrency(Key, drSpotPosition("Realized"), dsMarketData) 'dsPLTrade, accountID, dsMarketData)
                    'Catch ex As Exception
                    '    drSpotPosition("RealizedBaseCurrency") = 0
                    'End Try

                    'Try
                    drSpotPosition("UnRealizedBaseCurrency") = GetUnRealizedPIPSBaseCurrency(Key, drSpotPosition("UnRealized"), dsMarketData) 'BidPrice, dsPLTrade, accountID, dsMarketData, OfferPrice)
                    'Catch ex As Exception
                    '    drSpotPosition("UnRealizedBaseCurrency") = 0
                    'End Try

                    Dim netccy1 As Decimal = GetNetCC1(Key, dsPLTrade, accountID)
                    If netccy1 > 0 Then
                        drSpotPosition("MktRate") = BidPrice
                    ElseIf netccy1 < 0 Then
                        drSpotPosition("MktRate") = OfferPrice
                    Else
                        drSpotPosition("MktRate") = 0
                    End If

                    'If (Not (StrategyPerformanceReport._Instance Is Nothing)) Then
                    RaiseEvent PLCalculation(drSpotPosition)
                    'End If
                Else
                'This part of code is used to calculate the pips with respect to the BaseCurrency(in this case it is USD) 
                Dim reverseSymbol As String = ""
                reverseSymbol = EServerDependents.GetCombinedCurrency(EServerDependents.GetSecondCurrency(marketData.Symbol), EServerDependents.GetFirstCurrency(marketData.Symbol))
                If (dsPLCalc.Tables(0).Select("BaseSymbol = '" + marketData.Symbol + "' AND AccountID = '" + accountID + "'").Length > 0) Then
                    Dim drr() As DataRow = dsPLCalc.Tables(0).Select("BaseSymbol = '" + marketData.Symbol + "' AND AccountID = '" + accountID + "'")
                    For Each drow As DataRow In drr
                        If Not (drow.Item("Symbol") = drow.Item("BaseSymbol")) Then
                            CalculatePL(drow, dsPLTrade, dsMarketData, accountID)
                        End If
                    Next

                ElseIf (dsPLCalc.Tables(0).Select("BaseSymbol = '" + reverseSymbol + "' AND AccountID = '" + accountID + "'").Length > 0) Then
                    Dim drr() As DataRow = dsPLCalc.Tables(0).Select("BaseSymbol = '" + reverseSymbol + "' AND AccountID = '" + accountID + "'")
                    For Each drow As DataRow In drr
                        CalculatePL(drow, dsPLTrade, dsMarketData, accountID)
                    Next
                End If
                End If

                'Dim reverseSymbol As String = ""
                'reverseSymbol = EServerDependents.GetCombinedCurrency(EServerDependents.GetSecondCurrency(marketData.Symbol), EServerDependents.GetFirstCurrency(marketData.Symbol))
                'If (dsPLCalc.Tables(0).Select("BaseSymbol = '" + marketData.Symbol + "' AND AccountID = '" + accountID + "'").Length > 0) Then
                '    Dim drr() As DataRow = dsPLCalc.Tables(0).Select("BaseSymbol = '" + marketData.Symbol + "' AND AccountID = '" + accountID + "'")
                '    For Each drow As DataRow In drr
                '        If Not (drow.Item("Symbol") = drow.Item("BaseSymbol")) Then
                '            CalculatePL(drow, dsPLTrade, dsMarketData, accountID)
                '        End If
                '    Next

                'ElseIf (dsPLCalc.Tables(0).Select("BaseSymbol = '" + reverseSymbol + "' AND AccountID = '" + accountID + "'").Length > 0) Then
                '    Dim drr() As DataRow = dsPLCalc.Tables(0).Select("BaseSymbol = '" + reverseSymbol + "' AND AccountID = '" + accountID + "'")
                '    For Each drow As DataRow In drr
                '        CalculatePL(drow, dsPLTrade, dsMarketData, accountID)
                '    Next
                'End If
            Next
        End SyncLock
    End Sub

    Private Sub CalculatePL(ByVal drow As DataRow, ByVal dsPLTrade As DataSet, ByVal dsMarketData As DataSet, ByVal accountID As String)
        Dim Key As String
        Dim BidPrice As Double
        Dim OfferPrice As Double
        Key = drow.Item("Symbol")
        BidPrice = ah.GetBidPriceBySymbol(dsMarketData, Key)
        OfferPrice = ah.GetOfferPriceBySymbol(dsMarketData, Key)
        Dim drSpotPosition As DataRow = dsPLCalc.Tables(0).NewRow()
        drSpotPosition("Symbol") = Key
        drSpotPosition("AccountID") = accountID
        drSpotPosition("Realized") = GetRealizedPIPS(Key, dsPLTrade, accountID)  'drow.Item("Realized") '
        drSpotPosition("UnRealized") = GetUnRealizedPIPS(Key, BidPrice, dsPLTrade, accountID, OfferPrice)  'drow.Item("UnRealized") '
        Try
            drSpotPosition("RealizedBaseCurrency") = GetRealizedPIPSBaseCurrency(Key, drSpotPosition("Realized"), dsMarketData) 'dsPLTrade, accountID, dsMarketData)
        Catch ex As Exception
            Util.WriteDebugLog("RealizedBaseCurrency " & ex.Message)
            drSpotPosition("RealizedBaseCurrency") = 0
        End Try
        Try
            drSpotPosition("UnRealizedBaseCurrency") = GetUnRealizedPIPSBaseCurrency(Key, drSpotPosition("UnRealized"), dsMarketData) 'BidPrice, dsPLTrade, accountID, dsMarketData, OfferPrice)
        Catch ex As Exception
            Util.WriteDebugLog("UnRealizedBaseCurrency " & ex.Message)
            drSpotPosition("UnRealizedBaseCurrency") = 0
        End Try
        'If (Not (StrategyPerformanceReport._Instance Is Nothing)) Then
        RaiseEvent PLCalculation(drSpotPosition)
        'End If
    End Sub

    Private Sub UpdateAsyncDB(ByVal dr As DataRow)
        ah.UpdatePLCal(dr)
    End Sub

    Public Function GetRealizedPIPS(ByVal symbol As String, ByVal dsPLTrade As DataSet, ByVal senderID As String) As Decimal
        Dim filter As String = ""
        If (symbol <> "") Then
            filter = "Symbol = '" + symbol + "' AND SenderID = '" + senderID + "'"
        End If
        Dim tRPips As Decimal = 0
        Try
            tRPips = CDec(dsPLTrade.Tables(0).Compute("Sum(Pips)", filter))
            tRPips = Decimal.Round(CDec(tRPips), 2)
        Catch ex As Exception
            Throw ex
        End Try
        Return tRPips

    End Function

    Public Function GetUnRealizedPIPS(ByVal symbol As String, ByVal currentBidPrice As Double, ByVal dsPLTrade As DataSet, ByVal senderID As String, ByVal currentOfferPrice As Double) As Decimal
        Dim filter1 As String = ""
        Dim filter2 As String = ""
        If (symbol <> "") Then
            filter1 = "Actions = 1 AND Remaining > 0 AND Symbol = '" + symbol + "' AND SenderID = '" + senderID + "'" 'AND ServerDateTime >= " + tradingdate.Date.Date.ToOADate().ToString() + " AND ServerDateTime < " + tradingdate.Date.Date.AddDays(1).ToOADate().ToString()
            filter2 = "Actions = 2 AND Remaining > 0 AND Symbol = '" + symbol + "' AND SenderID = '" + senderID + "'" 'AND ServerDateTime >= " + tradingdate.Date.Date.ToOADate().ToString() + " AND ServerDateTime < " + tradingdate.Date.Date.AddDays(1).ToOADate().ToString()
        End If
        Dim drs() As DataRow = dsPLTrade.Tables(0).Select(filter1, "DateID")
        Dim totalPips As Decimal = 0

        If (drs.Length > 0) Then
            For Each dr As DataRow In drs
                totalPips = totalPips + (CDec(dr("Remaining")) * (currentBidPrice - CDec(dr("Price"))))
            Next
        Else
            drs = dsPLTrade.Tables(0).Select(filter2, "DateID")
            For Each dr As DataRow In drs
                totalPips = totalPips + (CDec(dr("Remaining")) * (CDec(dr("Price")) - currentOfferPrice))
            Next
        End If

        Return Decimal.Round(totalPips, 2)

    End Function
    Public Function GetRealizedPIPSBaseCurrency(ByVal symbol As String, ByVal tRPips As Decimal, ByVal dsMarketData As DataSet) As Decimal 'ByVal dsPLTrade As DataSet, ByVal senderID As String,
        Dim firstcurrency, secondcurrency, basecurrency As String
        basecurrency = se.BaseCurrency
        firstcurrency = EServerDependents.GetFirstCurrency(symbol)
        secondcurrency = EServerDependents.GetSecondCurrency(symbol)
        Try
            If (firstcurrency = basecurrency) Then 'case 1 first currency is base currency
                tRPips = tRPips / ah.GetBidPriceBySymbol(dsMarketData, symbol)
            ElseIf (secondcurrency = basecurrency) Then 'case 2 second currency is base currency
                tRPips = tRPips
            Else 'case 3 none of the currency is base currency
                Dim price As Double = 0
                price = ah.GetBidPriceBySymbol(dsMarketData, EServerDependents.GetCombinedCurrency(basecurrency, secondcurrency))
                If (price <> -1) Then
                    tRPips = tRPips / price
                Else
                    price = ah.GetBidPriceBySymbol(dsMarketData, EServerDependents.GetCombinedCurrency(secondcurrency, basecurrency))
                    If (price <> -1) Then
                        tRPips = tRPips * price
                    Else
                        tRPips = 0
                    End If
                End If
            End If
        Catch ex As Exception
            Util.WriteDebugLog("GetRealizedPIPSBaseCurrency --- " & ex.Message)
            tRPips = 0
        End Try

        Return Decimal.Round(CDec(tRPips), 2)
    End Function

    Public Function GetUnRealizedPIPSBaseCurrency(ByVal symbol As String, ByVal pipsBaseCurrency As Decimal, ByVal dsMarketData As DataSet) As Decimal 'ByVal currentBidPrice As Double, ByVal dsPLTrade As DataSet, ByVal senderID As String, ByVal dsMarketData As DataSet, ByVal currentOfferPrice As Double) As Decimal

        Dim firstcurrency, secondcurrency, basecurrency As String
        basecurrency = se.BaseCurrency
        firstcurrency = EServerDependents.GetFirstCurrency(symbol)
        secondcurrency = EServerDependents.GetSecondCurrency(symbol)

        If (firstcurrency = basecurrency) Then
            pipsBaseCurrency = pipsBaseCurrency / ah.GetBidPriceBySymbol(dsMarketData, symbol)
        ElseIf (secondcurrency = basecurrency) Then
            pipsBaseCurrency = pipsBaseCurrency
        Else
            Dim price As Double = 0
            Dim flag As Boolean = False
            Try
                price = ah.GetBidPriceBySymbol(dsMarketData, EServerDependents.GetCombinedCurrency(basecurrency, secondcurrency))
                If (price <> -1) Then
                    pipsBaseCurrency = pipsBaseCurrency / price
                Else
                    price = ah.GetBidPriceBySymbol(dsMarketData, EServerDependents.GetCombinedCurrency(secondcurrency, basecurrency))
                    If (price <> -1) Then
                        pipsBaseCurrency = pipsBaseCurrency * price
                    Else
                        pipsBaseCurrency = 0
                    End If
                End If
            Catch ex As Exception
                Util.WriteDebugLog("GetUnRealizedPIPSBaseCurrency()" & ex.Message)
                pipsBaseCurrency = 0
            End Try
        End If
        Return Decimal.Round(pipsBaseCurrency, 2)
    End Function
    'Private Function GetBidPriceBySymbol(ByVal dsMarketdata As DataSet, ByVal symbol As String) As Double
    '    Dim dv As DataView = dsMarketdata.Tables(0).DefaultView
    '    dv.RowFilter = "Symbol = '" + symbol + "'"
    '    If (dv.Count > 0) Then
    '        Return CDbl(dv(0)("BidPrice"))
    '    Else
    '        Return -1
    '    End If
    'End Function
    'Private Function GetOfferPriceBySymbol(ByVal dsMarketdata As DataSet, ByVal symbol As String) As Double
    '    Dim dv As DataView = dsMarketdata.Tables(0).DefaultView
    '    dv.RowFilter = "Symbol = '" + symbol + "'"
    '    Return CDbl(dv(0)("OfferPrice"))
    'End Function
    'Public Function GetSymbolsTradedByAccount(ByVal accountID As String, ByVal dsPLTrade As DataSet) As Hashtable
    '    Dim filter As String = ""
    '    filter = "SenderID = '" + accountID + "'" '"ServerDateTime >= " + tradingdate.Date.Date.ToOADate().ToString() + " AND ServerDateTime < " + tradingdate.Date.Date.AddDays(1).ToOADate().ToString()
    '    Dim drs() As DataRow = dsPLTrade.Tables(0).Select(filter)
    '    Return SelectDistinct(drs, "Symbol")
    'End Function

    Private Function SelectDistinct(ByVal sourceRows() As DataRow, ByVal sourceColumn As String) As Hashtable
        Try
            Dim ht As Hashtable = New Hashtable
            For Each dr As DataRow In sourceRows
                If Not ht.ContainsKey(dr(sourceColumn)) Then
                    ht.Add(dr(sourceColumn), Nothing)
                End If
            Next
            Return ht
        Catch ex As System.Exception
            Util.WriteDebugLog("SelectDistinct SPR " & ex.Message)
            Return Nothing
        End Try
    End Function
    Public Function GetAvgBuyRate(ByVal symbol As String, ByVal dsPLTrade As DataSet, ByVal senderID As String) As Decimal
        Try
            Dim filter As String = ""
            filter = "Symbol = '" + symbol + "' AND SenderID = '" + senderID + "' AND Actions = 1 "
            Dim drs() As DataRow = dsPLTrade.Tables(0).Select(filter)
            Dim totalAmount As Decimal = 0
            Dim avBR As Decimal = 0
            Dim sum As Decimal = 0
            If drs.Length <> 0 Then
                totalAmount = CDec(dsPLTrade.Tables(0).Compute("Sum(Amount)", filter))
                sum = CDec(dsPLTrade.Tables(0).Compute("Sum(NetAmount)", filter))
                avBR = sum / totalAmount
                Return Decimal.Round(avBR, 9)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetAvgSellRate(ByVal symbol As String, ByVal dsPLTrade As DataSet, ByVal senderID As String) As Decimal
        Try
            Dim filter As String = ""
            filter = "Symbol = '" + symbol + "' AND SenderID = '" + senderID + "' AND Actions = 2 "
            Dim drs() As DataRow = dsPLTrade.Tables(0).Select(filter)
            Dim totalAmount As Decimal = 0
            Dim avSR As Decimal = 0
            If drs.Length <> 0 Then
                totalAmount = CDec(dsPLTrade.Tables(0).Compute("Sum(Amount)", filter))
                Dim sum As Decimal = CDec(dsPLTrade.Tables(0).Compute("Sum(NetAmount)", filter))
                avSR = sum / totalAmount
                Return Decimal.Round(avSR, 9)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetNetCC1(ByVal symbol As String, ByVal dsPLTrade As DataSet, ByVal senderID As String) As Decimal
        Try
            Dim filter As String = ""
            filter = "Symbol = '" + symbol + "' AND SenderID = '" + senderID + "' AND Actions = 1 "
            Dim totalBuyAmount As Decimal = 0
            Dim drs() As DataRow
            drs = dsPLTrade.Tables(0).Select(filter)
            If drs.Length <> 0 Then totalBuyAmount = CDec(dsPLTrade.Tables(0).Compute("Sum(Amount)", filter))
            Dim totalSellAmount As Decimal = 0
            filter = "Symbol = '" + symbol + "' AND SenderID = '" + senderID + "' AND Actions = 2 "
            drs = dsPLTrade.Tables(0).Select(filter)
            If drs.Length <> 0 Then totalSellAmount = CDec(dsPLTrade.Tables(0).Compute("Sum(Amount)", filter))
            Return (totalBuyAmount - totalSellAmount)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetNetCC2(ByVal symbol As String, ByVal dsPLTrade As DataSet, ByVal senderID As String) As Decimal
        Try
            Dim filter As String = ""
            filter = "Symbol = '" + symbol + "' AND SenderID = '" + senderID + "' AND Actions = 1 "
            Dim totalBuyNetAmount As Decimal = 0
            Dim drs() As DataRow
            drs = dsPLTrade.Tables(0).Select(filter)
            If drs.Length <> 0 Then totalBuyNetAmount = CDec(dsPLTrade.Tables(0).Compute("Sum(NetAmount)", filter))
            Dim totalSellNetAmount As Decimal = 0
            filter = "Symbol = '" + symbol + "' AND SenderID = '" + senderID + "' AND Actions = 2 "
            drs = dsPLTrade.Tables(0).Select(filter)
            If drs.Length <> 0 Then totalSellNetAmount = CDec(dsPLTrade.Tables(0).Compute("Sum(NetAmount)", filter))
            Return (totalSellNetAmount - totalBuyNetAmount)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
