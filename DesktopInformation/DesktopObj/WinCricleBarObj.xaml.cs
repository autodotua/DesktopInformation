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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DesktopInformation.DesktopObj
{
    /// <summary>
    /// WinBarObj.xaml 的交互逻辑
    /// </summary>
    public partial class WinCricleBarObj : WinPercentageDataTypeObjBase
    {
        public WinCricleBarObj(Info.ObjInfo item, DataAnalysis.DataManager dataManager, bool adjust) :base(item,dataManager,  adjust)
        {
            InitializeComponent();
            bar.RenderTransformOrigin = new Point(0.5, 0.5);

        }

        public override void UpdateDisplay()
        {
            bar.Foreground = Item.Foreground;
            bar.Background = Item.Backgounrd;
            bar.BorderBrush = Item.BorderColor;
            bar.InnerR = Item.InnerR;
            bar.BorderThickness = new Thickness(Item.BorderThickness);
            bar.Orientation = Item.Orientation == 0 ? Orientation.Horizontal : Orientation.Vertical;
            if (Item.Reverse)
            {
                bar.RenderTransform = new RotateTransform(180);
            }
            else
            {
                bar.RenderTransform = null;
            }
        }

        public override void Update()
        {
            base.Update();
            
            if (Item.Animation)
            {
              NewDoubleAnimation(bar, ProgressBar.ValueProperty, (value-min) / (max - min), 0.5, 0, null, false, new CubicEase() { EasingMode = EasingMode.EaseInOut });
            }
            else
            {
                bar.Value = value / (max - min);
            }
        }
        
    }
}
