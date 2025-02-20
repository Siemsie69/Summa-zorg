using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zorgdossier.Models;
using Zorgdossier.Helpers;
using System.ComponentModel;

namespace Zorgdossier.Databases
{
    public class DossierService : ObservableObject
    {
        public class NewDossier
        {
            public Dossier Dossier { get; set; } = new Dossier();
            public BasicInformation BasicInformation { get; set; } = new BasicInformation();
            public Phone Phone { get; set; } = new Phone();
            public Question Question { get; set; } = new Question();
            public Organ Organ { get; set; } = new Organ();
            public ComplaintsSymptoms ComplaintsSymptoms { get; set; } = new ComplaintsSymptoms();
            public Research Research { get; set; } = new Research();
            public Policy Policy { get; set; } = new Policy();
            public ContactAdvice ContactAdvice { get; set; } = new ContactAdvice();
            public Treatment Treatment { get; set; } = new Treatment();
        }

        public NewDossier CentralDossier { get; private set; } = new NewDossier();

        public class Dossier
        {
            public int StudentId { get; set; }
            public string Title { get; set; }
            public DateTime CreatedAt { get; set; }
        }
        public class BasicInformation
        {
            public int DossierId { get; set; }
            public string Name { get; set; }
            public string Complaint { get; set; }
            public string Gender { get; set; }
        }
        public class Phone
        {
            public int DossierId { get; set; }
            public string PhoneSummary { get; set; }
        }
        public class Question
        {
            public int DossierId { get; set; }
            public string QuestionSummary { get; set; }
        }

        public class Organ()
        {
            public int DossierId { get; set; }
            public string Organs { get; set; } = string.Empty;
        }

        public class ComplaintsSymptoms
        {
            public int DossierId { get; set; }
            public string ComplaintsSymptomsSummary { get; set; }
        }
        public class Research
        {
            public int DossierId { get; set; }
            public string ResearchSummary { get; set; }
        }
        public class Policy : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler? PropertyChanged;

            private int _dossierId;
            private string _urgency = string.Empty;
            private string? _triageCriteria = string.Empty;
            private string _policyChoice = string.Empty;
            private DateTime? _policyDateTime;

            public int DossierId
            {
                get => _dossierId;
                set { _dossierId = value; OnPropertyChanged(nameof(DossierId)); }
            }

            public string Urgency
            {
                get => _urgency;
                set { _urgency = value; OnPropertyChanged(nameof(Urgency)); }
            }

            public string? TriageCriteria
            {
                get => _triageCriteria;
                set { _triageCriteria = value; OnPropertyChanged(nameof(TriageCriteria)); }
            }

            public string PolicyChoice
            {
                get => _policyChoice;
                set { _policyChoice = value; OnPropertyChanged(nameof(PolicyChoice)); }
            }

            public DateTime? PolicyDateTime
            {
                get => _policyDateTime;
                set { _policyDateTime = value; OnPropertyChanged(nameof(PolicyDateTime)); }
            }

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public class ContactAdvice
        {
            public int DossierId { get; set; }
            public string Advice { get; set; }
            public string? ContactAdviceText { get; set; }

        }
        public class Treatment
        {
            public int DossierId { get; set; }
            public string TreatmentSummary { get; set; }
        }
    }
}