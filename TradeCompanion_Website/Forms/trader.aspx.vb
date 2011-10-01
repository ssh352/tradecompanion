Imports WSScalper
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI.Page

Partial Class Trader
    Inherits Page
    Dim TraderIDQuery As String = "select LOGINID from users where ACTIVES='T' order by LOGINID"
    Dim SenderIDQuery As String = "select Distinct SENDERID from PLTRADE where TCID='"
    Dim SymbolQuery As String = "Select Distinct SYMBOL from PLTRADE where TCID='"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)

        If Session("UserName") = "" And Session("Access") < 5 Then
            'Response.Write("<script language='javascript'>window.open ('frmLogin.aspx', '_top')</script>")
        End If

        If Not Me.IsPostBack Then
            lblMessage.Text = ""
            GetTraderID()
            GetExistPage()
            stDate.DateTimeDisplayFormat = SilkWebware.DateTimeSelector.DateTimeDisplayFormats.MMDDYY
            edDate.DateTimeDisplayFormat = SilkWebware.DateTimeSelector.DateTimeDisplayFormats.MMDDYY
            EnblDsblControl(False)
        End If

    End Sub

    Protected Sub CreatePage_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        SltTrader.Text = TraderList.SelectedItem.ToString
        GetSenderID()
        GetSymbols()
        SltSenderID.Text = ""
        SltSymbols.Text = ""
        If Symbols.Items.Count > 0 And SenderID.Items.Count > 0 Then
            EnblDsblControl(True)
        Else
            EnblDsblControl(False)
        End If
    End Sub

    Public Sub GetTraderID()

        Dim ds As New System.Data.DataSet
        Dim WSS As New WSScalper.WebServicesScalper()
        ds = WSS.GetUsersDSFromQuery(TraderIDQuery)
        TraderList.DataSource = ds.Tables(0)
        TraderList.DataTextField = "loginid"
        TraderList.DataValueField = "loginid"
        TraderList.DataBind()

    End Sub

    Public Sub GetExistPage()

        Dim sql As String = "select trader from pageref"
        Dim conn As OleDb.OleDbConnection

        'Dim conString As String = "provider=Microsoft.Jet.OLEDB.4.0; Data source=c:\inetpub\wwwroot_test\app_data\scalper.mdb;"

        'Dim conString As String = "provider=Microsoft.Jet.OLEDB.4.0; Data source=D:\web Applications\Scalper\App_Data\SCALPER.mdb;"

        Dim conString As String = ConfigurationSettings.AppSettings("DatabaseConnectionString")

        Dim ds As New DataSet
        conn = New OleDb.OleDbConnection(conString)
        conn.Open()
        Dim da As New Data.OleDb.OleDbDataAdapter(sql, conn)
        da.Fill(ds)
        conn.Close()
        ExistPageList.DataSource = ds.Tables(0)
        ExistPageList.DataTextField = "trader"
        ExistPageList.DataValueField = "trader"
        ExistPageList.DataBind()
    End Sub

    Public Sub GetSenderID()

        Dim ds As New System.Data.DataSet
        Dim WSS As New WebServicesScalper()
        SenderIDQuery = SenderIDQuery + SltTrader.Text + "'"
        ds = WSS.GetPLTradeDSFromQuery(SenderIDQuery)
        SenderID.DataSource = ds.Tables(0)
        SenderID.DataTextField = "SenderID"
        SenderID.DataValueField = "SenderID"
        SenderID.DataBind()

    End Sub

    Public Sub GetSymbols()

        Dim ds As New System.Data.DataSet
        Dim WSS As New WebServicesScalper()
        SymbolQuery = SymbolQuery + SltTrader.Text + "'"
        ds = WSS.GetPLTradeDSFromQuery(SymbolQuery)
        Symbols.DataSource = ds.Tables(0)
        Symbols.DataTextField = "symbol"
        Symbols.DataValueField = "symbol"
        Symbols.DataBind()

    End Sub

    Protected Sub SenderID_Selected(ByVal sender As Object, ByVal e As System.EventArgs) Handles SenderID.SelectedIndexChanged

        Dim i As Integer
        SltSenderID.Text = ""
        For i = 0 To SenderID.Items.Count - 1
            If SenderID.Items(i).Selected = True Then
                SltSenderID.Text = SltSenderID.Text + SenderID.Items(i).Value + " "
            End If
        Next

    End Sub

    Protected Sub Symbols_Selected(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim i As Integer
        SltSymbols.Text = ""
        For i = 0 To Symbols.Items.Count - 1
            If Symbols.Items(i).Selected = True Then
                SltSymbols.Text = SltSymbols.Text + Symbols.Items(i).Value + " "
            End If
        Next

    End Sub

    Protected Sub Logo_Selected(ByVal sender As Object, ByVal e As System.EventArgs)
        Preview.ImageUrl = "Logo" + "/" + LogoImage.SelectedItem.ToString()
    End Sub
    Protected Sub LogoImage_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles LogoImage.Load

        If Not IsPostBack Then
            'Dim dir() As String = Directory.GetFiles("c:\inetpub\wwwroot_test\forms\Logo")
            'Dim dir() As String = Directory.GetFiles("D:\web Applications\Scalper\From Client\Logo")

            Dim dir() As String = Directory.GetFiles(ConfigurationSettings.AppSettings("LogoPath"))

            Dim i As Integer
            For i = 0 To dir.Length - 1
                Dim str() As String = dir(i).Split("\")
                LogoImage.Items.Add(str(str.Length - 1))
            Next
            Logo_Selected(New Object, New EventArgs)
        End If

    End Sub
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        SltTrader.Text = ""
        txtTraderName.Text = ""
        SltSenderID.Text = ""
        SltSymbols.Text = ""
        stDate.SelectedDate = DateTime.Today
        edDate.SelectedDate = DateTime.Today
        txtAddress.Text = ""
        txtContact.Text = ""
        txtDescription.Text = ""
        TxtEmail.Text = ""
        txtTraderName.Text = ""
        EnblDsblControl(False)
    End Sub
    Protected Sub EnblDsblControl(ByVal arg As Boolean)
        LogoImage.Enabled = arg
        Symbols.Enabled = arg
        SenderID.Enabled = arg
        txtAddress.Enabled = arg
        txtContact.Enabled = arg
        txtDescription.Enabled = arg
        TxtEmail.Enabled = arg
        txtTraderName.Enabled = arg
    End Sub
    Protected Sub TraderList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TraderList.SelectedIndexChanged
        lblMessage.Text = ""
        btnReset_Click(sender, e)
    End Sub
    Protected Sub ViewPage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ViewPage.Click
        Response.Redirect("chart.aspx?name=" + ExistPageList.SelectedItem.ToString)
    End Sub
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim td As String = ""
        Dim QS As String = "Proccess.aspx?name=;"
        Dim flag As Boolean
        lblMessage.ForeColor = Drawing.Color.Red
        flag = True
        If SltSymbols.Text = "" Then
            lblMessage.Text = "Symbol should be selected."
            flag = False
            Return
        Else
            QS = QS + "QSSymbols=" + SltSymbols.Text + ";"
        End If
        If SltSenderID.Text = "" Then
            lblMessage.Text = "Atleast one senderID should be selected."
            flag = False
            Return
        Else
            QS = QS + "QSSenderID=" + SltSenderID.Text + ";"
        End If
        If txtTraderName.Text = "" Then
            lblMessage.Text = "Please Enter Trader Name."
            flag = False
            Return
        Else
            QS = QS + "QSTraderName=" + txtTraderName.Text + ";"
        End If
        If stDate.SelectedDate.ToString = "" Then
            lblMessage.Text = "Start Date is not set."
            flag = False
            Return
        Else
            QS = QS + "QSstDate=" + stDate.SelectedDate.ToString + ";"
        End If
        If edDate.SelectedDate.ToString = "" Then
            lblMessage.Text = "End Date is not set."
            flag = False
            Return
        Else
            QS = QS + "QSedDate=" + edDate.SelectedDate.ToString + ";"
        End If
        If SltTrader.Text = "" Then
            lblMessage.Text = "Trader Name can't be blank."
            flag = False
            Return
        Else
            QS = QS + "QSTrader=" + SltTrader.Text + ";"
        End If
        td = txtAddress.Text
        td = td.Replace(Chr(13) + Chr(10), "@")
        td = td.Replace(" ", ",")
        If td = "" Then
            lblMessage.Text = "Trader Address can't be blank."
            flag = False
            Return
        Else
            QS = QS + "QSAddress=" + td + ";"
        End If
        If txtContact.Text = "" Then
            lblMessage.Text = "Please give contact no."
            flag = False
            Return
        Else
            QS = QS + "QSContact=" + txtContact.Text + ";"
        End If
        If TxtEmail.Text = "" Then
            lblMessage.Text = "Please give Email ID."
            flag = False
            Return
        Else
            QS = QS + "QSEmail=" + TxtEmail.Text + ";"
        End If
        td = txtDescription.Text
        td = td.Replace(Chr(13) + Chr(10), "@")
        td = td.Replace(" ", ",")
        If td = "" Then
            lblMessage.Text = "Trader Description can't be blank."
            flag = False
            Return
        Else
            QS = QS + "QSDescription=" + td + ";"
        End If
        If LogoImage.SelectedItem.ToString = "" Then
            lblMessage.Text = "You must select one from Logo."
            flag = False
            Return
        Else
            QS = QS + "QSLogo=" + LogoImage.SelectedItem.ToString + ";"
        End If
        If flag Then
            lblMessage.Text = ""
            Response.Redirect(QS, True)
        End If
    End Sub
    Protected Sub DeletePage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DeletePage.Click

        Dim sql As String = "Delete from PageRef where Trader='" + ExistPageList.SelectedItem.ToString + "'"

        ' Change By RG Infotech
        'Dim conString As String = "provider=Microsoft.Jet.OLEDB.4.0; Data source=c:\inetpub\wwwroot_test\app_data\scalper.mdb;"

        Dim conString As String = ConfigurationSettings.AppSettings("DatabaseConnectionString")


        Dim conn As OleDb.OleDbConnection = New OleDb.OleDbConnection(conString)
        Dim DBUpdate As New OleDb.OleDbCommand()
        DBUpdate.CommandText = sql
        DBUpdate.Connection = conn
        DBUpdate.Connection.Open()
        DBUpdate.ExecuteNonQuery()
        DBUpdate.Connection.Close()
        GetExistPage()
    End Sub

    'Protected Sub Button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button.Click
    '    Dim i As Integer
    '    SltSenderID.Text = ""
    '    For i = 0 To SenderID.Items.Count - 1
    '        If SenderID.Items(i).Selected = True Then
    '            SltSenderID.Text = SltSenderID.Text + SenderID.Items(i).Value + " "
    '        End If
    '    Next
    'End Sub
End Class
