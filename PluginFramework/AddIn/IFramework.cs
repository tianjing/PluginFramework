using System;
using PluginFramework.Configuration.Plugin;

namespace PluginFramework.AddIn
{
    public interface IFramework
    {
        IBundleRepository Bundles { get; }
        void Close();
        AppDomain CreateDomain(IBundleContext context);
        event FrameworkEventHandler FrameworkEvent;
        int GetNextServiceId();
        IServiceReference[] GetServiceReferences(string clazz, string filterString, IBundleContext context, bool allservices);
        IBundle InstallBundle(string location);
        IBundle InstallBundle(string location, BundleData bd);
        void Launch();
        void Shutdown();
        IBundle StartBundle(int id);
        void StartBundle(IBundle bundle);
        void StopBundle(int id);
        void UninstallBundle(int id);
    }
}
