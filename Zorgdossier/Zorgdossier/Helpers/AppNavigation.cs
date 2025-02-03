using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zorgdossier.Helpers;

namespace Zorgdossier.Helpers
{
    public class AppNavigation : IAppNavigation, INotifyPropertyChanged
    {
        private object? _activeViewModel;

        public object? ActiveViewModel
        {
            get => _activeViewModel;
            set
            {
                if (_activeViewModel != value)
                {
                    _activeViewModel = value;
                    OnPropertyChanged(nameof(ActiveViewModel));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}