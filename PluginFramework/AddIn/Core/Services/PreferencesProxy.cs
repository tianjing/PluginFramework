using System.Collections;

namespace PluginFramework.AddIn.Services
{
    public class PreferencesProxy
    {
        Hashtable prefSubscribers;
        Preferences prefs;
        Hashtable currentlyHandling;

        public PreferencesProxy(Preferences prefs)
        {
            prefSubscribers = new Hashtable();
            currentlyHandling = new Hashtable();
            this.prefs = prefs;
            this.enable = true;
        }

        public void Subscribe(string pref, string id, PreferencesChangedHandler handler)
        {
            if (!prefSubscribers.Contains(pref))
                prefSubscribers[pref] = new Hashtable();

            (prefSubscribers[pref] as Hashtable).Add(id, handler);
        }

        public void Unsubscribe(string pref, string id)
        {
            if (!prefSubscribers.Contains(pref))
                return;

            (prefSubscribers[pref] as Hashtable).Remove(id);
        }

        public void Change(string pref, string val, string id)
        {
            if (enable == false)
                return;

            if (currentlyHandling.Contains(pref))
                return;

            if (id != "__Preferences__")
            {
                prefs.SetWithoutNotify(pref, val);
            }

            if (!prefSubscribers.Contains(pref))
                return;

            currentlyHandling.Add(pref, null);

            foreach (DictionaryEntry subscriber in (prefSubscribers[pref] as Hashtable))
                if ((subscriber.Key as string) != id)
                {
                    (subscriber.Value as PreferencesChangedHandler)(prefs);
                }

            currentlyHandling.Remove(pref);
        }

        public void NotifyAll()
        {
            if (enable == false)
                return;

            foreach (DictionaryEntry prefSub in prefSubscribers)
            {
                currentlyHandling.Add(prefSub.Key, null);
                foreach (DictionaryEntry subscriber in (prefSub.Value as Hashtable))
                    (subscriber.Value as PreferencesChangedHandler)(prefs);
                currentlyHandling.Remove(prefSub.Key);
            }
        }

        bool enable;

        ///<summary>
        /// Enable or disable emission of the Changed event
        ///</summary>
        public bool Enable
        {
            get { return enable; }
            set { enable = value; }
        }
    }
}
