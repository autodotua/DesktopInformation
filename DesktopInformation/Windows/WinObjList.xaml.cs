using DesktopInformation.Info;
using DesktopInformation.DataAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DesktopInformation.Windows
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WinObjList : Window
    {
        private ObjListInfoHelper helper;
        public WinObjList()
        {
            InitializeComponent();

            helper = new ObjListInfoHelper();
            lvw.ItemsSource = Config.Instance.Objs;

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
                    Config.Instance.Save();
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

            //MenuItem menuText = new MenuItem() { Header = "文本" };
            //menuText.Click += (p1, p2) => helper.OpenEditWindow(Enums.InfoType.Text);
            //MenuItem menuPlainText = new MenuItem() { Header = "纯文本" };
            //menuPlainText.Click += (p1, p2) => helper.OpenEditWindow(Enums.InfoType.PlainText);
            //MenuItem menuBar = new MenuItem() { Header = "直条" };
            //menuBar.Click += (p1, p2) => helper.OpenEditWindow(Enums.InfoType.Bar);
            //MenuItem menuPie = new MenuItem() { Header = "饼图" };
            //menuPie.Click += (p1, p2) => helper.OpenEditWindow(Enums.InfoType.Pie);
            //MenuItem menuClone = new MenuItem() { Header = "建立副本" };
            //menuClone.Click += (p1, p2) =>
            //  {
            //      foreach (var i in lvw.SelectedItems)
            //      {
            //          helper.Clone(i as ObjListBInfo);
            //      }
            //  };
            //ContextMenu menu = new ContextMenu()
            //{
            //    IsOpen = true,
            //    PlacementTarget = sender as UIElement,
            //    Items =
            //    {
            //        menuText,
            //       menuPlainText,
            //       menuBar,
            //       menuPie,
            //    }
            //};
            //if(lvw.SelectedIndex!=-1)
            //{
            //    menu.Items.Add(menuClone);
            //}
        }
        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowClosingEventHandler(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Config.Instance.Save();
        }
        /// <summary>
        /// 单击编辑按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEditClickEventHandler(object sender, RoutedEventArgs e)
        {
            if (lvw.SelectedItem != null)
            {
                helper.OpenEditWindow(lvw.SelectedItem as ObjInfo);

            }
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
                    App.Instance.Manager.Adjust(i as ObjInfo);
                }
            }
            else
            {
                App.Instance.Manager.Adjust();
            }
        }
        /// <summary>
        /// 列表选择项改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LvwSelectionChangedEventHandler(object sender, SelectionChangedEventArgs e)
        {
            btnEdit.IsEnabled = btnChangeStatues.IsEnabled = btnDelete.IsEnabled = (lvw.SelectedIndex >= 0);
            if ((lvw.SelectedItem as ObjInfo)?.Status == Enums.Status.Stoped)
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
            var selectedItems = lvw.SelectedItems.Cast<ObjInfo>().ToArray();
            foreach (var i in selectedItems)
            {
                App.Instance.Manager.RemoveWindow(i);
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
            IEnumerable<ObjInfo> items = lvw.SelectedItems.Cast<ObjInfo>();
            MenuItem menuRunning = new MenuItem() { Header = "运行" };
            menuRunning.Click += (p1, p2) => helper.SetStatues(items, Enums.Status.Running);
            MenuItem menuPausing = new MenuItem() { Header = "暂停" };
            menuPausing.Click += (p1, p2) => helper.SetStatues(items, Enums.Status.Pausing);
            MenuItem menuStopped = new MenuItem() { Header = "停止" };
            menuStopped.Click += (p1, p2) => helper.SetStatues(items, Enums.Status.Stoped);
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
            WinSettings win = new WinSettings() { Owner = this };
            win.ShowDialog();
            if (win.DialogResult.HasValue && win.DialogResult.Value)
            {
                App.Instance.Manager.ResetTimerInterval();
            }
        }
        /// <summary>
        /// 在列表上按下键盘事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LvwPreviewKeyDownEventHandler(object sender, KeyEventArgs e)
        {
            if (lvw.SelectedIndex != -1)
            {
                if (e.Key == Key.Delete)
                {
                    BtnDeleteClickEventHandler(null, null);
                }
                else if (e.Key == Key.Enter)
                {
                    BtnEditClickEventHandler(null, null);
                }
            }
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            new WinFind() { Owner = this }.ShowDialog();
        }

        private void ComboBoxItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            switch ((sender as ComboBoxItem).Content as string)
            {
                case "文本":
                    helper.OpenEditWindow(Enums.ObjType.Text);
                    break;
                case "纯文本":
                    helper.OpenEditWindow(Enums.ObjType.PlainText);
                    break;
                case "进度条":
                    helper.OpenEditWindow(Enums.ObjType.Bar);
                    break;
                case "饼图":
                    helper.OpenEditWindow(Enums.ObjType.Pie);
                    break;
                case "建立副本":
                    if (lvw.SelectedIndex != -1)
                    {
                        foreach (var i in lvw.SelectedItems)
                        {
                            helper.Clone(i as ObjInfo);
                        }
                    }
                    break;
            }
            cbbAdd.Text = "";
        }
    }
}
