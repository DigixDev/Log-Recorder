using Log_Recorder.Classes;
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
    /// Interaction logic for RenameSheet.xaml
    /// </summary>
    public partial class RenameSheet : Window
    {
        private ControlButtons _controlButtons;
        public RenameSheet()
        {
            InitializeComponent();
            _controlButtons = new ControlButtons();
        }

        public string SheetName { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtSheetName.Text = SheetName;
            txtSheetName.Focus();
            txtSheetName.SelectAll();

            _controlButtons.Add(txtSheetName);
            _controlButtons.ApplyButton(btnRename);
        }

        private void btnRename_Click(object sender, RoutedEventArgs e)
        {
            SheetName = txtSheetName.Text;
            this.DialogResult = true;
            this.Close();
        }
    }
}
