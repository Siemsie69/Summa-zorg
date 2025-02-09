using System.Windows;
using System.Windows.Input;
using Zorgdossier.Helpers;
using Zorgdossier.Models;
using Zorgdossier.Views.SectieViews;

namespace Zorgdossier.ViewModels.SectieViewModels
{
    public class IntroductionViewModel : ObservableObject
    {
        #region Fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        #endregion

        #region Constructors
        public IntroductionViewModel(IAppNavigation appNavigation, UserMessage userMessage)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;

            ShowBasicInformationCommand = new RelayCommand(ExecuteShowBasicInformation);
            ShowDossiersCommand = new RelayCommand(ExecuteShowDossiers);
            ShowInfoCommand = new RelayCommand(ExecuteShowInfo);
        }

        public IntroductionViewModel() { }
        #endregion

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

        #region Commands
        public ICommand ShowBasicInformationCommand { get; }
        public ICommand ShowDossiersCommand { get; }
        public ICommand ShowInfoCommand { get; }
        #endregion

        #region Methods
        private void ExecuteShowBasicInformation(object? obj)
        {
            _appNavigation.ActiveViewModel = new BasicInformationViewModel(_appNavigation, _userMessage);
        }

        private void ExecuteShowDossiers(object? obj)
        {
            var result = MessageBox.Show(
                "Weet je zeker dat je wilt stoppen met het aanmaken van een dossier?",
                "Bevestiging",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.Yes)
            {
                _appNavigation.ActiveViewModel = new DossiersViewModel(_appNavigation, _userMessage);
            }
        }

        private void ExecuteShowInfo(object? obj)
        {
            MessageBox.Show("Heb je vragen of onzekerheden? Aarzel niet om hulp te vragen aan je docent of medestudenten. Zij kunnen je ondersteunen bij het correct invullen en begrijpen van het dossier.",
                            "Aanvullende Informatie en Handige Tips", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion
    }
}
