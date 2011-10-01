
Partial Class Forms_Admin_FrmAdmin
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("UserName") = "" And Session("Access") < 5 Then
            'Response.Write("<script language='javascript'>window.open ('frmLogin.aspx', '_top')</script>")
        End If

    End Sub
End Class
