Imports System.Runtime.InteropServices
Public Class NSTfeed
    Declare Auto Sub setdata Lib "C:\NeuroShell Trader 5\Servers\TradeCompanion DataPump.dll" Alias "setdata" (<MarshalAs(UnmanagedType.AnsiBStr)> ByVal ticker As String, ByVal da As Double, ByVal tickprice As Double)
    Declare Auto Sub sethisdata Lib "C:\NeuroShell Trader 5\Servers\TradeCompanion DataPump.dll" Alias "sethisdata" (<MarshalAs(UnmanagedType.AnsiBStr)> ByVal ticker As String, ByVal da As Double, ByVal tickprice As Double, ByVal tickClose As Double, ByVal tickHigh As Double, ByVal tickLow As Double, ByVal type As Single)
    Declare Auto Function getHistoricalSymbol Lib "C:\NeuroShell Trader 5\Servers\TradeCompanion DataPump.dll" Alias "getHistoricalSymbol" () As <MarshalAs(UnmanagedType.AnsiBStr)> String
    Declare Auto Function getHistoricalBarInterval Lib "C:\NeuroShell Trader 5\Servers\TradeCompanion DataPump.dll" Alias "getHistoricalBarInterval" () As Single
    Declare Auto Function getHistoricalDataTime Lib "C:\NeuroShell Trader 5\Servers\TradeCompanion DataPump.dll" Alias "getHistoricalDataTime" () As Double
    Declare Auto Sub SetFlag Lib "C:\NeuroShell Trader 5\Servers\TradeCompanion DataPump.dll" Alias "SetFlag" ()
    Declare Auto Sub SetTCStatus Lib "C:\NeuroShell Trader 5\Servers\TradeCompanion DataPump.dll" Alias "SetTCStatus" (ByVal TcStatus As Boolean)
End Class
