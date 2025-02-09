using System.Windows;
using System.Windows.Input;
using Zorgdossier.Helpers;
using Zorgdossier.Models;
using Zorgdossier.Views.SectieViews;

namespace Zorgdossier.ViewModels.SectieViewModels
{
    public class FinishProgressViewModel : ObservableObject
    {
        #region Fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        #endregion

        #region Constructors
        public FinishProgressViewModel(IAppNavigation appNavigation, UserMessage userMessage)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;

            ShowTreatmentCommand = new RelayCommand(ExecuteShowTreatment);
            ShowInfoCommand = new RelayCommand(ExecuteShowInfo);
        }

        public FinishProgressViewModel() { }
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
        public ICommand ShowInfoCommand { get; }
        #endregion

        #region Methods
        private void ExecuteShowTreatment(object? obj)
        {
            _appNavigation.ActiveViewModel = new TreatmentViewModel(_appNavigation, _userMessage);
        }

        private void ExecuteShowInfo(object? obj)
        {
            MessageBox.Show("Ben je zeker dat je alles hebt gecontroleerd en klaar bent met invullen? Zorg ervoor dat je alle benodigde informatie hebt ingevuld. Je kunt hieronder de voortgang van het invullen bekijken om te zien of je alles hebt afgerond.",
                            "Aanvullende Informatie en Handige Tips", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion
    }
}
