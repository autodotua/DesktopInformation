using DesktopInformation.DesktopObj;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static DesktopInformation.Tools.Tools;
using static DesktopInformation.Properties.Resources;
using static System.Environment;
using static DesktopInformation.Enums;
using DesktopInformation.AddObjWindow;

namespace DesktopInformation.Binding
{
    public class ObjListBindingHelper
    {
        ObservableCollection<ObjListBinding> listBinding;
        WinObjManager manager;
        Properties.Settings set;
        public ObjListBindingHelper(ListView list, WinObjManager manager, Properties.Settings set)
        {
            this.set = set;
            this.manager = manager;
            if (File.Exists(ObjListFileName))
            {
                try
                {
                    listBinding = DeserializeObject(File.ReadAllBytes(ObjListFileName)) as ObservableCollection<ObjListBinding>;
                }
                catch (Exception ex)
                {
                    ShowAlert("保存的对象配置已损坏，将重置。若不希望保存重置的文件，请强退。" + NewLine + ex.ToString());
                    listBinding = new ObservableCollection<ObjListBinding>();
                }
            }
            else
            {
                listBinding = new ObservableCollection<ObjListBinding>();
            }
            list.ItemsSource = listBinding;
            manager.Load(listBinding.ToArray());
        }

        public void AddItem(InfoType type, ObjListBinding item)
        {
            item.Type = type;
            listBinding.Add(item);
        }



        public void OpenEditWindow(InfoType type)
        {
            WinAddObjBase win = null;
            ObjListBinding item = new ObjListBinding(true);
            switch (type)
            {
                case InfoType.Text:
                    win = new WinAddTextObj(item,"请输入含通配符的内容");
                    win.ShowDialog();
                    if (win.DialogResult == true)
                    {
                       AddItem(type, item);
                    }
                    break;
                case InfoType.PlainText:
                    win = new WinAddTextObj(item,"请输入内容");
                    win.ShowDialog();
                    if (win.DialogResult == true)
                    {
                        AddItem(type, item);
                    }
                    break;
                case InfoType.Bar:
                    win = new WinAddPercentageDataTypeObj(item);
                    win.ShowDialog();
                    if (win?.DialogResult == true)
                    {
                       AddItem(type, item);
                    }
                    break;
            }
            if (win?.DialogResult == true)
            {
                manager.AddWindow(item);
            }
        }

        public void OpenEditWindow(ObjListBinding item)
        {
            WinAddObjBase win = null;
            switch (item.Type)
            {
                case InfoType.Text:
                    win = new WinAddTextObj(item, "请输入含通配符的内容");
                    win.ShowDialog();
                    break;
                case InfoType.PlainText:
                    win = new WinAddTextObj(item, "请输入内容");
                    win.ShowDialog();
                    break;
                case InfoType.Bar:
                    win = new WinAddPercentageDataTypeObj(item);
                    win.ShowDialog();
                    break;
            }
            if (win?.DialogResult == true)
            {
                manager.RefreshAll();
            }
        }

        public void RemoveItem(ObjListBinding item)
        {
            listBinding.Remove(item);
        }

        public void SaveConfig()
        {
            try
            {
                set.Save();
                File.WriteAllBytes(ObjListFileName, SerializeObject(listBinding));
            }
            catch (Exception ex)
            {
                ShowAlert("文件保存失败：" + NewLine + ex.ToString());
            }
        }
    }

}
