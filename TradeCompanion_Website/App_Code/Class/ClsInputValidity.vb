'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Project ID :- PRJ021                                                      '
' Purpose    :- Class For Checking the Proper Input values for a TextBox    '
' Created By :- Nitin Bansal                                                '
' Depandancy :- None                                                        '
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Imports System.Text.RegularExpressions

Public Class ClsInputValidity

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Function For Check The Input Should Accept Only Numeric Values, Decimal & Negative Sign  '
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    'Public Sub NumericInputValidate(ByVal e As System.Windows.Forms.KeyPressEventArgs, ByVal TxtBox As System.Windows.Forms.TextBox)

    '    Dim KeyAscii As Integer
    '    KeyAscii = Asc(e.KeyChar)

    '    Select Case KeyAscii

    '        Case 48 To 57, 8, 13       'These are 0-9, backspace, and carriage returns

    '            'These are fine, do nothing

    '        Case 45                    'Minus sign (Number can only have 1 minus sign)

    '            If InStr(TxtBox.Text, "-") <> 0 Then

    '                KeyAscii = 0

    '            End If

    '            'If the insertion point is not sitting at zero, throw away the minus sign
    '            'Minus can Only Come On First Place

    '            Dim i As Integer

    '            If TxtBox.SelectionStart <> 0 Then
    '                KeyAscii = 0
    '            End If

    '        Case 46               'Period (Number can have only 1 period)

    '            If InStr(TxtBox.Text, ".") <> 0 Then
    '                KeyAscii = 0
    '            End If

    '        Case Else
    '            'provide no handling for any other keys 
    '            KeyAscii = 0

    '    End Select

    '    If KeyAscii = 0 Then
    '        e.Handled = True
    '    Else
    '        e.Handled = False
    '    End If

    'End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '' ***  Function For Check The Input Should Accept Only Numeric Values & Negative Sign   ***'
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    'Public Sub NumericInputValidateWithoutDecimal(ByVal e As System.Windows.Forms.KeyPressEventArgs, ByVal TxtBox As System.Windows.Forms.TextBox)

    '    Dim KeyAscii As Integer
    '    KeyAscii = Asc(e.KeyChar)

    '    Select Case KeyAscii

    '        Case 48 To 57, 8, 13       'These are 0-9, backspace, and carriage returns

    '            'These are fine, do nothing

    '        Case Else
    '            'provide no handling for any other keys 
    '            KeyAscii = 0

    '    End Select

    '    If KeyAscii = 0 Then
    '        e.Handled = True
    '    Else
    '        e.Handled = False
    '    End If

    'End Sub
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Function For Check The Input Should Accept Only String  '
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    'Public Sub StringInputValidate(ByVal e As System.Windows.Forms.KeyPressEventArgs, ByVal TxtBox As System.Windows.Forms.TextBox)

    '    Dim KeyAscii As Integer
    '    KeyAscii = Asc(e.KeyChar)

    '    Select Case KeyAscii

    '        Case 97 To 122, 65 To 90, 8, 32, 13    'These are a-z,A-Z, Backspace, Space and carriage returns

    '            'These are fine, do nothing

    '        Case Else                'provide no handling for any other keys

    '            KeyAscii = 0

    '    End Select

    '    If KeyAscii = 0 Then
    '        e.Handled = True
    '    Else
    '        e.Handled = False
    '    End If

    'End Sub

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Function For Check The Input Should Accept Only AlphaNumeric  ''''
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    'Public Sub AlphaNumericInputValidate(ByVal e As System.Windows.Forms.KeyPressEventArgs, ByVal TxtBox As System.Windows.Forms.TextBox)

    '    Dim KeyAscii As Integer
    '    KeyAscii = Asc(e.KeyChar)

    '    Select Case KeyAscii

    '        Case 97 To 122, 65 To 90     'These are a-z,A-Z

    '        Case 8, 32, 13               'Backspace, Space and carriage

    '        Case 48 To 57, 8, 13         'These are 0-9

    '        Case Else                    'provide no handling for any other keys

    '            KeyAscii = 0

    '    End Select

    '    If KeyAscii = 0 Then
    '        e.Handled = True
    '    Else
    '        e.Handled = False
    '    End If

    'End Sub
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '      Function For Check The Valide EMail Input       ''''
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    Function EmailInputValidate(ByVal emailAddress As String) As Boolean

        Dim pattern As String = "^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
        Dim emailAddressMatch As Match = Regex.Match(emailAddress, pattern)

        If emailAddressMatch.Success Then
            EmailInputValidate = True
        Else
            EmailInputValidate = False
            'MsgBox("Please Enter Valid EMail ID")

        End If

    End Function

    'Function EmailInputValidate(ByVal emailAddress As String) As Boolean

    '    Dim pattern As String = "^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
    '    Dim emailAddressMatch As Match = Regex.Match(emailAddress, pattern)

    '    If Not emailAddress = "" Then
    '        If emailAddressMatch.Success Then
    '            EmailInputValidate = True
    '        Else
    '            EmailInputValidate = False
    '            MsgBox("Please Enter Valid EMail ID")
    '        End If
    '    End If


    'End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '     Function For Check The Valide Web URL Input      ''''
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    Function WebAddressInputValidate(ByVal webAddress As String) As Boolean

        Dim pattern As String = "www.([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"

        Dim WebAddressMatch As Match = Regex.Match(webAddress, pattern)

        If WebAddressMatch.Success Then
            WebAddressInputValidate = True
        Else
            WebAddressInputValidate = False
        End If

    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '     Function For Check The Valide Phone No Input     ''''
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    'Function PhoneNoValidate(ByVal e As System.Windows.Forms.KeyPressEventArgs, ByVal TxtBox As System.Windows.Forms.TextBox)

    '    Dim KeyAscii As Integer
    '    KeyAscii = Asc(e.KeyChar)

    '    Select Case KeyAscii

    '        Case 48 To 57, 8, 45, 13      'These are 0-9, backspace, Hyphen and carriage returns

    '            'These are fine, do nothing

    '        Case Else
    '            'provide no handling for any other keys 
    '            KeyAscii = 0

    '    End Select

    '    If KeyAscii = 0 Then
    '        e.Handled = True
    '    Else
    '        e.Handled = False
    '    End If

    'End Function

End Class
