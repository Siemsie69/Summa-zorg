using System.Windows;
using System.Windows.Input;
using Zorgdossier.Helpers;
using Zorgdossier.Models;
using Zorgdossier.Views.SectieViews;

namespace Zorgdossier.ViewModels.SectieViewModels
{
    public class ComplaintsAndSymptomsViewModel : ObservableObject
    {
        #region Fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        #endregion

        #region Constructors
        public ComplaintsAndSymptomsViewModel(IAppNavigation appNavigation, UserMessage userMessage)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;

            ShowResearchCommand = new RelayCommand(ExecuteShowResearch);
            ShowOrgansCommand = new RelayCommand(ExecuteShowOrgans);
        }

        public ComplaintsAndSymptomsViewModel() { }
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
        public ICommand ShowResearchCommand { get; }
        public ICommand ShowOrgansCommand { get; }
        #endregion

        #region Methods
        private void ExecuteShowResearch(object? obj)
        {
            _appNavigation.ActiveViewModel = new ResearchViewModel(_appNavigation, _userMessage);
        }

        private void ExecuteShowOrgans(object? obj)
        {
            _appNavigation.ActiveViewModel = new OrgansViewModel(_appNavigation, _userMessage);
        }
        #endregion
    }
}
