using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginFramework.AddIn
{
    /// <summary>
    /// Customizes the starting and stopping of a bundle.
    /// </summary>
    public interface IBundleActivator
    {
        void Start(IBundleContext context);

        void Stop(IBundleContext context);
    }
}
