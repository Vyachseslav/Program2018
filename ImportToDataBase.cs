using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace Kachatel2018
{
    public class ImportToDataBase
    {
        public delegate void SetProgressValueIntoBar(double step);
        public event SetProgressValueIntoBar SetProgress;

        public delegate void SetMessage(string message);
        public event SetMessage WriteMessageToConsole;

        private void WriteMessage(string mess)
        {
            if (WriteMessageToConsole != null)
            {
                WriteMessageToConsole.Invoke(mess);
            }
        }

        /// <summary>
        /// Таблица с данными из Excel.
        /// </summary>
        DataTable dataTable { get; set; }

        DataTable ColumnKOKS { get; set; }

        /// <summary>
        /// Число столбцов в таблице.
        /// </summary>
        private int ColumnsCount { get; set; }

        /// <summary>
        /// Число строк в таблице.
        /// </summary>
        private int RowsCount { get; set; }

        private double ProgressStep { get; set; }



        public ImportToDataBase() { }

        public ImportToDataBase(DataTable table)
        {
            dataTable = new DataTable(); // Инициализация dataTable.
            dataTable = table.Copy(); // Копирование DataTable.

            ColumnKOKS = GetKOKSColumns(); // получить столбцы из KOKS.

            ColumnsCount = table.Columns.Count; // Расчет числа столбцов в таблице с данными.
            RowsCount = table.Rows.Count; // Расчет числа строк в таблице с данными.            

            Indexing();

            ProgressStep = (double)(100.0 / RowsCount); // Шаг.
        }

        /// <summary>
        /// Загрузить данные в кокс по-моднявому!
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="spName"></param>
        public void RunSp(DataTable dt1)
        {
            SqlConnection connection = new SqlConnection(Connections.ConnectionStrings.SQLConnectionStirng);
            connection.Open();
            SqlCommand cmd = new SqlCommand("dbo.KoksInsert", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter dtparam = cmd.Parameters.AddWithValue("@KoksTempType", dt1);
            dtparam.SqlDbType = SqlDbType.Structured;
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        /// <summary>
        /// Загрузить данные в табилцу Кокс.
        /// </summary>
        public void LoadIntoKoks()
        {
            double res = 0.0;
            int koksCol = ColumnKOKS.Rows.Count; // Число столбцов в таблице KOKS.

            string startReqLine = GetColumnsName(); // Получаем начальную строку для вставки данных.
            string valuesReqLine = String.Empty; // Строка для вставки с данными из Excel.
            string totalRequest = ""; // Общая строка запроса.

            

            using (SqlConnection connection = new SqlConnection(Connections.ConnectionStrings.SQLConnectionStirng))
            {
                connection.Open();
                //SqlTransaction transaction = connection.BeginTransaction();
                SqlCommand command = connection.CreateCommand();
                //command.Transaction = transaction;

                for (int i = 0; i < RowsCount; i++) // Должно быть RowsCount.
                {
                    valuesReqLine = "";


                    for (int j = 1; j < koksCol; j++)
                    {
                        if (KoksInExcel.ContainsKey(ColumnKOKS.Rows[j][0].ToString()))
                        {
                            int index = KoksInExcel[ColumnKOKS.Rows[j][0].ToString()];
                            valuesReqLine += GetValueFromData(dataTable.Rows[i][index].ToString()) + ",";
                        }
                        else
                        {
                            valuesReqLine += "NULL,";
                        }
                    }

                    valuesReqLine = valuesReqLine.TrimEnd(',');
                    valuesReqLine += ")";

                    /*foreach (KeyValuePair<string,int> col in KoksInExcel)
                    {
                        string val = dataTable.Rows[i][col.Value].ToString();
                        valuesReqLine += GetValueFromData(val) + ",";
                    }

                    valuesReqLine = valuesReqLine.TrimEnd(',') + ")";

                    for (int j = 0; j < koksCol; j++)
                    {

                        
                        string val = dataTable.Rows[i][ColumnKOKS.Rows[j][0].ToString()].ToString();
                        if (j < koksCol - 1)
                        {
                            valuesReqLine += GetValueFromData(val) + ",";
                        }
                        else
                        {
                            valuesReqLine += GetValueFromData(val) + ")";
                        }
                    }*/

                    totalRequest = startReqLine + valuesReqLine;

                    //if (i == 6858)
                    //{
                    //    MessageBox.Show(totalRequest);
                    //}

                    try
                    {
                        command.CommandText = totalRequest;
                        command.ExecuteNonQuery();
                                              
                        //transaction.Commit(); // Подтверждаем транзакцию.
                    }
                    catch (Exception ex)
                    {
                        //transaction.Rollback();
                        MessageBox.Show("Ошибка при добавлении экземпляра:\n" + ex.Message, "Ошибка. Row " + i.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                        WriteMessage("Ошибка при добавлении экземпляра (строка №" + i.ToString() + "):\n" + ex.Message + "\n" + totalRequest);
                        break;
                    }
                    finally
                    {
                        res += ProgressStep;
                        if (SetProgress != null) { SetProgress.Invoke(res); }
                    }                    
                }
            }

            //MessageBox.Show("Загружено");
            if (SetProgress != null) { SetProgress.Invoke(0); }
            WriteMessage("Данные из Excel-шаблона загружены в таблицу КОКС.");
        }

        /// <summary>
        /// Сформировать sql файл с запросами.
        /// </summary>
        public void LoadIntoFile(string path)
        {
            double res = 0.0;
            int koksCol = ColumnKOKS.Rows.Count; // Число столбцов в таблице KOKS.

            string startReqLine = GetColumnsName(); // Получаем начальную строку для вставки данных.
            string valuesReqLine = String.Empty; // Строка для вставки с данными из Excel.
            string totalRequest = ""; // Общая строка запроса.

            //List<string> sqlRequests = new List<string>();

            using (StreamWriter streamWriter = new StreamWriter(path, true, Encoding.UTF8))
            {
                for (int i = 0; i < RowsCount; i++) // Должно быть RowsCount.
                {
                    valuesReqLine = "";

                    for (int j = 1; j < koksCol; j++)
                    {
                        if (KoksInExcel.ContainsKey(ColumnKOKS.Rows[j][0].ToString()))
                        {
                            int index = KoksInExcel[ColumnKOKS.Rows[j][0].ToString()];
                            valuesReqLine += GetValueFromData(dataTable.Rows[i][index].ToString()) + ",";
                        }
                        else
                        {
                            valuesReqLine += "NULL,";
                        }
                    }

                    valuesReqLine = valuesReqLine.TrimEnd(',');
                    valuesReqLine += ")";
                    totalRequest = startReqLine + valuesReqLine;

                    try
                    {
                        //sqlRequests.Add(totalRequest);
                        streamWriter.WriteLine(totalRequest);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при добавлении экземпляра:\n" + ex.Message, "Ошибка. Row " + i.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                        WriteMessage("Ошибка при добавлении экземпляра (строка №" + i.ToString() + "):\n" + ex.Message + "\n" + totalRequest);
                        break;
                    }
                    finally
                    {
                        res += ProgressStep;
                        if (SetProgress != null) { SetProgress.Invoke(res); }
                    }
                }
            }

            if (SetProgress != null) { SetProgress.Invoke(0); }
            WriteMessage("Sql скрипт сформирован.");

            //using (SqlConnection connection = new SqlConnection(Connections.ConnectionStrings.SQLConnectionStirng))
            //{
            //    connection.Open();
            //    SqlCommand command = connection.CreateCommand();

            //    for (int i = 0; i < RowsCount; i++) // Должно быть RowsCount.
            //    {
            //        valuesReqLine = "";

            //        for (int j = 1; j < koksCol; j++)
            //        {
            //            if (KoksInExcel.ContainsKey(ColumnKOKS.Rows[j][0].ToString()))
            //            {
            //                int index = KoksInExcel[ColumnKOKS.Rows[j][0].ToString()];
            //                valuesReqLine += GetValueFromData(dataTable.Rows[i][index].ToString()) + ",";
            //            }
            //            else
            //            {
            //                valuesReqLine += "NULL,";
            //            }
            //        }

            //        valuesReqLine = valuesReqLine.TrimEnd(',');
            //        valuesReqLine += ")";

            //        totalRequest = startReqLine + valuesReqLine;

            //        try
            //        {
            //            //command.CommandText = totalRequest;
            //            sqlRequests.Add(totalRequest);
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show("Ошибка при добавлении экземпляра:\n" + ex.Message, "Ошибка. Row " + i.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            //            WriteMessage("Ошибка при добавлении экземпляра (строка №" + i.ToString() + "):\n" + ex.Message + "\n" + totalRequest);
            //            break;
            //        }
            //        finally
            //        {
            //            res += ProgressStep;
            //            if (SetProgress != null) { SetProgress.Invoke(res); }
            //        }
            //    }
            //}

            //if (SetProgress != null) { SetProgress.Invoke(0); }
            //WriteMessage("Данные из Excel-шаблона загружены в таблицу КОКС.");
        }


        private Dictionary<string, int> KoksInExcel { get; set; }

        private void Indexing()
        {
            KoksInExcel = new Dictionary<string, int>();

            for (int i = 0; i < ColumnKOKS.Rows.Count; i++)
            {
                foreach(DataColumn column in dataTable.Columns)
                {
                    if (column.ColumnName.Equals(ColumnKOKS.Rows[i][0].ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        KoksInExcel.Add(ColumnKOKS.Rows[i][0].ToString(), column.Ordinal);
                        break;
                    }
                }
            }
        }

        
        /// <summary>
        /// Возвращать данные для запроса.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetValueFromData(string value)
        {
            if (value != "" && value != String.Empty)
            {
                return String.Format("'{0}'", value);
            }
            else
            {
                return "NULL";
            }
        }

        /// <summary>
        /// Получение строки для запроса, содержащей названия столбцов.
        /// </summary>
        /// <returns></returns>
        private string GetColumnsName()
        {
            int columnCount = ColumnKOKS.Rows.Count;

            string sqlQuery = "set dateformat dmy INSERT INTO [koks](";
            for (int i = 1; i < columnCount; i++)
            {
                if (i < (columnCount - 1))
                {
                    sqlQuery += String.Format("[{0}],", ColumnKOKS.Rows[i][0].ToString());
                }
                else
                {
                    sqlQuery += String.Format("[{0}]) VALUES (", ColumnKOKS.Rows[i][0].ToString());
                }
            }
            return sqlQuery;
        }


        /// <summary>
        /// Получить столбцы из таблицы KOKS.
        /// </summary>
        /// <returns></returns>
        private DataTable GetKOKSColumns()
        {
            string req = String.Format("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'koks'");
            return GetSQLDataPattern(req);
        }

        /// <summary>
        /// Шаблон для запросов к базе, возвращающий DataTable.
        /// </summary>
        /// <param name="request">Запрос к базе.</param>
        /// <returns>Результат выполнения запроса в виде DataTable.</returns>
        private DataTable GetSQLDataPattern(string request)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(Connections.ConnectionStrings.SQLConnectionStirng))
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