using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private string _hintTextUrgencyChoice = string.Empty;
        private string _hintTextTriageCriteria = string.Empty;
        private string _hintTextPolicyChoice = string.Empty;
        private DateTime _hintTextPolicyDateTime;
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
            ShowFinishProgressCommand = new RelayCommand(ExecuteShowFinishedView);

            HintTextUrgencyChoice = IsSampleMode ? "U3 Dringend" : "U1 Levensbedreigend";
            HintTextTriageCriteria = IsSampleMode
                ? "Categorie: Urologie | " +
                "Hoofdklacht: Pijn bij het plassen (dysurie) | " +
                "Leeftijd: 23 jaar\n" +
                "Triagecriterium: " +
                "Waarschijnlijke oorzaak: Mogelijke urineweginfectie (UTI) of urethritis. " +
                "Urgentiecode: U3 (Dringend: Afspraak binnen 24 uur nodig).\n" +
                "Aanvullend: Patiënt vraagt specifiek om een afspraak."
                : "Vul hier het triagecriterium in (optioneel).";
            HintTextPolicyChoice = IsSampleMode ? "Afspraak" : "Afspraak";
            HintTextPolicyDateTime = IsSampleMode ? DateTime.Now : DateTime.Now;

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

        public string HintTextUrgencyChoice
        {
            get => _hintTextUrgencyChoice;
            set
            {
                if (_hintTextUrgencyChoice != value)
                {
                    _hintTextUrgencyChoice = value;
                    OnPropertyChanged(nameof(HintTextUrgencyChoice));
                }
            }
        }

        public string HintTextTriageCriteria
        {
            get => _hintTextTriageCriteria;
            set
            {
                if (_hintTextTriageCriteria != value)
                {
                    _hintTextTriageCriteria = value;
                    OnPropertyChanged(nameof(HintTextTriageCriteria));
                }
            }
        }

        public string HintTextPolicyChoice
        {
            get => _hintTextPolicyChoice;
            set
            {
                if (_hintTextPolicyChoice != value)
                {
                    _hintTextPolicyChoice = value;
                    OnPropertyChanged(nameof(HintTextPolicyChoice));
                }
            }
        }

        public DateTime HintTextPolicyDateTime
        {
            get => _hintTextPolicyDateTime;
            set
            {
                if (_hintTextPolicyDateTime != value)
                {
                    _hintTextPolicyDateTime = value;
                    OnPropertyChanged(nameof(HintTextPolicyDateTime));
                }
            }
        }

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
            MessageBox.Show("Op basis van de urgentie en de triagecriteria kunt u nu beslissen of het noodzakelijk is om direct een afspraak bij de dokter te plannen en welke tijd het beste past voor de patiënt.",
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

        private void ExecuteShowFinishedView(object? obj)
        {
            _appNavigation.ActiveViewModel = new FinishProgressViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
        }
        #endregion
    }
}