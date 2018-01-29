using DesktopInformation.Binding;
using DesktopInformation.Tools;
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
                if (i.Statue != Enums.Statue.Stoped)
                {
                    AddWindow(i);
                }
            }
            UpdateAll();
            timer.Interval = TimeSpan.FromSeconds(set.UpdateInterval);
            timer.Tick += (p1, p2) =>
            {
                deviceInfo.Update();
                UpdateAll();
            };
            timer.Start();
        }

        public void UpdateAll()
        {
            foreach (var i in wins)
            {
                if (i.Value.Statue == Enums.Statue.Running && i.Value.Type != PlainText)
                {
                    i.Key.Update();
                }
            }
        }

        public void RefreshAll()
        {
            foreach (var i in wins)
            {
                i.Key.Load();
            }
        }

        public void Adjust(ObjListBinding item)
        {
            if (wins.ContainsValue(item))
            {
                WinObjBase win = wins.FirstOrDefault(p => p.Value == item).Key;
                win.Adjuest = !win.Adjuest;
            }
        }

        Dictionary<WinObjBase, ObjListBinding> wins = new Dictionary<WinObjBase, ObjListBinding>();

        public WinObjManager(Properties.Settings set)
        {
            this.set = set;
            deviceInfo = new DeviceInfo(set);
        }

        public void AddWindow(ObjListBinding item)
        {
            WinObjBase win=null;
            switch (item.Type)
            {
                case Text:
                    win = new WinTextObj(item,set, deviceInfo);
                    win.Load();
                    wins.Add(win, item);
                    win.Show();
                    break;
                case PlainText:
                    win = new WinPlainTextObj(item,set);
                    win.Load();
                    wins.Add(win, item);
                    win.Show();
                    break;
                case Bar:
                    win = new WinBarObj(item,set, deviceInfo);
                    win.Load();
                    wins.Add(win, item);
                    win.Show();
                    break;
            }
        }

        public void SetStatue(ObjListBinding item, Enums.Statue statue)
        {
            if (wins.ContainsValue(item) && statue == Enums.Statue.Stoped)
            {
                RemoveWindow(item);
            }
            else if((!wins.ContainsValue(item)) && statue != Enums.Statue.Stoped)
            {
                AddWindow(item);
            }
            item.Statue = statue;
        }

        DeviceInfo deviceInfo;

        public void RemoveWindow(ObjListBinding item)
        {
            if (wins.ContainsValue(item))
            {
                WinObjBase win = wins.First(p => p.Value == item).Key;
                win.Close();
                wins.Remove(win);
            }
        }

        public void ResetTimerInterval()
        {
            timer.Stop();
            timer.Interval = TimeSpan.FromSeconds(set.UpdateInterval);
            timer.Start();

        }

    }
}
