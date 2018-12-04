using DesktopInformation.Info;
using FzLib.Basic;
using FzLib.Control.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static DesktopInformation.DataAnalysis.DeviceInfo;

namespace DesktopInformation.DataAnalysis
{
    public class TextDataAnalysis
    {
        public Dictionary<string, DateTime> timer = new Dictionary<string, DateTime>();
        private DataManager dataManager;
        public ObjInfo Item;
        public string text;
        public TextDataAnalysis(DataManager manager,ObjInfo item)
        {
            dataManager = manager;
            Item = item;
            text = item.Value;
        }
        static TextDataAnalysis()
        {
            Timing = @"\{(?<TimingName>[a-zA-Z0-9]+):(?<Type>" + supportTimeSpan + @")\}";
            NormalWithFormat = @"\{(?<Type>" + supportDateTime + "|" + SupportInfo + @"):(?<Length>[0-9]{1,2})\.(?<Decimal>[0-9])\}";
            TimingWithFormat = @"\{(?<TimingName>[a-zA-Z0-9]+):(?<Type>" + supportTimeSpan + @"):(?<Length>[0-9]{1,2})\.(?<Decimal>[0-9])\}";

            rTiming = new Regex(Timing, RegexOptions.Compiled);
            rNormalWithFormat = new Regex(NormalWithFormat, RegexOptions.Compiled);
            rTimingWithFormat = new Regex(TimingWithFormat, RegexOptions.Compiled);

        }
        public static void LoadRegex()
        {
            normal = @"\{(?<Type>" + supportDateTime + "|" + SupportInfo + @")\}";
            rNormal = new Regex(normal, RegexOptions.Compiled);
        }
        /// <summary>
        /// 支持的日期时间型正则子串
        /// </summary>
        private static readonly string supportDateTime = "Year|Month|Day|Hour|Minute|Second";
        /// <summary>
        /// 支持的时间差型正则子串
        /// </summary>
        private static readonly string supportTimeSpan = "Days|Hours|Minutes|Seconds|Milliseconds|TotalDays|TotalHours|TotalMinutes|TotalSeconds|TotalMilliseconds";
        /// <summary>
        /// 普通类型正则表达式
        /// </summary>
        private static  string normal;
        /// <summary>
        /// 计时类型正则表达式
        /// </summary>
        private static readonly string Timing;
        /// <summary>
        /// 普通类型带格式约束正则表达式
        /// </summary>
        private static readonly string NormalWithFormat;
        /// <summary>
        /// 即使类型带格式约束正则表达式
        /// </summary>
        private static readonly string TimingWithFormat;
        /// <summary>
        /// 普通类型正则
        /// </summary>
        private static  Regex rNormal;
        /// <summary>
        /// 普通类型计时
        /// </summary>
        private static readonly Regex rTiming;
        /// <summary>
        /// 普通类型带格式约束正则
        /// </summary>
        private static readonly Regex rNormalWithFormat;
        /// <summary>
        /// 计时类型带格式约束正则
        /// </summary>
        private static readonly Regex rTimingWithFormat;

        /// <summary>
        /// 将指定的类型转变为当前值
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string ConvertToValue(string text)
        {
            if (rNormal.IsMatch(text))
            {
                return dataManager.GetValue(rNormal.Match(text).Groups["Type"].Value, Item.Absolute);
            }
            if (rNormalWithFormat.IsMatch(text))
            {
                Match match = rNormalWithFormat.Match(text);
                int dec = int.Parse(match.Groups["Decimal"].Value);
                int length = int.Parse(match.Groups["Length"].Value);
                string type = match.Groups["Type"].Value;
                return dataManager.GetValue(type, length, dec, Item.Absolute);
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
                return GetValue(name, type, length, dec);
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
            if (Item.Absolute)
            {
                result = Math.Abs(result);
            }
            return DataManager.ToSpecifiedLengthAndDec(result, length, dec);

        }
        private void LoadCountdown()
        {
            text = "";
            Regex rDate = new Regex(@"\{(?<Name>[a-zA-Z0-9]+):(?<Year>\d{4}),(?<Month>\d{1,2}),(?<Day>\d{1,2})\}");
            Regex rDateTime = new Regex(@"\{(?<Name>[a-zA-Z0-9]+):(?<Year>\d{4}),(?<Month>\d{1,2}),(?<Day>\d{1,2}),(?<Hour>\d{1,2}),(?<Minute>\d{1,2}),(?<Second>\d{1,2})\}");
            timer.Clear();
            foreach (var i in Item.Value.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {

                if (rDate.IsMatch(i))
                {
                    Match match = rDate.Match(i);
                    if (timer.ContainsKey(match.Groups["Name"].Value))
                    {
                        DialogBox.ShowError("存在相同的计时名：" + match.Groups["Name"].Value + "！请立即更改。");
                        continue;
                    }
                    try
                    {
                        timer.Add(match.Groups["Name"].Value, new DateTime(int.Parse(match.Groups["Year"].Value), int.Parse(match.Groups["Month"].Value), int.Parse(match.Groups["Day"].Value)));
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        DialogBox.ShowError("“" + match.Groups["Name"].Value + "”的日期不合法！");
                        return;
                    }
                }
                else if (rDateTime.IsMatch(i))
                {
                    Match match = rDateTime.Match(i);
                    if (timer.ContainsKey(match.Groups["Name"].Value))
                    {
                        DialogBox.ShowError("存在相同的计时名：" + match.Groups["Name"].Value + "！请更改。");
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
                    catch (ArgumentOutOfRangeException)
                    {
                        DialogBox.ShowError("“" + match.Groups["Name"].Value + "”的日期不合法！");
                        return;
                    }
                }
                else
                {
                    this.text += i + Environment.NewLine;
                }
            }
            this.text = text.RemoveEnd(Environment.NewLine);
        }

        /// <summary>
        /// 经过处理的文本
        /// </summary>
        public List<string> MatchedString { get; private set; }
        public void Analysis()
        {
            LoadCountdown();
            if (text == null || text == "")
            {
                return;
            }
            Regex rAll = new Regex($"({normal}|{Timing}|{NormalWithFormat}|{TimingWithFormat})");
            MatchedString = rAll.Matches(text).Cast<Match>().Select(p => p.Value).ToList();
        }
    }
}
