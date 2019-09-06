using System.Windows.Controls;
using System.Windows.Xps.Packaging;

namespace Kachatel2018.View.Helper
{
    /// <summary>
    /// Логика взаимодействия для ConvertationIntoDatabaseView.xaml
    /// </summary>
    public partial class ConvertationIntoDatabaseView : UserControl
    {
        public ConvertationIntoDatabaseView()
        {
            InitializeComponent();
            Load();
        }

        private void Load()
        {
            XpsDocument doc = new XpsDocument("Documents/ConvertationIntoDatabase.xps", System.IO.FileAccess.Read);
            documentViewer.Document = doc.GetFixedDocumentSequence();
        }
    }
}
