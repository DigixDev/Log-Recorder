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
    /// Interaction logic for UserListWindow.xaml
    /// </summary>
    public partial class UserInfoWindow : Window
    {
        public byte UserRoleId { get; set; }
        public string UserName { get; set; }
        public UserInfoWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BindForm();
            if(UserRoleId>0)
            {
                txtUserName.Text = UserName;
                cbUserRole.SelectedValue = UserRoleId;
            }
            MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private void BindForm()
        {
            cbUserRole.ItemsSource = DA.Class.UserRole.UserRoles;
            cbUserRole.DisplayMemberPath = "Value";
            cbUserRole.SelectedValuePath = "Key";
            cbUserRole.SelectedIndex = 0;
            txtUserName.Focus();
        }

        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnSave.IsEnabled = (txtUserName.Text.Length == 0 ? false : true);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            UserName = txtUserName.Text;
            UserRoleId = Convert.ToByte( cbUserRole.SelectedValue);
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
