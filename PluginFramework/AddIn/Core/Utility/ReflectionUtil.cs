using System;
using System.Collections.Generic;
using System.Reflection;
using PluginFramework.AddIn.Reflection;

namespace PluginFramework.AddIn.Utility
{
    public static class ReflectionUtil
    {
        // Methods

        public static string GetAssemblyFilename(Assembly assembly)
        {
            return assembly.CodeBase.Replace("file:///", "").Replace('/', '\\');
        }

        public static string GetAssemblyVersion(Assembly assembly)
        {
            foreach (string part in assembly.FullName.Split(','))
            {
                string trimmed = part.Trim();

                if (trimmed.StartsWith("Version="))
                    return trimmed.Substring(8);
            }

            return "0.0.0.0";
        }

        public static AttributeInfo[] GetCustomAttributes(Assembly assembly, Type attribute)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            HashSet<AttributeInfo> attributes = new HashSet<AttributeInfo>();
            foreach (Type type in assembly.GetExportedTypes())
            {
                foreach (Attribute attr in type.GetCustomAttributes(attribute, true))
                {
                    AttributeInfo attributeInfo = new AttributeInfo(type, attr);
                    attributes.Add(attributeInfo);
                }
            }
            AttributeInfo[] result = new AttributeInfo[attributes.Count];
            attributes.CopyTo(result);
            return result;
        }
    }
}