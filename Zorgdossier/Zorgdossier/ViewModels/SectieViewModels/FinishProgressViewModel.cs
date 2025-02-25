using System.Windows;
using System.Windows.Input;
using Zorgdossier.Databases;
using Zorgdossier.Helpers;
using Zorgdossier.Models;

namespace Zorgdossier.ViewModels.SectieViewModels
{
    class FinishProgressViewModel : ObservableObject
    {
        #region fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        private DossierService _dossierService;
        private Dossier? _dossier;
        private bool _isEditMode;

        private bool _basicInfoName;
        private bool _basicInfoComplaint;
        private bool _basicInfoType;

        private bool _phoneSummary;

        private bool _questionSummary;

        private bool _organs;

        private bool _complaintsSymptomsSummary;

        private bool _researchSummary;

        private bool _policyUrgency;
        private bool _policyTriagecriterium;
        private bool _policyChoice;
        private bool _policyDateTime;

        private bool _advice;
        private bool _adviceForContact;

        private bool _treatmentSummary;
        #endregion

        #region constructers
        public FinishProgressViewModel(IAppNavigation appNavigation, UserMessage userMessage, DossierService dossierService, Dossier? dossier = null)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;
            _dossierService = dossierService;
            if(dossier != null)
            {
                _dossier = dossier;
                IsEditMode = true;
            }
            else
            {
                IsEditMode = false;
            }

            BasicInformation = _dossierService.CentralDossier.BasicInformation;
            _basicInfoName = !string.IsNullOrEmpty(BasicInformation.Name);
            _basicInfoComplaint = !string.IsNullOrEmpty(BasicInformation.Complaint);
            _basicInfoType = !string.IsNullOrEmpty(BasicInformation.Gender);

            Phone = _dossierService.CentralDossier.Phone;
            _phoneSummary = !string.IsNullOrEmpty(Phone.PhoneSummary);

            Question = _dossierService.CentralDossier.Question;
            _questionSummary = !string.IsNullOrEmpty(Question.QuestionSummary);

            Organ = _dossierService.CentralDossier.Organ;
            _organs = !string.IsNullOrEmpty(Organ.Organs);

            ComplaintsSymptoms = _dossierService.CentralDossier.ComplaintsSymptoms;
            _complaintsSymptomsSummary = !string.IsNullOrEmpty(ComplaintsSymptoms.ComplaintsSymptomsSummary);

            Research = _dossierService.CentralDossier.Research;
            _researchSummary = !string.IsNullOrEmpty(Research.ResearchSummary);

            Policy = _dossierService.CentralDossier.Policy;
            _policyUrgency = !string.IsNullOrEmpty(Policy.Urgency);
            _policyTriagecriterium = !string.IsNullOrEmpty(Policy.TriageCriteria);
            _policyChoice = !string.IsNullOrEmpty(Policy.PolicyChoice);
            _policyDateTime = Policy.PolicyDateTime.HasValue;

            ContactAdvice = _dossierService.CentralDossier.ContactAdvice;
            _advice = !string.IsNullOrEmpty(ContactAdvice.Advice);
            _adviceForContact = !string.IsNullOrEmpty(ContactAdvice.ContactAdviceText);

            Treatment = _dossierService.CentralDossier.Treatment;
            _treatmentSummary = !string.IsNullOrEmpty(Treatment.TreatmentSummary);

            ShowInfoCommand = new RelayCommand(ExecuteShowInfo);
            CreateDossierCommand = new RelayCommand(ExecuteCreateDossier);
            EditDossierCommand = new RelayCommand(ExecuteEditDossier, CanExecuteEditDossier);
            ShowBasicInfoCommand = new RelayCommand(ExecuteShowBasicInfo);
            ShowPhoneSummaryCommand = new RelayCommand(ExecuteShowPhoneSummary);
            ShowQuestionCommand = new RelayCommand(ExecuteShowQuestion);
            ShowOrgansCommand = new RelayCommand(ExecuteShowOrgans);
            ShowComplaintsSymptomsCommand = new RelayCommand(ExecuteShowComplaintsSymptoms);
            ShowResearchCommand = new RelayCommand(ExecuteShowResearch);
            ShowPolicyCommand = new RelayCommand(ExecuteShowPolicy);
            ShowContactCommand = new RelayCommand(ExecuteShowContact);
            ShowTreatmentCommand = new RelayCommand(ExecuteShowTreatment);
        }

        public FinishProgressViewModel()
        {

        }
        #endregion

        #region properties
        public DossierService.Dossier Dossier
        {
            get;
        }
        public DossierService.BasicInformation BasicInformation
        {
            get;
        }
        public bool BasicInfoName
        {
            get
            {
                return _basicInfoName;
            }
            set
            {
                if (_basicInfoName != value)
                {
                    _basicInfoName = value; OnPropertyChanged();
                }
            }
        }
        public bool BasicInfoComplaint
        {
            get
            {
                return _basicInfoComplaint;
            }
            set
            {
                if (_basicInfoComplaint != value)
                {
                    _basicInfoComplaint = value; OnPropertyChanged();
                }
            }
        }
        public bool BasicInfoType
        {
            get
            {
                return _basicInfoType;
            }
            set
            {
                if (_basicInfoType != value)
                {
                    _basicInfoType = value; OnPropertyChanged();
                }
            }
        }

        public DossierService.Phone Phone
        {
            get;
        }
        public bool PhoneSummary
        {
            get
            {
                return _phoneSummary;
            }
            set
            {
                if (_phoneSummary != value)
                {
                    _phoneSummary = value; OnPropertyChanged();
                }
            }
        }

        public DossierService.Question Question
        {
            get;
        }
        public bool QuestionSummary
        {
            get
            {
                return _questionSummary;
            }
            set
            {
                if (_questionSummary != value)
                {
                    _questionSummary = value; OnPropertyChanged();
                }
            }
        }

        public DossierService.Organ Organ
        {
            get;
        }
        public bool Organs
        {
            get
            {
                return _organs;
            }
            set
            {
                if (_organs != value)
                {
                    _organs = value; OnPropertyChanged();
                }
            }
        }

        public DossierService.ComplaintsSymptoms ComplaintsSymptoms
        {
            get;
        }
        public bool ComplaintsSymptomsSummary
        {
            get
            {
                return _complaintsSymptomsSummary;
            }
            set
            {
                if (_complaintsSymptomsSummary != value)
                {
                    _complaintsSymptomsSummary = value; OnPropertyChanged();
                }
            }
        }

        public DossierService.Research Research
        {
            get;
        }
        public bool ResearchSummary
        {
            get
            {
                return _researchSummary;
            }
            set
            {
                if (_researchSummary != value)
                {
                    _researchSummary = value; OnPropertyChanged();
                }
            }
        }

        public DossierService.Policy Policy
        {
            get;
        }
        public bool PolicyUrgency
        {
            get
            {
                return _policyUrgency;
            }
            set
            {
                if (_policyUrgency != value)
                {
                    _policyUrgency = value; OnPropertyChanged();
                }
            }
        }
        public bool PolicyTriagecriterium
        {
            get
            {
                return _policyTriagecriterium;
            }
            set
            {
                if (_policyTriagecriterium != value)
                {
                    _policyTriagecriterium = value; OnPropertyChanged();
                }
            }
        }
        public bool PolicyChoice
        {
            get
            {
                return _policyChoice;
            }
            set
            {
                if (_policyChoice != value)
                {
                    _policyChoice = value; OnPropertyChanged();
                }
            }
        }

        public bool PolicyDateTime
        {
            get
            {
                return _policyDateTime;
            }
            set
            {
                if (_policyDateTime != value)
                {
                    _policyDateTime = value; OnPropertyChanged();
                }
            }
        }
        public DossierService.ContactAdvice ContactAdvice
        {
            get;
        }
        public bool Advice
        {
            get
            {
                return _advice;
            }
            set
            {
                if (_advice != value)
                {
                    _advice = value; OnPropertyChanged();
                }
            }
        }
        public bool AdviceForContact
        {
            get
            {
                return _adviceForContact;
            }
            set
            {
                if (_adviceForContact != value)
                {
                    _adviceForContact = value; OnPropertyChanged();
                }
            }
        }

        public DossierService.Treatment Treatment
        {
            get;
        }
        public bool TreatmentSummary
        {
            get
            {
                return _treatmentSummary;
            }
            set
            {
                if (_treatmentSummary != value)
                {
                    _treatmentSummary = value; OnPropertyChanged();
                }
            }
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                if (_isEditMode != value)
                {
                    _isEditMode = value; OnPropertyChanged(nameof(IsEditMode)); OnPropertyChanged(nameof(IsNotEditMode));
                }
            }
        }
        public bool IsNotEditMode => !IsEditMode;
        #endregion

        #region commands
        public ICommand ShowInfoCommand
        {
            get;
        }
        public ICommand CreateDossierCommand
        {
            get;
        }
        public ICommand EditDossierCommand
        {
            get;
        }
        public ICommand ShowBasicInfoCommand
        {
            get;
        }
        public ICommand ShowPhoneSummaryCommand
        {
            get;
        }
        public ICommand ShowQuestionCommand
        {
            get;
        }
        public ICommand ShowOrgansCommand
        {
            get;
        }
        public ICommand ShowComplaintsSymptomsCommand
        {
            get;
        }
        public ICommand ShowResearchCommand
        {
            get;
        }
        public ICommand ShowPolicyCommand
        {
            get;
        }
        public ICommand ShowContactCommand
        {
            get;
        }
        public ICommand ShowTreatmentCommand
        {
            get;
        }
        #endregion

        #region methods
        private void ExecuteShowInfo(object? obj)
        {
            MessageBox.Show("Ben je zeker dat je alles hebt gecontroleerd en klaar bent met invullen? Zorg ervoor dat je alle benodigde informatie hebt ingevuld. Je kunt hieronder de voortgang van het invullen bekijken om te zien of je alles hebt afgerond.",
                            "Aanvullende Informatie en Handige Tips", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void ExecuteCreateDossier(object? obj)
        {
            MessageBoxResult result = MessageBox.Show("Weet je zeker dat je dit dossier wilt aanmaken? Het kan gewijzigd, maar niet verwijderd worden.", "Waarschuwing", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                using (var context = new ApplicationDbContext())
                {
                    try
                    {
                        var newDossier = new Dossier
                        {
                            StudentId = 1,
                            Title = BasicInformation.Complaint + " - " + BasicInformation.Name,
                            CreatedAt = DateOnly.FromDateTime(DateTime.Today),
                            UpdatedAt = DateOnly.FromDateTime(DateTime.Today)
                        };

                        context.Dossier.Add(newDossier);
                        context.SaveChanges();

                        var newBasicInformation = new BasicInformation
                        {
                            DossierId = newDossier.Id,
                            Name = BasicInformation.Name,
                            Complaint = BasicInformation.Complaint,
                            Gender = BasicInformation.Gender,
                            Dossier = newDossier
                        };

                        newDossier.BasicInformation = newBasicInformation;

                        var newPhone = new Phone
                        {
                            DossierId = newDossier.Id,
                            PhoneSummary = Phone.PhoneSummary
                        };
                        var newQuestion = new Question
                        {
                            DossierId = newDossier.Id,
                            QuestionSummary = Question.QuestionSummary
                        };
                        var newOrgan = new Organ
                        {
                            DossierId = newDossier.Id,
                            Organs = Organ.Organs
                        };
                        var newComplaintsSymptoms = new ComplaintsSymptoms
                        {
                            DossierId = newDossier.Id,
                            ComplaintsSymptomsSummary = ComplaintsSymptoms.ComplaintsSymptomsSummary
                        };
                        var newResearch = new Research
                        {
                            DossierId = newDossier.Id,
                            ResearchSummary = Research.ResearchSummary
                        };
                        var newPolicy = new Policy
                        {
                            DossierId = newDossier.Id,
                            Urgency = Policy.Urgency,
                            TriageCriteria = Policy.TriageCriteria,
                            PolicyChoice = Policy.PolicyChoice,
                            PolicyDateTime = Policy.PolicyDateTime
                        };
                        var newContactAdvice = new ContactAdvice
                        {
                            DossierId = newDossier.Id,
                            Advice = ContactAdvice.Advice,
                            ContactAdviceText = ContactAdvice.ContactAdviceText
                        };
                        var newTreatment = new Treatment
                        {
                            DossierId = newDossier.Id,
                            TreatmentSummary = Treatment.TreatmentSummary
                        };

                        context.BasicInformation.Add(newBasicInformation);
                        context.Phone.Add(newPhone);
                        context.Question.Add(newQuestion);
                        context.Organ.Add(newOrgan);
                        context.ComplaintsSymptoms.Add(newComplaintsSymptoms);
                        context.Research.Add(newResearch);
                        context.Policy.Add(newPolicy);
                        context.ContactAdvice.Add(newContactAdvice);
                        context.Treatment.Add(newTreatment);

                        context.SaveChanges();
                        RedirectToDossierView();
                    }
                    catch (Exception ex)
                    {
                        _userMessage.Text = ("Fout met het toevoegen van het dossier: " + ex.Message);
                    }
                }
            }
        }
        private bool CanExecuteEditDossier(object? obj)
        {
            return _dossier != null;
        }
        private void ExecuteEditDossier(object? obj)
        {
            MessageBoxResult result = MessageBox.Show("Weet je zeker dat je het dossier wilt weizigen?", "Waarschuwing", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                using (var context = new ApplicationDbContext())
                {
                    try
                    {
                        if (_dossier != null)
                        {
                            Dossier? dossierInDb = context.Dossier.FirstOrDefault(x => x.Id == _dossier.Id);
                            dossierInDb.Title = BasicInformation.Complaint + " - " + BasicInformation.Name;
                            dossierInDb.UpdatedAt = DateOnly.FromDateTime(DateTime.Today);

                            BasicInformation? basicInformationInDb = context.BasicInformation.FirstOrDefault(x => x.DossierId == _dossier.Id);
                            context.Entry(basicInformationInDb).CurrentValues.SetValues(BasicInformation);
                            basicInformationInDb.DossierId = dossierInDb.Id;

                            Phone? phoneInDb = context.Phone.FirstOrDefault(x => x.DossierId == _dossier.Id);
                            context.Entry(phoneInDb).CurrentValues.SetValues(Phone);
                            phoneInDb.DossierId = dossierInDb.Id;

                            Question? questionInDb = context.Question.FirstOrDefault(x => x.DossierId == _dossier.Id);
                            context.Entry(questionInDb).CurrentValues.SetValues(Question);
                            questionInDb.DossierId = dossierInDb.Id;

                            Organ? organInDb = context.Organ.FirstOrDefault(x => x.DossierId == _dossier.Id);
                            context.Entry(organInDb).CurrentValues.SetValues(Organ);
                            organInDb.DossierId = dossierInDb.Id;

                            ComplaintsSymptoms? complaintsSymptomsInDb = context.ComplaintsSymptoms.FirstOrDefault(x => x.DossierId == _dossier.Id);
                            context.Entry(complaintsSymptomsInDb).CurrentValues.SetValues(ComplaintsSymptoms);
                            complaintsSymptomsInDb.DossierId = dossierInDb.Id;

                            Research? researchInDb = context.Research.FirstOrDefault(x => x.DossierId == _dossier.Id);
                            context.Entry(researchInDb).CurrentValues.SetValues(Research);
                            researchInDb.DossierId = dossierInDb.Id;

                            Policy? policyInDb = context.Policy.FirstOrDefault(x => x.DossierId == _dossier.Id);
                            context.Entry(policyInDb).CurrentValues.SetValues(Policy);
                            policyInDb.DossierId = dossierInDb.Id;

                            ContactAdvice? contactAdviceInDb = context.ContactAdvice.FirstOrDefault(x => x.DossierId == _dossier.Id);
                            context.Entry(contactAdviceInDb).CurrentValues.SetValues(ContactAdvice);
                            contactAdviceInDb.DossierId = dossierInDb.Id;

                            Treatment? treatmentInDb = context.Treatment.FirstOrDefault(x => x.DossierId == _dossier.Id);
                            context.Entry(treatmentInDb).CurrentValues.SetValues(Treatment);
                            treatmentInDb.DossierId = dossierInDb.Id;

                            context.SaveChanges();
                            RedirectToDossierView();
                        }
                    }
                    catch (Exception ex)
                    {
                        _userMessage.Text = ("Fout met het wijzigen van het dossier: " + ex.Message);
                    }
                }
            }
        }
        private void RedirectToDossierView()
        {
            _appNavigation.ActiveViewModel = new DossiersViewModel(_appNavigation, _userMessage);
        }
        private void ExecuteShowBasicInfo(object? obj)
        {
            _appNavigation.ActiveViewModel = new BasicInformationViewModel(_appNavigation, _userMessage, _dossierService, _dossier);
        }
        private void ExecuteShowPhoneSummary(object? obj)
        {
            _appNavigation.ActiveViewModel = new PhoneSummaryViewModel(_appNavigation, _userMessage, _dossierService, _dossier);
        }
        private void ExecuteShowQuestion(object? obj)
        {
            _appNavigation.ActiveViewModel = new QuestionsViewModel(_appNavigation, _userMessage, _dossierService, _dossier);
        }
        private void ExecuteShowOrgans(object? obj)
        {
            _appNavigation.ActiveViewModel = new OrgansViewModel(_appNavigation, _userMessage, _dossierService, _dossier);
        }
        private void ExecuteShowComplaintsSymptoms(object? obj)
        {
            _appNavigation.ActiveViewModel = new ComplaintsAndSymptomsViewModel(_appNavigation, _userMessage, _dossierService, _dossier);
        }
        private void ExecuteShowResearch(object? obj)
        {
            _appNavigation.ActiveViewModel = new ResearchViewModel(_appNavigation, _userMessage, _dossierService, _dossier);
        }
        private void ExecuteShowPolicy(object? obj)
        {
            _appNavigation.ActiveViewModel = new PolicyViewModel(_appNavigation, _userMessage, _dossierService, _dossier);
        }
        private void ExecuteShowContact(object? obj)
        {
            _appNavigation.ActiveViewModel = new ContactAdvicesViewModel(_appNavigation, _userMessage, _dossierService, _dossier);
        }
        private void ExecuteShowTreatment(object? obj)
        {
            _appNavigation.ActiveViewModel = new TreatmentViewModel(_appNavigation, _userMessage, _dossierService, _dossier);
        }
        #endregion
    }
}