using DesktopInformation.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shell;
using System.Windows.Threading;

namespace DesktopInformation.DesktopObj
{


    public abstract class WinObjBase : Window
    {
        protected Properties.Settings set;
        protected Binding.ObjListBinding item;
        public WinObjBase(Binding.ObjListBinding item, Properties.Settings set) : base()
        {
            this.set = set;
            this.item = item;

            Width = item.Width;
            Height = item.Height;
            Left = item.Left;
            Top = item.Top;
            FocusVisualStyle = null;
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
                winHandle = new WindowInteropHelper(this).Handle;

                SetToMouseThrough();
                SetToStickOnDesktop();

            };
            PreviewKeyDown += WinObjBasePreviewKeyDownEventHandler;

        }

        private void WinObjBasePreviewKeyDownEventHandler(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Adjuest)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    switch (e.Key)
                    {
                        case Key.Left:
                            Width--;
                            break;
                        case Key.Right:
                            Width++;
                            break;
                        case Key.Up:
                            Height++;
                            break;
                        case Key.Down:
                            Height--;
                            break;
                    }
                }
                else if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    switch (e.Key)
                    {
                        case Key.Left:
                            Left-=10;
                            break;
                        case Key.Right:
                            Left += 10;
                            break;
                        case Key.Up:
                            Top -= 10;
                            break;
                        case Key.Down:
                            Top += 10;
                            break;
                    }
                }
                else
                {
                    switch (e.Key)
                    {
                        case Key.Left:
                            Left--;
                            break;
                        case Key.Right:
                            Left++;
                            break;
                        case Key.Up:
                            Top--;
                            break;
                        case Key.Down:
                            Top++;
                            break;
                    }
                }
            }
        }

        public WinObjBase(Binding.ObjListBinding item, Properties.Settings set, DeviceInfo deviceInfo) : this(item, set)
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


        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);
        public const int WS_EX_TRANSPARENT = 0x00000020;
        public const int GWL_EXSTYLE = (-20);
        public const int WS_EX_TOOLWINDOW = 0x00000080;
        IntPtr winHandle;
        public void SetToStickOnDesktop()
        {
            IntPtr pWnd = FindWindow("Progman", null);
            pWnd = FindWindowEx(pWnd, IntPtr.Zero, "SHELLDLL_DefVIew", null);
            pWnd = FindWindowEx(pWnd, IntPtr.Zero, "SysListView32", null);
            SetParent(new WindowInteropHelper(this).Handle, pWnd);
        }

        private void SetToMouseThrough()
        {
            //extendedStyle = GetWindowLong(winHandle, GWL_EXSTYLE);
            SetWindowLong(winHandle, GWL_EXSTYLE, WS_EX_TRANSPARENT | WS_EX_TOOLWINDOW);
        }
        private void SetToNoMouseThrough()
        {
            SetWindowLong(winHandle, GWL_EXSTYLE, 0);
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
     
                    BorderThickness = new Thickness((value ? 4 : ((this is WinPieObj)?0:item.BorderThickness)));
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
