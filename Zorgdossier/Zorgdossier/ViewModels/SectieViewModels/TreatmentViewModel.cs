using System.Windows;
using System.Windows.Input;
using Zorgdossier.Databases;
using Zorgdossier.Helpers;
using Zorgdossier.Models;

namespace Zorgdossier.ViewModels.SectieViewModels
{
    class TreatmentViewModel : ObservableObject
    {
        #region fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        private DossierService _dossierService;
        private Dossier? _dossier;

        private bool _isSampleMode;
        #endregion

        #region constructers
        public TreatmentViewModel(IAppNavigation appNavigation, UserMessage userMessage, DossierService dossierService, Dossier? dossier = null, SampleDossierViewModel? instance = null)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;
            _dossierService = dossierService;

            Treatment = _dossierService.CentralDossier.Treatment;

            if (dossier != null)
            {
                _dossier = dossier;

                using (var context = new ApplicationDbContext())
                {
                    try
                    {
                        var treatmentInDb = context.Treatment.FirstOrDefault(x => x.DossierId == _dossier.Id);
                        Treatment.TreatmentSummary = treatmentInDb.TreatmentSummary;
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
            ShowFinishProgressCommand = new RelayCommand(ExecuteShowFinishedView);
            ShowContactAdvicesCommand = new RelayCommand(ExecuteShowContactAdvicesView);

            if (dossier == null)
            {
                Treatment.TreatmentSummary = IsSampleMode
                ? "Er wordt een antibioticakuur voorgeschreven, zoals nitrofurantoïne of fosfomycine, om de vermoedelijke urineweginfectie te behandelen.\n" +
                "De dosering en duur worden afgestemd op het klinische beeld en de ernst van de klachten."
                : "";
            }
        }

        public TreatmentViewModel()
        {

        }
        #endregion

        #region properties
        public DossierService.Treatment Treatment
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

        public ICommand ShowFinishProgressCommand
        {
            get;
        }

        public ICommand ShowContactAdvicesCommand
        {
            get;
        }
        #endregion

        #region methods
        private void ExecuteShowInfo(object? obj)
        {
            MessageBox.Show("Bepaal de behandelingen die passen bij de diagnose en de ernst van de klachten, zodat de patiënt de juiste zorg ontvangt.",
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

        private void ExecuteShowFinishedView(object? obj)
        {
            if (string.IsNullOrWhiteSpace(Treatment.TreatmentSummary))
            {
                _userMessage.Text = "Alle invoervelden moeten ingevuld zijn voordat je verder kan.";
                return;
            }
            else
            {
                _appNavigation.ActiveViewModel = new FinishProgressViewModel(_appNavigation, _userMessage, _dossierService, _dossier);
            }
        }

        private void ExecuteShowContactAdvicesView(object? obj)
        {
            _appNavigation.ActiveViewModel = new ContactAdvicesViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
        }
        #endregion
    }
}
