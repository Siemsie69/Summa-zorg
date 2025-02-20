using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Zorgdossier.Databases;
using Zorgdossier.Helpers;
using Zorgdossier.Models;

namespace Zorgdossier.ViewModels.SectieViewModels
{
    class PolicyViewModel : ObservableObject
    {
        #region fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        private DossierService _dossierService;
        private Dossier? _dossier;

        private bool _isSampleMode;
        #endregion

        #region constructers
        public PolicyViewModel(IAppNavigation appNavigation, UserMessage userMessage, DossierService dossierService, Dossier? dossier = null, SampleDossierViewModel? instance = null)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;
            _dossierService = dossierService;

            Policy = _dossierService.CentralDossier.Policy;

            if (dossier != null)
            {
                _dossier = dossier;

                using (var context = new ApplicationDbContext())
                {
                    try
                    {
                        var policyInDb = context.Policy.FirstOrDefault(x => x.DossierId == _dossier.Id);
                        Policy.Urgency = policyInDb.Urgency;
                        Policy.TriageCriteria = policyInDb.TriageCriteria;
                        Policy.PolicyChoice = policyInDb.PolicyChoice;
                        Policy.PolicyDateTime = policyInDb.PolicyDateTime;
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
            ShowContactAdvicesCommand = new RelayCommand(ExecuteShowContactAdvicesView);
            ShowResearchCommand = new RelayCommand(ExecuteShowResearchView);

            Policy.Urgency = IsSampleMode ? "U1 Levensbedreigend" : "";
            Policy.TriageCriteria = IsSampleMode
                ? "Categorie: Urologie | " +
                "Hoofdklacht: Pijn bij het plassen (dysurie) | " +
                "Leeftijd: 23 jaar\n" +
                "Triagecriterium: " +
                "Waarschijnlijke oorzaak: Mogelijke urineweginfectie (UTI) of urethritis. " +
                "Urgentiecode: U3 (Dringend: Afspraak binnen 24 uur nodig).\n" +
                "Aanvullend: Patiënt vraagt specifiek om een afspraak."
                : "";
            Policy.PolicyChoice = IsSampleMode ? "Afspraak" : "";
            Policy.PolicyDateTime = IsSampleMode ? DateTime.Now : DateTime.Now;

            if (Policy != null)
            {
                Policy.PropertyChanged += Policy_PropertyChanged;
            }
        }

        public PolicyViewModel()
        {

        }
        #endregion

        #region properties
        public DossierService.Policy Policy
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
        public bool Appointment => Policy?.PolicyChoice == "Afspraak";
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
        public ICommand ShowContactAdvicesCommand
        {
            get;
        }
        public ICommand ShowResearchCommand
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
        private void ExecuteShowContactAdvicesView(object? obj)
        {
            if (IsSampleMode != true)
            {
                if (Policy.Urgency == null || Policy.PolicyChoice == null)
                {
                    _userMessage.Text = "De invoervelden voor de urgentie en voor de beleidskeuze moeten ingevuld zijn voordat je verder kan.";
                    return;
                }
                else
                {
                    _appNavigation.ActiveViewModel = new ContactAdvicesViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
                }
            }
            else
            {
                _appNavigation.ActiveViewModel = new ContactAdvicesViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
            }
        }
        private void ExecuteShowResearchView(object? obj)
        {
            _appNavigation.ActiveViewModel = new ResearchViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
        }
        private void Policy_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Policy.PolicyChoice))
            {
                OnPropertyChanged(nameof(Appointment));
            }
        }
        #endregion
    }
}