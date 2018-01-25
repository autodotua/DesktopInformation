using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static DesktopInformation.Enums;
using static DesktopInformation.Tools.Tools;
using static DesktopInformation.Properties.Resources;
using static System.Environment;
using System.Windows;
using DesktopInformation.DesktopObj;

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
            this.type = type;
            Name = name;
            Value = value;
            this.statue = statue;
        }
        public InfoType type;
        public Statue statue;
        public string Type
        {
            get
            {
                switch (type)
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
        public string Statue
        {
            get
            {
                switch (statue)
                {
                    case Enums.Statue.Pausing:
                        return "暂停中";
                    case Enums.Statue.Running:
                        return "运行中";
                    case Enums.Statue.Stoped:
                        return "已停止";
                }
                return null;
            }
        }

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
        }

        public ObjListBinding AddTextObject(string name, string value)
        {
            ObjListBinding newBinding = new ObjListBinding(InfoType.Text, name, value, Statue.Running);
            listBinding.Add(newBinding);
            return newBinding;
        }

        public void OpenAddWindow(InfoType type)
        {
            dynamic win = null;
            ObjListBinding item = null;
            switch (type)
            {
                case InfoType.Text:
                    win = new AddObjWindow.WinAddTextObj();
                    win.ShowDialog();
                    if (win?.DialogResult == true)
                    {
                        item=AddTextObject(win.ObjName, win.ObjValue);
                    }
                    break;
            }
            if (win?.DialogResult == true)
            {
                manager.Add(item);
            }
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
