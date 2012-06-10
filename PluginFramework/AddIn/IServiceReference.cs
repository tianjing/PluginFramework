using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginFramework.AddIn
{
    public interface IServiceReference
    {
        [Obsolete]
        object GetProperty(string key);

        [Obsolete]
        string[] GetPropertyKeys();

        Dictionary<string, object> Properties { get; }

        IBundle GetBundle();

        IBundle[] GetUsingBundles();

        bool IsAssignableTo(IBundle bundle, string className);

        string[] GetClasses();
    }
}
