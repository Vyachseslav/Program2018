using System.Windows;
using System.Windows.Input;

namespace Kachatel2018
{
    /// <summary>
    /// Логика взаимодействия для About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
            lblVersion.Content = GetVersion();

            txtNews.Text = string.Format("{0}\n{1}\n{2}", 
                "1. Теперь можно менять цветовую схему ПО", 
                "2. Добавлен раздел FAQ",
                "3. Можно формировать Sql скрипт на INSERT");
        }

        private string GetVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
