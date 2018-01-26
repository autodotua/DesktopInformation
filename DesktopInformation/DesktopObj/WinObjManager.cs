using DesktopInformation.Binding;
using DesktopInformation.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using static DesktopInformation.Enums.InfoType;

namespace DesktopInformation.DesktopObj
{
    public class WinObjManager
    {
        DispatcherTimer timer = new DispatcherTimer();
        Properties.Settings set;
        public void Load(ObjListBinding[] list)
        {
            foreach (var i in list)
            {
                Add(i);
            }
            timer.Interval = TimeSpan.FromSeconds(set.UpdateInterval);
            timer.Tick += (p1, p2) =>
            {
                foreach (var i in wins)
                {
                    i.Key.Update();
                }
            };
            timer.Start();
        }

        public void RefreshAll()
        {
            foreach (var i in wins)
            {
                i.Key.Load(i.Value.Value);
            }
        }

        public void Adjust(ObjListBinding item)
        {
            if(wins.ContainsValue(item))
            {
                WinObjBase win = wins.FirstOrDefault(p => p.Value == item).Key;
                win.Adjuest = !win.Adjuest;
            }
        }

        Dictionary<WinObjBase,ObjListBinding> wins = new Dictionary<WinObjBase, ObjListBinding>();

        public WinObjManager(Properties.Settings set)
        {
            this.set = set;
            deviceInfo = new DeviceInfo(set);
        }
        
        public void Add(ObjListBinding item)
        {
            switch (item.type)
            {
                case Text:
                    //var sameWindows = wins.Where(p => (p.Key.Tag as string).Equals(item.Name)).ToDictionary(p=>p.Value);
                    //if (sameWindows.Count != 0)
                    //{
                    //    sameWindows[0].Key.Close();
                    //    wins.Remove(sameWindows[0]);
                    //}
                    WinTextObj win = new WinTextObj(set) { Tag = item.Name, DeviceInfo=deviceInfo};
                    win.Load(item.Value);
                    wins.Add(win,item);
                    win.Show();

                    break;
            }
        }

        DeviceInfo deviceInfo;

    }
}
