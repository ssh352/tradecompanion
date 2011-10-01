Imports System.Data.OleDb

Partial Class MstInternal

    Inherits System.Web.UI.MasterPage
    
    '*****************************************

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'If Request.QueryString("Flag") = "TradeCompanion" Then
        '    Response.Write("<script language='javascript'>alert ('TradeCompanion');</script>")
        'ElseIf Request.QueryString("Flag") = "TradeStation2000i" Then
        '    Response.Write("<script language='javascript'>alert ('TradeStation2000i');</script>")
        'End If

        If Session("UserName") = "" Then
            SignInFlag = False
        Else
            SignInFlag = True
        End If

    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click

        Session("URL") = Request.UrlReferrer.AbsoluteUri

        Response.Write("<script language='javascript'>window.open ('FrmSendtofrd.aspx', target='_blank', 'height=400, width=350')</script>")

    End Sub

End Class

