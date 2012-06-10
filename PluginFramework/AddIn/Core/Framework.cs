using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading;
using PluginFramework.AddIn;
using PluginFramework.AddIn.Services;
using PluginFramework.AddIn.Utility;
using System.Collections.Generic;
using PluginFramework.Configuration.Plugin;

namespace PluginFramework.AddIn.Core
{
    /// <summary>
    /// Main class of the Innosys framework. This class is internal, not accessible from outside the
    /// Innosys assembly.
    /// </summary>
    public class Framework : IFramework
    {
        #region --- Constant ---
        private static readonly string[] BundleExtention = {".dll",".exe"};
        private const string InitConfigurationFileName = @"Configuration\Init.config";
        #endregion

        #region --- Fields ---
        private BundleRepository bundleRepository;
        private ServiceRegistry serviceRegistry;
        private DirectoryInfo cache;
        private SystemBundle systemBundle;
        private BundleContext systemBundleContext;
        private int bundleAppDomains;
        private int serviceId;
        #endregion

        #region --- Properties ---
        public IBundleRepository Bundles
        {
            get
            {
                return bundleRepository;
            }
        }
        internal ServiceRegistry ServiceRegistry
        {
            get
            {
                return serviceRegistry;
            }
        }
        internal DirectoryInfo Cache
        {
            get
            {
                return cache;
            }
            set
            {
                cache = value;
            }
        }
        #endregion

        public event FrameworkEventHandler FrameworkEvent
        {
            add
            {
                EventManager.FrameworkEvent += value;
            }
            remove
            {
                EventManager.FrameworkEvent -= value;
            }
        }
        public Framework()
        {
            InitializeFramework();
        }
        private void InitializeFramework()
        {
            serviceId = 1;
            if (bundleRepository == null)
            {
                bundleRepository = new BundleRepository();
            }
            else
            {
                throw new InvalidOperationException("The framework is already started");
            }

            if (serviceRegistry == null)
            {
                serviceRegistry = new ServiceRegistry();
            }
            else
            {
                throw new InvalidOperationException("The framework is already started");
            }

            systemBundle = new SystemBundle(this);
            systemBundleContext = (BundleContext)systemBundle.Context;
        }
 
        private void SetupConfiguration(string fileName)
        {
            string configDirectory = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(configDirectory))
            {
                Directory.CreateDirectory(configDirectory);
            }
            if (!File.Exists(fileName))
            {
                Preferences.Instance.Save(fileName);
            }
        }

        public void Launch()
        {
            InstallBundleInternal(systemBundle);
            StartBundle(systemBundle);
        }

        public void Close()
        {
            Debug.Assert(bundleRepository != null);

            for (Int32 i = bundleRepository.Count - 1; i > -1; i--)
            {
                bundleRepository[i].Stop();
            }
        }

        public void Shutdown()
        {
            Debug.Assert(bundleRepository != null);

            for (int i = bundleRepository.Count - 1; i >= 0; i--)
            {
                IBundle bundle = bundleRepository[i];
                int bundleId = bundle.Id;
                StopBundle(bundleId);
                UninstallBundle(bundleId);
                bundle = null;
            }
        }
        /// <summary>
        /// ValidExtention
        /// </summary>
        /// <param name="location">File full path</param>
        /// <returns></returns>
        private Boolean ValidExtention(String location)
        {
            for (Int32 i = 0; i < BundleExtention.Length; i++)
            {
                if (location.IndexOf(BundleExtention[i],
                 StringComparison.OrdinalIgnoreCase) >0)
                {
                    return true;
                }
            }
            return false;
        }
        public IBundle InstallBundle(String location)
        {
            if (!ValidExtention(location))
            {
                location = location + BundleExtention[0];
            }

            string fullLocation = string.Empty;

            bool isPathRooted = Path.IsPathRooted(location);

            if (isPathRooted)
            {
                fullLocation = location;
            }

            if (!File.Exists(fullLocation))
            {
                throw new BundleException(String.Format("Bundle {0} not found.", location),
                    new FileNotFoundException(String.Format("file:{0} not found.", fullLocation)));
            }

            // Create the bundle object
            BundleData bundleData = new BundleData();
            bundleData.Id = bundleRepository.Count;
            bundleData.Location = fullLocation;

            Bundle bundle = new Bundle(bundleData, this);

            CheckInstallBundle(bundle);

            InstallBundleInternal(bundle);
            return bundle;
        }
        public IBundle InstallBundle(string location, BundleData bd)
        {
            if (!ValidExtention(location))
            {
                location = location + BundleExtention;
            }

            string fullLocation = string.Empty;

            bool isPathRooted = Path.IsPathRooted(location);

            if (isPathRooted)
            {
                fullLocation = location;
            }

            if (!File.Exists(fullLocation))
            {
                throw new BundleException(String.Format("Bundle {0} not found.", location),
                    new FileNotFoundException(String.Format("file:{0} not found.", fullLocation)));
            }

            // Create the bundle object
            BundleData bundleData = bd;
            bundleData.Id = bundleRepository.Count;
            bundleData.Location = fullLocation;

            Bundle bundle = new Bundle(bundleData, this);

            CheckInstallBundle(bundle);

            InstallBundleInternal(bundle);
            return bundle;
        }
        private void CheckInstallBundle(Bundle bundle)
        {
            IBundle existsBundle = bundleRepository.GetBundle(bundle.SymbolicName, null);
            if (existsBundle != null)
            {
                throw new BundleException("Bundle is already installed.");
            }
        }

        private IBundle InstallBundleInternal(Bundle bundle)
        {
            bundleRepository.Register(bundle);

            if (bundle.State == BundleState.Installed)
            {
                EventManager.OnBundleChanged(
                    new BundleEventArgs(
                        BundleTransition.Installed, bundle)
                        );
            }

            return bundle;
        }

        public void UninstallBundle(int id)
        {
            try
            {
                IBundle bundle = bundleRepository.GetBundle(id);
                bundleRepository.Unregister(bundle);
            }
            catch (Exception ex)
            {
                throw new BundleException("Uninstall bundle threw a exception.", ex);
            }
        }

        public IBundle StartBundle(int id)
        {
            IBundle bundle = bundleRepository.GetBundle(id);
            if (bundle == null)
            {
                throw new BundleException(String.Format("Bundle not found.BundleId:{0}", id));
            }
            if (bundle.State != BundleState.Installed)
            {
                throw new BundleException("Bundle is aready started.");
            }
            bundle.Start();

            return bundle;
        }

        public void StopBundle(int id)
        {
            IBundle bundle = bundleRepository.GetBundle(id);
            if (bundle == null)
            {
                throw new BundleException(
                    String.Format("Bundle not found.BundleId:{0}", id));
            }
            if (bundle.State != BundleState.Active)
            {
                throw new BundleException("Bundle is not active.");
            }
            bundle.Stop();
        }

        public void StartBundle(IBundle bundle)
        {
            if (bundle == null)
            {
                throw new ArgumentNullException("bundle");
            }
            bundle.Start();
        }

        public IServiceReference[] GetServiceReferences(string clazz, string filterString,
            IBundleContext context, bool allservices)
        {
            Filter filter = string.IsNullOrEmpty(filterString) ? null : new Filter(filterString);
            IServiceReference[] services = null;

            lock (serviceRegistry)
            {
                services = serviceRegistry.LookupServiceReferences(clazz, filter);
                if (services == null)
                {
                    return null;
                }
                int removed = 0;
                for (int i = services.Length - 1; i >= 0; i--)
                {
                    ServiceReference reference = (ServiceReference)services[i];
                    string[] classes = reference.GetClasses();
                    if (allservices || context.IsAssignableTo((ServiceReference)services[i]))
                    {
                        if (clazz == null)
                            try
                            { /* test for permission to the classes */
                                //checkGetServicePermission(classes);
                            }
                            catch (SecurityException)
                            {
                                services[i] = null;
                                removed++;
                            }
                    }
                    else
                    {
                        services[i] = null;
                        removed++;
                    }
                }
                if (removed > 0)
                {
                    IServiceReference[] temp = services;
                    services = new ServiceReference[temp.Length - removed];
                    for (int i = temp.Length - 1; i >= 0; i--)
                    {
                        if (temp[i] == null)
                            removed--;
                        else
                            services[i - removed] = temp[i];
                    }
                }

            }
            return services == null || services.Length == 0 ? null : services;
        }

        public int GetNextServiceId()
        {
            int serviceId = this.serviceId;
            serviceId++;
            return serviceId;
        }

        public AppDomain CreateDomain(IBundleContext context)
        {
            string binpatch = Path.GetDirectoryName(context.Bundle.Location);
            AppDomainSetup info = new AppDomainSetup();
            DirectoryInfo dirinfo = new DirectoryInfo(binpatch);
            if (String.Equals(dirinfo.Name.ToLower(), "bin"))
            {
                info.ApplicationBase = dirinfo.Parent.FullName;
            }
            else
            { 
                 info.ApplicationBase = binpatch;
            }
           
            if  (!String.IsNullOrEmpty(AppDomain.CurrentDomain.DynamicDirectory))
            {
                info.ShadowCopyDirectories = Path.Combine(info.ApplicationBase, @"cache");
                info.ShadowCopyFiles = "true";
               info.PrivateBinPath = binpatch;
            }

            string domainName = "Bundle-" + context.Bundle.Id.ToString().PadLeft(3, '0');
            AppDomain domain = AppDomain.CreateDomain(domainName, AppDomain.CurrentDomain.Evidence, info);
            Interlocked.Increment(ref this.bundleAppDomains);
            //Assembly[] asses = domain.GetAssemblies();
            return domain;
        }

        public void UnloadDomain(AppDomain domain)
        {
            if (domain != null)
            {
                AppDomain.Unload(domain);
                Interlocked.Decrement(ref this.bundleAppDomains);
            }
        }
    }
}
