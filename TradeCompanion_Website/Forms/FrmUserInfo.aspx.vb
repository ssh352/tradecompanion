Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports FirebirdSql.Data.FirebirdClient
Imports System.Security.Cryptography


Partial Class FrmUserInfo

    Inherits System.Web.UI.Page
    Private lDataAccess As ClsDatabaseAccess
    Private objProviderFact As ClsProviderFactory
    Dim ObjClsInputValidity As ClsInputValidity
    Dim ObjPswdGen As ClsGeneratePswd

    Dim ValidityFlag As Boolean

    Dim MailSub As String
    Dim MailTo As String
    Dim MailFrom As String
    Dim MailCC As String
    Dim MailBody As String
    Dim ServerName As String
    Dim ObjClsMail As ClsMail
    Dim sb As New StringBuilder
    Dim Flag As Boolean
    Dim StrOrder As String
    Dim connectionstring As String = System.Configuration.ConfigurationManager.AppSettings("FireBirdDBConnectionString")
    Dim PASSWORD_CHARS_LCASE As String = "abcdefgijkmnopqrstwxyz"
    Dim PASSWORD_CHARS_UCASE As String = "ABCDEFGHJKLMNPQRSTWXYZ"
    Dim PASSWORD_CHARS_NUMERIC As String = "0123456789"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'If Session("UserName") = "" And Session("Access") < 5 Then
        'Response.Write("<script language='javascript'>window.open ('frmLogin.aspx', '_top')</script>")
        'End If


        If Page.IsPostBack = False Then
            Dim obj As ClsCommonFunctions
            obj = New ClsCommonFunctions
            '''''''''''''''''''''''''''''''''''''''''''
            DDLCountry.Items.Add("- Please select -")
            '''''''''''''''''''''''''''''''''''''''''''
            obj.FillDataInDropDown(DDLCountry, "Countries", "CountryName", "CountryID", "")
            obj = Nothing
        End If


        If Page.IsPostBack = False Then

            DDLHearAbtUs.Items.Add("- Please select -")
            DDLHearAbtUs.Items.Add("Bloomberg")
            DDLHearAbtUs.Items.Add("Reuters")
            DDLHearAbtUs.Items.Add("Google Ad")
            DDLHearAbtUs.Items.Add("TradeStation US")
            DDLHearAbtUs.Items.Add("Internet Search")
            DDLHearAbtUs.Items.Add("Rferred by Friend")
            DDLHearAbtUs.Items.Add("Other")

        End If


        If Request.QueryString("Flag") = "TradeCompanion" Then
            StrOrder = "Autoshark"
            'Response.Write("<script language='javascript'>alert ('Autoshark');</script>")

        ElseIf Request.QueryString("Flag") = "TradeStation2000i" Then
            StrOrder = "TradeStation 2000i"
            'Response.Write("<script language='javascript'>alert ('TradeStation2000i');</script>")

        ElseIf Request.QueryString("Flag") = "RadarScreen2000i" Then
            StrOrder = "RadarScreen 2000i"
            'Response.Write("<script language='javascript'>alert ('RadarScreen2000i');</script>")

        ElseIf Request.QueryString("Flag") = "OptionStation2000i" Then
            StrOrder = "OptionStation 2000i"
            'Response.Write("<script language='javascript'>alert ('OptionStation2000i');</script>")

        ElseIf Request.QueryString("Flag") = "ProSuite2000i" Then
            StrOrder = "ProSuite 2000i"
            'Response.Write("<script language='javascript'>alert ('ProSuite2000i');</script>")

        ElseIf Request.QueryString("Flag") = "TradeStation8.0" Then
            StrOrder = "TradeStation 8.0"
            'Response.Write("<script language='javascript'>alert ('TradeStation8.0');</script>")
        End If


        'If Page.IsPostBack = False Then

        '    Dim content As ContentPlaceHolder
        '    content = Page.Master.FindControl("ContentPlaceHolder1")
        '    Dim ddl As DropDownList
        '    'ddl = content.FindControl("DDLHearAbtUs")

        '    ddl = content.FindControl("DDLCountry")

        '    'ddl.Items.Add("aaaaaa")
        '    'ddl.Items.Add("bbbbb")
        '    'ddl.Items.Add("ccccc")
        '    'ddl.Items.Add("dddddd")

        '    Dim obj As ClsCommonFunctions
        '    obj = New ClsCommonFunctions
        '    obj.FillDataInDropDown(ddl, "Countries", "CountryName", "CountryID", "")
        '    obj = Nothing

        'End If

    End Sub
    Public Function Generate(ByVal minLength As Integer, ByVal maxLength As Integer) As String

        If minLength <= 0 OrElse maxLength <= 0 OrElse minLength > maxLength Then
            Return Nothing
        End If

        Dim charGroups As Char()() = New Char()() {PASSWORD_CHARS_LCASE.ToCharArray(), PASSWORD_CHARS_UCASE.ToCharArray(), PASSWORD_CHARS_NUMERIC.ToCharArray()}

        Dim charsLeftInGroup As Integer() = New Integer(charGroups.Length - 1) {}
        For i As Integer = 0 To charsLeftInGroup.Length - 1
            charsLeftInGroup(i) = charGroups(i).Length
        Next

        Dim leftGroupsOrder As Integer() = New Integer(charGroups.Length - 1) {}
        For i As Integer = 0 To leftGroupsOrder.Length - 1
            leftGroupsOrder(i) = i
        Next
        Dim randomBytes As Byte() = New Byte(3) {}

        Dim rng As New RNGCryptoServiceProvider()
        rng.GetBytes(randomBytes)

        ' Convert 4 bytes into a 32-bit integer value. 
        Dim seed As Integer = (randomBytes(0) And 127) << 24 Or randomBytes(1) << 16 Or randomBytes(2) << 8 Or randomBytes(3)
        Dim random As New Random(seed)
        Dim password As Char() = Nothing
        If minLength < maxLength Then
            password = New Char(random.[Next](minLength, maxLength + 1) - 1) {}
        Else
            password = New Char(minLength - 1) {}
        End If

        Dim nextCharIdx As Integer
        Dim nextGroupIdx As Integer
        Dim nextLeftGroupsOrderIdx As Integer
        Dim lastCharIdx As Integer

        Dim lastLeftGroupsOrderIdx As Integer = leftGroupsOrder.Length - 1
        For i As Integer = 0 To password.Length - 1

            If lastLeftGroupsOrderIdx = 0 Then
                nextLeftGroupsOrderIdx = 0
            Else
                nextLeftGroupsOrderIdx = random.[Next](0, lastLeftGroupsOrderIdx)
            End If

            nextGroupIdx = leftGroupsOrder(nextLeftGroupsOrderIdx)

            lastCharIdx = charsLeftInGroup(nextGroupIdx) - 1

            If lastCharIdx = 0 Then
                nextCharIdx = 0
            Else
                nextCharIdx = random.[Next](0, lastCharIdx + 1)
            End If

            password(i) = charGroups(nextGroupIdx)(nextCharIdx)

            If lastCharIdx = 0 Then
                charsLeftInGroup(nextGroupIdx) = charGroups(nextGroupIdx).Length
            Else
                If lastCharIdx <> nextCharIdx Then
                    Dim temp As Char = charGroups(nextGroupIdx)(lastCharIdx)
                    charGroups(nextGroupIdx)(lastCharIdx) = charGroups(nextGroupIdx)(nextCharIdx)
                    charGroups(nextGroupIdx)(nextCharIdx) = temp
                End If
                charsLeftInGroup(nextGroupIdx) -= 1
            End If

            If lastLeftGroupsOrderIdx = 0 Then
                lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1
            Else
                If lastLeftGroupsOrderIdx <> nextLeftGroupsOrderIdx Then
                    Dim temp As Integer = leftGroupsOrder(lastLeftGroupsOrderIdx)
                    leftGroupsOrder(lastLeftGroupsOrderIdx) = leftGroupsOrder(nextLeftGroupsOrderIdx)
                    leftGroupsOrder(nextLeftGroupsOrderIdx) = temp
                End If
                lastLeftGroupsOrderIdx -= 1
            End If
        Next

        Return New String(password)
    End Function

    Function AddUser(ByVal loginId As String, ByVal userName As String, ByVal emailId As String, ByVal phoneNo As String, ByVal address As String, ByVal city As String, ByVal country As String) As Integer
        Dim emailText As String
        emailText = "PERSONAL DETAILS" + Environment.NewLine + Environment.NewLine
        emailText = emailText + "Loginid: " + loginId + Environment.NewLine
        emailText = emailText + "Name: " + userName + Environment.NewLine
        emailText = emailText + "Address: " + address + Environment.NewLine
        emailText = emailText + "Town/City: " + city + Environment.NewLine
        emailText = emailText + "Country: " + country + Environment.NewLine + Environment.NewLine
        emailText = emailText + "CONTACT INFO" + Environment.NewLine + Environment.NewLine
        emailText = emailText + "Telephone: " + phoneNo + Environment.NewLine
        emailText = emailText + "Email Address: " + emailId + Environment.NewLine + Environment.NewLine

        Dim fbcon As New FbConnection(connectionstring)
        fbcon.Open()
        Dim query As String = "insert into users (id,loginid, username,passwords, emailid,phoneno,actives,loggedin,address,RegistrationDate,Trial,Lastupdated) values (gen_id(gen_users_id, 1), @LoginId,@Username,@Password,@EmailId,@Phoneno,@Actives,@Loggedin,@Address,@RegistrationDate,@Trial,@Lastupdated);"
        Dim objCmd As New FbCommand(query)

        Dim paramLoginID As New FbParameter("@LoginID", FbDbType.VarChar, 50)
        paramLoginID.Value = loginId
        objCmd.Parameters.Add(paramLoginID)

        Dim paramUsername As New FbParameter("@Username", FbDbType.VarChar, 20)
        paramUsername.Value = userName
        objCmd.Parameters.Add(paramUsername)

        'Create and Encrypt the password
        Dim pwd As String = Generate(8, 10)
        Dim md5Hasher As New MD5CryptoServiceProvider()
        Dim hashedBytes As Byte()
        Dim encoder As New UTF8Encoding()
        hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(pwd))
        Dim paramPwd As New FbParameter("@Password", FbDbType.Char, 16)
        paramPwd.Value = hashedBytes
        objCmd.Parameters.Add(paramPwd)

        Dim paramEmailId As New FbParameter("@EmailId", FbDbType.VarChar, 50)
        paramEmailId.Value = emailId
        objCmd.Parameters.Add(paramEmailId)


        Dim paramPhoneno As New FbParameter("@Phoneno", FbDbType.VarChar, 20)
        paramPhoneno.Value = phoneNo
        objCmd.Parameters.Add(paramPhoneno)

        Dim paramActives As New FbParameter("@Actives", FbDbType.Char, 1)
        paramActives.Value = "T"
        objCmd.Parameters.Add(paramActives)

        Dim paramLoggedIn As New FbParameter("@Loggedin", FbDbType.Char, 1)
        paramLoggedIn.Value = "N"
        objCmd.Parameters.Add(paramLoggedIn)

        Dim paramAddress As New FbParameter("@Address", FbDbType.VarChar, 50)
        paramAddress.Value = address + " " + city + " " + country
        objCmd.Parameters.Add(paramAddress)

        Dim paramRegistrationDate As New FbParameter("@RegistrationDate", FbDbType.TimeStamp)
        paramRegistrationDate.Value = DateTime.Now
        objCmd.Parameters.Add(paramRegistrationDate)

        Dim paramTrial = New FbParameter("@Trial", FbDbType.Char, 1)
        paramTrial.Value = "Y"
        objCmd.Parameters.Add(paramTrial)

        Dim paramLastupdated = New FbParameter("@Lastupdated", FbDbType.TimeStamp)
        paramLastupdated.Value = DateTime.Now
        objCmd.Parameters.Add(paramLastupdated)


        Try
            objCmd.Connection = fbcon
            Dim rowseffected As Integer = objCmd.ExecuteNonQuery()

            If (rowseffected >= 1) Then
                Dim subject As String = "[BGC Autoshark] User Info Request"
                fbcon.Close()
                Dim Body As String
                Body = "Hi ," + Environment.NewLine
                Body = Body + "A request was made to send you your  password for the" + Environment.NewLine
                Body = Body + "BGC Autoshark Login. Your details are as follows:" + Environment.NewLine + Environment.NewLine
                Body = Body + "Loginid    : " + loginId + Environment.NewLine
                Body = Body + "Password    : " + pwd + Environment.NewLine + Environment.NewLine + Environment.NewLine
                Body = Body + "Regards, " + Environment.NewLine + "-BGC"

                Dim mailObj As New ClsMail()

                'send the new password by email
                Dim mailed As Boolean = mailObj.SendMail(subject, emailId, "UserInfo@tradercompanion.co.uk", "", Body, "88.208.220.198")

                If (mailed) Then
                    Return 1
                Else
                    'TODO delete user from FireBird DB
                    Return -4
                End If


            Else
                fbcon.Close()
                Return -5
            End If
        Catch ex As Exception
            fbcon.Close()
            Try
                If (ex.Message.Substring(0, 45) = "violation of PRIMARY or UNIQUE KEY constraint") Then
                    Return -3
                Else
                    Return -5
                End If
            Catch
                Return -6
            End Try
        End Try

    End Function

    Protected Sub BttnSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BttnSend.Click


        '**********   Validate Mendatory ields ************************
        If TxtEMailAdd.Text = "" Then
            Response.Write("<script language='javascript'>alert ('Please Enter EMail Address');</script>")
            Exit Sub
        Else
            ObjClsInputValidity = New ClsInputValidity
            ValidityFlag = ObjClsInputValidity.EmailInputValidate(TxtEMailAdd.Text)
            If ValidityFlag = False Then
                Response.Write("<script language='javascript'>alert ('Please Enter Valid EMail Address');</script>")
                TxtEMailAdd.Text = ""
                Exit Sub
            End If
        End If

        '  ******  Check Duplicate UserName **************************
        'TODO call webservice to validate an existing user
        'Dim wsServ As WSScalper.WebServicesScalper = New WSScalper.WebServicesScalper
        'Dim result As Boolean = wsServ.IsLoginIDExist(TxtEMailAdd.Text().Trim())

        Dim result As Boolean
        Dim fbcon As New FbConnection(connectionstring)
        fbcon.Open()
        Dim query As String
        query = "select * from USERS where LOGINID='" + TxtEMailAdd.Text + "'"
        Dim fbcmd As New FbCommand(query, fbcon)
        Dim fda As New FbDataAdapter(fbcmd)
        Dim ds As New DataSet
        fda.Fill(ds)
        If ds.Tables(0).Rows.Count = 1 Then
            result = True
        Else
            result = False
        End If
        fbcon.Close()

        If (result = True) Then
            Response.Write("<script language='javascript'>alert ('Sorry, This User Name is already taken.');</script>")
            TxtEMailAdd.Text = ""
            Exit Sub
        End If

        '  ***********************************************************

        If TxtFirstName.Text = "" Then
            Response.Write("<script language='javascript'>alert ('Please Enter First Name');</script>")
            Exit Sub
        End If

        If TxtAddress1.Text = "" Then
            Response.Write("<script language='javascript'>alert ('Please Enter Address1');</script>")
            Exit Sub
        End If

        If TxtTown_City.Text = "" Then
            Response.Write("<script language='javascript'>alert ('Please Enter Town/City');</script>")
            Exit Sub
        End If

        If TxtPostalCode.Text = "" Then
            Response.Write("<script language='javascript'>alert ('Please Enter PostalCode');</script>")
            Exit Sub
        End If

        ' Country

        If TxtContactPhoneNo.Text = "" Then
            Response.Write("<script language='javascript'>alert ('Please Enter Contact Phone No');</script>")
            Exit Sub
        End If

        'TODO Call Webservice to add a new user to Firebird DB
        'Dim result1 As Integer = wsServ.AddUser(TxtEMailAdd.Text().Trim(), TxtFirstName.Text(), TxtEMailAdd.Text, TxtContactPhoneNo.Text, TxtAddress1.Text, TxtTown_City.Text, DDLCountry.SelectedValue)

        Dim result1 As Integer
        result1 = AddUser(TxtEMailAdd.Text().Trim(), TxtFirstName.Text(), TxtEMailAdd.Text, TxtContactPhoneNo.Text, TxtAddress1.Text, TxtTown_City.Text, DDLCountry.SelectedValue)
        
        If (result1 = -10 Or result1 = -11) Then
            Response.Write("Email id already registered")
            Exit Sub
        ElseIf (result1 = -2) Then
            Response.Write("Connection Problem, please try after some time")
            Exit Sub
        ElseIf (result1 = -4) Then
            Response.Write("Email error, please try after some time")
            Exit Sub
        ElseIf (result1 = -5) Then
            Response.Write("Database error, please contact software provider")
            Exit Sub
        End If

        'lDataAccess = New ClsDatabaseAccess
        'lDataAccess.ConnectToDatabase()
        'objProviderFact = New ClsProviderFactory
        'lDataAccess.objCommand = objProviderFact.GetCommandType
        'lDataAccess.objCommand.Connection = lDataAccess.objConnection
        'lDataAccess.objCommand.CommandText = "Insert into TestAdmin (UserName,YourName,Address1,Address2,County, PostCode,Telephone,Access) Values (' " & TxtEMailAdd.Text & " ',' " & TxtFirstName.Text & " " & TxtLastName.Text & " ' ,' " & TxtAddress1.Text & " ',' " & TxtAddress2.Text & " ',' " & DDLCountry.SelectedItem.Text & " ',' " & TxtPostalCode.Text & " ',' " & TxtContactPhoneNo.Text & " ',0 )"
        'lDataAccess.objCommand.ExecuteNonQuery()
        'lDataAccess.DisconnectFromDatabase()

        ' *********  Sending Mail to Admin  ********************************************

        '        MailSub = "Completed Order Form : " & Request.QueryString("Flag")
        MailSub = "[TC] New Registration Info : " & StrOrder
        'MailTo = TxtSendTo.Text
        MailTo = System.Configuration.ConfigurationManager.AppSettings("AdminID")
        MailFrom = TxtEMailAdd.Text
        ServerName = System.Configuration.ConfigurationManager.AppSettings("ServerStr")
        ' **************  Sending Mail *********************************
        ' ********    Making Table For Mail body ***********************
        ' **************************************************************
        sb.Append("<Table>")
        sb.Append("<tr>")
        sb.Append("<td>")
        sb.Append(" -******- PERSONAL DETAILS -******->")
        sb.Append("</td>")
        sb.Append("</tr>")
        sb.Append("<tr>")
        sb.Append("<td> First Name : " & TxtFirstName.Text)
        sb.Append("</td>")
        sb.Append("</tr>")
        sb.Append("<tr>")
        sb.Append("<td> Company : " & TxtCompany.Text)
        sb.Append("</td>")
        sb.Append("</tr>")
        sb.Append("<tr>")
        sb.Append("<td> Address 1 : " & TxtAddress1.Text)
        sb.Append("</td>")
        sb.Append("</tr>")
        sb.Append("<tr>")
        sb.Append("<td> Town/City : " & TxtTown_City.Text)
        sb.Append("</td>")
        sb.Append("<tr>")
        sb.Append("<td> Province : " & TxtProvince.Text)
        sb.Append("</td>")
        sb.Append("</tr>")
        sb.Append("<tr>")
        sb.Append("<td> Postal Code : " & TxtPostalCode.Text)
        sb.Append("</td>")
        sb.Append("</tr>")
        sb.Append("<tr>")
        sb.Append("<td> Country : " & DDLCountry.SelectedItem.Text)
        sb.Append("</td>")
        sb.Append("</tr>")
        sb.Append("<tr>")
        sb.Append("<td>")
        sb.Append(" -******- CONTACT INFO -******- ")
        sb.Append("</td>")
        sb.Append("</tr>")
        sb.Append("<tr>")
        sb.Append("<td> Telephone : " & TxtContactPhoneNo.Text)
        sb.Append("</td>")
        sb.Append("</tr>")
        sb.Append("<tr>")
        sb.Append("<td> Fax : " & TxtFaxNo.Text)
        sb.Append("</td>")
        sb.Append("</tr>")
        sb.Append("<tr>")
        sb.Append("<td> Email Address : " & TxtEMailAdd.Text)
        sb.Append("</td>")
        sb.Append("</tr>")
        sb.Append("</Table>")
        MailBody = sb.ToString
        '************************************************************************

        ObjClsMail = New ClsMail

        'TODO Check this mail to admin
        Flag = ObjClsMail.SendMail(MailSub, MailTo, MailFrom, "", MailBody, ServerName)

        'If Flag = True Then
        '    td1.Visible = True
        '    td2.Visible = True
        '    td3.Visible = True
        '    td4.Visible = True

        'End If
        '******************************************************************************
        Response.Redirect("FrmUserSubmit.aspx")
        '******************************************************************************

    End Sub


    'Protected Sub BttnSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BttnSend.Click
    '    Response.Write("<script language='javascript'>alert ('Sorry, This User Name is already taken.');</script>")
    'End Sub

    'Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
    '    Response.Write("<script language='javascript'>alert ('Sorry, This User Name is already taken.');</script>")
    'End Sub

End Class