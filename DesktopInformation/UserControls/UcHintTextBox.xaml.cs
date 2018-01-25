﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopInformation.UserControls
{
    /// <summary>
    /// UcHintTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class UcHintTextBox : UserControl
    {
        public UcHintTextBox()
        {
            InitializeComponent();
        }
        public string HintText { get => Resources["HintText"] as string; set => Resources["HintText"] = value; }
        public string Text { get => txt.Text; set => txt.Text = value; }
        public bool TextWrapping
        {
            set
            {
                txt.TextWrapping = value ? System.Windows.TextWrapping.Wrap : System.Windows.TextWrapping.NoWrap;
                Resources["alignmentY"] = value ? AlignmentY.Top : AlignmentY.Center;
                txt.AcceptsReturn = value;
            }
        }
        public VerticalAlignment TextVerticalContentAlignment
        {
            get => txt.VerticalContentAlignment;
            set => txt.VerticalContentAlignment = value;
        }
    }
}
