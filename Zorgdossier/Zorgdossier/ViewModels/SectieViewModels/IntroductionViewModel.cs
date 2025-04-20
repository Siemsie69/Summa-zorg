using System.Windows;
using System.Windows.Input;
using Zorgdossier.Databases;
using Zorgdossier.Helpers;
using Zorgdossier.Models;
using Zorgdossier.Views;

namespace Zorgdossier.ViewModels.SectieViewModels
{
    class IntroductionViewModel : ObservableObject
    {
        #region fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        private Dossier _dossier;
        private DossierService _dossierService;
        private string _introductionText;
        private string _buttonText;
        #endregion

        #region constructers
        public IntroductionViewModel(IAppNavigation appNavigation, UserMessage userMessage, DossierService dossierService, Dossier? dossier = null)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;
            _dossierService = dossierService;
            if(dossier != null)
            {
                _dossier = dossier;
                String IntroductionEditText = (string)Application.Current.Resources["IntroductionEditText"];
                String IntroductionEditButtonText = (string)Application.Current.Resources["IntroductionEditButtonText"];

                IntroductionText = IntroductionEditText;
                ButtonText = IntroductionEditButtonText;
            }
            else
            {
                String IntroductionCreateText = (string)Application.Current.Resources["IntroductionCreateText"];
                String IntroductionCreateButtonText = (string)Application.Current.Resources["IntroductionCreateButtonText"];

                IntroductionText = IntroductionCreateText;
                ButtonText = IntroductionCreateButtonText;
            }
            ShowInfoCommand = new RelayCommand(ExecuteShowInfo);
            ShowHomeCommand = new RelayCommand(ExecuteShowMainView);
            ShowBasicInformationCommand = new RelayCommand(ExecuteShowBasicInformation);
        }

        public IntroductionViewModel()
        {
        }
        #endregion

        #region properties
        public string IntroductionText
        {
            get => _introductionText;
            set
            {
                if (_introductionText != value)
                {
                    _introductionText = value;
                    OnPropertyChanged(nameof(IntroductionText));
                }
            }
        }
        public string ButtonText
        {
            get => _buttonText;
            set
            {
                if (_buttonText != value)
                {
                    _buttonText = value;
                    OnPropertyChanged(nameof(ButtonText));
                }
            }
        }
        #endregion

        #region commands
        public ICommand ShowInfoCommand
        {
            get;
        }

        public ICommand ShowHomeCommand
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
            String IntroductionMessageText = (string)Application.Current.Resources["IntroductionMessageText"];
            String InfoMessageTitle = (string)Application.Current.Resources["InfoMessageTitle"];

            MessageBox.Show(IntroductionMessageText, InfoMessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExecuteShowMainView(object? obj)
        {
            var mainViewModel = new MainViewModel(_appNavigation, _userMessage);

            // Open nieuwe window
            var mainView = new MainView
            {
                DataContext = mainViewModel
            };
            mainView.Show();

            Window? currentWindow = Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.IsActive);

            currentWindow?.Close();
        }

        private void ExecuteShowBasicInformation(object? obj)
        {
            _appNavigation.ActiveViewModel = new BasicInformationViewModel(_appNavigation, _userMessage, _dossierService, _dossier);
        }
        #endregion
    }
}
