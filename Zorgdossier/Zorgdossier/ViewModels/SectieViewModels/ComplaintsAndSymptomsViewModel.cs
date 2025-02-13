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
    class ComplaintsAndSymptomsViewModel : ObservableObject
    {
        #region fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        private DossierService _dossierService;
        private Dossier? _dossier;

        private bool _isSampleMode;
        private string _hintTextComplaintsSymptomsSummary = string.Empty;
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

                using (var context = new ApplicationDbContext())
                {
                    try
                    {
                        var complaintsSymptomsInDb = context.ComplaintsSymptoms.FirstOrDefault(x => x.DossierId == _dossier.Id);
                        ComplaintsSymptoms.ComplaintsSymptomsSummary = complaintsSymptomsInDb.ComplaintsSymptomsSummary;
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

            ShowMainViewCommand = new RelayCommand(ExecuteShowMainView);
            ShowResearchViewCommand = new RelayCommand(ExecuteShowResearchView);
            ShowOrganSelectionViewCommand = new RelayCommand(ExecuteShowOrganSelectionView);

            HintTextComplaintsSymptomsSummary = IsSampleMode
                ? "•\tPijn bij het plassen\n" +
                  "•\tBranderig gevoel bij het plassen\n" +
                  "•\tKleine beetjes plassen"
                : "Welke klachten en symptomen ervaart de patiënt?";
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
        public string HintTextComplaintsSymptomsSummary
        {
            get => _hintTextComplaintsSymptomsSummary;
            set
            {
                if (_hintTextComplaintsSymptomsSummary != value)
                {
                    _hintTextComplaintsSymptomsSummary = value;
                    OnPropertyChanged(nameof(HintTextComplaintsSymptomsSummary));
                }
            }
        }
        public SampleDossierViewModel Instance
        {
            get;
        }
        #endregion

        #region commands
        public ICommand ShowMainViewCommand
        {
            get;
        }
        public ICommand ShowResearchViewCommand
        {
            get;
        }
        public ICommand ShowOrganSelectionViewCommand
        {
            get;
        }
        #endregion

        #region methods
        private void ExecuteShowMainView(object? obj)
        {
            MessageBoxResult result = MessageBox.Show("Weet je zeker dat je terug wilt gaan naar de Home pagina? Al je voortgang van dit dossier raakt dan verloren.", "Waarschuwing", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                _appNavigation.ActiveViewModel = new HomeViewModel(_appNavigation, _userMessage);
            }
        }
        private void ExecuteShowResearchView(object? obj)
        {
            if (IsSampleMode != true)
            {
                if (string.IsNullOrWhiteSpace(ComplaintsSymptoms.ComplaintsSymptomsSummary))
                {
                    _userMessage.Text = "Alle invoervelden moeten ingevuld zijn voordat je verder kan.";
                    return;
                }
                else
                {
                    if (_dossier != null)
                    {
                        _appNavigation.ActiveViewModel = new ResearchViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
                    }
                    else
                    {
                        _appNavigation.ActiveViewModel = new ResearchViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
                    }
                }
            }
            else
            {
                if (_dossier != null)
                {
                    _appNavigation.ActiveViewModel = new ResearchViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
                }
                else
                {
                    _appNavigation.ActiveViewModel = new ResearchViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
                }
            }
        }
        private void ExecuteShowOrganSelectionView(object? obj)
        {
            if (_dossier != null)
            {
                _appNavigation.ActiveViewModel = new OrgansViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
            }
            else
            {
                _appNavigation.ActiveViewModel = new OrgansViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
            }
        }
        #endregion
    }
}