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
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void txtCurrentPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ValidateForm();

        }

        private void txtNewPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ValidateForm();

        }

        private void txtConfirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ValidateForm();
        }

        private void ValidateForm()
        {
            bool enable;

            if (txtConfirmPassword.Password.Length == 0 || txtNewPassword.Password.Length == 0 || txtCurrentPassword.Password.Length == 0)
                enable = false;
            else if (txtNewPassword.Password != txtConfirmPassword.Password)
                enable = false;
            else
                enable = true;
            btnUpdate.IsEnabled = enable;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string result;
            if(DA.Class.UserRepository.ChangePassword(txtCurrentPassword.Password, txtNewPassword.Password, out result)==false)
            {

            }
            else
            {
                MessageBox.Show("Password changed.");
                this.Close();
            }
        }
    }
}
