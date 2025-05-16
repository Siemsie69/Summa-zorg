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
                var modelCopy = CloneModelVisual3D(originalModel);
                Viewport.Children.Add(modelCopy);

                // Calculate model bounds and center
                var bounds = GetBounds(modelCopy);
                var center = new Point3D(
                    bounds.X + bounds.SizeX / 2,
                    bounds.Y + bounds.SizeY / 2,
                    bounds.Z + bounds.SizeZ / 2);

                // Calculate optimal distance based on model size
                var maxSize = Math.Max(bounds.SizeX, Math.Max(bounds.SizeY, bounds.SizeZ));
                var distance = maxSize * 2.0;

                // Set camera to perfect frontal perspective
                var camera = Viewport.Camera as PerspectiveCamera;
                if (camera != null)
                {
                    // Position camera in front of the model at 2/3 height (eye level)
                    camera.Position = new Point3D(
                        center.X,                // Centered horizontally
                        bounds.Y + bounds.SizeY * 0.66, // Eye level (2/3 of height)
                        center.Z + distance     // In front of model (positive Z)
                    );

                    // Look directly at the center of the model
                    camera.LookDirection = new Vector3D(0, 0, -1); // Always look along negative Z-axis
                    camera.UpDirection = new Vector3D(0, 1, 0);   // Keep world up

                    // Adjust field of view for better perspective
                    camera.FieldOfView = 50;
                }

                Viewport.ZoomExtents();
            }
        }

        private Rect3D GetBounds(ModelVisual3D model)
        {
            var bounds = new Rect3D();
            if (model.Content is Model3DGroup group)
            {
                foreach (var child in group.Children)
                {
                    if (child is GeometryModel3D geometryModel)
                    {
                        bounds.Union(geometryModel.Bounds);
                    }
                }
            }
            return bounds;
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
            while (Viewport.Children.Count > 4)
            {
                Viewport.Children.RemoveAt(4);
            }
        }
    }
}