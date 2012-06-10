using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginFramework.AddIn;

namespace PluginFramework.AddIn.Core
{
    public class BundleRepository : IBundleRepository
    {
        private List<IBundle> bundlesByInstallOrder;

        private Dictionary<int, IBundle> bundlesById;

        private Dictionary<string, IBundle> bundlesBySymbolicName;

      
        private object syncObj;

        public int Count
        {
            get
            {
                lock (this)
                {
                    return bundlesByInstallOrder.Count;
                }
            }
        }

        public BundleRepository()
        {
            this.bundlesByInstallOrder = new List<IBundle>();
            this.bundlesById = new Dictionary<int, IBundle>();
            this.bundlesBySymbolicName = new Dictionary<string, IBundle>();

            this.syncObj = new object();
        }

        public IBundle this[int index]
        {
            get
            {
                lock (syncObj)
                {
                    return bundlesByInstallOrder[index];
                }
            }
        }

        public IBundle[] GetBundles()
        {
            lock (syncObj)
            {
                return bundlesByInstallOrder.ToArray<IBundle>();
            }
        }

        public IBundle GetBundle(int bundleId)
        {
            lock (syncObj)
            {
                return (Bundle)bundlesById[bundleId];
            }
        }

        public IBundle[] GetBundles(string symbolicName)
        {
            lock (syncObj)
            {
                List<IBundle> bundles = new List<IBundle>();
                Dictionary<string, IBundle>.Enumerator enumerator = bundlesBySymbolicName.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (string.Compare(enumerator.Current.Key, symbolicName, true) == 0)
                    {
                        bundles.Add(enumerator.Current.Value);
                    }
                }

                return bundles.ToArray<IBundle>();
            }
        }

        public IBundle GetBundle(string symbolicName, Version version)
        {
            IBundle[] bundles = GetBundles(symbolicName);
            if (bundles != null)
            {
                if (bundles.Length > 0)
                {
                    // for (int i = 0; i < bundles.Length; i++)
                    //{
                    //if (bundles[i].Version.Equals(version))
                    //   {
                    return bundles[0];
                    //  }
                    //}
                }
            }
            return null;
        }

        public void Register(IBundle bundle)
        {
            lock (syncObj)
            {
                bundlesByInstallOrder.Add(bundle);
                bundlesById.Add(bundle.Id, bundle);
                bundlesBySymbolicName.Add(bundle.SymbolicName, bundle);
            }
        }

        public bool Unregister(IBundle bundle)
        {
            lock (syncObj)
            {
                // remove by install order
                bool found = bundlesByInstallOrder.Remove(bundle);
                if (!found)
                {
                    return false;
                }

                // remove by bundle ID
                bundlesById.Remove(bundle.Id);

                bundlesBySymbolicName.Remove(bundle.SymbolicName);

                return true;
            }
        }


        public List<String> BundlePaths
        {
            get
            {
                lock (syncObj)
                {
                    List<String> dirs = new List<String>();
                    Dictionary<string, IBundle>.Enumerator enumerator = bundlesBySymbolicName.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        String path = AppDomain.CurrentDomain.BaseDirectory + enumerator.Current.Value.DataInfo.Path;
                        if (!dirs.Contains(path))
                        {
                            dirs.Add(path);
                        }
                    }
                    return dirs;
                }
            }
        }
    }
}
