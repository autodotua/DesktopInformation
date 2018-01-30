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
    public partial class WinPieObj : WinPercentageDataTypeObjBase
    {
        public WinPieObj(Binding.ObjListBinding item, Properties.Settings set, Tools.DeviceInfo deviceInfo) :base(item,set,deviceInfo)
        {
            InitializeComponent();
            //UpdateDisplay();
        }

        public override void UpdateDisplay()
        {
            pie.Foreground =(SolidColorBrush) ToBrush(item.ForegroundColor);
            pie.Background = (SolidColorBrush)ToBrush(item.BackgounrdColor);
            pie.BorderBrush = ToBrush(item.BorderColor);
            pie.BorderThickness = item.BorderThickness;
        }

        public override void Update()
        {
            base.Update();
            
            if (item.Animation)
            {
                pie.AnimationToValue(value / (max - min));
            }
            else
            {
                //pie.Value = value / (max - min);
            }
        }
        
    }
}
