using DesktopInformation.Binding;
using DesktopInformation.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using static DesktopInformation.Enums.InfoType;

namespace DesktopInformation.DesktopObj
{
    public class WinObjManager
    {
        DispatcherTimer timer = new DispatcherTimer();
        DataManager dataManager;
        Properties.Settings set;

        public WinObjManager(Properties.Settings set)
        {
            this.set = set;
            dataManager = new DataManager(set, () => RefreshAll());
        }


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
                dataManager.Update();
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
                if (Thread.CurrentThread.ManagedThreadId == i.Key.Dispatcher.Thread.ManagedThreadId)
                {
                    i.Key.Load();
                }
                else
                {
                    i.Key.Dispatcher.Invoke(() => i.Key.Load());
                }
            }
        }

        public void Adjust(ObjListBinding item)
        {
          //  if (wins.ContainsValue(item))
          //  {
                WinObjBase win = wins.FirstOrDefault(p => p.Value == item).Key;
                win.Adjuest = !win.Adjuest;
           // }
        }

        public void Adjust()
        {
            foreach (var i in wins)
            {
                i.Key.Adjuest = !i.Key.Adjuest;
            }
        }

        Dictionary<WinObjBase, ObjListBinding> wins = new Dictionary<WinObjBase, ObjListBinding>();



        public void AddWindow(ObjListBinding item)
        {
            WinObjBase win=null;
            switch (item.Type)
            {
                case Text:
                    win = new WinTextObj(item,set,dataManager);
                    break;
                case PlainText:
                    win = new WinPlainTextObj(item,set);
                    break;
                case Bar:
                    win = new WinBarObj(item,set, dataManager);
                    break;
                case Pie:
                    win = new WinPieObj(item, set, dataManager);
                    break;
            }
            if(item.Statue!=Enums.Statue.Stoped)
            {
                win.Load();
                wins.Add(win, item);
                win.Show();
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
