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
            helper = new ObjListBindingHelper(lvw,manager,set);
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            MenuItem menuText = new MenuItem() { Header = "文本" };
            menuText.Click += (p1, p2) => helper.OpenAddWindow(Enums.InfoType.Text);
            ContextMenu menu = new ContextMenu()
            {
                IsOpen = true,
                PlacementTarget=sender as UIElement,
                Items=
                {
                    menuText
                }
            };
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            helper.Dispose();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            manager.Add(new ObjListBinding(Enums.InfoType.Text, "123", "{Year:1.1}年{Month}月{Second:2.2}秒", Enums.Statue.Running));
        }
    }
}
