using System.Windows;
using System.Windows.Input;
using Zorgdossier.Databases;
using Zorgdossier.Helpers;
using Zorgdossier.Models;
using Zorgdossier.Views;

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
            ShowFinishProgressCommand = new RelayCommand(ExecuteShowFinishedView);
            ShowContactAdvicesCommand = new RelayCommand(ExecuteShowContactAdvicesView);

            if (dossier == null)
            {
                String SampleFirstTreatmentText = (string)Application.Current.Resources["SampleFirstTreatmentText"];
                String SampleSecondTreatmentText = (string)Application.Current.Resources["SampleSecondTreatmentText"];

                Treatment.TreatmentSummary = IsSampleMode
                ? SampleFirstTreatmentText + "\n" +
                SampleSecondTreatmentText
                : Treatment.TreatmentSummary;
            }
        }

        public TreatmentViewModel()
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
            String TreatmentMessageText = (string)Application.Current.Resources["TreatmentMessageText"];
            String InfoMessageTitle = (string)Application.Current.Resources["InfoMessageTitle"];

            MessageBox.Show(TreatmentMessageText, InfoMessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void ExecuteShowFinishedView(object? obj)
        {
            if (string.IsNullOrWhiteSpace(Treatment.TreatmentSummary))
            {
                String StandardUserMessageText = (string)Application.Current.Resources["StandardUserMessageText"];

                _userMessage.Text = StandardUserMessageText;
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
