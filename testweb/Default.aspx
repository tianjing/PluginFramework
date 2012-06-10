<%@ Page Language="C#" CodeBehind="Default.aspx.cs" Inherits="testweb._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
</head>
<body>
    <form id="form1" runat="server" >
    <div>
    fdfdfd
    <asp:Button runat="server" ID="Install_Btn" OnClick="Install_Btn_OnClick" Text="安装" />
     <asp:Button runat="server" ID="test"  Text="测试" Onclick="test_Click" />
        <asp:Button ID="Button1" runat="server" Text="卸载组件" onclick="Button1_Click" />
    </div>
   
    </form>
    </body>
</html>
