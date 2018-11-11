using DesktopInformation.DataAnalysis;
using FzLib.Control.Dialog;
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
    public partial class WinAddBarObj : WinAddObjBase
    {
        string[] supportList = DeviceInfo.SupportInfo.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

        public WinAddBarObj(Info.ObjInfo item) : base(item)
        {
            InitializeComponent();
            txtName.Text = item.Name;
            foreach (var i in supportList)
            {
                cbbValue.Items.Add(i);
            }
            string[] temp = item.Value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length == 3)
            {
                txtMin.Text = temp[0];
                txtMax.Text = temp[2];
                if (cbbValue.Items.Contains(temp[1]))
                {
                    cbbValue.SelectedItem = temp[1];
                }
                else
                {
                    txtValue.Text = temp[1];
                    chkValue.IsChecked = true;
                }
            }
            txtBack.ColorBrush = item.Backgounrd ?? new SolidColorBrush(Color.FromArgb(0x22,0,0,0));
            txtFore.ColorBrush = item.Foreground ?? new SolidColorBrush(Color.FromArgb(0xEE, 0, 0xFF, 0));
            txtBorderColor.ColorBrush= item.BorderColor;
            txtBorderThickness.Text = item.BorderThickness.ToString();
            chkAnimation.IsChecked = item.Animation;
            cbbOrientation.SelectedIndex = item.Orientation;
            chkReverse.IsChecked = item.Reverse;
        }

        private string Check()
        {
            string min = txtMin.Text;
            string max = txtMax.Text;
            if (chkValue.IsChecked.Value)
            {
                if (txtValue.Text.Replace(" ", "") == "")
                {
                    return "未输入任何值！";
                }
            }
            else
            {
                if (cbbValue.SelectedItem == null)
                {
                    return "未在组合框中选择任何项！";
                }
            }
            if ((!double.TryParse(min, out double dMin)) && (!supportList.Contains(min)))
            {
                return "格式有误！";
            }

            if ((!double.TryParse(max, out double dMax)) && (!supportList.Contains(max)))
            {
                return "格式有误！";
            }
            if (double.TryParse(min, out dMin) && double.TryParse(max, out dMax) && dMin >= dMax)
            {
                return "最小值应小于最大值!";
            }
            //if (!(IsColor(txtBack.Text) && IsColor(txtFore.Text) && IsColor(txtBorderColor.Text)))
            //{
            //    return "输入的颜色值有误!";
            //}
            if (!(double.TryParse(txtBorderThickness.Text, out double thickness) && thickness >= 0 & thickness <= 10))
            {
                return "输入的边框粗细应≥0且≤10!";
            }
            return null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string check = Check();
            if (check == null)
            {
                item.Name = txtName.Text;
                if (chkValue.IsChecked.Value)
                {
                    item.Value = txtMin.Text + "|" + txtValue.Text+ "|" + txtMax.Text;
                }
                else
                {
                    item.Value = txtMin.Text + "|" + cbbValue.SelectedItem as string + "|" + txtMax.Text;
                }
                item.Backgounrd = txtBack.ColorBrush;
                item.Foreground = txtFore.ColorBrush;
                item.BorderColor = txtBorderColor.ColorBrush;
                item.BorderThickness = double.Parse(txtBorderThickness.Text);
                item.Animation = chkAnimation.IsChecked.Value;
                item.Orientation = cbbOrientation.SelectedIndex;
                item.Reverse = chkReverse.IsChecked.Value;
                DialogResult = true;
                Close();
            }
            else
            {
                DialogHelper.ShowError(check);
            }
        }
    }
}
