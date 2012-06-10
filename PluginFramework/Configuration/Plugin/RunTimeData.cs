using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginFramework.Configuration.Plugin
{
    public class RuntimeData
    {
        public List<DependencyData> Dependencies
        {
            get;
            private set;
        }
        public AssemblyData Assemblie
        {
            get;
            private set;
        }

        public void SetAssembly(AssemblyData assembly)
        {
            Assemblie=assembly;
        }
        public void AddDependency(DependencyData newItem)
        {
            if (null == Dependencies)
            {
                Dependencies = new List<DependencyData>();
            }
            Dependencies.Add(newItem);
        }
    }
}
