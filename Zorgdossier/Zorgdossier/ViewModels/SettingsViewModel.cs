﻿using System.Windows;
using System.Windows.Input;
using Zorgdossier.Helpers;
using Zorgdossier.Models;
using System.IO;
using System.Data.SQLite;
using System.Data.Entity;
using Zorgdossier.Databases;
using Microsoft.EntityFrameworkCore;

namespace Zorgdossier.ViewModels
{
    internal class SettingsViewModel : ObservableObject
    {
        #region Fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        private ApplicationDbContext _dbContext;
        private string _selectedTheme;
        private string _selectedLanguage;

        private static string DatabasePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Databases", "database.db");
        #endregion

        #region Constructors
        public SettingsViewModel(IAppNavigation appNavigation, UserMessage userMessage, ApplicationDbContext dbContext)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;
            _dbContext = dbContext;

            ResetCommand = new RelayCommand(ExecuteReset);
        }

        public SettingsViewModel() { }
        #endregion

        #region Properties
        public IAppNavigation AppNavigation
        {
            get => _appNavigation;
        }

        public UserMessage UserMessage
        {
            get => _userMessage;
            set
            {
                _userMessage = value;
                OnPropertyChanged();
            }
        }
        public string SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                _selectedTheme = value;
                OnPropertyChanged();
            }
        }
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                _selectedLanguage = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand ResetCommand { get; }
        #endregion

        #region Methods
        private async void ExecuteReset(object? obj)
        {
            MessageBoxResult result = MessageBox.Show(
                "Weet je zeker dat je al je gegevens wilt verwijderen? Ze kunnen niet meer worden teruggehaald.",
                "Waarschuwing", MessageBoxButton.YesNo, MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await CloseDatabaseConnectionAsync();

                    // Extra wachttijd om er zeker van te zijn dat het bestand niet meer gelocked is
                    await Task.Delay(500);

                    if (File.Exists(DatabasePath))
                    {
                        File.Delete(DatabasePath);
                        _userMessage.Text = "Database succesvol verwijderd.";
                    }
                    else
                    {
                        _userMessage.Text = "Database bestand niet gevonden.";
                    }

                    // Sluit de applicatie om een volledig schone herstart te forceren
                    Application.Current.Shutdown();
                }
                catch (Exception ex)
                {
                    _userMessage.Text = "Fout met het verwijderen van gegevens: " + ex.Message;
                }
            }
        }
        private async Task CloseDatabaseConnectionAsync()
        {
            try
            {
                if (_dbContext != null)
                {
                    await _dbContext.Database.CloseConnectionAsync();
                    await _dbContext.DisposeAsync();
                }

                // Forceer SQLite om alle verbindingen los te laten
                SQLiteConnection.ClearAllPools();

                // Garbage Collector forceren om openstaande objecten op te ruimen
                GC.Collect();
                GC.WaitForPendingFinalizers();

                // Korte pauze om ervoor te zorgen dat het bestand echt wordt vrijgegeven
                await Task.Delay(500);
            }
            catch (Exception ex)
            {
                _userMessage.Text = "Fout bij sluiten van de databaseconnectie: " + ex.Message;
            }
        }
        #endregion
    }
}