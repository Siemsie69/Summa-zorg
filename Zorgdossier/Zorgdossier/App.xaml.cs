using System.Configuration;
using System.Data;
using System.Windows;
using Zorgdossier.Helpers;
using Zorgdossier.Models;
using Zorgdossier.ViewModels;
using Zorgdossier.Views;

namespace Zorgdossier
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AppNavigation _appNavigation = new();
            UserMessage _userMessage = new();

            base.OnStartup(e);
            MainWindow = new MainView()
            {
                DataContext = new MainViewModel(_appNavigation, _userMessage)
            };
            MainWindow.Show();
        }
    }
}