using DesktopInformation.DataAnalysis;
using FzLib.Control.Dialog;
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
    public partial class WinAddTextObj : WinAddObjBase
    {
        string[] supportList = DeviceInfo.SupportInfo.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

        public WinAddTextObj(Info.ObjInfo item, string hintText) : base(item)
        {
            InitializeComponent();
            HintText = hintText;
            txtName.Text = item.Name;
            txtValue.Text = item.Value;
            txtBack.ColorBrush= item.Backgounrd ??Brushes.Transparent;
            txtFore.ColorBrush = item.Foreground ?? Brushes.White;
            txtBorderColor.ColorBrush = item.BorderColor;
            txtBorderThickness.Text = item.BorderThickness.ToString();
            chkAbs.IsChecked = item.Absolute;
            chkAnimation.IsChecked = item.Animation;
            foreach (var support in supportList)
            {
                cbbAdd.Items.Add(support);
            }
        }



        public string HintText
        {
            get => txtValue.HintText;
            set => txtValue.HintText = value;
        }



        private void BtnOkClickEventHandler(object sender, RoutedEventArgs e)
        {
            if (txtName.Text.Replace(" ", "") == "" || txtValue.Text.Replace(" ", "") == "")
            {
                DialogBox.ShowError("请填写名称和内容！");
                return;
            }
            //if (!(IsColor(txtBack.Text) && IsColor(txtFore.Text) && IsColor(txtBorderColor.Text)))
            //{
            //    ShowAlert("输入的颜色值有误!");
            //    return;
            //}
            if (!(double.TryParse(txtBorderThickness.Text, out double thickness) && thickness >= 0 & thickness <= 10))
            {
                DialogBox.ShowError("输入的边框粗细应≥0且≤10!");
                return;
            }

            item.BorderColor = txtBorderColor.ColorBrush;
            item.BorderThickness = thickness;
            item.Name = txtName.Text;
            item.Value = txtValue.Text;
            item.Backgounrd = txtBack.ColorBrush;
            item.Foreground = txtFore.ColorBrush;
            item.Absolute = chkAbs.IsChecked.Value;
            item.Animation = chkAnimation.IsChecked.Value;
            DialogResult = true;
            Close();
        }

        private void cbbAdd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbbAdd.SelectedIndex == 0)
            {
                return;
            }

            txtValue.SelectedText = "{"+cbbAdd.SelectedItem as string+"}";

            cbbAdd.SelectedIndex = 0;
        }
    }
}
