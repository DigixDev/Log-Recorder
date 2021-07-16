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
    /// Interaction logic for StratumWindow.xaml
    /// </summary>
    
    public partial class StratumWindow : Window
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public StratumWindow()
        {
            InitializeComponent();
            Code = -1;
        }

        private void txtCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateForm();
        }

        private void txtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateForm();
        }

        private void ValidateForm()
        {
            btnSave.IsEnabled = (txtCode.Text.Length == 0 || txtDescription.Text.Length == 0) ? false : true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            if(Code!=-1)
            {
                txtCode.NumberValue = Code;
                txtDescription.Text = Description;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Code = txtCode.NumberValue;
            Description = txtDescription.Text;
            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
