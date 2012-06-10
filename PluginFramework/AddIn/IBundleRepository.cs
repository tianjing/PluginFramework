using System;
using System.Collections.Generic;

namespace PluginFramework.AddIn
{
    public interface IBundleRepository
    {
        void Register(IBundle bundle);
        bool Unregister(IBundle bundle);
        int Count { get; }
        IBundle GetBundle(int bundleId);
        IBundle GetBundle(string symbolicName, Version version);
        IBundle[] GetBundles(string symbolicName);
        IBundle[] GetBundles();
        IBundle this[int index] { get; }
        List<String> BundlePaths { get; }
    }
}
