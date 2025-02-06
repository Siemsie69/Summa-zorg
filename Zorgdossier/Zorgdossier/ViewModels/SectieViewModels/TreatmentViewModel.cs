using System.Windows;
using System.Windows.Input;
using Zorgdossier.Helpers;
using Zorgdossier.Models;
using Zorgdossier.Views.SectieViews;

namespace Zorgdossier.ViewModels.SectieViewModels
{
    public class TreatmentViewModel : ObservableObject
    {
        #region Fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        #endregion

        #region Constructors
        public TreatmentViewModel(IAppNavigation appNavigation, UserMessage userMessage)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;

            ShowFinishProgressCommand = new RelayCommand(ExecuteShowFinishProgress);
            ShowContactAdvicesCommand = new RelayCommand(ExecuteShowContactAdvicesCommand);
        }

        public TreatmentViewModel() { }
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
        public ICommand ShowFinishProgressCommand { get; }
        public ICommand ShowContactAdvicesCommand { get; }
        #endregion

        #region Methods
        private void ExecuteShowFinishProgress(object? obj)
        {
            _appNavigation.ActiveViewModel = new FinishProgressViewModel(_appNavigation, _userMessage);
        }
        
        private void ExecuteShowContactAdvicesCommand(object? obj)
        {
            _appNavigation.ActiveViewModel = new ContactAdvicesViewModel(_appNavigation, _userMessage);
        }
        #endregion
    }
}
