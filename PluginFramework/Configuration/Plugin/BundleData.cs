using System;
using System.Collections.Generic;

namespace PluginFramework.Configuration.Plugin
{
   public class BundleData
    {
        public List<ExtensionPointData> ExtensionPoints
        {            
            get;
            internal set;
        }

        public List<ExtensionData> Extensions
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            set;
        }

        public string SymbolicName
        {
            get;
            set;
        }

        public Version Version
        {
            get;
            set;
        }

        public BundleInfoData BundleInfo
        {
            get;
            set;
        }

        public ActivatorData Activator
        {
            get;
            set;
        }

        public RuntimeData Runtime
        {
            get;
            set;
        }

        public List<ServiceData> Services
        {
            get;
            set;
        }

        public string Path
        {
            get;
            set;
        }

        public int Id{get;set;}

        public string Location { get; set; }

        public BundleData(int id,string location) 
        {
            Id = id;
            Location = location;
        }
        public BundleData()
        {
        }
    }
}
