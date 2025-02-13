using System.Windows;
using System.Windows.Input;
using Zorgdossier.Databases;
using Zorgdossier.Helpers;
using Zorgdossier.Models;

namespace Zorgdossier.ViewModels.SectieViewModels
{
    class IntroductionViewModel : ObservableObject
    {
        #region fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        private Dossier _dossier;
        private DossierService _dossierService;
        #endregion

        #region constructers
        public IntroductionViewModel(IAppNavigation appNavigation, UserMessage userMessage, DossierService dossierService, Dossier? dossier = null)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;
            _dossierService = dossierService;
            if (dossier != null)
            {
                _dossier = dossier;
            }
            ShowInfoCommand = new RelayCommand(ExecuteShowInfo);
            ShowDossiersCommand = new RelayCommand(ExecuteShowDossiersView);
            ShowBasicInformationCommand = new RelayCommand(ExecuteShowBasicInformation);
        }

        public IntroductionViewModel()
        {

        }
        #endregion

        #region properties
        #endregion

        #region commands
        public ICommand ShowInfoCommand 
        { 
            get; 
        }
        public ICommand ShowDossiersCommand
        {
            get;
        }
        public ICommand ShowBasicInformationCommand
        {
            get;
        }
        #endregion

        #region methods
        private void ExecuteShowInfo(object? obj)
        {
            MessageBox.Show("Beste student, klik op deze knop voor extra informatie en uitleg. Je vindt deze knop overal terwijl je het dossier invult. Gebruik deze functie en houd het voorbeelddossier open om je dossier correct en volledig in te vullen.",
                            "Aanvullende Informatie en Handige Tips", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void ExecuteShowDossiersView(object? obj)
        {
            _appNavigation.ActiveViewModel = new DossiersViewModel(_appNavigation, _userMessage);
        }
        private void ExecuteShowBasicInformation(object? obj)
        {
            _appNavigation.ActiveViewModel = new BasicInformationViewModel(_appNavigation, _userMessage, _dossierService, _dossier);
        }
        #endregion
    }
}
