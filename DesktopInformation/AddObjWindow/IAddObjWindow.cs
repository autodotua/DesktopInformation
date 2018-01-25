using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopInformation.AddObjWindow
{
    interface IAddObjWindow
    {
         string ObjValue { get;  set; }
        string ObjName { get; set; }
    }
}
