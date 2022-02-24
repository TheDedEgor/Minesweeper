using System.Windows;
using Sapper.ViewModels.Base;
using System.Windows.Input;
using Sapper.Infrastructure.Commands;
using System.Windows.Controls;
using System.Windows.Media;
using Sapper.Models;
using System.Windows.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sapper.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Fields

        private SapperField Sapper = new(Difficulty.Beginner);

        private int CountCloseCells;

        #endregion

        #region Properties

        private CellVM[,] _cells;

        public IEnumerable<CellVM> AllCells => _cells.Cast<CellVM>();

        private double _minHeightWindow = 600;

        public double MinHeightWindow
        {
            get => _minHeightWindow;
            set => Set(ref _minHeightWindow, value);
        }

        private double _minWidthWindow = 600;

        public double MinWidthWindow
        {
            get => _minWidthWindow;
            set => Set(ref _minWidthWindow, value);
        }

        private double _heightWindow = 600;

        public double HeightWindow
        {
            get => _heightWindow;
            set => Set(ref _heightWindow, value);
        }

        private double _widthWindow = 600;

        public double WidthWindow
        {
            get => _widthWindow;
            set => Set(ref _widthWindow, value);
        }

        #endregion

        #region Commands

        //public ICommand ReplayGameCommand { get; }

        //private bool CanReplayGameCommandExecute(object p) => true;

        //private void OnReplayGameCommandExecuted(object p)
        //{
        //    _grid.Children.Clear();
        //    _grid.RowDefinitions.Clear();
        //    _grid.ColumnDefinitions.Clear();
        //}

        public ICommand CloseAppCommand { get; }

        private bool CanCloseAppCommandExecute(object p) => true;

        private void OnCloseAppCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }

        #region ClickLabelCommand

        private bool CanClickLabelCommandExecute(object p) => true;

        private void OnClickLabelCommandExecuted(object p)
        {
            var label = (Label)p;
            var cords = label.Uid.Split('-');
            int idx = int.Parse(cords[0]);
            int jdx = int.Parse(cords[1]);
            sbyte count = 0;
            for (int i = idx - 1; i < idx + 2; i++)
            {
                for (int j = jdx - 1; j < jdx + 2; j++)
                {
                    try
                    {
                        if (_cells[i,j].IsFlag)
                            count++;
                    }
                    catch { }
                }
            }
            if (count == Sapper.Field[idx, jdx])
            {
                for (int i = idx - 1; i < idx + 2; i++)
                {
                    for (int j = jdx - 1; j < jdx + 2; j++)
                    {
                        try
                        {
                            if (_cells[i, j].IsFlag)
                                continue;
                            else
                            {
                                if (_cells[i, j].Uid.Contains("mine"))
                                {
                                    _cells[i, j].Visibility = Visibility.Collapsed;
                                    _cells[i, j].Background = new SolidColorBrush(Color.FromArgb(255, 209, 99, 99));
                                    ShowAllMines();
                                    //_grid.Children.Add(CreateBlockedGrid());
                                }
                                else if (Sapper.Field[i, j] == 0)
                                {
                                    _cells[i, j].Visibility = Visibility.Collapsed;
                                    CountCloseCells--;
                                    OpeningRecursion(i, j);
                                }
                                else
                                {
                                    _cells[i, j].Visibility = Visibility.Collapsed;
                                    CountCloseCells--;
                                    continue;
                                }
                            }
                        }
                        catch { }
                    }
                }
            }
        }

        #endregion

        #region SetFlagCommand

        private bool CanSetFlagCommandExecute(object p) => true;

        private void OnSetFlagCommandExecuted(object p)
        {
            var border = (Border)p;
            var cords = border.Uid.Split('-');
            int idx = int.Parse(cords[0]);
            int jdx = int.Parse(cords[1]);
            if (!_cells[idx,jdx].IsFlag)
            {
                Image flagImage = new Image() { Source = new BitmapImage(new Uri("Data/Images/flag.png", UriKind.Relative)) };
                border.Child = flagImage;
                _cells[idx, jdx].IsFlag = true;
            }
            else
            {
                border.Child = null;
                _cells[idx, jdx].IsFlag = false;
            }
        }

        #endregion

        #region ClickBorderCommand

        private bool CanClickBorderCommandExecute(object p) => true;

        private void OnClickBorderCommandExecuted(object p)
        {
            var border = (Border)p;
            if (border.Child == null)
            {
                var cords = border.Uid.Split('-');
                int idx = int.Parse(cords[0]);
                int jdx = int.Parse(cords[1]);
                _cells[idx, jdx].Visibility = Visibility.Collapsed;
                CountCloseCells--;
                if (Sapper.Field[idx, jdx] == -1)
                {
                    _cells[idx, jdx].Background = new SolidColorBrush(Color.FromArgb(255, 209, 99, 99));
                    ShowAllMines();
                    //_grid.Children.Add(CreateBlockedGrid());
                }
                else if (Sapper.Field[idx, jdx] == 0)
                {
                    OpeningRecursion(idx, jdx);
                }
                //if (CountCloseCells == 0)
                //    _grid.Children.Add(CreateBlockedGrid());
            }
        }

        #endregion

        //public ICommand CreateSapperFieldCommand { get; }

        //private bool CanCreateSapperFieldCommandExecute(object p) => true;

        //private void OnCreateSapperFieldCommandExecuted(object p)
        //{
        //    if (_grid.ColumnDefinitions.Count == 0)
        //    {
        //        var difficult = (string)p;
        //        if (difficult == "Beginner")
        //        {
        //            HeightWindow = 600;
        //            WidthWindow = 600;
        //            MinHeightWindow = 600;
        //            MinWidthWindow = 600;
        //            Sapper = new SapperField(Difficulty.Beginner);
        //        }
        //        else if (difficult == "Amateur")
        //        {
        //            HeightWindow = 700;
        //            WidthWindow = 800;
        //            MinHeightWindow = 700;
        //            MinWidthWindow = 800;
        //            Sapper = new SapperField(Difficulty.Amateur);
        //        }
        //        else
        //        {
        //            HeightWindow = 750;
        //            WidthWindow = 1100;
        //            MinHeightWindow = 750;
        //            MinWidthWindow = 1100;
        //            Sapper = new SapperField(Difficulty.Professional);
        //        }
        //        CreateGridSapper();
        //        FillingGridSapper();
        //    }
        //    else
        //        MessageBox.Show("Игра уже запущена!");
            
        //}

        #endregion

        #region Methods

        private void ShowAllMines()
        {
            for (int i = 0; i < Sapper.Field.GetLength(0); i++)
            {
                for (int j = 0; j < Sapper.Field.GetLength(1); j++)
                {
                    if (_cells[i, j].Uid.Contains("mine"))
                        _cells[i, j].Visibility = Visibility.Collapsed;
                }
            }
        }

        private void GenerateCells()
        {
            var cells = new CellVM[9, 9];
            var image = new BitmapImage(new Uri("Data/Images/mine.png", UriKind.Relative));
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    cells[i, j] = new CellVM(Sapper.Field[i, j], Visibility.Visible, $"{i}-{j}", null, new SolidColorBrush(Color.FromArgb(255, 198, 198, 198)), OnSetFlagCommandExecuted, CanSetFlagCommandExecute, OnClickBorderCommandExecuted, CanClickBorderCommandExecute, OnClickLabelCommandExecuted, CanClickLabelCommandExecute);
                    if (Sapper.Field[i, j] == -1)
                    {
                        Image img = new Image() { Source = image };
                        cells[i, j].Value = img;
                        cells[i, j].Uid = $"{i}-{j}-mine";
                    }
                    else if (Sapper.Field[i, j] == 0)
                    {
                        cells[i, j].Value = null;
                    }
                    else if (Sapper.Field[i, j] == 1)
                    {
                        cells[i, j].Foreground = new SolidColorBrush(Color.FromArgb(255, 24, 29, 237));
                    }
                    else if (Sapper.Field[i, j] == 2)
                    {
                        cells[i, j].Foreground = new SolidColorBrush(Color.FromArgb(255, 62, 151, 30));
                    }
                    else if (Sapper.Field[i, j] == 3)
                    {
                        cells[i, j].Foreground = new SolidColorBrush(Color.FromArgb(255, 215, 22, 50));
                    }
                    else if (Sapper.Field[i, j] == 4)
                    {
                        cells[i, j].Foreground = new SolidColorBrush(Color.FromArgb(255, 22, 41, 164));
                    }
                    else if (Sapper.Field[i, j] == 5)
                    {
                        cells[i, j].Foreground = new SolidColorBrush(Color.FromArgb(255, 138, 12, 12));
                    }
                    else
                    {
                        cells[i, j].Foreground = new SolidColorBrush(Color.FromArgb(255, 250, 10, 10));
                    }
                }
            }
            this._cells = cells;
            
        }

        //private Grid CreateBlockedGrid()
        //{
        //    Grid blockGrid = new Grid();
        //    blockGrid.RowDefinitions.Add(new RowDefinition());
        //    blockGrid.RowDefinitions.Add(new RowDefinition());
        //    blockGrid.ColumnDefinitions.Add(new ColumnDefinition());
        //    blockGrid.ColumnDefinitions.Add(new ColumnDefinition());
        //    blockGrid.Background = new SolidColorBrush(Color.FromArgb(125, 108, 99, 99));
        //    Grid.SetColumnSpan(blockGrid, 50);
        //    Grid.SetRowSpan(blockGrid, 50);
        //    TextBlock text = new TextBlock();
        //    text.Text = "You've lost!";
        //    text.HorizontalAlignment = HorizontalAlignment.Center;
        //    text.VerticalAlignment = VerticalAlignment.Bottom;
        //    text.FontSize = 40;
        //    text.FontWeight = FontWeights.Bold;
        //    Grid.SetRow(text, 0);
        //    Grid.SetColumnSpan(text, 2);
        //    Button button = new Button();
        //    button.Content = "Replay";
        //    button.Command = ReplayGameCommand;
        //    button.Width = 110;
        //    button.Height = 36;
        //    button.Margin = new Thickness(5);
        //    button.Background = new SolidColorBrush(Color.FromArgb(255, 90, 170, 180));
        //    button.BorderThickness = new Thickness(3);
        //    button.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 139, 25, 224));
        //    button.FontFamily = new FontFamily("Book Antiqua");
        //    button.FontSize = 20;
        //    button.VerticalAlignment = VerticalAlignment.Top;
        //    button.HorizontalAlignment = HorizontalAlignment.Right;
        //    Grid.SetRow(button, 1);
        //    Grid.SetColumn(button, 0);  
        //    if(CountCloseCells == 0)
        //    {
        //        button.Content = "New Game";
        //        text.Text = "You won!";
        //    }
        //    blockGrid.Children.Add(text);
        //    blockGrid.Children.Add(button);
        //    button = new Button();
        //    button.Content = "Exit";
        //    button.Command = CloseAppCommand;
        //    button.Width = 110;
        //    button.Height = 36;
        //    button.Margin = new Thickness(5);
        //    button.Background = new SolidColorBrush(Color.FromArgb(255, 90, 170, 180));
        //    button.BorderThickness = new Thickness(3);
        //    button.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 139, 25, 224));
        //    button.FontFamily = new FontFamily("Book Antiqua");
        //    button.FontSize = 20;
        //    button.VerticalAlignment = VerticalAlignment.Top;
        //    button.HorizontalAlignment = HorizontalAlignment.Left;
        //    Grid.SetRow(button, 1);
        //    Grid.SetColumn(button, 1);
        //    blockGrid.Children.Add(button);
        //    return blockGrid;
        //}

        private void OpeningRecursion(int idx, int jdx)
        {
            for (int i = idx - 1; i < idx + 2; i++)
            {
                for (int j = jdx - 1; j < jdx + 2; j++)
                {
                    try
                    {
                        if (_cells[i, j].Visibility == Visibility.Collapsed)
                            continue;
                        if (Sapper.Field[i, j] == 0)
                        {
                            _cells[i, j].Visibility = Visibility.Collapsed;
                            CountCloseCells--;
                            OpeningRecursion(i, j);
                        }
                        else
                        {
                            _cells[i, j].Visibility = Visibility.Collapsed;
                            CountCloseCells--;
                            continue;
                        }
                    }
                    catch { }
                }
            }
        }

        #endregion

        public MainWindowViewModel()
        {
            GenerateCells();

            #region Commands

            CloseAppCommand = new LambdaCommand(OnCloseAppCommandExecuted, CanCloseAppCommandExecute);

            #endregion
        }
    }
}
