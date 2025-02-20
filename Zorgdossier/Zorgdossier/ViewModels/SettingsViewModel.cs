using Zorgdossier.Helpers;
using Zorgdossier.Models;

namespace Zorgdossier.ViewModels
{
    internal class SettingsViewModel : ObservableObject
    {
        #region Fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        #endregion

        #region Constructors
        public SettingsViewModel(IAppNavigation appNavigation, UserMessage userMessage)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;
        }

        public SettingsViewModel() { }
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
    }
}
