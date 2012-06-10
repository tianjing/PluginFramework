using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginFramework.AddIn.Core.Metadata;
using System.IO;
using PluginFramework.AddIn.Utility;
using PluginFramework.Configuration.Plugin;
namespace PluginFramework.Configuration
{
    public class PluginXmlProcess
    {
        private static string addinFile = "Plugin.addin";
        /// <summary>
        /// Search *.addin for dirName
        /// </summary>
        /// <returns>错误返回NULL</returns>
        private static ComponentMetadata SearchXml(string dirName)
        {
            string xmlpatch = dirName + "\\" + addinFile;
            if (System.IO.File.Exists(xmlpatch))
            {
                string xml = string.Empty;
                using (StreamReader reader = File.OpenText(xmlpatch))
                {
                    xml = reader.ReadToEnd();
                }
                return (ComponentMetadata)XmlConvertor.XmlToObject(typeof(ComponentMetadata), xml);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get Xml For BundleData
        /// </summary>
        /// <returns></returns>
        public static BundleData GetXmlForBundleData(string dirName)
        {
            BundleData result = new BundleData();
            //获取配置信息
            ComponentMetadata metadata = SearchXml(dirName);
            //将配置信息加入组件信息中
            if (null != metadata)
            {
                result.Name = metadata.Name;
                result.SymbolicName = metadata.Name;
                result.Path = metadata.Path;
                result.Runtime = GetRuntimeDataForRuntimeData(metadata);
            }
            return result;
        }
        /// <summary>
        /// Get Runtime Data For RuntimeData
        /// </summary>
        /// <returns></returns>
        private static RuntimeData GetRuntimeDataForRuntimeData(ComponentMetadata metadata)
        {
            RuntimeData result = new RuntimeData();
            GetDependencyForRuntimeData(result, metadata);
            GetAssemblyDataForRuntimeData(result, metadata);
            return result;
        }
        /// <summary>
        /// Get Dependency For RuntimeData
        /// </summary>
        /// <returns></returns>
        private static void GetDependencyForRuntimeData(RuntimeData runtimeData, ComponentMetadata metadata)
        {
            if (null != metadata.Runtime.Items1 && metadata.Runtime.Items1.Length > 0)
            {
                foreach (Dependency de in metadata.Runtime.Items1)
                {
                    DependencyData dd = new DependencyData();
                    dd.AssemblyName = de.AssemblyName;
                    dd.BundleSymbolicName = de.BundleSymbolicName;
                    runtimeData.AddDependency(dd);
                }
            }
        }
        /// <summary>
        /// Get Assembly Data For RuntimeData
        /// </summary>
        /// <returns></returns>
        private static void GetAssemblyDataForRuntimeData(RuntimeData runtimeData, ComponentMetadata metadata)
        {
            if (null != metadata.Runtime.Items && metadata.Runtime.Items.Length > 0)
            {
                foreach (ImportInfo de in metadata.Runtime.Items)
                {
                    AssemblyData dd = new AssemblyData();
                    dd.AssemblyPatch = de.assembly;
                    dd.IsWeb = de.isweb;
                    runtimeData.SetAssembly(dd);
                }
            }
        }

        /// <summary>
        /// Get Bundle Datas
        /// </summary>
        /// <returns></returns>
        public static List<BundleData> GetBundleDatas()
        {
            List<BundleData> bundledatas = new List<BundleData>();
            List<string> dirs = AddIn.Utility.FileHelper.SearchDir();
            for (int index = 0; index < dirs.Count; index++)
            {
                BundleData bd = PluginFramework.Configuration.PluginXmlProcess.GetXmlForBundleData(dirs[index]);
                if (null != bd)
                {
                    bundledatas.Add(bd);
                }
            }


            return bundledatas;
        }
    }
}
