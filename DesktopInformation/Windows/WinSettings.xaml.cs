using DesktopInformation.DataAnalysis;
using FzLib.Control.Dialog;
using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DesktopInformation.Windows
{
    /// <summary>
    /// WinSettings.xaml 的交互逻辑
    /// </summary>
    public partial class WinSettings : Window
    {

        public WinSettings()
        {
            InitializeComponent();
            chkStartup.IsChecked = FzLib.Program.Startup.IsRegistryKeyExist()==FzLib.IO.Shortcut.ShortcutStatus.Exist;
            sldUpdate.Value = Config.Instance.UpdateInterval;
            sldUpdate.TextConvert = p => ((int)p).ToString() + "秒";
        }

        private void BtnSaveClickEventHandler(object sender, RoutedEventArgs e)
        {
            Config.Instance.UpdateInterval = sldUpdate.Value;
            DialogResult = true;
            Close();

        }

        private void chkStartup_Click(object sender, RoutedEventArgs e)
        {
            if(chkStartup.IsChecked.Value && FzLib.Program.Startup.IsRegistryKeyExist()!=FzLib.IO.Shortcut.ShortcutStatus.Exist)
            {
                FzLib.Program.Startup.CreateRegistryKey("startup");
            }
            else
            {
                FzLib.Program.Startup.DeleteRegistryKey();
            }
        }
    }
}
