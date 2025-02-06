using System;
using System.Diagnostics;
using System.Windows.Input;
using Zorgdossier.Helpers;
using Zorgdossier.Models;

namespace Zorgdossier.ViewModels
{
    internal class CreditsViewModel : ObservableObject
    {
        #region Fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        #endregion

        #region Constructors
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
        #endregion

        #region Properties

        #endregion

        #region Commands
        public ICommand OpenWebsiteCommand { get; }
        #endregion

        #region Methods
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
        #endregion
    }
}
