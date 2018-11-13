using DesktopInformation.DataAnalysis;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace DesktopInformation.DesktopObj
{
    /// <summary>
    /// WinTextObj.xaml 的交互逻辑
    /// </summary>
    public partial class WinTextObj : WinObjBase
    {
        public WinTextObj(Info.ObjInfo item, DataManager dataManager, bool adjust) : base(item, dataManager, adjust)
        {
            InitializeComponent();
            dataAnalysis = new TextDataAnalysis(dataManager, item);
        }

        TextDataAnalysis dataAnalysis;
        public override void Load()
        {

            string text = Item.Value;
            LoadUI();
            dataAnalysis.Analysis();
        }
        

        /// <summary>
        /// 更新界面
        /// </summary>
        public override void Update()
        {
            StringBuilder temp = new StringBuilder(dataAnalysis.text);

            foreach (var i in dataAnalysis.MatchedString)
            {
                temp = temp.Replace(i, dataAnalysis.ConvertToValue(i));
            }
            string str = temp.ToString();
            if (lastText != str)
            {
                UpdateText(str);
                lastText = str;
            }
        }
        public string lastText;
        /// <summary>
        /// 更新文字
        /// </summary>
        /// <param name="text"></param>
        public void UpdateText(string text)
        {
            if (Item.Animation)
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




        public override void LoadUI()
        {
            tbk.Background = Item.Backgounrd;
            tbk.Foreground = tbkAni.Foreground = Item.Foreground;
            BorderBrush = Item.BorderColor;
            BorderThickness = new Thickness(Item.BorderThickness);
            if (Item.ShadowColor != Colors.Transparent)
            {
                grd.Effect = new DropShadowEffect()
                {
                    Color = Item.ShadowColor,
                    BlurRadius = Item.ShadowBlurRadius,
                    ShadowDepth = Item.ShadowDepth,
                    Direction = Item.ShadowDirection,
                };
            }
        }


    }
}
