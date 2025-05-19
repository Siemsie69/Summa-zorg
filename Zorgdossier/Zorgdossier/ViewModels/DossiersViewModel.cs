using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Zorgdossier.Databases;
using Zorgdossier.Helpers;
using Zorgdossier.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.Win32;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Colors;

namespace Zorgdossier.ViewModels
{
    class DossiersViewModel : ObservableObject
    {
        #region fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        private DossierService _dossierService;

        private readonly IWindowService _windowService = new WindowService();
        #endregion

        #region constructers
        public DossiersViewModel(IAppNavigation appNavigation, UserMessage userMessage, DossierService dossierService)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;
            _dossierService = dossierService;

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

            var mainWindow = Application.Current.MainWindow;
            var left = mainWindow.Left;
            var top = mainWindow.Top;
            var width = mainWindow.Width;
            var height = mainWindow.Height;

            double sampleDossierWidth = 900;
            double sampleDossierHeight = 540; 

            var sampleDossierWindow = new Window
            {
                Title = "Voorbeeld/Sample Dossier",
                Content = sharedViewModel, 
                Width = sampleDossierWidth,
                Height = sampleDossierHeight,
                Left = left + width, 
                Top = top, 
                WindowStartupLocation = WindowStartupLocation.Manual
            };

            sampleDossierWindow.Show();
        }

        private bool CanExportDossier(object? obj)
        {
            return obj is Dossier;
        }

        private void ExportDossier(object? obj)
        {
            if (obj is Dossier dossier)
            {
                String RecordsExportMessageText = (string)Application.Current.Resources["RecordsExportMessageText"];
                String ShowMainViewTitle = (string)Application.Current.Resources["ShowMainViewTitle"];

                MessageBoxResult result = MessageBox.Show(RecordsExportMessageText, ShowMainViewTitle, MessageBoxButton.YesNo, MessageBoxImage.Information);

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

                                PdfFont normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                                PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                                DeviceRgb indigoColor = new DeviceRgb(36, 13, 104);

                                string RecordsPDFTitleText = (string)Application.Current.Resources["RecordsPDFTitleText"];
                                string RecordsPDFUndefinedText = (string)Application.Current.Resources["RecordsPDFUndefinedText"];

                                string titleContent = (basicInformation?.Complaint != null && basicInformation?.Name != null)
                                    ? $"{basicInformation.Complaint} - {basicInformation.Name}"
                                    : RecordsPDFUndefinedText;

                                string titleText = $"{RecordsPDFTitleText} {titleContent}";

                                document.Add(new Paragraph(titleText)
                                    .SetFont(boldFont)
                                    .SetFontSize(24)
                                    .SetFontColor(indigoColor)
                                    .SetMarginBottom(20));

                                void AddStyledParagraph(string header, string value, bool isList = false)
                                {
                                    document.Add(new Paragraph(header)
                                        .SetFont(boldFont)
                                        .SetFontSize(12)
                                        .SetFontColor(indigoColor)
                                        .SetMarginBottom(5));

                                    if (isList && !string.IsNullOrEmpty(value))
                                    {
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

                                AddStyledParagraph(Application.Current.Resources["ExportStudentNameText"] as string, student?.Name);
                                AddStyledParagraph(Application.Current.Resources["ExportStudentNumberText"] as string, student?.StudentNumber);
                                AddStyledParagraph(Application.Current.Resources["ExportCreatedAtText"] as string, dossierRow?.CreatedAt.ToString());
                                AddStyledParagraph(Application.Current.Resources["ExportUpdatedAtText"] as string, dossierRow?.UpdatedAt.ToString());

                                document.Add(new Paragraph("\n"));
                                document.Add(new Paragraph("\n"));

                                AddStyledParagraph(Application.Current.Resources["ExportTitleText"] as string, dossierRow?.Title);
                                AddStyledParagraph(Application.Current.Resources["ExportPatientNameText"] as string, basicInformation?.Name);
                                AddStyledParagraph(Application.Current.Resources["ExportComplaintText"] as string, basicInformation?.Complaint);
                                AddStyledParagraph(Application.Current.Resources["ExportGenderText"] as string, basicInformation?.Gender);
                                AddStyledParagraph(Application.Current.Resources["ExportPhoneSummaryText"] as string, phone?.PhoneSummary);
                                AddStyledParagraph(Application.Current.Resources["ExportPatientQuestionText"] as string, question?.QuestionSummary);
                                document.Add(new Paragraph("\n"));
                                AddStyledParagraph(Application.Current.Resources["ExportOrgansText"] as string, organ?.Organs, isList: true);
                                document.Add(new Paragraph("\n"));
                                AddStyledParagraph(Application.Current.Resources["ExportSymptomsText"] as string, complaintsSymptoms?.ComplaintsSymptomsSummary);
                                AddStyledParagraph(Application.Current.Resources["ExportResearchText"] as string, research?.ResearchSummary);
                                AddStyledParagraph(Application.Current.Resources["ExportUrgencyText"] as string, policy?.Urgency);
                                AddStyledParagraph(Application.Current.Resources["ExportTriageCriteriaText"] as string, policy?.TriageCriteria);
                                AddStyledParagraph(Application.Current.Resources["ExportPolicyChoiceText"] as string, policy?.PolicyChoice);
                                AddStyledParagraph(Application.Current.Resources["ExportPolicyDateTimeText"] as string, policy?.PolicyDateTime?.ToString());
                                AddStyledParagraph(Application.Current.Resources["ExportAdviceText"] as string, contactAdvice?.Advice);
                                AddStyledParagraph(Application.Current.Resources["ExportContactAdviceText"] as string, contactAdvice?.ContactAdviceText);
                                AddStyledParagraph(Application.Current.Resources["ExportTreatmentText"] as string, treatment?.TreatmentSummary);

                                document.Add(new Paragraph(Application.Current.Resources["ExportEndOfFileText"] as string)
                                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE))
                                    .SetFontSize(10)
                                    .SetMarginTop(20)
                                    .SetFontColor(indigoColor));

                                document.Close();
                            }

                            string RecordsPDFSavedText = (string)Application.Current.Resources["RecordsPDFSavedText"];

                            MessageBox.Show($"{RecordsPDFSavedText}: {filePath}", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
        }
        #endregion
    }
}