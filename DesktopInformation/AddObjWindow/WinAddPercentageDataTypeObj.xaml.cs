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
    /// WinAddPercentageDataTypeObj.xaml 的交互逻辑
    /// </summary>
    public partial class WinAddPercentageDataTypeObj : WinAddObjBase
    {
        public WinAddPercentageDataTypeObj()
        {
            InitializeComponent();
        }

        private string Check()
        {
            string min = txtMin.Text;
            string value = txtValue.Text;
            string max = txtMax.Text;
            string[] supportList = Toolx.DeviceInfo.supportInfo.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            if ((!double.TryParse(min, out double dMin)) && (!supportList.Contains(min)))
            {
                return "格式有误！";
            }
            if ((!double.TryParse(value, out double dValue)) && (!supportList.Contains(value)))
            {
                return "格式有误！";
            }
            if ((!double.TryParse(max, out double dMax)) && (!supportList.Contains(max)))
            {
                return "格式有误！";
            }
            if(double.TryParse(min, out dMin) && double.TryParse(max, out  dMax) && dMin>=dMax)
            {
                return "最小值应小于最大值!";
            }
            return null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string check = Check();
            if(check==null)
            {
                ObjName = txtName.Text;
                ObjValue = txtMin.Text + "|" + txtValue.Text + "|" + txtMax.Text;
                DialogResult = true;
                Close();
            }
            else
            {
                Toolx.Tools.ShowAlert(check);
            }
        }
    }
}
