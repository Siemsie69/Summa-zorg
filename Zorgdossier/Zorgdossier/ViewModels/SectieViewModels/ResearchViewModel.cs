using System;
using System.Collections.Generic;
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
    class ResearchViewModel : ObservableObject
    {
        #region fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        private DossierService _dossierService;
        private Dossier? _dossier;

        private bool _isSampleMode;
        private string _hintTextResearchSummary = string.Empty;
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
            ShowFinishProgressCommand = new RelayCommand(ExecuteShowFinishedView);

            HintTextResearchSummary = IsSampleMode ? "Urineonderzoek " : "Welke onderzoeken moeten er worden uitgevoerd?";
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

        public string HintTextResearchSummary
        {
            get => _hintTextResearchSummary;
            set
            {
                if (_hintTextResearchSummary != value)
                {
                    _hintTextResearchSummary = value;
                    OnPropertyChanged(nameof(HintTextResearchSummary));
                }
            }
        }

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

        public ICommand ShowFinishProgressCommand
        {
            get;
        }
        #endregion

        #region methods
        private void ExecuteShowInfo(object? obj)
        {
            MessageBox.Show("Focus op onderzoeken die niet alleen de diagnose ondersteunen, maar ook helpen om complicaties of andere onderliggende aandoeningen uit te sluiten, zodat je het juiste behandelplan kunt bepalen.",
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

        private void ExecuteShowFinishedView(object? obj)
        {
            _appNavigation.ActiveViewModel = new FinishProgressViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
        }
        #endregion
    }
}