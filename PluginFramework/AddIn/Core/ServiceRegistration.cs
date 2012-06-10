using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginFramework.AddIn;

namespace PluginFramework.AddIn.Core
{
    public class ServiceRegistration : IServiceRegistration
    {
        // private void 
        private ServiceReference referance = null;
        private BundleContext context = null;
        private IBundle bundle = null;
        private Framework framework = null;
        private string[] classes = null;
        private int serviceId = -1;
        private object serviceObject = null;
        private IServiceFactory factory = null;
        private Dictionary<string, object> properties = null;
        
        private bool available;
        protected RegistrationState state = RegistrationState.Registered;
        protected List<IBundleContext> contextsUsing;
        private object registrationLock = new object();

        public string[] Classes
        {
            get
            {
                return classes;
            }
        }

        public ServiceRegistration(BundleContext context, string[] classes,
            object serviceObject, Dictionary<string, object> properties)
        {
            this.context = context;
            this.bundle = context.Bundle;
            this.framework = context.Framework;
            this.classes = classes;
            this.serviceObject = serviceObject;
            this.serviceId = framework.GetNextServiceId();
            this.referance = new ServiceReference(this, bundle);
            this.contextsUsing = null;
            this.factory = serviceObject as IServiceFactory;
            available = true;
            InitializeProperties(properties);

            framework.ServiceRegistry.PublishService(context, this);
            
            EventManager.OnServiceChanged(new ServiceEventArgs(ServiceState.Registered, referance));
        }

        private void InitializeProperties(Dictionary<string, object> properties)
        {
            if (properties != null)
            {
                this.properties = properties;
            }
            else
            {
                this.properties = new Dictionary<string, object>();
            }
        }

        #region IServiceRegistration Members

        public IServiceReference GetReference()
        {
            if (referance != null)
            {
                return referance;
            }
            else
            {
                throw new NotSupportedException("Service is unregistered");
            }
        }

        public Dictionary<string, object> Properties
        {
            get
            {
                lock (registrationLock)
                {
                    lock (properties)
                    {
                        return properties;
                    }
                }
            }
            set
            {
                lock (registrationLock)
                {
                    lock (properties)
                    {
                        if (available)
                        {
                            properties = value;
                        }
                        else
                        {
                            throw new NotSupportedException("Service is unregistered.");
                        }
                    }

                    EventManager.OnServiceChanged(new ServiceEventArgs(ServiceState.Modified, referance));
                }
            }
        }

        public void Unregister()
        {
            lock (properties)
            {
                if (available)
                {
                
                    if (null != bundle)
                    {
                        //bundle.Framework.Bundles
                        framework.ServiceRegistry.UnpublishService(context, this);

                        EventManager.OnServiceChanged(new ServiceEventArgs(ServiceState.Unregistering, referance));
                    }
                    available = false;
                }
                else
                {
                   throw new NotSupportedException("Service is unregistered");
                }

                contextsUsing = null;

                referance = null;
                context = null;
            }
        }

        #endregion

        public IBundle[] GetUsingBundles()
        {
            lock (registrationLock)
            {
                if (state == RegistrationState.Unregistered) /* service unregistered */
                {
                    return (null);
                }

                if (contextsUsing == null)
                {
                    return (null);
                }

                int size = contextsUsing.Count;
                if (size == 0)
                {
                    return (null);
                }

                /* Copy list of BundleContext into an array of Bundle. */
                IBundle[] bundles = new IBundle[size];
                for (int i = 0; i < size; i++)
                {
                    bundles[i] = ((BundleContext)contextsUsing[i]).Bundle;
                }

                return bundles;
            }
        }

        public object GetService(BundleContext user)
        {
            lock (registrationLock)
            {
                if (state == RegistrationState.Unregistered)
                {
                    return null;
                }

                Dictionary<ServiceReference, ServiceRegistration> servicesInUse = user.ServicesInUse;

                ServiceRegistration serviceRegistration = null;
                if (servicesInUse.ContainsKey(referance))
                {
                    serviceRegistration = servicesInUse[referance];
                }

                if (serviceRegistration == null)
                {
                    serviceRegistration = this;

                    object service = serviceRegistration.serviceObject;

                    if (service != null)
                    {
                        servicesInUse.Add(referance, serviceRegistration);

                        if (contextsUsing == null)
                        {
                            contextsUsing = new List<IBundleContext>();
                        }

                        contextsUsing.Add(user);
                    }

                    return service;
                }
                else
                {
                    object service = null;
                    if (contextsUsing.Contains(user))
                    {
                        if (serviceRegistration.available)
                        {
                            service = serviceRegistration.serviceObject;
                        }
                    }
                    return service;
                }
            }
        }

        public bool UngetService(BundleContext user)
        {
            lock (registrationLock)
            {
                if (state == RegistrationState.Unregistered)
                {
                    return false;
                }

                Dictionary<ServiceReference, ServiceRegistration> servicesInUse = user.ServicesInUse;

                if (servicesInUse != null)
                {
                    ServiceRegistration serviceRegistration = servicesInUse[referance];

                    if (serviceRegistration != null)
                    {
                        serviceRegistration.Unregister();
                        #region 注释原无效注销代码
                        //  if (UngetServiceInternal())
                   //     {
                            /* use count is now zero */
                          //  servicesInUse.Remove(referance);

                            //contextsUsing.Remove(user);
                        //   }
                        #endregion
                        return true;
                    }
                }

                return false;
            }
        }

        public void ReleaseService(BundleContext user)
        {
            lock (registrationLock)
            {
                if (referance == null)
                {
                    return;
                }

                Dictionary<ServiceReference, ServiceRegistration> servicesInUse = user.ServicesInUse;

                if (servicesInUse != null)
                {
                    servicesInUse.Remove(referance);
                    UngetServiceInternal();

                    if (contextsUsing != null)
                    {
                        contextsUsing.Remove(user);
                    }
                }
            }
        }

        private bool UngetServiceInternal()
        {
            factory.UngetService(bundle, this, serviceObject);
            return true;
        }
    }
}
