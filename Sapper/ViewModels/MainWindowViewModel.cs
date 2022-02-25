using System;
using System.Windows;
using Sapper.ViewModels.Base;
using System.Windows.Input;
using Sapper.Infrastructure.Commands;
using System.Windows.Controls;
using System.Windows.Media;
using Sapper.Models;
using System.Windows.Media.Imaging;
using Sapper.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;

namespace Sapper.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Fields

        private SapperField Sapper = new(Difficulty.Beginner);

        private bool GameIsOver = false;

        #endregion

        #region Properties

        private BitmapImage _sourceImage = new BitmapImage(new Uri("Data/Images/sm.png", UriKind.Relative));

        public BitmapImage SourceImage
        {
            get => _sourceImage;
            set => Set(ref _sourceImage, value);
        }

        #region AllCells

        private CellVM[,] _cells;

        private ObservableCollection<CellVM> _allCells;

        public ObservableCollection<CellVM> AllCells
        {
            get => _allCells;
            set => Set(ref _allCells, value);
        }

        #endregion

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

        public ICommand ReplayGameCommand { get; }

        private bool CanReplayGameCommandExecute(object p) => true;

        private void OnReplayGameCommandExecuted(object p)
        {
            AllCells.Clear();
            Sapper = new(Difficulty.Beginner);
            GameIsOver = false;
            GenerateCells();
            SourceImage = new BitmapImage(new Uri("Data/Images/sm.png", UriKind.Relative));
        }

        #region CloseAppCommand
        public ICommand CloseAppCommand { get; }

        private bool CanCloseAppCommandExecute(object p) => true;

        private void OnCloseAppCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region ClickLabelCommand

        private bool CanClickLabelCommandExecute(object p) => !GameIsOver;

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
                                    SourceImage = new BitmapImage(new Uri("Data/Images/dead.png", UriKind.Relative));
                                    GameIsOver = true;
                                }
                                else if (Sapper.Field[i, j] == 0)
                                {
                                    _cells[i, j].Visibility = Visibility.Collapsed;
                                    OpeningCellsRecursion(i, j);
                                }
                                else
                                {
                                    _cells[i, j].Visibility = Visibility.Collapsed;
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

        private bool CanSetFlagCommandExecute(object p) => !GameIsOver;

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

        private bool CanClickBorderCommandExecute(object p) => !GameIsOver;

        private void OnClickBorderCommandExecuted(object p)
        {
            var border = (Border)p;
            if (border.Child == null)
            {
                var cords = border.Uid.Split('-');
                int idx = int.Parse(cords[0]);
                int jdx = int.Parse(cords[1]);
                _cells[idx, jdx].Visibility = Visibility.Collapsed;
                if (Sapper.Field[idx, jdx] == -1)
                {
                    _cells[idx, jdx].Background = new SolidColorBrush(Color.FromArgb(255, 209, 99, 99));
                    ShowAllMines();
                    SourceImage = new BitmapImage(new Uri("Data/Images/dead.png", UriKind.Relative));
                    GameIsOver = true;
                }
                else if (Sapper.Field[idx, jdx] == 0)
                {
                    OpeningCellsRecursion(idx, jdx);
                }
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
            AllCells = _cells.Cast<CellVM>().ToObservable();
        }

        private void OpeningCellsRecursion(int idx, int jdx)
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
                            OpeningCellsRecursion(i, j);
                        }
                        else
                        {
                            _cells[i, j].Visibility = Visibility.Collapsed;
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
            ReplayGameCommand = new LambdaCommand(OnReplayGameCommandExecuted, CanReplayGameCommandExecute);

            #endregion
        }
    }
}
