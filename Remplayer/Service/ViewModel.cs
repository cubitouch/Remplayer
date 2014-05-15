using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Remplayer.Service
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string _source;
        public string Source
        {
            get
            {
                return _source;
            }
            set
            {
                _source = value;
                State = System.Windows.Controls.MediaState.Play;
                RaisePropertyChanged("Source");
            }
        }

        private string _server;
        public string Server
        {
            get
            {
                return _server;
            }
            set
            {
                _server = value;
                RaisePropertyChanged("Server");
            }
        }
        private System.Windows.Controls.MediaState _state;
        public System.Windows.Controls.MediaState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                RaisePropertyChanged("State");
            }
        }

        private double _volume;
        public double Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                if (value < 0)
                {
                    _volume = 0;
                }
                else if (value > 1)
                {
                    _volume = 1;
                }
                else
                {
                    _volume = value;
                }

                RaisePropertyChanged("Volume");
            }
        }
        public ViewModel()
        {
            State = System.Windows.Controls.MediaState.Play;
            Volume = 0.5;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        private void RaisePropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
