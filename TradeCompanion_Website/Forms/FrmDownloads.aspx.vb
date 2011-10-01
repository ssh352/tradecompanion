
Partial Class Forms_FrmDownloads
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        LblUser.Text = Session("UserName")

    End Sub
End Class
