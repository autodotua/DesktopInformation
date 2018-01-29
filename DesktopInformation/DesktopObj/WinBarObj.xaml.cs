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

namespace DesktopInformation.DesktopObj
{
    /// <summary>
    /// WinBarObj.xaml 的交互逻辑
    /// </summary>
    public partial class WinBarObj : WinPercentageDataTypeObjBase
    {
        public WinBarObj(Binding.ObjListBinding item, Properties.Settings set, Tools.DeviceInfo deviceInfo) :base(item,set,deviceInfo)
        {
            InitializeComponent();
            UpdateDisplay();
        }

        public override void UpdateDisplay()
        {
            bar.Foreground = ToBrush(item.ForegroundColor);
            bar.Background = ToBrush(item.BackgounrdColor);
            bar.BorderBrush = ToBrush(item.BorderColor);
            bar.BorderThickness = new Thickness(item.BorderThickness);
        }

        public override void Update()
        {
            base.Update();
            
            if (item.Animation)
            {
                Tools.Tools.NewDoubleAnimation(bar, ProgressBar.ValueProperty, value / (max - min), 0.5);
            }
            else
            {
                bar.Value = value / (max - min);
            }
        }
        
    }
}
