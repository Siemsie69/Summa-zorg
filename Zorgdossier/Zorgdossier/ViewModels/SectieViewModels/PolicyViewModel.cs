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
            ShowFinishProgressCommand = new RelayCommand(ExecuteShowFinishedView);

            if (dossier == null)
            {
                Policy.Urgency = IsSampleMode ? "U1 Levensbedreigend" : Policy.Urgency;
                Policy.TriageCriteria = IsSampleMode
                    ? "Categorie: Urologie | " +
                    "Hoofdklacht: Pijn bij het plassen (dysurie) | " +
                    "Leeftijd: 23 jaar\n" +
                    "Triagecriterium: " +
                    "Waarschijnlijke oorzaak: Mogelijke urineweginfectie (UTI) of urethritis. " +
                    "Urgentiecode: U3 (Dringend: Afspraak binnen 24 uur nodig).\n" +
                    "Aanvullend: Patiënt vraagt specifiek om een afspraak."
                    : Policy.TriageCriteria;
                Policy.PolicyChoice = IsSampleMode ? "Afspraak" : Policy.PolicyChoice;
                Policy.PolicyDateTime = IsSampleMode ? DateTime.Now : null;
            }

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

        public ICommand ShowFinishProgressCommand
        {
            get;
        }
        #endregion

        #region methods
        private void ExecuteShowInfo(object? obj)
        {
            String PolicyMessageText = (string)Application.Current.Resources["PolicyMessageText"];
            String InfoMessageTitle = (string)Application.Current.Resources["InfoMessageTitle"];

            MessageBox.Show(PolicyMessageText, InfoMessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExecuteShowMainView(object? obj)
        {
            MessageBoxResult result = MessageBox.Show("Weet je zeker dat je terug wilt gaan naar de Home pagina? Al je voortgang van dit dossier raakt dan verloren.", "Waarschuwing", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                _appNavigation.ActiveViewModel = new MainViewModel(_appNavigation, _userMessage);
            }
        }

        private void ExecuteShowContactAdvicesView(object? obj)
        {
            if (IsSampleMode != true)
            {
                if (string.IsNullOrWhiteSpace(Policy.Urgency) || string.IsNullOrWhiteSpace(Policy.PolicyChoice))
                {
                    String PolicyUserMessageText = (string)Application.Current.Resources["PolicyUserMessageText"];

                    _userMessage.Text = PolicyUserMessageText;
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

        private void ExecuteShowFinishedView(object? obj)
        {
            _appNavigation.ActiveViewModel = new FinishProgressViewModel(_appNavigation, _userMessage, _dossierService, _dossier);
        }
        #endregion
    }
}