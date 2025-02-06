using System.Windows;
using System.Windows.Input;
using Zorgdossier.Helpers;
using Zorgdossier.Models;
using Zorgdossier.Views.SectieViews;

namespace Zorgdossier.ViewModels.SectieViewModels
{
    public class BasicInformationViewModel : ObservableObject
    {
        #region Fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        #endregion

        #region Constructors
        public BasicInformationViewModel(IAppNavigation appNavigation, UserMessage userMessage)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;

            ShowPhoneSummaryCommand = new RelayCommand(ExecuteShowPhoneSummary);
            ShowIntroductionViewCommand = new RelayCommand(ExecuteShowIntroduction);
        }

        public BasicInformationViewModel() { }
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
        public ICommand ShowPhoneSummaryCommand { get; }
        public ICommand ShowIntroductionViewCommand { get; }
        #endregion

        #region Methods
        private void ExecuteShowPhoneSummary(object? obj)
        {
            _appNavigation.ActiveViewModel = new PhoneSummaryViewModel(_appNavigation, _userMessage);
        }
        
        private void ExecuteShowIntroduction(object? obj)
        {
            _appNavigation.ActiveViewModel = new IntroductionViewModel(_appNavigation, _userMessage);
        }
        #endregion
    }
}
