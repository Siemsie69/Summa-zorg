using System.Windows;
using System.Windows.Input;
using Zorgdossier.Databases;
using Zorgdossier.Helpers;
using Zorgdossier.Models;

namespace Zorgdossier.ViewModels.SectieViewModels
{
    class QuestionsViewModel : ObservableObject
    {
        #region fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        private DossierService _dossierService;
        private Dossier? _dossier;

        private bool _isSampleMode;
        #endregion

        #region constructers
        public QuestionsViewModel(IAppNavigation appNavigation, UserMessage userMessage, DossierService dossierService, Dossier? dossier = null, SampleDossierViewModel? instance = null)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;
            _dossierService = dossierService;

            Question = _dossierService.CentralDossier.Question;

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
            ShowOrgansCommand = new RelayCommand(ExecuteShowOrganSelectionView);
            ShowPhoneSummaryCommand = new RelayCommand(ExecuteShowPhoneSummaryView);
            ShowFinishProgressCommand = new RelayCommand(ExecuteShowFinishedView);

            if (dossier == null)
            {
                Question.QuestionSummary = IsSampleMode
                ? "Heeft u een katheter? Loopt deze nog goed door?\n" +
                  "Heeft u een neurologische aandoening, waardoor uw blaasfunctie afwijkend is?\n" +
                  "Herkent u de klachten van een eerdere urineweginfectie?"
                : Question.QuestionSummary;
            }
        }

        public QuestionsViewModel()
        {

        }
        #endregion

        #region properties
        public DossierService.Question Question
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

        public ICommand ShowOrgansCommand
        {
            get;
        }

        public ICommand ShowPhoneSummaryCommand
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
            String QuestionsMessageText = (string)Application.Current.Resources["QuestionsMessageText"];
            String InfoMessageTitle = (string)Application.Current.Resources["InfoMessageTitle"];

            MessageBox.Show(QuestionsMessageText, InfoMessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
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
                _appNavigation.ActiveViewModel = new MainViewModel(_appNavigation, _userMessage);
            }
        }

        private void ExecuteShowOrganSelectionView(object? obj)
        {
            if (IsSampleMode != true)
            {
                if (string.IsNullOrWhiteSpace(Question.QuestionSummary))
                {
                    String StandardUserMessageText = (string)Application.Current.Resources["StandardUserMessageText"];

                    _userMessage.Text = StandardUserMessageText;
                    return;
                }
                else
                {
                    _appNavigation.ActiveViewModel = new OrgansViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
                }
            }
            else
            {
                _appNavigation.ActiveViewModel = new OrgansViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
            }
        }

        private void ExecuteShowPhoneSummaryView(object? obj)
        {
            _appNavigation.ActiveViewModel = new PhoneSummaryViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
        }

        private void ExecuteShowFinishedView(object? obj)
        {
            _appNavigation.ActiveViewModel = new FinishProgressViewModel(_appNavigation, _userMessage, _dossierService, _dossier);
        }
        #endregion
    }
}