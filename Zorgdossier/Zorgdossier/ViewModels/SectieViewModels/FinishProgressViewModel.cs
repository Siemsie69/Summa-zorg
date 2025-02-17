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
        #endregion

        #region constructers
        public FinishProgressViewModel(IAppNavigation appNavigation, UserMessage userMessage, DossierService dossierService, Dossier? dossier = null)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;
            _dossierService = dossierService;
            _dossier = dossier;

            ShowInfoCommand = new RelayCommand(ExecuteShowInfo);
            ShowTreatmentCommand = new RelayCommand(ExecuteShowTreatmentView);
            CreateDossierCommand = new RelayCommand(ExecuteCreateDossier);

            Dossier = _dossierService.CentralDossier.Dossier;
            BasicInformation = _dossierService.CentralDossier.BasicInformation;
            Phone = _dossierService.CentralDossier.Phone;
            Question = _dossierService.CentralDossier.Question;
            Organ = _dossierService.CentralDossier.Organ;
            ComplaintsSymptoms = _dossierService.CentralDossier.ComplaintsSymptoms;
            Research = _dossierService.CentralDossier.Research;
            Policy = _dossierService.CentralDossier.Policy;
            ContactAdvice = _dossierService.CentralDossier.ContactAdvice;
            Treatment = _dossierService.CentralDossier.Treatment;
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
        #endregion

        #region commands
        public ICommand ShowInfoCommand
        {
            get;
        }
        public ICommand ShowTreatmentCommand
        {
            get;
        }
        public ICommand CreateDossierCommand
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
        private void ExecuteShowTreatmentView(object? obj)
        {
            _appNavigation.ActiveViewModel = new TreatmentViewModel(_appNavigation, _userMessage, _dossierService, _dossier);
        }
        private void ExecuteCreateDossier(object? obj)
        {
            MessageBoxResult result = MessageBox.Show("Weet je zeker dat je dit dossier wilt aanmaken? Het kan geweizigd, maar niet verwijderd worden.", "Waarschuwing", MessageBoxButton.YesNo, MessageBoxImage.Information);

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
        private void RedirectToDossierView()
        {
            _appNavigation.ActiveViewModel = new DossiersViewModel(_appNavigation, _userMessage);
        }
        #endregion
    }
}