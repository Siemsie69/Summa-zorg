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

            // Verwijder eventueel oude thema dictionary
            var existing = appResources.FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Theme"));
            if (existing != null)
                appResources.Remove(existing);

            // Voeg nieuwe toe
            appResources.Insert(0, themeDict); // Plaats vooraan voor prioriteit
        }
    }

}
