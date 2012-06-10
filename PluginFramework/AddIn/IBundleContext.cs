using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PluginFramework.AddIn
{
    public interface IBundleContext
    {
        /// <summary>
        /// Event raised when a bundle transition occurs.
        /// </summary>
        event BundleEventHandler BundleEvent;
        /// <summary>
        /// 服务事件
        /// </summary>
        event ServiceEventHandler ServiceEvent;
        /// <summary>
        /// 框架事件
        /// </summary>
        event FrameworkEventHandler FrameworkEvent;
        /// <summary>
        /// 根据key获取属性
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        string GetProperty(string key);
        /// <summary>
        /// 组件对象
        /// </summary>
        IBundle Bundle { get; }
        /// <summary>
        /// 根据组件文件路径安装组件
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        IBundle Install(string location);
        /// <summary>
        /// 根据组件路径及内存信息安装组件
        /// </summary>
        /// <param name="location">文件路径</param>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        IBundle Install(string location, Stream inputStream);
        /// <summary>
        /// 获取组件ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IBundle GetBundle(int id);
        /// <summary>
        /// 组件
        /// </summary>
        IBundle[] Bundles { get; }
        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="clazzes">类 对象 名称集合</param>
        /// <param name="service">对象服务</param>
        /// <param name="properties">对象属性</param>
        /// <returns></returns>
        IServiceRegistration RegisterService(string[] clazzes,
            object service, Dictionary<string, object> properties);
        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="clazz">类 对象名称</param>
        /// <param name="service">服务</param>
        /// <param name="properties">属性</param>
        /// <returns></returns>
        IServiceRegistration RegisterService(string clazz,
            object service, Dictionary<string, object> properties);
        /// <summary>
        ///  注册服务
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="service">服务</param>
        /// <param name="properties">属性</param>
        /// <returns></returns>
        IServiceRegistration RegisterService(Type type,
            object service, Dictionary<string, object> properties);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="service">服务</param>
        /// <param name="properties">属性</param>
        /// <returns></returns>
        IServiceRegistration RegisterService<T>(object service,
            Dictionary<string, object> properties);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        IServiceReference[] GetServiceReferences(string clazz,
            string filter);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        IServiceReference[] GetAllServiceReferences(string clazz,
            string filter);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clazz"></param>
        /// <returns></returns>
        IServiceReference GetServiceReference(string clazz);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        object GetService(IServiceReference reference);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clazz"></param>
        /// <returns></returns>
        object GetService(string clazz);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object GetService(Type type);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetService<T>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        bool UngetService(IServiceReference reference);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        FileSystemInfo GetDataFile(string filename);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IFilter CreateFilter(string filter);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        bool IsAssignableTo(IServiceReference reference);
    }
}
