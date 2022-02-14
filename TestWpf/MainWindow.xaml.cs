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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sapper;
using Sapper.Models;

namespace TestWpf
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SapperField sapper = new SapperField(Difficulty.Beginner);
            Grid grid = new Grid();
            var image = new BitmapImage(new Uri("Data/Images/mine.png", UriKind.Relative));
            Button[,] buttons = new Button[9,9];
            Label[,] labels = new Label[9,9];
            for (int i = 0; i < 9; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            Grid.SetRow(grid, 1);
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Label label = new Label();
                    label.Content = sapper.Field[i, j].Value;
                    label.FontWeight = FontWeights.Bold;
                    label.HorizontalContentAlignment = HorizontalAlignment.Center;
                    label.VerticalContentAlignment = VerticalAlignment.Center;
                    label.FontSize = 20;
                    label.Foreground = new SolidColorBrush(Color.FromArgb(255, 24, 29, 237));
                    label.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 68, 64, 71));
                    label.BorderThickness = new Thickness(0.5);
                    label.Background = new SolidColorBrush(Color.FromArgb(255, 197, 197, 204));
                    if (sapper.Field[i, j].Value == -1)
                    {
                        Image img = new Image();
                        img.Source = image;
                        label.Content = img;
                    }
                    else if (sapper.Field[i, j].Value == 0)
                    {
                        label.Content = null;
                    }
                    else if (sapper.Field[i, j].Value == 1)
                    {
                        label.Foreground = new SolidColorBrush(Color.FromArgb(255, 24, 29, 237));
                    }
                    else if (sapper.Field[i, j].Value == 2)
                    {
                        label.Foreground = new SolidColorBrush(Color.FromArgb(255, 62, 151, 30));
                    }
                    else if (sapper.Field[i, j].Value == 3)
                    {
                        label.Foreground = new SolidColorBrush(Color.FromArgb(255, 215, 22, 50));
                    }
                    else if (sapper.Field[i, j].Value == 4)
                    {
                        label.Foreground = new SolidColorBrush(Color.FromArgb(255, 22, 41, 164));
                    }
                    else if (sapper.Field[i, j].Value == 5)
                    {
                        label.Foreground = new SolidColorBrush(Color.FromArgb(255, 138, 12, 12));
                    }
                    Grid.SetRow(label, i);
                    Grid.SetColumn(label, j);
                    labels[i, j] = label;
                    grid.Children.Add(label);
                    Button bt = new Button();
                    bt.Background = new SolidColorBrush(Color.FromArgb(255, 184, 184, 190));
                    bt.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 68, 64, 71));
                    Grid.SetRow(bt, i);
                    Grid.SetColumn(bt, j);
                    bt.Click += Bt_Click;
                    buttons[i,j] = bt;
                    grid.Children.Add(bt);
                }
            }
            table.Children.Add(grid);
        }
        private void Bt_Click(object sender, RoutedEventArgs e)
        {
            var bt = (Button)sender;
            bt.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
