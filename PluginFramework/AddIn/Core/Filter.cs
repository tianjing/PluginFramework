using System.Collections.Generic;

namespace PluginFramework.AddIn.Core
{
    public class Filter : IFilter
    {
        private string filterString;

        public string FilterString
        {
            get
            {
                return filterString;
            }
        }

        public Filter(string filter)
        {
            this.filterString = filter;
        }

        #region IFilter Members

        public bool Match(IServiceReference reference)
        {
            return true;
        }

        public bool Match(Dictionary<string, object> dictionary)
        {
            return true;
        }

        public bool MatchCase(Dictionary<string, object> dictionary)
        {
            return true;
        }

        #endregion
    }
}
