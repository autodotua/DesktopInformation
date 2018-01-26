using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopInformation.AddObjWindow
{
    public abstract class AddObjWindowBase : Window
    {
        public string ObjValue { get; set; }
        public string ObjName { get; set; }
    }
}
