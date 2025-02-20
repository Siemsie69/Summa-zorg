using System.Windows;
using System.Windows.Input;
using Zorgdossier.Databases;
using Zorgdossier.Helpers;
using Zorgdossier.Models;

namespace Zorgdossier.ViewModels
{
    class SampleDossierViewModel : ObservableObject
    {
        #region fields
        private IAppNavigation _appNavigation;
        private AppNavigation _navigation = new AppNavigation();
        private UserMessage _userMessage = new UserMessage();
        private DossierService _dossierService = new DossierService();
        private Dossier _dossier = null;

        private static readonly SampleDossierViewModel _instance = new SampleDossierViewModel();
        private bool _isSampleMode;
        #endregion

        #region constructers
        public SampleDossierViewModel()
        {
            _appNavigation = _navigation;
            IsSampleMode = true;

            _appNavigation.ActiveViewModel = new SectieViewModels.BasicInformationViewModel(_appNavigation, _userMessage, _dossierService, _dossier, this);
            CloseCommand = new RelayCommand(ExecuteCloseWindow);
        }
        #endregion

        #region properties
        public IAppNavigation AppNavigation
        {
            get
            {
                return _appNavigation;
            }
        }
        public static SampleDossierViewModel Instance => _instance;
        public bool IsSampleMode
        {
            get => _isSampleMode;
            set
            {
                if (_isSampleMode != value)
                {
                    _isSampleMode = value; OnPropertyChanged(nameof(IsSampleMode));
                }
            }
        }
        #endregion

        #region commands
        public ICommand CloseCommand
        {
            get;
        }
        #endregion

        #region methods
        private void ExecuteCloseWindow(object? parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
        }
        #endregion
    }
}