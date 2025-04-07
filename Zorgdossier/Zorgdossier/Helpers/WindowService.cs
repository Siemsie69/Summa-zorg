using System.Windows;

namespace Zorgdossier.Helpers
{
    public class WindowService : IWindowService
    {
        public void ShowWindow(object viewModel)
        {
            // Maak een nieuwe Window
            var window = new Window();

            // Zoek de bijbehorende View
            var view = CreateViewForViewModel(viewModel);

            if (view == null)
            {
                throw new InvalidOperationException($"No view found for ViewModel of type {viewModel.GetType().FullName}");
            }

            // Stel de Content en DataContext van de Window in
            window.Content = view;
            window.DataContext = viewModel;
            window.MinHeight = 580;
            window.MaxHeight = 580;
            window.MinWidth = 840;
            window.MaxWidth = 840;

            // Toon de Window
            window.Show();
        }

        private FrameworkElement? CreateViewForViewModel(object viewModel)
        {
            // Logica om de View te vinden: gebruik conventies (bijvoorbeeld naamgeving)
            var viewModelType = viewModel.GetType();
            var viewTypeName = viewModelType.FullName.Replace("ViewModel", "View");
            var viewType = Type.GetType(viewTypeName);

            return viewType != null ? (FrameworkElement?)Activator.CreateInstance(viewType) : null;
        }
    }
}
