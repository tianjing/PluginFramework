
using System;
using System.Collections.Generic;
using System.Reflection;
using PluginFramework.AddIn;
using PluginFramework.Configuration.Plugin;
using System.IO;
using System.Web.Compilation;
namespace PluginFramework
{
    /// <summary>
    /// Author ：TG
    /// </summary>
    internal class PluginLoad
    {
        #region private member
        private static PluginFramework.AddIn.IFramework mFramework { get { return PluginFramework.PluginManage.Current.Framework; } }

        static IList<Assembly> mTopLevelReferencedAssemblies;
        #endregion
        /// <summary>
        /// Get Bundle Datas
        /// </summary>
        /// <param name="bunldeCache">Bundle Cache</param>
        internal static void GetBundleDatas(Dictionary<BundleData, List<Assembly>> bunldeCache)
        {

            List<BundleData> dirs = PluginFramework.Configuration.PluginXmlProcess.GetBundleDatas();
            //安装所有组件
            if (dirs.Count > 0)
            {
                for (int index = 0; index < dirs.Count; index++)
                {
                    List<Assembly> lass = new List<Assembly>();

                    bunldeCache.Add(dirs[index], lass);
                }

            }
        }

        #region 安装组件
        /// <summary>
        /// Install All Bundles By BundleData
        /// </summary>
        /// <param name="bunldeCache">bunldeCache</param>
        internal static void InstallAllBundlesByBundleData(Dictionary<BundleData, List<Assembly>> bunldeCache)
        {
            foreach (KeyValuePair<BundleData, List<Assembly>> value in bunldeCache)
            {
                LoadBundleForBundleData(value.Key);
            }
        }

        /// <summary>
        /// load Bundle by BundleData info
        /// </summary>
        /// <param name="bd">BundleData info </param>
        private static void LoadBundleForBundleData(PluginFramework.Configuration.Plugin.BundleData bd)
        {
            if (null != bd.Runtime.Assemblie)
            {
                string location = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, bd.Runtime.Assemblie.AssemblyPatch);
                //Install Bundle
                mFramework.InstallBundle(location, bd);

            }
        }
        #endregion
        #region 启动组件
        //加载程序集
        /// <summary>
        /// load Assembly For Location
        /// </summary>
        /// <param name="location">Assembly file full path</param>
        /// <returns></returns>
        private static Assembly GetAssemblyForLocation(string location)
        {
            Assembly ass = Assembly.LoadFrom(PluginFramework.AddIn.Utility.FileHelper.FileCopyToDynamicDirectory(location));
            return ass;
        }
        /// <summary>
        /// Start All Bundles
        /// </summary>
        internal static void StartAllBundles()
        {
            IBundle[] bundles = mFramework.Bundles.GetBundles();
            foreach (IBundle bundle in bundles)
            {
                if (bundle.State == BundleState.Starting || bundle.State == BundleState.Active)
                {
                    continue;
                }
                //check bundle`s Dependencies and state
                if (null == bundle.DataInfo.Runtime || null == bundle.DataInfo.Runtime.Dependencies || bundle.DataInfo.Runtime.Dependencies.Count < 1)
                {
                    StartBundle(bundle);
                }
                else
                {
                    for (int i = 0; i < bundle.DataInfo.Runtime.Dependencies.Count; i++)
                    {
                        StartBundleByDependencie(bundle.DataInfo.Runtime.Dependencies[i]);
                    }
                    StartBundle(bundle);
                }
            }
        }
        /// <summary>
        /// Start Dependencie Bundles(note:Recursive)
        /// </summary>
        /// <param name="depenData">DependencyData</param>
        private static void StartBundleByDependencie(PluginFramework.Configuration.Plugin.DependencyData depenData)
        {
            IBundle[] bundles = mFramework.Bundles.GetBundles(depenData.BundleSymbolicName);
            if (null != bundles && bundles.Length > 0)
            {
                for (int i = 0; i < bundles.Length; i++)
                {
                    if (!(bundles[i].State == BundleState.Starting || bundles[i].State == BundleState.Active))
                    {
                        if (null != bundles[i].DataInfo.Runtime.Dependencies && bundles[i].DataInfo.Runtime.Dependencies.Count > 0)
                        {
                            for (int index = 0; index < bundles[i].DataInfo.Runtime.Dependencies.Count; index++)
                            {
                                StartBundleByDependencie(bundles[i].DataInfo.Runtime.Dependencies[index]);
                            }
                        }
                        else
                        {
                            StartBundle(bundles[i]);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Start Bundle and add Assembly to TopLevelReferencedAssemblies
        /// (note: web application need Assembly to TopLevelReferencedAssemblies)
        /// </summary>
        /// <param name="bundle">bundle</param>
        private static void StartBundle(IBundle bundle)
        {
            Assembly ass = null;
            //加载组件程序集
            ass = GetAssemblyForLocation(bundle.Location);
            //启动组件
            mFramework.StartBundle(bundle);
            //将web项目的dll加入到动态编译区
            if (null != mTopLevelReferencedAssemblies &&
                bundle.DataInfo.Runtime.Assemblie.IsWeb)
            {
                mTopLevelReferencedAssemblies.Add(ass);
            }
        }
        #endregion

        /// <summary>
        /// Get Current web application`s TopLevelReferencedAssemblies
        /// </summary>
        internal static void GetHostAssembly()
        {
            PropertyInfo buildManagerProp = typeof(BuildManager).GetProperty("TheBuildManager",
                BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetProperty);
            if (buildManagerProp != null)
            {
                BuildManager buildManager = buildManagerProp.GetValue(null, null) as BuildManager;
                if (buildManager != null)
                {
                    PropertyInfo toplevelAssembliesProp = typeof(BuildManager).GetProperty(
             "TopLevelReferencedAssemblies", BindingFlags.NonPublic |
                    BindingFlags.Instance | BindingFlags.GetProperty);
                    if (toplevelAssembliesProp != null)
                    {
                        mTopLevelReferencedAssemblies = toplevelAssembliesProp.GetValue(
             buildManager, null) as IList<Assembly>;
                    }
                    else
                    {
                        throw new Exception("toplevelAssemb is null ,maybe is not web app,plase use PluginManage.WinAppLaunch() again!");
                    }

                }
            }
        }
    }
}

