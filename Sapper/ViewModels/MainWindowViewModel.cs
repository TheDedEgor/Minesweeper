using System.Windows;
using Sapper.ViewModels.Base;
using System.Windows.Input;
using Sapper.Infrastructure.Commands;
using System.Windows.Controls;
using System.Windows.Media;
using Sapper.Models;
using System.Windows.Media.Imaging;
using System;
using System.Windows.Data;

namespace Sapper.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Fields

        private SapperField Sapper;
        private Button[,] buttons = new Button[9, 9];
        private Label[,] labels = new Label[9, 9];

        #endregion

        #region Properties

        private Grid _grid = new Grid();
        public Grid Grid
        {
            get => _grid;
            set => Set(ref _grid, value);
        }

        #endregion

        #region Commands

        public ICommand ClickButtonCommand { get; }

        private bool CanClickButtonCommandExecute(object p) => true;

        private void OnClickButtonCommandExecuted(object p)
        {
            var button = (Button)p;
            var cords = button.Uid.Split('-');
            int idx = int.Parse(cords[0]);
            int jdx = int.Parse(cords[1]);
            buttons[idx, jdx].Visibility = Visibility.Collapsed;
            if (Sapper.Field[idx, jdx].Value == -1) 
            {
                labels[idx,jdx].Background = new SolidColorBrush(Color.FromArgb(255, 209, 99, 99));
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (labels[i, j].Uid == "mine")
                            buttons[i, j].Visibility = Visibility.Collapsed;
                    }
                }
                MessageBox.Show("Вы проиграли!");
            }
            else if(Sapper.Field[idx, jdx].Value == 0)
            {
                Recurse(idx, jdx);
            }
        }

        private void Recurse(int idx, int jdx)
        {
            for (int i = idx - 1; i < idx + 2; i++)
            {
                for (int j = jdx - 1; j < jdx + 2; j++)
                {
                    try
                    {
                        if (buttons[i, j].Visibility == Visibility.Collapsed)
                            continue;
                        if (Sapper.Field[i, j].Value == 0)
                        {
                            buttons[i, j].Visibility = Visibility.Collapsed;
                            Recurse(i, j);
                        }
                        else
                        {
                            buttons[i, j].Visibility = Visibility.Collapsed;
                            continue;
                        }
                    }
                    catch { }
                }
            }
        }

        public ICommand CreateSapperFieldCommand { get; }

        private bool CanCreateSapperFieldCommandExecute(object p) => true;

        private void OnCreateSapperFieldCommandExecuted(object p)
        {
            Sapper = new SapperField(Difficulty.Beginner);
            var image = new BitmapImage(new Uri("Data/Images/mine.png", UriKind.Relative));
            for (int i = 0; i < 9; i++)
            {
                _grid.RowDefinitions.Add(new RowDefinition());
                _grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Label label = new Label();
                    label.Content = Sapper.Field[i, j].Value;
                    label.FontWeight = FontWeights.Bold;
                    label.HorizontalContentAlignment = HorizontalAlignment.Center;
                    label.VerticalContentAlignment = VerticalAlignment.Center;
                    label.FontSize = 20;
                    label.Foreground = new SolidColorBrush(Color.FromArgb(255, 24, 29, 237));
                    label.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 68, 64, 71));
                    label.BorderThickness = new Thickness(0.5);
                    label.Background = new SolidColorBrush(Color.FromArgb(255, 197, 197, 204));
                    if (Sapper.Field[i, j].Value == -1)
                    {
                        Image img = new Image();
                        img.Source = image;
                        label.Content = img;
                        label.Uid = $"mine";
                    }
                    else if (Sapper.Field[i, j].Value == 0)
                    {
                        label.Content = null;
                    }
                    else if (Sapper.Field[i, j].Value == 1)
                    {
                        label.Foreground = new SolidColorBrush(Color.FromArgb(255, 24, 29, 237));
                    }
                    else if (Sapper.Field[i, j].Value == 2)
                    {
                        label.Foreground = new SolidColorBrush(Color.FromArgb(255, 62, 151, 30));
                    }
                    else if (Sapper.Field[i, j].Value == 3)
                    {
                        label.Foreground = new SolidColorBrush(Color.FromArgb(255, 215, 22, 50));
                    }
                    else if (Sapper.Field[i, j].Value == 4)
                    {
                        label.Foreground = new SolidColorBrush(Color.FromArgb(255, 22, 41, 164));
                    }
                    else if (Sapper.Field[i, j].Value == 5)
                    {
                        label.Foreground = new SolidColorBrush(Color.FromArgb(255, 138, 12, 12));
                    }
                    Grid.SetRow(label, i);
                    Grid.SetColumn(label, j);
                    labels[i,j] = label;
                    _grid.Children.Add(label);
                    Button bt = new Button();
                    bt.Uid = $"{i}-{j}";
                    //bt.DataContext = this;
                    //Binding binding = new Binding() { Path = new PropertyPath("Visibility"), Mode = BindingMode.OneWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                    //bt.SetBinding(Button.VisibilityProperty, binding);
                    bt.Command = ClickButtonCommand;
                    bt.CommandParameter = bt;
                    bt.Background = new SolidColorBrush(Color.FromArgb(255, 184, 184, 190));
                    bt.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 68, 64, 71));
                    Grid.SetRow(bt, i);
                    Grid.SetColumn(bt, j);
                    buttons[i,j] = bt;
                    _grid.Children.Add(bt);
                }
            }
        }

        #endregion

        public MainWindowViewModel()
        {
            #region Commands

            CreateSapperFieldCommand = new LambdaCommand(OnCreateSapperFieldCommandExecuted,CanCreateSapperFieldCommandExecute);
            ClickButtonCommand = new LambdaCommand(OnClickButtonCommandExecuted, CanClickButtonCommandExecute);

            #endregion
        }
    }
}
