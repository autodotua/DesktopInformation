using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DesktopInformation.AddObjWindow
{
    public class WinAddObjBase:System.Windows.Window
    {
        public Binding.ObjListBinding item;
        public WinAddObjBase(Binding.ObjListBinding item)
        {
            this.item = item;
        }

        protected bool IsColor(string color)
        {
            Regex r = new Regex("^#[0-9A-F]{8}$");
            return r.IsMatch(color);
        }
    }
}
