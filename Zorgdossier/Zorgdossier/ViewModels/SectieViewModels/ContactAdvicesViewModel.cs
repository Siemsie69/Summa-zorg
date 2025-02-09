using System.Windows;
using System.Windows.Input;
using Zorgdossier.Helpers;
using Zorgdossier.Models;
using Zorgdossier.Views.SectieViews;

namespace Zorgdossier.ViewModels.SectieViewModels
{
    public class ContactAdvicesViewModel : ObservableObject
    {
        #region Fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        #endregion

        #region Constructors
        public ContactAdvicesViewModel(IAppNavigation appNavigation, UserMessage userMessage)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;

            ShowTreatmentCommand = new RelayCommand(ExecuteShowTreatment);
            ShowPolicyCommand = new RelayCommand(ExecuteShowPolicy);
            ShowHomeCommand = new RelayCommand(ExecuteShowHome);
            ShowInfoCommand = new RelayCommand(ExecuteShowInfo);
        }

        public ContactAdvicesViewModel() { }
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
        public ICommand ShowTreatmentCommand { get; }
        public ICommand ShowPolicyCommand { get; }
        public ICommand ShowHomeCommand { get; }
        public ICommand ShowInfoCommand { get; }
        #endregion

        #region Methods
        private void ExecuteShowTreatment(object? obj)
        {
            _appNavigation.ActiveViewModel = new TreatmentViewModel(_appNavigation, _userMessage);
        }

        private void ExecuteShowPolicy(object? obj)
        {
            _appNavigation.ActiveViewModel = new PolicyViewModel(_appNavigation, _userMessage);
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
            MessageBox.Show("Geef de patiënt duidelijke zelfzorginstructies, inclusief advies over activiteiten die vermeden moeten worden. Informeer ook wanneer het noodzakelijk is om contact op te nemen, bijvoorbeeld bij verergering van de klachten of het optreden van nieuwe symptomen.",
                            "Aanvullende Informatie en Handige Tips", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion
    }
}
