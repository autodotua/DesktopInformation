using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DesktopInformation.Tool;

namespace DesktopInformation.Tool
{
   public class DataManager
    {
        public double GetDeviceValue(string type)
        {
           if( double.TryParse(GetValue(type, 0, 0, false), out double result))
            {
                return result;
            }
            return 0;
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetValue(string type, bool abs)
        {
            return GetValue(type, 0, 0,abs);
        }
        /// <summary>
        /// 获取指定格式的值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="length"></param>
        /// <param name="dec"></param>
        /// <returns></returns>
        public string GetValue(string type, int length, int dec,bool abs)
        {
            double result = double.NaN;

            if (type.StartsWith("AIDA64_"))
            {
                type= type.Remove(0, 7);
                if(aida.SupportValueName.Contains(type))
                {
                    string value = aida.GetValue(type);
                    if(double.TryParse(value,out result))
                    {
                        return ToSpecifiedLengthAndDec(result, length, dec);
                    }
                    else
                    {
                        return value;
                    }
                }
                else
                {
                    return "#" + type + "#";
                }
            }
            else
            {
                DateTime now = DateTime.Now;
                switch (type)
                {
                    case "Year":
                        result = now.Year;
                        break;
                    case "Month":
                        result = now.Month;
                        break;
                    case "Day":
                        result = now.Day;
                        break;
                    case "Hour":
                        result = now.Hour;
                        break;
                    case "Minute":
                        result = now.Minute;
                        break;
                    case "Second":
                        result = now.Second;
                        break;

                    case "TotalMemory":
                        result = deviceInfo.TotalMemory;
                        break;
                    case "FreeMemory":
                        result = deviceInfo.FreeMemory;
                        break;
                    case "UsedMemory":
                        result = deviceInfo.UsedMemory;
                        break;
                    case "MemoryUsage":
                        result = deviceInfo.MemoryUsage;
                        break;
                    case "ProcessCount":
                        result = deviceInfo.ProcessCount;
                        break;
                    case "CpuUsage":
                        result = deviceInfo.CpuUsage;
                        break;

                    case "DownloadSpeedKB":
                        result = deviceInfo.DownloadSpeedKB;
                        break;
                    case "DownloadSpeedMB":
                        result = deviceInfo.DownloadSpeedMB;
                        break;
                    case "UploadSpeedKB":
                        result = deviceInfo.UploadSpeedKB;
                        break;
                    case "UploadSpeedMB":
                        result = deviceInfo.UploadSpeedMB;
                        break;

                    //case "Battery1Voltage":
                    //    result = deviceInfo.GetBattery1Voltage();
                    //    break;
                    //case "Battery2Voltage":
                    //    result = deviceInfo.Battery2Voltage;
                    //    break;
                    //case "Battery1Rate":
                    //    result = deviceInfo.GetBattery1Rate();
                    //    break;
                    //case "Battery2Rate":
                    //    result = deviceInfo.Battery2Rate;
                    //    break;
                    case "BatteryPercent":
                        result = deviceInfo.GetBatteryPercent(0);
                        break;
                    case "BatteryRemainHours":
                        if (deviceInfo.BatteryRemain.HasValue)
                        {
                            if (deviceInfo.BatteryRemain.Value == TimeSpan.Zero)
                            {
                                return "∞";
                            }
                            result = deviceInfo.BatteryRemain.Value.Hours;
                        }
                        else
                        {
                            return "";
                        }
                        break;
                    case "BatteryRemainMinutes":
                        if (deviceInfo.BatteryRemain.HasValue)
                        {
                            if (deviceInfo.BatteryRemain.Value == TimeSpan.Zero)
                            {
                                return "∞";
                            }
                            result = deviceInfo.BatteryRemain.Value.Minutes;
                        }
                        else
                        {
                            return "";
                        }
                        break;
                    case "BatteryRemainTotalHours":
                        if (deviceInfo.BatteryRemain.HasValue)
                        {
                            if (deviceInfo.BatteryRemain.Value == TimeSpan.Zero)
                            {
                                return "∞";
                            }
                            result = deviceInfo.BatteryRemain.Value.TotalHours;
                        }
                        else
                        {
                            return "";
                        }
                        break;
                    case "BatteryRemainTotalMinutes":
                        if (deviceInfo.BatteryRemain.HasValue)
                        {
                            if (deviceInfo.BatteryRemain.Value == TimeSpan.Zero)
                            {
                                return "∞";
                            }
                            result = deviceInfo.BatteryRemain.Value.TotalMinutes;
                        }
                        else
                        {
                            return "";
                        }
                        break;
                }
            }
            if (double.IsNaN(result))
            {
                if (Regex.IsMatch(type, "^Battery[0-9]Voltage$"))
                {
                    int index = type.FirstOrDefault(p => p <= '9' && p >= '0') - '0';
                    result = deviceInfo.GetBatteryVoltage(index);
                }
                if (Regex.IsMatch(type, "^Battery[0-9]Percent$"))
                {
                    int index = type.FirstOrDefault(p => p <= '9' && p >= '0') - '0';
                    result = deviceInfo.GetBatteryPercent(index);
                }
                if (Regex.IsMatch(type, "^Battery[0-9]Rate$"))
                {
                    int index = type.FirstOrDefault(p => p <= '9' && p >= '0') - '0';
                    result = deviceInfo.GetBatteryRate(index);
                }
            }
            if (!double.IsNaN(result))
            {
                if (abs)
                {
                    result = Math.Abs(result);
                }
                return ToSpecifiedLengthAndDec(result, length, dec);
            }
            return "#" + type+"#";
        }
        /// <summary>
        /// 到指定精度和长度
        /// </summary>
        /// <param name="number"></param>
        /// <param name="length"></param>
        /// <param name="dec"></param>
        /// <returns></returns>
        public static string ToSpecifiedLengthAndDec(double number, int length, int dec)
        {

            if (dec == 0 && length == 0)
            {
                return number.ToString();
            }
            if (dec == 0)
            {
                return string.Format("{0:" + GetRepeatedZero(length) + "}", number);
            }
            return string.Format("{0:" + GetRepeatedZero(length) + "." + GetRepeatedZero(dec) + "}", number);

        }
        /// <summary>
        /// 获取一串指定重复次数的0
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRepeatedZero(int length)
        {
            return new string('0', length);
        }

        DeviceInfo deviceInfo;
        Properties.Settings set;
        Aida64Linker aida = new Aida64Linker();

        public DataManager(Properties.Settings set)
        {
            this.set = set;
            deviceInfo = new DeviceInfo(set);
        }

        public void Update()
        {
            deviceInfo.Update();
        }
    }
}
