using System.Collections.Generic;
using static DesktopInformation.Enums;
using DesktopInformation.AddObjWindow;
using System.Windows.Data;
using DesktopInformation.DataAnalysis;

namespace DesktopInformation.Info
{
    public class ObjListInfoHelper
    {


        private void AddItemToList(ObjType type, ObjInfo item)
        {
            item.Type = type;
            Config.Instance.Objs.Add(item);
        }
        
        public void OpenEditWindow(ObjType type)
        {
            WinAddObjBase win = null;
            ObjInfo item = new ObjInfo(true);
            switch (type)
            {
                case ObjType.Text:
                    win = new WinAddTextObj(item,"请输入含通配符的内容");
                    break;
                case ObjType.PlainText:
                    win = new WinAddTextObj(item,"请输入内容");
                    break;
                case ObjType.Bar:
                    win = new WinAddBarObj(item);
                    break;
               case ObjType.Pie:
                   win = new WinAddBarObj(item);
                   break;
            }
            win.Owner = App.Current.MainWindow ;
            win.ShowDialog();

            if (win?.DialogResult == true)
            {
                AddItemToList(type, item);
                App.Instance.Manager.AddWindow(item,false);
            }
        }

        public void OpenEditWindow(ObjInfo item)
        {
            WinAddObjBase win = null;
            switch (item.Type)
            {
                case ObjType.Text:
                    win = new WinAddTextObj(item, "请输入含通配符的内容");
                    break;
                case ObjType.PlainText:
                    win = new WinAddTextObj(item, "请输入内容");
                    break;
                case ObjType.Bar:
                    win = new WinAddBarObj(item);
                    break;
                case ObjType.Pie:
                    win = new WinAddBarObj(item);
                    break;
            }
            win.Owner = App.Current.MainWindow;

            win.ShowDialog();

            if (win?.DialogResult == true)
            {
                App.Instance.Manager.RefreshWindow(item);
                CollectionViewSource.GetDefaultView(Config.Instance.Objs).Refresh();//刷新列表
            }
        }

        public void Clone(ObjInfo item)
        {
            ObjInfo newItem = item.Clone();
            Config.Instance.Objs.Add(newItem);
            App.Instance.Manager.AddWindow(newItem,false);
        }

        public void RemoveItem(ObjInfo item)
        {
            Config.Instance.Objs.Remove(item);
        }

        public void SetStatues(IEnumerable<ObjInfo> items, Status statue)
        {
            foreach (var i in items)
            {
                i.Status = statue;
                App.Instance.Manager.SetStatue(i, statue);
            }
            CollectionViewSource.GetDefaultView(Config.Instance.Objs).Refresh();
        }

 
    }

}
