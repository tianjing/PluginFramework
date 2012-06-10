using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PluginFramework.AddIn
{
    public class FrameworkUtil
    {
        /// <summary>
        /// Framework Util Class Path
        /// </summary>
        private const string FrameworkUtilClassPath = "PluginFramework.AddIn.Framework.FrameworkUtil, PluginFramework.AddIn.Framework";
        /// <summary>
        /// create Filter
        /// </summary>
        private static MethodInfo createFilter;

        static FrameworkUtil()
        {
            Type type = Type.GetType(FrameworkUtilClassPath);
            createFilter = type.GetMethod("CreateFilter",
                    new Type[] { typeof(string) });
        }

        public FrameworkUtil()
        {

        }
        /// <summary>
        /// Create Filter
        /// </summary>
        /// <param name="filter">filter String</param>
        /// <returns></returns>
        public static IFilter CreateFilter(string filter)
        {
            try
            {
                try
                {
                    return (IFilter)createFilter
                        .Invoke(null, new Object[] { filter });
                }
                catch (TargetException e)
                {
                    throw e.InnerException;
                }
            }
            catch (InvalidSyntaxException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
