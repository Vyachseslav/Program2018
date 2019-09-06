using System.Windows.Controls;
using System.Windows.Xps.Packaging;

namespace Kachatel2018.View.Helper
{
    /// <summary>
    /// Логика взаимодействия для RecoveryBackupIntoMETR6View.xaml
    /// </summary>
    public partial class RecoveryBackupIntoMETR6View : UserControl
    {
        public RecoveryBackupIntoMETR6View()
        {
            InitializeComponent();
            Load();
        }

        private void Load()
        {
            XpsDocument doc = new XpsDocument("Documents/RecoveryBackupIntoMETR6.xps", System.IO.FileAccess.Read);
            documentViewer.Document = doc.GetFixedDocumentSequence();
        }
    }
}
