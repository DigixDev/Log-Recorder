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
    /// Interaction logic for NewSheet.xaml
    /// </summary>
    public partial class NewSheet : Window
    {
        private ControlButtons _controlButtons;
        public int SheetType { get; set; }
        public string SheetName { get; set; }

        public NewSheet()
        {
            InitializeComponent();
            SheetType = DA.Class.SheetType.WS;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BindForm();
            MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private void BindForm()
        {
            try
            {
                cbSheetType.ItemsSource = DA.Class.SheetType.SheetFullHeader;
                cbSheetType.DisplayMemberPath = "Value";
                cbSheetType.SelectedValuePath = "Key";
                cbSheetType.SelectedValue = SheetType;

                _controlButtons = new ControlButtons();
                _controlButtons.Add(txtSheetName as TextBox);
                _controlButtons.ApplyButton(btnOk);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            SheetType = (int)cbSheetType.SelectedValue;
            SheetName = txtSheetName.Text;
            this.DialogResult = true;
        }

    }
}
