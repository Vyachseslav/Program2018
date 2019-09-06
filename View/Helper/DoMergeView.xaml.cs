using System.Windows.Controls;
using System.Windows.Xps.Packaging;

namespace Kachatel2018.View.Helper
{
    /// <summary>
    /// Логика взаимодействия для DoMergeView.xaml
    /// </summary>
    public partial class DoMergeView : UserControl
    {
        public DoMergeView()
        {
            InitializeComponent();
            Load();
        }

        private void Load()
        {
            XpsDocument doc = new XpsDocument("Documents/DoMerge.xps", System.IO.FileAccess.Read);
            documentViewer.Document = doc.GetFixedDocumentSequence();
        }
    }
}
