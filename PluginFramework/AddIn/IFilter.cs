using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginFramework.AddIn
{
    public interface IFilter
    {
        bool Match(IServiceReference reference);

        bool Match(Dictionary<string, object> dictionary);

        bool MatchCase(Dictionary<string, object> dictionary);

        string ToString();

        bool Equals(Object obj);

        int GetHashCode();
    }
}
