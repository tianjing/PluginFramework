using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginFramework.AddIn
{
    /// <summary>
    /// Prototype of the method to be implemented to receive service events.
    /// </summary>
    /// <param name="sender">The event sender</param>
    /// <param name="e">The event argurment <see cref="ServiceEventArgs"/></param>
    public delegate void ServiceEventHandler(object sender, ServiceEventArgs e);
}
