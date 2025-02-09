using System.Windows;
using System.Windows.Input;
using Zorgdossier.Helpers;
using Zorgdossier.Models;
using Zorgdossier.Views.SectieViews;

namespace Zorgdossier.ViewModels.SectieViewModels
{
    public class QuestionsViewModel : ObservableObject
    {
        #region Fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        #endregion

        #region Constructors
        public QuestionsViewModel(IAppNavigation appNavigation, UserMessage userMessage)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;

            ShowOrgansCommand = new RelayCommand(ExecuteShowOrgans);
            ShowPhoneSummaryCommand = new RelayCommand(ExecuteShowPhoneSummary);
            ShowHomeCommand = new RelayCommand(ExecuteShowHome);
            ShowInfoCommand = new RelayCommand(ExecuteShowInfo);
        }

        public QuestionsViewModel() { }
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
        public ICommand ShowOrgansCommand { get; }
        public ICommand ShowPhoneSummaryCommand { get; }
        public ICommand ShowHomeCommand { get; }
        public ICommand ShowInfoCommand { get; }
        #endregion

        #region Methods
        private void ExecuteShowOrgans(object? obj)
        {
            _appNavigation.ActiveViewModel = new OrgansViewModel(_appNavigation, _userMessage);
        }
        
        private void ExecuteShowPhoneSummary(object? obj)
        {
            _appNavigation.ActiveViewModel = new PhoneSummaryViewModel(_appNavigation, _userMessage);
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
            MessageBox.Show("Zorg ervoor dat je vragen stelt die zowel breed als specifiek zijn, zodat je het volledige spectrum van klachten en symptomen kunt begrijpen en het juiste behandelplan kunt opstellen.",
                            "Aanvullende Informatie en Handige Tips", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion
    }
}
