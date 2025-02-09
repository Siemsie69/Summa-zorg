using System.Windows;
using System.Windows.Input;
using Zorgdossier.Helpers;
using Zorgdossier.Models;
using Zorgdossier.Views.SectieViews;

namespace Zorgdossier.ViewModels.SectieViewModels
{
    public class OrgansViewModel : ObservableObject
    {
        #region Fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        #endregion

        #region Constructors
        public OrgansViewModel(IAppNavigation appNavigation, UserMessage userMessage)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;

            ShowComplaintsAndSymptomsCommand = new RelayCommand(ExecuteShowComplaintsAndSymptoms);
            ShowQuestionsCommand = new RelayCommand(ExecuteShowQuestions);
            ShowHomeCommand = new RelayCommand(ExecuteShowHome);
            ShowInfoCommand = new RelayCommand(ExecuteShowInfo);
        }

        public OrgansViewModel() { }
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
        public ICommand ShowComplaintsAndSymptomsCommand { get; }
        public ICommand ShowQuestionsCommand { get; }
        public ICommand ShowHomeCommand { get; }
        public ICommand ShowInfoCommand { get; }
        #endregion

        #region Methods
        private void ExecuteShowComplaintsAndSymptoms(object? obj)
        {
            _appNavigation.ActiveViewModel = new ComplaintsAndSymptomsViewModel(_appNavigation, _userMessage);
        }

        private void ExecuteShowQuestions(object? obj)
        {
            _appNavigation.ActiveViewModel = new QuestionsViewModel(_appNavigation, _userMessage);
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
            MessageBox.Show("Kies de organen die direct gerelateerd zijn aan de klachten en symptomen die de patiënt ervaart. Dit helpt om een gerichter dossier op te stellen, zodat je de juiste zorg en behandeling kunt plannen.",
                            "Aanvullende Informatie en Handige Tips", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion
    }
}
