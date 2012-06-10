using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginFramework.AddIn
{
    public interface IServiceRegistration
    {
        IServiceReference GetReference();

        Dictionary<string, object> Properties { set; }

        void Unregister();
    }
}
