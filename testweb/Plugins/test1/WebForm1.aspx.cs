using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Configuration;
using System.Data;
using Log2Service;
namespace test1
{
    public partial class WebForm1 : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Response.Clear();
                string ss = Activator.Context.GetService<IBLL.IMyBll>().hello();
                ss += "   " + Activator.Context.GetService<TestIBll.ISayYear>().Year();
                ss += "   " + Activator.Context.GetService<TestIBll.IMessage>().HelloMessage();
                Activator.Context.GetService<Log2Service.ITextLog>().OutLog("12321");

                Response.Write(ss);
            }
        }

        protected void test_Click(object sender, EventArgs e)
        {
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        protected void StopBlun_Click(object sender, EventArgs e)
        {
            Activator.Context.Bundle.Stop();
        }
    }
}
