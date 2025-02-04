using System;
using System.Diagnostics;
using System.Windows.Input;
using Zorgdossier.Helpers;
using Zorgdossier.Models;

namespace Zorgdossier.ViewModels
{
    internal class CreditsViewModel : ObservableObject
    {
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;

        public CreditsViewModel(IAppNavigation appNavigation, UserMessage userMessage)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;
            OpenWebsiteCommand = new RelayCommand(OpenWebsite);
        }

        public CreditsViewModel()
        {
            OpenWebsiteCommand = new RelayCommand(OpenWebsite);
        }

        public ICommand OpenWebsiteCommand { get; }

        private void OpenWebsite(object? parameter)
        {
            if (parameter is string url)
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error opening URL: {ex.Message}");
                }
            }
        }
    }
}
