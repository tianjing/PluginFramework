using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PluginFramework.AddIn.Utility
{
    public class StringTokenizer : IEnumerable<string>
    {
        public const string DefaultDelimiters = " \n\t\r;+=-\"\')(}{][<>";

        private string value;
        private string delimiters;
        private bool returnDelims;
        private string[] splitValues;
        private bool isInitialized;
        private object thisLock = new object();

        public StringTokenizer(string value)
            : this(value, DefaultDelimiters)
        {

        }

        public StringTokenizer(string value, string delim)
            : this(value, delim, true)
        {

        }

        public StringTokenizer(string value, string delim, bool returnDelims)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("owner");
            }

            if (string.IsNullOrEmpty(delim))
            {
                delim = DefaultDelimiters;
            }

            this.value = value;
            this.delimiters = delim;
            this.returnDelims = returnDelims;
            this.isInitialized = false;

            this.InitializeToken();
        }

        private void InitializeToken()
        {
            if (!isInitialized)
            {
                lock (thisLock)
                {
                    if (!isInitialized)
                    {
                        if (string.IsNullOrEmpty(this.value))
                        {
                            throw new NullReferenceException();
                        }
                        char[] delimCharArray = this.delimiters.ToCharArray();
                        splitValues = this.value.Split(delimCharArray);
                    }
                }
            }
        }

        #region IEnumerable<string> Members

        public IEnumerator<string> GetEnumerator()
        {
            foreach (string splitValue in splitValues)
            {
                yield return splitValue;
            }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
