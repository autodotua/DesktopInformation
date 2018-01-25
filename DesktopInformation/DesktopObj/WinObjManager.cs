using DesktopInformation.Binding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DesktopInformation.Enums.InfoType;

namespace DesktopInformation.DesktopObj
{
    public class WinObjManager
    {
        Properties.Settings set;
        public void Load(ObjListBinding[] list)
        {
            foreach (var i in list)
            {

            }
        }




        List<WinObjBase> wins = new List<WinObjBase>();

        public WinObjManager(Properties.Settings set)
        {
            this.set = set;
        }

        public void Add(ObjListBinding item)
        {
            switch (item.type)
            {
                case Text:
                    var sameWindows = wins.Where(p => (p.Tag as string).Equals(item.Name)).ToArray();
                    if (sameWindows.Length != 0)
                    {
                        sameWindows[0].Close();
                        wins.Remove(sameWindows[0]);
                    }
                    WinTextObj win = new WinTextObj(set) { Tag = item.Name };
                    win.SetText(item.Value);
                    wins.Add(win);
                    win.Show();

                    break;
            }
        }
    }
}
