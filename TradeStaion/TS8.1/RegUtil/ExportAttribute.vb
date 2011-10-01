'=====================================================================
' This file is part of the Desaware SpyWorks Professional sample.
'
' Copyright ©2001-2003 Desaware Inc. All rights reserved.
'
'This source code is intended only as a supplement to Microsoft
'Development Tools and/or on-line documentation. See these other
'materials for detailed information regarding Microsoft code samples.
'
'THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
'KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
'IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
'PARTICULAR PURPOSE.
'=====================================================================
' Export function sample project, ExportAttribute.vb file
' Copyright ©2001-2003 by Desaware Inc. All Rights Reserved
' www.desaware.com
Option Strict On
Option Explicit On 

' Desaware Function Export Template file for Visual Basic .NET
' Include this file with your .NET Project to Export Functions from the Assembly.

<AttributeUsage(AttributeTargets.Method)> Public Class ExportedAttribute

    Inherits System.Attribute

    Public ExportName As String

    Public Ordinal As Short

    Public CCall As Boolean

    Public Sub New(ByVal ExportNameValue As String, ByVal OrdinalValue As Short)

        MyBase.New()

        ExportName = ExportNameValue

        Ordinal = OrdinalValue

    End Sub

    Public Overrides Function ToString() As String

        Return ExportName & " &(" & Ordinal.ToString() & ")"

    End Function

End Class
