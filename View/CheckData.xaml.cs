using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для CheckData.xaml
    /// </summary>
    public partial class CheckData : Window
    {
        public CheckData()
        {
            InitializeComponent();
        }

        public CheckData(DataTable datas)
        {
            InitializeComponent();

            dataGrid.ItemsSource = datas.DefaultView;
        }
    }
}
