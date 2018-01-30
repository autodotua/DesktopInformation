using System;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopInformation.UserControls
{
    /// <summary>
    /// ProgressPie.xaml 的交互逻辑
    /// </summary>
    public partial class ProgressPie : UserControl
    {
        public ProgressPie()
        {
            InitializeComponent();
        }
        public void AnimationToValue(double value)
        {
            Tools.Tools.NewDoubleAnimation(p, ProgressBar.ValueProperty, value, 0.5,0,null,false,new CubicEase() { EasingMode = EasingMode.EaseInOut });
        }
        public new double BorderThickness
        {
            get => ValueToProcessConverter.Thickness;
            set => ValueToProcessConverter.Thickness = value;
        }
        public new SolidColorBrush Foreground
        {
            get => ValueToProcessConverter.foregroundBrush;
            set => ValueToProcessConverter.foregroundBrush = value;
        }
        public new SolidColorBrush Background
        {
            get => ValueToProcessConverter.backgroundBrush;
            set => ValueToProcessConverter.backgroundBrush = value;
        }
    }


    class ValueToProcessConverter : IValueConverter
    {
        public static double Thickness = 0.1;
        public static SolidColorBrush foregroundBrush;
        public static SolidColorBrush backgroundBrush;
        private Point centerPoint;
        private double radius;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double && !string.IsNullOrEmpty((string)parameter))
            {
                double arg = (double)value;
                double width = 10;
                radius = width / 2;
                centerPoint = new Point(radius, radius);

                return DrawBrush(arg, 1, radius, radius, Thickness);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据角度获取坐标
        /// </summary>
        /// <param name="CenterPoint"></param>
        /// <param name="r"></param>
        /// <param name="angel"></param>
        /// <returns></returns>
        private Point GetPointByAngel(Point CenterPoint, double r, double angel)
        {
            return new Point
            {
                X = Math.Sin(angel * Math.PI / 180) * r + CenterPoint.X,
                Y = CenterPoint.Y - Math.Cos(angel * Math.PI / 180) * r
            };
        }

        /// <summary>
        /// 根据4个坐标画出扇形
        /// </summary>
        /// <param name="bigFirstPoint"></param>
        /// <param name="bigSecondPoint"></param>
        /// <param name="smallFirstPoint"></param>
        /// <param name="smallSecondPoint"></param>
        /// <param name="bigRadius"></param>
        /// <param name="smallRadius"></param>
        /// <param name="isLargeArc"></param>
        /// <returns></returns>
        private Geometry DrawingArcGeometry(Point bigFirstPoint, Point bigSecondPoint, Point smallFirstPoint, Point smallSecondPoint, double bigRadius, double smallRadius, bool isLargeArc)
        {
            PathFigure pathFigure = new PathFigure { IsClosed = true };
            pathFigure.StartPoint = bigFirstPoint;
            pathFigure.Segments.Add(
              new ArcSegment
              {
                  Point = bigSecondPoint,
                  IsLargeArc = isLargeArc,
                  Size = new Size(bigRadius, bigRadius),
                  SweepDirection = SweepDirection.Clockwise
              });
            pathFigure.Segments.Add(new LineSegment { Point = smallSecondPoint });
            pathFigure.Segments.Add(
             new ArcSegment
             {
                 Point = smallFirstPoint,
                 IsLargeArc = isLargeArc,
                 Size = new Size(smallRadius, smallRadius),
                 SweepDirection = SweepDirection.Counterclockwise
             });
            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);

            return pathGeometry;
        }

        /// <summary>
        /// 根据当前值和最大值获取扇形
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        private Geometry GetGeometry(double value, double radiusX, double radiusY, double thickness)
        {
            double percent = value;
            double angel = percent * 360;
            double bigR = radiusX+ 0.5*thickness;
            double smallR = radiusX -0.5* thickness;
            Point firstpoint = GetPointByAngel(centerPoint, bigR, 0);
            Point secondpoint = GetPointByAngel(centerPoint, bigR, angel);
            Point thirdpoint = GetPointByAngel(centerPoint, smallR, 0);
            Point fourthpoint = GetPointByAngel(centerPoint, smallR, angel);
            return DrawingArcGeometry(firstpoint, secondpoint, thirdpoint, fourthpoint, bigR, smallR, angel > 180);
        }

        private void DrawingGeometry(DrawingContext drawingContext, double value, double maxValue, double radiusX, double radiusY, double thickness)
        {
            if (value < maxValue)
            {
                drawingContext.DrawEllipse(null, new Pen(backgroundBrush, thickness), centerPoint, radiusX, radiusY);
                drawingContext.DrawGeometry(foregroundBrush, new Pen(), GetGeometry(value, radiusX, radiusY, thickness));

            }
            else
            {
                drawingContext.DrawEllipse(null, new Pen(foregroundBrush, thickness), centerPoint, radiusX, radiusY);

            }

            drawingContext.Close();
        }

        /// <summary>
        /// 根据当前值和最大值画出进度条
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        private Visual DrawShape(double value, double maxValue, double radiusX, double radiusY, double thickness)
        {
            DrawingVisual drawingWordsVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingWordsVisual.RenderOpen();

            DrawingGeometry(drawingContext, value, maxValue, radiusX, radiusY, thickness);

            return drawingWordsVisual;
        }

        /// <summary>
        /// 根据当前值和最大值画出进度条
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        private Brush DrawBrush(double value, double maxValue, double radiusX, double radiusY, double thickness)
        {
            DrawingGroup drawingGroup = new DrawingGroup();
            DrawingContext drawingContext = drawingGroup.Open();

            DrawingGeometry(drawingContext, value, maxValue, radiusX, radiusY, thickness);

            DrawingBrush brush = new DrawingBrush(drawingGroup);

            return brush;
        }

    }

}
