using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Kachatel2018
{
    /// <summary>
    /// Содержит шаблоны для выполнения SQL-запросов.
    /// </summary>
    class SQLPatterns
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public SQLPatterns() { }

        /// <summary>
        /// Шаблон для запросов к базе, возвращающий DataTable.
        /// </summary>
        /// <param name="request">Запрос к базе.</param>
        /// <returns>Результат выполнения запроса в виде DataTable.</returns>
        private DataTable GetSQLDataPattern(string request, string sqlConString)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlConString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(request, connection);
                    adapter.SelectCommand.CommandTimeout = 1800;
                    adapter.Fill(dt);
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                dt = null;
            }
            return dt;
        }
    }
}
