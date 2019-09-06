using System.ComponentModel;

namespace Kachatel2018.Model
{
    /// <summary>
    /// Модель "Файл".
    /// Отображается информация о выбранном файле.
    /// </summary>
    public class ExcelFile : INotifyPropertyChanged
    {
        private string _name;
        private string _fullPath;
        private string _extension;
        private string _icon = "pack://application:,,,/Kachatel2018;component/Image/excel.ico";
        private string _size;
        private string _rowsActive;
        private string _colsActive;

        /// <summary>
        /// Название файла.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Полный путь к файлу.
        /// </summary>
        public string FullPath
        {
            get { return _fullPath; }
            set
            {
                _fullPath = value;
                OnPropertyChanged("FullPath");
            }
        }

        /// <summary>
        /// Расширение файла.
        /// </summary>
        public string Extension
        {
            get { return _extension; }
            set
            {
                _extension = value;
                OnPropertyChanged("Extension");
            }
        }
        /*
        private string GetIconForFile(string extension)
        {
            if (extension == "xlsm")
            {
                
            }
            
            //return "pack://application:,,,/PhoneBook;component/Image/anonymous.ico";
        }*/

        /// <summary>
        /// Иконка файла.
        /// </summary>
        public string Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                OnPropertyChanged("Icon");
            }
        }

        /// <summary>
        /// Число строк с данными.
        /// </summary>
        public string RowsActive
        {
            get { return _rowsActive; }
            set
            {
                _rowsActive = value;
                OnPropertyChanged("RowsActive");
            }
        }

        /// <summary>
        /// Число столбцов с данными.
        /// </summary>
        public string СolsActive
        {
            get { return _colsActive; }
            set
            {
                _colsActive = value;
                OnPropertyChanged("СolsActive");
            }
        }

        /// <summary>
        /// Размер файла.
        /// </summary>
        public string Size
        {
            get { return _size; }
            set
            {
                _size = value;
                OnPropertyChanged("Size");
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
