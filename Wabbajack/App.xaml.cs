﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Wabbajack.Common;
using Wabbajack.Common.StoreHandlers;
using Wabbajack.Util;

namespace Wabbajack
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            CLIOld.ParseOptions(Environment.GetCommandLineArgs());
            if (CLIArguments.Help)
                CLIOld.DisplayHelpText();
            var storeHandler = new StoreHandler();
        }
    }
}
