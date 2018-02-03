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
using static DesktopInformation.Tool.Tools;

namespace DesktopInformation
{
    /// <summary>
    /// WinSettings.xaml 的交互逻辑
    /// </summary>
    public partial class WinSettings : Window
    {
        Properties.Settings set;
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Startup)+ "\\DesktopInformation.lnk";

        public WinSettings(Properties.Settings set)
        {
            this.set = set;
            InitializeComponent();
            chkStartup.IsChecked = System.IO.File.Exists(path);
            sldUpdate.Value = set.UpdateInterval;
            chkHide.IsChecked = set.AutoHide;
        }

        private void BtnSaveClickEventHandler(object sender, RoutedEventArgs e)
        {
            set.UpdateInterval = (int)sldUpdate.Value;
            set.AutoHide = chkHide.IsChecked.Value;
            if(System.IO.File.Exists(path) && (!chkStartup.IsChecked.Value))
            {
                RemoveStartup();
            }
            else if ((!System.IO.File.Exists(path)) && chkStartup.IsChecked.Value)
            {
                SetStartup();
            }
            DialogResult = true;
            Close();

        }

        private void RemoveStartup()
        {
            try
            {
                System.IO.File.Delete(path);
                System.Windows.Forms.MessageBox.Show("取消自启成功", "提示", System.Windows.Forms.MessageBoxButtons.OK);

            }
            catch (Exception ex)
            {
                ShowAlert("取消自启失败：" + Environment.NewLine + ex.ToString());
            }
        }

        private void SetStartup()
        {
                try
                {

                    WshShell shell = new WshShell();
                    IWshShortcut sc = (IWshShortcut)shell.CreateShortcut(path);
                    sc.TargetPath = Process.GetCurrentProcess().MainModule.FileName;
                    sc.WorkingDirectory = Environment.CurrentDirectory;
                    sc.Save();

                    System.Windows.Forms.MessageBox.Show("设置自启成功", "提示", System.Windows.Forms.MessageBoxButtons.OK);
                }
                catch(Exception ex)
            {
                ShowAlert("设置自启失败" + Environment.NewLine + ex.ToString());
                }
        }

        private void SldUpdateValueChangedEventHandler(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tbkSldValue.Text = string.Format("{0:00}", sldUpdate.Value);
        }
    }
}
