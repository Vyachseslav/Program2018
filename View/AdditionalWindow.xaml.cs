using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kachatel2018
{
    /// <summary>
    /// Логика взаимодействия для AdditionalWindow.xaml
    /// </summary>
    public partial class AdditionalWindow : Window
    {
        public AdditionalWindow()
        {
            InitializeComponent();
        }

        private void btnOLEDB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo("AccessDatabaseEngine.exe");
                startInfo.WorkingDirectory = Environment.CurrentDirectory + "\\Settings\\Drivers";

                Process.Start(startInfo);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка запуска драйвера", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
