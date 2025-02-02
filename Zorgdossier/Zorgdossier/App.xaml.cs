using System.Configuration;
using System.Data;
using System.Windows;
using Zorgdossier.ViewModels;
using Zorgdossier.Views;

namespace Zorgdossier
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //Deze methode vervangt de MainWindow door de MainView en past de datacontext van de MainView aan zodat het gebruikt maakt van de MainViewModel.
        //Vervolgens wordt de MainView laten zien.
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow = new MainView()
            {
                DataContext = new MainViewModel()
            };
            MainWindow.Show();
        }
    }
}