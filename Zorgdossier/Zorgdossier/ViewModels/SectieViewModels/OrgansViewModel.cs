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
using System.Collections.Generic;
using Zorgdossier.Views.SectieViews;

namespace Zorgdossier.ViewModels.SectieViewModels
{
    class OrgansViewModel : ObservableObject
    {
        #region fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        private DossierService _dossierService;
        private Dossier? _dossier;

        private bool _isSampleMode;
        private string _hintTextOrganChoice = string.Empty;
        private string _selectedOrgan;

        private bool _isViewerOpen = false;
        private Window _viewerWindow;

        private readonly Dictionary<string, ModelVisual3D> organModels = new();
        private readonly Dictionary<string, Point3D> organPositions = new()
        {
            { "Aorta", new Point3D(0, 0, 0) },
            { "Alvleesklier", new Point3D(0, 0, 0) },
            { "Baarmoeder", new Point3D(0, 0, 0) },
            { "Bloedomloop", new Point3D(0, 0, 0) },
            { "Brein", new Point3D(0, 0, 0) },
            { "Dikke Darm", new Point3D(0, 0, 0) },
            { "Dunne Darm", new Point3D(0, 0, 0) },
            { "Endeldarm", new Point3D(0, 0, 0) },
            { "Eierstokken", new Point3D(0, 0, 0) },
            { "Galblaas", new Point3D(0, 0, 0) },
            { "Hart", new Point3D(0, 0, 0) },
            { "Huid", new Point3D(0, 0, 0) },
            { "Hypofyse", new Point3D(0, 0, 0) },
            { "Keelholte", new Point3D(0, 0, 0) },
            { "Ledematen (Armen)", new Point3D(0, 0, 0) },
            { "Ledematen (Benen)", new Point3D(0, 0, 0) },
            { "Ledematen (Vingers)", new Point3D(0, 0, 0) },
            { "Ledematen (Tenen)", new Point3D(0, 0, 0) },
            { "Lever", new Point3D(0, 0, 0) },
            { "Longen", new Point3D(0, 0, 0) },
            { "Luchtpijp", new Point3D(0, 0, 0) },
            { "Lymfeklieren", new Point3D(0, 0, 0) },
            { "Maag", new Point3D(0, 0, 0) },
            { "Mondholte", new Point3D(0, 0, 0) },
            { "Nieren", new Point3D(0, 0, 0) },
            { "Neus", new Point3D(0, 0, 0) },
            { "Ogen", new Point3D(0, 0, 0) },
            { "Oren", new Point3D(0, 0, 0) },
            { "Prostaat", new Point3D(0, 0, 0) },
            { "Schildklier", new Point3D(0, 0, 0) },
            { "Skelet", new Point3D(0, 0, 0) },
            { "Slokdarm", new Point3D(0, 0, 0) },
            { "Twaalfvingerige Darm", new Point3D(0, 0, 0) },
            { "Urineblaas", new Point3D(0, 0, 0) },
            { "Zaadballen", new Point3D(0, 0, 0) },
            { "Zenuwstelsel", new Point3D(0, 0, 0) }
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

            LoadOrgans();
            AddToListCommand = new RelayCommand(AddToList, CanAddToList);
            RemoveFromListCommand = new RelayCommand(RemoveFromList, CanRemoveFromList);
            OpenViewerCommand = new RelayCommand(ExecuteOpenViewer, CanOpenViewer);

            if (dossier == null)
            {
                HintTextOrganChoice = IsSampleMode ? "Selecteer hier de organen." : "Selecteer hier de organen.";
            }
        }

        public OrgansViewModel()
        {
            
        }
        #endregion

        #region properties
        public DossierService.Organ Organ { get; }

        public bool IsSampleMode
        {
            get => _isSampleMode;
            set
            {
                if (_isSampleMode != value)
                {
                    _isSampleMode = value;
                    OnPropertyChanged(nameof(IsSampleMode));
                    OnPropertyChanged(nameof(IsNotSampleMode));
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

        public string SelectedOrgan
        {
            get => _selectedOrgan;
            set
            {
                _selectedOrgan = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ObservableCollection<string> AvailableOrgans { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> SelectedOrgans { get; } = new ObservableCollection<string>();
        public ObservableCollection<Visual3D> ViewportChildren { get; } = new ObservableCollection<Visual3D>();

        public SampleDossierViewModel Instance { get; }
        #endregion

        #region commands
        public ICommand ShowInfoCommand { get; }
        public ICommand ShowHomeCommand { get; }
        public ICommand ShowComplaintsAndSymptomsCommand { get; }
        public ICommand ShowQuestionsCommand { get; }
        public ICommand ShowFinishProgressCommand { get; }
        public ICommand AddToListCommand { get; }
        public ICommand RemoveFromListCommand { get; }
        public ICommand OpenViewerCommand { get; }
        #endregion

        #region methods
        private void LoadOrgans()
        {
            var organFolder = @"C:\Summa-zorg\Zorgdossier\Zorgdossier\Organs\stl\";

            foreach (var organName in organPositions.Keys)
            {
                AvailableOrgans.Add(organName);

                var organPath = Path.Combine(organFolder, $"{organName}.stl");
                if (File.Exists(organPath))
                {
                    try
                    {
                        var reader = new StLReader();
                        var model = reader.Read(organPath);
                        var visual = CreateOrganModel(model, organPositions[organName]);
                        organModels[organName] = visual;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to load model for {organName}: {ex.Message}");
                    }
                }
            }
        }

        private ModelVisual3D CreateOrganModel(Model3DGroup model, Point3D position)
        {
            var materialGroup = new MaterialGroup();
            var diffuseMaterial = new DiffuseMaterial(
                new SolidColorBrush(Color.FromRgb(150, 200, 230)));
            var specularMaterial = new SpecularMaterial(
                new SolidColorBrush(Color.FromRgb(220, 220, 220)), 60);

            materialGroup.Children.Add(diffuseMaterial);
            materialGroup.Children.Add(specularMaterial);

            foreach (var geometry in model.Children)
            {
                if (geometry is GeometryModel3D geometryModel)
                {
                    geometryModel.Material = materialGroup;
                    geometryModel.BackMaterial = materialGroup;
                }
            }

            var organModel = new ModelVisual3D { Content = model };
            organModel.Transform = new TranslateTransform3D(position.X, position.Y, position.Z);
            return organModel;
        }

        private void AddToList(object parameter)
        {
            if (!string.IsNullOrEmpty(SelectedOrgan) && !SelectedOrgans.Contains(SelectedOrgan))
            {
                SelectedOrgans.Add(SelectedOrgan);

                if (_isViewerOpen && _viewerWindow?.Content is Organ3DViewerView viewer)
                {
                    if (organModels.TryGetValue(SelectedOrgan, out var visual))
                    {
                        viewer.AddOrganModel(visual);
                    }
                }
                else if (SelectedOrgans.Count == 1)
                {
                    ExecuteOpenViewer(null);
                }
            }
        }

        private bool CanAddToList(object parameter)
        {
            return !string.IsNullOrEmpty(SelectedOrgan) && !SelectedOrgans.Contains(SelectedOrgan);
        }

        private void RemoveFromList(object parameter)
        {
            if (!string.IsNullOrEmpty(SelectedOrgan) && SelectedOrgans.Contains(SelectedOrgan))
            {
                SelectedOrgans.Remove(SelectedOrgan);

                if (_isViewerOpen && _viewerWindow?.Content is Organ3DViewerView viewer)
                {
                    viewer.ClearModels();
                    foreach (var organName in SelectedOrgans)
                    {
                        if (organModels.TryGetValue(organName, out var visual))
                        {
                            viewer.AddOrganModel(visual);
                        }
                    }
                }
            }
        }

        private bool CanRemoveFromList(object parameter)
        {
            return !string.IsNullOrEmpty(SelectedOrgan) && SelectedOrgans.Contains(SelectedOrgan);
        }

        private void ExecuteShowInfo(object? obj)
        {
            MessageBox.Show("Selecteer hier de organen die direct gerelateerd zijn aan de klachten en symptomen die de patiënt ervaart. Dit helpt om een gerichter dossier op te stellen, zodat je de juiste zorg en behandeling kunt plannen.",
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

        private void ExecuteShowFinishedView(object? obj)
        {
            _appNavigation.ActiveViewModel = new FinishProgressViewModel(_appNavigation, _userMessage, _dossierService, _dossier);
        }

        private bool CanOpenViewer(object parameter)
        {
            return SelectedOrgans.Count > 0 && !_isViewerOpen;
        }

        private void ExecuteOpenViewer(object parameter)
        {
            if (_isViewerOpen)
            {
                _viewerWindow?.Activate();
                return;
            }

            _isViewerOpen = true;

            var viewer = new Organ3DViewerView();

            foreach (var organName in SelectedOrgans)
            {
                if (organModels.TryGetValue(organName, out var visual))
                {
                    viewer.AddOrganModel(visual);
                }
            }

            _viewerWindow = new Window
            {
                Title = "3D Organ Viewer",
                Content = viewer,
                Width = 800,
                Height = 600,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            _viewerWindow.Closed += (sender, args) =>
            {
                _isViewerOpen = false;
                _viewerWindow = null;
            };

            _viewerWindow.Show();
        }
        #endregion
    }
}