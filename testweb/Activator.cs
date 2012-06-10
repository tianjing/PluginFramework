using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PluginFramework.AddIn;
using BLL;
namespace testweb
{
    [Serializable]
    [AddIn("Activator")]
    public class Activator:AddInBase
    {
        private static IBundleContext mcontext;
        public static IBundleContext Context
        {
            get { return mcontext; }
        }

        IServiceRegistration mybllserver = null;
        public override void Start(IBundleContext context)
        {
           IBLL.IMyBll mybll = new BLL.MyBll();
           mybllserver=context.RegisterService<IBLL.IMyBll>(mybll, null);
           mcontext = context;
        }

        public override void Stop(IBundleContext context)
        {
            mybllserver.Unregister();
            mcontext = null;
        }
    }
}