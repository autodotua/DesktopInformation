using DesktopInformation.DataAnalysis;
using DesktopInformation.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using static DesktopInformation.Enums.ObjType;

namespace DesktopInformation.DesktopObj
{
    public class WinObjManager
    {
        DispatcherTimer timer = new DispatcherTimer();
        DataManager dataManager;

        public WinObjManager()
        {
            dataManager = new DataManager();
        }


        public async Task Load()
        {
            await RefreshWindows();
            UpdateAll();
            timer.Interval = TimeSpan.FromSeconds(Config.Instance.UpdateInterval);
            timer.Tick += (p1, p2) =>
            {
                if (dataManager.Update())
                {
                    UpdateAll();
                }
            };
            timer.Start();

        }
        private bool isBusy = false;
        public void UpdateAll()
        {
            if (isBusy)
            {
                return;
            }
            foreach (var i in wins)
            {
                if (i.Value.Status == Enums.Status.Running && i.Value.Type != PlainText)
                {
                    try
                    {
                        i.Key.Update();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }


        public void Adjust(ObjInfo item)
        {
            isBusy = true;
            WinObjBase win = wins.First(p => p.Value == item).Key;
            var adjusting = win.Adjuesting;
            RemoveWindow(win);
            AddWindow(item, !adjusting);
            isBusy = false;
        }

        public void Adjust()
        {
            foreach (var item in wins.Values.ToArray())
            {
                Adjust(item);
            }

        }

        public void RefreshWindow(ObjInfo item)
        {
            isBusy = true;
            RemoveWindow(item);
            AddWindow(item, false);
            isBusy = false;
        }
        public async Task RefreshWindows()
        {
            isBusy = true;
            await dataManager.Load();
            TextDataAnalysis.LoadRegex();
            foreach (var win in wins)
            {
                win.Key.Close();
            }
            foreach (var i in Config.Instance.Objs)
            {
                if (i.Status != Enums.Status.Stoped)
                {
                    AddWindow(i, false);
                    await Task.Delay(200);
                }
            }
            isBusy = false;
        }
        Dictionary<WinObjBase, ObjInfo> wins = new Dictionary<WinObjBase, ObjInfo>();



        public Window AddWindow(ObjInfo item, bool adjust)
        {
            WinObjBase win = null;
            switch (item.Type)
            {
                case Text:
                    win = new WinTextObj(item, dataManager, adjust);
                    break;
                case PlainText:
                    win = new WinPlainTextObj(item, adjust);
                    break;
                case Bar:
                    win = new WinBarObj(item, dataManager, adjust);
                    break;
                case Pie:
                    win = new WinCricleBarObj(item, dataManager, adjust);
                    break;
            }
            if (item.Status != Enums.Status.Stoped)
            {
                win.Load();
                wins.Add(win, item);
                win.Show();
            }
            return win;
        }

        public void SetStatue(ObjInfo item, Enums.Status statue)
        {
            if (wins.ContainsValue(item) && statue == Enums.Status.Stoped)
            {
                RemoveWindow(item);
            }
            else if ((!wins.ContainsValue(item)) && statue != Enums.Status.Stoped)
            {
                AddWindow(item, false);
            }
            item.Status = statue;
        }

        public void RemoveWindow(ObjInfo item)
        {
            if (wins.ContainsValue(item))
            {
                WinObjBase win = wins.First(p => p.Value == item).Key;
                RemoveWindow(win);
            }
        }
        public void RemoveWindow(WinObjBase win)
        {
            win.Close();
            wins.Remove(win);
        }

        public void ResetTimerInterval()
        {
            timer.Stop();
            timer.Interval = TimeSpan.FromSeconds(Config.Instance.UpdateInterval);
            timer.Start();

        }

    }
}
