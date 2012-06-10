using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginFramework.AddIn;

namespace testconsol
{
    [Serializable]
    [AddIn("Activator")]
    public class Activator : AddInBase
    {
        private static IBundleContext mcontext;
        public static IBundleContext Context
        {
            get { return mcontext; }
        }
        public static Log2Service.ITextLog LogServer 
        {
            get 
            {
                if (null != mcontext)
                {
                  return mcontext.GetService<Log2Service.ITextLog>();
                }
            
            return null;
            }
        
        }

        public override void Start(IBundleContext context)
        {
            mcontext = context;
        }

        public override void Stop(IBundleContext context)
        {
            throw new NotImplementedException();
        }
    }
}
