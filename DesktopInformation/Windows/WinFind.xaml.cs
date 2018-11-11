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
using System.Windows.Shapes;

namespace DesktopInformation.Windows
{
    /// <summary>
    /// WinFind.xaml 的交互逻辑
    /// </summary>
    public partial class WinFind : Window
    {
        public WinFind()
        {
            InitializeComponent();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                txtTick.Text = (sender as DatePicker).SelectedDate.Value.Ticks.ToString();
            }
            catch
            {
                txtTick.Text = "";
            }
        }
    }
}
