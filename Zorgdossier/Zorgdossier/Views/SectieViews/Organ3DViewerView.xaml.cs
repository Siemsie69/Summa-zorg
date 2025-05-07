using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using Zorgdossier.ViewModels.SectieViewModels;
using System.Collections.Generic;

namespace Zorgdossier.Views.SectieViews
{
    public partial class Organ3DViewerView : UserControl
    {
        private Organ3DViewerViewModel _viewModel;

        public Organ3DViewerView()
        {
            InitializeComponent();
            SetupLights();
        }

        public void Initialize(Dictionary<string, ModelVisual3D> organModels)
        {
            _viewModel = new Organ3DViewerViewModel(Viewport, organModels);
        }

        private void SetupLights()
        {
            Viewport.Children.Clear();

            var ambientLight = new AmbientLight(Color.FromRgb(100, 100, 100));
            Viewport.Children.Add(new ModelVisual3D { Content = ambientLight });

            var keyLight = new DirectionalLight(Colors.White, new Vector3D(-0.7, -0.7, -1));
            Viewport.Children.Add(new ModelVisual3D { Content = keyLight });

            var fillLight = new DirectionalLight(Color.FromRgb(60, 60, 60), new Vector3D(0.3, 0.3, -1));
            Viewport.Children.Add(new ModelVisual3D { Content = fillLight });

            var rimLight = new DirectionalLight(Color.FromRgb(80, 80, 80), new Vector3D(0, 0.5, -1));
            Viewport.Children.Add(new ModelVisual3D { Content = rimLight });

            // Set up initial camera
            var camera = new PerspectiveCamera
            {
                Position = new Point3D(0, 0, 5),
                LookDirection = new Vector3D(0, 0, -5),
                UpDirection = new Vector3D(0, 1, 0),
                FieldOfView = 60
            };
            Viewport.Camera = camera;
        }

        public void AddOrganModel(ModelVisual3D organModel)
        {
            _viewModel?.AddOrganModel(organModel);
        }

        public void ClearModels()
        {
            _viewModel?.ClearModels();
        }
    }
}