using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DesktopInformation.Binding;
using DesktopInformation.DesktopObj;
using static DesktopInformation.Tools.Tools;

namespace DesktopInformation
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WinObjList : Window
    {
        private System.Windows.Forms.NotifyIcon notifyIcon = null;
        private ObjListBindingHelper helper;
        private WinObjManager manager;
        private Properties.Settings set;
        public WinObjList()
        {
            InitializeComponent();
            set = new Properties.Settings();
            manager = new WinObjManager(set);
            helper = new ObjListBindingHelper(lvw, manager, set);
            InitialTray();
        }

        private void InitialTray()
        {
            
            notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                BalloonTipText = "设置界面在任务栏托盘",
                Text = "桌面信息员",
                Icon = Properties.Resources.icon,
                Visible = true
            };
            if (!File.Exists(Properties.Resources.ObjListFileName))
            {
                notifyIcon.ShowBalloonTip(2000);
            }
            notifyIcon.MouseClick += (p1, p2) =>
            {
                if (p2.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    Visibility = (Visibility == Visibility.Hidden) ? Visibility.Visible : Visibility.Hidden;
                }
                else if (p2.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    ShowNotifyIconMenu(p1);
                }
            };

        }

        private void ShowNotifyIconMenu(object sender)
        {

            MenuItem menuText = new MenuItem() { Header = "文本" };
            menuText.Click += (p1, p2) => helper.OpenEditWindow(Enums.InfoType.Text);
            MenuItem menuPlainText = new MenuItem() { Header = "纯文本" };
            menuPlainText.Click += (p1, p2) => helper.OpenEditWindow(Enums.InfoType.PlainText);
            MenuItem menuShow = new MenuItem() { Header = "显示所有" };
            menuShow.Click += (p1, p2) =>
            {

            };
            MenuItem menuExit = new MenuItem() { Header = "退出" };
            menuExit.Click += (p1, p2) =>
            {
                if (Visibility == Visibility.Visible)
                {
                    helper.SaveConfig();
                }
                Application.Current.Shutdown();
            };
            ContextMenu menu = new ContextMenu()
            {
                IsOpen = true,
                PlacementTarget = sender as UIElement,
                Items =
                {
                   // menuText,
                   //menuPlainText,
                   menuShow,
                   menuExit
                }
            };
            menu.MouseLeave += (p1, p2) =>
              {
                  menu.IsOpen = false;
              };
        }


        /// <summary>
        /// 单击新增按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddClickEventHandler(object sender, RoutedEventArgs e)
        {

            MenuItem menuText = new MenuItem() { Header = "文本" };
            menuText.Click += (p1, p2) => helper.OpenEditWindow(Enums.InfoType.Text);
            MenuItem menuPlainText = new MenuItem() { Header = "纯文本" };
            menuPlainText.Click += (p1, p2) => helper.OpenEditWindow(Enums.InfoType.PlainText);
            MenuItem menuBar = new MenuItem() { Header = "直条" };
            menuBar.Click += (p1, p2) => helper.OpenEditWindow(Enums.InfoType.Bar);
            MenuItem menuPie = new MenuItem() { Header = "饼图" };
            menuPie.Click += (p1, p2) => helper.OpenEditWindow(Enums.InfoType.Pie);
            ContextMenu menu = new ContextMenu()
            {
                IsOpen = true,
                PlacementTarget = sender as UIElement,
                Items =
                {
                    menuText,
                   menuPlainText,
                   menuBar
                }
            };
        }
        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowClosingEventHandler(object sender, System.ComponentModel.CancelEventArgs e)
        {
            helper.SaveConfig();
            Visibility = Visibility.Hidden;
            e.Cancel = true;
        }
        /// <summary>
        /// 单击编辑按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEditClickEventHandler(object sender, RoutedEventArgs e)
        {

            helper.OpenEditWindow(lvw.SelectedItem as ObjListBinding);
        }
        /// <summary>
        /// 双击列表项事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LvwItemPreviewMouseDoubleClickEventHandler(object sender, MouseButtonEventArgs e)
        {
            BtnEditClickEventHandler(null, null);
        }
        /// <summary>
        /// 单击调整事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAdjustClickEventHandler(object sender, RoutedEventArgs e)
        {
            foreach (var i in lvw.SelectedItems)
            {
                manager.Adjust(i as ObjListBinding);
            }
        }
        /// <summary>
        /// 列表选择项改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LvwSelectionChangedEventHandler(object sender, SelectionChangedEventArgs e)
        {
            btnEdit.IsEnabled = btnAdjust.IsEnabled=btnChangeStatues.IsEnabled = btnDelete.IsEnabled = (lvw.SelectedIndex >= 0);
            if ((lvw.SelectedItem as ObjListBinding)?.Statue == Enums.Statue.Stoped)
            {
                btnEdit.IsEnabled = false;
            }
        }
        /// <summary>
        /// 右键列表项事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LvwItemPreviewMouseRightButtonUpEventHandler(object sender, MouseButtonEventArgs e)
        {
            MenuItem menuAdjust = new MenuItem() { Header = "调整" };
            menuAdjust.Click += (p1, p2) => BtnAdjustClickEventHandler(null, null);
            MenuItem menuEdit = new MenuItem() { Header = "编辑" };
            menuEdit.Click += (p1, p2) => BtnEditClickEventHandler(null, null);
            MenuItem menuDelete = new MenuItem() { Header = "删除" };
            menuEdit.Click += (p1, p2) => BtnDeleteClickEventHandler(null, null);
            ContextMenu menu = new ContextMenu()
            {
                IsOpen = true,
                PlacementTarget = sender as UIElement,
                Items =
                {
                    menuAdjust,
                   menuEdit,
                   menuDelete
                }
            };
        }

        private void BtnDeleteClickEventHandler(object sender, RoutedEventArgs e)
        {
            manager.RemoveWindow(lvw.SelectedItem as ObjListBinding);
            helper.RemoveItem(lvw.SelectedItem as ObjListBinding);
        }

        private void BtnChangeStatuesClickEventHandler(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSettingsClickEventHandler(object sender, RoutedEventArgs e)
        {
            WinSettings win = new WinSettings(set);
            win.ShowDialog();
            if(win.DialogResult.HasValue && win.DialogResult.Value)
            {
                manager.ResetTimerInterval();
            }
        }
    }
}
