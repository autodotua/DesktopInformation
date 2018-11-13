using System;
using System.Windows.Media;
using static DesktopInformation.Enums;
using System.Windows;

namespace DesktopInformation.Info
{
    public class ObjInfo
    {
        public ObjInfo() 
        {

        }
     
        public ObjInfo Clone()
        {
            return new ObjInfo()
            {
                Type = Type,
                Status = Status,
                Name = Name.Clone() as string,
                Value = Value.Clone() as string,
                Left = Left,
                Top = Top,
                Width = Width,
                Height = Height,
                Foreground = Foreground.Clone(),
                Backgounrd = Backgounrd.Clone(),
                BorderColor = BorderColor.Clone(),
                BorderThickness = BorderThickness,
                Absolute = Absolute,
                Animation = Animation,
                Orientation = Orientation,
                Reverse = Reverse,
                InnerR=InnerR,
                ShadowBlurRadius=ShadowBlurRadius,
                ShadowColor=ShadowColor,
                ShadowDepth=ShadowDepth,
                ShadowDirection=ShadowDirection,
            };
        }

        public ObjType Type { get; set; } = ObjType.None;
        public Status Status { get; set; } = Status.Running;
        public string TypeText
        {
            get
            {
                switch (Type)
                {
                    case ObjType.Bar:
                        return "直条";
                    case ObjType.Pie:
                        return "饼图";
                    case ObjType.PlainText:
                        return "纯文本";
                    case ObjType.Text:
                        return "文本";
                }
                return null;
            }
        }
        public string Name { get; set; } = "";
        public string Value { get; set; } = "";
        public string StatusText
        {
            get
            {
                switch (Status)
                {
                    case Status.Pausing:
                        return "暂停中";
                    case Status.Running:
                        return "运行中";
                    case Status.Stoped:
                        return "已停止";
                }
                return null;
            }
        }

        public double Left { get; set; }= SystemParameters.PrimaryScreenWidth - 200;
        public double Top { get; set; } = 0;
        public double Width { get; set; } = 200;
        public double Height { get; set; } = 100;



        public SolidColorBrush Foreground { get; set; }
        public SolidColorBrush Backgounrd { get; set; }

        public double BorderThickness { get; set; }
        public SolidColorBrush BorderColor { get; set; } = Brushes.White;

        public bool Absolute { get; set; } = false;
        public bool Animation { get; set; } = true;

        public int Orientation { get; set; } = 0;
        public bool Reverse { get; set; } = false;

        public double InnerR { get; set; } = 0;
        public double ShadowBlurRadius { get; set; } = 0;
        public double ShadowDepth { get; set; } = 0;
        public double ShadowDirection { get; set; } = 0;
        public Color ShadowColor { get; set; } = Colors.Transparent;
    }

}
