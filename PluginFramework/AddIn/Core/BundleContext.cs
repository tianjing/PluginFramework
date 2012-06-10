using System;
using System.ComponentModel;
using System.IO;
using PluginFramework.AddIn;
using System.Collections.Generic;
using System.Diagnostics;

namespace PluginFramework.AddIn.Core
{
    /// <summary>
    /// IBundleContext implementation.
    /// </summary>
    public class BundleContext : IBundleContext
    {
        #region --- Fields ---
        private Bundle bundle;
        //private DirectoryInfo storage;
        private Framework framework;
        private Dictionary<ServiceReference, ServiceRegistration> servicesInUse;
        #endregion

        #region --- Events ---
        public event BundleEventHandler BundleEvent
        {
            add
            {
                EventManager.BundleEvent += value;
            }
            remove
            {
                EventManager.BundleEvent -= value;
            }
        }

        public event ServiceEventHandler ServiceEvent
        {
            add
            {
                EventManager.ServiceEvent += value;
            }
            remove
            {
                EventManager.ServiceEvent -= value;
            }
        }

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
        #endregion

        #region --- Properties ---
        public IBundle Bundle
        {
            get
            {
                return bundle;
            }
        }

        public IBundle[] Bundles
        {
            get
            {
                Bundle[] bundles = new Bundle[framework.Bundles.Count];
                framework.Bundles.GetBundles().CopyTo(bundles, 0);
                return bundles;
            }
        }

        public Framework Framework
        {
            get
            {
                return framework;
            }
        }

        public Dictionary<ServiceReference, ServiceRegistration> ServicesInUse
        {
            get
            {
                return servicesInUse;
            }
        }
        #endregion

        internal BundleContext(Bundle bundle)
        {
            this.bundle = bundle;
            //this.storage = storage;
            this.framework = bundle.Framework;
            this.servicesInUse = new Dictionary<ServiceReference,ServiceRegistration>();
        }

        public IBundle GetBundle(int id)
        {
            return framework.Bundles.GetBundle(id);
        }

        public FileSystemInfo GetDataFile(string name)
        {
            if (bundle.State == BundleState.Installed)
            {
                throw new InvalidOperationException("The bundle has stopped");
            }
            else
            {
               
                string path =  bundle.Location + bundle.Assembly.Location;
                
                //if (!name.StartsWith(Path.DirectorySeparatorChar.ToString()))
                //{
                //    path += Path.DirectorySeparatorChar;
                //}

                //path += name;

                return new FileInfo(path);
            }
        }

        #region IBundleContext Members

        public string GetProperty(string key)
        {
            throw new NotImplementedException();
        }

        public IBundle Install(string location)
        {
            return framework.InstallBundle(location);
        }

        public IBundle Install(string location, Stream inputStream)
        {
            return framework.InstallBundle(location);
        }

        public IServiceRegistration RegisterService(string[] clazzes, object service,
            Dictionary<string, object> properties)
        {
            if (service == null)
            {
                //if (Debug.DEBUG && Debug.DEBUG_SERVICES) {
                //    Debug.println("Service object is null");
                throw new ArgumentNullException("service", "SERVICE_ARGUMENT_NULL_EXCEPTION");
			}

            int size = clazzes.Length;

            if (size == 0)
            {
                //if (Debug
                throw new ArgumentException("Classes array is empty");
            }

            /* copy the array so that changes to the original will not affect us. */
		    string[] copy = new string[clazzes.Length];
		    // doing this the hard way so we can intern the strings
		    for (int i = clazzes.Length - 1; i >= 0; i--)
            {
			    copy[i] = (string)clazzes[i].Clone();
            }
            clazzes = copy;

            if (!(service is IServiceFactory))
            {
                string invalidService = CheckServiceClass(clazzes, service);
                if (!string.IsNullOrEmpty(invalidService))
                {
                    //if (Debug.DEBUG && Debug.DEBUG_SERVICES) {
                    //    Debug.println("Service object is not an instanceof " + invalidService); //$NON-NLS-1$
                    //}
                    throw new ArgumentException("Service is not instance of class", "invalidService");
                }
            }

            return (CreateServiceRegistration(clazzes, service, properties));
        }

        public IServiceRegistration RegisterService(string clazz, object service,
            Dictionary<string, object> properties)
        {
            String[] clazzes = new String[] {clazz};

            return (RegisterService(clazzes, service, properties));
        }

        public IServiceRegistration RegisterService(Type type,
            object service, Dictionary<string, object> properties)
        {
            return RegisterService(type.FullName, service, properties);
        }

        public IServiceRegistration RegisterService<T>(object service,
            Dictionary<string, object> properties)
        {
            return RegisterService(typeof(T), service, properties);
        }

        public IServiceReference[] GetServiceReferences(string clazz, string filter)
        {
            return framework.GetServiceReferences(clazz, null, this, false);
        }

        public IServiceReference[] GetAllServiceReferences(string clazz, string filter)
        {
            return framework.GetServiceReferences(clazz, filter, this, true);
        }

        public IServiceReference GetServiceReference(string clazz)
        {
            try
            {
                IServiceReference[] references = GetServiceReferences(clazz, null);
                /* if more than one service, select highest ranking */
                if (references != null)
                {
                    int index = 0;
                    int length = references.Length;

                    //if (length > 1)
                  //  {
                        //index = 
                   // }

                    return references[index];
                }
            }
            catch (InvalidSyntaxException)
            {
                throw ;
            }

            return null;
        }

        public object GetService(IServiceReference reference)
        {
            ServiceRegistration registration = ((ServiceReference) reference).Registration;
           
		    return registration.GetService(this);
        }

        public object GetService(string clazz)
        {
            IServiceReference reference = GetServiceReference(clazz);
            if (reference != null)
            {
                return GetService(reference);
            }

            return null;
        }

        public object GetService(Type type)
        {
            return GetService(type.FullName);
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        public bool UngetService(IServiceReference reference)
        {
            ServiceRegistration registration = ((ServiceReference)reference).Registration;

            return registration.UngetService(this);
        }

        public IFilter CreateFilter(string filter)
        {
            return new Filter(filter);
        }

        #endregion

        private IServiceRegistration CreateServiceRegistration(string[] clazzes, object service,
            Dictionary<string, object> properties)
        {
            return (new ServiceRegistration(this, clazzes, service, properties));
        }

        private static string CheckServiceClass(string[] clazzes, object serviceObject)
        {
            Type serviceClass = serviceObject.GetType();

            for (int i = 0; i < clazzes.Length; i++)
            {
                try
                {
                    Type serviceClazz = Type.GetType(clazzes[i]);
                    if (!serviceClass.IsInstanceOfType(serviceObject))
                    {
                        return clazzes[i];
                    }
                }
                catch (TypeLoadException ex)
                {
                    return ex.TypeName;
                }
            }

            return string.Empty;
        }

        public bool IsAssignableTo(IServiceReference reference)
        {
            //if (!scopeEvents)
            //    return true;
            string[] clazzes = reference.GetClasses();
            for (int i = 0; i < clazzes.Length; i++)
                if (!reference.IsAssignableTo(bundle, clazzes[i]))
                    return false;
            return true;
        }
    }
}