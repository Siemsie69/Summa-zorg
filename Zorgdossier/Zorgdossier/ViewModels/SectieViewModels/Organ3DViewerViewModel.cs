using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;
using System.Collections.Generic;
using Zorgdossier.Helpers;

namespace Zorgdossier.ViewModels.SectieViewModels
{
    public class Organ3DViewerViewModel : ObservableObject
    {
        private readonly HelixViewport3D _viewport;
        private readonly Dictionary<string, ModelVisual3D> _organModels;

        public Organ3DViewerViewModel(HelixViewport3D viewport, Dictionary<string, ModelVisual3D> organModels)
        {
            _viewport = viewport;
            _organModels = organModels;
        }

        public void FocusOnOrgan(ModelVisual3D organModel)
        {
            if (organModel == null) return;

            // Get the bounds of the organ model
            var bounds = Visual3DHelper.FindBounds(organModel, Transform3D.Identity);

            // Calculate the distance based on the size of the organ
            var diagonal = new Vector3D(bounds.SizeX, bounds.SizeY, bounds.SizeZ).Length;
            var distance = diagonal * 1.5; // Adjust multiplier as needed

            // Calculate the target position (center of the organ)
            var target = bounds.Location + new Vector3D(bounds.SizeX / 2, bounds.SizeY / 2, bounds.SizeZ / 2);

            // Calculate camera position (offset from target)
            var cameraPosition = new Point3D(
                target.X,
                target.Y,
                target.Z + distance); // Convert Vector3D to Point3D

            // Smoothly animate the camera
            var camera = _viewport.Camera as PerspectiveCamera;
            if (camera != null)
            {
                camera.AnimateTo(
                    cameraPosition,
                    new Point3D(target.X, target.Y, target.Z), // Convert target to Point3D
                    new Vector3D(0, 1, 0),
                    1000);
            }
        }

        public void AddOrganModel(ModelVisual3D organModel)
        {
            if (organModel != null)
            {
                var modelCopy = CloneModelVisual3D(organModel);
                _viewport.Children.Add(modelCopy);
                FocusOnOrgan(modelCopy);
            }
        }

        public void ClearModels()
        {
            // Keep the first 4 children (lights)
            while (_viewport.Children.Count > 4)
            {
                _viewport.Children.RemoveAt(4);
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

                return new ModelVisual3D
                {
                    Content = newGroup,
                    Transform = original.Transform
                };
            }
            return new ModelVisual3D();
        }
    }
}