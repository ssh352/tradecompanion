
Partial Class FrmLogOut
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Session("UserName") = ""
        Session("Access") = ""
        SignInFlag = False
        Session.Abandon()
        Session.RemoveAll()

    End Sub
End Class
