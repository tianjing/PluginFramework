using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginFramework.AddIn
{
    public class BundleEventArgs : EventArgs
    {
        /// <summary>
        /// 组件对象
        /// </summary>
        private IBundle mbundle;
        /// <summary>
        /// 当前状态
        /// </summary>
        private BundleTransition transition;
        /// <summary>
        /// 组件对象
        /// </summary>
        public IBundle Bundle
        {
            get { return mbundle; }
        }
        /// <summary>
        /// 当前状态
        /// </summary>
        public BundleTransition Transition
        {
            get { return transition; }
        }

        public BundleEventArgs(BundleTransition transition, IBundle bundle)
        {
            this.mbundle = bundle;
            this.transition = transition;
        }
    }
}
