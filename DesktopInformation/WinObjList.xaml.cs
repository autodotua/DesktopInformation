using System;
using System.Collections.Generic;
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
using static DesktopInformation.Toolx.Tools;

namespace DesktopInformation
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WinObjList : Window
    {
        ObjListBindingHelper helper;// = new ObjListBindingHelper();
        WinObjManager manager;
        Properties.Settings set;
        public WinObjList()
        {
            InitializeComponent();
            set = new Properties.Settings();
            manager = new WinObjManager(set);
            helper = new ObjListBindingHelper(lvw, manager, set);

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
            helper.Dispose();
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
            manager.Adjust(lvw.SelectedItem as ObjListBinding);
        }
        /// <summary>
        /// 列表选择项改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LvwSelectionChangedEventHandler(object sender, SelectionChangedEventArgs e)
        {
            btnEdit.IsEnabled = btnAdjust.IsEnabled = btnDelete.IsEnabled = (lvw.SelectedIndex >= 0);
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
    }
}
