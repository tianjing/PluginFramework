using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using System.Xml;
using System.Xml.Serialization;
using PluginFramework.AddIn;
using PluginFramework.AddIn.Reflection;
using PluginFramework.AddIn.Services;
using PluginFramework.AddIn.Utility;
using PluginFramework.Configuration.Plugin;

namespace PluginFramework.AddIn.Core
{
    /// <summary>
    /// Summary description for Bundle.
    /// </summary>
    public class Bundle : IBundle
    {
        #region --- Fields ---
        private Framework framework;
        private Configuration.Plugin.BundleData bundleData;
        protected BundleState state;
        private Int32 id;
        private Assembly assembly;
        private AppDomain domain;
        //private IBundleActivator activator;
        private IBundleActivator[] activators;
        private string location;
        private string symbolicName;
        private IBundleContext context;
        private DirectoryInfo storage;
        private string dynamicDirectory;
        private string dynamicFile;

        #endregion

        #region --- Properties ---

        public string BundleDynamicDirectory { get { return dynamicDirectory; } }
        public string BundleDynamicFile { get { return dynamicFile; } }
        public BundleState State
        {
            get
            {
                return state;
            }
        }

        public Int32 Id
        {
            get
            {
                return id;
            }
        }

        public string Location
        {
            get
            {
                return location;
            }
        }

        public Framework Framework
        {
            get
            {
                if (framework == null)
                {
                    throw new NullReferenceException("Framework is null.");
                }
                return framework;
            }
        }
        public IBundleContext Context
        {
            get
            {
                if (context == null)
                {
                    context = new BundleContext(this);
                }

                return context;
            }
        }
        public BundleData DataInfo { get { return bundleData; } }
        private IBundleActivator[] Acitvators
        {
            get
            {
                if (activators == null)
                {
                    // Look for the activator attribute
                    AttributeInfo[] attributes = ReflectionUtil.GetCustomAttributes(assembly, typeof(AddInAttribute));
                    if (attributes == null || attributes.Length <= 0)
                    {
                        //TracesProvider.TracesOutput.OutputTrace("No activator found");
                        return null;
                    }
                    else
                    {
                        HashSet<IBundleActivator> activatorSet = new HashSet<IBundleActivator>();
                        foreach (AttributeInfo attribute in attributes)
                        {
                            string typeName = attribute.Owner.FullName;
                            object obj = domain.CreateInstanceAndUnwrap(assembly.FullName, typeName);
                            //todoo 会锁定文件
                            //object obj = domain.CreateInstanceFromAndUnwrap(location, typeName);
                            IBundleActivator proxy = obj as IBundleActivator;
                            //动态代理
                            //IBundleActivator proxy = (IBundleActivator)DynamicProxyFactory.Instance.CreateProxy(obj, new InvocationDelegate(InvocationHandler));
                            if (null != proxy)
                            {
                                activatorSet.Add(proxy);
                            }
                        }
                        this.activators = new IBundleActivator[activatorSet.Count];
                        activatorSet.CopyTo(this.activators);
                    }
                }

                return activators;
            }
        }

        internal Assembly Assembly
        {
            get
            {
                return assembly;
            }
            set
            {
                this.assembly = value;
            }
        }

        public Version Version
        {
            get
            {
                return bundleData.Version;
            }
        }
        #endregion

        internal Bundle(BundleData bundleData, Framework framework)
        {
            this.id = bundleData.Id;
            this.storage = new DirectoryInfo(bundleData.Location);
            this.framework = framework;
            this.bundleData = bundleData;
            this.location = bundleData.Location;
            this.symbolicName = Path.GetFileNameWithoutExtension(bundleData.Location);
            this.state = BundleState.Installed;
            SetDynamicInfo();

        }
        private void SetDynamicInfo()
        {
            if (!String.IsNullOrEmpty(AppDomain.CurrentDomain.DynamicDirectory))
            {
                string ddir = Path.Combine(AppDomain.CurrentDomain.DynamicDirectory, Guid.NewGuid().ToString());
                if (!Directory.Exists(ddir))
                {
                    Directory.CreateDirectory(ddir);
                }
                else
                {
                    ddir = Path.Combine(ddir, Guid.NewGuid().ToString());
                    Directory.CreateDirectory(ddir);
                }
                this.dynamicDirectory = ddir;

                FileInfo fi = new FileInfo(location);
                this.dynamicFile = dynamicDirectory + "\\" + fi.Name;
            }
        }
        private static object InvocationHandler(object target, MethodBase method, object[] parameters)
        {
            Debug.WriteLine("Before: " + method.Name);

            object result = method.Invoke(target, parameters);

            Debug.WriteLine("After: " + method.Name);

            return result;
        }

        public virtual void Start()
        {
            try
            {
                this.state = BundleState.Starting;

                EventManager.OnBundleChanged(new BundleEventArgs(BundleTransition.Starting, this));

                domain = framework.CreateDomain(this.Context);
                Debug.Assert(domain != null, "Bundle AppDomain can't be set to null.");

                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolve);


                assembly = Assembly.LoadFrom(Utility.FileHelper.FileCopyToDynamicDirectory(location));

                Debug.Assert(assembly != null, "Bundle Assembly can's be set to null.");

                foreach (IBundleActivator activator in this.Acitvators)
                {
                    if (activator == null)
                    {
                        throw new BundleException("No activator for: " + this.Location);
                    }

                    activator.Start(this.Context);
                }

                this.state = BundleState.Active;

                EventManager.OnBundleChanged(new BundleEventArgs(BundleTransition.Started, this));
            }
            catch (Exception ex)
            {
                this.state = BundleState.Installed;
                throw new BundleException(ex.Message, ex);
            }
        }

        private string GetAssemblyPatch(string filepatch)
        {
            string result = string.Empty;
            int index = filepatch.LastIndexOf("\\") + 1;
            if (index < 1)
            {
                index = filepatch.LastIndexOf("/") + 1;

            }
            result = filepatch.Substring(0, index);

            return result;
        }


        /// <summary>
        /// Search Assembly By Directory
        /// </summary>
        /// <param name="assemblyName">assemblyName</param>
        /// <param name="directorys">plugin directorys</param>
        /// <returns></returns>
        private static string SearchAssemblyByDirectory(String assemblyName, List<String> directorys)
        {
            List<String> filePaths = new List<string>();
            for (int i = 0; i < directorys.Count; i++)
            {
                String[] paths = Directory.GetFiles(directorys[i],
            assemblyName + ".dll", SearchOption.TopDirectoryOnly);
                if (null != paths)
                {
                    filePaths.AddRange(paths);
                }
                String path=directorys[i] + "\\bin";
                if (Directory.Exists(path))
                {
                    paths = Directory.GetFiles(path,
               assemblyName + ".dll", SearchOption.TopDirectoryOnly);

                    if (null != paths)
                    {
                        filePaths.AddRange(paths);
                    }
                }
                if (filePaths != null && filePaths.Count > 0)
                {
                    return filePaths[0];
                }
            }
            return null;
        }
        //查询程序
        private static string SearchAssembly(string assemblyName, List<String> dirs)
        {
            String assemblyPath = String.Empty;

            assemblyPath = SearchAssemblyByDirectory(assemblyName, dirs);
            return assemblyPath;

        }

        private static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
           List<String> dirs = PluginManage.Current.Framework.Bundles.BundlePaths;

            AssemblyName name = new AssemblyName(args.Name);
            string assemblyFile = SearchAssembly(name.Name, dirs);
            if (!String.IsNullOrEmpty(assemblyFile) && File.Exists(assemblyFile))
            {
                return Assembly.LoadFrom(Utility.FileHelper.FileCopyToDynamicDirectory(assemblyFile));
            }
            return null;

        }

        public virtual void Stop()
        {
            try
            {
                this.state = BundleState.Stopping;
                EventManager.OnBundleChanged(new BundleEventArgs(BundleTransition.Stopping, this));

                foreach (IBundleActivator activator in this.Acitvators)
                {
                    if (activator == null)
                    {
                        throw new Exception("No activator for: " + this.Location);
                    }

                    activator.Stop(this.Context);
                }
                activators = null;
                framework.UnloadDomain(domain);
            }
            catch (Exception ex)
            {
                throw new BundleException(ex.Message, ex);
            }

            this.state = BundleState.Installed;
            EventManager.OnBundleChanged(new BundleEventArgs(BundleTransition.Stopped, this));
        }

        public void Uninstall()
        {
            framework.UninstallBundle(id);
            this.state = BundleState.Uninstalled;
            EventManager.OnBundleChanged(new BundleEventArgs(BundleTransition.Uninstalled, this));
        }

        #region IBundle Members

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void Update(Stream inputStream)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> GetProperties()
        {
            throw new NotImplementedException();
        }

        public IServiceReference[] GetRegisteredServices()
        {
            throw new NotImplementedException();
        }

        public IServiceReference[] GetServicesInUse()
        {
            throw new NotImplementedException();
        }

        public Uri GetResource(string name)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> GetProperties(string locale)
        {
            throw new NotImplementedException();
        }

        public string SymbolicName
        {
            get { return symbolicName; }
        }

        public Type LoadClass(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetResources(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEntryPaths(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEntry(string name)
        {
            throw new NotImplementedException();
        }

        public long GetLastModified()
        {
            throw new NotImplementedException();
        }

        public IEnumerator FindEntries(string path, string filePattern, bool recurse)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
