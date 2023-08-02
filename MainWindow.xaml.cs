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
    public partial class MainWindow : Window
    {
        readonly ViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel = new ViewModel(this);
        }  

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IsInitialized)
            {
                viewModel.Slider_ValueChanged(sender as Slider);
            }
        }

        private void Slider_MouseMove(object sender, MouseEventArgs e)
        {
            viewModel.Slider_MouseMove(sender as Slider);
        }

        private void MarkerButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.MarkerButtonClick(sender as Button);
        }
    }
}

