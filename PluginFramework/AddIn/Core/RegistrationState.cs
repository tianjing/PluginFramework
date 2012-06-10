using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginFramework.AddIn.Core
{
    public enum RegistrationState
    {
        Registered = 0x00,
        Unregistering = 0x01,
        Unregistered = 0x02
    }
}
