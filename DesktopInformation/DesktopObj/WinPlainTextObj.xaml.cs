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
using DesktopInformation.Tools;

namespace DesktopInformation.DesktopObj
{
    /// <summary>
    /// WinPlainTextObj.xaml 的交互逻辑
    /// </summary>
    public partial class WinPlainTextObj : WinObjBase
    {
        public WinPlainTextObj(Binding.ObjListBinding item, Properties.Settings set) : base(item,set)
        {
            UpdateDisplay();

            InitializeComponent();
        }

        public override DeviceInfo DeviceInfo { get; set; }

        public override void Load()
        {
            tbk.Text = item.Value;
        }
        public override void Update()
        {

        }
        public override void UpdateDisplay()
        {
            tbk.Background = ToBrush(item.BackgounrdColor);
            tbk.Foreground = ToBrush(item.ForegroundColor);
            BorderBrush = ToBrush(item.BorderColor);
            BorderThickness =new Thickness( item.BorderThickness);
        }
    }
}
