using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginFramework.AddIn;

namespace PluginFramework.AddIn.Core
{
    public class ServiceReference : IServiceReference
    {
        private ServiceRegistration registration = null;
        private IBundle bundle = null;

        public ServiceRegistration Registration
        {
            get
            {
                return registration;
            }
        }

        public ServiceReference(ServiceRegistration registration, IBundle bundle)
        {
            this.registration = registration;
            this.bundle = bundle;
        }

        #region IServiceReference Members

        public object GetProperty(string key)
        {
            if (registration.Properties != null)
            {
                return registration.Properties[key];
            }
            return null;
        }

        public string[] GetPropertyKeys()
        {
            return registration.Properties.Keys.ToArray<string>();
        }

        public Dictionary<string, object> Properties
        {
            get { return registration.Properties; }
        }

        public IBundle GetBundle()
        {
            return bundle;
        }

        public IBundle[] GetUsingBundles()
        {
            return registration.GetUsingBundles();
        }

        public string[] GetClasses()
        {
            return registration.Classes;
        }

        public bool IsAssignableTo(IBundle requester, string className)
        {
            // Always return true if the requester is the same as the provider.
            // if (requester == bundle)
            // {
            return true;
            // }

            // Boolean flag.
            //  bool allow = true;

            // return allow;
        }

        #endregion
    }
}
