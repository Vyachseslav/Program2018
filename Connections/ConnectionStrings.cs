using System;

namespace Kachatel2018.Connections
{
    /// <summary>
    /// Класс для строки соединения с базой данных.
    /// </summary>
    public static class ConnectionStrings
    {
        /// <summary>
        /// Строка подключения к базе данных.
        /// </summary>
        public static string SQLConnectionStirng
        {
            get { return PathTermDll.PathTermDllCl.GetSqlUDL(Environment.CurrentDirectory + "\\Settings\\ServerUDL\\"); }
        }
    }
}
