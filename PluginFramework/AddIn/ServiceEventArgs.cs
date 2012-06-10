using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginFramework.AddIn
{
    public class ServiceEventArgs:EventArgs
    {
        private IServiceReference reference;
        private ServiceState state;

        public IServiceReference Reference
        {
            get { return reference; }
        }

        public ServiceState State
        {
            get { return state; }
        }

        public ServiceEventArgs(ServiceState state, IServiceReference reference)
        {
            this.reference = reference;
            this.state = state;
        }
    }
}
