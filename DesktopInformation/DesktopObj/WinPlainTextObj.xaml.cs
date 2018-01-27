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
using DesktopInformation.Toolx;

namespace DesktopInformation.DesktopObj
{
    /// <summary>
    /// WinPlainTextObj.xaml 的交互逻辑
    /// </summary>
    public partial class WinPlainTextObj : WinObjBase
    {
        public WinPlainTextObj() : base()
        {
            InitializeComponent();
        }

        public override DeviceInfo DeviceInfo { get; set; }

        public override void Load()
        {
            txt.Text = item.Value;
        }
        public override void Update()
        {

        }
    }
}
