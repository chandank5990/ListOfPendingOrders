<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Drawing2.aspx.cs" Inherits="Points_Pending_Supplier.Drawing2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Button ID="Button1" runat="server" 
            style="z-index: 1; left: 174px; top: -1px; position: absolute" 
            Text="Drawing" onclick="Button1_Click" />
        <br />
        <br />
        <asp:Image ID="Image1" runat="server" 
            style="z-index: 1; left: 0px; position: absolute; height: 520px; width: 471px; top: -1px;" 
            ImageAlign="Middle" 
             
            />
    
    </div>
    </form>
</body>
</html>
