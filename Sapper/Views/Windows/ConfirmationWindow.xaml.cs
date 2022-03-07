using Minesweeper.Models;
using Minesweeper.ViewModels;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace Minesweeper.Views.Windows
{
    public partial class ConfirmationWindow : Window
    {
        private Window previousWindow;

        public ConfirmationWindow(Window window)
        {
            InitializeComponent();
            previousWindow = window;
        }

        private void Button_Click_No(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_Yes(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.minesweeperStatistics = new();
            XmlSerializer xmlSerializer = new(typeof(MinesweeperStatistics));
            using (var stream = new StreamWriter("statistics.xml"))
            {
                xmlSerializer.Serialize(stream, MainWindowViewModel.minesweeperStatistics);
            }
            this.Close();
            previousWindow.Close();
        }
    }
}
