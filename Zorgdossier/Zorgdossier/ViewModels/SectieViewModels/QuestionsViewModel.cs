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

                using (var context = new ApplicationDbContext())
                {
                    try
                    {
                        var questionInDb = context.Question.FirstOrDefault(x => x.DossierId == _dossier.Id);
                        Question.QuestionSummary = questionInDb.QuestionSummary;
                    }
                    catch (Exception ex)
                    {
                        _userMessage.Text = ("Fout met het ophalen van bestaande data: " + ex.Message);
                    }
                }
            }
            if (instance != null)
            {
                Instance = instance;
                IsSampleMode = Instance.IsSampleMode;
            }

            ShowInfoCommand = new RelayCommand(ExecuteShowInfo);
            ShowHomeCommand = new RelayCommand(ExecuteShowMainView);
            ShowOrgansCommand = new RelayCommand(ExecuteShowOrganSelectionView);
            ShowPhoneSummaryViewCommand = new RelayCommand(ExecuteShowPhoneSummaryView);

            Question.QuestionSummary = IsSampleMode
                ? "•\tHeeft u een katheter? Loopt deze nog goed door?\n" +
                  "•\tHeeft u een neurologische aandoening, waardoor uw blaasfunctie afwijkend is?\n" +
                  "•\tHerkent u de klachten van een eerdere urineweginfectie?"
                : "";
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
        public ICommand ShowPhoneSummaryViewCommand
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
        private void ExecuteShowMainView(object? obj)
        {
            MessageBoxResult result = MessageBox.Show("Weet je zeker dat je terug wilt gaan naar de Home pagina? Al je voortgang van dit dossier raakt dan verloren.", "Waarschuwing", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                _appNavigation.ActiveViewModel = new HomeViewModel(_appNavigation, _userMessage);
            }
        }
        private void ExecuteShowOrganSelectionView(object? obj)
        {
            if (IsSampleMode != true)
            {
                if (string.IsNullOrWhiteSpace(Question.QuestionSummary))
                {
                    _userMessage.Text = "Alle invoervelden moeten ingevuld zijn voordat je verder kan.";
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
        #endregion
    }
}