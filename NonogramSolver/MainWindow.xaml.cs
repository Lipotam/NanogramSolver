using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NonogramSolver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            int columns = 5;
            int rows = 10;

            List<SolidColorBrush> colours = new List<SolidColorBrush>
                {
                                Brushes.AliceBlue,
                                Brushes.Aqua,
                                Brushes.Blue
                };

            Random random = new Random();

            for (int i = 0; i < rows; i++)
            {
                GridName.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });
            }

            for (int i = 0; i < columns; i++)
            {
                GridName.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });

                for (int j = 0; j < rows; j++)
                {
                    Rectangle rectangle = new Rectangle { Height = 30, Width = 40, Fill = colours.ToArray()[random.Next(3)] };
                    rectangle.Tag = String.Format("{0} {1}", i, j);
                    rectangle.MouseLeftButtonUp += this.Rectangle_MouseLeftButtonUp;
                    rectangle.MouseRightButtonDown += this.Rectangle_MouseRightButtonUp;
                    Grid.SetColumn(rectangle, i);
                    Grid.SetRow(rectangle, j);
                    GridName.Children.Add(rectangle);
                }
            }
        }

        private void Rectangle_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            ((Rectangle)sender).Fill = Brushes.BlueViolet;
        }

        private void Rectangle_MouseRightButtonUp(object sender, RoutedEventArgs e)
        {
            ((Rectangle)sender).Fill = Brushes.Chartreuse;
        }
    }
}
