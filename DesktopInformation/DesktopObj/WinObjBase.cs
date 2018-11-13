using DesktopInformation.DataAnalysis;
using DesktopInformation.Info;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shell;

namespace DesktopInformation.DesktopObj
{


    public abstract class WinObjBase : Window
    {
        public ObjInfo Item { get; set; }

        public WinObjBase(Info.ObjInfo item, DataManager dataManager, bool adjust) : this(item, adjust)
        {
            this.dataManager = dataManager;
        }
        WindowChrome chrome = new WindowChrome();
        FzLib.Windows.WindowStyle style;
        public WinObjBase(Info.ObjInfo item, bool adjust) : base()
        {
            DataContext = this;
            this.Item = item;

            Width = item.Width;
            Height = item.Height;
            Left = item.Left;
            Top = item.Top;
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
            Background = Brushes.Transparent;
            ShowInTaskbar = false;
            BorderBrush = Brushes.Gray;
            WindowChrome.SetWindowChrome(this, chrome);
            if (adjust)
            {
                StartAdjust();
                Closing += (p1, p2) =>
                  {
                      StopAdjust();
                  };
            }
            else
            {
                Loaded += (p1, p2) =>
                  {

                      style = new FzLib.Windows.WindowStyle(this);
                      style.SetStickOnDesktop(false);
                      style.Set(FzLib.Windows.WindowStyle.WindowModes.ToolWindow);

                      LoadUI();
                  };
            }
        }


        private void WinObjBasePreviewKeyDownEventHandler(object sender, KeyEventArgs e)
        {
            if (Adjuesting)
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
                            Left -= 10;
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



        //#region WinAPI



        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //public static extern IntPtr FindWindow([MarshalAs(UnmanagedType.LPTStr)] string lpClassName, [MarshalAs(UnmanagedType.LPTStr)] string lpWindowName);

        //[DllImport("user32")]
        //private static extern IntPtr FindWindowEx(IntPtr hWnd1, IntPtr hWnd2, string lpsz1, string lpsz2);

        //[DllImport("user32.dll")]
        //public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);


        //[DllImport("user32.dll")]
        //public static extern int GetWindowLong(IntPtr hwnd, int index);

        //[DllImport("user32.dll")]
        //public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);
        //public const int WS_EX_TRANSPARENT = 0x00000020;
        //public const int GWL_EXSTYLE = (-20);
        //public const int WS_EX_TOOLWINDOW = 0x00000080;
        //IntPtr winHandle;
        //public void SetToStickOnDesktop()
        //{
        //    IntPtr pWnd = FindWindow("Progman", null);
        //    pWnd = FindWindowEx(pWnd, IntPtr.Zero, "SHELLDLL_DefVIew", null);
        //    pWnd = FindWindowEx(pWnd, IntPtr.Zero, "SysListView32", null);
        //    SetParent(new WindowInteropHelper(this).Handle, pWnd);
        //}

        //private void SetToMouseThrough()
        //{
        //    //extendedStyle = GetWindowLong(winHandle, GWL_EXSTYLE);
        //    SetWindowLong(winHandle, GWL_EXSTYLE, WS_EX_TRANSPARENT | WS_EX_TOOLWINDOW);
        //}
        //private void SetToNoMouseThrough()
        //{
        //    SetWindowLong(winHandle, GWL_EXSTYLE, 0);
        //}

        //#endregion

        public abstract void Load();
        public abstract void Update();
        public abstract void LoadUI();
        public bool Adjuesting { get; set; }
        private void StartAdjust()
        {
            Adjuesting = true;
            IsHitTestVisible = true;
            BorderThickness = new Thickness(4);
            chrome.ResizeBorderThickness = new Thickness(4);
            Background = Brushes.LightGray;
        }

        private void StopAdjust()
        {
            Item.Left = Left;
            Item.Top = Top;
            Item.Width = Width;
            Item.Height = Height;
            Config.Instance.Save();
        }


        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            DragMove();
        }
        public DataManager dataManager;
        protected static Storyboard NewDoubleAnimation(FrameworkElement obj, DependencyProperty property, double to, double duration, double decelerationRatio = 0, EventHandler completed = null, bool stopAfterComplete = false, EasingFunctionBase easingFunction = null)
        {
            DoubleAnimation ani = new DoubleAnimation
            {
                To = to,
                Duration = new Duration(TimeSpan.FromSeconds(duration)),//动画时间1秒
                DecelerationRatio = decelerationRatio,
                FillBehavior = stopAfterComplete ? FillBehavior.Stop : FillBehavior.HoldEnd,
                EasingFunction = easingFunction,
            };


            Storyboard.SetTarget(ani, obj);
            Storyboard.SetTargetProperty(ani, new PropertyPath(property));
            Storyboard story = new Storyboard();
            //Debug.WriteLine(Timeline.GetDesiredFrameRate(story));

            story.Children.Add(ani);
            if (completed != null)
            {
                story.Completed += completed;
            }
            story.Begin();
            return story;
        }

    }
}
