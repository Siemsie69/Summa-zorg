using System.Windows;
using System.Windows.Input;
using Zorgdossier.Helpers;
using Zorgdossier.Models;
using Zorgdossier.Views.SectieViews;

namespace Zorgdossier.ViewModels.SectieViewModels
{
    public class OrgansViewModel : ObservableObject
    {
        #region Fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        #endregion

        #region Constructors
        public OrgansViewModel(IAppNavigation appNavigation, UserMessage userMessage)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;

            ShowComplaintsAndSymptomsCommand = new RelayCommand(ExecuteShowComplaintsAndSymptoms);
            ShowQuestionsCommand = new RelayCommand(ExecuteShowQuestions);
        }

        public OrgansViewModel() { }
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
        public ICommand ShowComplaintsAndSymptomsCommand { get; }
        public ICommand ShowQuestionsCommand { get; }
        #endregion

        #region Methods
        private void ExecuteShowComplaintsAndSymptoms(object? obj)
        {
            _appNavigation.ActiveViewModel = new ComplaintsAndSymptomsViewModel(_appNavigation, _userMessage);
        }

        private void ExecuteShowQuestions(object? obj)
        {
            _appNavigation.ActiveViewModel = new QuestionsViewModel(_appNavigation, _userMessage);
        }
        #endregion
    }
}
