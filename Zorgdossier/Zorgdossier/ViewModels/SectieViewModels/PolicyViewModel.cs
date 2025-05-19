using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Zorgdossier.Databases;
using Zorgdossier.Helpers;
using Zorgdossier.Models;
using Zorgdossier.Views;

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

            BasicInformation = _dossierService.CentralDossier.BasicInformation;
            Phone = _dossierService.CentralDossier.Phone;
            Question = _dossierService.CentralDossier.Question;
            Organ = _dossierService.CentralDossier.Organ;
            ComplaintsSymptoms = _dossierService.CentralDossier.ComplaintsSymptoms;
            Research = _dossierService.CentralDossier.Research;
            Policy = _dossierService.CentralDossier.Policy;
            ContactAdvice = _dossierService.CentralDossier.ContactAdvice;
            Treatment = _dossierService.CentralDossier.Treatment;

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
                String SamplePolicyUrgencyText = (string)Application.Current.Resources["SamplePolicyUrgencyText"];
                String SampleFirstPolicyTriageText = (string)Application.Current.Resources["SampleFirstPolicyTriageText"];
                String SampleSecondPolicyTriageText = (string)Application.Current.Resources["SampleSecondPolicyTriageText"];
                String SampleThirdPolicyTriageText = (string)Application.Current.Resources["SampleThirdPolicyTriageText"];
                String SamplePolicyChoiceText = (string)Application.Current.Resources["SamplePolicyChoiceText"];

                Policy.Urgency = IsSampleMode ? SamplePolicyUrgencyText : Policy.Urgency;
                Policy.TriageCriteria = IsSampleMode
                    ? SampleFirstPolicyTriageText + "\n" +
                    SampleSecondPolicyTriageText + "\n" +
                    SampleThirdPolicyTriageText
                    : Policy.TriageCriteria;
                Policy.PolicyChoice = IsSampleMode ? SamplePolicyChoiceText : Policy.PolicyChoice;
                Policy.PolicyDateTime = IsSampleMode ? DateTime.Now : Policy.PolicyDateTime;
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
        public bool Appointment => Policy?.PolicyChoice == "Afspraak" || Policy?.PolicyChoice == "Appointment";
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

                string message = (string)Application.Current.FindResource("ShowMainViewMessage");
                string title = (string)Application.Current.FindResource("ShowMainViewTitle");

                MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Warning);

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
                else if (Policy.PolicyDateTime < DateTime.Now)
                {
                    String PolicyDateTimeText = (string)Application.Current.Resources["PolicyDateTimeText"];

                    _userMessage.Text = PolicyDateTimeText;
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