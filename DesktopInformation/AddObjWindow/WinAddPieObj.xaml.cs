using DesktopInformation.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class WinAddPieObj : WinAddObjBase
    {
        string[] supportList = DeviceInfo.SupportInfo.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

        public WinAddPieObj(Info.ObjListBInfo item):base(item)
        {
            InitializeComponent();
            txtName.Text = item.Name;
            foreach (var i in supportList)
            {
                cbbValue.Items.Add(i);
            }
            string[] temp = item.Value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            if(temp.Length==3)
            {
                txtMin.Text = temp[0];
                txtValue.Text = temp[1];
                txtMax.Text = temp[2];
            }
            txtBack.Text = item.BackgounrdColor??"#22000000";
            txtFore.Text = item.ForegroundColor??"#EE00FF00";
            txtBorderColor.Text = item.BorderColor;
            txtBorderThickness.Text = item.BorderThickness.ToString();
            chkAnimation.IsChecked = item.Animation;
        }

        private string Check()
        {
            string min = txtMin.Text;
            string value = cbbValue.SelectedItem as string;
            string max = txtMax.Text;
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
            if (!(IsColor(txtBack.Text) && IsColor(txtFore.Text) && IsColor(txtBorderColor.Text)))
            {
                return "输入的颜色值有误!";
            }
            if (!(double.TryParse(txtBorderThickness.Text, out double thickness) && thickness >= 0 & thickness <=10))
            {
                return "输入的边框粗细应≥0且≤10!";
            }
            return null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string check = Check();
            if(check==null)
            {
                item.Name = txtName.Text;
                item.Value = txtMin.Text + "|" + txtValue.Text + "|" + txtMax.Text;
                item.BackgounrdColor = txtBack.Text;
                item.ForegroundColor = txtFore.Text;
                item.BorderColor = txtBorderColor.Text;
                item.BorderThickness = double.Parse(txtBorderThickness.Text);
                item.Animation = chkAnimation.IsChecked.Value;
                DialogResult = true;
                Close();
            }
            else
            {
                Tool.Tools.ShowAlert(check);
            }
        }
    }
}
