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

namespace DesktopInformation.AddObjWindow
{
    /// <summary>
    /// WinAddTextObj.xaml 的交互逻辑
    /// </summary>
    public partial class WinAddTextObj : AddObjWindowBase
    {
        public WinAddTextObj()
        {
            InitializeComponent();
        }

        public WinAddTextObj(string name,string value)
        {
            InitializeComponent();
            txtName.Text = name;
            txtValue.Text = value;
        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(txtName.Text.Replace(" ","") !="" && txtValue.Text.Replace(" ", "") != "")
            {
                ObjValue = txtValue.Text;
                ObjName = txtName.Text;
                DialogResult = true;
                Close();
            }
            else
            {
                Tool.Tools.ShowAlert("请填写名称和内容！");
            }
        }
    }
}
