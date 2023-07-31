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

        DoubleAnimation showAnimation;
        DoubleAnimation hideAnimation;

        List<Color> colors = new List<Color>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeEraser();
            InitializeColors();
            InitializeAnimations();
            //DataContext = Resources["ViewModel"];
        }

        void InitializeAnimations()
        {
            showAnimation = new DoubleAnimation(1f, new Duration(TimeSpan.FromMilliseconds(100)));
            hideAnimation = new DoubleAnimation(0f, new Duration(TimeSpan.FromMilliseconds(250)));
            hideAnimation.Completed += (sender, e) =>
            {
                inkCanvas.Strokes.Clear();
                inkCanvas.BeginAnimation(OpacityProperty, showAnimation);
            };
            hideAnimation.EasingFunction = new PowerEase();
        }

        void InitializeColors()
        {
            colors.Add(Color.FromRgb(91, 192, 248));
            colors.Add(Color.FromRgb(243, 78, 78));
            colors.Add(Color.FromRgb(91, 248, 157));
            colors.Add(Colors.Black);
        }

        void InitializeEraser()
        {
            translation = new TranslateTransform();
            Eraser.RenderTransform = translation;

            CompositionTarget.Rendering += SmoothEraserMove;
        }

        private void SmoothEraserMove(object sender, EventArgs e)
        {
            var eraser = InstrumentPanel.Children[0] as Image; // eraser - first canvas child

            if (eraser != null && eraser.IsMouseCaptured)
            {
                double x = Mouse.GetPosition(InstrumentPanel).X - startPoint.X;
                double y = Mouse.GetPosition(InstrumentPanel).Y - startPoint.Y;

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
            inkCanvas.DefaultDrawingAttributes.Width = slider.Value;
            inkCanvas.DefaultDrawingAttributes.Height = slider.Value;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int buttonTag = Convert.ToInt32(button.Tag);

            inkCanvas.DefaultDrawingAttributes.Color = colors[buttonTag];
        }

        private void CopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            int width = (int)inkCanvas.ActualWidth;
            int height = (int)inkCanvas.ActualHeight;

            RenderTargetBitmap rtb = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);

            rtb.Render(inkCanvas);

            Clipboard.SetImage(rtb);
        }

        private void ClearCanvas_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.BeginAnimation(OpacityProperty, hideAnimation);
        }
    }
}

