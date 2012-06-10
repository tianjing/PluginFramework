using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginFramework.AddIn;

namespace PluginFramework.AddIn.Core
{
    public class ServiceRegistry : IServiceRegistry
    {
        /** Published services by class name. Key is a String class name; Value is a ArrayList of ServiceRegistrations */
        protected Dictionary<string, List<IServiceRegistration>> publishedServicesByClass;
        /** All published services. Value is ServiceRegistrations */
        protected List<IServiceRegistration> allPublishedServices;
        /** Published services by BundleContext.  Key is a BundleContext; Value is a ArrayList of ServiceRegistrations*/
        protected Dictionary<IBundleContext, List<IServiceRegistration>> publishedServicesByContext;

        public ServiceRegistry()
        {
            Initialize();
        }

        public void Initialize()
        {
            publishedServicesByClass = new Dictionary<string, List<IServiceRegistration>>();
            allPublishedServices = new List<IServiceRegistration>();
            publishedServicesByContext = new Dictionary<IBundleContext, List<IServiceRegistration>>();
        }

        #region IServiceRegistry Members

        public void PublishService(IBundleContext context, IServiceRegistration serviceRegistration)
        {
            // Add the ServiceRegistration to the list of Services published by BundleContext.
            List<IServiceRegistration> contextServices = null;
            if (publishedServicesByContext.ContainsKey(context))
            {
                contextServices = (List<IServiceRegistration>)publishedServicesByContext[context];
            }
            if (contextServices == null)
            {
                contextServices = new List<IServiceRegistration>();
                publishedServicesByContext.Add(context, contextServices);
            }
            contextServices.Add(serviceRegistration);

            // Add the ServiceRegistration to the list of Services published by Class Name.
            string[] clazzes = ((ServiceRegistration)serviceRegistration).Classes;
            int size = clazzes.Length;

            for (int i = 0; i < size; i++)
            {
                string clazz = clazzes[i];

                List<IServiceRegistration> services = null;
                if (publishedServicesByClass.ContainsKey(clazz))
                {
                    services = (List<IServiceRegistration>)publishedServicesByClass[clazz];
                }

                if (services == null)
                {
                    services = new List<IServiceRegistration>();
                    publishedServicesByClass.Add(clazz, services);
                }

                services.Add(serviceRegistration);
            }

            // Add the ServiceRegistration to the list of all published Services.
            allPublishedServices.Add(serviceRegistration);
        }

        public void UnpublishService(IBundleContext context, IServiceRegistration serviceRegistration)
        {
            // Remove the ServiceRegistration from the list of Services published by BundleContext.
            List<IServiceRegistration> contextServices = (List<IServiceRegistration>)publishedServicesByContext[context];
            if (contextServices != null)
            {
                contextServices.Remove(serviceRegistration);
            }

            // Remove the ServiceRegistration from the list of Services published by Class Name.
            string[] clazzes = ((ServiceRegistration)serviceRegistration).Classes;
            int size = clazzes.Length;

            for (int i = 0; i < size; i++)
            {
                string clazz = clazzes[i];
                List<IServiceRegistration> services = (List<IServiceRegistration>)publishedServicesByClass[clazz];
                services.Remove(serviceRegistration);
            }

            // Remove the ServiceRegistration from the list of all published Services.
            allPublishedServices.Remove(serviceRegistration);
        }

        public void UnpublishServices(IBundleContext context)
        {
            // Get all the Services published by the BundleContext.
            List<IServiceRegistration> serviceRegs = (List<IServiceRegistration>)publishedServicesByContext[context];
            if (serviceRegs != null)
            {
                // Remove this list for the BundleContext
                publishedServicesByContext.Remove(context);
                int size = serviceRegs.Count();
                for (int i = 0; i < size; i++)
                {
                    IServiceRegistration serviceReg = (IServiceRegistration)serviceRegs[i];
                    // Remove each service from the list of all published Services
                    allPublishedServices.Remove(serviceReg);

                    // Remove each service from the list of Services published by Class Name. 
                    string[] clazzes = ((ServiceRegistration)serviceReg).Classes;
                    int numclazzes = clazzes.Length;

                    for (int j = 0; j < numclazzes; j++)
                    {
                        string clazz = clazzes[j];
                        if (publishedServicesByClass.ContainsKey(clazz))
                        {
                            List<IServiceRegistration> services = (List<IServiceRegistration>)publishedServicesByClass[clazz];
                            services.Remove(serviceReg);
                        }
                    }
                }
            }
        }
        public IServiceReference[] LookupServiceReferences(string clazz, IFilter filter)
        {
            int size;
            List<IServiceReference> references = null;
            List<IServiceRegistration> serviceRegs = null;
            if (clazz == null) /* all services */
            {
                serviceRegs = allPublishedServices;
            }
            else
            {
                /* services registered under the class name */
                if (publishedServicesByClass.ContainsKey(clazz))
                {
                    serviceRegs = (List<IServiceRegistration>)publishedServicesByClass[clazz];
                }
            }

            if (serviceRegs == null)
                return (null);

            size = serviceRegs.Count;

            if (size == 0)
                return (null);

            references = new List<IServiceReference>();
            for (int i = 0; i < size; i++)
            {
                IServiceRegistration registration = (IServiceRegistration)serviceRegs[i];

                IServiceReference reference = registration.GetReference();
                if ((filter == null) || filter.Match(reference))
                {
                    references.Add(reference);
                }
            }

            if (references.Count == 0)
            {
                return null;
            }

            return (IServiceReference[])references.ToArray();
        }

        public IServiceReference[] LookupServiceReferences(IBundleContext context)
        {
            int size;
            List<IServiceReference> references;
            List<IServiceRegistration> serviceRegs = (List<IServiceRegistration>)publishedServicesByContext[context];

            if (serviceRegs == null)
            {
                return (null);
            }

            size = serviceRegs.Count;

            if (size == 0)
            {
                return (null);
            }

            references = new List<IServiceReference>();
            for (int i = 0; i < size; i++)
            {
                IServiceRegistration registration = (IServiceRegistration)serviceRegs[i];

                IServiceReference reference = registration.GetReference();
                references.Add(reference);
            }

            if (references.Count == 0)
            {
                return null;
            }

            return (IServiceReference[])references.ToArray();
        }

        #endregion
    }
}
