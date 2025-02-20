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
    class ContactAdvicesViewModel : ObservableObject
    {
        #region fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        private DossierService _dossierService;
        private Dossier? _dossier;

        private bool _isSampleMode;
        private string _hintTextAdvice = string.Empty;
        private string _hintTextContactAdvice = string.Empty;
        #endregion

        #region constructers
        public ContactAdvicesViewModel(IAppNavigation appNavigation, UserMessage userMessage, DossierService dossierService, Dossier? dossier = null, SampleDossierViewModel? instance = null)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;
            _dossierService = dossierService;

            ContactAdvice = _dossierService.CentralDossier.ContactAdvice;

            if (dossier != null)
            {
                _dossier = dossier;

                using (var context = new ApplicationDbContext())
                {
                    try
                    {
                        var contactAdviceInDb = context.ContactAdvice.FirstOrDefault(x => x.DossierId == _dossier.Id);
                        ContactAdvice.Advice = contactAdviceInDb.Advice;
                        ContactAdvice.ContactAdviceText = contactAdviceInDb.ContactAdviceText;
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
            ShowTreatmentCommand = new RelayCommand(ExecuteShowTreatmentView);
            ShowPolicyCommand = new RelayCommand(ExecuteShowPolicyView);
            ShowFinishProgressCommand = new RelayCommand(ExecuteShowFinishedView);

            HintTextAdvice = IsSampleMode ? "Voor in de tussentijd veel drinken, plas de blaas regelmatig en goed leeg. Stel bij aandrang het plassen niet uit. Ga direct plassen na seksueel contact." : "Welke adviezen geef je aan de patiënt?";
            HintTextContactAdvice = IsSampleMode ? "Contact opnemen bij koorts, bloederige urine of hevige pijn." : "Welke contact adviezen geef je aan de patiënt? (optioneel)";
        }

        public ContactAdvicesViewModel()
        {

        }
        #endregion

        #region properties
        public DossierService.ContactAdvice ContactAdvice
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

        public string HintTextAdvice
        {
            get => _hintTextAdvice;
            set
            {
                if (_hintTextAdvice != value)
                {
                    _hintTextAdvice = value;
                    OnPropertyChanged(nameof(HintTextAdvice));
                }
            }
        }

        public string HintTextContactAdvice
        {
            get => _hintTextContactAdvice;
            set
            {
                if (_hintTextContactAdvice != value)
                {
                    _hintTextContactAdvice = value;
                    OnPropertyChanged(nameof(HintTextContactAdvice));
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

        public ICommand ShowTreatmentCommand
        {
            get;
        }

        public ICommand ShowPolicyCommand
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
            MessageBox.Show("Geef de patiënt duidelijke zelfzorginstructies, inclusief advies over activiteiten die vermeden moeten worden. Informeer ook wanneer het noodzakelijk is om contact op te nemen, bijvoorbeeld bij verergering van de klachten of het optreden van nieuwe symptomen.",
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

        private void ExecuteShowTreatmentView(object? obj)
        {
            if (IsSampleMode != true)
            {
                if (string.IsNullOrWhiteSpace(ContactAdvice.Advice))
                {
                    _userMessage.Text = "Het invoerveld voor de adviezen voor de patiënt moet ingevuld zijn voordat je verder kan.";
                    return;
                }
                else
                {
                    _appNavigation.ActiveViewModel = new TreatmentViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
                }
            }
            else
            {
                _appNavigation.ActiveViewModel = new TreatmentViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
            }
        }

        private void ExecuteShowPolicyView(object? obj)
        {
            _appNavigation.ActiveViewModel = new PolicyViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
        }

        private void ExecuteShowFinishedView(object? obj)
        {
            _appNavigation.ActiveViewModel = new FinishProgressViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
        }
        #endregion
    }
}