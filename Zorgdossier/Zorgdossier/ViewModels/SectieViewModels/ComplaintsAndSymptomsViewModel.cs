using System.Windows;
using System.Windows.Input;
using Zorgdossier.Databases;
using Zorgdossier.Helpers;
using Zorgdossier.Models;
using Zorgdossier.Views;

namespace Zorgdossier.ViewModels.SectieViewModels
{
    class ComplaintsAndSymptomsViewModel : ObservableObject
    {
        #region fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        private DossierService _dossierService;
        private Dossier? _dossier;

        private bool _isSampleMode;
        #endregion

        #region constructers
        public ComplaintsAndSymptomsViewModel(IAppNavigation appNavigation, UserMessage userMessage, DossierService dossierService, Dossier? dossier = null, SampleDossierViewModel? instance = null)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;
            _dossierService = dossierService;

            ComplaintsSymptoms = _dossierService.CentralDossier.ComplaintsSymptoms;

            if (dossier != null)
            {
                _dossier = dossier;
            }
            if (instance != null)
            {
                Instance = instance;
                IsSampleMode = Instance.IsSampleMode;
            }

            ShowInfoCommand = new RelayCommand(ExecuteShowInfo);
            ShowHomeCommand = new RelayCommand(ExecuteShowMainView);
            ShowResearchCommand = new RelayCommand(ExecuteShowResearchView);
            ShowOrgansCommand = new RelayCommand(ExecuteShowOrganSelectionView);
            ShowFinishProgressCommand = new RelayCommand(ExecuteShowFinishedView);

            if (dossier == null)
            {
                String SampleFirstComplaintText = (string)Application.Current.Resources["SampleFirstComplaintText"];
                String SampleSecondComplaintText = (string)Application.Current.Resources["SampleSecondComplaintText"];
                String SampleThirdComplaintText = (string)Application.Current.Resources["SampleThirdComplaintText"];

                ComplaintsSymptoms.ComplaintsSymptomsSummary = IsSampleMode
                ? SampleFirstComplaintText + "\n" +
                  SampleSecondComplaintText + "\n" +
                  SampleThirdComplaintText
                : ComplaintsSymptoms.ComplaintsSymptomsSummary;
            }
        }

        public ComplaintsAndSymptomsViewModel()
        {

        }
        #endregion

        #region properties
        public DossierService.ComplaintsSymptoms ComplaintsSymptoms
        {
            get;
        }

        public bool IsSampleMode
        {
            get => _isSampleMode;
            set
            {
                if (_isSampleMode != value)
                {
                    _isSampleMode = value; OnPropertyChanged(nameof(IsSampleMode)); OnPropertyChanged(nameof(IsNotSampleMode));
                }
            }
        }

        public bool IsNotSampleMode => !IsSampleMode;

        public SampleDossierViewModel Instance
        {
            get;
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

        public ICommand ShowResearchCommand
        {
            get;
        }

        public ICommand ShowOrgansCommand
        {
            get;
        }

        public ICommand ShowFinishProgressCommand
        {
            get;
        }
        #endregion

        #region methods
        private void ExecuteShowInfo(object? obj)
        {
            String ComplaintsAndSymptomsMessageText = (string)Application.Current.Resources["ComplaintsAndSymptomsMessageText"];
            String InfoMessageTitle = (string)Application.Current.Resources["InfoMessageTitle"];

            MessageBox.Show(ComplaintsAndSymptomsMessageText, InfoMessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExecuteShowMainView(object? obj)
        {
            string message = (string)Application.Current.FindResource("ShowMainViewMessage");
            string title = (string)Application.Current.FindResource("ShowMainViewTitle");

            MessageBoxResult result = MessageBox.Show(
                message,
                title,
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
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
            else
            {
                return;
            }
        }

        private void ExecuteShowResearchView(object? obj)
        {
            if (IsSampleMode != true)
            {
                if (string.IsNullOrWhiteSpace(ComplaintsSymptoms.ComplaintsSymptomsSummary))
                {
                    String StandardUserMessageText = (string)Application.Current.Resources["StandardUserMessageText"];

                    _userMessage.Text = StandardUserMessageText;
                    return;
                }
                else
                {
                    _appNavigation.ActiveViewModel = new ResearchViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
                }
            }
            else
            {
                _appNavigation.ActiveViewModel = new ResearchViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
            }
        }

        private void ExecuteShowOrganSelectionView(object? obj)
        {
            _appNavigation.ActiveViewModel = new OrgansViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
        }

        private void ExecuteShowFinishedView(object? obj)
        {
            _appNavigation.ActiveViewModel = new FinishProgressViewModel(_appNavigation, _userMessage, _dossierService, _dossier);
        }
        #endregion
    }
}