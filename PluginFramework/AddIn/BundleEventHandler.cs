﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginFramework.AddIn
{
    /// <summary>
    ///Bundle Event Handler
    /// </summary>
    /// <param name="sender">Bundle object</param>
    /// <param name="e">Bundle Event Args <see cref="BundleEventArgs"/></param>
    public delegate void BundleEventHandler(object sender, BundleEventArgs e);
}
