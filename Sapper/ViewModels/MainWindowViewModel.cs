using System.Windows;
using Sapper.ViewModels.Base;
using System.Windows.Input;
using Sapper.Infrastructure.Commands;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sapper.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Properties

        private string _title = "Основное окно";
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        #endregion

        #region Commands

        public ICommand CloseAppCommand { get; }

        private bool CanCloseAppCommandExecute(object p) => true;

        private void OnCloseAppCommandExecuted(object p)
        {
            Application.Current.Shutdown();
            //Label label = new Label();
            //label.Content = sapper.Field[i, j].ToString();
            //label.FontWeight = FontWeights.Bold;
            //label.HorizontalContentAlignment = HorizontalAlignment.Center;
            //label.VerticalContentAlignment = VerticalAlignment.Center;
            //label.FontSize = 20;
            //label.Foreground = new SolidColorBrush(Color.FromArgb(255, 24, 29, 237));
            //label.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 68, 64, 71));
            //label.BorderThickness = new Thickness(0.5);
            //label.Background = new SolidColorBrush(Color.FromArgb(255, 197, 197, 204));
            //if (sapper.Field[i, j] == -1)
            //{
            //    Image img = new Image();
            //    img.Source = BitmapFrame.Create(new Uri(@"C:\Desktop\Work_VS\MyApps\Sapper\Sapper\Data\mine.png"));
            //    label.Content = img;
            //}
            //else if (sapper.Field[i, j] == 0)
            //{
            //    label.Content = null;
            //}
            //else if (sapper.Field[i, j] == 1)
            //{
            //    label.Foreground = new SolidColorBrush(Color.FromArgb(255, 24, 29, 237));
            //}
            //else if (sapper.Field[i, j] == 2)
            //{
            //    label.Foreground = new SolidColorBrush(Color.FromArgb(255, 62, 151, 30));
            //}
            //else if (sapper.Field[i, j] == 3)
            //{
            //    label.Foreground = new SolidColorBrush(Color.FromArgb(255, 215, 22, 50));
            //}
            //else if (sapper.Field[i, j] == 4)
            //{
            //    label.Foreground = new SolidColorBrush(Color.FromArgb(255, 22, 41, 164));
            //}
            //else if (sapper.Field[i, j] == 5)
            //{
            //    label.Foreground = new SolidColorBrush(Color.FromArgb(255, 138, 12, 12));
            //}
            //Grid.SetRow(label, i);
            //Grid.SetColumn(label, j);
            //grid.Children.Add(label);
            //Button bt = new Button();
            //bt.Background = new SolidColorBrush(Color.FromArgb(255, 184, 184, 190));
            //bt.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 68, 64, 71));
            //Grid.SetRow(bt, i);
            //Grid.SetColumn(bt, j);
            //bt.Click += Bt_Click;
            //grid.Children.Add(bt);
        }

        #endregion

        public MainWindowViewModel()
        {
            #region Commands

            CloseAppCommand = new LambdaCommand(OnCloseAppCommandExecuted,CanCloseAppCommandExecute);

            #endregion
        }
    }
}
