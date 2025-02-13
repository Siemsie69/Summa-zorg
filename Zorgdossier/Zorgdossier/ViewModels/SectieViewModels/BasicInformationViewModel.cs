using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Zorgdossier.Databases;
using Zorgdossier.Helpers;
using Zorgdossier.Models;

namespace Zorgdossier.ViewModels.SectieViewModels
{
    class BasicInformationViewModel : ObservableObject
    {
        #region fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        private Dossier? _dossier;
        private DossierService _dossierService;

        private bool _isSampleMode;
        private string _hintTextName = string.Empty;
        private string _hintTextComplaint = string.Empty;
        private string _hintTextType = string.Empty;
        #endregion

        #region constructers
        public BasicInformationViewModel(IAppNavigation appNavigation, UserMessage userMessage, DossierService dossierService, Dossier? dossier = null, SampleDossierViewModel? instance = null)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;
            _dossierService = dossierService;

            BasicInformation = _dossierService.CentralDossier.BasicInformation;

            if (dossier != null)
            {
                _dossier = dossier;

                using (var context = new ApplicationDbContext())
                {
                    try
                    {
                        var basicInformationInDb = context.BasicInformation.FirstOrDefault(x => x.DossierId == _dossier.Id);
                        BasicInformation.Name = basicInformationInDb.Name;
                        BasicInformation.Complaint = basicInformationInDb.Complaint;
                        BasicInformation.Gender = basicInformationInDb.Gender;
                    }
                    catch (Exception ex)
                    {
                        _userMessage.Text = ("Fout met het ophalen van bestaande data: " + ex.Message);
                    }
                }
            }
            if (instance != null)
            {
                Instance = instance ?? throw new ArgumentNullException(nameof(instance));
                IsSampleMode = Instance.IsSampleMode;
            }

            ShowInfoCommand = new RelayCommand(ExecuteShowInfo);
            ShowHomeCommand = new RelayCommand(ExecuteShowMainView);
            ShowPhoneSummaryCommand = new RelayCommand(ExecuteShowPhoneSummaryView);
            ShowIntroductionCommand = new RelayCommand(ExecuteShowIntroductionView);

            HintTextName = IsSampleMode ? "Jan Jansen" : "Naam Patiënt";
            HintTextComplaint = IsSampleMode ? "Buikpijn" : "Klacht Patiënt";
            HintTextType = IsSampleMode ? "Man" : "Man";
        }

        public BasicInformationViewModel()
        {

        }
        #endregion

        #region properties
        public DossierService.BasicInformation BasicInformation
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
        public string HintTextName
        {
            get => _hintTextName;
            set
            {
                if (_hintTextName != value)
                {
                    _hintTextName = value;
                    OnPropertyChanged(nameof(HintTextName));
                }
            }
        }
        public string HintTextComplaint
        {
            get => _hintTextComplaint;
            set
            {
                if (_hintTextComplaint != value)
                {
                    _hintTextComplaint = value;
                    OnPropertyChanged(nameof(HintTextComplaint));
                }
            }
        }
        public string HintTextType
        {
            get => _hintTextType;
            set
            {
                if (_hintTextType != value)
                {
                    _hintTextType = value;
                    OnPropertyChanged(nameof(HintTextType));
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
        public ICommand ShowPhoneSummaryCommand
        {
            get;
        }
        public ICommand ShowIntroductionCommand
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
        private void ExecuteShowPhoneSummaryView(object? obj)
        {
            if (IsSampleMode != true)
            {
                if (string.IsNullOrWhiteSpace(BasicInformation.Name) || string.IsNullOrWhiteSpace(BasicInformation.Complaint) || BasicInformation.Gender == null)
                {
                    _userMessage.Text = "Alle invoervelden moeten ingevuld zijn voordat je verder kan.";
                    return;
                }
                else
                {
                    _appNavigation.ActiveViewModel = new PhoneSummaryViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
                }
            }
            else
            {
                _appNavigation.ActiveViewModel = new PhoneSummaryViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
            }
        }
        private void ExecuteShowIntroductionView(object? obj)
        {
            _appNavigation.ActiveViewModel = new IntroductionViewModel(_appNavigation, _userMessage, _dossierService, _dossier);
        }
        #endregion
    }
}