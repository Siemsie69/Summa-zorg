using System.Windows;
using System.Windows.Input;
using Zorgdossier.Helpers;
using Zorgdossier.Models;
using Zorgdossier.Views.SectieViews;

namespace Zorgdossier.ViewModels.SectieViewModels
{
    public class BasicInformationViewModel : ObservableObject
    {
        #region Fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        #endregion

        #region Constructors
        public BasicInformationViewModel(IAppNavigation appNavigation, UserMessage userMessage)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;

            ShowPhoneSummaryCommand = new RelayCommand(ExecuteShowPhoneSummary);
            ShowIntroductionCommand = new RelayCommand(ExecuteShowIntroduction);
            ShowHomeCommand = new RelayCommand(ExecuteShowHome);
            ShowInfoCommand = new RelayCommand(ExecuteShowInfo);
        }

        public BasicInformationViewModel() { }
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
        public ICommand ShowPhoneSummaryCommand { get; }
        public ICommand ShowIntroductionCommand { get; }
        public ICommand ShowHomeCommand { get; }
        public ICommand ShowInfoCommand { get; }
        #endregion

        #region Methods
        private void ExecuteShowPhoneSummary(object? obj)
        {
            _appNavigation.ActiveViewModel = new PhoneSummaryViewModel(_appNavigation, _userMessage);
        }
        
        private void ExecuteShowIntroduction(object? obj)
        {
            _appNavigation.ActiveViewModel = new IntroductionViewModel(_appNavigation, _userMessage);
        }

        private void ExecuteShowHome(object? obj)
        {
            MessageBoxResult result = MessageBox.Show("Ben je zeker dat je wilt afsluiten en terugkeren naar de homepagina? Je voortgang in dit dossier gaat dan verloren.", "Waarschuwing", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                _appNavigation.ActiveViewModel = new HomeViewModel(_appNavigation, _userMessage);
            }
        }

        private void ExecuteShowInfo(object? obj)
        {
            MessageBox.Show("De basisinformatie van een patiënt omvat onder andere de naam, de klacht waarvoor de patiënt zich heeft aangemeld en de patiëntcategorie waarmee we te maken hebben. Het is belangrijk om een duidelijk beeld te krijgen van wie de patiënt is en welke zorgbehoeften er spelen, zodat de juiste zorg kan worden geboden.",
                            "Aanvullende Informatie en Handige Tips", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion
    }
}
