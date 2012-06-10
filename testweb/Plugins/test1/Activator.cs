using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PluginFramework.AddIn;
using Log2Service;
namespace test1
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
        public static TestIBll.IMessage Message { get; private set; }

        public static TestIBll.ISayYear SayYear { get; private set; }
        private IServiceRegistration messageReg = null;

        private IServiceRegistration sayyear;
        public override void Start(IBundleContext context)
        {
            TestIBll.IMessage message = new TestBll.MessageBll();
            Message = message;

            TestIBll.ISayYear year = new TestBll.SayYearBll();
            SayYear = year;
            messageReg = context.RegisterService<TestIBll.IMessage>(message, null);
            sayyear = context.RegisterService<TestIBll.ISayYear>(year, null);

            mcontext = context;
        }

        public override void Stop(IBundleContext context)
        {
            if (null != sayyear)
            {
                sayyear.Unregister();
                //context.UngetService(sayyear.);
            }
            if (null != messageReg)
            {
                messageReg.Unregister();
                //context.UngetService(messageReg);
            }
            //messageReg.(context.Bundle.Context.);
            mcontext = context;
        }
    }
}