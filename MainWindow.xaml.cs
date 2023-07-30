using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MarkerBoard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point startPoint;
        private TranslateTransform translation;

        Color drawingColor = Color.FromRgb(0, 0, 0);
        double drawingSize = 20;


        private Point previousPoint;
        private bool isDrawing;
        public MainWindow()
        {
            InitializeComponent();
            InitializeEraser();
            //DataContext = Resources["ViewModel"];
        }


        private void inkCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentPoint = e.GetPosition(inkCanvas);
                if (!isDrawing)
                {
                    // Начало нового рисунка
                    previousPoint = currentPoint;
                    isDrawing = true;
                }

                // Сглаживание линий
                List<Point> smoothedPoints = SmoothLine(previousPoint, currentPoint);

                // Рисование линии сглаженными точками
                for (int i = 1; i < smoothedPoints.Count; i++)
                {
                    var inkStroke = new System.Windows.Ink.Stroke(
                        new StylusPointCollection(new[]
                        {
                            new StylusPoint(smoothedPoints[i - 1].X, smoothedPoints[i - 1].Y),
                            new StylusPoint(smoothedPoints[i].X, smoothedPoints[i].Y)
                        }));

                    inkStroke.DrawingAttributes.Color = drawingColor;
                    inkStroke.DrawingAttributes.Height = drawingSize;
                    inkStroke.DrawingAttributes.Width = drawingSize;

                    inkCanvas.Strokes.Add(inkStroke);
                }

                // Обновление предыдущей точки
                previousPoint = currentPoint;
            }
            else
            {
                isDrawing = false;
            }
        }

        // Сглаживание линии методом интерполяции средних точек
        private List<Point> SmoothLine(Point startPoint, Point endPoint)
        {
            List<Point> points = new List<Point>();
            points.Add(startPoint);

            double deltaX = endPoint.X - startPoint.X;
            double deltaY = endPoint.Y - startPoint.Y;

            if (System.Math.Abs(deltaX) > System.Math.Abs(deltaY))
            {
                double slope = deltaY / deltaX;
                double step = deltaX > 0 ? 1 : -1;

                for (double x = startPoint.X + step; step > 0 ? x <= endPoint.X : x >= endPoint.X; x += step)
                {
                    double y = startPoint.Y + slope * (x - startPoint.X);
                    points.Add(new Point(x, y));
                }
            }
            else
            {
                double slope = deltaX / deltaY;
                double step = deltaY > 0 ? 1 : -1;

                for (double y = startPoint.Y + step; step > 0 ? y <= endPoint.Y : y >= endPoint.Y; y += step)
                {
                    double x = startPoint.X + slope * (y - startPoint.Y);
                    points.Add(new Point(x, y));
                }
            }

            return points;
        }

        void InitializeEraser()
        {
            translation = new TranslateTransform();
            Eraser.RenderTransform = translation;

            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            var eraser = InstrumentPanel.Children[0] as Image; // Предполагаем, что eraser - первый элемент в canvas

            // Проверяем, что куб был захвачен мышью
            if (eraser.IsMouseCaptured)
            {
                // Вычисляем новые координаты куба на основе смещения мыши
                double x = Mouse.GetPosition(InstrumentPanel).X - startPoint.X;
                double y = Mouse.GetPosition(InstrumentPanel).Y - startPoint.Y;

                // Плавно перемещаем куб к новым координатам
                var animation = new DoubleAnimation(x, new Duration(TimeSpan.FromMilliseconds(65)));
                animation.EasingFunction = new PowerEase();
                translation.BeginAnimation(TranslateTransform.XProperty, animation);

                animation = new DoubleAnimation(y, new Duration(TimeSpan.FromMilliseconds(65)));
                animation.EasingFunction = new PowerEase();
                translation.BeginAnimation(TranslateTransform.YProperty, animation);
            }
        }

        private void Eraser_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var eraser = sender as Image;
            startPoint = e.GetPosition(eraser);
            eraser.CaptureMouse();
            translation.BeginAnimation(TranslateTransform.XProperty, null);
            translation.BeginAnimation(TranslateTransform.YProperty, null);
        }

        private void Eraser_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var eraser = sender as Image;
            eraser.ReleaseMouseCapture();

            // Smoothly return the cube to its original position
            var animation = new DoubleAnimation(0f, new Duration(TimeSpan.FromMilliseconds(600)));
            animation.EasingFunction = new QuarticEase();
            translation.BeginAnimation(TranslateTransform.XProperty, animation);
            translation.BeginAnimation(TranslateTransform.YProperty, animation);
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            drawingSize = slider.Value;
        }

        private void Slider_MouseMove(object sender, MouseEventArgs e)
        {
            var slider = sender as Slider;
            TransformGroup? transformGroup = brushScaleImage.RenderTransform as TransformGroup;
            if (transformGroup != null)
            {
                ScaleTransform scaleTransform = transformGroup.Children.OfType<ScaleTransform>().FirstOrDefault();

                if (scaleTransform != null)
                {
                    scaleTransform.ScaleX = slider.Value / 11;
                    scaleTransform.ScaleY = slider.Value / 11;
                }
            }
        }
    }
}
