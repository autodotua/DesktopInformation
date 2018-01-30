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

            Visibility = set.AutoHide ? Visibility.Hidden : Visibility.Visible;


            InitialTray();
        }
        /// <summary>
        /// 初始化托盘
        /// </summary>
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
                    if (Visibility == Visibility.Hidden)
                    {
                        Visibility = Visibility.Visible;
                        Activate();
                    }
                    else
                    {
                        Visibility = Visibility.Hidden;
                    }
                }
                else if (p2.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    ShowNotifyIconMenu(p1);
                }
            };

        }
        /// <summary>
        /// 显示托盘图标菜单
        /// </summary>
        /// <param name="sender"></param>
        private void ShowNotifyIconMenu(object sender)
        {
            MenuItem menuSettings = new MenuItem() { Header = "设置" };
            menuSettings.Click += (p1, p2) => BtnSettingsClickEventHandler(null, null);
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
                   menuSettings,
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
            MenuItem menuClone = new MenuItem() { Header = "克隆选中" };
            menuClone.Click += (p1, p2) =>
              {
                  foreach (var i in lvw.SelectedItems)
                  {
                      helper.Clone(i as ObjListBinding);
                  }
              };
            ContextMenu menu = new ContextMenu()
            {
                IsOpen = true,
                PlacementTarget = sender as UIElement,
                Items =
                {
                    menuText,
                   menuPlainText,
                   menuBar,
                   menuPie,
                }
            };
            if(lvw.SelectedIndex!=-1)
            {
                menu.Items.Add(menuClone);
            }
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
            if (lvw.SelectedIndex != -1)
            {
                foreach (var i in lvw.SelectedItems)
                {
                    manager.Adjust(i as ObjListBinding);
                }
            }
            else
            {
                manager.Adjust();
            }
        }
        /// <summary>
        /// 列表选择项改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LvwSelectionChangedEventHandler(object sender, SelectionChangedEventArgs e)
        {
            btnEdit.IsEnabled  = btnChangeStatues.IsEnabled = btnDelete.IsEnabled = (lvw.SelectedIndex >= 0);
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
        /// <summary>
        /// 单击删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeleteClickEventHandler(object sender, RoutedEventArgs e)
        {
            var selectedItems = lvw.SelectedItems.Cast<ObjListBinding>().ToArray();
            foreach (var i in selectedItems)
            {
                manager.RemoveWindow(i);
                helper.RemoveItem(i);
            }
        }
        /// <summary>
        /// 单击状态事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSetStatuesClickEventHandler(object sender, RoutedEventArgs e)
        {
            IEnumerable<ObjListBinding> items = lvw.SelectedItems.Cast<ObjListBinding>();
            MenuItem menuRunning = new MenuItem() { Header = "运行" };
            menuRunning.Click += (p1, p2) => helper.SetStatues(items,Enums.Statue.Running);
            MenuItem menuPausing= new MenuItem() { Header = "暂停" };
            menuPausing.Click += (p1, p2) => helper.SetStatues(items, Enums.Statue.Pausing);
            MenuItem menuStopped = new MenuItem() { Header = "停止" };
            menuStopped.Click += (p1, p2) => helper.SetStatues(items, Enums.Statue.Stoped);
            ContextMenu menu = new ContextMenu()
            {
                IsOpen = true,
                PlacementTarget = sender as UIElement,
                Items =
                {
                    menuRunning,
                   menuPausing,
                   menuStopped,
                }
            };
        }
        
        /// <summary>
        /// 单击设置按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSettingsClickEventHandler(object sender, RoutedEventArgs e)
        {
            WinSettings win = new WinSettings(set);
            win.ShowDialog();
            if (win.DialogResult.HasValue && win.DialogResult.Value)
            {
                manager.ResetTimerInterval();
            }
        }
        /// <summary>
        /// 在列表上按下键盘事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LvwPreviewKeyDownEventHandler(object sender, KeyEventArgs e)
        {
            if(lvw.SelectedIndex!=-1)
            {
                if(e.Key==Key.Delete)
                {
                    BtnDeleteClickEventHandler(null, null);
                }
                else if(e.Key==Key.Enter)
                {
                    BtnEditClickEventHandler(null, null);
                }
            }
        }
    }
}
