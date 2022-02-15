using System.Windows;
using Sapper.ViewModels.Base;
using System.Windows.Input;
using Sapper.Infrastructure.Commands;
using System.Windows.Controls;
using System.Windows.Media;
using Sapper.Models;
using System.Windows.Media.Imaging;
using System;

namespace Sapper.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Fields

        private SapperField Sapper;

        private Button[,] Buttons;

        private Label[,] Labels;

        private int CountCloseCells;

        #endregion

        #region Properties

        private Grid _grid = new Grid();

        public Grid GridSapper
        {
            get => _grid;
            set => Set(ref _grid, value);
        }

        #endregion

        #region Commands

        public ICommand ReplayGameCommand { get; }

        private bool CanReplayGameCommandExecute(object p) => true;

        private void OnReplayGameCommandExecuted(object p)
        {
            _grid.Children.Clear();
            _grid.RowDefinitions.Clear();
            _grid.ColumnDefinitions.Clear();
        }

        public ICommand CloseAppCommand { get; }

        private bool CanCloseAppCommandExecute(object p) => true;

        private void OnCloseAppCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }

        public ICommand SetFlagCommand { get; }

        private bool CanSetFlagCommandExecute(object p) => true;

        private void OnSetFlagCommandExecuted(object p)
        {
            var button = (Button)p;
            if(button.Content == null)
            {
                Image image = new Image() { Source = new BitmapImage(new Uri("Data/Images/flag.png", UriKind.Relative)) };
                button.Content = image;
            }
            else
            {
                button.Content = null;
            }
        }

        public ICommand ClickButtonCommand { get; }

        private bool CanClickButtonCommandExecute(object p) => true;

        private void OnClickButtonCommandExecuted(object p)
        {
            var button = (Button)p;
            if(button.Content == null)
            {
                var cords = button.Uid.Split('-');
                int idx = int.Parse(cords[0]);
                int jdx = int.Parse(cords[1]);
                Buttons[idx, jdx].Visibility = Visibility.Collapsed;
                CountCloseCells--;
                if (Sapper.Field[idx, jdx] == -1)
                {
                    Labels[idx, jdx].Background = new SolidColorBrush(Color.FromArgb(255, 209, 99, 99));
                    for (int i = 0; i < Labels.GetLength(0); i++)
                    {
                        for (int j = 0; j < Labels.GetLength(1); j++)
                        {
                            if (Labels[i, j].Uid == "mine")
                                Buttons[i, j].Visibility = Visibility.Collapsed;
                        }
                    }
                    _grid.Children.Add(CreateBlockedGrid());
                }
                else if (Sapper.Field[idx, jdx] == 0)
                {
                    OpeningRecursion(idx, jdx);
                }
                if(CountCloseCells == 0)
                    _grid.Children.Add(CreateBlockedGrid());
            }
        }

        public ICommand CreateSapperFieldCommand { get; }

        private bool CanCreateSapperFieldCommandExecute(object p) => true;

        private void OnCreateSapperFieldCommandExecuted(object p)
        {
            if (_grid.ColumnDefinitions.Count == 0)
            {
                var difficult = (string)p;
                if (difficult == "Beginner")
                    Sapper = new SapperField(Difficulty.Beginner);
                else if (difficult == "Amateur")
                    Sapper = new SapperField(Difficulty.Amateur);
                else
                    Sapper = new SapperField(Difficulty.Professional);
                CreateGridSapper();
                FillingGridSapper();
            }
            else
                MessageBox.Show("Игра уже запущена!");
            
        }

        #endregion

        #region Methods

        private Grid CreateBlockedGrid()
        {
            Grid blockGrid = new Grid();
            blockGrid.RowDefinitions.Add(new RowDefinition());
            blockGrid.RowDefinitions.Add(new RowDefinition());
            blockGrid.ColumnDefinitions.Add(new ColumnDefinition());
            blockGrid.ColumnDefinitions.Add(new ColumnDefinition());
            blockGrid.Background = new SolidColorBrush(Color.FromArgb(125, 108, 99, 99));
            Grid.SetColumnSpan(blockGrid, 50);
            Grid.SetRowSpan(blockGrid, 50);
            TextBlock text = new TextBlock();
            text.Text = "You've lost!";
            text.HorizontalAlignment = HorizontalAlignment.Center;
            text.VerticalAlignment = VerticalAlignment.Bottom;
            text.FontSize = 40;
            text.FontWeight = FontWeights.Bold;
            Grid.SetRow(text, 0);
            Grid.SetColumnSpan(text, 2);
            Button button = new Button();
            button.Content = "Replay";
            button.Command = ReplayGameCommand;
            button.Width = 110;
            button.Height = 36;
            button.Margin = new Thickness(5);
            button.Background = new SolidColorBrush(Color.FromArgb(255, 90, 170, 180));
            button.BorderThickness = new Thickness(3);
            button.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 139, 25, 224));
            button.FontFamily = new FontFamily("Book Antiqua");
            button.FontSize = 20;
            button.VerticalAlignment = VerticalAlignment.Top;
            button.HorizontalAlignment = HorizontalAlignment.Right;
            Grid.SetRow(button, 1);
            Grid.SetColumn(button, 0);  
            if(CountCloseCells == 0)
            {
                button.Content = "New Game";
                text.Text = "You won!";
            }
            blockGrid.Children.Add(text);
            blockGrid.Children.Add(button);
            button = new Button();
            button.Content = "Exit";
            button.Command = CloseAppCommand;
            button.Width = 110;
            button.Height = 36;
            button.Margin = new Thickness(5);
            button.Background = new SolidColorBrush(Color.FromArgb(255, 90, 170, 180));
            button.BorderThickness = new Thickness(3);
            button.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 139, 25, 224));
            button.FontFamily = new FontFamily("Book Antiqua");
            button.FontSize = 20;
            button.VerticalAlignment = VerticalAlignment.Top;
            button.HorizontalAlignment = HorizontalAlignment.Left;
            Grid.SetRow(button, 1);
            Grid.SetColumn(button, 1);
            blockGrid.Children.Add(button);
            return blockGrid;
        }

        private void OpeningRecursion(int idx, int jdx)
        {
            for (int i = idx - 1; i < idx + 2; i++)
            {
                for (int j = jdx - 1; j < jdx + 2; j++)
                {
                    try
                    {
                        if (Buttons[i, j].Visibility == Visibility.Collapsed)
                            continue;
                        if (Sapper.Field[i, j] == 0)
                        {
                            Buttons[i, j].Visibility = Visibility.Collapsed;
                            CountCloseCells--;
                            OpeningRecursion(i, j);
                        }
                        else
                        {
                            Buttons[i, j].Visibility = Visibility.Collapsed;
                            CountCloseCells--;
                            continue;
                        }
                    }
                    catch { }
                }
            }
        }

        private void FillingGridSapper()
        {
            var image = new BitmapImage(new Uri("Data/Images/mine.png", UriKind.Relative));
            if (Sapper.Difficulty == Difficulty.Beginner)
            {
                CountCloseCells = 81 - (byte)Difficulty.Beginner;
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        CreateLabel(image, i, j);
                        CreateButton(i, j);
                    }
                }
            }
            else if (Sapper.Difficulty == Difficulty.Amateur)
            {
                CountCloseCells = 256 - (byte)Difficulty.Amateur;
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        CreateLabel(image, i, j);
                        CreateButton(i, j);
                    }
                }
            }
            else
            {
                CountCloseCells = 480 - (byte)Difficulty.Professional;
                for (int i = 0; i < 30; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        CreateLabel(image, i, j);
                        CreateButton(i, j);
                    }
                }
            }
        }

        private void CreateButton(int i, int j)
        {
            Button button = new Button();
            button.Uid = $"{i}-{j}";
            button.DataContext = this;
            button.Command = ClickButtonCommand;
            button.CommandParameter = button;
            button.Background = new SolidColorBrush(Color.FromArgb(255, 184, 184, 190));
            button.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 68, 64, 71));
            button.InputBindings.Add(new MouseBinding() { Command = SetFlagCommand, MouseAction = MouseAction.RightClick, CommandParameter = button });
            Grid.SetRow(button, i);
            Grid.SetColumn(button, j);
            Buttons[i, j] = button;
            _grid.Children.Add(button);
        }

        private void CreateLabel(BitmapImage image, int i, int j)
        {
            Label label = new Label();
            label.Content = Sapper.Field[i, j];
            label.FontWeight = FontWeights.Bold;
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            label.VerticalContentAlignment = VerticalAlignment.Center;
            label.FontSize = 20;
            label.Foreground = new SolidColorBrush(Color.FromArgb(255, 24, 29, 237));
            label.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 68, 64, 71));
            label.BorderThickness = new Thickness(0.5);
            label.Background = new SolidColorBrush(Color.FromArgb(255, 197, 197, 204));
            if (Sapper.Field[i, j] == -1)
            {
                Image img = new Image();
                img.Source = image;
                label.Content = img;
                label.Uid = $"mine";
            }
            else if (Sapper.Field[i, j] == 0)
            {
                label.Content = null;
            }
            else if (Sapper.Field[i, j] == 1)
            {
                label.Foreground = new SolidColorBrush(Color.FromArgb(255, 24, 29, 237));
            }
            else if (Sapper.Field[i, j] == 2)
            {
                label.Foreground = new SolidColorBrush(Color.FromArgb(255, 62, 151, 30));
            }
            else if (Sapper.Field[i, j] == 3)
            {
                label.Foreground = new SolidColorBrush(Color.FromArgb(255, 215, 22, 50));
            }
            else if (Sapper.Field[i, j] == 4)
            {
                label.Foreground = new SolidColorBrush(Color.FromArgb(255, 22, 41, 164));
            }
            else if (Sapper.Field[i, j] == 5)
            {
                label.Foreground = new SolidColorBrush(Color.FromArgb(255, 138, 12, 12));
            }
            else
            {
                label.Foreground = new SolidColorBrush(Color.FromArgb(255, 250, 10, 10));
            }
            Grid.SetRow(label, i);
            Grid.SetColumn(label, j);
            Labels[i, j] = label;
            _grid.Children.Add(label);
        }

        private void CreateGridSapper()
        {
            if (Sapper.Difficulty == Difficulty.Beginner)
            {
                Buttons = new Button[9, 9];
                Labels = new Label[9, 9];
                for (int i = 0; i < 9; i++)
                {
                    _grid.RowDefinitions.Add(new RowDefinition());
                    _grid.ColumnDefinitions.Add(new ColumnDefinition());
                }
            }
            else if (Sapper.Difficulty == Difficulty.Amateur)
            {
                Buttons = new Button[16, 16];
                Labels = new Label[16, 16];
                for (int i = 0; i < 16; i++)
                {
                    _grid.RowDefinitions.Add(new RowDefinition());
                    _grid.ColumnDefinitions.Add(new ColumnDefinition());
                }
            }
            else
            {
                Buttons = new Button[30, 16];
                Labels = new Label[30, 16];
                for (int i = 0; i < 30; i++)
                {
                    _grid.RowDefinitions.Add(new RowDefinition());
                }
                for (int i = 0; i < 16; i++)
                {
                    _grid.ColumnDefinitions.Add(new ColumnDefinition());
                }
            }
        }

        #endregion

        public MainWindowViewModel()
        {
            #region Commands

            CreateSapperFieldCommand = new LambdaCommand(OnCreateSapperFieldCommandExecuted,CanCreateSapperFieldCommandExecute);
            ClickButtonCommand = new LambdaCommand(OnClickButtonCommandExecuted, CanClickButtonCommandExecute);
            SetFlagCommand = new LambdaCommand(OnSetFlagCommandExecuted, CanSetFlagCommandExecute);
            CloseAppCommand = new LambdaCommand(OnCloseAppCommandExecuted, CanCloseAppCommandExecute);
            ReplayGameCommand = new LambdaCommand(OnReplayGameCommandExecuted, CanReplayGameCommandExecute);

            #endregion
        }
    }
}
