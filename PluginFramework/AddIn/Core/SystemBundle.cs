using System;
using System.Collections.Generic;
using System.IO;
using PluginFramework.Configuration.Plugin;

namespace PluginFramework.AddIn.Core
{
    [Serializable]
    internal sealed class SystemBundle : Bundle
    {
        public SystemBundle(Framework framework)
            : base(new BundleData(0,
                typeof(SystemBundle).Assembly.CodeBase.Replace("file:///",""))
            
            , framework)
        {
            Initialize();
        }

        void Initialize()
        {
            EventManager.BundleEvent += new BundleEventHandler(OnBundleEvent);
        }

        void OnBundleEvent(object sender, BundleEventArgs e)
        {
            if (e.Transition == BundleTransition.Started)
            {
                //Context.RegisterService(typeof(IShell).FullName, Shell.Current, null);
            }
        }

        #region IBundle Members

        public override void Start()
        {
            try
            {
                this.state = BundleState.Starting;

                EventManager.OnBundleChanged(new BundleEventArgs(BundleTransition.Starting, this));

                this.state = BundleState.Active;

                EventManager.OnBundleChanged(new BundleEventArgs(BundleTransition.Started, this));
            }
            catch (Exception ex)
            {
                this.state = BundleState.Installed;
                //TracesProvider.TracesOutput.OutputTrace(ex.Message);
                throw new BundleException(ex.Message, ex);
            }
        }

        public override void Stop()
        {
            try
            {
                this.state = BundleState.Stopping;
                EventManager.OnBundleChanged(new BundleEventArgs(BundleTransition.Stopping, this));

                this.state = BundleState.Installed;
                EventManager.OnBundleChanged(new BundleEventArgs(BundleTransition.Stopped, this));
            }
            catch (Exception ex)
            {
                //TracesProvider.TracesOutput.OutputTrace(ex.Message);
                throw new BundleException(ex.Message, ex);
            }
        }

        public new void Update()
        {
            
        }

        public new void Update(Stream inputStream)
        {
            
        }

        public new void Uninstall()
        {
            
        }

        public new Dictionary<string, object> GetProperties()
        {
            return null;
        }

        #endregion
    }
}
