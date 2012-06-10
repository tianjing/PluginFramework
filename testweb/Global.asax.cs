using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace testweb
{
    public class Global : PluginFramework.WebLaunch
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            base.Application_Start(sender, e);
        } 

    }
}