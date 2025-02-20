using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Zorgdossier.Databases;
using Zorgdossier.Helpers;
using Zorgdossier.Models;
using Zorgdossier.Views;
using System.Diagnostics;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.IO;
using Microsoft.Win32;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.IO.Image;
using iText.Kernel.Colors;

namespace Zorgdossier.ViewModels
{
    class DossiersViewModel : ObservableObject
    {
        #region fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        private DossierService _dossierService = new DossierService();

        private readonly IWindowService _windowService = new WindowService();
        #endregion

        #region constructers
        public DossiersViewModel(IAppNavigation appNavigation, UserMessage userMessage)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;

            ShowIntroductionCommand = new RelayCommand(ExecuteShowIntroduction);
            ShowSampleDossierCommand = new RelayCommand(ExecuteShowSampleDossier);
            ExportDossierCommand = new RelayCommand(ExportDossier, CanExportDossier);

            using (var context = new ApplicationDbContext())
            {
                if (context.Dossier.ToList().Any())
                {
                    Dossiers = new ObservableCollection<Dossier>(context.Dossier.ToList());
                }
            }
        }

        public DossiersViewModel()
        {

        }
        #endregion

        #region properties
        public ObservableCollection<Dossier> Dossiers
        {
            get;
        }
        #endregion

        #region commands
        public ICommand ShowIntroductionCommand
        {
            get;
        }
        public ICommand ShowSampleDossierCommand
        {
            get;
        }
        public ICommand ExportDossierCommand
        {
            get;
        }
        #endregion

        #region methods
        private void ExecuteShowIntroduction(object? obj)
        {
            if(obj is Dossier dossier) 
            {
                _appNavigation.ActiveViewModel = new SectieViewModels.IntroductionViewModel(_appNavigation, _userMessage, _dossierService, dossier);
            }
            else
            {
                _appNavigation.ActiveViewModel = new SectieViewModels.IntroductionViewModel(_appNavigation, _userMessage, _dossierService);
            }
        }

        private void ExecuteShowSampleDossier(object? obj)
        {
            var sharedViewModel = SampleDossierViewModel.Instance;
            _windowService.ShowWindow(sharedViewModel);
        }

        private bool CanExportDossier(object? obj)
        {
            return obj is Dossier;
        }

        private void ExportDossier(object? obj)
        {
            if (obj is Dossier dossier)
            {
                MessageBoxResult result = MessageBox.Show("Weet je zeker dat je dit dossier wilt exporteren? Het Pdf-bestand wordt dan gedownload op je lokale computer.", "Waarschuwing", MessageBoxButton.YesNo, MessageBoxImage.Information);

                if (result == MessageBoxResult.Yes)
                {
                    using (var context = new ApplicationDbContext())
                    {
                        var student = context.Student.FirstOrDefault(d => d.Id == 1);
                        var dossierRow = context.Dossier.FirstOrDefault(d => d.Id == dossier.Id);
                        var basicInformation = context.BasicInformation.FirstOrDefault(d => d.DossierId == dossier.Id);
                        var phone = context.Phone.FirstOrDefault(d => d.DossierId == dossier.Id);
                        var question = context.Question.FirstOrDefault(d => d.DossierId == dossier.Id);
                        var organ = context.Organ.FirstOrDefault(d => d.DossierId == dossier.Id);
                        var complaintsSymptoms = context.ComplaintsSymptoms.FirstOrDefault(d => d.DossierId == dossier.Id);
                        var research = context.Research.FirstOrDefault(d => d.DossierId == dossier.Id);
                        var policy = context.Policy.FirstOrDefault(d => d.DossierId == dossier.Id);
                        var contactAdvice = context.ContactAdvice.FirstOrDefault(d => d.DossierId == dossier.Id);
                        var treatment = context.Treatment.FirstOrDefault(d => d.DossierId == dossier.Id);

                        SaveFileDialog saveFileDialog = new SaveFileDialog
                        {
                            Filter = "PDF Files (*.pdf)|*.pdf",
                            Title = "Opslaan als Pdf"
                        };

                        if (saveFileDialog.ShowDialog() == true)
                        {
                            string filePath = saveFileDialog.FileName;

                            using (var writer = new PdfWriter(filePath))
                            {
                                PdfDocument pdf = new PdfDocument(writer);
                                Document document = new Document(pdf);

                                // Lettertypes en kleuren
                                PdfFont normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                                PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                                DeviceRgb indigoColor = new DeviceRgb(36, 13, 104); // Updated indigo color

                                // Titel links
                                string titleText = $"Dossier: {basicInformation?.Complaint + " - " + basicInformation?.Name ?? "Onbekend"}";
                                document.Add(new Paragraph(titleText)
                                    .SetFont(boldFont)
                                    .SetFontSize(24)
                                    .SetFontColor(indigoColor)
                                    .SetMarginBottom(20));

                                // Helper voor tekst
                                void AddStyledParagraph(string header, string value, bool isList = false)
                                {
                                    document.Add(new Paragraph(header)
                                        .SetFont(boldFont)
                                        .SetFontSize(12)
                                        .SetFontColor(indigoColor)
                                        .SetMarginBottom(5));

                                    if (isList && !string.IsNullOrEmpty(value))
                                    {
                                        // Splits op de komma en zet elke waarde onder elkaar met een streepje
                                        var items = value.Split(',');
                                        foreach (var item in items)
                                        {
                                            document.Add(new Paragraph($"- {item.Trim()}")
                                                .SetFont(normalFont)
                                                .SetFontSize(10)
                                                .SetMarginBottom(5));
                                        }
                                    }
                                    else
                                    {
                                        document.Add(new Paragraph(string.IsNullOrEmpty(value) ? "N.v.t." : value)
                                            .SetFont(normalFont)
                                            .SetFontSize(10)
                                            .SetMarginBottom(15));
                                    }
                                }


                                // Gegevens toevoegen

                                AddStyledParagraph("Naam student:", student?.Name);
                                AddStyledParagraph("Studentnummer:", student?.StudentNumber);
                                AddStyledParagraph("Aangemaakt op:", dossierRow?.CreatedAt.ToString());
                                AddStyledParagraph("Gewijzigd op:", dossierRow?.UpdatedAt.ToString());

                                document.Add(new Paragraph("\n"));
                                document.Add(new Paragraph("\n"));

                                AddStyledParagraph("Titel:", dossierRow?.Title);
                                AddStyledParagraph("Naam Patiënt:", basicInformation?.Name);
                                AddStyledParagraph("Klacht:", basicInformation?.Complaint);
                                AddStyledParagraph("Type:", basicInformation?.Gender);
                                AddStyledParagraph("Telefoonsamenvatting:", phone?.PhoneSummary);
                                AddStyledParagraph("Vragen voor de patiënt:", question?.QuestionSummary);
                                document.Add(new Paragraph("\n"));
                                AddStyledParagraph("Orgaan/Organen:", organ?.Organs, isList: true);
                                document.Add(new Paragraph("\n"));
                                AddStyledParagraph("Klachten en Symptomen:", complaintsSymptoms?.ComplaintsSymptomsSummary);
                                AddStyledParagraph("Onderzoeken:", research?.ResearchSummary);
                                AddStyledParagraph("Urgentie:", policy?.Urgency);
                                AddStyledParagraph("Triagecriterium (indien ingevuld):", policy?.TriageCriteria);
                                AddStyledParagraph("Beleidskeuze:", policy?.PolicyChoice);
                                AddStyledParagraph("Datum en tijd van de afspraak (indien gekozen):", policy?.PolicyDateTime?.ToString());
                                AddStyledParagraph("Advies voor de patiënt:", contactAdvice?.Advice);
                                AddStyledParagraph("Contactadvies voor de patiënt (indien ingevuld):", contactAdvice?.ContactAdviceText);
                                AddStyledParagraph("Behandeling:", treatment?.TreatmentSummary);

                                // Afsluiting
                                document.Add(new Paragraph("Einde van het dossier.")
                                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE))
                                    .SetFontSize(10)
                                    .SetMarginTop(20)
                                    .SetFontColor(indigoColor));

                                document.Close();
                            }

                            MessageBox.Show($"PDF opgeslagen als: {filePath}", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
        }
        #endregion
    }
}