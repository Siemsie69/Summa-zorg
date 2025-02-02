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
    internal class MainViewModel : ObservableObject
    {
        #region fields
        private UserMessage _userMessage;
        private object _activeViewModel = null!;
        #endregion

        #region constructers
        public MainViewModel()
        {
            _userMessage = new()
            {
                Text = "Standaard MVVM template"
            };
            _activeViewModel = new ContactInfoViewModel();
            ShowContactInfoCommand = new RelayCommand(ExecuteShowContactInfo);
        }
        #endregion

        #region properties
        public UserMessage UserMessage
        {
            get
            {
                return _userMessage;
            }
            set
            {
                _userMessage = value; OnPropertyChanged();
            }
        }
        public object ActiveViewModel
        {
            get
            {
                return _activeViewModel;
            }
            set
            {
                _activeViewModel = value; OnPropertyChanged();
            }
        }
        #endregion

        #region commands
        public ICommand ShowContactInfoCommand
        {
            get;
        }
        #endregion

        #region methods
        //Deze methode zet de ContactInfoViewModel in ActiveViewModel, zodat de ContactInfoView in de ContentControl in de MainView weergegeven wordt.
        private void ExecuteShowContactInfo(object? obj)
        {
            ActiveViewModel = new ContactInfoViewModel();
        }
        #endregion
    }
}