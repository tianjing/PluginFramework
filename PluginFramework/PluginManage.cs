
using System;
using System.Collections.Generic;
using PluginFramework.AddIn.Core;
using PluginFramework.AddIn;
using System.Reflection;
using PluginFramework.Configuration.Plugin;
namespace PluginFramework
{
    /// <summary>
    /// Author ：TG
    /// </summary>
    public class PluginManage
    {
        public PluginManage()
        {
            framework = new Framework();
            GetPluginDirs();
            PluginManage.osgi = this;
        }
        #region private Member
        private static List<String> m_PluginDirs = new List<string>();
        private Framework framework;        // The framework instanse
        /// <summary>
        /// Bundle datas
        /// </summary>
        private static Dictionary<BundleData, List<Assembly>> _registeredBunldeCache =
            new Dictionary<BundleData, List<Assembly>>();
        #region --- Singleton ---
        private static PluginManage osgi;
        private static object syncLock = new object();
        #endregion
        #endregion
        

        #region public Member


        /// <summary>
        /// CurrentDomain Plugin Dirs
        /// </summary>
        internal List<String> PluginDirs
        {
            get { return m_PluginDirs; }
        }
        /// <summary>
        /// CurrentDomain Directory
        /// </summary>
        internal String BaseDir
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }
        /// <summary>
        /// Get framework Container
        /// </summary>
        public static PluginManage Current
        {
            get
            {
                lock (syncLock)
                {
                    if (osgi == null)
                    {
                        osgi = new PluginManage();
                    }
                }
                return osgi;
            }
        }
        /// <summary>
        /// Gets the OSGi framework object.
        /// </summary>
        internal IFramework Framework
        {
            get
            {
                return framework;
            }
        }
        #endregion
        private static void GetPluginDirs()
        {
            String path = AppDomain.CurrentDomain.BaseDirectory + "Plugins";
            if (System.IO.Directory.Exists(path))
            m_PluginDirs.AddRange(
                System.IO.Directory.GetDirectories(path
                )
                                    );
        }
        /// <summary>
        /// framework Launch
        /// </summary>
        public void WinAppLaunch()
        {
            framework.Launch();
            //加载配置文件和组件
            PluginLoad.GetBundleDatas(_registeredBunldeCache);
            //根据组件信息 安装 组件
            PluginLoad.InstallAllBundlesByBundleData(_registeredBunldeCache);
            //启动所有已经安装的组件
            PluginLoad.StartAllBundles();
        }
        /// <summary>
        /// framework Launch
        /// </summary>
        internal void WebAppLaunch()
        {
            PluginLoad.GetHostAssembly();
            WinAppLaunch();
        }
        /// <summary>
        ///  framework Close
        /// </summary>
        internal void Close()
        {
            framework.Close();
        }
        /// <summary>
        /// framework Shutdown
        /// </summary>
        internal void Shutdown()
        {
            framework.Shutdown();
        }


    }
}

