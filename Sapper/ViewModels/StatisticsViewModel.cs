using Minesweeper.Infrastructure.Commands;
using Minesweeper.ViewModels.Base;
using Minesweeper.Views.Windows;
using System.Windows;
using System.Windows.Input;

namespace Minesweeper.ViewModels
{
    internal class StatisticsViewModel : ViewModel
    {
        #region Properties

        private string _totalGames = $"Total number of games played - {MainWindowViewModel.minesweeperStatistics.TotalCountGames}";

        public string TotalGames
        {
            get => _totalGames;
        }

        private string _winsGames = $"Total games won - {MainWindowViewModel.minesweeperStatistics.WinsGames}";

        public string WinsGames
        {
            get => _winsGames;
        }

        private string _lostGames = $"Total games lost - {MainWindowViewModel.minesweeperStatistics.LostGames}";

        public string LostGames
        {
            get => _lostGames;
        }

        private string _beginnerWinsGames = $"Won games in \"Beginner\" mode - {MainWindowViewModel.minesweeperStatistics.BeginnerWinsGames}";

        public string BeginnerWinsGames
        {
            get => _beginnerWinsGames;
        }

        private string _intermediateWinsGames = $"Won games in \"Intermediate\" mode - {MainWindowViewModel.minesweeperStatistics.IntermediateWinsGames}";

        public string IntermediateWinsGames
        {
            get => _intermediateWinsGames;
        }

        private string _expertWinsGames = $"Won games in \"Expert\" mode - {MainWindowViewModel.minesweeperStatistics.ExpertWinsGames}";

        public string ExpertWinsGames
        {
            get => _expertWinsGames;
        }

        private string _bestTimeBeginner = $"The best time in \"Beginner\" mode in seconds - {MainWindowViewModel.minesweeperStatistics.BestTimeBeginner}";

        public string BestTimeBeginner
        {
            get => _bestTimeBeginner;
        }

        private string _bestTimeIntermediate = $"The best time in \"Intermediate\" mode in seconds - {MainWindowViewModel.minesweeperStatistics.BestTimeIntermediate}";

        public string BestTimeIntermediate
        {
            get => _bestTimeIntermediate;
        }

        private string _bestTimeExpert = $"The best time in \"Expert\" mode in seconds - {MainWindowViewModel.minesweeperStatistics.BestTimeExpert}";

        public string BestTimeExpert
        {
            get => _bestTimeExpert;
        }

        #endregion

        #region Commands

        #region OKClickButtonCommand

        public ICommand OKClickButtonCommand { get; }

        private bool CanOkClickButtonCommandExecute(object p) => true;

        private void OnOkClickButtonCommandExecuted(object p)
        {
           ((Window)p).Close();
        }

        #endregion

        #region DeleteClickButtonCommand

        public ICommand DeleteClickButtonCommand { get; }

        private bool CanDeleteClickButtonCommandExecute(object p) => true;

        private void OnDeleteClickButtonCommandExecuted(object p)
        {
            ConfirmationWindow confirmationWindow = new((Window)p);
            confirmationWindow.ShowDialog();
        }

        #endregion

        #endregion

        public StatisticsViewModel()
        {
            OKClickButtonCommand = new LambdaCommand(OnOkClickButtonCommandExecuted, CanOkClickButtonCommandExecute);
            DeleteClickButtonCommand = new LambdaCommand(OnDeleteClickButtonCommandExecuted, CanDeleteClickButtonCommandExecute);
        }
    }
}
