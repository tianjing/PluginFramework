using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PluginFramework.AddIn;
namespace Log2Net
{
    [Serializable]
    [AddIn("Activator")]
    public class Activator : AddInBase
    {
        private static IBundleContext mcontext;
        public static IBundleContext Context
        {
            get { return mcontext; }
        }

        IServiceRegistration mybllserver = null;
        public override void Start(IBundleContext context)
        {
            Log2Service.ITextLog log = new Log2Net.TextLog();
            mybllserver = context.RegisterService<Log2Service.ITextLog>(log, null);
            mcontext = context;
        }

        public override void Stop(IBundleContext context)
        {
            mybllserver.Unregister();
            mcontext = null;
        }
    }
}