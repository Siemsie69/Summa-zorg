using System.IO;
using System.Windows;
using Zorgdossier.Databases;
using Zorgdossier.Helpers;
using Zorgdossier.Models;
using Zorgdossier.Views;
using Zorgdossier.ViewModels;

namespace Zorgdossier
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            string savedLanguage = Zorgdossier.Properties.Settings.Default.languageName ?? "Dutch";
            ApplyLanguage(savedLanguage);

            string savedTheme = Zorgdossier.Properties.Settings.Default.themeName ?? "Light";
            ApplyTheme(savedTheme);

            try
            {
                // Verkrijg het apparaat-ID
                var deviceName = GetDeviceName();

                AppNavigation _appNavigation = new();
                UserMessage _userMessage = new();

                // Controleer of de database bestaat voor dit apparaat
                if (DoesDatabaseExistForDevice(deviceName))
                {
                    MainWindow = new MainView()
                    {
                        DataContext = new MainViewModel(_appNavigation, _userMessage)
                    };
                    MainWindow.Show();
                }
                else
                {
                    MainWindow = new RegistrationView()
                    {
                        DataContext = new RegistrationViewModel(_appNavigation, _userMessage, deviceName)
                    };
                    MainWindow.Show();
                }
            }
            catch (Exception ex)
            {
                // Toon foutmelding als er iets misgaat
                MessageBox.Show($"Fout bij het opstarten van de applicatie: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }

        public static void ApplyLanguage(string languageName)
        {
            string uri = $"Languages/{languageName}Language.xaml";
            var languageDict = new ResourceDictionary() { Source = new Uri(uri, UriKind.Relative) };

            var appResources = Application.Current.Resources.MergedDictionaries;

            var existing = appResources.FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Language"));
            if (existing != null)
                appResources.Remove(existing);

            appResources.Insert(0, languageDict);

            Zorgdossier.Properties.Settings.Default.languageName = languageName;
            Zorgdossier.Properties.Settings.Default.Save();
        }

        public static void ApplyTheme(string themeName)
        {
            string uri = $"Themes/{themeName}Theme.xaml";
            var themeDict = new ResourceDictionary() { Source = new Uri(uri, UriKind.Relative) };

            var appResources = Application.Current.Resources.MergedDictionaries;

            var existing = appResources.FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Theme"));
            if (existing != null)
                appResources.Remove(existing);

            appResources.Insert(0, themeDict);

            Zorgdossier.Properties.Settings.Default.themeName = themeName;
            Zorgdossier.Properties.Settings.Default.Save();
        }

        // Verkrijg het apparaat-ID (bijv. machine naam)
        private string GetDeviceName()
        {
            return Environment.MachineName; // Alternatief: een GUID genereren voor meer unieke identificatie
        }

        // Controleer of er al een database bestaat voor dit apparaat
        private bool DoesDatabaseExistForDevice(string deviceName)
        {
            string databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Databases", "database.db");

            if (File.Exists(databasePath))
            {
                try
                {
                    using (var context = new ApplicationDbContext())
                    {
                        var student = context.Student.FirstOrDefault(s => s.DeviceName == deviceName);
                        return student != null; // Als er een student is gevonden, betekent dit dat de database bestaat voor dit apparaat
                    }
                }
                catch (Exception)
                {
                    // Er is een fout opgetreden bij het openen van de database
                    Console.WriteLine("Fout bij het openen van de database.");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}