<%@ Control language="c#" Inherits="Forum.header" CodeFile="header.cs" %>
<img src="images/forum1.jpg">
<table>
<tr><td valign="top" >


  <table id="Menu_holder" runat="Server" style="width: 100%">
  
  <tr>
  
    <td style="">
    <asp:HyperLink NavigateUrl="index.aspx" id=Menu_Field1
style="font-family: Arial; font-size: 13px" runat="server"><img border="0" src="images/home.gif"></asp:HyperLink></td>
    <td style="">
    <asp:HyperLink NavigateUrl="newthread.aspx" id=Menu_Field2
style="font-family: Arial; font-size: 13px" runat="server"><img border="0" src="images/new.gif"></asp:HyperLink></td>
    </tr>
  </table>

</td>
 </tr></table>