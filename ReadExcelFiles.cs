using System;
using System.Data;

namespace Kachatel2018
{
    /// <summary>
    /// Класс для чтения данных из Excel-шаблона.
    /// </summary>
    class ReadExcelFiles
    {
        public delegate void SetMessage(string message);
        /// <summary>
        /// Событие записи сообщения на консоль.
        /// </summary>
        public event SetMessage WriteMessageToConsole;

        /// <summary>
        /// Записать сообщение на консоль.
        /// </summary>
        /// <param name="mess"></param>
        private void WriteMessage(string mess)
        {
            if (WriteMessageToConsole != null)
            {
                WriteMessageToConsole.Invoke(mess);
            }
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ReadExcelFiles() { }

        /// <summary>
        /// Считать данные с Excel-файла (только данные, без заголовков).
        /// </summary>
        /// <param name="path">Путь к входному Excel-файлу.</param>
        /// <returns>Возвращает данные из Excel-файла в DataTable.</returns>
        public DataTable GetExcelDataOnly(string path)
        {
            DataTable tb = new DataTable();
            String constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0 XML;HDR=YES;';";
            // Создаем новый DataTable.
            DataTable myTable_New = new DataTable();
            string[,] myData = null;
            string select = "";

            try
            {
                System.Data.OleDb.OleDbConnection con = new System.Data.OleDb.OleDbConnection(constr);
                con.Open();
                select = String.Format("SELECT * FROM [Data$] WHERE [idekz] IS NOT NULL"); // WHERE [nmoi] IS NOT NULL
                System.Data.OleDb.OleDbDataAdapter ad = new System.Data.OleDb.OleDbDataAdapter(select, con);
                ad.Fill(tb);
                con.Close();

                
                // Создать число столбцов
                for (int j = 0; j < tb.Columns.Count; j++)
                {
                    myTable_New.Columns.Add(tb.Columns[j].ColumnName);
                }

                int removingRows = 0; // Число строк, которые будем удалять.
                foreach (DataRow row in tb.Rows)
                {
                    if (row[0].ToString() == "№паспорта" || (row[0].ToString() == "1" && row[1].ToString() == "2"))
                    {
                        removingRows++;
                        continue;
                    }
                    break;
                }

                int rowLen = tb.Rows.Count - removingRows; // Число строк в будущем DataTable.
                int colLen = tb.Columns.Count; // Число столбцов в будущем DataTable.
                myData = new string[rowLen, colLen]; // Массив для данных без учета строк заголовка.
                int rowCounter = 0; // Счетчик строк.

                // Запись данных в массив string.
                foreach (DataRow row in tb.Rows)
                {
                    // Если строка row - это строка заголовка таблицы, то переходим к следующей строке.
                    if (row[0].ToString() == "№паспорта" || (row[0].ToString() == "1" && row[1].ToString() == "2"))
                    {
                        continue;
                    }

                    // Если строка row - это строка с данными, то записываем их в массив данных.
                    for (int i = 0; i < colLen; i++)
                    {
                        myData[rowCounter, i] = row[i].ToString(); // Запись данных по всем столбцам.
                    }
                    rowCounter++; // Счетчик строк увеличиваем на единицу.
                }

                // Записываем все данные в DataTable.
                for (int i = 0; i < rowLen; i++)
                {
                    myTable_New.Rows.Add(); // Добавляем новую строку.
                    for (int j = 0; j < colLen; j++)
                    {
                        myTable_New.Rows[i][j] = myData[i, j]; // Запись данных в добавленную строку.
                    }
                }
                WriteMessage("Данные загужены в память.\nТеперь Вы можете просмотреть содержимое, выбрав в главном меню пункт 'Просмотр'.");
            }
            catch (Exception ex)
            {
                WriteMessage("Ошибка чтения Excel-файла.\n" + ex.Message);
            }
            finally
            {
                // Сборщик мусора. Чтобы не оставались зависшие процессы Excel
                select = null;
                constr = null;
                tb = null;
                myData = null;
                GC.Collect();
            }
            return myTable_New;
        }
    }
}
