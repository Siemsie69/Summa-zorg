using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Zorgdossier.Helpers;
using Zorgdossier.Models;

namespace Zorgdossier.ViewModels
{
    internal class ExplanationViewModel : ObservableObject
    {
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;

        public ExplanationViewModel(IAppNavigation appNavigation, UserMessage userMessage)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;

            ShowDossiersCommand = new RelayCommand(ExecuteShowDossiers);
        }

        public ExplanationViewModel() { }

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

        public ICommand ShowDossiersCommand { get; }

        private void ExecuteShowDossiers(object? obj)
        {
            _appNavigation.ActiveViewModel = new DossiersViewModel(_appNavigation, _userMessage);
        }
    }
}
