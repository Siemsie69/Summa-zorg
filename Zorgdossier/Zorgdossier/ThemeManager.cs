using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Zorgdossier
{
    public static class ThemeManager
    {
        public static void ApplyTheme(string themeName)
        {
            string uri = $"Themes/{themeName}Theme.xaml";
            var themeDict = new ResourceDictionary() { Source = new Uri(uri, UriKind.Relative) };

            var appResources = Application.Current.Resources.MergedDictionaries;

            var existing = appResources.FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Theme"));
            if (existing != null)
                appResources.Remove(existing);

            appResources.Insert(0, themeDict);
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
        }
    }

}
