using System;
using System.Collections;
using System.Xml;

namespace PluginFramework.AddIn.Services
{

    public delegate void PreferencesChangedHandler(Preferences prefs);

    ///<summary>
    /// A class that holds the application preferences (using singletons)
    ///</summary>
    public class Preferences
    {
        Hashtable prefs;
        string autoSavePath;

        static Preferences instance = null;
        static Preferences defaultPrefs = null;
        static PreferencesProxy proxy = null;

        ///<summary>
        /// The current preferences
        ///</summary>
        static public Preferences Instance
        {
            get
            {
                if (instance == null)
                    instance = new Preferences();
                return instance;
            }
        }

        ///<summary>
        /// The default preferences
        ///</summary>
        static public Preferences Default
        {
            get
            {
                if (defaultPrefs == null)
                    defaultPrefs = new Preferences();
                return defaultPrefs;
            }
        }

        static public PreferencesProxy Proxy
        {
            get
            {
                if (proxy == null)
                    proxy = new PreferencesProxy(Preferences.Instance);
                return proxy;
            }
        }

        private Preferences()
        {
            prefs = new Hashtable();
            autoSavePath = null;
            notifyWhenSetting = true;
        }

        public T GetValue<T>(string key)
        {
            T s = (T)prefs[key];
            return s;
        }

        public void SetValue<T>(string key, T value)
        {
            prefs[key] = value;
            // save the preferences if autoSavePath is set
            // ignore exceptions
            try
            {
                if (autoSavePath != null)
                    Save(autoSavePath);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }

            if (notifyWhenSetting)
                Preferences.Proxy.Change(key, value.ToString(), "__Preferences__");

        }

        ///<summary>
        /// Get or Set the value of a preference
        ///</summary>
        public string this[string key]
        {
            get
            {
                string s = (string)prefs[key];
                if (s == null)
                    s = string.Empty;
                return s;
            }

            set
            {
                prefs[key] = value;
                // save the preferences if autoSavePath is set
                // ignore exceptions
                try
                {
                    if (autoSavePath != null)
                        Save(autoSavePath);
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }

                if (notifyWhenSetting)
                    Preferences.Proxy.Change(key, value, "__Preferences__");

            }
        }

        public string AutoSavePath
        {
            get { return autoSavePath; }
            set { autoSavePath = value; }
        }

        public IEnumerator GetEnumerator()
        {
            return prefs.GetEnumerator();
        }

        public void SetWithoutNotify(string pref, string val)
        {
            notifyWhenSetting = false;
            this[pref] = val;
            notifyWhenSetting = true;
        }

        ///<summary>
        /// Save preferences to an Xml file
        ///</summary>
        public void Save(string path)
        {
            using (XmlTextWriter xml = new XmlTextWriter(path, null))
            {
                xml.Indentation = 1;
                xml.IndentChar = '\t';

                xml.WriteStartDocument();
                xml.WriteStartElement(null, "preferences", null);

                foreach (DictionaryEntry entry in prefs)
                {
                    xml.WriteStartElement(null, "pref", null);
                    xml.WriteStartAttribute(null, "name", null);
                    xml.WriteString((string)entry.Key);
                    xml.WriteEndAttribute();
                    xml.WriteString((string)entry.Value);
                    xml.WriteEndElement();
                }

                xml.WriteEndElement();
                xml.WriteEndDocument();
                xml.Close();
            }
        }

        ///<summary>
        /// Load preferences from an Xml file
        ///</summary>
        public void Load(string path)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            XmlNodeList prefList = xmlDoc.GetElementsByTagName("pref");

            foreach (XmlNode prefNode in prefList)
            {
                XmlAttributeCollection attrColl = prefNode.Attributes;
                string name = attrColl["name"].Value;
                prefs[name] = prefNode.InnerText;
            }
        }

        ///<summary>
        /// Load preferences from another Preferences instance
        ///</summary>
        public void Load(Preferences p)
        {
            if (p != null)
            {
                foreach (DictionaryEntry entry in p)
                    prefs[entry.Key] = entry.Value;
            }
        }

        ///<summary>
        /// Display preferences
        ///</summary>
        public void Display()
        {
            foreach (DictionaryEntry entry in prefs)
            {
                System.Console.WriteLine("[{0}]: {1}", entry.Key, entry.Value);
            }
        }

        private bool notifyWhenSetting;
    }
} // end namespace