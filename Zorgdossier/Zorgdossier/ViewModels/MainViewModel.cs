using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Zorgdossier.Databases;
using Zorgdossier.Helpers;
using Zorgdossier.Models;
using Zorgdossier.Views;

namespace Zorgdossier.ViewModels
{
    internal class MainViewModel : ObservableObject
    {
        #region Fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        private DossierService _dossierService = new DossierService();
        private bool _isMenuExpanded = true;
        #endregion

        #region Constructors
        public MainViewModel(IAppNavigation appNavigation, UserMessage userMessage)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;
            _appNavigation.ActiveViewModel = new HomeViewModel(_appNavigation, _userMessage, _dossierService);

            BasicInformation = _dossierService.CentralDossier.BasicInformation;
            Phone = _dossierService.CentralDossier.Phone;
            Question = _dossierService.CentralDossier.Question;
            Organ = _dossierService.CentralDossier.Organ;
            ComplaintsSymptoms = _dossierService.CentralDossier.ComplaintsSymptoms;
            Research = _dossierService.CentralDossier.Research;
            Policy = _dossierService.CentralDossier.Policy;
            ContactAdvice = _dossierService.CentralDossier.ContactAdvice;
            Treatment = _dossierService.CentralDossier.Treatment;


            ShowHomeCommand = new RelayCommand(ExecuteShowHome);
            ShowDossiersCommand = new RelayCommand(ExecuteShowDossiers);
            ShowExplanationCommand = new RelayCommand(ExecuteShowExplanation);
            ShowCreditsCommand = new RelayCommand(ExecuteShowCredits);
            ToggleMenuCommand = new RelayCommand(ExecuteToggleMenu);
            ShowSettingsCommand = new RelayCommand(ExecuteShowSettings);
        }

        public MainViewModel() { }
        #endregion

        #region Properties
        public IAppNavigation AppNavigation
        {
            get => _appNavigation;
        }

        public UserMessage UserMessage
        {
            get => _userMessage;
            set
            {
                _userMessage = value;
                OnPropertyChanged();
            }
        }

        public DossierService.BasicInformation BasicInformation
        {
            get;
        }

        public DossierService.Phone Phone
        {
            get;
        }

        public DossierService.Question Question
        {
            get;
        }

        public DossierService.Organ Organ
        {
            get;
        }

        public DossierService.ComplaintsSymptoms ComplaintsSymptoms
        {
            get;
        }

        public DossierService.Research Research
        {
            get;
        }

        public DossierService.Policy Policy
        {
            get;
        }

        public DossierService.ContactAdvice ContactAdvice
        {
            get;
        }

        public DossierService.Treatment Treatment
        {
            get;
        }

        public bool IsMenuExpanded
        {
            get => _isMenuExpanded;
            set
            {
                _isMenuExpanded = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsMenuCollapsed));
            }
        }
        #endregion

        #region Commands
        public bool IsMenuCollapsed => !IsMenuExpanded;
        public ICommand ShowHomeCommand { get; }
        public ICommand ShowDossiersCommand { get; }
        public ICommand ShowExplanationCommand { get; }
        public ICommand ShowCreditsCommand { get; }
        public ICommand ToggleMenuCommand { get; }
        public ICommand ShowSettingsCommand { get; }
        #endregion

        #region Methods
        private void ExecuteShowHome(object? obj)
        {
            if (!string.IsNullOrWhiteSpace(BasicInformation.Name) || !string.IsNullOrWhiteSpace(BasicInformation.Complaint) || !string.IsNullOrWhiteSpace(BasicInformation.Gender)
                || !string.IsNullOrWhiteSpace(Phone.PhoneSummary)
                || !string.IsNullOrWhiteSpace(Question.QuestionSummary)
                || !string.IsNullOrWhiteSpace(Organ.Organs)
                || !string.IsNullOrWhiteSpace(ComplaintsSymptoms.ComplaintsSymptomsSummary)
                || !string.IsNullOrWhiteSpace(Research.ResearchSummary)
                || !string.IsNullOrWhiteSpace(Policy.Urgency) || !string.IsNullOrWhiteSpace(Policy.TriageCriteria) || !string.IsNullOrWhiteSpace(Policy.PolicyChoice) || Policy.PolicyDateTime != null
                || !string.IsNullOrWhiteSpace(ContactAdvice.Advice) || !string.IsNullOrWhiteSpace(ContactAdvice.ContactAdviceText)
                || !string.IsNullOrWhiteSpace(Treatment.TreatmentSummary))
            {
                String ShowMainViewTitle = (string)Application.Current.Resources["ShowMainViewTitle"];
                String ShowMainViewMessage = (string)Application.Current.Resources["ShowMainViewMessage"];

                MessageBoxResult result = MessageBox.Show(ShowMainViewMessage, ShowMainViewTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    BasicInformation.Name = string.Empty;
                    BasicInformation.Complaint = string.Empty;
                    BasicInformation.Gender = string.Empty;
                    Phone.PhoneSummary = string.Empty;
                    Question.QuestionSummary = string.Empty;
                    Organ.Organs = string.Empty;
                    ComplaintsSymptoms.ComplaintsSymptomsSummary = string.Empty;
                    Research.ResearchSummary = string.Empty;
                    Policy.Urgency = string.Empty;
                    Policy.TriageCriteria = string.Empty;
                    Policy.PolicyChoice = string.Empty;
                    Policy.PolicyDateTime = null;
                    ContactAdvice.Advice = string.Empty;
                    ContactAdvice.ContactAdviceText = string.Empty;
                    Treatment.TreatmentSummary = string.Empty;

                    _appNavigation.ActiveViewModel = new HomeViewModel(_appNavigation, _userMessage, _dossierService);
                }
                else
                {
                    return;
                }
            }
            else
            {
                _appNavigation.ActiveViewModel = new HomeViewModel(_appNavigation, _userMessage, _dossierService);
            }
        }

        private void ExecuteShowDossiers(object? obj)
        {
            if (!string.IsNullOrWhiteSpace(BasicInformation.Name) || !string.IsNullOrWhiteSpace(BasicInformation.Complaint) || !string.IsNullOrWhiteSpace(BasicInformation.Gender)
                || !string.IsNullOrWhiteSpace(Phone.PhoneSummary)
                || !string.IsNullOrWhiteSpace(Question.QuestionSummary)
                || !string.IsNullOrWhiteSpace(Organ.Organs)
                || !string.IsNullOrWhiteSpace(ComplaintsSymptoms.ComplaintsSymptomsSummary)
                || !string.IsNullOrWhiteSpace(Research.ResearchSummary)
                || !string.IsNullOrWhiteSpace(Policy.Urgency) || !string.IsNullOrWhiteSpace(Policy.TriageCriteria) || !string.IsNullOrWhiteSpace(Policy.PolicyChoice) || Policy.PolicyDateTime != null
                || !string.IsNullOrWhiteSpace(ContactAdvice.Advice) || !string.IsNullOrWhiteSpace(ContactAdvice.ContactAdviceText)
                || !string.IsNullOrWhiteSpace(Treatment.TreatmentSummary))
            {
                String ShowMainViewTitle = (string)Application.Current.Resources["ShowMainViewTitle"];
                String ShowMainViewMessage = (string)Application.Current.Resources["ShowMainViewMessage"];

                MessageBoxResult result = MessageBox.Show(ShowMainViewMessage, ShowMainViewTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    BasicInformation.Name = string.Empty;
                    BasicInformation.Complaint = string.Empty;
                    BasicInformation.Gender = string.Empty;
                    Phone.PhoneSummary = string.Empty;
                    Question.QuestionSummary = string.Empty;
                    Organ.Organs = string.Empty;
                    ComplaintsSymptoms.ComplaintsSymptomsSummary = string.Empty;
                    Research.ResearchSummary = string.Empty;
                    Policy.Urgency = string.Empty;
                    Policy.TriageCriteria = string.Empty;
                    Policy.PolicyChoice = string.Empty;
                    Policy.PolicyDateTime = null;
                    ContactAdvice.Advice = string.Empty;
                    ContactAdvice.ContactAdviceText = string.Empty;
                    Treatment.TreatmentSummary = string.Empty;

                    _appNavigation.ActiveViewModel = new DossiersViewModel(_appNavigation, _userMessage, _dossierService);
                }
                else
                {
                    return;
                }
            }
            else
            {
                _appNavigation.ActiveViewModel = new DossiersViewModel(_appNavigation, _userMessage, _dossierService);
            }
        }

        private void ExecuteShowExplanation(object? obj)
        {
            if (!string.IsNullOrWhiteSpace(BasicInformation.Name) || !string.IsNullOrWhiteSpace(BasicInformation.Complaint) || !string.IsNullOrWhiteSpace(BasicInformation.Gender)
                || !string.IsNullOrWhiteSpace(Phone.PhoneSummary)
                || !string.IsNullOrWhiteSpace(Question.QuestionSummary)
                || !string.IsNullOrWhiteSpace(Organ.Organs)
                || !string.IsNullOrWhiteSpace(ComplaintsSymptoms.ComplaintsSymptomsSummary)
                || !string.IsNullOrWhiteSpace(Research.ResearchSummary)
                || !string.IsNullOrWhiteSpace(Policy.Urgency) || !string.IsNullOrWhiteSpace(Policy.TriageCriteria) || !string.IsNullOrWhiteSpace(Policy.PolicyChoice) || Policy.PolicyDateTime != null
                || !string.IsNullOrWhiteSpace(ContactAdvice.Advice) || !string.IsNullOrWhiteSpace(ContactAdvice.ContactAdviceText)
                || !string.IsNullOrWhiteSpace(Treatment.TreatmentSummary))
            {
                String ShowMainViewTitle = (string)Application.Current.Resources["ShowMainViewTitle"];
                String ShowMainViewMessage = (string)Application.Current.Resources["ShowMainViewMessage"];

                MessageBoxResult result = MessageBox.Show(ShowMainViewMessage, ShowMainViewTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    BasicInformation.Name = string.Empty;
                    BasicInformation.Complaint = string.Empty;
                    BasicInformation.Gender = string.Empty;
                    Phone.PhoneSummary = string.Empty;
                    Question.QuestionSummary = string.Empty;
                    Organ.Organs = string.Empty;
                    ComplaintsSymptoms.ComplaintsSymptomsSummary = string.Empty;
                    Research.ResearchSummary = string.Empty;
                    Policy.Urgency = string.Empty;
                    Policy.TriageCriteria = string.Empty;
                    Policy.PolicyChoice = string.Empty;
                    Policy.PolicyDateTime = null;
                    ContactAdvice.Advice = string.Empty;
                    ContactAdvice.ContactAdviceText = string.Empty;
                    Treatment.TreatmentSummary = string.Empty;

                    _appNavigation.ActiveViewModel = new ExplanationViewModel(_appNavigation, _userMessage, _dossierService);
                }
                else
                {
                    return;
                }
            }
            else
            {
                _appNavigation.ActiveViewModel = new ExplanationViewModel(_appNavigation, _userMessage, _dossierService);
            }
        }

        private void ExecuteShowCredits(object? obj)
        {
            if (!string.IsNullOrWhiteSpace(BasicInformation.Name) || !string.IsNullOrWhiteSpace(BasicInformation.Complaint) || !string.IsNullOrWhiteSpace(BasicInformation.Gender)
                || !string.IsNullOrWhiteSpace(Phone.PhoneSummary)
                || !string.IsNullOrWhiteSpace(Question.QuestionSummary)
                || !string.IsNullOrWhiteSpace(Organ.Organs)
                || !string.IsNullOrWhiteSpace(ComplaintsSymptoms.ComplaintsSymptomsSummary)
                || !string.IsNullOrWhiteSpace(Research.ResearchSummary)
                || !string.IsNullOrWhiteSpace(Policy.Urgency) || !string.IsNullOrWhiteSpace(Policy.TriageCriteria) || !string.IsNullOrWhiteSpace(Policy.PolicyChoice) || Policy.PolicyDateTime != null
                || !string.IsNullOrWhiteSpace(ContactAdvice.Advice) || !string.IsNullOrWhiteSpace(ContactAdvice.ContactAdviceText)
                || !string.IsNullOrWhiteSpace(Treatment.TreatmentSummary))
            {
                String ShowMainViewTitle = (string)Application.Current.Resources["ShowMainViewTitle"];
                String ShowMainViewMessage = (string)Application.Current.Resources["ShowMainViewMessage"];

                MessageBoxResult result = MessageBox.Show(ShowMainViewMessage, ShowMainViewTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    BasicInformation.Name = string.Empty;
                    BasicInformation.Complaint = string.Empty;
                    BasicInformation.Gender = string.Empty;
                    Phone.PhoneSummary = string.Empty;
                    Question.QuestionSummary = string.Empty;
                    Organ.Organs = string.Empty;
                    ComplaintsSymptoms.ComplaintsSymptomsSummary = string.Empty;
                    Research.ResearchSummary = string.Empty;
                    Policy.Urgency = string.Empty;
                    Policy.TriageCriteria = string.Empty;
                    Policy.PolicyChoice = string.Empty;
                    Policy.PolicyDateTime = null;
                    ContactAdvice.Advice = string.Empty;
                    ContactAdvice.ContactAdviceText = string.Empty;
                    Treatment.TreatmentSummary = string.Empty;

                    _appNavigation.ActiveViewModel = new CreditsViewModel(_appNavigation, _userMessage);
                }
                else
                {
                    return;
                }
            }
            else
            {
                _appNavigation.ActiveViewModel = new CreditsViewModel(_appNavigation, _userMessage);
            }
        }

        private void ExecuteToggleMenu(object? obj)
        {
            var targetWidth = IsMenuExpanded ? new GridLength(80) : new GridLength(200);
            var menuColumn = (Application.Current.MainWindow as MainView)?.FindName("MenuColumn") as ColumnDefinition;

            if (menuColumn != null)
            {
                var gridLengthAnimation = new GridLengthAnimation
                {
                    From = menuColumn.Width,
                    To = targetWidth,
                    Duration = TimeSpan.FromMilliseconds(300),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
                };

                var storyboard = new Storyboard();
                storyboard.Children.Add(gridLengthAnimation);

                Storyboard.SetTarget(gridLengthAnimation, menuColumn);
                Storyboard.SetTargetProperty(gridLengthAnimation, new PropertyPath(ColumnDefinition.WidthProperty));

                storyboard.Begin();
            }

            IsMenuExpanded = !IsMenuExpanded;
        }

        private void ExecuteShowSettings(object? obj)
        {
            if (!string.IsNullOrWhiteSpace(BasicInformation.Name) || !string.IsNullOrWhiteSpace(BasicInformation.Complaint) || !string.IsNullOrWhiteSpace(BasicInformation.Gender)
                || !string.IsNullOrWhiteSpace(Phone.PhoneSummary)
                || !string.IsNullOrWhiteSpace(Question.QuestionSummary)
                || !string.IsNullOrWhiteSpace(Organ.Organs)
                || !string.IsNullOrWhiteSpace(ComplaintsSymptoms.ComplaintsSymptomsSummary)
                || !string.IsNullOrWhiteSpace(Research.ResearchSummary)
                || !string.IsNullOrWhiteSpace(Policy.Urgency) || !string.IsNullOrWhiteSpace(Policy.TriageCriteria) || !string.IsNullOrWhiteSpace(Policy.PolicyChoice) || Policy.PolicyDateTime != null
                || !string.IsNullOrWhiteSpace(ContactAdvice.Advice) || !string.IsNullOrWhiteSpace(ContactAdvice.ContactAdviceText)
                || !string.IsNullOrWhiteSpace(Treatment.TreatmentSummary))
            {
                String ShowMainViewTitle = (string)Application.Current.Resources["ShowMainViewTitle"];
                String ShowMainViewMessage = (string)Application.Current.Resources["ShowMainViewMessage"];

                MessageBoxResult result = MessageBox.Show(ShowMainViewMessage, ShowMainViewTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    BasicInformation.Name = string.Empty;
                    BasicInformation.Complaint = string.Empty;
                    BasicInformation.Gender = string.Empty;
                    Phone.PhoneSummary = string.Empty;
                    Question.QuestionSummary = string.Empty;
                    Organ.Organs = string.Empty;
                    ComplaintsSymptoms.ComplaintsSymptomsSummary = string.Empty;
                    Research.ResearchSummary = string.Empty;
                    Policy.Urgency = string.Empty;
                    Policy.TriageCriteria = string.Empty;
                    Policy.PolicyChoice = string.Empty;
                    Policy.PolicyDateTime = null;
                    ContactAdvice.Advice = string.Empty;
                    ContactAdvice.ContactAdviceText = string.Empty;
                    Treatment.TreatmentSummary = string.Empty;

                    var dbContext = new ApplicationDbContext();
                    _appNavigation.ActiveViewModel = new SettingsViewModel(_appNavigation, _userMessage, dbContext);
                }
                else
                {
                    return;
                }
            }
            else
            {
                var dbContext = new ApplicationDbContext();
                _appNavigation.ActiveViewModel = new SettingsViewModel(_appNavigation, _userMessage, dbContext);
            }
        }
        #endregion
    }
}