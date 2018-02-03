using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopInformation.Tool;

namespace DesktopInformation.DesktopObj
{
    public abstract class WinPercentageDataTypeObjBase:WinObjBase
    {
        public WinPercentageDataTypeObjBase(Binding.ObjListBinding item, Properties.Settings set, DataManager dataManager) : base(item, set,dataManager)
        {

        }
        
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


        public override void Update()
        {
            if(strMin!=null)
            {
                min =dataManager.GetDeviceValue(strMin);
            }
            if (strValue != null)
            {
                value = dataManager.GetDeviceValue(strValue);
            }
            if (strMax != null)
            {
                max = dataManager.GetDeviceValue(strMax);
            }


        }

        public abstract override void UpdateDisplay();
    }
}
