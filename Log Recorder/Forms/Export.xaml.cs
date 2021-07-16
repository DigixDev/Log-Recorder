using System;
using System.Collections.Generic;
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

namespace Log_Recorder.Forms
{
    /// <summary>
    /// Interaction logic for Export.xaml
    /// </summary>
    public partial class Export : Window
    {
        public Export()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbHoleNo.ItemsSource = HoleNoList;
        }

        public List<string> HoleNoList { get; set; }

        private void btnExportAll_Click(object sender, RoutedEventArgs e)
        {
            ExportAll = true;
            this.DialogResult = true;
            this.Close();
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            ExportAll = false;
            SelectedHoleNo = cbHoleNo.SelectedItem.ToString();
            this.DialogResult = true;
            this.Close();
        }

        public bool ExportAll { get; set; }

        public string SelectedHoleNo { get; set; }

        private void cbHoleNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnExport.IsEnabled = (cbHoleNo.SelectedIndex >= 0 ? true : false);

        }
    }
}
