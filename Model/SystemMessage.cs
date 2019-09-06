using System;
using System.ComponentModel;

namespace Kachatel2018.Model
{
    public class SystemMessage : INotifyPropertyChanged
    {
        private string title;
        private DateTime _time;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                DateTime = DateTime.Now;
                OnPropertyChanged("Title");
            }
        }

        public DateTime DateTime
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged("DateTime");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
