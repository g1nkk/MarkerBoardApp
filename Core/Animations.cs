using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Animation;

namespace MarkerBoard
{
    public class Animations
    {
        public ThicknessAnimation showMarkerAnimation;

        public ThicknessAnimation hideMarkerAnimation;

        public DoubleAnimation hideNotificationAnimation;

        public DoubleAnimation showNotificationPanelAnimation;

        public DoubleAnimation showCanvasAnimation;
        public DoubleAnimation hideCanvasAnimation;

        public DoubleAnimation defaultHideAnimation;
        public DoubleAnimation InstrumentPanelHideAnimation;
        public DoubleAnimation defaultShowAnimation;
        public DoubleAnimation defaultShortShowAnimation;

        public DoubleAnimation showButtonHideAnimation;
        public DoubleAnimation showButtonShowAnimation;


        public Animations(MainWindow mainWindow)
        {
            showMarkerAnimation = new ThicknessAnimation
            {
                To = new Thickness(50, 5, 5, 5),
                Duration = TimeSpan.FromMilliseconds(200),
                EasingFunction = new PowerEase()
            };
            hideMarkerAnimation = new ThicknessAnimation
            {
                To = new Thickness(0, 5, 5, 5),
                Duration = TimeSpan.FromMilliseconds(200),
                EasingFunction = new PowerEase()
            };

            defaultShowAnimation = new DoubleAnimation
            {
                To = 1,
                Duration = TimeSpan.FromMilliseconds(750),
                EasingFunction = new PowerEase()
            };
            defaultShortShowAnimation = new DoubleAnimation
            {
                To = 1,
                Duration = TimeSpan.FromMilliseconds(350),
                EasingFunction = new PowerEase()
            };
            defaultHideAnimation = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromMilliseconds(1100),
                EasingFunction = new PowerEase()
            };
            showButtonShowAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(2200),
                EasingFunction = new PowerEase()
            };
            showButtonHideAnimation = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromMilliseconds(150),
                EasingFunction = new PowerEase()
            };
            showButtonHideAnimation.Completed += (sender, e) =>
            {
                mainWindow.showButton.Visibility = Visibility.Collapsed;
            };
            InstrumentPanelHideAnimation = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromMilliseconds(200),
                EasingFunction = new PowerEase()
            };
            InstrumentPanelHideAnimation.Completed += (sender, e) =>
            {
                mainWindow.InstrumentPanel.Visibility = Visibility.Collapsed;
            };

            hideNotificationAnimation = new DoubleAnimation(0, new Duration(TimeSpan.FromMilliseconds(1300)));
            hideNotificationAnimation.EasingFunction = new QuadraticEase();
            hideNotificationAnimation.Completed += (sender, e) =>
            {
                if (mainWindow.NotificationsPanel.Opacity == 0)
                {
                    mainWindow.NotificationsPanel.Visibility = Visibility.Collapsed;
                }
            };

            showNotificationPanelAnimation = new DoubleAnimation(1, new Duration(TimeSpan.FromMilliseconds(400)));
            showNotificationPanelAnimation.EasingFunction = new PowerEase();
            showNotificationPanelAnimation.Completed += (sender, e) =>
            {
                mainWindow.NotificationsPanel.BeginAnimation(MainWindow.OpacityProperty, hideNotificationAnimation);
            };

            showCanvasAnimation = new DoubleAnimation(1, new Duration(TimeSpan.FromMilliseconds(50)));

            hideCanvasAnimation = new DoubleAnimation(0, new Duration(TimeSpan.FromMilliseconds(250)));
            hideCanvasAnimation.EasingFunction = new PowerEase();
            hideCanvasAnimation.Completed += (sender, e) =>
            {
                mainWindow.inkCanvas.Strokes.Clear();
                mainWindow.inkCanvas.BeginAnimation(MainWindow.OpacityProperty, showCanvasAnimation);
            };
        }
    }
}
