using System;
using System.Collections.Generic;
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

namespace DesktopInformation.DesktopObj
{
    /// <summary>
    /// WinTextObj.xaml 的交互逻辑
    /// </summary>
    public partial class WinTextObj : WinObjBase
    {
        Properties.Settings set;
        public WinTextObj(Properties.Settings set) : base(set)
        {
            this.set = set;
            InitializeComponent();


            normal = @"\{(?<Type>" + supportTime+"|"+supportInfo + @")\}";
            Timing = @"\{(?<TimingName>[a-zA-Z0-9]+):(?<Type>" + supportTime + @")\}";
            NormalWithFormat = @"\{(?<Type>" + supportTime + "|" + supportInfo + @"):(?<Length>[0-9]{1,2})\.(?<Decimal>[0-9])\}";
            TimingWithFormat = @"\{(?<TimingName>[a-zA-Z0-9]+):(?<Type>" + supportTime + @"):(?<Length>[0-9]{1,2})\.(?<Decimal>[0-9])\}";
            rNormal = new Regex(normal, RegexOptions.Compiled);
            rTiming = new Regex(Timing, RegexOptions.Compiled);
            rNormalWithFormat = new Regex(NormalWithFormat, RegexOptions.Compiled);
            rTimingWithFormat = new Regex(TimingWithFormat, RegexOptions.Compiled);

        }
        public void SetText(string text)
        {
            this.text = text;
            Analysis();
        }
        string text = null;
        string supportTime = "Year|Month|Day|Hour|Minute|Second";
        string supportInfo = "Cpu";
        public override void Update()
        {
            StringBuilder temp = new StringBuilder(text);

            foreach (var i in matchedString)
            {
                temp=temp.Replace(i, Analysis(i));
            }
            txt.Text = temp.ToString();
        }



        string normal;
        string Timing;
        string NormalWithFormat;
        string TimingWithFormat;
        Regex rNormal;
        Regex rTiming;
        Regex rNormalWithFormat;
        Regex rTimingWithFormat;

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

        private double GetValue(string format)
        {
            DateTime now = DateTime.Now;
            switch (format)
            {
                case "Year":
                    return now.Year;
                case "Month":
                    return now.Month;
                case "Day":
                    return now.Day;
                case "Hour":
                    return now.Hour;
                case "Minute":
                    return now.Minute;
                case "Second":
                    return now.Second;
            }
            return 0;
        }

        private string GetRepeatedZero(int length)
        {
            return new string(' ', length);
        }

        private string Analysis(string text)
        {
            if(rNormal.IsMatch(text))
            {
                return GetValue(rNormal.Match(text).Groups["Type"].Value).ToString();
            }

            if(rNormalWithFormat.IsMatch(text))
            {
                Match match = rNormalWithFormat.Match(text);
                int dec = int.Parse(match.Groups["Decimal"].Value);
                int length= int.Parse(match.Groups["Length"].Value);
                string type = match.Groups["Type"].Value;
                if (dec==0 && length==0)
                {
                    return GetValue(type).ToString();
                }
                if(dec==0)
                {
                    return string.Format("{0:" + GetRepeatedZero(length) + "}", GetValue(type));
                }
                return string.Format("{0:" + GetRepeatedZero (length)+ "."+GetRepeatedZero(dec) + "}", GetValue(type));
            }
            return text;
        }
    }
}
