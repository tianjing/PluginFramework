using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PluginFramework.Configuration.Plugin;

namespace PluginFramework.AddIn
{
    /// <summary>
    /// IBundle
    /// </summary>
    public interface IBundle
    {
        /// <summary>
        /// Bundle State
        /// </summary>
        BundleState State { get; }
        /// <summary>
        /// Bundle Data
        /// </summary>
        BundleData DataInfo { get; } 
        /// <summary>
        /// Start Bundle
        /// </summary>
        void Start();
        /// <summary>
        /// Stop Bundle
        /// </summary>
        void Stop();
        /// <summary>
        /// Bundle Update
        /// </summary>
        void Update();
        /// <summary>
        /// Bundle Update with Stream
        /// </summary>
        /// <param name="inputStream"></param>
        void Update(Stream inputStream);
        /// <summary>
        /// Bundle Uninstall
        /// </summary>
        void Uninstall();
        /// <summary>
        /// Bundle GetProperties
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> GetProperties();
        /// <summary>
        /// Bundle Context
        /// </summary>
        IBundleContext Context { get; }
        /// <summary>
        /// Bundle ID
        /// </summary>
        Int32 Id { get; }
        /// <summary>
        /// Bundle`s file path
        /// </summary>
        string Location { get; }
        /// <summary>
        /// Symbolic Name
        /// </summary>
        string SymbolicName { get; }
        /// <summary>
        /// Get Registered Services
        /// </summary>
        /// <returns></returns>
        IServiceReference[] GetRegisteredServices();
        /// <summary>
        /// Get Services In Use
        /// </summary>
        /// <returns></returns>
        IServiceReference[] GetServicesInUse();

        //bool HasPermission(object permission);
        /// <summary>
        /// Get Resource
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        Uri GetResource(string name);
        /// <summary>
        /// Get Properties
        /// </summary>
        /// <param name="locale">文件</param>
        /// <returns></returns>
        Dictionary<string, object> GetProperties(string locale);
        /// <summary>
        /// Load Class by Name
        /// </summary>
        /// <param name="name">Type name</param>
        /// <returns></returns>
        Type LoadClass(string name);
        /// <summary>
        /// Get Resources
        /// </summary>
        /// <param name="name">Resources name</param>
        /// <returns></returns>
        IEnumerator GetResources(string name);
        /// <summary>
        /// Get Entry Paths
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        IEnumerator GetEntryPaths(string name);
        /// <summary>
        /// Get Entry
        /// </summary>
        /// <param name="name">name</param>
        /// <returns></returns>
        IEnumerator GetEntry(string name);
        /// <summary>
        /// Get Last Modified
        /// </summary>
        /// <returns></returns>
        long GetLastModified();
        /// <summary>
        /// Find Entries
        /// </summary>
        /// <param name="path">path</param>
        /// <param name="filePattern">file Pattern</param>
        /// <param name="recurse">recurse</param>
        /// <returns></returns>
        IEnumerator FindEntries(string path, string filePattern,
			bool recurse);
        string BundleDynamicDirectory { get; }
        string BundleDynamicFile { get ;  }
    }
}
