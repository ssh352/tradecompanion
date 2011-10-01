
'******************************************************************
'**********  Class For Generating Password Automatically **********
'**********             Dependancy :: NONE               **********
'******************************************************************

Imports System.Security.Cryptography

Public Class ClsGeneratePswd

    Public Function getRandomAlphaNumeric() As String

        Dim rm As RandomNumberGenerator
        Dim sRand As String = ""
        Dim sTmp As String = ""
        Dim nCnt As Integer = 0

        rm = RandomNumberGenerator.Create()

        Dim data(3) As Byte

        rm.GetNonZeroBytes(data)

        While nCnt <= data.Length - 1

            Dim nVal = Convert.ToInt32(data.GetValue(nCnt))
            If ((nVal >= 48 And nVal <= 57) Or (nVal >= 65 And nVal <= 90) Or (nVal >= 97 And nVal <= 122)) Then
                sTmp = Convert.ToChar(nVal).ToString()
            Else
                sTmp = nVal.ToString
            End If
            sRand = sRand + sTmp.ToString

            nCnt = nCnt + 1

        End While

        Return sRand

    End Function


End Class
