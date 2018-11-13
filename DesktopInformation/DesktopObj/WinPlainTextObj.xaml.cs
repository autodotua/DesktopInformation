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
using DesktopInformation.DataAnalysis;

namespace DesktopInformation.DesktopObj
{
    /// <summary>
    /// WinPlainTextObj.xaml 的交互逻辑
    /// </summary>
    public partial class WinPlainTextObj : WinObjBase
    {
        public WinPlainTextObj(Info.ObjInfo item, bool adjust) : base(item,adjust)
        {
            LoadUI();

            InitializeComponent();
        }
        

        public override void Load()
        {
            tbk.Text = Item.Value;
        }
        public override void Update()
        {

        }
        public override void LoadUI()
        {
            if(tbk==null)
            {
                return;
            }
            tbk.Background = Item.Backgounrd;
            tbk.Foreground = Item.Foreground;
            BorderBrush = Item.BorderColor;
            BorderThickness =new Thickness( Item.BorderThickness);
        }
    }
}
