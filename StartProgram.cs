using System;
using System.Diagnostics;

namespace Kachatel2018
{
    /// <summary>
    /// Запуск различных модулей.
    /// </summary>
    public class StartProgram
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public StartProgram() { }

        /// <summary>
        /// Запуск bat-файлов.
        /// </summary>
        /// <param name="fileName">Название bat-файла.</param>
        public void StartBATFile(string fileName)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(fileName);
            startInfo.WorkingDirectory = Environment.CurrentDirectory + "\\Settings\\ShellScripts";

            Process.Start(startInfo);
        }

        /// <summary>
        /// Запуск обычных SQL-скриптов.
        /// </summary>
        /// <param name="filename">Путь до файла.</param>
        public void StartSQLScript(string filename)
        {
            Process.Start(filename);
        }

        /// <summary>
        /// Запуск exe-программ.
        /// </summary>
        /// <param name="filename"></param>
        public void StartExecutableFile(string filename)
        {
            Process.Start(filename);
        }
    }
}
