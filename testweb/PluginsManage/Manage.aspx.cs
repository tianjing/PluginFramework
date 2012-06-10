using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PluginFramework.AddIn;
namespace testweb.PluginsManage
{
    public partial class Manage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ProcessRequest();
            bundles.DataSource = Activator.Context.Bundles;

            bundles.DataBind();

            
        }

        protected void ProcessRequest()
        {
            String type = Request.QueryString["type"];
            Int32 Id = 0;
            Int32.TryParse(Request.QueryString["Id"], out Id);
            if (!String.IsNullOrEmpty(type) && Id > 0)
            {
                switch (type)
                {
                    case "Uninstall": UnInstall(Id); break;
                    case "Start": Start(Id); break;
                    case "Stop": Stop(Id); break;
                    default: break;
                }
            }
            else if (!String.IsNullOrEmpty(type)&&Id == 0)
            {
                Response.Flush();
                Response.Write("请不要操作核心组件");
                Response.End();
            }
        }
        protected Boolean UnInstall(Int32 id)
        {
            IBundle bundle = Activator.Context.GetBundle(id);
            bundle.Uninstall();
            return false;
        }
        protected Boolean Start(Int32 id)
        {
            IBundle bundle = Activator.Context.GetBundle(id);
            if (bundle.State == BundleState.Installed)
            {
                bundle.Start();
                return true;
            }
            return false;
        }
        protected Boolean Stop(Int32 id)
        {
            IBundle bundle = Activator.Context.GetBundle(id);
            if (bundle.State == BundleState.Active)
            {
                bundle.Stop();
                return true;
            }
            return false;
        }

    }
}