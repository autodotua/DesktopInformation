using DesktopInformation.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shell;
using System.Windows.Threading;

namespace DesktopInformation.DesktopObj
{


    public abstract class WinObjBase:Window
    {
        protected Properties.Settings set;
        protected Binding.ObjListBinding item;
        public WinObjBase(Binding.ObjListBinding item,Properties.Settings set):base()
        {
            this.set = set;
            this.item = item;

            Width = item.Width;
            Height = item.Height;
            Left = item.Left;
            Top = item.Top;

            WindowStyle = WindowStyle.None;
            ShowInTaskbar = false;
            AllowsTransparency = true;
            Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            BorderBrush = new SolidColorBrush(Colors.DarkGray);
            WindowChrome.SetWindowChrome(this, new WindowChrome()
            {
                CaptionHeight = 0,
                ResizeBorderThickness = new Thickness(4),
            });
            PreviewMouseLeftButtonDown += (p1, p2) => DragMove();
            Loaded += (p1, p2) =>
            {
                SetToStickOnDesktop();
                SetToMouseThrough();
            };
            

        }

        public WinObjBase(Binding.ObjListBinding item, Properties.Settings set,DeviceInfo deviceInfo) : this(item,set)
        {
            DeviceInfo = deviceInfo;
        }



        #region WinAPI
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow([MarshalAs(UnmanagedType.LPTStr)] string lpClassName, [MarshalAs(UnmanagedType.LPTStr)] string lpWindowName);

        [DllImport("user32")]
        private static extern IntPtr FindWindowEx(IntPtr hWnd1, IntPtr hWnd2, string lpsz1, string lpsz2);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        public void SetToStickOnDesktop()
        {
            IntPtr pWnd = FindWindow("Progman", null);
            pWnd = FindWindowEx(pWnd, IntPtr.Zero, "SHELLDLL_DefVIew", null);
            pWnd = FindWindowEx(pWnd, IntPtr.Zero, "SysListView32", null);
            SetParent(new WindowInteropHelper(this).Handle, pWnd);
        }

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);
        int extendedStyle;
        public const int WS_EX_TRANSPARENT = 0x00000020;
        public const int GWL_EXSTYLE = (-20);
        private void SetToMouseThrough()
        {

            // Get this window's handle
            IntPtr hwnd = new WindowInteropHelper(this).Handle;

            // Change the extended window style to include WS_EX_TRANSPARENT
            extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }
        private void SetToNoMouseThrough()
        {
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle);
        }
        #endregion
        
        public abstract void Update();
        public abstract void Load();
        public abstract void UpdateDisplay();
        private bool adjuesting;
        public bool Adjuest
        {
            set
            {
                adjuesting = value;
                Background = new SolidColorBrush(value ? Colors.Gray : Colors.Transparent);
                BorderThickness = new Thickness((value ? 4 : item.BorderThickness));
                ResizeMode = value ? ResizeMode.CanResize : ResizeMode.NoResize;
                IsHitTestVisible = value;
                if (value)
                {
                    SetToNoMouseThrough();
                }
                else
                {
                    SetToMouseThrough();
                    item.Left = Left;
                    item.Top = Top;
                    item.Width = Width;
                    item.Height = Height;
                }
            }
            get => adjuesting;
        }
       public abstract DeviceInfo DeviceInfo { get; set; }

        protected Brush ToBrush(string color)
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
        }
    }
}
