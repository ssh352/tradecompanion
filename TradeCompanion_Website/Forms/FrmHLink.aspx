<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FrmHLink.aspx.vb" Inherits="FrmHLink" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script language=javascript type="text/javascript">
    function load()
    {
    //alert(document.getElementById('TextBox1').value);
    window.open(document.getElementById('HiddenField1').value); 
    }
    </script>
</head>
<body onload=javascript:load()>
    <form id="form1" runat="server">
    <div>
        &nbsp;<asp:HiddenField ID="HiddenField1" runat="server" />
    </div>
    </form>
</body>
</html>
