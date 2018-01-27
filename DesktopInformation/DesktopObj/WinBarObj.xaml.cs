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
        public WinBarObj(Binding.ObjListBinding item, Properties.Settings set, Toolx.DeviceInfo deviceInfo) :base(item,set,deviceInfo)
        {
            this.set = set;
            InitializeComponent();
        }
       
        public override void Update()
        {
            base.Update();
            
            if (set.Animation)
            {
                Toolx.Tools.NewDoubleAnimation(bar, ProgressBar.ValueProperty, value / (max - min), 0.5);
            }
            else
            {
                bar.Value = value / (max - min);
            }
        }
    }
}
