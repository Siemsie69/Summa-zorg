using System.Windows;
using System.Windows.Input;
using Zorgdossier.Databases;
using Zorgdossier.Helpers;
using Zorgdossier.Models;
using Zorgdossier.Views;

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
        #endregion

        #region constructers
        public BasicInformationViewModel(IAppNavigation appNavigation, UserMessage userMessage, DossierService dossierService, Dossier? dossier = null, SampleDossierViewModel? instance = null)
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

                using (var context = new ApplicationDbContext())
                {
                    try
                    {
                        var basicInformationInDb = context.BasicInformation.FirstOrDefault(x => x.DossierId == _dossier.Id);
                        BasicInformation.Name = basicInformationInDb.Name;
                        BasicInformation.Complaint = basicInformationInDb.Complaint;
                        BasicInformation.Gender = basicInformationInDb.Gender;

                        var phoneInDb = context.Phone.FirstOrDefault(x => x.DossierId == _dossier.Id);
                        Phone.PhoneSummary = phoneInDb.PhoneSummary;

                        var questionInDb = context.Question.FirstOrDefault(x => x.DossierId == _dossier.Id);
                        Question.QuestionSummary = questionInDb.QuestionSummary;

                        var organInDb = context.Organ.FirstOrDefault(x => x.DossierId == _dossier.Id);
                        Organ.Organs = organInDb.Organs;

                        var complaintsSymptomsInDb = context.ComplaintsSymptoms.FirstOrDefault(x => x.DossierId == _dossier.Id);
                        ComplaintsSymptoms.ComplaintsSymptomsSummary = complaintsSymptomsInDb.ComplaintsSymptomsSummary;

                        var researchInDb = context.Research.FirstOrDefault(x => x.DossierId == _dossier.Id);
                        Research.ResearchSummary = researchInDb.ResearchSummary;

                        var policyInDb = context.Policy.FirstOrDefault(x => x.DossierId == _dossier.Id);
                        Policy.Urgency = policyInDb.Urgency;
                        Policy.TriageCriteria = policyInDb.TriageCriteria;
                        Policy.PolicyChoice = policyInDb.PolicyChoice;
                        Policy.PolicyDateTime = policyInDb.PolicyDateTime;

                        var contactAdviceInDb = context.ContactAdvice.FirstOrDefault(x => x.DossierId == _dossier.Id);
                        ContactAdvice.Advice = contactAdviceInDb.Advice;
                        ContactAdvice.ContactAdviceText = contactAdviceInDb.ContactAdviceText;

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
                Instance = instance ?? throw new ArgumentNullException(nameof(instance));
                IsSampleMode = Instance.IsSampleMode;
            }

            ShowInfoCommand = new RelayCommand(ExecuteShowInfo);
            ShowHomeCommand = new RelayCommand(ExecuteShowMainView);
            ShowPhoneSummaryCommand = new RelayCommand(ExecuteShowPhoneSummaryView);
            ShowIntroductionCommand = new RelayCommand(ExecuteShowIntroductionView);
            ShowFinishProgressCommand = new RelayCommand(ExecuteShowFinishedView);

            if (dossier == null)
            {
                String SampleBasicInfoNameText = (string)Application.Current.Resources["SampleBasicInfoNameText"];
                String SampleBasicInfoComplaintText = (string)Application.Current.Resources["SampleBasicInfoComplaintText"];
                String SampleBasicInfoGenderText = (string)Application.Current.Resources["SampleBasicInfoGenderText"];

                BasicInformation.Name = IsSampleMode ? SampleBasicInfoNameText : BasicInformation.Name;
                BasicInformation.Complaint = IsSampleMode ? SampleBasicInfoComplaintText : BasicInformation.Complaint;
                BasicInformation.Gender = IsSampleMode ? SampleBasicInfoGenderText : BasicInformation.Gender;
            }
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

        public ICommand ShowPhoneSummaryCommand
        {
            get;
        }

        public ICommand ShowIntroductionCommand
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
            String BasicInformationMessageText = (string)Application.Current.Resources["BasicInformationMessageText"];
            String InfoMessageTitle = (string)Application.Current.Resources["InfoMessageTitle"];

            MessageBox.Show(BasicInformationMessageText, InfoMessageTitle, MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void ExecuteShowPhoneSummaryView(object? obj)
        {
            if (IsSampleMode != true)
            {
                if (string.IsNullOrWhiteSpace(BasicInformation.Name) || string.IsNullOrWhiteSpace(BasicInformation.Complaint) || BasicInformation.Gender == null)
                {
                    String StandardUserMessageText = (string)Application.Current.Resources["StandardUserMessageText"];

                    _userMessage.Text = StandardUserMessageText;
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

        private void ExecuteShowFinishedView(object? obj)
        {
            _appNavigation.ActiveViewModel = new FinishProgressViewModel(_appNavigation, _userMessage, _dossierService, _dossier);
        }
        #endregion
    }
}