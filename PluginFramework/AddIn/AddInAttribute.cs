using System;

namespace PluginFramework.AddIn
{
    /// <summary>
    /// Attribute to be specified for each Innosys bundle to specify
    /// which class is the bundle activator.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AddInAttribute : Attribute
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        private string publisher;

        public string Publisher
        {
            get { return publisher; }
            set { publisher = value; }
        }
        private string version;

        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        public AddInAttribute(string name)
        {
            this.name = name;
        }
    }
}
