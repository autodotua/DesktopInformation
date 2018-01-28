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
using static DesktopInformation.Tools.Tools;

namespace DesktopInformation.AddObjWindow
{
    /// <summary>
    /// WinAddTextObj.xaml 的交互逻辑
    /// </summary>
    public partial class WinAddTextObj : WinAddObjBase
    {
        public WinAddTextObj(Binding.ObjListBinding item, string hintText) : base(item)
        {
            InitializeComponent();
            HintText = hintText;
            txtName.Text = item.Name;
            txtValue.Text = item.Value;
            txtBack.Text = item.BackgounrdColor??"#00000000";
            txtFore.Text = item.ForegroundColor??"#FFFFFFFF";
            txtBorderColor.Text = item.BorderColor;
            txtBorderThickness.Text = item.BorderThickness.ToString();
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
                ShowAlert("请填写名称和内容！");
                return;
            }
            if (!(IsColor(txtBack.Text) && IsColor(txtFore.Text) && IsColor(txtBorderColor.Text)))
            {
                ShowAlert("输入的颜色值有误!");
                return;
            }
            if(!(double.TryParse(txtBorderThickness.Text,out double thickness) && thickness>=0 & thickness<=10))
            {
                ShowAlert("输入的边框粗细应≥0且≤10!");
                return;
            }

            item.BorderColor = txtBorderColor.Text;
            item.BorderThickness = thickness;
            item.Name = txtName.Text;
            item.Value = txtValue.Text;
            item.BackgounrdColor = txtBack.Text;
            item.ForegroundColor = txtFore.Text;
            DialogResult = true;
            Close();
        }

    }
}
