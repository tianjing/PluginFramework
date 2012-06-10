using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Collections;
using System.Configuration;
using PluginFramework.AddIn;
namespace testweb
{
    public partial class _Default : System.Web.UI.Page
    {

        
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        protected void Install_Btn_OnClick(object sender, EventArgs e)
        {
            string path = @"E:\KingPan\测试项目\插件项目\testweb\testweb\Plugins\test1\bin\test1.dll";
            string binpath = @"E:\KingPan\测试项目\插件项目\testweb\testweb\Plugins\test1\bin";
            Assembly[] asses = AppDomain.CurrentDomain.GetAssemblies();


           // OsguLoad();

        }


        protected void test_Click(object sender, EventArgs e)
        {
          string ss=  Activator.Context.GetService<IBLL.IMyBll>().hello();
          Response.Write(ss);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Activator.Context.Bundle.Stop();
        }
    }
}
