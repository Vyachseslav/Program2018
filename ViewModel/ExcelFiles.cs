using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kachatel2018.ViewModel
{
    /// <summary>
    /// ViewModel модели "Файл".
    /// </summary>
    public class ExcelFiles
    {
        public Model.ExcelFile SelectedFile { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ExcelFiles() { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="pathToFile">Путь к файлу.</param>
        public ExcelFiles(string pathToFile)
        {
            SelectedFile = new Model.ExcelFile();
            FileInfo ff = new FileInfo(pathToFile);

            SelectedFile.FullPath = pathToFile;
            SelectedFile.Name = "Файл: " + ff.Name;
            SelectedFile.Extension = "Расширение: " + ff.Extension;
            SelectedFile.Size = "Размер (кб): " + GetFileSize(ff.Length);
        }

        private string GetFileSize(long fileLength)
        {
            return (fileLength / 1024).ToString();
        }
    }
}
