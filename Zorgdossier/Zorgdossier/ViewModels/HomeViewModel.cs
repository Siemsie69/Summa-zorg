using System.Windows.Input;
using Zorgdossier.Databases;
using Zorgdossier.Helpers;
using Zorgdossier.Models;

namespace Zorgdossier.ViewModels
{
    internal class HomeViewModel : ObservableObject
    {
        #region Fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        private DossierService _dossierService;
        #endregion

        #region Constructors
        public HomeViewModel(IAppNavigation appNavigation, UserMessage userMessage, DossierService dossierService)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;
            _dossierService = dossierService;

            ShowDossiersCommand = new RelayCommand(ExecuteShowDossiers);
            ShowExplanationCommand = new RelayCommand(ExecuteShowExplanation);
        }

        public HomeViewModel() { }
        #endregion

        #region Properties

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
        public ICommand ShowDossiersCommand { get; }
        public ICommand ShowExplanationCommand { get; }
        #endregion

        #region Methods
        private void ExecuteShowDossiers(object? obj)
        {
            _appNavigation.ActiveViewModel = new DossiersViewModel(_appNavigation, _userMessage, _dossierService);
        }

        private void ExecuteShowExplanation(object? obj)
        {
            _appNavigation.ActiveViewModel = new ExplanationViewModel(_appNavigation, _userMessage, _dossierService);
        }
        #endregion
    }
}
