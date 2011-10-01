Dim sDrv,curdir,fso, OutFile, sReport, sFile,strText,strNewText,WshShell,wind
Set WshShell = CreateObject( "WScript.Shell" ) 

curdir=session.property("CustomActionData")

wind=replace(WshShell.currentdirectory,"system32","")

Const ForReading = 1, ForWriting = 2, ForAppending = 8

On Error Resume Next
Err.clear
Set fso = CreateObject("Scripting.FileSystemObject")

OutFile = wind + "winros.ini"

Set sReport = fso.OpenTextFile(OutFile,ForReading)
strText = sReport.ReadAll
sReport.Close
strNewText = Replace(strText, "E:\Scalper", curdir)
Set sReport = fso.OpenTextFile(OutFile, ForWriting)
sReport.WriteLine strNewText
sReport.Close


'Registering the dll for espeed
 Dim strFilePath,regMethod,strFilePath1
 strFilePath = WshShell.currentdirectory + "\libESPD.dll"
 
  
 strFilePath1 = WshShell.currentdirectory + "\fxcore.dll" 
 
 
Dim theFile, strFile, oShell, exitcode
 
 strFile = strFilePath
 Set oShell = CreateObject ("WScript.Shell")
 oShell.Run  WshShell.currentdirectory & "\regsvr32.exe /s " _
	& strFile, 0, False
 exitcode = oShell.Run(WshShell.currentdirectory & "\regsvr32.exe /s " _
	& strFile, 0, False)
 EchoB("regsvr32.exe exitcode = " & exitcode)
 Cleanup oShell

 'Set oShell = CreateObject ("WScript.Shell") 'strFile = strFilePath1  'oShell.Run  WshShell.currentdirectory & "\regsvr32.exe /s " _'	& strFile, 0, False 'exitcode = oShell.Run(WshShell.currentdirectory & "\regsvr32.exe /s " _'	& strFile, 0, False) 'EchoB("regsvr32.exe exitcode = " & exitcode) 'Cleanup oShell 
