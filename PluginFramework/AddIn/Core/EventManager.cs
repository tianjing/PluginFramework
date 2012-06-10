using System;
using System.ComponentModel;
using PluginFramework.AddIn;

namespace PluginFramework.AddIn.Core
{
    /// <summary>
    /// Framework event manager.
    /// </summary>
    internal class EventManager
    {
        #region --- Fields ---
        private static EventHandlerList events;
        private static readonly object bundlekey = new object();
        private static readonly object servicekey = new object();
        private static readonly object frameworkkey = new object();
        #endregion

        #region --- Event management ---
        internal static event BundleEventHandler BundleEvent
        {
            add
            {
                events.AddHandler(bundlekey, value);
            }
            remove
            {
                events.RemoveHandler(bundlekey, value);
            }
        }

        internal static void OnBundleChanged(BundleEventArgs e)
        {
            if(events[bundlekey] != null)
            {
                BundleEventHandler handler = events[bundlekey] as BundleEventHandler;
                handler(null, e);
            }
        }

        internal static event ServiceEventHandler ServiceEvent
        {
            add
            {
                events.AddHandler(servicekey, value);
            }
            remove
            {
                events.RemoveHandler(servicekey, value);
            }
        }

        internal static void OnServiceChanged(ServiceEventArgs e)
        {
            if(events[servicekey] != null)
            {
                ServiceEventHandler handler = events[servicekey] as ServiceEventHandler;
                handler(null, e);
            }
        }

        internal static event FrameworkEventHandler FrameworkEvent
        {
            add
            {
                events.AddHandler(frameworkkey, value);
            }
            remove
            {
                events.RemoveHandler(frameworkkey, value);
            }
        }
        
        internal static void OnFrameworkChanged(FrameworkEventArgs e)
        {
            if (events[frameworkkey] != null)
            {
                FrameworkEventHandler handler = events[frameworkkey] as FrameworkEventHandler;
                handler(null, e);
            }
        }
        #endregion

        static EventManager()
        {
            events = new EventHandlerList();
        }
    }
}
