using System;
using System.ComponentModel;

namespace Kachatel2018.Model
{
    /*public class User
    {
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Роль пользователя.
        /// </summary>
        public int Role { get; set; }
    }*/

    
    /// <summary>
    /// Модель "Пользователь".
    /// </summary>
    public class User : IDataErrorInfo
    {
        private string _userName = "";
        private int _role = 512;

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        /// <summary>
        /// Роль пользователя.
        /// </summary>
        public int Role
        {
            get { return _role; }
            set { _role = value; }
        }

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "Role":
                        if ((Role < 0) || (Role > 512))
                        {
                            error = "Значение роли не может быть больше 512!";
                        }
                        break;
                    case "UserName":
                        if ((UserName.Length <= 0) || (UserName.Length > 64))
                        {
                            error = "Имя пользователя больше допустимого или пустое!";
                        }
                        break;
                }
                return error;
            }
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }
    }
}
