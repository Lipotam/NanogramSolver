using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;
using Microsoft.Win32;
using NonogramSolver.Models;
using NonogramSolver.Core;

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

        private CrosswordData crosswordData;

        private void Rectangle_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            ((Rectangle)sender).Fill = Brushes.BlueViolet;
        }

        private void Rectangle_MouseRightButtonUp(object sender, RoutedEventArgs e)
        {
            ((Rectangle)sender).Fill = Brushes.Chartreuse;
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents (.txt)|*.txt";

            if (dlg.ShowDialog() == true)
            {
                using (TextReader reader = new StreamReader(dlg.OpenFile()))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(CrosswordData));
                    crosswordData = (CrosswordData)serializer.Deserialize(reader);
                }
            }
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //TODO remove MakeFakeCrossword()
            MakeFakeCrossword();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text documents (.txt)|*.txt";
            saveFileDialog.FileName = "result";
            saveFileDialog.DefaultExt = ".txt";
            if (saveFileDialog.ShowDialog() == true)
            {
                using (TextWriter textWriter = new StreamWriter(saveFileDialog.FileName))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(CrosswordData));
                    serializer.Serialize(textWriter, crosswordData);
                    textWriter.Close();
                }
            }
        }

        private void MakeFakeCrossword()
            {

            crosswordData = GetCrosswordSample_1();
            //crosswordData = new CrosswordData
            //{
            //    FieldHeight = 3,
            //    FieldWidth = 3,
            //    FieldCells = new CellState[3][],
            //    TopPanelLines = new[] { new PanelLine(), new PanelLine(), new PanelLine(), },
            //    LeftPanelLines = new[] { new PanelLine(), new PanelLine(), new PanelLine(), },
            //};
            //for (int i = 0; i < 2; i++)
            //{
            //    crosswordData.FieldCells[i] = new[] { CellState.Empty, CellState.Empty, CellState.Empty };
            //}
            }

        private void DoTheJob_Click(object sender, RoutedEventArgs e)
            {
            //crosswordData = GetCrosswordSample_1();

            NonogramResolver solver = new NonogramResolver(crosswordData);
            solver.StartResolving();

            int columns = crosswordData.FieldWidth;
            int rows = crosswordData.FieldHeight;

            for (int i = 0; i < rows; i++)
            {
                GridName.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });
            }

            for (int i = 0; i < columns; i++)
            {
                GridName.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });

                for (int j = 0; j < rows; j++)
                {
                    Rectangle rectangle = new Rectangle { Height = 10, Width = 10 };

                    var fieldCells = this.crosswordData.FieldCells;
                    if (fieldCells != null)
                        switch (fieldCells[i][j])
                        {
                            case CellState.Undefined:
                                rectangle.Fill = Brushes.Blue;
                                break;
                            case CellState.Empty:
                                rectangle.Fill = Brushes.White;
                                break;
                            case CellState.Filled:
                                rectangle.Fill = Brushes.Black;
                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    rectangle.Tag = String.Format("{0} {1}", i, j);
                    rectangle.MouseLeftButtonUp += this.Rectangle_MouseLeftButtonUp;
                    rectangle.MouseRightButtonDown += this.Rectangle_MouseRightButtonUp;
                    Grid.SetColumn(rectangle, i);
                    Grid.SetRow(rectangle, j);
                    GridName.Children.Add(rectangle);
                }
            }
        }


        private CrosswordData GetCrosswordSample_1()
        {
            crosswordData = new CrosswordData
            {
                FieldHeight = 3,
                FieldWidth = 4,
                TopPanelLines = new[] { new PanelLine { LineValues = new List<int> { 2 } }, new PanelLine { LineValues = new List<int> { 1 } }, new PanelLine { LineValues = new List<int> { 1 } }, new PanelLine { LineValues = new List<int> { 1 } } },
                LeftPanelLines = new[] { new PanelLine { LineValues = new List<int> { 2, 1 } }, new PanelLine { LineValues = new List<int> { 1 } }, new PanelLine { LineValues = new List<int> { 1 } } },
            };

            return crosswordData;
        }
    }
}
