using System.Windows;
using System.Windows.Input;
using Zorgdossier.Helpers;
using Zorgdossier.Models;
using Zorgdossier.Views.SectieViews;

namespace Zorgdossier.ViewModels.SectieViewModels
{
    public class ContactAdvicesViewModel : ObservableObject
    {
        #region Fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        #endregion

        #region Constructors
        public ContactAdvicesViewModel(IAppNavigation appNavigation, UserMessage userMessage)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;

            ShowTreatmentCommand = new RelayCommand(ExecuteShowTreatment);
            ShowPolicyCommand = new RelayCommand(ExecuteShowPolicy);
        }

        public ContactAdvicesViewModel() { }
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
        public ICommand ShowTreatmentCommand { get; }
        public ICommand ShowPolicyCommand { get; }
        #endregion

        #region Methods
        private void ExecuteShowTreatment(object? obj)
        {
            _appNavigation.ActiveViewModel = new TreatmentViewModel(_appNavigation, _userMessage);
        }

        private void ExecuteShowPolicy(object? obj)
        {
            _appNavigation.ActiveViewModel = new PolicyViewModel(_appNavigation, _userMessage);
        }
        #endregion
    }
}
