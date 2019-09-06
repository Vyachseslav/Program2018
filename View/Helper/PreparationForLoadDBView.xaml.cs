using System.Windows.Controls;
using System.Windows.Xps.Packaging;

namespace Kachatel2018.View.Helper
{
    /// <summary>
    /// Логика взаимодействия для PreparationForLoadDBView.xaml
    /// </summary>
    public partial class PreparationForLoadDBView : UserControl
    {
        public PreparationForLoadDBView()
        {
            InitializeComponent();
            Load();
        }

        private void Load()
        {
            XpsDocument doc = new XpsDocument("Documents/PreparationForLoadDB.xps", System.IO.FileAccess.Read);
            documentViewer.Document = doc.GetFixedDocumentSequence();
        }
    }
}
