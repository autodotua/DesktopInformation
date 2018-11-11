using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace DesktopInformation.UserControls
{
    /// <summary>
    /// CircleProgressBar.xaml 的交互逻辑
    /// </summary>
    public partial class CircleProgressBar : ProgressBar
    {
        public CircleProgressBar()
        {
            InitializeComponent();
        }



        public double InnerR
        {
            get { return (double)GetValue(InnerRProperty); }
            set { SetValue(InnerRProperty, value); }
        }

        // Using a DependencyProperty as the backing store for smallR.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InnerRProperty =
            DependencyProperty.Register("InnerR", typeof(double), typeof(CircleProgressBar), new PropertyMetadata(0d));



    }
    public class CircleProgressCenterConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double Height = System.Convert.ToDouble(values[0]);
            double Width = System.Convert.ToDouble(values[1]);
            return new Point(Width / 2, Height / 2);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CircleProgressRadiusConverter : IValueConverter, IMultiValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double Length = System.Convert.ToDouble(value);
            return Length / 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double Length = System.Convert.ToDouble(values[0]);
            double Padding = System.Convert.ToDouble(values[1]);
            return Length / 2 - Padding;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CircleProgressValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double value;
            double minimum;
            double maximum;
            if (values[0].Equals(DependencyProperty.UnsetValue))
            {
                value = 1;
                minimum = 0;
                maximum = 1;
            }
            else
            {
                value = System.Convert.ToDouble(values[0]);
                minimum = System.Convert.ToDouble(values[1]);
                maximum = System.Convert.ToDouble(values[2]);
            }
            double BigR = Math.Min(System.Convert.ToDouble(values[3]), System.Convert.ToDouble(values[4])) / 2;
            double SmallR = System.Convert.ToDouble(values[5]);

            if (value < maximum)
            {
                PathGeometry PathGeometry = new PathGeometry();
                double Angel = (value - minimum) / (maximum - minimum) * 360D;

                bool isLargeArc = Angel > 180;
                Point centerPoint = new Point(BigR, BigR);
                Point p1 = GetPointByAngel(centerPoint, BigR, 0);
                Point p2 = GetPointByAngel(centerPoint, BigR, Angel);
                Point p3 = GetPointByAngel(centerPoint, SmallR, 0);
                Point p4 = GetPointByAngel(centerPoint, SmallR, Angel);

                PathFigure pathFigure = new PathFigure
                {
                    //IsClosed = true,
                    StartPoint = p1
                };
                pathFigure.Segments.Add(new ArcSegment
                {
                    Point = p2,
                    IsLargeArc = isLargeArc,
                    Size = new Size(BigR, BigR),
                    SweepDirection = SweepDirection.Clockwise
                });
                pathFigure.Segments.Add(new LineSegment { Point = p4 });
                pathFigure.Segments.Add(
                    new ArcSegment
                    {
                        Point = p3,
                        IsLargeArc = isLargeArc,
                        Size = new Size(SmallR, SmallR),
                        SweepDirection = SweepDirection.Counterclockwise
                    });
                pathFigure.Segments.Add(new LineSegment { Point = p1 });
                PathGeometry.Figures.Add(pathFigure);

                //PathGeometry.Figures.Add(new PathFigure()
                //{
                //    StartPoint = new Point(0, 0),
                //    Segments = { new LineSegment { Point = new Point(Width, Height) } },
                //    IsClosed = true
                //});

                return PathGeometry;
            }
            else
            {
                return new GeometryGroup()
                {
                    Children =
                    {
                        new EllipseGeometry(new Point(BigR, BigR), BigR, BigR),
                        new EllipseGeometry(new Point(BigR, BigR), SmallR,  SmallR)
                    }
                };
            }
        }

        private Point GetPointByAngel(Point CenterPoint, double r, double angel)
        {
            return new Point
            {
                X = Math.Sin(angel * Math.PI / 180) * r + CenterPoint.X,
                Y = CenterPoint.Y - Math.Cos(angel * Math.PI / 180) * r
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
