using System.Windows;
using Sapper.ViewModels.Base;
using System.Windows.Input;
using Sapper.Infrastructure.Commands;

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
