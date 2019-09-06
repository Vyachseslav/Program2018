using Kachatel2018.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Kachatel2018.ViewModel
{
    public class SystemMessages : INotifyPropertyChanged
    {
        private ObservableCollection<SystemMessage> _mesList;

        /// <summary>
        /// Лист с сообщениями системы.
        /// </summary>
        public ObservableCollection<SystemMessage> MessageList
        {
            get { return _mesList; }
            set
            {
                _mesList = value;
                OnPropertyChanged("MessageList");
            }
        }

        public SystemMessages()
        {
            MessageList = new ObservableCollection<SystemMessage>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
