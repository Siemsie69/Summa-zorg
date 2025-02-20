using System.Windows;
using System.Windows.Input;
using Zorgdossier.Databases;
using Zorgdossier.Helpers;
using Zorgdossier.Models;

namespace Zorgdossier.ViewModels.SectieViewModels
{
    class ResearchViewModel : ObservableObject
    {
        #region fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        private DossierService _dossierService;
        private Dossier? _dossier;

        private bool _isSampleMode;
        #endregion

        #region constructers
        public ResearchViewModel(IAppNavigation appNavigation, UserMessage userMessage, DossierService dossierService, Dossier? dossier = null, SampleDossierViewModel? instance = null)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;
            _dossierService = dossierService;

            Research = _dossierService.CentralDossier.Research;

            if (dossier != null)
            {
                _dossier = dossier;

                using (var context = new ApplicationDbContext())
                {
                    try
                    {
                        var researchInDb = context.Research.FirstOrDefault(x => x.DossierId == _dossier.Id);
                        Research.ResearchSummary = researchInDb.ResearchSummary;
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
            ShowPolicyCommand = new RelayCommand(ExecuteShowPolicyView);
            ShowComplaintsAndSymptomsCommand = new RelayCommand(ExecuteShowComplaintsAndSymptomsView);

            Research.ResearchSummary = IsSampleMode ? "Urineonderzoek" : "";
        }

        public ResearchViewModel()
        {

        }
        #endregion

        #region properties
        public DossierService.Research Research
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
        public ICommand ShowPolicyCommand
        {
            get;
        }
        public ICommand ShowComplaintsAndSymptomsCommand
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
        private void ExecuteShowPolicyView(object? obj)
        {
            if (IsSampleMode != true)
            {
                if (string.IsNullOrWhiteSpace(Research.ResearchSummary))
                {
                    _userMessage.Text = "Alle invoervelden moeten ingevuld zijn voordat je verder kan.";
                    return;
                }
                else
                {
                    _appNavigation.ActiveViewModel = new PolicyViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
                }
            }
            else
            {
                _appNavigation.ActiveViewModel = new PolicyViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
            }
        }
        private void ExecuteShowComplaintsAndSymptomsView(object? obj)
        {
            _appNavigation.ActiveViewModel = new ComplaintsAndSymptomsViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
        }
        #endregion
    }
}