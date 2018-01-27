using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static DesktopInformation.Enums;
using static DesktopInformation.Toolx.Tools;
using static DesktopInformation.Properties.Resources;
using static System.Environment;
using System.Windows;
using DesktopInformation.DesktopObj;
using DesktopInformation.AddObjWindow;

namespace DesktopInformation.Binding
{
    [Serializable]
    public class ObjListBinding
    {
        public ObjListBinding()
        {
        }
        public ObjListBinding(InfoType type, string name, string value, Statue statue)
        {
            Type = type;
            Name = name;
            Value = value;
            this.Statue = statue;
        }
        public InfoType Type { get; set; }
        public Statue Statue { get; set; }
        public string ShownType
        {
            get
            {
                switch (Type)
                {
                    case InfoType.Bar:
                        return "直条";
                    case InfoType.Pie:
                        return "饼图";
                    case InfoType.PlainText:
                        return "纯文本";
                    case InfoType.Text:
                        return "文本";
                }
                return null;
            }
        }
        public string Name { get; set; }
        public string Value { get; set; }
        public string ShownStatue
        {
            get
            {
                switch (Statue)
                {
                    case Statue.Pausing:
                        return "暂停中";
                    case Statue.Running:
                        return "运行中";
                    case Statue.Stoped:
                        return "已停止";
                }
                return null;
            }
        }


        public string ForegroundColor{get;set;}

    }





    public class ObjListBindingHelper : IDisposable
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

        public ObjListBinding AddItem(InfoType type, string name, string value)
        {
            ObjListBinding newBinding = new ObjListBinding(type, name, value, Statue.Running);
            listBinding.Add(newBinding);
            return newBinding;
        }
        


        public void OpenEditWindow(InfoType type)
        {
            WinAddObjBase win = null;
            ObjListBinding item = null;
            switch (type)
            {
                case InfoType.Text:
                    win = new WinAddTextObj("请输入含通配符的内容");
                    win.ShowDialog();
                    if (win?.DialogResult == true)
                    {
                        item=AddItem(type,win.ObjName, win.ObjValue);
                    }
                    break;
                case InfoType.PlainText:
                    win = new WinAddTextObj("请输入内容");
                    win.ShowDialog();
                    if (win?.DialogResult == true)
                    {
                        item = AddItem(type, win.ObjName, win.ObjValue);
                    }
                    break;
                case InfoType.Bar:
                    win = new WinAddPercentageDataTypeObj();
                    win.ShowDialog();
                    if (win?.DialogResult == true)
                    {
                        item = AddItem(type, win.ObjName, win.ObjValue);
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
            WinAddTextObj win = null;
            switch (item.Type)
            {
                case InfoType.Text:
                    win = new WinAddTextObj(item.Name,item.Value, "请输入含通配符的内容");
                    win.ShowDialog();
                    if (win?.DialogResult == true)
                    {
                        item.Name = win.ObjName;
                        item.Value = win.ObjValue;
                    }
                    break;
                case InfoType.PlainText:
                    win = new WinAddTextObj(item.Name, item.Value, "请输入内容");
                    win.ShowDialog();
                    if (win?.DialogResult == true)
                    {
                        item.Name = win.ObjName;
                        item.Value = win.ObjValue;
                    }
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

        public void Dispose()
        {
            try
            {
                File.WriteAllBytes(ObjListFileName, SerializeObject(listBinding));
            }
            catch (Exception ex)
            {
                ShowAlert("文件保存失败：" + NewLine + ex.ToString());
            }
        }
    }
}
