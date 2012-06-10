
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;


namespace PluginFramework
{
    /// <summary>
    /// Author ：TG
    /// </summary>
    public class WebLaunch : System.Web.HttpApplication
    {
        protected virtual void Application_Start(object sender, EventArgs e)
        {
            PluginFramework.PluginManage.Current.WebAppLaunch();
        }

        protected virtual void Session_Start(object sender, EventArgs e)
        {

        }

        protected virtual void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected virtual void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected virtual void Application_Error(object sender, EventArgs e)
        {

        }

        protected virtual void Session_End(object sender, EventArgs e)
        {

        }

        protected virtual void Application_End(object sender, EventArgs e)
        {

        }
    }
}

