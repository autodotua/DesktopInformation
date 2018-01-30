using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Windows.Forms.SystemInformation;

namespace DesktopInformation.Tools
{
    public class DeviceInfo
    {
        /// <summary>
        /// 支持的系统信息正则子串
        /// </summary>
        public static string supportInfo = "TotalMemory|MemoryUsage|FreeMemory|UsedMemory|ProcessCount|CpuUsage|" +
            "DownloadSpeedKB|DownloadSpeedMB|UploadSpeedKB|UploadSpeedMB|" +
            "Battery1Voltage|Battery2Voltage|Battery1Rate|Battery2Rate|BatteryPercent|BatteryRemainHours|BatteryRemainMinutes|BatteryRemainTotalHours|BatteryRemainTotalMinutes";
        private NetworkMonitor monitor;
        NetworkAdapter adapter = null;
        Properties.Settings set;
        public DeviceInfo(Properties.Settings set)
        {
            this.set = set;
            monitor = new NetworkMonitor();
            var adapters = monitor.Adapters.Where(p => p.Name == set.NetworkAdapter);
            if (adapters.Count() > 0)
            {
                adapter = adapters.ToArray()[0];
                monitor.StartMonitoring();
            }
            cpuCounter = new PerformanceCounter("Processor", "% Idle Time", "_Total");
            cpuCounter.NextValue();
            // computer.Open();
            Update();
        }
        PerformanceCounter cpuCounter;
        ManagementObjectSearcher systemInfoSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");
        ManagementObjectSearcher batteryInfoSearcher = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM BatteryStatus");
        ManagementObject systemInfo;
        ManagementObject[] batteryInfo;
        public void Update()
        {
            systemInfo = systemInfoSearcher.Get().Cast<ManagementObject>().ToArray()[0];
            batteryInfo = batteryInfoSearcher.Get().Cast<ManagementObject>().ToArray();
            cpuUsage = 100-cpuCounter.NextValue();

        }
        //[DllImport("kernel32.dll")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //static extern bool GetPhysicallyInstalledSystemMemory(out long TotalMemoryInKilobytes);
        //public double TotalMemory
        //{
        //    get
        //    {
        //        long memKb;
        //        GetPhysicallyInstalledSystemMemory(out memKb);
        //        return memKb * 1.0 / 1024 / 1024;
        //    }
        //}
        /// <summary>
        /// 总内存（GB）
        /// </summary>
        public double TotalMemory => double.Parse(systemInfo["TotalVisibleMemorySize"].ToString()) / (1024 * 1024);
        /// <summary>
        /// 已用内存（GB）
        /// </summary>
        public double UsedMemory => TotalMemory - FreeMemory;
        /// <summary>
        /// 可用内存（GB)
        /// </summary>
        public double FreeMemory => double.Parse(systemInfo["FreePhysicalMemory"].ToString()) / (1024 * 1024);
        /// <summary>
        /// 内存使用率（%）
        /// </summary>
        public double MemoryUsage => UsedMemory / TotalMemory*100;
        /// <summary>
        /// 进程数量
        /// </summary>
        public double ProcessCount => double.Parse(systemInfo["NumberOfProcesses"].ToString());
        /// <summary>
        /// 电池1电压（V）
        /// </summary>
        public double Battery1Voltage => double.Parse(batteryInfo[0]["Voltage"].ToString()) / 1000;
        /// <summary>
        /// 电池2电压（V）
        /// </summary>
        public double Battery2Voltage => double.Parse(batteryInfo[1]["Voltage"].ToString()) / 1000;
        /// <summary>
        /// 电池1功率（W，充电为正）
        /// </summary>
        public double Battery1Rate => (double.Parse(batteryInfo[0]["ChargeRate"].ToString())- double.Parse(batteryInfo[0]["DischargeRate"].ToString()) )/ 1000;
        /// <summary>
        /// 电池2功率（W，充电为正）
        /// </summary>
        public double Battery2Rate =>( double.Parse(batteryInfo[1]["ChargeRate"].ToString())- double.Parse(batteryInfo[1]["DischargeRate"].ToString())) / 1000;
        /// <summary>
        /// 点亮百分比
        /// </summary>
        public double BatteryPercent => Math.Round(PowerStatus.BatteryLifePercent * 100);
        /// <summary>
        /// 电池剩余时间，0为正在充电，null为未知
        /// </summary>
        public TimeSpan? BatteryRemain
        {
            get
            {
                TimeSpan? remain = TimeSpan.FromSeconds(PowerStatus.BatteryLifeRemaining);
                return PowerStatus.PowerLineStatus == System.Windows.Forms.PowerLineStatus.Online ?
                     TimeSpan.Zero : (PowerStatus.BatteryLifeRemaining == -1 ? null : remain);

            }
        }

        //public double CpuTemp
        //{
        //    get
        //    {
        //        return -1;
        //    }
        //}

        //public double CpuLoad
        //{
        //    get
        //    {
        //        //CheckAdministration();
        //        //var sensor = computer.Hardware.Where(p => p.HardwareType == HardwareType.CPU).ToArray()[0].Sensors;
        //        //var value = sensor.Where(p => p.Name == "CPU Total" && p.SensorType==SensorType.Load).ToArray()[0].Value;
        //        //return value == null ? -1 : (double)value;
        //        return -1;
        //    }
        //}


        //private void CheckAdministration()
        //{
        //    if(set.ignoreAdministration)
        //    {
        //        return ;
        //    }
        //   WindowsIdentity identity = WindowsIdentity.GetCurrent();
        //   WindowsPrincipal principal = new WindowsPrincipal(identity);
        //   if( principal.IsInRole(WindowsBuiltInRole.Administrator))
        //    {
        //        return ;
        //    }
        //    if(Tools.Tools.ShowAlert("获取某些系统信息需要管理员权限!"+Environment.NewLine+"是否给予？",MessageBoxButton.YesNo)==false)
        //    {
        //        set.ignoreAdministration = true;
        //    }

        //    ProcessStartInfo psi = new ProcessStartInfo();
        //    psi.FileName = Process.GetCurrentProcess().MainModule.FileName;
        //    psi.Verb = "runas";
        //    try
        //    {
        //        Process.Start(psi);
        //        Application.Current.Shutdown();
        //    }
        //    catch (Exception eee)
        //    {
        //        MessageBox.Show(eee.Message);
        //    }
        //}

        /// <summary>
        /// CPU使用率（%）
        /// </summary>
        public double CpuUsage => cpuUsage;
        public double DownloadSpeedKB => adapter == null ? -1 : adapter.DownloadSpeedKbps;
        public double DownloadSpeedMB => adapter == null ? -1 : adapter.DownloadSpeedKbps / 1024;
        public double UploadSpeedKB => adapter == null ? -1 : adapter.UploadSpeedKbps;
        public double UploadSpeedMB => adapter == null ? -1 : adapter.UploadSpeedKbps / 1024;

        private double cpuUsage;
    }
}



/*


    BootDevice    \Device\HarddiskVolume1
BuildNumber    16299
BuildType    Multiprocessor Free
Caption    Microsoft Windows 10 专业版
CodeSet    936
CountryCode    86
CreationClassName    Win32_OperatingSystem
CSCreationClassName    Win32_ComputerSystem
CSDVersion
CSName    FZ-LAPTOP
CurrentTimeZone    480
DataExecutionPrevention_32BitApplications    True
DataExecutionPrevention_Available    True
DataExecutionPrevention_Drivers    True
DataExecutionPrevention_SupportPolicy    2
Debug    False
Description
Distributed    False
EncryptionLevel    256
ForegroundApplicationBoost    2
FreePhysicalMemory    3485364
FreeSpaceInPagingFiles    3449924
FreeVirtualMemory    4298452
InstallDate    20171019161946.000000+480
LargeSystemCache
LastBootUpTime    20180124193156.485130+480
LocalDateTime    20180126082819.720000+480
Locale    0804
Manufacturer    Microsoft Corporation
MaxNumberOfProcesses    4294967295
MaxProcessMemorySize    137438953344
MUILanguages    System.String[]
Name    Microsoft Windows 10 专业版|C:\WINDOWS|\Device\Harddisk0\Partition3
NumberOfLicensedUsers
NumberOfProcesses    219
NumberOfUsers    2
OperatingSystemSKU    48
Organization
OSArchitecture    64 位
OSLanguage    2052
OSProductSuite    256
OSType    18
OtherTypeDescription
PAEEnabled
PlusProductID
PlusVersionNumber
PortableOperatingSystem    False
Primary    True
ProductType    1
RegisteredUser
SerialNumber    00330-80000-00000-AA824
ServicePackMajorVersion    0
ServicePackMinorVersion    0
SizeStoredInPagingFiles    3538944
Status    OK
SuiteMask    272
SystemDevice    \Device\HarddiskVolume3
SystemDirectory    C:\WINDOWS\system32
SystemDrive    C:
TotalSwapSpaceSize
TotalVirtualMemorySize    11803488
TotalVisibleMemorySize    8264544
Version    10.0.16299
WindowsDirectory    C:\WINDOWS


    */
