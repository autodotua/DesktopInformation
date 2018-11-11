using System;
using System.Windows.Media;
using static DesktopInformation.Enums;
using System.Windows;

namespace DesktopInformation.Info
{
    public class ObjInfo
    {
        public ObjInfo() : this(false)
        {

        }
        public ObjInfo(bool initialize)
        {
            if (initialize)
            {
                Status = Status.Running;
                Name = "";
                Value = "";
                BorderColor = Brushes.White;
                BorderThickness = 0;
                Left = SystemParameters.PrimaryScreenWidth - 300;
                Top = 0;
                Width = 300;
                Height = 300;
                Animation = true;
                Absolute = false;
                Orientation = 0;
                Reverse = false;
            }
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
            };
        }

        public ObjType Type { get; set; }
        public Status Status { get; set; }
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
        public string Name { get; set; }
        public string Value { get; set; }
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

        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }



        public SolidColorBrush Foreground { get; set; }
        public SolidColorBrush Backgounrd { get; set; }

        public double BorderThickness { get; set; }
        public SolidColorBrush BorderColor { get; set; }

        public bool Absolute { get; set; }
        public bool Animation { get; set; }

        public int Orientation { get; set; }
        public bool Reverse { get; set; }

        public double InnerR { get; set; }
        public double ShadowBlurRadius { get; set; } = 0;
        public double ShadowDepth { get; set; } = 0;
        public double ShadowDirection { get; set; } = 0;
        public Color ShadowColor { get; set; } = Colors.Transparent;
    }

}
