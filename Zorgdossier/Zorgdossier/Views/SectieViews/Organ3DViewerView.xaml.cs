using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace Zorgdossier.Views.SectieViews
{
    public partial class Organ3DViewerView : UserControl
    {
        public Organ3DViewerView()
        {
            InitializeComponent();
            SetupLights();
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
        }

        public void AddOrganModel(ModelVisual3D originalModel)
        {
            if (originalModel != null)
            {
                // Create a deep copy of the model to avoid the "already a child" error
                var modelCopy = CloneModelVisual3D(originalModel);
                Viewport.Children.Add(modelCopy);
                Viewport.ZoomExtents();
            }
        }

        private ModelVisual3D CloneModelVisual3D(ModelVisual3D original)
        {
            if (original.Content is Model3DGroup originalGroup)
            {
                var newGroup = new Model3DGroup();
                foreach (var model in originalGroup.Children)
                {
                    if (model is GeometryModel3D geometryModel)
                    {
                        var newGeometryModel = new GeometryModel3D
                        {
                            Geometry = geometryModel.Geometry,
                            Material = geometryModel.Material,
                            BackMaterial = geometryModel.BackMaterial,
                            Transform = geometryModel.Transform
                        };
                        newGroup.Children.Add(newGeometryModel);
                    }
                }

                var newVisual = new ModelVisual3D
                {
                    Content = newGroup,
                    Transform = original.Transform
                };

                return newVisual;
            }
            return new ModelVisual3D();
        }

        public void ClearModels()
        {
            // Keep the lights (first 4 children) and remove all models
            while (Viewport.Children.Count > 4)
            {
                Viewport.Children.RemoveAt(4);
            }
        }
    }
}