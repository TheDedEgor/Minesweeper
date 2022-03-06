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
using Sapper.Common;
using Sapper.Data;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace Sapper.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Static fields

        static double screenHeight = SystemParameters.FullPrimaryScreenHeight;

        static double screenWidth = SystemParameters.FullPrimaryScreenWidth;

        #endregion

        #region Fields

        private SapperField Sapper = new(Difficulty.Beginner);

        private bool GameIsOver = false;

        private bool GameTimerStarted = false;

        private int Time = 0;

        private int Mines = 10;

        private int CountClosedCells = 71;

        private DispatcherTimer GameTimer;

        #endregion

        #region Properties

        #region Timer

        private string _timer = "000";

        public string Timer
        {
            get => _timer;
            set => Set(ref _timer, value);
        }

        #endregion

        #region CounterMines

        private string _counterMines = "010";

        public string CounterMines
        {
            get => _counterMines;
            set => Set(ref _counterMines, value);
        }

        #endregion

        #region MainSmile

        private BitmapImage _sourceImage = SharedUtils.SmileImage;

        public BitmapImage SourceImage
        {
            get => _sourceImage;
            set => Set(ref _sourceImage, value);
        }

        #endregion

        #region AllCells

        private CellVM[,] _cells;

        private ObservableCollection<CellVM> _allCells;

        public ObservableCollection<CellVM> AllCells
        {
            get => _allCells;
            set => Set(ref _allCells, value);
        }

        #endregion

        #region HeightWindow

        private double _heightWindow = 600;

        public double HeightWindow
        {
            get => _heightWindow;
            set => Set(ref _heightWindow, value);
        }

        #endregion

        #region WidthWindow

        private double _widthWindow = 600;

        public double WidthWindow
        {
            get => _widthWindow;
            set => Set(ref _widthWindow, value);
        }

        #endregion

        #region Rows

        private int _rows = 9;

        public int Rows
        {
            get => _rows;
            set => Set(ref _rows, value);
        }

        #endregion

        #region Columns

        private int _columns = 9;

        public int Columns
        {
            get => _columns;
            set => Set(ref _columns, value);
        }

        #endregion

        #endregion

        #region Commands

        #region ChooseBeginnerCommand

        public ICommand ChooseBeginnerCommand { get; }

        private bool CanChooseBeginnerCommandExecute(object p) => true;

        private void OnChooseBeginnerCommandExecuted(object p = null)
        {
            if (GameTimerStarted)
                GameTimer.Stop();
            AllCells.Clear();
            Rows = 9;
            Columns = 9;
            Sapper = new(Difficulty.Beginner);
            GameIsOver = false;
            GameTimerStarted = false;
            Time = 0;
            Mines = 10;
            Timer = "000";
            CounterMines = "010";
            CountClosedCells = 71;
            HeightWindow = 600;
            WidthWindow = 600;
            var window = (Window)p;
            if(window != null)
            {
                window.Top = (screenHeight - window.Height) / 2 + 40;
                window.Left = (screenWidth - window.Width) / 2;
            }
            GenerateCells();
            SourceImage = SharedUtils.SmileImage;
        }

        #endregion

        #region ChooseIntermediateCommand

        public ICommand ChooseIntermediateCommand { get; }

        private bool CanChooseIntermediateCommandExecute(object p) => true;

        private void OnChooseIntermediateCommandExecuted(object p = null)
        {
            if (GameTimerStarted)
                GameTimer.Stop();
            AllCells.Clear();
            Rows = 16;
            Columns = 16;
            Sapper = new(Difficulty.Intermediate);
            GameIsOver = false;
            GameTimerStarted = false;
            Time = 0;
            Mines = 40;
            Timer = "000";
            CounterMines = "040";
            CountClosedCells = 216;
            HeightWindow = 750;
            WidthWindow = 750;
            var window = (Window)p;
            if(window != null)
            {
                window.Top = (screenHeight - window.Height) / 2 + 40;
                window.Left = (screenWidth - window.Width) / 2;
            }
            GenerateCells();
            SourceImage = SharedUtils.SmileImage;
        }

        #endregion

        #region ChooseExpertCommand

        public ICommand ChooseExpertCommand { get; }

        private bool CanChooseExpertCommandExecute(object p) => true;

        private void OnChooseExpertCommandExecuted(object p = null)
        {
            if (GameTimerStarted)
                GameTimer.Stop();
            AllCells.Clear();
            Rows = 16;
            Columns = 30;
            Sapper = new(Difficulty.Expert);
            GameIsOver = false;
            GameTimerStarted = false;
            Time = 0;
            Mines = 99;
            Timer = "000";
            CounterMines = "099";
            CountClosedCells = 381;
            HeightWindow = 750;
            WidthWindow = 1350;
            var window = (Window)p;
            if(window != null)
            {
                window.Top = (screenHeight - window.Height) / 2 + 40;
                window.Left = (screenWidth - window.Width) / 2;
            }
            GenerateCells();
            SourceImage = SharedUtils.SmileImage;
        }

        #endregion

        #region PlayGameCommand

        public ICommand PlayGameCommand { get; }

        private bool CanPlayGameCommandExecute(object p) => true;

        private void OnPlayGameCommandExecuted(object p)
        {
            #region Animation

            //var border = (Border)p;
            //ThicknessAnimation borderAnimation = new();
            //borderAnimation.From = border.BorderThickness;
            //borderAnimation.To = new Thickness(6, 6, 3, 3);
            //borderAnimation.Duration = TimeSpan.FromSeconds(0.5);
            ////borderAnimation.SpeedRatio = 1.7;
            //borderAnimation.FillBehavior = FillBehavior.Stop;

            //ColorAnimation colorAnimation = new();
            //colorAnimation.From = Color.FromArgb(255, 128, 128, 128); 
            //colorAnimation.To = Color.FromArgb(255, 255, 255, 255);
            //colorAnimation.Duration = TimeSpan.FromSeconds(1);
            //colorAnimation.FillBehavior = FillBehavior.Stop;
            ////colorAnimation.SpeedRatio = 5;
            //Storyboard.SetTargetName(colorAnimation, "gradstop1");
            //Storyboard.SetTargetProperty(colorAnimation, new PropertyPath(GradientStop.ColorProperty));
            //Storyboard.SetTargetProperty(borderAnimation, new PropertyPath(Border.BorderThicknessProperty));

            //Storyboard storyboard = new();
            //storyboard.Children.Add(colorAnimation);
            //storyboard.Children.Add(borderAnimation);
            //storyboard.Begin(border);

            #endregion

            if (Sapper.Difficulty == Difficulty.Beginner)
                OnChooseBeginnerCommandExecuted();
            else if (Sapper.Difficulty == Difficulty.Intermediate)
                OnChooseIntermediateCommandExecuted();
            else
                OnChooseExpertCommandExecuted();
        }

        #endregion

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
                                    GameTimer.Stop();
                                    SourceImage = SharedUtils.DeadSmileImage;
                                    GameIsOver = true;
                                }
                                else if (Sapper.Field[i, j] == 0)
                                {
                                    _cells[i, j].Visibility = Visibility.Collapsed;
                                    CountClosedCells--;
                                    OpeningCellsRecursion(i, j);
                                }
                                else
                                {
                                    _cells[i, j].Visibility = Visibility.Collapsed;
                                    CountClosedCells--;
                                    continue;
                                }
                            }
                        }
                        catch { }
                    }
                }
            }
            if(CountClosedCells == 0)
                GameTimer.Stop();
        }

        #endregion

        #region SetFlagCommand

        private bool CanSetFlagCommandExecute(object p) => !GameIsOver;

        private void OnSetFlagCommandExecuted(object p)
        {
            if (!GameTimerStarted)
            {
                GameTimer.Start();
                GameTimerStarted = true;
            }
            var border = (Border)p;
            var cords = border.Uid.Split('-');
            int idx = int.Parse(cords[0]);
            int jdx = int.Parse(cords[1]);
            if (!_cells[idx,jdx].IsFlag)
            {
                Image flagImage = new Image() { Source = SharedUtils.FlagImage };
                border.Child = flagImage;
                _cells[idx, jdx].IsFlag = true;
                CountingMines(false);
            }
            else
            {
                border.Child = null;
                _cells[idx, jdx].IsFlag = false;
                CountingMines(true);
            }
        }

        #endregion

        #region ClickBorderCommand

        private bool CanClickBorderCommandExecute(object p) => !GameIsOver;

        private void OnClickBorderCommandExecuted(object p)
        {
            if(!GameTimerStarted)
            {
                GameTimer.Start();
                GameTimerStarted = true;
            }
            var border = (Border)p;
            if (border.Child == null)
            {
                var cords = border.Uid.Split('-');
                int idx = int.Parse(cords[0]);
                int jdx = int.Parse(cords[1]);
                _cells[idx, jdx].Visibility = Visibility.Collapsed;
                CountClosedCells--;
                if (Sapper.Field[idx, jdx] == -1)
                {
                    _cells[idx, jdx].Background = new SolidColorBrush(Color.FromArgb(255, 209, 99, 99));
                    ShowAllMines();
                    GameTimer.Stop();
                    SourceImage = SharedUtils.DeadSmileImage;
                    GameIsOver = true;
                }
                else if (Sapper.Field[idx, jdx] == 0)
                {
                    OpeningCellsRecursion(idx, jdx);
                }
                if (CountClosedCells == 0)
                    GameTimer.Stop();
            }
        }

        #endregion

        #endregion

        #region Methods

        private void TimerTick(object sender, EventArgs e)
        {
            Time++;
            Timer = Time.ToString("000");
        }

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
            GameTimer = new();
            GameTimer.Interval = new TimeSpan(0, 0, 1);
            GameTimer.Tick += new EventHandler(TimerTick);
            int n = Sapper.Field.GetLength(0);
            int m = Sapper.Field.GetLength(1);
            var cells = new CellVM[n, m];
            var image = SharedUtils.MineImage;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    cells[i, j] = new CellVM(Sapper.FontSize, Sapper.Field[i, j], Visibility.Visible, $"{i}-{j}", null, new SolidColorBrush(Color.FromArgb(255, 198, 198, 198)), OnSetFlagCommandExecuted, CanSetFlagCommandExecute, OnClickBorderCommandExecuted, CanClickBorderCommandExecute, OnClickLabelCommandExecuted, CanClickLabelCommandExecute);
                    if (Sapper.Field[i, j] == -1)
                    {
                        Image img = new() { Source = image };
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
                            CountClosedCells--;
                            OpeningCellsRecursion(i, j);
                        }
                        else
                        {
                            _cells[i, j].Visibility = Visibility.Collapsed;
                            CountClosedCells--;
                            continue;
                        }
                    }
                    catch { }
                }
            }
        }

        private void CountingMines(bool operation)
        {
            if (operation)
                Mines++;
            else
                Mines--;
            var line = Math.Abs(Mines).ToString();
            if (Mines >= 0)
            {
                CounterMines = Mines.ToString("000");
            }
            else
            {
                if (line.Length == 1)
                    CounterMines = "0-" + line;
                else if (line.Length == 2)
                    CounterMines = "-" + line;
                else
                    CounterMines = line;
            }
        }

        #endregion

        public MainWindowViewModel()
        {
            GenerateCells();

            #region Commands

            CloseAppCommand = new LambdaCommand(OnCloseAppCommandExecuted, CanCloseAppCommandExecute);
            PlayGameCommand = new LambdaCommand(OnPlayGameCommandExecuted, CanPlayGameCommandExecute);
            ChooseBeginnerCommand = new LambdaCommand(OnChooseBeginnerCommandExecuted, CanChooseBeginnerCommandExecute);
            ChooseIntermediateCommand = new LambdaCommand(OnChooseIntermediateCommandExecuted, CanChooseIntermediateCommandExecute);
            ChooseExpertCommand = new LambdaCommand(OnChooseExpertCommandExecuted, CanChooseExpertCommandExecute);

            #endregion
        }
    }
}
