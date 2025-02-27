﻿using System.Windows.Input;
using Zorgdossier.Helpers;
using Zorgdossier.Models;

namespace Zorgdossier.ViewModels
{
    internal class HomeViewModel : ObservableObject
    {
        #region Fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        #endregion

        #region Constructors
        public HomeViewModel(IAppNavigation appNavigation, UserMessage userMessage)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;

            ShowDossiersCommand = new RelayCommand(ExecuteShowDossiers);
            ShowExplanationCommand = new RelayCommand(ExecuteShowExplanation);
        }

        public HomeViewModel() { }
        #endregion

        #region Properties

        #endregion

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

        #region Commands
        public ICommand ShowDossiersCommand { get; }
        public ICommand ShowExplanationCommand { get; }
        #endregion

        #region Methods
        private void ExecuteShowDossiers(object? obj)
        {
            _appNavigation.ActiveViewModel = new DossiersViewModel(_appNavigation, _userMessage);
        }

        private void ExecuteShowExplanation(object? obj)
        {
            _appNavigation.ActiveViewModel = new ExplanationViewModel(_appNavigation, _userMessage);
        }
        #endregion
    }
}
