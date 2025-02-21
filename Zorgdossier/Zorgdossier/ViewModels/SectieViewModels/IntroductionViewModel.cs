using System.Windows;
using System.Windows.Input;
using Zorgdossier.Databases;
using Zorgdossier.Helpers;
using Zorgdossier.Models;

namespace Zorgdossier.ViewModels.SectieViewModels
{
    class IntroductionViewModel : ObservableObject
    {
        #region fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        private Dossier _dossier;
        private DossierService _dossierService;
        private string _introductionText;
        private string _buttonText;
        #endregion

        #region constructers
        public IntroductionViewModel(IAppNavigation appNavigation, UserMessage userMessage, DossierService dossierService, Dossier? dossier = null)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;
            _dossierService = dossierService;
            if(dossier != null)
            {
                _dossier = dossier;
                IntroductionText = "Welkom bij de introductie van dossiers. Je staat op het punt om een bestaand dossier te wijzigen. Alle gegevens van het bestaande dossier zijn al ingevuld voor je, waardoor het makkelijker is om de gegevens te wijzigen. Verder werkt dit proces precies hetzelfde als het aanmaken van een dossier. Je kunt hierna het gewijzigde dossier ook weer exporteren naar een Pdf-bestand.";
                ButtonText = "Wijzigen beginnen";
            }
            else
            {
                IntroductionText = "Welkom bij de introductie van dossiers. Hier kun je starten met het aanmaken van een nieuw dossier, gebaseerd op een casus, rollenspel of uitleg uit de les.Deze stap helpt je om het proces van het invullen van patiëntendossiers te oefenen, inclusief het verzamelen van gegevens en het vastleggen van belangrijke informatie. Of je nu net begint of verdergaat, deze oefening is essentieel voor het ontwikkelen van je vaardigheden.";
                ButtonText = "Aanmaken beginnen";
            }
            ShowInfoCommand = new RelayCommand(ExecuteShowInfo);
            ShowDossiersCommand = new RelayCommand(ExecuteShowDossiersView);
            ShowBasicInformationCommand = new RelayCommand(ExecuteShowBasicInformation);
        }
        public IntroductionViewModel()
        {
        }
        #endregion

        #region properties
        public string IntroductionText
        {
            get => _introductionText;
            set
            {
                if (_introductionText != value)
                {
                    _introductionText = value;
                    OnPropertyChanged(nameof(IntroductionText));
                }
            }
        }
        public string ButtonText
        {
            get => _buttonText;
            set
            {
                if (_buttonText != value)
                {
                    _buttonText = value;
                    OnPropertyChanged(nameof(ButtonText));
                }
            }
        }
        #endregion

        #region commands
        public ICommand ShowInfoCommand
        {
            get;
        }
        public ICommand ShowDossiersCommand
        {
            get;
        }
        public ICommand ShowBasicInformationCommand
        {
            get;
        }
        #endregion

        #region methods
        private void ExecuteShowInfo(object? obj)
        {
            MessageBox.Show("Heb je vragen of onzekerheden? Aarzel niet om hulp te vragen aan je docent of medestudenten. Zij kunnen je ondersteunen bij het correct invullen en begrijpen van het dossier.",
                            "Aanvullende Informatie en Handige Tips", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void ExecuteShowDossiersView(object? obj)
        {
            _appNavigation.ActiveViewModel = new DossiersViewModel(_appNavigation, _userMessage);
        }
        private void ExecuteShowBasicInformation(object? obj)
        {
            _appNavigation.ActiveViewModel = new BasicInformationViewModel(_appNavigation, _userMessage, _dossierService, _dossier);
        }
        #endregion
    }
}
