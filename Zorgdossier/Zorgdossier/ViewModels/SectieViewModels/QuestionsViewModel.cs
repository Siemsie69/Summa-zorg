using System.Windows;
using System.Windows.Input;
using Zorgdossier.Helpers;
using Zorgdossier.Models;
using Zorgdossier.Views.SectieViews;

namespace Zorgdossier.ViewModels.SectieViewModels
{
    public class QuestionsViewModel : ObservableObject
    {
        #region Fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        #endregion

        #region Constructors
        public QuestionsViewModel(IAppNavigation appNavigation, UserMessage userMessage)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;

            ShowOrgansCommand = new RelayCommand(ExecuteShowOrgans);
            ShowPhoneSummaryCommand = new RelayCommand(ExecuteShowPhoneSummary);
        }

        public QuestionsViewModel() { }
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
        public ICommand ShowOrgansCommand { get; }
        public ICommand ShowPhoneSummaryCommand { get; }
        #endregion

        #region Methods
        private void ExecuteShowOrgans(object? obj)
        {
            _appNavigation.ActiveViewModel = new OrgansViewModel(_appNavigation, _userMessage);
        }
        
        private void ExecuteShowPhoneSummary(object? obj)
        {
            _appNavigation.ActiveViewModel = new PhoneSummaryViewModel(_appNavigation, _userMessage);
        }
        #endregion
    }
}
