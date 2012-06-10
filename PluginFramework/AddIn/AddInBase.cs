using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;

namespace PluginFramework.AddIn
{
    [Serializable]
    public abstract class AddInBase : IBundleActivator
    {
        #region IBundleActivator Members

        public abstract void Start(IBundleContext context);

        public abstract void Stop(IBundleContext context);

        #endregion
    }
}
