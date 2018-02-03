﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using static DesktopInformation.Tool.Tools;

namespace DesktopInformation
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationDispatcherUnhandledExceptionEventHandler(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            ShowAlert(e.Exception.Message);
            string logName = "UnhandledException.log";
            if (File.Exists(logName))
            {
                string oldFile = File.ReadAllText(logName);
                File.WriteAllText(logName,
                oldFile
                + Environment.NewLine + Environment.NewLine
                + DateTime.Now.ToString()
                + Environment.NewLine
                + e.Exception.ToString());
            }
            else
            {
                File.WriteAllText(logName,
                  DateTime.Now.ToString()
                  + Environment.NewLine
                   + e.Exception.ToString());
            }
            Current.Shutdown();
            return;
        }
    }
}
