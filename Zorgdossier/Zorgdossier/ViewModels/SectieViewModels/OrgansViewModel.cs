using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using System.IO;
using Path = System.IO.Path;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Zorgdossier.Databases;
using Zorgdossier.Helpers;
using Zorgdossier.Models;

namespace Zorgdossier.ViewModels.SectieViewModels
{
    class OrgansViewModel : ObservableObject
    {
        #region fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        private DossierService _dossierService;
        private Dossier? _dossier;
        public ObservableCollection<Visual3D> ViewportChildren { get; } = new();
        private bool _isSampleMode;
        private string _hintTextOrganChoice = string.Empty;


        private readonly Dictionary<string, ModelVisual3D> organModels = new();
        private readonly Dictionary<string, Point3D> organPositions = new()
        {
            { "Brein", new Point3D(0, 0, 0) },
            { "Ogen", new Point3D(0, 0, 0) },
            { "Longen", new Point3D(0, 0, 0) },
            { "Slokdarm", new Point3D(0, 0, 0) },
            { "Luchtpijp", new Point3D(0, 0, 0) },
            { "Dikke darm", new Point3D(0, 0, 0) },
            { "Dunne darm", new Point3D(0, 0, 0) },
            { "Twaalfvingerige darm", new Point3D(0, 0, 0) },
            { "Schildklier", new Point3D(0, 0, 0) },
            { "Alvleesklier", new Point3D(0, 0, 0) },
            { "Maag", new Point3D(0, 0, 0) },
            { "Lever", new Point3D(0, 0, 0) },
            { "Galblaas", new Point3D(0, 0, 0) },
            { "Nieren", new Point3D(0, 0, 0) },
            { "Urineblaas", new Point3D(0, 0, 0) },
            { "Prostaat", new Point3D(0, 0, 0) },
            { "Hypofyse", new Point3D(0, 0, 0) },
            { "Zenuwstelsel", new Point3D(0, 0, 0) },
            { "Hart", new Point3D(0, 0, 0) },
            { "Aorta", new Point3D(0, 0, 0) }
        };
        #endregion

        #region constructers
        public OrgansViewModel(IAppNavigation appNavigation, UserMessage userMessage, DossierService dossierService, Dossier? dossier = null, SampleDossierViewModel? instance = null)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;
            _dossierService = dossierService;
            _dossier = dossier;

            Organ = _dossierService.CentralDossier.Organ;

            if (instance != null)
            {
                Instance = instance;
                IsSampleMode = Instance.IsSampleMode;
            }

            ShowInfoCommand = new RelayCommand(ExecuteShowInfo);
            ShowHomeCommand = new RelayCommand(ExecuteShowMainView);
            ShowComplaintsAndSymptomsCommand = new RelayCommand(ExecuteShowComplaintsAndSymptomsView);
            ShowQuestionsCommand = new RelayCommand(ExecuteShowQuestionsView);
            ShowFinishProgressCommand = new RelayCommand(ExecuteShowFinishedView);

            HintTextOrganChoice = IsSampleMode ? "Kies hier de organen." : "Kies hier de organen.";

            LoadOrgans();
            LoadSelectedOrgans();

            AddToListCommand = new RelayCommand(AddToList, CanAddToList);
            RemoveFromListCommand = new RelayCommand(RemoveFromList, CanRemoveFromList);
        }
        public OrgansViewModel()
        {

        }

        private void LoadOrgans()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var organName in organPositions.Keys)
                {
                    if (!AvailableOrgans.Contains(organName))
                    {
                        AvailableOrgans.Add(organName);
                    }
                }

                // Voeg extra niet-zichtbare organen toe
                var extraOrgans = new List<string>
                {
                    "Skelet",
                    "Endeldarm",
                    "Oren",
                    "Neus",
                    "Mondholte",
                    "Keelholte",
                    "Lymfeklieren",
                    "Ledematen (arm)",
                    "Ledematen (been)",
                    "Ledematen (vingers)",
                    "Ledematen (tenen)",
                    "Huid",
                    "Bloedomloop",
                    "Zaadballen",
                    "Eierstokken",
                    "Baarmoeder"
                };

                foreach (var organ in extraOrgans)
                {
                    if (!AvailableOrgans.Contains(organ))
                    {
                        AvailableOrgans.Add(organ);
                    }
                }
            });
        }

        private void SaveOrgans()
        {
            // Zet de lijst van organen om naar een string, gescheiden door komma's
            string organsAsString = string.Join(",", SelectedOrgans);

            // Sla de string op in DossierService
            _dossierService.CentralDossier.Organ.Organs = organsAsString;
            if (_dossierService?.CentralDossier?.Organ == null)
            {
                MessageBox.Show("Organ is niet geïnitialiseerd!");
                return;
            }
        }

        private void UpdateOrgans()
        {
            // Zelfde logica als opslaan: werk de string in DossierService bij
            string organsAsString = string.Join(",", SelectedOrgans);
            _dossierService.CentralDossier.Organ.Organs = organsAsString;
        }
        #endregion

        #region properties
        public DossierService.Organ Organ
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

        public string HintTextOrganChoice
        {
            get => _hintTextOrganChoice;
            set
            {
                if (_hintTextOrganChoice != value)
                {
                    _hintTextOrganChoice = value;
                    OnPropertyChanged(nameof(HintTextOrganChoice));
                }
            }
        }

        public SampleDossierViewModel Instance
        {
            get;
        }

        public ObservableCollection<string> AvailableOrgans { get; } = new();

        public ObservableCollection<string> SelectedOrgans { get; } = new();

        private string? _selectedOrgan;
        public string? SelectedOrgan
        {
            get => _selectedOrgan;
            set => SetProperty(ref _selectedOrgan, value); // Notify UI
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

        public ICommand ShowComplaintsAndSymptomsCommand
        {
            get;
        }

        public ICommand ShowQuestionsCommand
        {
            get;
        }

        public ICommand AddToListCommand
        {
            get;
        }

        public ICommand RemoveFromListCommand
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
            MessageBox.Show("Kies de organen die direct gerelateerd zijn aan de klachten en symptomen die de patiënt ervaart. Dit helpt om een gerichter dossier op te stellen, zodat je de juiste zorg en behandeling kunt plannen.",
                            "Aanvullende Informatie en Handige Tips", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExecuteShowMainView(object? obj)
        {
            MessageBoxResult result = MessageBox.Show("Weet je zeker dat je terug wilt gaan naar de Home pagina? Al je voortgang van dit dossier raakt dan verloren.", "Waarschuwing", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                _appNavigation.ActiveViewModel = new HomeViewModel(_appNavigation, _userMessage);
            }
        }

        private void ExecuteShowComplaintsAndSymptomsView(object? obj)
        {
            if (IsSampleMode != true)
            {
                _appNavigation.ActiveViewModel = new ComplaintsAndSymptomsViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
            }
            else
            {
                _appNavigation.ActiveViewModel = new ComplaintsAndSymptomsViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
            }
        }

        private void ExecuteShowQuestionsView(object? obj)
        {
            _appNavigation.ActiveViewModel = new QuestionsViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
        }

        private void AddToList(object? obj = null)
        {
            if (SelectedOrgan != null && !SelectedOrgans.Contains(SelectedOrgan))
            {
                SelectedOrgans.Add(SelectedOrgan); 
                SaveOrgans(); 
            }
        }

        private bool CanAddToList(object? obj = null)
        {
            return SelectedOrgan != null && !SelectedOrgans.Contains(SelectedOrgan);
        }

        private void RemoveFromList(object? obj = null)
        {
            if (SelectedOrgan != null && SelectedOrgans.Contains(SelectedOrgan))
            {
                SelectedOrgans.Remove(SelectedOrgan);
                SaveOrgans(); 
            }
        }

        private bool CanRemoveFromList(object? obj = null)
        {
            return SelectedOrgan != null && SelectedOrgans.Contains(SelectedOrgan);
        }

        private async Task<bool> FileExistsAsync(string path)
        {
            return await Task.Run(() => File.Exists(path));
        }

        private void LoadSelectedOrgans()
        {
            string? organsAsString = _dossierService.CentralDossier.Organ.Organs;

            if (!string.IsNullOrEmpty(organsAsString))
            {
                var organsList = organsAsString.Split(',').ToList();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var organ in organsList)
                    {
                        if (!SelectedOrgans.Contains(organ))
                        {
                            SelectedOrgans.Add(organ);
                        }
                    }
                });
            }
        }

        private void UpdateViewport()
        {
            ViewportChildren.Clear();

            // Add SunLight to the viewport
            //var sunLight = new SunLight();
            //ViewportChildren.Add(sunLight);

            foreach (string organName in SelectedOrgans)
            {
                var organPath = Path.Combine("Resources/stl", $"{organName}.stl");

                if (File.Exists(organPath))
                {
                    try
                    {
                        var reader = new StLReader();
                        var model = reader.Read(organPath);
                        var visual = CreateOrganModel(model, organPositions[organName]);
                        ViewportChildren.Add(visual);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to load model for {organName}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"File not found for organ: {organName}", "File Missing", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private ModelVisual3D CreateOrganModel(Model3DGroup model, Point3D position)
        {
            var material = new DiffuseMaterial(new SolidColorBrush(Colors.Red));

            foreach (var geometry in model.Children)
            {
                if (geometry is GeometryModel3D geometryModel)
                {
                    geometryModel.Material = material;
                    geometryModel.BackMaterial = material;
                }
            }

            var organModel = new ModelVisual3D { Content = model };
            organModel.Transform = new TranslateTransform3D(position.X, position.Y, position.Z);
            return organModel;
        }

        protected bool SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private void ExecuteShowFinishedView(object? obj)
        {
            _appNavigation.ActiveViewModel = new FinishProgressViewModel(_appNavigation, _userMessage, _dossierService, _dossier, Instance);
        }
        #endregion
    }
}