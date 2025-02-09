using System.Windows;
using System.Windows.Input;
using Zorgdossier.Helpers;
using Zorgdossier.Models;
using Zorgdossier.Views.SectieViews;

namespace Zorgdossier.ViewModels.SectieViewModels
{
    public class ResearchViewModel : ObservableObject
    {
        #region Fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        #endregion

        #region Constructors
        public ResearchViewModel(IAppNavigation appNavigation, UserMessage userMessage)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;

            ShowPolicyCommand = new RelayCommand(ExecuteShowPolicy);
            ShowComplaintsAndSymptomsCommand = new RelayCommand(ExecuteShowComplaintsAndSymptoms);
            ShowHomeCommand = new RelayCommand(ExecuteShowHome);
            ShowInfoCommand = new RelayCommand(ExecuteShowInfo);
        }

        public ResearchViewModel() { }
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
        public ICommand ShowPolicyCommand { get; }
        public ICommand ShowComplaintsAndSymptomsCommand { get; }
        public ICommand ShowHomeCommand { get; }
        public ICommand ShowInfoCommand { get; }
        #endregion

        #region Methods
        private void ExecuteShowPolicy(object? obj)
        {
            _appNavigation.ActiveViewModel = new PolicyViewModel(_appNavigation, _userMessage);
        }

        private void ExecuteShowComplaintsAndSymptoms(object? obj)
        {
            _appNavigation.ActiveViewModel = new ComplaintsAndSymptomsViewModel(_appNavigation, _userMessage);
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
            MessageBox.Show("Focus op onderzoeken die niet alleen de diagnose ondersteunen, maar ook helpen om complicaties of andere onderliggende aandoeningen uit te sluiten, zodat je het juiste behandelplan kunt bepalen.",
                            "Aanvullende Informatie en Handige Tips", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion
    }
}
