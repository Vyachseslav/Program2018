using System.Windows.Controls;
using System.Windows.Xps.Packaging;

namespace Kachatel2018.View.Helper
{
    /// <summary>
    /// Логика взаимодействия для FastInsertPassportIdIntoExcelView.xaml
    /// </summary>
    public partial class FastInsertPassportIdIntoExcelView : UserControl
    {
        public FastInsertPassportIdIntoExcelView()
        {
            InitializeComponent();
            Load();
        }

        private void Load()
        {
            XpsDocument doc = new XpsDocument("Documents/FastInsertPassportIdIntoExcel.xps", System.IO.FileAccess.Read);
            documentViewer.Document = doc.GetFixedDocumentSequence();
        }
    }
}
