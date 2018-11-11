using DesktopInformation.Info;
using FzLib.Control.Dialog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopInformation.DataAnalysis
{
    class Config:FzLib.Data.Serialization.JsonSerializationBase
    {
        public const string ConfigPath= "DesktopInformationConfig.json";
        public Config()
        {
            Settings.Formatting = Newtonsoft.Json.Formatting.Indented;
        }
        public static  Config Instance = Config.TryOpenOrCreate<Config>(ConfigPath);
        public bool DisableNetwork { get; set; } = true;
        public string NetworkAdapter { get; set; } = "";
        public double UpdateInterval { get; set; } = 1;
        public ObservableCollection<ObjInfo> Objs = new ObservableCollection<ObjInfo>();

        public override void Save()
        {
            try
            {
                base.Save();
            }
            catch(Exception ex)
            {
                DialogHelper.ShowException("保存配置文件失败", ex);
            }
        }
    }
}
