using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopInformation.Tools;

namespace DesktopInformation.DesktopObj
{
    public abstract class WinPercentageDataTypeObjBase:WinObjBase
    {
        public WinPercentageDataTypeObjBase(Binding.ObjListBinding item, Properties.Settings set, DeviceInfo deviceInfo) : base(item, set,deviceInfo)
        {

        }

        public override DeviceInfo DeviceInfo { get; set; }
        protected string strMin;
        protected string strValue;
        protected string strMax;

        protected double min;
        protected double value;
        protected double max;
        public override void Load()
        {
            string[] temp = item.Value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            if(double.TryParse(temp[0],out min))
            {
                strMin = null;
            }
            else
            {
                strMin = temp[0];
            }
            if (double.TryParse(temp[1], out value))
            {
                strValue = null;
            }
            else
            {
                strValue = temp[1];
            }
            if (double.TryParse(temp[2], out max))
            {
                strMax = null;
            }
            else
            {
                strMax = temp[2];
            }
            UpdateDisplay();

        }

        private double GetValue(string type)
        {

            double result = double.NaN;
            switch (type)
            {

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
                

            }

            if (!double.IsNaN(result))
            {
                return result;
            }


            return 0;
        }

        public override void Update()
        {
            if(strMin!=null)
            {
                min = GetValue(strMin);
            }
            if (strValue != null)
            {
                value = GetValue(strValue);
            }
            if (strMax != null)
            {
                max = GetValue(strMax);
            }


        }

        public abstract override void UpdateDisplay();
    }
}
