using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Zorgdossier.Helpers;

namespace Zorgdossier.Models
{
    public class UserMessage : ObservableObject
    {
        #region fields
        private string _text = string.Empty;
        private DispatcherTimer _errorTimer;
        #endregion

        #region constructers
        public UserMessage()
        {
            _errorTimer = new DispatcherTimer();
            _errorTimer.Interval = TimeSpan.FromSeconds(8);
            _errorTimer.Tick += DispatcherTimer_Tick;
        }
        #endregion

        #region properties
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value; OnPropertyChanged();
                _errorTimer.Start();
            }
        }
        #endregion

        #region methods
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            Text = string.Empty; OnPropertyChanged();
            _errorTimer.Stop();
        }
        #endregion
    }
}