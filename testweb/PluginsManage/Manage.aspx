<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="testweb.PluginsManage.Manage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Repeater ID="bundles" runat="server">
            <ItemTemplate>
                <a>
                    <%#Eval("Id")%></a>
                <br />
                <a>
                    <%#Eval("SymbolicName")%></a>
                <br />
                <a>
                    <%#Eval("State")%></a>
                <br />
                
                <input name="Uninstall" type="button" value='卸载' onclick="javascript:window.location='Manage.aspx?type=Uninstall&Id=<%#Eval("Id") %>'" />
                <input name="Start" type="button" value='启动' onclick="javascript:window.location='Manage.aspx?type=Start&Id=<%#Eval("Id") %>'" />
                <input name="Stop" type="button" value='停止' onclick="javascript:window.location='Manage.aspx?type=Stop&Id=<%#Eval("Id") %>'" />
                <br /><span>----------------------------</span><br />
            </ItemTemplate>
        </asp:Repeater>
    </div>
    </form>
</body>
</html>
