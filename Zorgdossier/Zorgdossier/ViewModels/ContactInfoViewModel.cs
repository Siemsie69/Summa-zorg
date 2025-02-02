using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zorgdossier.Helpers;

namespace Zorgdossier.ViewModels
{
    internal class ContactInfoViewModel : ObservableObject
    {
        #region fields
        private string _naam = string.Empty;
        private string _telefoon = string.Empty;
        private string _email = string.Empty;
        #endregion

        #region constructers
        public ContactInfoViewModel()
        {
            _naam = "Siem van Bree";
            _telefoon = "06 12345678";
            _email = "ps247548@summacollege.nl";
        }
        #endregion

        #region properties
        public string Naam
        {
            get
            {
                return _naam;
            }
            set
            {
                _naam = value; OnPropertyChanged();
            }
        }
        public string Telefoon
        {
            get
            {
                return _telefoon;
            }
            set
            {
                _telefoon = value; OnPropertyChanged();
            }
        }
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value; OnPropertyChanged();
            }
        }
        #endregion

        #region commands

        #endregion

        #region methods

        #endregion
    }
}