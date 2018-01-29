using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DesktopInformation.Properties;
using static DesktopInformation.Tools.Tools;
using static DesktopInformation.Tools.DeviceInfo;

namespace DesktopInformation.DesktopObj
{
    /// <summary>
    /// WinTextObj.xaml 的交互逻辑
    /// </summary>
    public partial class WinTextObj : WinObjBase
    {
        public WinTextObj(Binding.ObjListBinding item, Settings set, Tools.DeviceInfo deviceInfo) : base(item,set,deviceInfo)
        {
            
            InitializeComponent();


            normal = @"\{(?<Type>" + supportDateTime + "|" + supportInfo + @")\}";
            Timing = @"\{(?<TimingName>[a-zA-Z0-9]+):(?<Type>" + supportTimeSpan + @")\}";
            NormalWithFormat = @"\{(?<Type>" + supportDateTime + "|" + supportInfo + @"):(?<Length>[0-9]{1,2})\.(?<Decimal>[0-9])\}";
            TimingWithFormat = @"\{(?<TimingName>[a-zA-Z0-9]+):(?<Type>" + supportTimeSpan + @"):(?<Length>[0-9]{1,2})\.(?<Decimal>[0-9])\}";
            rNormal = new Regex(normal, RegexOptions.Compiled);
            rTiming = new Regex(Timing, RegexOptions.Compiled);
            rNormalWithFormat = new Regex(NormalWithFormat, RegexOptions.Compiled);
            rTimingWithFormat = new Regex(TimingWithFormat, RegexOptions.Compiled);

        }
        public Dictionary<string, DateTime> timer = new Dictionary<string, DateTime>();
        public override void Load()
        {
            string text = item.Value;
            UpdateDisplay();
            this.text = "";
            Regex rDate = new Regex(@"\{(?<Name>[a-zA-Z0-9]+):(?<Year>\d{4}),(?<Month>\d{1,2}),(?<Day>\d{1,2})\}");
            Regex rDateTime = new Regex(@"\{(?<Name>[a-zA-Z0-9]+):(?<Year>\d{4}),(?<Month>\d{1,2}),(?<Day>\d{1,2}),(?<Hour>\d{1,2}),(?<Minute>\d{1,2}),(?<Second>\d{1,2})\}");
            timer.Clear();
            foreach (var i in text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                
                if (rDate.IsMatch(i))
                {
                    Match match = rDate.Match(i);
                    if (timer.ContainsKey(match.Groups["Name"].Value))
                    {
                        ShowAlert("存在相同的计时名：" + match.Groups["Name"].Value + "！请立即更改。");
                        continue;
                    }
                    try
                    {
                        timer.Add(match.Groups["Name"].Value, new DateTime(int.Parse(match.Groups["Year"].Value), int.Parse(match.Groups["Month"].Value), int.Parse(match.Groups["Day"].Value)));
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        ShowAlert("“" + match.Groups["Name"].Value + "”的日期不合法！");
                        return;
                    }
                }
                else if (rDateTime.IsMatch(i))
                {
                    Match match = rDateTime.Match(i);
                    if (timer.ContainsKey(match.Groups["Name"].Value))
                    {
                        ShowAlert("存在相同的计时名：" + match.Groups["Name"].Value + "！请立即更改。");
                        continue;
                    }
                    try
                    {
                        timer.Add(match.Groups["Name"].Value, new DateTime(int.Parse(match.Groups["Year"].Value),
                            int.Parse(match.Groups["Month"].Value),
                            int.Parse(match.Groups["Day"].Value),
                              int.Parse(match.Groups["Hour"].Value),
                                int.Parse(match.Groups["Minute"].Value),
                                  int.Parse(match.Groups["Second"].Value)
                            ));
                    }
                    catch (ArgumentOutOfRangeException )
                    {
                        ShowAlert("“" + match.Groups["Name"].Value + "”的日期不合法！");
                        return;
                    }
                }
                else
                {
                    this.text += i + Environment.NewLine;
                }
            }
            this.text = this.text.TrimEnd(Environment.NewLine.ToArray());

            // this.text= this.text.Remove(text.Length - 2);
            Analysis();
        }
        string text = null;
        /// <summary>
        /// 支持的日期时间型正则子串
        /// </summary>
        string supportDateTime = "Year|Month|Day|Hour|Minute|Second";
        /// <summary>
        /// 支持的时间差型正则子串
        /// </summary>
        string supportTimeSpan = "Days|Hours|Minutes|Seconds|Milliseconds|TotalDays|TotalHours|TotalMinutes|TotalSeconds|TotalMilliseconds";

        /// <summary>
        /// 更新界面
        /// </summary>
        public override void Update()
        {
            StringBuilder temp = new StringBuilder(text);

            foreach (var i in matchedString)
            {
                temp = temp.Replace(i, ConvertToValue(i));
            }
            UpdateText(temp.ToString());
        }
        /// <summary>
        /// 更新文字
        /// </summary>
        /// <param name="text"></param>
        public void UpdateText(string text)
        {
            if(item.Animation)
            {
                tbkAni.ChangeText(text);
                tbk.Text = "";
            }
            else
            {
                tbkAni.Text = "";
                tbk.Text = text.ToString();
            }
        }
        /// <summary>
        /// 普通类型正则表达式
        /// </summary>
        string normal;
        /// <summary>
        /// 计时类型正则表达式
        /// </summary>
        string Timing;
        /// <summary>
        /// 普通类型带格式约束正则表达式
        /// </summary>
        string NormalWithFormat;
        /// <summary>
        /// 即使类型带格式约束正则表达式
        /// </summary>
        string TimingWithFormat;
        /// <summary>
        /// 普通类型正则
        /// </summary>
        Regex rNormal;
        /// <summary>
        /// 普通类型计时
        /// </summary>
        Regex rTiming;
        /// <summary>
        /// 普通类型带格式约束正则
        /// </summary>
        Regex rNormalWithFormat;
        /// <summary>
        /// 计时类型带格式约束正则
        /// </summary>
        Regex rTimingWithFormat;
        /// <summary>
        /// 经过处理的文本
        /// </summary>
        List<string> matchedString;
        private void Analysis()
        {
            if (text == null || text == "")
            {
                return;
            }
            Regex rAll = new Regex($"({normal}|{Timing}|{NormalWithFormat}|{TimingWithFormat})");
            matchedString = rAll.Matches(text).Cast<Match>().Select(p => p.Value).ToList();
        }
        /// <summary>
        /// 将指定的类型转变为当前值
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string ConvertToValue(string text)
        {
            if (rNormal.IsMatch(text))
            {
                return GetValue(rNormal.Match(text).Groups["Type"].Value);
            }
            if (rNormalWithFormat.IsMatch(text))
            {
                Match match = rNormalWithFormat.Match(text);
                int dec = int.Parse(match.Groups["Decimal"].Value);
                int length = int.Parse(match.Groups["Length"].Value);
                string type = match.Groups["Type"].Value;
                return GetValue(type, length, dec);
            }
            if (rTiming.IsMatch(text))
            {
                Match match = rTiming.Match(text);
                return GetValue(match.Groups["TimingName"].Value, match.Groups["Type"].Value);
            }
            if (rTimingWithFormat.IsMatch(text))
            {
                Match match = rTimingWithFormat.Match(text);
                string name = match.Groups["TimingName"].Value;
                int dec = int.Parse(match.Groups["Decimal"].Value);
                int length = int.Parse(match.Groups["Length"].Value);
                string type = match.Groups["Type"].Value;
                return GetValue(name,type, length, dec);
            }
            return text;
        }
        /// <summary>
        /// 获取计时类型的值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        private string GetValue(string name, string text)
        {

            return GetValue(name, text, 0, 0);

        }
        /// <summary>
        /// 获取指定格式计时类型的值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="length"></param>
        /// <param name="dec"></param>
        /// <returns></returns>
        private string GetValue(string name, string type, int length, int dec)
        {
            if (!timer.ContainsKey(name))
            {
                return "??" + type;
            }

            DateTime target = timer[name];
            TimeSpan span = target - DateTime.Now;
            double result = double.NaN;
            switch (type)
            {
                case "Days":
                    result = span.Days;
                    break;
                case "Hours":
                    result = span.Hours;
                    break;
                case "Minutes":
                    result = span.Minutes;
                    break;
                case "Seconds":
                    result = span.Seconds;
                    break;
                case "Milliseconds":
                    result = span.Milliseconds;
                    break;
                case "TotalDays":
                    result = span.TotalDays;
                    break;
                case "TotalHours":
                    result = span.TotalHours;
                    break;
                case "TotalMinutes":
                    result = span.TotalMinutes;
                    break;
                case "TotalSeconds":
                    result = span.TotalSeconds;
                    break;
                case "TotalMilliseconds":
                    result = span.TotalMilliseconds;
                    break;
            }
            if (double.IsNaN(result))
            {
                return "??" + type;
            }
            return ToSpecifiedLengthAndDec(result, length, dec);

        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetValue(string type)
        {
            return GetValue(type, 0, 0);
        }
        /// <summary>
        /// 获取指定格式的值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="length"></param>
        /// <param name="dec"></param>
        /// <returns></returns>
        private string GetValue(string type, int length, int dec)
        {
            double result = double.NaN;
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
                    result = DeviceInfo.TotalMemory;
                    break;
                case "FreeMemory":
                    result = DeviceInfo.FreeMemory;
                    break;
                case "UsedMemory":
                    result = DeviceInfo.UsedMemory;
                    break;
                case "MemoryUsage":
                    result = DeviceInfo.MemoryUsage;
                    break;
                case "ProcessCount":
                    result = DeviceInfo.ProcessCount;
                    break;
                case "CpuUsage":
                    result = DeviceInfo.CpuUsage;
                    break;

                case "DownloadSpeedKB":
                    result = DeviceInfo.DownloadSpeedKB;
                    break;
                case "DownloadSpeedMB":
                    result = DeviceInfo.DownloadSpeedMB;
                    break;
                case "UploadSpeedKB":
                    result = DeviceInfo.UploadSpeedKB;
                    break;
                case "UploadSpeedMB":
                    result = DeviceInfo.UploadSpeedMB;
                    break;

                case "Battery1Voltage":
                    result = DeviceInfo.Battery1Voltage;
                    break;
                case "Battery2Voltage":
                    result = DeviceInfo.Battery2Voltage;
                    break;
                case "Battery1Rate":
                    result = DeviceInfo.Battery1Rate;
                    break;
                case "Battery2Rate":
                    result = DeviceInfo.Battery2Rate;
                    break;
                case "BatteryPercent":
                    result = DeviceInfo.BatteryPercent;
                    break;
                case "BatteryRemainHours":
                    if(DeviceInfo.BatteryRemain.HasValue)
                    {
                        if(DeviceInfo.BatteryRemain.Value==TimeSpan.Zero)
                        {
                            return "∞";
                        }
                        result = DeviceInfo.BatteryRemain.Value.Hours;
                    }
                    else
                    {
                        return "";
                    }
                    break;
                case "BatteryRemainMinutes":
                    if (DeviceInfo.BatteryRemain.HasValue)
                    {
                        if (DeviceInfo.BatteryRemain.Value == TimeSpan.Zero)
                        {
                            return "∞";
                        }
                        result = DeviceInfo.BatteryRemain.Value.Minutes;
                    }
                    else
                    {
                        return "";
                    }
                    break;
                case "BatteryRemainTotalHours":
                    if (DeviceInfo.BatteryRemain.HasValue)
                    {
                        if (DeviceInfo.BatteryRemain.Value == TimeSpan.Zero)
                        {
                            return "∞";
                        }
                        result = DeviceInfo.BatteryRemain.Value.TotalHours;
                    }
                    else
                    {
                        return "";
                    }
                    break;
                case "BatteryRemainTotalMinutes":
                    if (DeviceInfo.BatteryRemain.HasValue)
                    {
                        if (DeviceInfo.BatteryRemain.Value == TimeSpan.Zero)
                        {
                            return "∞";
                        }
                        result = DeviceInfo.BatteryRemain.Value.TotalMinutes;
                    }
                    else
                    {
                        return "";
                    }
                    break;
            }

            if (!double.IsNaN(result))
            {
                if(item.ForcedAbsolute)
                {
                    result = Math.Abs(result);
                }
                return ToSpecifiedLengthAndDec(result, length, dec);
            }
            //string resultStr;
            //List<double> infos;
            //switch (type)
            //{
            //    case "BatteryVoltage":
            //        infos = DeviceInfo.BatteryVoltage;
            //        resultStr = ToSpecifiedLengthAndDec(infos[0], length, dec);
            //        for (int i = 1; i < infos.Count; i++)
            //        {
            //            resultStr += set.Spliter + ToSpecifiedLengthAndDec(infos[i], length, dec);
            //        }
            //        return resultStr;
            //    case "BatteryRate":
            //        infos = DeviceInfo.BatteryRate;
            //        resultStr = ToSpecifiedLengthAndDec(infos[0], length, dec);
            //        for (int i = 1; i < infos.Count; i++)
            //        {
            //            resultStr += set.Spliter + ToSpecifiedLengthAndDec(infos[i], length, dec);
            //        }
            //        return resultStr;

            //}
            return "??" + type;
        }
        /// <summary>
        /// 到指定精度和长度
        /// </summary>
        /// <param name="number"></param>
        /// <param name="length"></param>
        /// <param name="dec"></param>
        /// <returns></returns>
        private string ToSpecifiedLengthAndDec(double number, int length, int dec)
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
        private string GetRepeatedZero(int length)
        {
            return new string('0', length);
        }

        public override void UpdateDisplay()
        {
            tbk.Background = ToBrush(item.BackgounrdColor);
            tbk.Foreground = tbkAni.Foreground = ToBrush(item.ForegroundColor);
            BorderBrush = ToBrush(item.BorderColor);
            BorderThickness = new Thickness(item.BorderThickness);
        }


        /// <summary>
        /// 设备信息
        /// </summary>
        public override Tools.DeviceInfo DeviceInfo { get; set; }

    }
}
