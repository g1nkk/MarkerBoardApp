using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using MarkerBoard;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Reflection;
using System.Windows.Media.Media3D;

namespace MarkerBoard
{
    public class ViewModel
    {
        MainWindow mainWindow;
        Animations animations;

        Button currentMarkerSelected;

        private Point startPoint;
        private TranslateTransform translation;


        List<Color> colors = new List<Color>();

        public ICommand CopyToClipboardClick { get; set; }
        public ICommand HideButtonClick { get; set;  }
        public ICommand ShowButtonClick { get; set; }
        public ICommand ClearCanvasClick { get; set; }
        public ICommand MinimizeButtonClick { get; set; }
        public ICommand CloseButtonClick { get; set; }

        public ViewModel(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            animations = new Animations(mainWindow);

            InitializeDefaultComponents();
            InitializeColors();
            InitializeCommands();
            InitializeEraser();
        }

        void InitializeDefaultComponents()
        {
            currentMarkerSelected = mainWindow.defaultMarker;
            mainWindow.defaultMarker.BeginAnimation(MainWindow.MarginProperty, animations.showMarkerAnimation);
            mainWindow.inkCanvas.DefaultDrawingAttributes.Width = 11;
            mainWindow.inkCanvas.DefaultDrawingAttributes.Height = 11;
        }

        public void Slider_ValueChanged(Slider slider)
        {
            mainWindow.inkCanvas.DefaultDrawingAttributes.Width = slider.Value;
            mainWindow.inkCanvas.DefaultDrawingAttributes.Height = slider.Value;

            ShowNotification($"marker thickness: {((int)slider.Value)}");
        }

        public void Slider_MouseMove(Slider slider)
        {
            TransformGroup? transformGroup = mainWindow.brushScaleImage.RenderTransform as TransformGroup;
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

        void InitializeCommands()
        {
            MinimizeButtonClick = new RelayCommand(Minimize);
            CloseButtonClick = new RelayCommand(Close);
            CopyToClipboardClick = new RelayCommand(CopyToClipboard);
            ShowButtonClick = new RelayCommand(ShowButton);
            HideButtonClick = new RelayCommand(HideButton);
            ClearCanvasClick = new RelayCommand(ClearCanvas);
        }

        void InitializeColors()
        {
            colors.Add(Color.FromRgb(91, 192, 248));
            colors.Add(Color.FromRgb(243, 78, 78));
            colors.Add(Color.FromRgb(91, 248, 157));
            colors.Add(Colors.Black);
            colors.Add(Colors.White);
        }

        void InitializeEraser()
        {
            translation = new TranslateTransform();
            mainWindow.Eraser.RenderTransform = translation;

            //CompositionTarget.Rendering += SmoothEraserMove;
        }

        // IMPLEMENTING ERASING GRAB MODE
        /* 
          
        private void SmoothEraserMove(object sender, EventArgs e)
        {
            var eraser = mainWindow.InstrumentPanel.Children[0] as Image; // eraser - first canvas child

            if (eraser != null && eraser.IsMouseCaptured)
            {
                double x = Mouse.GetPosition(mainWindow.InstrumentPanel).X - startPoint.X;
                double y = Mouse.GetPosition(mainWindow.InstrumentPanel).Y - startPoint.Y;

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

        */

        private void Minimize()
        {
            mainWindow.WindowState = WindowState.Minimized;
        }

        private void Close()
        {
            mainWindow.Close();
        }

        void ShowButton()
        {
            mainWindow.InstrumentPanel.Visibility = Visibility.Visible;

            ScaleTransform scaleTransform = FindScaleTransform();

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, animations.defaultShortShowAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, animations.defaultShortShowAnimation);
            mainWindow.InstrumentPanel.BeginAnimation(MainWindow.OpacityProperty, animations.defaultShowAnimation);
            mainWindow.showButton.BeginAnimation(MainWindow.OpacityProperty, animations.showButtonHideAnimation);
        }

        private ScaleTransform FindScaleTransform()
        {
            TransformGroup transformGroup = mainWindow.InstrumentPanel.RenderTransform as TransformGroup;
            if (transformGroup != null)
            {
                foreach (Transform transform in transformGroup.Children)
                {
                    if (transform is ScaleTransform scaleTransform)
                    {
                        return scaleTransform;
                    }
                }
            }

            // Если ScaleTransform не найден, создаем новый и добавляем его в TransformGroup
            ScaleTransform newScaleTransform = new ScaleTransform();
            transformGroup.Children.Add(newScaleTransform);

            return newScaleTransform;
        }

        void HideButton()
        {
            ScaleTransform scaleTransform = FindScaleTransform();

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, animations.defaultHideAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, animations.defaultHideAnimation);
            mainWindow.InstrumentPanel.BeginAnimation(MainWindow.OpacityProperty, animations.InstrumentPanelHideAnimation);
            mainWindow.showButton.Visibility = Visibility.Visible;
            mainWindow.showButton.BeginAnimation(MainWindow.OpacityProperty, animations.showButtonShowAnimation);
        }

        void CopyToClipboard()
        {
            int width = (int)mainWindow.inkCanvas.ActualWidth;
            int height = (int)mainWindow.inkCanvas.ActualHeight;

            RenderTargetBitmap rtb = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);

            rtb.Render(mainWindow.inkCanvas);

            Clipboard.SetImage(rtb);

            ShowNotification("Copied to Clipboard!");
        }

        void ShowNotification(string message)
        {
            mainWindow.NotificationsPanel.Visibility = Visibility.Visible;
            mainWindow.NotificationsText.Text = message;
            mainWindow.NotificationsPanel.BeginAnimation(MainWindow.OpacityProperty, animations.showNotificationPanelAnimation);
        }

        private void ClearCanvas()
        {
            mainWindow.inkCanvas.BeginAnimation(MainWindow.OpacityProperty, animations.hideCanvasAnimation);
            ShowNotification("Canvas Cleaned!");
        }

        public void MarkerButtonClick(Button button)
        {
            int buttonTag = Convert.ToInt32(button.Tag);


            currentMarkerSelected.BeginAnimation(MainWindow.MarginProperty, animations.hideMarkerAnimation);

            if (buttonTag == 4) //eraser
            {
                mainWindow.inkCanvas.UseCustomCursor = true;
                mainWindow.inkCanvas.Cursor = Cursors.Cross;
            }
            else mainWindow.inkCanvas.UseCustomCursor = false;

            currentMarkerSelected = button;

            currentMarkerSelected.BeginAnimation(MainWindow.MarginProperty, animations.showMarkerAnimation);

            mainWindow.inkCanvas.DefaultDrawingAttributes.Color = colors[buttonTag];
        }
    }
}
