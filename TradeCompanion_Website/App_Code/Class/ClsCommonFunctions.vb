'PURPOSE: - FILL DATA IN LIST BOX, DROP DOWN AND RETURNS SINGLE QUOTED STRING FOR QUERY
'DEPANDANCY: - TO USE THIS CLASS YOU NEED TO INCLUDE CLSDATAACCESS.VB CLASS.

Imports System.Data

Public Class ClsCommonFunctions
#Region "Variables"
    Private lDouble As Double
    Private lInteger As Integer
    Private lString As String
    Private lDataAccess As ClsDatabaseAccess
    Private lSQLQuery As String
    Private lDataReader As IDataReader
#End Region

    'Function used to fill data in drop down list 

    Public Sub FillDataInDropDown(ByRef cmbBox As DropDownList, ByVal tblName As String, ByVal argDataTextFieldName As String, Optional ByVal argDataValueFieldName As String = "", Optional ByVal argCondition As String = "")
        If Trim(tblName) = "" Then Exit Sub
        If Trim(argDataTextFieldName) = "" Then Exit Sub

        If Trim(argDataValueFieldName) = "" Then
            lSQLQuery = "Select [" & argDataTextFieldName & "] from " & tblName & " " & argCondition
        Else
            lSQLQuery = "Select [" & argDataTextFieldName & "], [" & argDataValueFieldName & "] from " & tblName & " " & argCondition
        End If


        lDataAccess = New ClsDatabaseAccess
        lDataReader = lDataAccess.ExecReader(lSQLQuery)
        cmbBox.Items.Clear()

        'cmbBox.Items.Add("Select Country")

        While lDataReader.Read
            cmbBox.Items.Add(lDataReader(argDataTextFieldName))
            If Trim(argDataValueFieldName) <> "" Then
                cmbBox.Items(cmbBox.Items.Count - 1).Value = lDataReader(argDataValueFieldName)
            End If
        End While

        lDataAccess.DestroyReaderObject()
        lDataAccess = Nothing
    End Sub

    'Function will return a single quoted string for direct query from database
    Public Function GetSingleQuotedString(ByVal argString As String) As String
        lString = ""
        For lInteger = 1 To argString.Length

            If Asc(Mid(argString, lInteger, 1)) = 39 Then
                If lInteger <> 1 And lInteger <> argString.Length Then
                    lString = lString & Mid(argString, lInteger, 1) & "'"
                Else
                    lString = lString & Mid(argString, lInteger, 1)
                End If
            Else
                lString = lString & Mid(argString, lInteger, 1)
            End If
        Next
        GetSingleQuotedString = lString
    End Function

    'Sub used to clear contents of the form
    'Public Sub ClearFormContents(ByVal frm As Form)
    '    For Each ctl As Control In frm.Controls
    '        If TypeOf ctl Is TextBox Then
    '            ctl.Text = ""
    '        ElseIf TypeOf ctl Is ComboBox Then
    '            Dim cmb As ComboBox
    '            cmb = ctl
    '            cmb.SelectedIndex = -1
    '        ElseIf TypeOf ctl Is ListBox Then
    '            Dim lstBox As ListBox
    '            lstBox = ctl
    '            lstBox.SelectedIndex = -1
    '            'ElseIf TypeOf ctl Is TabControl Then
    '            '    Dim tabcntl As TabControl
    '            '    tabcntl = ctl


    '        End If

    '    Next
    'End Sub

    'Public Sub ClearTabControlContents(ByVal TabCtl As TabControl)

    '    For Each ctl As Control In TabCtl.TabPages
    '        If TypeOf ctl Is TextBox Then
    '            ctl.Text = ""
    '        End If
    '    Next

    'End Sub
    'Function used to fill data in list box


    '***************************************
    'Public Sub FillDataInListBox(ByRef lstBox As ListBox, ByVal tblName As String, ByVal argDataTextFieldName As String, Optional ByVal argDataValueFieldName As String = "", Optional ByVal argCondition As String = "")

    'If Trim(tblName) = "" Then Exit Sub
    'If Trim(argDataTextFieldName) = "" Then Exit Sub

    'If Trim(argDataValueFieldName) = "" Then
    '    lSQLQuery = "Select [" & argDataTextFieldName & "] from " & tblName & " " & argCondition
    'Else
    '    lSQLQuery = "Select [" & argDataTextFieldName & "], [" & argDataValueFieldName & "] from " & tblName & " " & argCondition
    'End If

    'lDataAccess = New ClsDatabaseAccess
    'lDataReader = lDataAccess.ExecReader(lSQLQuery)

    'lstBox.Items.Clear()
    'While lDataReader.Read
    '    lstBox.Items.Add(lDataReader(argDataTextFieldName))
    '    If Trim(argDataValueFieldName) <> "" Then
    '        lstBox.Items(lstBox.Items.Count - 1) = lDataReader(argDataValueFieldName)
    '    End If
    'End While

    'lDataAccess.DestroyReaderObject()
    'lDataAccess = Nothing

    'End Sub


End Class
