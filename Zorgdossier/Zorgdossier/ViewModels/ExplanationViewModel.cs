using System.Windows;
using System.Windows.Input;
using Zorgdossier.Helpers;
using Zorgdossier.Models;

namespace Zorgdossier.ViewModels
{
    internal class ExplanationViewModel : ObservableObject
    {
        #region Fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;

        private readonly IWindowService _windowService = new WindowService();
        #endregion

        #region Constructors
        public ExplanationViewModel(IAppNavigation appNavigation, UserMessage userMessage)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;

            ShowInfoCommand = new RelayCommand(ExecuteShowInfo);
            ShowDossiersCommand = new RelayCommand(ExecuteShowDossiers);
            ShowSampleDossierCommand = new RelayCommand(ExecuteShowSampleDossier);
        }

        public ExplanationViewModel() { }
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
        public ICommand ShowInfoCommand { get; }
        public ICommand ShowDossiersCommand { get; }
        public ICommand ShowSampleDossierCommand { get; }
        #endregion

        #region 
        private void ExecuteShowInfo(object? obj)
        {
            String ExplanationMessageText = (string)Application.Current.Resources["ExplanationMessageText"];
            String InfoMessageTitle = (string)Application.Current.Resources["InfoMessageTitle"];

            MessageBox.Show(ExplanationMessageText, InfoMessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExecuteShowDossiers(object? obj)
        {
            _appNavigation.ActiveViewModel = new DossiersViewModel(_appNavigation, _userMessage);
        }

        private void ExecuteShowSampleDossier(object? obj)
        {
            var sharedViewModel = SampleDossierViewModel.Instance;
            _windowService.ShowWindow(sharedViewModel);
        }
        #endregion
    }
}
