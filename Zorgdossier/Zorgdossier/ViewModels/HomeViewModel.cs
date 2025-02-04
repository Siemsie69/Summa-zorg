using System.Windows.Input;
using Zorgdossier.Helpers;
using Zorgdossier.Models;

namespace Zorgdossier.ViewModels
{
    internal class HomeViewModel : ObservableObject
    {
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;

        public HomeViewModel(IAppNavigation appNavigation, UserMessage userMessage)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;

            ShowDossiersCommand = new RelayCommand(ExecuteShowDossiers);
            ShowExplanationCommand = new RelayCommand(ExecuteShowExplanation);
        }

        public HomeViewModel() { }

        public IAppNavigation AppNavigation
        {
            get => _appNavigation;
        }

        public UserMessage UserMessage
        {
            get => _userMessage;
            set
            {
                _userMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShowDossiersCommand { get; }
        public ICommand ShowExplanationCommand { get; }

        private void ExecuteShowDossiers(object? obj)
        {
            _appNavigation.ActiveViewModel = new DossiersViewModel(_appNavigation, _userMessage);
        }

        private void ExecuteShowExplanation(object? obj)
        {
            _appNavigation.ActiveViewModel = new ExplanationViewModel(_appNavigation, _userMessage);
        }
    }
}
