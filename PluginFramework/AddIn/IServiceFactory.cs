using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginFramework.AddIn
{
    public interface IServiceFactory
    {
        object GetService(IBundle bundle, IServiceRegistration registration);

        void UngetService(IBundle bundle, IServiceRegistration registration,
            object service);
    }
}
