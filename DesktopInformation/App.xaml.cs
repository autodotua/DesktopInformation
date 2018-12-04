using DesktopInformation.DataAnalysis;
using DesktopInformation.DesktopObj;
using DesktopInformation.Windows;
using FzLib.Extension;
using FzLib.Program.Runtime;
using System.Diagnostics;
using System.Windows;

namespace DesktopInformation
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application, ISingleObject<WinObjList>
    {
        public static App Instance { get; private set; }
        public WinObjManager Manager { get; private set; }
        public WinObjList SingleObject { get; set; }

        private TrayIcon tray = new TrayIcon(DesktopInformation.Properties.Resources.icon, "桌面信息员");

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            FzLib.Program.Information.WorkingDirectory = FzLib.Program.Information.ProgramDirectoryPath;
            Instance = this;
            FzLib.Program.Runtime.UnhandledException.RegistAll();
            FzLib.Program.Runtime.SingleInstance singleInstance = new FzLib.Program.Runtime.SingleInstance("DesktopInformation");
            if (await singleInstance.CheckAndOpenWindow<WinObjList>(this,this))
            {
                return;
            }
            tray.AddContextMenuItem("设置", new WinSettings().Show);
            tray.AddContextMenuItem("调整", ()=>Manager.Adjust());
            tray.AddContextMenuItem("刷新", async () => await Manager.RefreshWindows());
            tray.AddContextMenuItem("配置", () =>
             {
                 try
                 {
                     Process.Start("notepad++", Config.ConfigPath);
                 }
                 catch
                 {
                     Process.Start(Config.ConfigPath);
                 }
             });
            tray.AddContextMenuItem("退出", Shutdown);
            tray.ReShowWhenDisplayChanged = true;
            tray.ClickToOpenOrHideWindow(this);
            
            tray.Show();

            if (e.Args.Length == 0 || e.Args[0] != "startup")
            {
                (MainWindow = new WinObjList()).Show();
            }
            Manager = new WinObjManager();
            await Manager.Load();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            tray?.Dispose();
        }
    }
}
