using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace PluginFramework.AddIn.Utility
{
    public static class XmlConvertor
    {
        /// <summary>
        /// serialize an object to string.
        /// </summary>
        /// <param name="obj">
        /// the object.
        /// </param>
        /// <returns>
        /// the serialized string.
        /// </returns>
        public static string ObjectToXml(object obj)
        {
            return XmlConvertor.ObjectToXml(obj, false);
        }

        /// <summary>
        /// serialize an object to string.
        /// </summary>
        /// <param name="obj">
        /// the object.
        /// </param>
        /// <param name="toBeIndented">
        /// whether to be indented.
        /// </param>
        public static string ObjectToXml(object obj, bool toBeIndented)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            String content = String.Empty;
            UTF8Encoding encoding = new UTF8Encoding(false);
            XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, encoding))
                {
                    xmlTextWriter.Formatting = toBeIndented ? Formatting.Indented : Formatting.None;
                    xmlSerializer.Serialize(xmlTextWriter, obj);

                    content = encoding.GetString(memoryStream.ToArray());
                }
            }
            return content;
        }

        /// <summary>
        /// deserialize string.to an object.
        /// </summary>
        /// <param name="type">
        /// the type of the object.
        /// </param>
        /// <param name="xml">
        /// the string need to be deserialized.
        /// </param>
        /// <returns>
        /// the deserialized object.
        /// </returns>
        public static object XmlToObject(Type type, string xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException("xml");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            object obj = null;
            XmlSerializer xmlSerializer = new XmlSerializer(type);
            using (StringReader stringReader = new StringReader(xml))
            {
                using (XmlReader xmlReader = new XmlTextReader(stringReader))
                {
                    obj = xmlSerializer.Deserialize(xmlReader);
                }
            }
            return obj;
        }

    }
}
