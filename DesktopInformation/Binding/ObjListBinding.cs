using System;
using static DesktopInformation.Enums;
using static DesktopInformation.Tools.Tools;

namespace DesktopInformation.Binding
{
    [Serializable]
    public class ObjListBinding
    {
        public ObjListBinding(bool initialize=false)
        {
            if(initialize)
            {
                Statue = Statue.Running;
                Name = "";
                Value = "";
                BorderColor = "#FFFFFFFF";
                BorderThickness = 0;
                Left = ScreenWidth - 300;
                Top = 0;
                Width = 300;
                Height = 300;
                Animation = true;
                ForcedAbsolute = false;
            }
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

        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }


        
        public string ForegroundColor { get; set; }
        public string BackgounrdColor { get; set; }

        public double BorderThickness { get; set; }
        public string BorderColor { get; set; }
        
        public bool ForcedAbsolute { get; set; } 
        public bool Animation { get; set; }
    }
    
}
